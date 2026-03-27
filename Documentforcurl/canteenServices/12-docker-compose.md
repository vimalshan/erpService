# Canteen Services API Documentation

---

## Docker Compose Setup

```bash
cd src/Services/canteenServices
docker-compose -f docker-compose.shared.yml -f docker-compose.yml up -d
```

### Infrastructure Services

| Service | Port | Description |
|---|---|---|
| SQL Server 2022 | 1434 | Shared database (7 databases) |
| RabbitMQ | 5673 / 15673 | Message broker / management UI |
| Azurite | 10010-10012 | Azure Storage emulator |

### Application Services

| Service | Port |
|---|---|
| API Gateway | 5188 |
| CanteenUnit | 5190 |
| CardManagement | 5191 |
| Deduction | 5192 |
| Eligibility | 5193 |
| ItemMaster | 5194 |
| ReferenceData | 5195 |
| SwipeTransaction | 5196 |
| CanteenTransaction | 5197 |
