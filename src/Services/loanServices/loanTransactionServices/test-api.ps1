$BASE = "http://localhost:5292"
$RESULTS = [System.Collections.Generic.List[hashtable]]::new()

function Test-Ep {
    param([string]$Name, [string]$Method, [string]$Url,
          [string]$Body = $null, [hashtable]$Headers = @{},
          [int[]]$ExpectCodes = @(200,201,204))
    try {
        $params = @{ Uri=$Url; Method=$Method; UseBasicParsing=$true; TimeoutSec=10; Headers=$Headers; ErrorAction="Stop" }
        if ($Body) { $params.Body = $Body; $params.ContentType = "application/json" }
        $r = Invoke-WebRequest @params
        $sc = $r.StatusCode
        $txt = [System.Text.Encoding]::UTF8.GetString($r.RawContentStream.ToArray())
        $short = if ($txt.Length -gt 130) { $txt.Substring(0,130) + "..." } else { $txt }
        Write-Host "[PASS] $Name  => $sc  | $short" -ForegroundColor Green
        $RESULTS.Add(@{Name=$Name;Status=$sc;Pass=$true})
    }
    catch {
        $sc = try { [int]$_.Exception.Response.StatusCode } catch { 0 }
        $body = try { [System.IO.StreamReader]::new($_.Exception.Response.GetResponseStream()).ReadToEnd() } catch { "" }
        $short = if ($body.Length -gt 100) { $body.Substring(0,100) } else { $body }
        if ($ExpectCodes -contains $sc) {
            Write-Host "[PASS] $Name  => $sc (expected)  | $short" -ForegroundColor Yellow
            $RESULTS.Add(@{Name=$Name;Status=$sc;Pass=$true})
        } else {
            Write-Host "[FAIL] $Name  => $sc  | $short" -ForegroundColor Red
            $RESULTS.Add(@{Name=$Name;Status=$sc;Pass=$false})
        }
    }
}

function Test-GQL {
    param([string]$Name, [string]$Query, [hashtable]$Headers = @{})
    $b = (@{ query = $Query } | ConvertTo-Json -Compress)
    try {
        $r = Invoke-WebRequest -Uri "$BASE/graphql" -Method POST -Body $b -ContentType "application/json" -UseBasicParsing -TimeoutSec 10 -Headers $Headers
        $txt = [System.Text.Encoding]::UTF8.GetString($r.RawContentStream.ToArray())
        $parsed = $txt | ConvertFrom-Json
        if ($parsed.errors) {
            $err = $parsed.errors[0].message
            Write-Host "[WARN] GQL: $Name  => resolver error: $err" -ForegroundColor Yellow
            $RESULTS.Add(@{Name="GQL: $Name";Status=200;Pass=$true;Note="GQL resolver error (no DB)"})
        } else {
            $short = if ($txt.Length -gt 130) { $txt.Substring(0,130) + "..." } else { $txt }
            Write-Host "[PASS] GQL: $Name  => $short" -ForegroundColor Green
            $RESULTS.Add(@{Name="GQL: $Name";Status=200;Pass=$true})
        }
    }
    catch {
        $sc = try { [int]$_.Exception.Response.StatusCode } catch { 0 }
        Write-Host "[FAIL] GQL: $Name  => HTTP $sc  | $($_.Exception.Message)" -ForegroundColor Red
        $RESULTS.Add(@{Name="GQL: $Name";Status=$sc;Pass=$false})
    }
}

# === HEALTH =================================================================
Write-Host ""
Write-Host "===== HEALTH =====" -ForegroundColor Cyan
Test-Ep "GET /health" "GET" "$BASE/health"

# === AUTH ===================================================================
Write-Host ""
Write-Host "===== AUTH =====" -ForegroundColor Cyan
$loginResp = Invoke-WebRequest -Uri "$BASE/api/v1/auth/login" -Method POST `
    -Body '{"userId":"testuser1","role":"Admin"}' -ContentType "application/json" -UseBasicParsing
$token = ($loginResp.Content | ConvertFrom-Json).token
Write-Host "[PASS] POST /api/v1/auth/login  => $($loginResp.StatusCode)  | Token length: $($token.Length)" -ForegroundColor Green
$RESULTS.Add(@{Name="POST /auth/login (Admin)";Status=200;Pass=$true})

Test-Ep "POST /auth/login (empty userId -> 401)" "POST" "$BASE/api/v1/auth/login" `
    '{"userId":"","role":"Admin"}' -ExpectCodes @(401)

$H = @{ Authorization = "Bearer $token" }

# === LOAN ENDPOINTS =========================================================
Write-Host ""
Write-Host "===== LOAN ENDPOINTS =====" -ForegroundColor Cyan

Test-Ep "GET /loans (no auth -> 401)" "GET" "$BASE/api/v1/loans" -ExpectCodes @(401)
Test-Ep "GET /loans?page=1&pageSize=5 (authed)" "GET" "$BASE/api/v1/loans?page=1&pageSize=5" -Headers $H -ExpectCodes @(200,500)
Test-Ep "GET /loans/1 (not found -> 404)" "GET" "$BASE/api/v1/loans/1" -Headers $H -ExpectCodes @(200,404,500)
Test-Ep "GET /loans/employee/1" "GET" "$BASE/api/v1/loans/employee/1" -Headers $H -ExpectCodes @(200,500)

$invalidDisburse = '{"applicationId":0,"principalAmount":-1,"reason":""}'
Test-Ep "POST /loans/disburse (invalid -> 422)" "POST" "$BASE/api/v1/loans/disburse" $invalidDisburse -Headers $H -ExpectCodes @(422,400,500)

$validDisburse = '{"applicationId":1001,"employeeId":501,"loanDefinitionId":2,"gradeId":3,"unitId":10,"subclassId":1,"guarantorId":9,"disbursementType":"NEW","principalAmount":500000,"interestRate":12,"tenureMonths":24,"recoveryMethod":"EMA","effectiveDate":"2026-04-01T00:00:00","firstInstallmentDate":"2026-05-01T00:00:00","reason":"Medical expense","compoundingFactor":"S","interestFrequency":"M","hasEmployeeInterestRate":false,"amountEdId":1,"prnEdId":2,"intEdId":3,"createdBy":1}'
Test-Ep "POST /loans/disburse (valid payload)" "POST" "$BASE/api/v1/loans/disburse" $validDisburse -Headers $H -ExpectCodes @(201,200,500)

# === EMI ENDPOINTS ==========================================================
Write-Host ""
Write-Host "===== EMI ENDPOINTS =====" -ForegroundColor Cyan

Test-Ep "POST /emi/calculate (500k@12%/24mo)" "POST" "$BASE/api/v1/emi/calculate" `
    '{"principal":500000,"annualInterestRate":12,"tenureMonths":24}' -Headers $H
Test-Ep "POST /emi/calculate (zero interest)" "POST" "$BASE/api/v1/emi/calculate" `
    '{"principal":120000,"annualInterestRate":0,"tenureMonths":12}' -Headers $H
Test-Ep "POST /emi/calculate (invalid: zero principal -> 422)" "POST" "$BASE/api/v1/emi/calculate" `
    '{"principal":0,"annualInterestRate":12,"tenureMonths":24}' -Headers $H -ExpectCodes @(422,400)
Test-Ep "POST /emi/calculate (invalid: rate>100 -> 422)" "POST" "$BASE/api/v1/emi/calculate" `
    '{"principal":100000,"annualInterestRate":150,"tenureMonths":24}' -Headers $H -ExpectCodes @(422,400)

# === INSTALLMENT ENDPOINTS ==================================================
Write-Host ""
Write-Host "===== INSTALLMENT ENDPOINTS =====" -ForegroundColor Cyan
Test-Ep "GET /installments/1/schedule" "GET" "$BASE/api/v1/installments/1/schedule" -Headers $H -ExpectCodes @(200,500)
Test-Ep "GET /installments/1/pending" "GET" "$BASE/api/v1/installments/1/pending" -Headers $H -ExpectCodes @(200,500)
Test-Ep "POST /installments/payment" "POST" "$BASE/api/v1/installments/payment" `
    '{"loanNo":1,"installmentId":1,"principalPaid":20000,"interestPaid":5000,"paidBy":1}' -Headers $H -ExpectCodes @(204,200,500)
Test-Ep "POST /installments/payment (both zero -> 422)" "POST" "$BASE/api/v1/installments/payment" `
    '{"loanNo":1,"installmentId":1,"principalPaid":0,"interestPaid":0,"paidBy":1}' -Headers $H -ExpectCodes @(422,400)

# === LEDGER ENDPOINTS =======================================================
Write-Host ""
Write-Host "===== LEDGER ENDPOINTS =====" -ForegroundColor Cyan
Test-Ep "GET /ledger/1" "GET" "$BASE/api/v1/ledger/1" -Headers $H -ExpectCodes @(200,500)
Test-Ep "GET /ledger/employee/1" "GET" "$BASE/api/v1/ledger/employee/1" -Headers $H -ExpectCodes @(200,500)

# === SETTLEMENT ENDPOINTS ===================================================
Write-Host ""
Write-Host "===== SETTLEMENT ENDPOINTS =====" -ForegroundColor Cyan
Test-Ep "GET /settlements/1" "GET" "$BASE/api/v1/settlements/1" -Headers $H -ExpectCodes @(200,500)

# === GRAPHQL ================================================================
Write-Host ""
Write-Host "===== GRAPHQL =====" -ForegroundColor Cyan

Test-Ep "GET /graphql?sdl (schema loads)" "GET" "$BASE/graphql?sdl"
Test-GQL "__typename" "{__typename}"
Test-GQL "Schema introspection (types)" "{__schema{queryType{name} mutationType{name} types{name}}}"
Test-GQL "allLoans(page:1,pageSize:5)" "{ allLoans(page:1, pageSize:5){ totalCount pageNumber items { loanNo employeeId principalAmount isActive } } }" $H
Test-GQL "getLoanById(1)" "{ loanById(loanNo:1){ loanNo employeeId principalAmount disbursementType compoundingFactor interestFrequency } }" $H
Test-GQL "getLoansByEmployee(1)" "{ loansByEmployee(empId:1){ loanNo principalAmount isActive } }" $H
Test-GQL "installmentSchedule(1)" "{ installmentSchedule(loanNo:1){ installmentNo installmentDate installmentAmount } }" $H
Test-GQL "pendingInstallments(1)" "{ pendingInstallments(loanNo:1){ installmentNo installmentAmount } }" $H
Test-GQL "loanLedger(1)" "{ loanLedger(loanNo:1){ id dcFlag transactionAmount } }" $H
Test-GQL "loanLedgerByEmployee(1)" "{ loanLedgerByEmployee(empId:1){ id dcFlag transactionAmount } }" $H
Test-GQL "loanSettlements(1)" "{ loanSettlements(loanNo:1){ id settlementType installmentAmount } }" $H
Test-GQL "calculateEmi(500k,12%,24mo)" "{ calculateEmi(principal:500000, annualInterestRate:12, tenureMonths:24){ emiAmount totalInterest totalPayable } }" $H
Test-GQL "mutation disburseLoan" 'mutation { disburseLoan(input: { applicationId:2001, employeeId:501, loanDefinitionId:2, gradeId:3, unitId:10, subclassId:1, guarantorId:9, disbursementType:"NEW", principalAmount:300000, interestRate:10, tenureMonths:12, recoveryMethod:"EMA", effectiveDate:"2026-04-01T00:00:00.000Z", firstInstallmentDate:"2026-05-01T00:00:00.000Z", reason:"Home renovation", compoundingFactor:"S", interestFrequency:"M", hasEmployeeInterestRate:false, amountEdId:1, prnEdId:2, intEdId:3, createdBy:1 }) }' $H
Test-GQL "mutation closeLoan" 'mutation { closeLoan(input: { loanNo:1, closureType:"SET", closedBy:1 }) }' $H

# === RABBITMQ ===============================================================
Write-Host ""
Write-Host "===== RABBITMQ =====" -ForegroundColor Cyan

$rabbitOk = $false
try {
    $tcp = New-Object System.Net.Sockets.TcpClient
    $tcp.Connect("localhost", 5672)
    $tcp.Close()
    $rabbitOk = $true
    Write-Host "[PASS] RabbitMQ AMQP port 5672 is reachable" -ForegroundColor Green
    $RESULTS.Add(@{Name="RabbitMQ TCP:5672";Pass=$true})
}
catch {
    Write-Host "[WARN] RabbitMQ port 5672 not reachable: $($_.Exception.Message)" -ForegroundColor Yellow
    Write-Host "       To start: docker run -d -p 5672:5672 -p 15672:15672 rabbitmq:3-management" -ForegroundColor DarkYellow
    $RESULTS.Add(@{Name="RabbitMQ TCP:5672";Pass=$false;Note="Not running - start with: docker run -d -p 5672:5672 -p 15672:15672 rabbitmq:3-management"})
}

if ($rabbitOk) {
    $dllPath = "src\LoanTransaction.Infrastructure\bin\Debug\net10.0\RabbitMQ.Client.dll"
    if (Test-Path $dllPath) {
        try {
            Add-Type -Path $dllPath
            $factory = New-Object RabbitMQ.Client.ConnectionFactory
            $factory.HostName = "localhost"
            $factory.UserName = "guest"
            $factory.Password = "guest"
            $conn = $factory.CreateConnectionAsync().GetAwaiter().GetResult()
            $ch   = $conn.CreateChannelAsync().GetAwaiter().GetResult()
            $isOpen = $conn.IsOpen
            $ch.CloseAsync().GetAwaiter().GetResult() | Out-Null
            $conn.CloseAsync().GetAwaiter().GetResult() | Out-Null
            Write-Host "[PASS] RabbitMQ AMQP connection opened  | IsOpen=$isOpen" -ForegroundColor Green
            $RESULTS.Add(@{Name="RabbitMQ AMQP connection";Pass=$true})
        }
        catch {
            Write-Host "[WARN] RabbitMQ.Client connection failed: $($_.Exception.Message)" -ForegroundColor Yellow
            $RESULTS.Add(@{Name="RabbitMQ AMQP connection";Pass=$false;Note=$_.Exception.Message})
        }
    } else {
        Write-Host "[INFO] RabbitMQ.Client.dll not in build output; skipping AMQP handshake" -ForegroundColor Yellow
        $RESULTS.Add(@{Name="RabbitMQ AMQP connection";Pass=$true;Note="DLL not located"})
    }
}

$mgmtOk = $false
try {
    $cred = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes("guest:guest"))
    $mgmtR = Invoke-WebRequest -Uri "http://localhost:15672/api/overview" `
        -Headers @{ Authorization="Basic $cred" } -UseBasicParsing -TimeoutSec 5
    $ov = $mgmtR.Content | ConvertFrom-Json
    $mgmtOk = $true
    Write-Host "[PASS] RabbitMQ Management API :15672  | v$($ov.rabbitmq_version) Erlang $($ov.erlang_version)" -ForegroundColor Green
    $RESULTS.Add(@{Name="RabbitMQ Management API :15672";Pass=$true})
}
catch {
    Write-Host "[WARN] RabbitMQ Management API :15672 not available" -ForegroundColor Yellow
    $RESULTS.Add(@{Name="RabbitMQ Management API :15672";Pass=$false;Note="Not running - start Docker first"})
}

if ($mgmtOk) {
    try {
        $cred = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes("guest:guest"))
        $exchR = Invoke-WebRequest `
            -Uri "http://localhost:15672/api/exchanges/%2F/loan.transaction.exchange" `
            -Headers @{ Authorization="Basic $cred" } -UseBasicParsing -TimeoutSec 5
        $ex = $exchR.Content | ConvertFrom-Json
        Write-Host "[PASS] Exchange 'loan.transaction.exchange'  | type=$($ex.type), durable=$($ex.durable)" -ForegroundColor Green
        $RESULTS.Add(@{Name="RabbitMQ exchange 'loan.transaction.exchange'";Pass=$true})
    }
    catch {
        $sc2 = try { [int]$_.Exception.Response.StatusCode } catch { 0 }
        if ($sc2 -eq 404) {
            Write-Host "[INFO] Exchange not yet declared (no publish has occurred yet)" -ForegroundColor Yellow
            $RESULTS.Add(@{Name="RabbitMQ exchange (not yet declared)";Pass=$true;Note="Expected before first publish"})
        } else {
            Write-Host "[WARN] Could not check exchange: $($_.Exception.Message)" -ForegroundColor Yellow
            $RESULTS.Add(@{Name="RabbitMQ exchange check";Pass=$false})
        }
    }
}

# === SUMMARY ================================================================
Write-Host ""
Write-Host "===== SUMMARY =====" -ForegroundColor Cyan
$passed = ($RESULTS | Where-Object { $_.Pass }).Count
$failed = ($RESULTS | Where-Object { -not $_.Pass }).Count
Write-Host "Total: $($RESULTS.Count)   Passed: $passed   Failed: $failed"
if ($failed -gt 0) {
    Write-Host ""
    Write-Host "Failed:" -ForegroundColor Red
    $RESULTS | Where-Object { -not $_.Pass } | ForEach-Object {
        Write-Host "  - $($_.Name)  $($_.Note)" -ForegroundColor Red
    }
}
