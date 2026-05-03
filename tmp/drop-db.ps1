param([string]$DbName = 'ERPCertificateDB')
$conn = New-Object System.Data.SqlClient.SqlConnection "Server=(localdb)\MSSQLLocalDB;Database=master;Integrated Security=True;TrustServerCertificate=True"
$conn.Open()
$cmd = $conn.CreateCommand()
$cmd.CommandText = "IF DB_ID('$DbName') IS NOT NULL BEGIN ALTER DATABASE [$DbName] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [$DbName]; END"
$cmd.ExecuteNonQuery() | Out-Null
$conn.Close()
Write-Host "$DbName dropped (or did not exist)" -ForegroundColor Green
