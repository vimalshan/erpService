#!/usr/bin/env python3
"""Split each module's Postman collection into per-service JSON files inside module folders."""

import json
import os
import shutil

POSTMAN_DIR = os.path.join(os.path.dirname(__file__), '..', 'docs', 'postman')
POSTMAN_DIR = os.path.normpath(POSTMAN_DIR)

SCHEMA = "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"

def split_collection(collection_file):
    """Split a single module collection into per-service files in a module folder."""
    with open(collection_file, 'r', encoding='utf-8') as f:
        data = json.load(f)

    module_name = data['info']['name'].replace('ERP - ', '')
    base_vars = data.get('variable', [])
    items = data.get('item', [])

    if not items:
        return 0

    # Create module folder
    module_dir = os.path.join(POSTMAN_DIR, module_name)
    os.makedirs(module_dir, exist_ok=True)

    count = 0
    for service_item in items:
        service_name = service_item['name']
        sub_items = service_item.get('item', [])
        if not sub_items:
            continue

        # Build individual service collection
        service_collection = {
            "info": {
                "name": f"{module_name} - {service_name}",
                "description": f"API collection for {service_name} in {module_name}",
                "schema": SCHEMA,
                "_postman_id": f"erp-{module_name.lower()}-{service_name.lower()}"
            },
            "variable": base_vars,
            "item": sub_items
        }

        safe_name = service_name.replace(' ', '_').replace('/', '_')
        out_path = os.path.join(module_dir, f"{safe_name}-collection.json")
        with open(out_path, 'w', encoding='utf-8') as f:
            json.dump(service_collection, f, indent=2, ensure_ascii=False)
        count += 1

    return count


def main():
    # Find all module collection files (not the combined one)
    collection_files = sorted([
        os.path.join(POSTMAN_DIR, f)
        for f in os.listdir(POSTMAN_DIR)
        if f.endswith('-collection.json') and f != 'ERP-Complete-Collection.json'
        and os.path.isfile(os.path.join(POSTMAN_DIR, f))
    ])

    print(f"Found {len(collection_files)} module collections to split\n")

    total_services = 0
    total_modules = 0

    for cfile in collection_files:
        basename = os.path.basename(cfile)
        count = split_collection(cfile)
        if count > 0:
            total_modules += 1
            total_services += count
            print(f"  {basename} -> {count} service files")
        else:
            print(f"  {basename} -> (empty, skipped)")

    print(f"\n{'='*50}")
    print(f"Split complete!")
    print(f"  Modules processed: {total_modules}")
    print(f"  Service files created: {total_services}")
    print(f"  Output: {POSTMAN_DIR}/<module>/<service>-collection.json")


if __name__ == '__main__':
    main()
