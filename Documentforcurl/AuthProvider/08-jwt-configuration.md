# AuthProvider API Documentation

---

## JWT Configuration

| Setting | Value |
|---|---|
| Secret Key | `AuthProviderSuperSecretKey_ChangeInProduction_Min32Chars!` |
| Issuer | `AuthProvider` |
| Audience | `AuthProviderClients` |
| Access Token Expiry | 60 minutes |
| Refresh Token Expiry | 7 days |
| Algorithm | HMAC-SHA256 |
| Clock Skew | Zero |

### JWT Claims

| Claim | Description |
|---|---|
| `sub` | User ID (GUID) |
| `email` | User email address |
| `unique_name` | Username |
| `jti` | Unique token identifier |
| `firstName` | User first name |
| `lastName` | User last name |
| `role` | User roles (one claim per role) |

---

