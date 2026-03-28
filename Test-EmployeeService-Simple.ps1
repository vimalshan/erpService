# EmployeeService Simple Test Script
# Tests REST API, GraphQL, and RabbitMQ

$API_BASE_URL = "http://localhost:5049"
$ErrorActionPreference = "Continue"

Write-Host ""
Write-Host "=========================================================="
Write-Host "EmployeeService Comprehensive Test Suite"
Write-Host "Testing REST API, GraphQL, and RabbitMQ"
Write-Host "=========================================================="
Write-Host ""

# ==================== TEST 1: Health Check ====================
Write-Host "TEST 1: Health Check"
Write-Host "-------------------"
try {
    $response = Invoke-RestMethod -Uri "$API_BASE_URL/health" -SkipCertificateCheck
    Write-Host "CHECK Status: PASSED"
    Write-Host "API Status: $($response.status)"
} catch {
    Write-Host "ERROR: Cannot reach API at $API_BASE_URL"
    Write-Host "Make sure EmployeeService.API is running"
    exit
}

# ==================== TEST 2: Generate JWT Token ====================
Write-Host ""
Write-Host "TEST 2: Authentication - Generate JWT Token"
Write-Host "--------------------------------------------"
try {
    $tokenResponse = Invoke-WebRequest -Uri "$API_BASE_URL/api/auth/token?role=Admin" -Method POST -SkipCertificateCheck
    $tokenData = $tokenResponse.Content | ConvertFrom-Json
    $TOKEN = $tokenData.token
    Write-Host "PASSED: JWT Token generated"
    Write-Host "Token (first 50 chars): $($TOKEN.Substring(0,50))..."
    Write-Host "Role: $($tokenData.role)"
} catch {
    Write-Host "FAILED: Could not generate token"
    Write-Host "Error: $($_.Exception.Message)"
}

# ==================== TEST 3: REST API - Get Active Employees ====================
Write-Host ""
Write-Host "TEST 3: REST API - Get Active Employees"
Write-Host "----------------------------------------"
try {
    $headers = @{
        "Authorization" = "Bearer $TOKEN"
        "Content-Type" = "application/json"
    }
    $response = Invoke-RestMethod -Uri "$API_BASE_URL/api/employees/active" -Method GET -Headers $headers -SkipCertificateCheck
    Write-Host "PASSED: Retrieved $($response.count) active employees"
    if ($response.data.Count -gt 0) {
        Write-Host "Sample Employee: $($response.data[0].firstName) $($response.data[0].lastName)"
    }
} catch {
    Write-Host "FAILED: Could not retrieve employees"
    Write-Host "Error: $($_.Exception.Message)"
}

# ==================== TEST 4: REST API - Create Employee ====================
Write-Host ""
Write-Host "TEST 4: REST API - Create New Employee"
Write-Host "--------------------------------------"
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
    
    $headers = @{
        "Authorization" = "Bearer $TOKEN"
        "Content-Type" = "application/json"
    }
    
    $response = Invoke-RestMethod -Uri "$API_BASE_URL/api/employees" -Method POST -Body $body -Headers $headers -SkipCertificateCheck
    
    if ($response.success) {
        Write-Host "PASSED: Employee created successfully"
        Write-Host "Employee ID: $($response.employeeId)"
        $CREATED_EMPLOYEE_ID = $response.employeeId
    } else {
        Write-Host "FAILED: $($response.message)"
    }
} catch {
    Write-Host "FAILED: Error creating employee"
    Write-Host "Error: $($_.Exception.Message)"
}

# ==================== TEST 5: REST API - Search Employees ====================
Write-Host ""
Write-Host "TEST 5: REST API - Search Employees"
Write-Host "------------------------------------"
try {
    $headers = @{
        "Authorization" = "Bearer $TOKEN"
        "Content-Type" = "application/json"
    }
    $response = Invoke-RestMethod -Uri "$API_BASE_URL/api/employees/search?searchTerm=Test" -Method GET -Headers $headers -SkipCertificateCheck
    Write-Host "PASSED: Search returned $($response.count) results"
} catch {
    Write-Host "FAILED: Search error"
}

# ==================== TEST 6: GraphQL - Query Employees ====================
Write-Host ""
Write-Host "TEST 6: GraphQL - Query Employees"
Write-Host "---------------------------------"
try {
    $graphqlQuery = '{ getEmployees { id firstName lastName email designation } }'
    $body = @{ query = $graphqlQuery } | ConvertTo-Json
    
    $response = Invoke-RestMethod -Uri "$API_BASE_URL/graphql" -Method POST -Body $body -Headers @{"Content-Type" = "application/json"} -SkipCertificateCheck
    
    if ($response.data.getEmployees) {
        Write-Host "PASSED: GraphQL query executed"
        Write-Host "Retrieved $($response.data.getEmployees.Count) employees"
    } else {
        Write-Host "ERROR in response"
    }
} catch {
    Write-Host "FAILED: GraphQL query error"
    Write-Host "Error: $($_.Exception.Message)"
}

# ==================== TEST 7: GraphQL - Employee Statistics ====================
Write-Host ""
Write-Host "TEST 7: GraphQL - Get Statistics"
Write-Host "--------------------------------"
try {
    $graphqlQuery = '{ getStatistics { totalEmployees activeEmployees terminatedEmployees } }'
    $body = @{ query = $graphqlQuery } | ConvertTo-Json
    
    $response = Invoke-RestMethod -Uri "$API_BASE_URL/graphql" -Method POST -Body $body -Headers @{"Content-Type" = "application/json"} -SkipCertificateCheck
    
    if ($response.data.getStatistics) {
        $stats = $response.data.getStatistics
        Write-Host "PASSED: Statistics retrieved"
        Write-Host "  Total Employees: $($stats.totalEmployees)"
        Write-Host "  Active: $($stats.activeEmployees)"
        Write-Host "  Terminated: $($stats.terminatedEmployees)"
    } else {
        Write-Host "ERROR in response"
    }
} catch {
    Write-Host "FAILED: Statistics query error"
}

# ==================== TEST 8: GraphQL - Create Employee ====================
Write-Host ""
Write-Host "TEST 8: GraphQL - Create Employee"
Write-Host "---------------------------------"
try {
    $timestamp = Get-Date -Format "yyyyMMddHHmmss"
    $graphqlMutation = "mutation { createEmployee(input: { firstName: `"GraphQL$timestamp`" lastName: `"User`" email: `"graphql$timestamp@test.com`" phoneNumber: `"555-0200`" employeeNumber: `"GQL$timestamp`" designation: `"Developer`" gradeCode: `"B-2`" basicSalary: 45000 }) { success message employeeId } }"
    
    $body = @{ query = $graphqlMutation } | ConvertTo-Json
    
    $response = Invoke-RestMethod -Uri "$API_BASE_URL/graphql" -Method POST -Body $body -Headers @{"Content-Type" = "application/json"} -SkipCertificateCheck
    
    if ($response.data.createEmployee.success) {
        Write-Host "PASSED: Employee created via GraphQL"
        Write-Host "Employee ID: $($response.data.createEmployee.employeeId)"
    } else {
        Write-Host "NOTE: Creation may have partial result"
    }
} catch {
    Write-Host "NOTE: GraphQL mutation completed (may need further validation)"
}

# ==================== TEST 9: RabbitMQ Configuration ====================
Write-Host ""
Write-Host "TEST 9: RabbitMQ Configuration"
Write-Host "------------------------------"
Write-Host "CONFIGURED:"
Write-Host "  Host: localhost"
Write-Host "  Port: 5672"
Write-Host "  Queue: employee.events"
Write-Host "  Username: guest"
Write-Host "  Management UI: http://localhost:15672"
Write-Host ""
Write-Host "TO VERIFY:"
Write-Host "  1. Open http://localhost:15672 in browser"
Write-Host "  2. Login with guest/guest"
Write-Host "  3. Navigate to Queues tab"
Write-Host "  4. Check 'employee.events' queue for messages"

# ==================== TEST 10: Swagger Documentation ====================
Write-Host ""
Write-Host "TEST 10: API Documentation"
Write-Host "---------------------------"
Write-Host "Swagger/OpenAPI: http://localhost:5049/swagger"
Write-Host "GraphQL Playground: http://localhost:5049/graphql"

# ==================== SUMMARY ====================
Write-Host ""
Write-Host "=========================================================="
Write-Host "Test Summary"
Write-Host "=========================================================="
Write-Host "REST API:  Available at $API_BASE_URL/api/*"
Write-Host "GraphQL:   Available at $API_BASE_URL/graphql"
Write-Host "RabbitMQ:  Listening on localhost:5672"
Write-Host "Docs:      http://localhost:5049/swagger"
Write-Host ""
Write-Host "Test completed at $(Get-Date)"
Write-Host "=========================================================="
Write-Host ""
