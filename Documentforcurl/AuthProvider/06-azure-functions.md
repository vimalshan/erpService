# AuthProvider API Documentation

---

## Azure Functions

AuthProvider includes two Azure Functions in the `functions/AuthProvider.Functions` project.

### TokenCleanupFunction

**Trigger:** Timer (`0 0 * * * *` – every hour at :00)  
**Purpose:** Cleans up expired and revoked refresh tokens from the database.

- Queries `RefreshTokens` table for tokens where `ExpiresAt < NOW()` or `RevokedAt IS NOT NULL`
- Deletes matching records via EF Core
- Logs the count of deleted tokens

### UserCreatedFunction

**Trigger:** Azure Service Bus (`auth.events.usercreatedevent` topic)  
**Purpose:** Processes new user creation events published via RabbitMQ/Service Bus.

**Message Schema:**
```json
{
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "email": "john@example.com",
  "username": "johndoe"
}
```

**Actions Performed:**
1. Sends a welcome email (stub/placeholder)
2. Archives the user creation event to Azure Blob Storage at `auth-user-events/users/{userId}/created-{timestamp}.json`

---

