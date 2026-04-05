param([string]$BaseUrl = "http://localhost:5000")

function Write-Section($title) { Write-Host "`n===== $title =====" -ForegroundColor Cyan }
function Write-Pass($msg)       { Write-Host "  [PASS] $msg" -ForegroundColor Green }
function Write-Fail($msg)       { Write-Host "  [FAIL] $msg" -ForegroundColor Red }
function Write-Info($msg)       { Write-Host "  $msg" -ForegroundColor Gray }

$errors = 0
$ts = Get-Date -Format 'HHmmss'  # unique suffix to avoid code conflicts

# ── AUTH ──────────────────────────────────────────────────────────────────────
Write-Section "AUTH: POST /api/auth/login"
try {
    $login = Invoke-RestMethod "$BaseUrl/api/auth/login" -Method Post -ContentType "application/json" -UseBasicParsing `
        -Body '{"username":"admin","password":"admin123"}'
    $TOKEN = $login.AccessToken
    $H  = @{ Authorization = "Bearer $TOKEN" }
    $HJ = @{ Authorization = "Bearer $TOKEN"; "Content-Type" = "application/json" }
    Write-Pass "Login OK  |  Token: $($TOKEN.Substring(0,40))..."
} catch {
    Write-Fail "Login failed: $_"
    exit 1
}

# ── REST: Create Approval ─────────────────────────────────────────────────────
Write-Section "REST: POST /api/approvals"
try {
    $body = "{`"Code`":`"REST-$ts`",`"Name`":`"Leave Approval`",`"Module`":`"LeaveManagement`",`"Level`":2}"
    $created = Invoke-RestMethod "$BaseUrl/api/approvals" -Method Post -Headers $HJ -Body $body -UseBasicParsing
    Write-Pass "Created  |  Id=$($created.Id)  Code=$($created.Code)"
    $approvalId = $created.Id
} catch {
    Write-Fail "Create failed: $($_.ErrorDetails.Message)"
    $errors++
    $approvalId = 1
}

# ── REST: Get All ─────────────────────────────────────────────────────────────
Write-Section "REST: GET /api/approvals"
try {
    $all = Invoke-RestMethod "$BaseUrl/api/approvals" -Headers $H -UseBasicParsing
    Write-Pass "GetAll OK  |  Count=$($all.Count)"
    $all | ForEach-Object { Write-Info "  Id=$($_.Id)  Code=$($_.Code)  Module=$($_.Module)" }
} catch {
    Write-Fail "GetAll failed: $($_.ErrorDetails.Message)"
    $errors++
}

# ── REST: Get By ID ───────────────────────────────────────────────────────────
Write-Section "REST: GET /api/approvals/$approvalId"
try {
    $byId = Invoke-RestMethod "$BaseUrl/api/approvals/$approvalId" -Headers $H -UseBasicParsing
    Write-Pass "GetById OK  |  Id=$($byId.Id)  Name=$($byId.Name)"
} catch {
    Write-Fail "GetById failed: $($_.ErrorDetails.Message)"
    $errors++
}

# ── REST: Get By Module ───────────────────────────────────────────────────────
Write-Section "REST: GET /api/approvals/module/LeaveManagement"
try {
    $byMod = Invoke-RestMethod "$BaseUrl/api/approvals/module/LeaveManagement" -Headers $H -UseBasicParsing
    Write-Pass "GetByModule OK  |  Count=$($byMod.Count)"
} catch {
    Write-Fail "GetByModule failed: $($_.ErrorDetails.Message)"
    $errors++
}

# ── Minimal API: Get All ──────────────────────────────────────────────────────
Write-Section "MINIMAL API: GET /api/minimal/approvals"
try {
    $mAll = Invoke-RestMethod "$BaseUrl/api/minimal/approvals" -UseBasicParsing
    Write-Pass "Minimal GetAll OK  |  Count=$($mAll.Count)"
} catch {
    Write-Fail "Minimal GetAll failed: $($_.ErrorDetails.Message)"
    $errors++
}

# ── Minimal API: Get By ID ────────────────────────────────────────────────────
Write-Section "MINIMAL API: GET /api/minimal/approvals/$approvalId"
try {
    $mById = Invoke-RestMethod "$BaseUrl/api/minimal/approvals/$approvalId" -UseBasicParsing
    Write-Pass "Minimal GetById OK  |  Id=$($mById.Id)  Name=$($mById.Name)"
} catch {
    Write-Fail "Minimal GetById failed: $($_.ErrorDetails.Message)"
    $errors++
}

# ── Minimal API: Create ───────────────────────────────────────────────────────
Write-Section "MINIMAL API: POST /api/minimal/approvals"
try {
    $mBody = "{`"Code`":`"MIN-$ts`",`"Name`":`"Expense Approval`",`"Module`":`"Expense`",`"Level`":1}"
    $mCreated = Invoke-RestMethod "$BaseUrl/api/minimal/approvals" -Method Post -ContentType "application/json" -UseBasicParsing -Body $mBody
    Write-Pass "Minimal Create OK  |  Id=$($mCreated.Id)  Code=$($mCreated.Code)"
} catch {
    Write-Fail "Minimal Create failed: $($_.ErrorDetails.Message)"
    $errors++
}

# ── GraphQL: Query ────────────────────────────────────────────────────────────
Write-Section "GRAPHQL: POST /graphql (getApprovals query)"
try {
    $gqlQuery = '{"query":"{ approvals { id code name module level status } }"}'
    $gql = Invoke-RestMethod "$BaseUrl/graphql" -Method Post -ContentType "application/json" -UseBasicParsing -Body $gqlQuery
    if ($gql.errors) {
        Write-Fail "GraphQL errors: $($gql.errors | ConvertTo-Json -Compress)"
        $errors++
    } else {
        Write-Pass "GraphQL Query OK  |  Count=$($gql.data.approvals.Count)"
        $gql.data.approvals | ForEach-Object { Write-Info "  id=$($_.id)  code=$($_.code)  module=$($_.module)" }
    }
} catch {
    Write-Fail "GraphQL failed: $($_.ErrorDetails.Message)"
    $errors++
}

# ── GraphQL: Query by ID ──────────────────────────────────────────────────────
Write-Section "GRAPHQL: POST /graphql (getApprovalById query)"
try {
    $gqlById = "{`"query`":`"{ approvalById(id: $approvalId) { id code name module level } }`"}"
    $gqlR = Invoke-RestMethod "$BaseUrl/graphql" -Method Post -ContentType "application/json" -UseBasicParsing -Body $gqlById
    if ($gqlR.errors) {
        Write-Fail "GraphQL by ID errors: $($gqlR.errors | ConvertTo-Json -Compress)"
        $errors++
    } else {
        Write-Pass "GraphQL ById OK  |  $($gqlR.data.approvalById | ConvertTo-Json -Compress)"
    }
} catch {
    Write-Fail "GraphQL ById failed: $($_.ErrorDetails.Message)"
    $errors++
}

# ── GraphQL: Mutation ─────────────────────────────────────────────────────────
Write-Section "GRAPHQL: POST /graphql (createApproval mutation)"
try {
    $gqlCode = "GQL-$ts"
    $gqlMut = '{"query":"mutation { createApproval(code: \"' + $gqlCode + '\", name: \"HR Approval\", module: \"HR\", level: 1, userId: 1) { id code } }"}'
    $gqlMR = Invoke-RestMethod "$BaseUrl/graphql" -Method Post -ContentType "application/json" -UseBasicParsing -Body $gqlMut
    if ($gqlMR.errors) {
        Write-Fail "GraphQL Mutation errors: $($gqlMR.errors | ConvertTo-Json -Compress)"
        $errors++
    } else {
        Write-Pass "GraphQL Mutation OK  |  $($gqlMR.data.createApproval | ConvertTo-Json -Compress)"
    }
} catch {
    Write-Fail "GraphQL Mutation failed: $($_.ErrorDetails.Message)"
    $errors++
}

# ── RabbitMQ Test ─────────────────────────────────────────────────────────────
Write-Section "RABBITMQ: GET /api/rabbitmq/test"
try {
    $rmq = Invoke-RestMethod "$BaseUrl/api/rabbitmq/test" -UseBasicParsing
    if ($rmq.status -eq "ok") {
        Write-Pass "RabbitMQ CONNECTED  |  $($rmq.message)"
    } elseif ($rmq.status -eq "unavailable") {
        Write-Pass "RabbitMQ not running (expected in dev)  |  $($rmq.message)"
    } else {
        Write-Fail "RabbitMQ unexpected status: $($rmq.message)"
        $errors++
    }
} catch {
    $statusCode = $_.Exception.Response.StatusCode.value__
    if ($statusCode -eq 503) {
        Write-Pass "RabbitMQ not configured (HTTP 503, graceful - expected in dev)"
    } else {
        Write-Fail "RabbitMQ returned HTTP $statusCode"
        $errors++
    }
}

# ── Summary ───────────────────────────────────────────────────────────────────
Write-Host ""
if ($errors -eq 0) {
    Write-Host "=== ALL TESTS PASSED ===" -ForegroundColor Green
} else {
    Write-Host "=== $errors TEST(S) FAILED ===" -ForegroundColor Red
}
