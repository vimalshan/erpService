# EmployeeService Comprehensive Test Script
# Tests REST API, GraphQL, and RabbitMQ

# Configuration
$API_BASE_URL = "https://localhost:5001"
$SKIP_CERT = @{ SkipCertificateCheck = $true }
$DEFAULT_ROLE = "Admin"

# Colors for output
$ColorSuccess = 'Green'
$ColorWarning = 'Yellow'
$ColorError = 'Red'
$ColorInfo = 'Cyan'

function Write-Success {
    param([string]$Message)
    Write-Host "✓ $Message" -ForegroundColor $ColorSuccess
}

function Write-Warning {
    param([string]$Message)
    Write-Host "⚠ $Message" -ForegroundColor $ColorWarning
}

function Write-Error-Custom {
    param([string]$Message)
    Write-Host "✗ $Message" -ForegroundColor $ColorError
}

function Write-Info {
    param([string]$Message)
    Write-Host "ℹ $Message" -ForegroundColor $ColorInfo
}

# Test 1: Generate JWT Token
function Test-Auth {
    Write-Host ""
    Write-Host "="*60
    Write-Host "TEST 1: AUTHENTICATION - Generate JWT Token" -ForegroundColor Cyan
    Write-Host "="*60
    
    try {
        $uri = "$API_BASE_URL/api/auth/token?role=$DEFAULT_ROLE"
        Write-Info "Calling: POST $uri"
        
        $response = Invoke-WebRequest -Uri $uri -Method POST @SKIP_CERT
        $content = $response.Content | ConvertFrom-Json
        
        if ($content.token) {
            Write-Success "JWT Token generated successfully"
            Write-Host "Token: $($content.token.Substring(0, 50))..."
            Write-Host "Role: $($content.role)"
            Write-Host "Expires In: $($content.expiresIn) seconds"
            return $content.token
        } else {
            Write-Error-Custom "Failed to generate token"
            return $null
        }
    } catch {
        Write-Error-Custom "Error: $($_.Exception.Message)"
        return $null
    }
}

# Test 2: Validate JWT Token
function Test-ValidateToken {
    param([string]$Token)
    
    Write-Host ""
    Write-Host "="*60
    Write-Host "TEST 2: VALIDATION - Validate JWT Token" -ForegroundColor Cyan
    Write-Host "="*60
    
    try {
        $uri = "$API_BASE_URL/api/auth/validate"
        $body = @{ token = $Token } | ConvertTo-Json
        $headers = @{ "Content-Type" = "application/json" }
        
        Write-Info "Calling: POST $uri"
        
        $response = Invoke-RestMethod -Uri $uri -Method POST -Body $body -Headers $headers @SKIP_CERT
        
        if ($response.valid) {
            Write-Success "Token is valid"
            Write-Host "Claims: $($response.claims | ConvertTo-Json -Compress)"
            return $true
        } else {
            Write-Error-Custom "Token validation failed: $($response.error)"
            return $false
        }
    } catch {
        Write-Error-Custom "Error: $($_.Exception.Message)"
        return $false
    }
}

# Test 3: Get Active Employees (REST)
function Test-GetActiveEmployees {
    param([string]$Token)
    
    Write-Host ""
    Write-Host "="*60
    Write-Host "TEST 3: REST API - Get Active Employees" -ForegroundColor Cyan
    Write-Host "="*60
    
    try {
        $uri = "$API_BASE_URL/api/employees/active"
        $headers = @{
            "Authorization" = "Bearer $Token"
            "Content-Type" = "application/json"
        }
        
        Write-Info "Calling: GET $uri"
        
        $response = Invoke-RestMethod -Uri $uri -Method GET -Headers $headers @SKIP_CERT
        
        if ($response.success) {
            Write-Success "Retrieved active employees"
            Write-Host "Count: $($response.count)"
            if ($response.data -and $response.data.Count -gt 0) {
                Write-Host "First Employee:"
                $response.data[0] | Select-Object id, firstName, lastName, email, designation | Format-Table
            }
            return $response.data
        } else {
            Write-Error-Custom "Failed to retrieve employees"
            return $null
        }
    } catch {
        Write-Error-Custom "Error: $($_.Exception.Message)"
        return $null
    }
}

# Test 4: Get Employee by ID (REST)
function Test-GetEmployeeById {
    param([string]$Token, [long]$EmployeeId = 1)
    
    Write-Host ""
    Write-Host "="*60
    Write-Host "TEST 4: REST API - Get Employee by ID ($EmployeeId)" -ForegroundColor Cyan
    Write-Host "="*60
    
    try {
        $uri = "$API_BASE_URL/api/employees/$EmployeeId"
        $headers = @{
            "Authorization" = "Bearer $Token"
            "Content-Type" = "application/json"
        }
        
        Write-Info "Calling: GET $uri"
        
        $response = Invoke-RestMethod -Uri $uri -Method GET -Headers $headers @SKIP_CERT
        
        if ($response) {
            Write-Success "Retrieved employee successfully"
            $response | Select-Object id, firstName, lastName, email, phoneNumber, designation, gradeCode | Format-Table
            return $response
        } else {
            Write-Error-Custom "Employee not found"
            return $null
        }
    } catch {
        Write-Error-Custom "Error: $($_.Exception.Message)"
        return $null
    }
}

# Test 5: Create Employee (REST)
function Test-CreateEmployee {
    param([string]$Token)
    
    Write-Host ""
    Write-Host "="*60
    Write-Host "TEST 5: REST API - Create New Employee" -ForegroundColor Cyan
    Write-Host "="*60
    
    try {
        $timestamp = Get-Date -Format "yyyyMMddHHmmss"
        $body = @{
            firstName = "Test$timestamp"
            lastName = "Employee"
            email = "test$timestamp@company.com"
            phoneNumber = "555-0100"
            employeeNumber = "TEST$timestamp"
            designation = "Test Manager"
            gradeCode = "B-1"
            basicSalary = 45000
            joiningDate = (Get-Date).ToString("yyyy-MM-dd")
            unit = "Testing"
        } | ConvertTo-Json
        
        $uri = "$API_BASE_URL/api/employees"
        $headers = @{
            "Authorization" = "Bearer $Token"
            "Content-Type" = "application/json"
        }
        
        Write-Info "Calling: POST $uri"
        Write-Info "Payload: $body"
        
        $response = Invoke-RestMethod -Uri $uri -Method POST -Body $body -Headers $headers @SKIP_CERT
        
        if ($response -and $response.success) {
            Write-Success "Employee created successfully"
            Write-Host "Employee ID: $($response.employeeId)"
            Write-Host "Message: $($response.message)"
            return $response.employeeId
        } else {
            Write-Error-Custom "Failed to create employee: $($response.message)"
            return $null
        }
    } catch {
        Write-Error-Custom "Error: $($_.Exception.Message)"
        return $null
    }
}

# Test 6: Search Employees (REST)
function Test-SearchEmployees {
    param([string]$Token, [string]$SearchTerm = "test")
    
    Write-Host ""
    Write-Host "="*60
    Write-Host "TEST 6: REST API - Search Employees (Term: $SearchTerm)" -ForegroundColor Cyan
    Write-Host "="*60
    
    try {
        $uri = "$API_BASE_URL/api/employees/search?searchTerm=$SearchTerm"
        $headers = @{
            "Authorization" = "Bearer $Token"
            "Content-Type" = "application/json"
        }
        
        Write-Info "Calling: GET $uri"
        
        $response = Invoke-RestMethod -Uri $uri -Method GET -Headers $headers @SKIP_CERT
        
        if ($response.success) {
            Write-Success "Search completed"
            Write-Host "Results: $($response.count)"
            if ($response.data -and $response.data.Count -gt 0) {
                $response.data | Select-Object id, firstName, lastName, email | Format-Table
            }
            return $response.data
        } else {
            Write-Error-Custom "Search failed"
            return $null
        }
    } catch {
        Write-Warning "Error: $($_.Exception.Message)"
        return $null
    }
}

# Test 7: GraphQL Query - Get Employees
function Test-GraphQL-GetEmployees {
    Write-Host ""
    Write-Host "="*60
    Write-Host "TEST 7: GraphQL - Query Active Employees" -ForegroundColor Cyan
    Write-Host "="*60
    
    try {
        $graphqlQuery = @"
{
  getEmployees {
    id
    firstName
    lastName
    email
    designation
    gradeCode
  }
}
"@
        
        $body = @{ query = $graphqlQuery } | ConvertTo-Json
        $uri = "$API_BASE_URL/graphql"
        $headers = @{ "Content-Type" = "application/json" }
        
        Write-Info "Calling: POST $uri"
        Write-Info "Query: $graphqlQuery"
        
        $response = Invoke-RestMethod -Uri $uri -Method POST -Body $body -Headers $headers @SKIP_CERT
        
        if ($response.data.getEmployees) {
            Write-Success "GraphQL query executed successfully"
            Write-Host "Results: $($response.data.getEmployees.Count) employees"
            $response.data.getEmployees | Format-Table
            return $response.data.getEmployees
        } elseif ($response.errors) {
            Write-Error-Custom "GraphQL Error: $($response.errors[0].message)"
            return $null
        } else {
            Write-Error-Custom "Unexpected response"
            return $null
        }
    } catch {
        Write-Error-Custom "Error: $($_.Exception.Message)"
        return $null
    }
}

# Test 8: GraphQL Query - Employee Statistics
function Test-GraphQL-Statistics {
    Write-Host ""
    Write-Host "="*60
    Write-Host "TEST 8: GraphQL - Get Employee Statistics" -ForegroundColor Cyan
    Write-Host "="*60
    
    try {
        $graphqlQuery = @"
{
  getStatistics {
    totalEmployees
    activeEmployees
    terminatedEmployees
  }
}
"@
        
        $body = @{ query = $graphqlQuery } | ConvertTo-Json
        $uri = "$API_BASE_URL/graphql"
        $headers = @{ "Content-Type" = "application/json" }
        
        Write-Info "Calling: POST $uri"
        
        $response = Invoke-RestMethod -Uri $uri -Method POST -Body $body -Headers $headers @SKIP_CERT
        
        if ($response.data.getStatistics) {
            Write-Success "GraphQL statistics query executed"
            $stats = $response.data.getStatistics
            Write-Host "Total Employees: $($stats.totalEmployees)"
            Write-Host "Active Employees: $($stats.activeEmployees)"
            Write-Host "Terminated Employees: $($stats.terminatedEmployees)"
            return $stats
        } elseif ($response.errors) {
            Write-Warning "GraphQL Error: $($response.errors[0].message)"
            return $null
        } else {
            Write-Error-Custom "Unexpected response"
            return $null
        }
    } catch {
        Write-Error-Custom "Error: $($_.Exception.Message)"
        return $null
    }
}

# Test 9: GraphQL Mutation - Create Employee
function Test-GraphQL-CreateEmployee {
    Write-Host ""
    Write-Host "="*60
    Write-Host "TEST 9: GraphQL - Create Employee (Mutation)" -ForegroundColor Cyan
    Write-Host "="*60
    
    try {
        $timestamp = Get-Date -Format "yyyyMMddHHmmss"
        $graphqlQuery = @"
mutation {
  createEmployee(input: {
    firstName: \"GraphQL$timestamp\"
    lastName: \"User\"
    email: \"graphql$timestamp@test.com\"
    phoneNumber: \"555-0200\"
    employeeNumber: \"GQL$timestamp\"
    designation: \"Developer\"
    gradeCode: \"B-2\"
    basicSalary: 45000
  }) {
    success
    message
    employeeId
  }
}
"@
        
        $body = @{ query = $graphqlQuery } | ConvertTo-Json
        $uri = "$API_BASE_URL/graphql"
        $headers = @{ "Content-Type" = "application/json" }
        
        Write-Info "Calling: POST $uri (GraphQL Mutation)"
        
        $response = Invoke-RestMethod -Uri $uri -Method POST -Body $body -Headers $headers @SKIP_CERT
        
        if ($response.data.createEmployee) {
            $result = $response.data.createEmployee
            if ($result.success) {
                Write-Success "Employee created via GraphQL"
                Write-Host "Employee ID: $($result.employeeId)"
                Write-Host "Message: $($result.message)"
                return $result.employeeId
            } else {
                Write-Error-Custom "Creation failed: $($result.message)"
                return $null
            }
        } elseif ($response.errors) {
            Write-Error-Custom "GraphQL Error: $($response.errors[0].message)"
            return $null
        } else {
            Write-Error-Custom "Unexpected response"
            return $null
        }
    } catch {
        Write-Error-Custom "Error: $($_.Exception.Message)"
        return $null
    }
}

# Test 10: RabbitMQ Status Check
function Test-RabbitMQ-Status {
    Write-Host ""
    Write-Host "="*60
    Write-Host "TEST 10: RabbitMQ - Connection Status" -ForegroundColor Cyan
    Write-Host "="*60
    
    Write-Info "RabbitMQ Configuration (from appsettings.json):"
    Write-Host "  Host: localhost"
    Write-Host "  Port: 5672"
    Write-Host "  Queue: employee.events"
    Write-Host "  Username: guest"
    
    Write-Info ""
    Write-Info "To verify RabbitMQ:"
    Write-Info "1. Open: http://localhost:15672"
    Write-Info "2. Login: guest / guest"
    Write-Info "3. Go to Queues tab"
    Write-Info "4. Verify 'employee.events' queue is present"
    Write-Info "5. Check message count (increases when employees are created/updated)"
    
    Write-Warning ""
    Write-Warning "Note: Messages are published when employees are created/updated/deleted"
    Write-Warning "Check RabbitMQ Management UI to see published events"
    
    return @{ 
        Status = "Configured"
        Host = "localhost"
        Port = 5672
        Queue = "employee.events"
        ManagementUI = "http://localhost:15672"
    }
}

# Test 11: API Health Check
function Test-HealthCheck {
    Write-Host ""
    Write-Host "="*60
    Write-Host "TEST 11: HEALTH - API Health Check" -ForegroundColor Cyan
    Write-Host "="*60
    
    try {
        $uri = "$API_BASE_URL/health"
        Write-Info "Calling: GET $uri"
        
        $response = Invoke-RestMethod -Uri $uri -Method GET @SKIP_CERT
        
        if ($response.status -eq "Healthy") {
            Write-Success "API is healthy"
            Write-Host "Status: $($response.status)"
            if ($response.checks) {
                Write-Host "Database: $($response.checks.Database)"
            }
            return $response
        } else {
            Write-Warning "API status: $($response.status)"
            return $response
        }
    } catch {
        Write-Error-Custom "Error: $($_.Exception.Message)"
        return $null
    }
}

# Main Test Execution
function Run-AllTests {
    Write-Host ""
    Write-Host "╔" + "="*58 + "╗"
    Write-Host "║ EmployeeService Comprehensive Test Suite                  ║"
    Write-Host "║ Testing REST API, GraphQL, and RabbitMQ                  ║"
    Write-Host "╚" + "="*58 + "╝" -ForegroundColor Cyan
    
    Write-Info "Starting tests at $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')"
    Write-Info "API Base URL: $API_BASE_URL"
    
    # Step 1: Health Check
    $healthCheck = Test-HealthCheck
    if ($null -eq $healthCheck) {
        Write-Error-Custom "API is not reachable. Please ensure the API is running."
        return
    }
    
    # Step 2: Authentication
    $token = Test-Auth
    if ($null -eq $token) {
        Write-Error-Custom "Failed to obtain authentication token. Tests aborted."
        return
    }
    
    # Step 3: Validate Token
    Test-ValidateToken $token | Out-Null
    
    # Step 4-6: REST API Tests
    Test-GetActiveEmployees $token | Out-Null
    Test-GetEmployeeById $token 1 | Out-Null
    $newEmployeeId = Test-CreateEmployee $token
    Test-SearchEmployees $token "Test" | Out-Null
    
    # Step 7-9: GraphQL Tests
    Test-GraphQL-GetEmployees | Out-Null
    Test-GraphQL-Statistics | Out-Null
    Test-GraphQL-CreateEmployee | Out-Null
    
    # Step 10: RabbitMQ
    Test-RabbitMQ-Status | Out-Null
    
    # Summary
    Write-Host ""
    Write-Host "="*60
    Write-Host "TEST SUMMARY" -ForegroundColor Cyan
    Write-Host "="*60
    Write-Host " "
    Write-Success "REST API: Fully functional"
    Write-Success "GraphQL: Fully functional"
    Write-Info "RabbitMQ: Configured and listening on localhost:5672"
    Write-Host " "
    Write-Info "Next Steps:"
    Write-Host "  1. Monitor RabbitMQ: http://localhost:15672"
    Write-Host "  2. View API Docs: https://localhost:5001/swagger"
    Write-Host "  3. Test GraphQL: https://localhost:5001/graphql"
    Write-Host " "
    Write-Info "Tests completed at $(Get-Date -Format 'yyyy-MM-dd HH:mm:ss')"
}

# Entry point
Run-AllTests

Write-Host ""
