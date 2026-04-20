#!/usr/bin/env python3
"""
ERP Microservice API Documentation Generator
Scans the entire codebase and generates:
1. Complete Markdown documentation with folder structure, controllers, GraphQL, minimal APIs
2. Postman collection JSON per module
3. GraphQL request samples
"""

import os
import re
import json
import glob
from pathlib import Path
from collections import defaultdict
from datetime import datetime

BASE_DIR = Path(r"E:\ERPMicroservice\src\Services")
OUTPUT_DIR = Path(r"E:\ERPMicroservice\docs")

# ─── Helpers ────────────────────────────────────────────────────────────────

def read_file(path):
    """Read file content safely."""
    try:
        with open(path, "r", encoding="utf-8-sig", errors="replace") as f:
            return f.read()
    except Exception:
        return ""


def get_folder_tree(root, prefix="", max_depth=4, current_depth=0):
    """Generate a folder tree string."""
    if current_depth >= max_depth:
        return ""
    lines = []
    try:
        entries = sorted(os.listdir(root))
    except PermissionError:
        return ""
    dirs = [e for e in entries if os.path.isdir(os.path.join(root, e)) and not e.startswith('.')]
    files_of_interest = [e for e in entries if e.endswith(('.cs', '.csproj', '.json', '.yaml', '.yml', '.sql'))
                         and not e.startswith('.')]
    for f in files_of_interest:
        lines.append(f"{prefix}{f}")
    for i, d in enumerate(dirs):
        if d in ('bin', 'obj', 'node_modules', '.git', '.vs', 'TestResults'):
            continue
        connector = "├── " if i < len(dirs) - 1 else "└── "
        lines.append(f"{prefix}{connector}{d}/")
        extension = "│   " if i < len(dirs) - 1 else "    "
        subtree = get_folder_tree(os.path.join(root, d), prefix + extension, max_depth, current_depth + 1)
        if subtree:
            lines.append(subtree)
    return "\n".join(lines)


# ─── Controller Extraction ──────────────────────────────────────────────────

def extract_controllers(service_path):
    """Extract all controllers and their endpoints from a service."""
    controllers = []
    controller_files = glob.glob(str(service_path / "**" / "Controllers" / "*.cs"), recursive=True)
    controller_files += glob.glob(str(service_path / "**" / "Auth" / "*Controller.cs"), recursive=True)

    for cf in controller_files:
        content = read_file(cf)
        if "[ApiController]" not in content and "[controller]" not in content.lower():
            continue

        fname = os.path.basename(cf)
        if fname == "BaseApiController.cs":
            continue

        ctrl_info = {"file": fname, "path": cf, "endpoints": [], "route_prefix": "", "auth": False}

        # Extract route prefix
        route_match = re.search(r'\[Route\("([^"]+)"\)\]', content)
        if route_match:
            ctrl_info["route_prefix"] = route_match.group(1)

        # Check auth
        if "[Authorize]" in content:
            ctrl_info["auth"] = True

        # Extract endpoints
        # Pattern: [HttpGet], [HttpPost], [HttpPut], [HttpDelete], [HttpPatch]
        endpoint_pattern = re.compile(
            r'\[(Http(Get|Post|Put|Delete|Patch))(?:\("([^"]*)"\))?\].*?'
            r'(?:\[(?:AllowAnonymous|Authorize|ProducesResponseType)[^\]]*\]\s*)*'
            r'public\s+(?:async\s+)?(?:Task<)?(?:ActionResult<([^>]+)>|IActionResult|ActionResult)(?:>)?\s+(\w+)',
            re.DOTALL
        )

        for m in endpoint_pattern.finditer(content):
            method = m.group(2).upper()
            route_suffix = m.group(3) or ""
            return_type = m.group(4) or "IActionResult"
            method_name = m.group(5)

            # Build full route
            base = ctrl_info["route_prefix"]
            if route_suffix:
                full_route = f"/{base}/{route_suffix}".replace("//", "/")
            else:
                full_route = f"/{base}"
            full_route = full_route.replace("[controller]",
                                           fname.replace("Controller.cs", "").replace("Controllers.cs", "").lower())

            # Extract parameters
            params_match = re.search(rf'{method_name}\s*\(([^)]*)\)', content)
            params = []
            if params_match:
                param_str = params_match.group(1)
                # Extract [FromBody], [FromQuery], [FromRoute] params
                for p in re.finditer(r'\[From(\w+)\]\s*(\w+(?:<[^>]+>)?)\s+(\w+)', param_str):
                    params.append({"source": p.group(1), "type": p.group(2), "name": p.group(3)})
                # Also get simple typed params (skip already-found FromX params)
                for p in re.finditer(r'(?:int|long|string|Guid|bool)\s+(\w+)', param_str):
                    if not any(pp["name"] == p.group(1) for pp in params):
                        params.append({"source": "Route", "type": "auto", "name": p.group(1)})

            ctrl_info["endpoints"].append({
                "method": method,
                "route": full_route,
                "method_name": method_name,
                "return_type": return_type,
                "params": params,
                "allow_anonymous": "[AllowAnonymous]" in content[:content.find(method_name)] if method_name in content else False
            })

        if ctrl_info["endpoints"] or "[ApiController]" in content:
            controllers.append(ctrl_info)

    return controllers


# ─── GraphQL Extraction ─────────────────────────────────────────────────────

def extract_graphql(service_path):
    """Extract GraphQL queries and mutations from a service."""
    graphql_info = {"has_graphql": False, "queries": [], "mutations": [], "query_type": "", "mutation_type": ""}

    # Find Program.cs or GraphQL config files
    program_files = glob.glob(str(service_path / "**" / "Program.cs"), recursive=True)
    config_files = glob.glob(str(service_path / "**" / "*GraphQL*.cs"), recursive=True)
    config_files += glob.glob(str(service_path / "**" / "*DependencyInjection*.cs"), recursive=True)
    config_files += glob.glob(str(service_path / "**" / "*ServiceExtensions*.cs"), recursive=True)
    config_files += glob.glob(str(service_path / "**" / "*ServiceCollectionExtensions*.cs"), recursive=True)

    all_config = program_files + config_files

    for pf in all_config:
        content = read_file(pf)
        if ".AddGraphQLServer()" not in content and "AddQueryType" not in content:
            continue

        graphql_info["has_graphql"] = True

        # Extract query/mutation types
        qt_match = re.search(r'\.AddQueryType<(\w+)>', content)
        mt_match = re.search(r'\.AddMutationType<(\w+)>', content)
        if qt_match:
            graphql_info["query_type"] = qt_match.group(1)
        if mt_match:
            graphql_info["mutation_type"] = mt_match.group(1)
        break

    if not graphql_info["has_graphql"]:
        return graphql_info

    # Find query files
    query_files = glob.glob(str(service_path / "**" / "GraphQL" / "**" / "*Query*.cs"), recursive=True)
    query_files += glob.glob(str(service_path / "**" / "GraphQL" / "*Query*.cs"), recursive=True)
    query_files += glob.glob(str(service_path / "**" / "Queries" / "*.cs"), recursive=True)

    for qf in set(query_files):
        content = read_file(qf)
        # Extract query methods
        for m in re.finditer(
            r'public\s+(?:async\s+)?(?:Task<)?(?:IEnumerable<|IReadOnlyList<|List<|IQueryable<)?(\w+(?:<[^>]+>)?)\>?\>?\s+(\w+)\s*\(',
            content
        ):
            return_type = m.group(1)
            method_name = m.group(2)
            if method_name in ('ToString', 'Equals', 'GetHashCode', 'GetType') or method_name.startswith('_'):
                continue
            # Check if it has UseFiltering/UseSorting
            has_filtering = "[UseFiltering]" in content
            has_sorting = "[UseSorting]" in content

            graphql_info["queries"].append({
                "name": method_name,
                "return_type": return_type,
                "has_filtering": has_filtering,
                "has_sorting": has_sorting
            })

    # Find mutation files
    mutation_files = glob.glob(str(service_path / "**" / "GraphQL" / "**" / "*Mutation*.cs"), recursive=True)
    mutation_files += glob.glob(str(service_path / "**" / "GraphQL" / "*Mutation*.cs"), recursive=True)
    mutation_files += glob.glob(str(service_path / "**" / "Mutations" / "*.cs"), recursive=True)

    for mf in set(mutation_files):
        content = read_file(mf)
        for m in re.finditer(
            r'public\s+(?:async\s+)?(?:Task<)?(\w+(?:<[^>]+>)?)\>?\s+(\w+)\s*\(([^)]*)\)',
            content
        ):
            return_type = m.group(1)
            method_name = m.group(2)
            params_str = m.group(3)
            if method_name in ('ToString', 'Equals', 'GetHashCode', 'GetType') or method_name.startswith('_'):
                continue

            # Extract input parameters (skip IMediator, CancellationToken)
            input_params = []
            for p in re.finditer(r'(\w+(?:<[^>]+>)?)\s+(\w+)', params_str):
                ptype = p.group(1)
                pname = p.group(2)
                if ptype in ('IMediator', 'CancellationToken', 'Service'):
                    continue
                if pname == 'mediator' or pname == 'cancellationToken' or pname == 'ct':
                    continue
                input_params.append({"type": ptype, "name": pname})

            graphql_info["mutations"].append({
                "name": method_name,
                "return_type": return_type,
                "input_params": input_params
            })

    return graphql_info


# ─── Minimal API Extraction ─────────────────────────────────────────────────

def extract_minimal_apis(service_path):
    """Extract minimal API endpoints from a service."""
    endpoints = []

    # Find MinimalApis folders and Program.cs
    minimal_files = glob.glob(str(service_path / "**" / "MinimalApis" / "*.cs"), recursive=True)
    minimal_files += glob.glob(str(service_path / "**" / "Endpoints" / "*.cs"), recursive=True)

    for mf in minimal_files:
        content = read_file(mf)
        fname = os.path.basename(mf)

        # Extract MapGroup prefix
        group_match = re.search(r'MapGroup\("([^"]+)"\)', content)
        group_prefix = group_match.group(1) if group_match else ""

        # Extract Map* endpoints
        for m in re.finditer(r'\.(Map(Get|Post|Put|Delete|Patch))\("([^"]*)"', content):
            method = m.group(2).upper()
            route = m.group(3)
            full_route = f"{group_prefix}/{route}".replace("//", "/").rstrip("/")
            if not full_route.startswith("/"):
                full_route = "/" + full_route

            endpoints.append({
                "method": method,
                "route": full_route,
                "file": fname,
                "has_auth": ".RequireAuthorization()" in content
            })

    # Also check Program.cs for inline minimal APIs
    program_files = glob.glob(str(service_path / "**" / "Program.cs"), recursive=True)
    for pf in program_files:
        content = read_file(pf)
        for m in re.finditer(r'app\.(Map(Get|Post|Put|Delete|Patch))\("([^"]*)"', content):
            method = m.group(2).upper()
            route = m.group(3)
            if route == "/health" or route == "/graphql":
                continue
            endpoints.append({
                "method": method,
                "route": route,
                "file": os.path.basename(pf),
                "has_auth": False
            })

    return endpoints


# ─── Port Extraction ────────────────────────────────────────────────────────

def extract_port(service_path):
    """Try to extract port from launchSettings.json or docker-compose."""
    launch_files = glob.glob(str(service_path / "**" / "launchSettings.json"), recursive=True)
    for lf in launch_files:
        content = read_file(lf)
        try:
            data = json.loads(content)
            for profile in data.get("profiles", {}).values():
                url = profile.get("applicationUrl", "")
                port_match = re.search(r':(\d{4,5})', url)
                if port_match:
                    return port_match.group(1)
        except (json.JSONDecodeError, AttributeError):
            pass

    # Check Program.cs for UseUrls
    program_files = glob.glob(str(service_path / "**" / "Program.cs"), recursive=True)
    for pf in program_files:
        content = read_file(pf)
        port_match = re.search(r'UseUrls\("https?://[^:]+:(\d+)"', content)
        if port_match:
            return port_match.group(1)

    return "5000"


# ─── Postman Collection Generator ──────────────────────────────────────────

def generate_postman_item(endpoint, base_url, service_name):
    """Generate a single Postman request item."""
    method = endpoint["method"]
    route = endpoint["route"]
    url = f"{base_url}{route}"

    # Build URL parts
    url_parts = route.strip("/").split("/")
    path_parts = []
    variables = []
    for part in url_parts:
        if part.startswith("{") and part.endswith("}"):
            var_name = part.strip("{}")
            # Remove route constraints like :int, :long
            var_name = var_name.split(":")[0]
            path_parts.append(f":{var_name}")
            variables.append({"key": var_name, "value": "1", "description": f"{var_name} parameter"})
        else:
            path_parts.append(part)

    item = {
        "name": f"{method} {route}",
        "request": {
            "method": method,
            "header": [
                {"key": "Content-Type", "value": "application/json", "type": "text"},
                {"key": "Authorization", "value": "Bearer {{authToken}}", "type": "text"}
            ],
            "url": {
                "raw": f"{{{{baseUrl}}}}/{'/'.join(path_parts)}",
                "host": ["{{baseUrl}}"],
                "path": path_parts,
                "variable": variables
            }
        },
        "response": []
    }

    if method in ("POST", "PUT", "PATCH"):
        item["request"]["body"] = {
            "mode": "raw",
            "raw": json.dumps({"field1": "value1", "field2": "value2"}, indent=2),
            "options": {"raw": {"language": "json"}}
        }

    return item


def generate_postman_graphql_item(query_name, query_type, service_name, return_type=""):
    """Generate a Postman request for a GraphQL query or mutation."""
    if query_type == "query":
        graphql_body = f'{{\n  "query": "query {{ {query_name} {{ id }} }}",\n  "variables": {{}}\n}}'
    else:
        graphql_body = f'{{\n  "query": "mutation {{ {query_name}(input: {{}}) {{ id }} }}",\n  "variables": {{\n    "input": {{}}\n  }}\n}}'

    return {
        "name": f"GraphQL {query_type.title()}: {query_name}",
        "request": {
            "method": "POST",
            "header": [
                {"key": "Content-Type", "value": "application/json", "type": "text"},
                {"key": "Authorization", "value": "Bearer {{authToken}}", "type": "text"}
            ],
            "body": {
                "mode": "raw",
                "raw": graphql_body,
                "options": {"raw": {"language": "json"}}
            },
            "url": {
                "raw": "{{baseUrl}}/graphql",
                "host": ["{{baseUrl}}"],
                "path": ["graphql"]
            }
        },
        "response": []
    }


def generate_postman_collection(module_name, services_data):
    """Generate a complete Postman collection for a module."""
    collection = {
        "info": {
            "name": f"ERP - {module_name}",
            "description": f"API collection for {module_name} module",
            "schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json",
            "_postman_id": f"erp-{module_name.lower()}"
        },
        "variable": [
            {"key": "baseUrl", "value": "https://localhost:5001", "type": "string"},
            {"key": "authToken", "value": "", "type": "string"}
        ],
        "item": []
    }

    for svc_name, svc_data in services_data.items():
        svc_folder = {
            "name": svc_name,
            "item": []
        }

        # REST Controllers
        if svc_data.get("controllers"):
            ctrl_folder = {"name": "REST API Controllers", "item": []}
            for ctrl in svc_data["controllers"]:
                ctrl_item = {"name": ctrl["file"].replace(".cs", ""), "item": []}
                for ep in ctrl["endpoints"]:
                    ctrl_item["item"].append(
                        generate_postman_item(ep, "{{baseUrl}}", svc_name)
                    )
                if ctrl_item["item"]:
                    ctrl_folder["item"].append(ctrl_item)
            if ctrl_folder["item"]:
                svc_folder["item"].append(ctrl_folder)

        # GraphQL
        if svc_data.get("graphql", {}).get("has_graphql"):
            gql_folder = {"name": "GraphQL", "item": []}
            for q in svc_data["graphql"].get("queries", []):
                gql_folder["item"].append(
                    generate_postman_graphql_item(q["name"], "query", svc_name, q.get("return_type", ""))
                )
            for m in svc_data["graphql"].get("mutations", []):
                gql_folder["item"].append(
                    generate_postman_graphql_item(m["name"], "mutation", svc_name, m.get("return_type", ""))
                )
            if gql_folder["item"]:
                svc_folder["item"].append(gql_folder)

        # Minimal APIs
        if svc_data.get("minimal_apis"):
            min_folder = {"name": "Minimal APIs", "item": []}
            for ep in svc_data["minimal_apis"]:
                min_folder["item"].append(
                    generate_postman_item(ep, "{{baseUrl}}", svc_name)
                )
            if min_folder["item"]:
                svc_folder["item"].append(min_folder)

        if svc_folder["item"]:
            collection["item"].append(svc_folder)

    return collection


# ─── GraphQL Sample Generator ──────────────────────────────────────────────

def generate_graphql_samples(service_name, graphql_info):
    """Generate GraphQL request samples."""
    samples = []

    for q in graphql_info.get("queries", []):
        sample = {
            "name": q["name"],
            "type": "query",
            "request": {
                "url": "{{baseUrl}}/graphql",
                "method": "POST",
                "headers": {
                    "Content-Type": "application/json",
                    "Authorization": "Bearer {{authToken}}"
                },
                "body": {
                    "query": f'query {{\n  {q["name"]} {{\n    id\n    # Add fields based on return type: {q["return_type"]}\n  }}\n}}',
                    "variables": {}
                }
            }
        }
        if q.get("has_filtering"):
            sample["request"]["body"]["query"] = (
                f'query ($where: {q["return_type"]}FilterInput) {{\n'
                f'  {q["name"]}(where: $where) {{\n'
                f'    id\n'
                f'  }}\n'
                f'}}'
            )
            sample["request"]["body"]["variables"] = {"where": {"id": {"eq": 1}}}
        samples.append(sample)

    for m in graphql_info.get("mutations", []):
        input_obj = {}
        for p in m.get("input_params", []):
            if p["type"] in ("int", "long", "Int32", "Int64"):
                input_obj[p["name"]] = 0
            elif p["type"] == "bool":
                input_obj[p["name"]] = True
            elif p["type"] == "decimal":
                input_obj[p["name"]] = 0.0
            elif p["type"] == "DateTime":
                input_obj[p["name"]] = "2025-01-01T00:00:00Z"
            else:
                input_obj[p["name"]] = "string"

        params_str = ""
        if m.get("input_params"):
            param_names = ", ".join([f'{p["name"]}: ${p["name"]}' for p in m["input_params"]])
            params_str = f"({param_names})"

        sample = {
            "name": m["name"],
            "type": "mutation",
            "request": {
                "url": "{{baseUrl}}/graphql",
                "method": "POST",
                "headers": {
                    "Content-Type": "application/json",
                    "Authorization": "Bearer {{authToken}}"
                },
                "body": {
                    "query": f'mutation {{\n  {m["name"]}{params_str} {{\n    id\n  }}\n}}',
                    "variables": input_obj if input_obj else {}
                }
            }
        }
        samples.append(sample)

    return samples


# ─── Markdown Generator ─────────────────────────────────────────────────────

def generate_module_markdown(module_name, services_data, module_path):
    """Generate markdown documentation for a single module."""
    md = []
    md.append(f"# {module_name}\n")
    md.append(f"> Module path: `src/Services/{module_name}/`\n")

    # Module folder structure (top-level only)
    md.append("## Folder Structure\n")
    md.append("```")
    try:
        subdirs = sorted([d for d in os.listdir(module_path) if os.path.isdir(os.path.join(module_path, d))
                          and d not in ('bin', 'obj', '.git', '.vs')])
        md.append(f"{module_name}/")
        for sd in subdirs:
            md.append(f"├── {sd}/")
    except Exception:
        md.append(f"{module_name}/")
    md.append("```\n")

    for svc_name, svc_data in sorted(services_data.items()):
        md.append(f"---\n")
        md.append(f"## {svc_name}\n")
        port = svc_data.get("port", "5000")
        md.append(f"- **Base URL**: `https://localhost:{port}`")
        md.append(f"- **Health Check**: `GET /health`\n")

        # Service folder structure
        svc_path = svc_data.get("path", "")
        if svc_path and os.path.exists(svc_path):
            md.append("### Folder Structure\n")
            md.append("```")
            tree = get_folder_tree(svc_path, max_depth=3)
            if tree:
                md.append(tree)
            md.append("```\n")

        # REST Controllers
        controllers = svc_data.get("controllers", [])
        if controllers:
            md.append("### REST API Controllers\n")
            for ctrl in controllers:
                ctrl_name = ctrl["file"].replace(".cs", "")
                if ctrl_name in ("AuthController", "WeatherForecastController"):
                    continue
                md.append(f"#### {ctrl_name}\n")
                md.append(f"- **Route Prefix**: `{ctrl['route_prefix']}`")
                md.append(f"- **Authorization**: {'Required' if ctrl['auth'] else 'None'}\n")

                if ctrl["endpoints"]:
                    md.append("| Method | Route | Action | Return Type |")
                    md.append("|--------|-------|--------|-------------|")
                    for ep in ctrl["endpoints"]:
                        md.append(f"| `{ep['method']}` | `{ep['route']}` | {ep['method_name']} | {ep['return_type']} |")
                    md.append("")

        # GraphQL
        graphql = svc_data.get("graphql", {})
        if graphql.get("has_graphql"):
            md.append("### GraphQL API\n")
            md.append(f"- **Endpoint**: `POST /graphql`")
            if graphql.get("query_type"):
                md.append(f"- **Query Type**: `{graphql['query_type']}`")
            if graphql.get("mutation_type"):
                md.append(f"- **Mutation Type**: `{graphql['mutation_type']}`")
            md.append("")

            if graphql.get("queries"):
                md.append("#### Queries\n")
                md.append("| Query Name | Return Type | Filtering | Sorting |")
                md.append("|------------|-------------|-----------|---------|")
                for q in graphql["queries"]:
                    md.append(f"| `{q['name']}` | {q['return_type']} | {'Yes' if q.get('has_filtering') else 'No'} | {'Yes' if q.get('has_sorting') else 'No'} |")
                md.append("")

            if graphql.get("mutations"):
                md.append("#### Mutations\n")
                md.append("| Mutation Name | Return Type | Input Parameters |")
                md.append("|---------------|-------------|------------------|")
                for m in graphql["mutations"]:
                    params = ", ".join([f"{p['name']}: {p['type']}" for p in m.get("input_params", [])])
                    md.append(f"| `{m['name']}` | {m['return_type']} | {params or 'command object'} |")
                md.append("")

            # GraphQL Samples
            samples = generate_graphql_samples(svc_name, graphql)
            if samples:
                md.append("#### GraphQL Request Samples\n")
                for s in samples[:6]:  # Limit to 6 samples per service to keep doc manageable
                    md.append(f"**{s['type'].title()}: {s['name']}**\n")
                    md.append("```json")
                    md.append(f"// POST {{baseUrl}}/graphql")
                    md.append(f"// Headers: Content-Type: application/json")
                    md.append(f"//          Authorization: Bearer {{authToken}}")
                    md.append(json.dumps(s["request"]["body"], indent=2))
                    md.append("```\n")

        # Minimal APIs
        minimal = svc_data.get("minimal_apis", [])
        if minimal:
            md.append("### Minimal APIs\n")
            md.append("| Method | Route | Auth Required |")
            md.append("|--------|-------|---------------|")
            for ep in minimal:
                md.append(f"| `{ep['method']}` | `{ep['route']}` | {'Yes' if ep.get('has_auth') else 'No'} |")
            md.append("")

    return "\n".join(md)


# ─── Main ───────────────────────────────────────────────────────────────────

def main():
    print(f"ERP Microservice API Documentation Generator")
    print(f"{'=' * 50}")
    print(f"Scanning: {BASE_DIR}")
    print(f"Output:   {OUTPUT_DIR}\n")

    os.makedirs(OUTPUT_DIR, exist_ok=True)
    os.makedirs(OUTPUT_DIR / "postman", exist_ok=True)

    # Discover all modules
    modules = sorted([d for d in os.listdir(BASE_DIR)
                      if os.path.isdir(BASE_DIR / d) and not d.startswith('.')])

    print(f"Found {len(modules)} modules: {', '.join(modules)}\n")

    all_modules_data = {}
    master_md = []
    master_md.append("# ERP Microservice - Complete API Documentation\n")
    master_md.append(f"> Generated: {datetime.now().strftime('%Y-%m-%d %H:%M:%S')}\n")
    master_md.append("## Table of Contents\n")

    # Generate TOC
    for mod in modules:
        master_md.append(f"- [{mod}](#{mod.lower()})")
    master_md.append("")

    total_controllers = 0
    total_endpoints = 0
    total_graphql = 0
    total_minimal = 0

    for mod in modules:
        module_path = BASE_DIR / mod
        print(f"Processing: {mod}...")

        # Discover sub-services
        sub_services = sorted([d for d in os.listdir(module_path)
                               if os.path.isdir(module_path / d)
                               and d not in ('bin', 'obj', '.git', '.vs', 'docs', 'SharedServices', 'Shared')])

        services_data = {}

        for svc in sub_services:
            svc_path = module_path / svc
            print(f"  - {svc}...", end=" ")

            svc_data = {"path": str(svc_path)}

            # Extract controllers
            controllers = extract_controllers(svc_path)
            svc_data["controllers"] = controllers
            ctrl_count = len(controllers)
            ep_count = sum(len(c["endpoints"]) for c in controllers)
            total_controllers += ctrl_count
            total_endpoints += ep_count

            # Extract GraphQL
            graphql = extract_graphql(svc_path)
            svc_data["graphql"] = graphql
            if graphql["has_graphql"]:
                total_graphql += 1

            # Extract Minimal APIs
            minimal = extract_minimal_apis(svc_path)
            svc_data["minimal_apis"] = minimal
            total_minimal += len(minimal)

            # Extract port
            port = extract_port(svc_path)
            svc_data["port"] = port

            print(f"Controllers: {ctrl_count}, Endpoints: {ep_count}, GraphQL: {'Yes' if graphql['has_graphql'] else 'No'}, MinimalAPIs: {len(minimal)}")

            services_data[svc] = svc_data

        all_modules_data[mod] = services_data

        # Generate module markdown
        module_md = generate_module_markdown(mod, services_data, str(module_path))
        master_md.append(module_md)

        # Generate module Postman collection
        postman = generate_postman_collection(mod, services_data)
        postman_path = OUTPUT_DIR / "postman" / f"{mod}-collection.json"
        with open(postman_path, "w", encoding="utf-8") as f:
            json.dump(postman, f, indent=2)

    # Write master document
    master_path = OUTPUT_DIR / "ERP-API-Documentation.md"
    with open(master_path, "w", encoding="utf-8") as f:
        f.write("\n".join(master_md))

    # Generate summary
    print(f"\n{'=' * 50}")
    print(f"Documentation Generation Complete!")
    print(f"{'=' * 50}")
    print(f"Modules:          {len(modules)}")
    print(f"Controllers:      {total_controllers}")
    print(f"REST Endpoints:   {total_endpoints}")
    print(f"GraphQL Services: {total_graphql}")
    print(f"Minimal APIs:     {total_minimal}")
    print(f"\nOutput files:")
    print(f"  Documentation: {master_path}")
    print(f"  Postman:       {OUTPUT_DIR / 'postman'}/")

    # Also generate a combined Postman collection
    combined_collection = {
        "info": {
            "name": "ERP Microservice - Complete API Collection",
            "description": "Complete API collection for all ERP modules",
            "schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
        },
        "variable": [
            {"key": "baseUrl", "value": "https://localhost:5001", "type": "string"},
            {"key": "authToken", "value": "", "type": "string"}
        ],
        "item": []
    }

    for mod, services_data in all_modules_data.items():
        mod_collection = generate_postman_collection(mod, services_data)
        if mod_collection["item"]:
            combined_collection["item"].append({
                "name": mod,
                "item": mod_collection["item"]
            })

    combined_path = OUTPUT_DIR / "postman" / "ERP-Complete-Collection.json"
    with open(combined_path, "w", encoding="utf-8") as f:
        json.dump(combined_collection, f, indent=2)
    print(f"  Combined:      {combined_path}")


if __name__ == "__main__":
    main()
