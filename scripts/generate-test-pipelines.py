"""
Generate a standalone test pipeline (azure-pipelines-test-<module>.yml) at
the repo root for every module.  Manual trigger only — safe for testing without
affecting the main CI.
"""

import pathlib, sys, importlib.util

# Import MODULES registry from generate-module-pipelines.py (single source of truth)
_spec = importlib.util.spec_from_file_location(
    "gen_module",
    pathlib.Path(__file__).parent / "generate-module-pipelines.py"
)
_mod = importlib.util.module_from_spec(_spec)
_spec.loader.exec_module(_mod)
MODULES = _mod.MODULES

REPO = pathlib.Path(r"E:\ERPMicroservice")


def make_test_pipeline(module_name: str, services: list) -> str:
    svc_names = [s[0] for s in services]
    param_values = "\n".join(f"      - {n}" for n in svc_names)

    def build_and_push_lines():
        parts = []
        for name, ctx, df in services:
            parts.append(
                f"              build_and_push '{name}' "
                f"'{ctx}' "
                f"'{ctx}/{df}'"
            )
        return "\n".join(parts)

    def verify_lines():
        return "\n".join(
            f"              verify_svc '{name}'" for name, _, _ in services
        )

    return f"""\
# =============================================================================
# TEST PIPELINE — {module_name}  ({len(services)} services)
#
# Purpose : Validate Build+Push (GHCR) → Verify for one module.
# Trigger : MANUAL ONLY — will never run automatically.
#
# Required pipeline variables (Azure DevOps → Pipeline → Edit → Variables):
#   GITHUB_TOKEN  – GitHub PAT with write:packages scope  (mark secret)
# =============================================================================

name: TEST · {module_name} · $(Build.BuildId)

trigger: none
pr: none

parameters:
  - name: service
    displayName: 'Service to build (or "all")'
    type: string
    default: all
    values:
      - all
{param_values}

variables:
  imagePrefix: 'ghcr.io/vimalshan/erp'
  GITHUB_ACTOR: 'vimalshan'

pool:
  vmImage: ubuntu-latest

stages:

  # ── BUILD + PUSH (single script — login, build, push per service) ───────────
  - stage: BuildAndPush
    displayName: 'Build & Push · {module_name}'
    jobs:
      - job: BuildPushImages
        displayName: Build & Push Docker images
        steps:
          - checkout: self
            fetchDepth: 1

          - bash: |
              set -euo pipefail

              # ── Validate token ──
              if [ -z "${{GITHUB_TOKEN:-}}" ]; then
                echo "##[error]GITHUB_TOKEN is not set. Add it as a secret pipeline variable."
                exit 1
              fi
              TOKEN_LEN=${{#GITHUB_TOKEN}}
              echo "Token length: $TOKEN_LEN chars"
              if [ "$TOKEN_LEN" -lt 36 ]; then
                echo "##[error]GITHUB_TOKEN looks invalid ($TOKEN_LEN chars). A GitHub PAT is 40+ chars."
                exit 1
              fi

              # ── Login ──
              echo "Logging in as: $(GITHUB_ACTOR)"
              echo "$GITHUB_TOKEN" | docker login ghcr.io -u "$(GITHUB_ACTOR)" --password-stdin
              echo "GHCR login succeeded."

              # ── Build + Push helper ──
              build_and_push() {{
                local NAME=$1 CTX=$2 DF=$3
                if [ "${{{{{{ parameters.service }}}}}}" = "all" ] || [ "${{{{{{ parameters.service }}}}}}" = "$NAME" ]; then
                  echo ""
                  echo "======= Building $NAME ======="
                  docker build \\
                    -t "$(imagePrefix)/$NAME:$(Build.BuildId)" \\
                    -t "$(imagePrefix)/$NAME:latest" \\
                    -f "$DF" "$CTX"

                  echo "======= Pushing $NAME ======="
                  docker push "$(imagePrefix)/$NAME:$(Build.BuildId)"
                  docker push "$(imagePrefix)/$NAME:latest"
                  echo "OK: $NAME built and pushed."
                fi
              }}

{build_and_push_lines()}

              echo ""
              echo "All done."
            displayName: 'Login, Build & Push all selected services'
            env:
              GITHUB_TOKEN: $(GITHUB_TOKEN)

  # ── VERIFY (separate agent — pulls from registry) ──────────────────────────
  - stage: Verify
    displayName: 'Verify · {module_name}'
    dependsOn: BuildAndPush
    condition: succeeded()
    jobs:
      - job: VerifyImages
        displayName: Verify images exist in GHCR
        steps:
          - checkout: none

          - bash: |
              set -euo pipefail
              if [ -z "${{GITHUB_TOKEN:-}}" ]; then
                echo "##[error]GITHUB_TOKEN is not set."
                exit 1
              fi
              echo "$GITHUB_TOKEN" | docker login ghcr.io -u "$(GITHUB_ACTOR)" --password-stdin
            displayName: Login to GHCR
            env:
              GITHUB_TOKEN: $(GITHUB_TOKEN)

          - script: |
              set -e
              verify_svc() {{
                local NAME=$1
                if [ "${{{{ parameters.service }}}}" = "all" ] || [ "${{{{ parameters.service }}}}" = "$NAME" ]; then
                  IMAGE="$(imagePrefix)/$NAME:$(Build.BuildId)"
                  echo "======= Verifying $IMAGE ======="
                  docker pull "$IMAGE"
                  echo "OK: $IMAGE"
                fi
              }}
{verify_lines()}
            displayName: 'Pull & verify all selected images'
"""


# ── Generate ───────────────────────────────────────────────────────────────────
count = 0
for module_name, cfg in MODULES.items():
    # Skip adminServices — already created manually
    out = REPO / f"azure-pipelines-test-{module_name}.yml"
    out.write_text(
        make_test_pipeline(module_name, cfg["services"]),
        encoding="utf-8"
    )
    print(f"  [{len(cfg['services']):2d} svcs]  {out.name}")
    count += 1

print(f"\nDone. Generated {count} test pipeline files.")
