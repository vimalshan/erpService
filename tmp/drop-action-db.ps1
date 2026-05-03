$conn = New-Object System.Data.SqlClient.SqlConnection 'Server=(localdb)\MSSQLLocalDB;Database=master;Integrated Security=True;TrustServerCertificate=True'
$conn.Open()
$cmd = $conn.CreateCommand()
$cmd.CommandText = @"
IF DB_ID('ERPActionDB') IS NOT NULL
BEGIN
    ALTER DATABASE ERPActionDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE ERPActionDB;
END
"@
$cmd.ExecuteNonQuery() | Out-Null
$conn.Close()
Write-Host "Drop complete" -ForegroundColor Green
