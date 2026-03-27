# AIMS Services — API Documentation

---

## Quick Reference — Port Summary

| Service             | Port | Auth Endpoint                    | GraphQL Endpoint          |
| ------------------- | ---- | -------------------------------- | ------------------------- |
| API Gateway         | 5020 | —                                | `/api/graphqlproxy/{svc}` |
| Access              | 5010 | `POST /api/auth/login`           | `/graphql`                |
| Attendance          | 5011 | `POST /api/auth/login`           | `/graphql`                |
| Bus                 | 5012 | `POST /api/auth/login`           | `/graphql`                |
| Calendar            | 5013 | `POST /api/auth/token`           | `/graphql`                |
| Employee            | 5014 | `POST /api/auth/token`           | `/graphql`                |
| Group Incentive     | 5015 | `POST /api/auth/login`           | `/graphql`                |
| Leave               | 5016 | (via gateway)                    | `/graphql`                |
| Reference           | 5017 | (via gateway)                    | `/graphql`                |
| Visitor             | 5018 | (via gateway)                    | `/graphql`                |
| AIMS Transaction    | 5019 | (via gateway)                    | `/graphql`                |
