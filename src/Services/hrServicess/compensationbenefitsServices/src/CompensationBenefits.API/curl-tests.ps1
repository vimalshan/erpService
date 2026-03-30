# ══════════════════════════════════════════════════════════════════
#  CompensationBenefits API — cURL Test Script (PowerShell)
#  Run: .\curl-tests.ps1
#  Requires: curl.exe (Windows 10+ built-in) or Git Bash curl
# ══════════════════════════════════════════════════════════════════

$HOST_URL = "http://localhost:5009"
$TOKEN    = "YOUR_JWT_TOKEN_HERE"          # <-- replace with a valid JWT
$GRAPHQL  = "$HOST_URL/graphql"

function Sep($label) {
    Write-Host "`n$('═' * 60)" -ForegroundColor Cyan
    Write-Host "  $label" -ForegroundColor Yellow
    Write-Host "$('═' * 60)" -ForegroundColor Cyan
}

function Run($label, $args_) {
    Write-Host "`n▶  $label" -ForegroundColor Green
    curl.exe @args_
    Write-Host ""
}

# ══════════════════════════════════════════════════════════════════
#  HEALTH CHECK
# ══════════════════════════════════════════════════════════════════
Sep "HEALTH CHECK"

Run "GET /health" @(
    "-s", "-o", "-", "-w", "\nHTTP %{http_code}\n",
    "$HOST_URL/health"
)

# ══════════════════════════════════════════════════════════════════
#  SALARY STRUCTURES  —  /api/salarystructures
# ══════════════════════════════════════════════════════════════════
Sep "SALARY STRUCTURES — REST Controller"

Run "GET all salary structures" @(
    "-s", "-o", "-", "-w", "\nHTTP %{http_code}\n",
    "-H", "Authorization: Bearer $TOKEN",
    "-H", "Accept: application/json",
    "$HOST_URL/api/salarystructures"
)

Run "GET salary structure by ID (1001)" @(
    "-s", "-o", "-", "-w", "\nHTTP %{http_code}\n",
    "-H", "Authorization: Bearer $TOKEN",
    "-H", "Accept: application/json",
    "$HOST_URL/api/salarystructures/1001"
)

Run "POST create salary structure" @(
    "-s", "-o", "-", "-w", "\nHTTP %{http_code}\n",
    "-X", "POST",
    "-H", "Authorization: Bearer $TOKEN",
    "-H", "Content-Type: application/json",
    "-d", '{"structureId":1001,"unitId":10,"name":"Senior Engineer Band","gradeCategory":"Technical","gradeId":5,"type":"C","ctcMin":800000.00,"ctcMax":1500000.00,"footerId":1,"createdBy":1}',
    "$HOST_URL/api/salarystructures"
)

Run "PUT update salary structure (1001)" @(
    "-s", "-o", "-", "-w", "\nHTTP %{http_code}\n",
    "-X", "PUT",
    "-H", "Authorization: Bearer $TOKEN",
    "-H", "Content-Type: application/json",
    "-d", '{"structureId":1001,"name":"Senior Engineer Band - Updated","ctcMin":900000.00,"ctcMax":1600000.00,"modifiedBy":1}',
    "$HOST_URL/api/salarystructures/1001"
)

# ══════════════════════════════════════════════════════════════════
#  SALARIES  —  /api/salaries
# ══════════════════════════════════════════════════════════════════
Sep "SALARIES — REST Controller"

Run "GET all salaries" @(
    "-s", "-o", "-", "-w", "\nHTTP %{http_code}\n",
    "-H", "Authorization: Bearer $TOKEN",
    "-H", "Accept: application/json",
    "$HOST_URL/api/salaries"
)

Run "GET salary by ID (2001)" @(
    "-s", "-o", "-", "-w", "\nHTTP %{http_code}\n",
    "-H", "Authorization: Bearer $TOKEN",
    "-H", "Accept: application/json",
    "$HOST_URL/api/salaries/2001"
)

Run "POST create salary" @(
    "-s", "-o", "-", "-w", "\nHTTP %{http_code}\n",
    "-X", "POST",
    "-H", "Authorization: Bearer $TOKEN",
    "-H", "Content-Type: application/json",
    "-d", '{"salaryId":2001,"salaryType":"C","salaryCTC":1200000.00,"salaryStructureId":1001,"salaryFooterId":1,"createdBy":1}',
    "$HOST_URL/api/salaries"
)

Run "DELETE cancel salary (2001)" @(
    "-s", "-o", "-", "-w", "\nHTTP %{http_code}\n",
    "-X", "DELETE",
    "-H", "Authorization: Bearer $TOKEN",
    "$HOST_URL/api/salaries/2001/cancel?cancelledBy=1"
)

# ══════════════════════════════════════════════════════════════════
#  MEDICLAIM  —  /api/mediclaim
# ══════════════════════════════════════════════════════════════════
Sep "MEDICLAIM — REST Controller"

Run "GET all mediclaims" @(
    "-s", "-o", "-", "-w", "\nHTTP %{http_code}\n",
    "-H", "Authorization: Bearer $TOKEN",
    "-H", "Accept: application/json",
    "$HOST_URL/api/mediclaim"
)

Run "GET mediclaim by ID (3001)" @(
    "-s", "-o", "-", "-w", "\nHTTP %{http_code}\n",
    "-H", "Authorization: Bearer $TOKEN",
    "-H", "Accept: application/json",
    "$HOST_URL/api/mediclaim/3001"
)

Run "POST create mediclaim" @(
    "-s", "-o", "-", "-w", "\nHTTP %{http_code}\n",
    "-X", "POST",
    "-H", "Authorization: Bearer $TOKEN",
    "-H", "Content-Type: application/json",
    "-d", '{"mediclaimId":3001,"refName":"Star Health Gold Plan 2026","type":"F","paidBy":"C","startDate":"2026-04-01T00:00:00Z","closeDate":"2027-03-31T00:00:00Z"}',
    "$HOST_URL/api/mediclaim"
)

# ══════════════════════════════════════════════════════════════════
#  MOBILE CONNECTIONS  —  /api/mobileconnections
# ══════════════════════════════════════════════════════════════════
Sep "MOBILE CONNECTIONS — REST Controller"

Run "GET mobile connections by employee (501)" @(
    "-s", "-o", "-", "-w", "\nHTTP %{http_code}\n",
    "-H", "Authorization: Bearer $TOKEN",
    "-H", "Accept: application/json",
    "$HOST_URL/api/mobileconnections/employee/501"
)

Run "POST create mobile connection" @(
    "-s", "-o", "-", "-w", "\nHTTP %{http_code}\n",
    "-X", "POST",
    "-H", "Authorization: Bearer $TOKEN",
    "-H", "Content-Type: application/json",
    "-d", '{"connId":4001,"empSysId":501,"type":"C","phoneNo":9876543210,"calendarId":1,"createdBy":1,"effDate":"2026-04-01T00:00:00Z"}',
    "$HOST_URL/api/mobileconnections"
)

# ══════════════════════════════════════════════════════════════════
#  MINIMAL API v2  —  /api/v2/salaries  &  /api/v2/salary-structures
# ══════════════════════════════════════════════════════════════════
Sep "MINIMAL API v2 — Salaries"

Run "GET all salaries (Minimal API)" @(
    "-s", "-o", "-", "-w", "\nHTTP %{http_code}\n",
    "-H", "Authorization: Bearer $TOKEN",
    "-H", "Accept: application/json",
    "$HOST_URL/api/v2/salaries"
)

Run "GET salary by ID (Minimal API)" @(
    "-s", "-o", "-", "-w", "\nHTTP %{http_code}\n",
    "-H", "Authorization: Bearer $TOKEN",
    "-H", "Accept: application/json",
    "$HOST_URL/api/v2/salaries/2001"
)

Sep "MINIMAL API v2 — Salary Structures"

Run "GET all salary structures (Minimal API)" @(
    "-s", "-o", "-", "-w", "\nHTTP %{http_code}\n",
    "-H", "Authorization: Bearer $TOKEN",
    "-H", "Accept: application/json",
    "$HOST_URL/api/v2/salary-structures"
)

Run "GET salary structure by ID (Minimal API)" @(
    "-s", "-o", "-", "-w", "\nHTTP %{http_code}\n",
    "-H", "Authorization: Bearer $TOKEN",
    "-H", "Accept: application/json",
    "$HOST_URL/api/v2/salary-structures/1001"
)

# ══════════════════════════════════════════════════════════════════
#  GRAPHQL — Queries
# ══════════════════════════════════════════════════════════════════
Sep "GRAPHQL — Queries"

Run "GraphQL: get all salaries" @(
    "-s", "-o", "-", "-w", "\nHTTP %{http_code}\n",
    "-X", "POST",
    "-H", "Content-Type: application/json",
    "-d", '{"query":"{ salaries { salaryId salaryType salaryCTC salaryStructureId salaryFooterId salaryCancelledOn } }"}',
    $GRAPHQL
)

Run "GraphQL: get salary by ID (2001)" @(
    "-s", "-o", "-", "-w", "\nHTTP %{http_code}\n",
    "-X", "POST",
    "-H", "Content-Type: application/json",
    "-d", '{"query":"query GetSalary($id: Long!) { salary(id: $id) { salaryId salaryType salaryCTC salaryStructureId salaryFooterId salaryCancelledOn } }","variables":{"id":2001}}',
    $GRAPHQL
)

Run "GraphQL: get all salary structures" @(
    "-s", "-o", "-", "-w", "\nHTTP %{http_code}\n",
    "-X", "POST",
    "-H", "Content-Type: application/json",
    "-d", '{"query":"{ salaryStructures { structureId unitId name gradeCategory gradeId type ctcMin ctcMax footerId } }"}',
    $GRAPHQL
)

Run "GraphQL: get salary structure by ID (1001)" @(
    "-s", "-o", "-", "-w", "\nHTTP %{http_code}\n",
    "-X", "POST",
    "-H", "Content-Type: application/json",
    "-d", '{"query":"query GetSalaryStructure($id: Long!) { salaryStructure(id: $id) { structureId unitId name gradeCategory gradeId type ctcMin ctcMax footerId } }","variables":{"id":1001}}',
    $GRAPHQL
)

Run "GraphQL: get all mediclaims" @(
    "-s", "-o", "-", "-w", "\nHTTP %{http_code}\n",
    "-X", "POST",
    "-H", "Content-Type: application/json",
    "-d", '{"query":"{ mediclaims { mediclaimId refName type paidBy startDate closeDate } }"}',
    $GRAPHQL
)

Run "GraphQL: get mediclaim by ID (3001)" @(
    "-s", "-o", "-", "-w", "\nHTTP %{http_code}\n",
    "-X", "POST",
    "-H", "Content-Type: application/json",
    "-d", '{"query":"query GetMediclaim($id: Long!) { mediclaim(id: $id) { mediclaimId refName type paidBy startDate closeDate } }","variables":{"id":3001}}',
    $GRAPHQL
)

# ══════════════════════════════════════════════════════════════════
#  GRAPHQL — Mutations
# ══════════════════════════════════════════════════════════════════
Sep "GRAPHQL — Mutations"

Run "GraphQL: createSalary" @(
    "-s", "-o", "-", "-w", "\nHTTP %{http_code}\n",
    "-X", "POST",
    "-H", "Content-Type: application/json",
    "-d", '{"query":"mutation CreateSalary($cmd: CreateSalaryCommandInput!) { createSalary(command: $cmd) }","variables":{"cmd":{"salaryId":2001,"salaryType":"C","salaryCTC":1200000.00,"salaryStructureId":1001,"salaryFooterId":1,"createdBy":1}}}',
    $GRAPHQL
)

Run "GraphQL: createSalaryStructure" @(
    "-s", "-o", "-", "-w", "\nHTTP %{http_code}\n",
    "-X", "POST",
    "-H", "Content-Type: application/json",
    "-d", '{"query":"mutation CreateSalaryStructure($cmd: CreateSalaryStructureCommandInput!) { createSalaryStructure(command: $cmd) }","variables":{"cmd":{"structureId":1001,"unitId":10,"name":"Senior Engineer Band","gradeCategory":"Technical","gradeId":5,"type":"C","ctcMin":800000.00,"ctcMax":1500000.00,"footerId":1,"createdBy":1}}}',
    $GRAPHQL
)

Run "GraphQL: createMediclaim" @(
    "-s", "-o", "-", "-w", "\nHTTP %{http_code}\n",
    "-X", "POST",
    "-H", "Content-Type: application/json",
    "-d", '{"query":"mutation CreateMediclaim($cmd: CreateMediclaimCommandInput!) { createMediclaim(command: $cmd) }","variables":{"cmd":{"mediclaimId":3001,"refName":"Star Health Gold Plan 2026","type":"F","paidBy":"C","startDate":"2026-04-01T00:00:00Z","closeDate":"2027-03-31T00:00:00Z"}}}',
    $GRAPHQL
)

# ══════════════════════════════════════════════════════════════════
#  GRAPHQL — Introspection
# ══════════════════════════════════════════════════════════════════
Sep "GRAPHQL — Introspection"

Run "GraphQL: list all schema types" @(
    "-s", "-o", "-", "-w", "\nHTTP %{http_code}\n",
    "-X", "POST",
    "-H", "Content-Type: application/json",
    "-d", '{"query":"{ __schema { types { name kind } } }"}',
    $GRAPHQL
)

Run "GraphQL: inspect Query type fields" @(
    "-s", "-o", "-", "-w", "\nHTTP %{http_code}\n",
    "-X", "POST",
    "-H", "Content-Type: application/json",
    "-d", '{"query":"{ __type(name: \"Query\") { fields { name args { name type { name kind } } type { name kind } } } }"}',
    $GRAPHQL
)

Write-Host "`n$('═' * 60)" -ForegroundColor Cyan
Write-Host "  All tests completed." -ForegroundColor Green
Write-Host "$('═' * 60)`n" -ForegroundColor Cyan
