# AuthProvider API Documentation

---

## Docker Compose Setup

Start the full stack:

```bash
cd src/Services/AuthProvider
docker-compose up -d
```

### Services

| Service | Container | Port | Description |
|---|---|---|---|
| SQL Server 2022 | `mssql-auth-db` | `1433` | Database (AuthProviderDB) |
| AuthProvider API | `auth-provider` | `5200` | API service |

### Connection String

```
Data Source=localhost,1433;Initial Catalog=AuthProviderDB;User ID=sa;Password=YourPassword123!;Encrypt=False;TrustServerCertificate=True;
```

---

