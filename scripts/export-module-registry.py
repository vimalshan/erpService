import argparse
import ast
import json
from pathlib import Path


REPO_ROOT = Path(__file__).resolve().parents[1]
SOURCE_FILE = REPO_ROOT / "scripts" / "generate-module-pipelines.py"


def load_modules() -> dict:
    tree = ast.parse(SOURCE_FILE.read_text(encoding="utf-8"), filename=str(SOURCE_FILE))
    for node in tree.body:
        if isinstance(node, ast.Assign):
            for target in node.targets:
                if isinstance(target, ast.Name) and target.id == "MODULES":
                    return ast.literal_eval(node.value)
    raise RuntimeError("MODULES registry not found in generate-module-pipelines.py")


def module_slug(name: str) -> str:
    return "".join(ch.lower() for ch in name if ch.isalnum())


def build_service_entries(modules: dict, selected_module: str) -> list[dict]:
    entries: list[dict] = []
    for module_name, config in modules.items():
        if selected_module != "all" and selected_module != module_name:
            continue

        slug = module_slug(module_name)
        for service_name, context, dockerfile in config["services"]:
            entries.append(
                {
                    "module": module_name,
                    "module_slug": slug,
                    "service": service_name,
                    "context": context,
                    "dockerfile": f"{context}/{dockerfile}",
                    "image": f"{slug}-{service_name}",
                }
            )
    return entries


def build_deploy_entries(selected_module: str) -> list[dict]:
    supported = [
        ("adminServices", "src/Services/adminServices"),
        ("aimsServices", "src/Services/aimsServices"),
        ("auditServices", "src/Services/auditServices"),
        ("AuthProvider", "src/Services/AuthProvider"),
        ("canteenServices", "src/Services/canteenServices"),
        ("cashServices", "src/Services/cashServices"),
    ]

    entries: list[dict] = []
    for module_name, path in supported:
        if selected_module != "all" and selected_module != module_name:
            continue
        entries.append(
            {
                "module": module_name,
                "module_slug": module_slug(module_name),
                "path": path,
            }
        )
    return entries


def main() -> None:
    parser = argparse.ArgumentParser()
    parser.add_argument("mode", choices=["services", "deploy"])
    parser.add_argument("--module", default="all")
    args = parser.parse_args()

    if args.mode == "services":
        entries = build_service_entries(load_modules(), args.module)
    else:
        entries = build_deploy_entries(args.module)

    print(json.dumps({"include": entries}, separators=(",", ":")))


if __name__ == "__main__":
    main()