
# TodoApi – Clean Architecture .NET 8 REST API

A fully structured Todo API built with **ASP.NET Core 8**, featuring clean layering, DTOs, FluentValidation, AutoMapper, EF Core 8, soft delete, idempotency protection, rate limiting, JWT authentication wiring, Serilog logging, and Swagger documentation.

This project is built for learning **real-world API standards** and production patterns.

---

## 🚀 Features

- Clean Architecture (Controllers → Services → Repositories → EF Core)
- Entity Framework Core 8 with SQL Server
- Global Soft Delete (`IsDeleted` + query filter)
- DTO-based input/output (safe API contracts)
- FluentValidation for all requests
- AutoMapper for mapping Entities ⇆ DTOs
- Idempotency Middleware (prevents duplicate POST/PUT/PATCH)
- Rate Limiting (60 requests/min per IP)
- Serilog Logging (console + rolling files)
- Swagger/OpenAPI with custom headers
- CORS Enabled
- JWT Authentication (wired, optional)

---

## 📁 Project Structure

```
TodoApi/
│── TodoApi/                      # Presentation Layer (Controllers, Program.cs)
│    ├── Controllers/
│    ├── Middleware/
│    ├── Swagger/
│
│── TodoApi.Application/          # Application Layer
│    ├── Dtos/
│    ├── Services/
│    ├── Mapping/
│    ├── Validation/
│
│── TodoApi.Domain/               # Domain Entities
│    ├── Entities/
│
│── TodoApi.Infrastructure/       # Infrastructure Layer
│    ├── Persistence (DbContext)
│    ├── Repositories
│
└── README.md
```

---

## 🧠 Why This Architecture?

### ✔ Repositories return **TodoItem** (Database shape)  
Because EF Core tracks database entity states.

### ✔ Services return **TodoDto** (API/Client shape)  
To hide internal fields, enforce API contracts, and avoid leaking DB structure.

---

## 🎛 Technologies Explained (WHY & WHEN)

| Tech | WHY | WHEN |
|------|------|------|
| **DTOs** | Protect API contract | Anytime external clients exist |
| **AutoMapper** | Avoid manual mapping | Medium/large projects |
| **FluentValidation** | Rich expressive rules | Beyond simple attributes |
| **Soft Delete** | Prevent data loss | User-generated data |
| **Idempotency** | Prevent duplicate writes | POST/PUT/PATCH |
| **Rate Limiting** | Protect from abuse | Public APIs |
| **JWT Auth** | Stateless identity | Mobile/SPA/microservices |
| **Serilog** | Structured production logs | Always |
| **MemoryCache** | Simple caching | Dev / single server |
| **Redis** | Distributed caching | Cloud / multi-server |

---

## 🗄 Database

### Global Query Filter (Soft Delete)

EF Core automatically hides deleted rows:

```csharp
modelBuilder.Entity<TodoItem>()
    .HasQueryFilter(t => !t.IsDeleted);
```

To fetch deleted rows:

```csharp
.IgnoreQueryFilters().Where(t => t.IsDeleted)
```

---

## 🔒 Idempotency (Duplicate Request Protection)

All POST/PUT/PATCH requests require:

```
Idempotency-Key: <GUID>
```

Prevents:
- Double-click submissions  
- Network retry duplicates  
- Load balancer retry requests  

Keys are stored for 30 minutes.

---

## 🚦 Rate Limiting

Configured as:

```
60 requests/min per IP
```

---

## 📜 Logging (Serilog)

- Console logging during development  
- Rolling log files under `/Logs/`  

---

## 🔧 Setup Instructions

### 1. Install EF Tools
```bash
dotnet tool install --global dotnet-ef
```

### 2. Update Connection String
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=TodoApi;Trusted_Connection=True;"
}
```

### 3. Add Migration
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 4. Run the API
```bash
dotnet run
```

Open:
```
https://localhost:<port>/swagger
```

---

## 📡 Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/todos` | Get paged items |
| GET | `/api/todos/{id}` | Get by ID |
| GET | `/api/todos/deleted` | Get deleted items |
| POST | `/api/todos` | Create (Idempotency-Key required) |
| PUT | `/api/todos/{id}` | Update |
| DELETE | `/api/todos/{id}` | Soft delete |

---

## 🧪 Testing (Curl)

```bash
curl -X POST "https://localhost:7208/api/todos" ^
  -H "Content-Type: application/json" ^
  -H "Idempotency-Key: $(uuidgen)" ^
  -d "{\"title\":\"Study .NET 8\",\"description\":\"Learn clean architecture\"}"
```

---

## 🔮 Future Extensions

- Refresh tokens / JWT roles  
- Redis-backed idempotency  
- Background jobs (Hangfire)  
- Email notifications  
- Pagination metadata  
- Dockerization  

---

## 🤝 Contributing

Pull requests welcome.  
You can fork the project and extend it (authentication, roles, Redis, Docker, etc.).

---

## 🏁 License  
MIT — free to use, learn from, and adapt.

---

## 📦 NuGet Packages Used
EF Core

Microsoft.EntityFrameworkCore — EF Core ORM

Microsoft.EntityFrameworkCore.SqlServer — SQL Server provider

Microsoft.EntityFrameworkCore.Design — enables migrations

Validation

FluentValidation — Fluent validation rules

FluentValidation.DependencyInjectionExtensions — DI support

Mapping

AutoMapper — object mapper

AutoMapper.Extensions.Microsoft.DependencyInjection — DI support

Authentication

Microsoft.AspNetCore.Authentication.JwtBearer — JWT token validation

Caching

Microsoft.Extensions.Caching.Memory — in-memory caching (Idempotency)

Logging

Serilog.AspNetCore — structured logging for ASP.NET

Serilog.Sinks.Console — console logs

Serilog.Sinks.File — rolling file logs

Swagger

Swashbuckle.AspNetCore — OpenAPI + Swagger UI

---

Rate Limiting

Microsoft.AspNetCore.RateLimiting — request throttling
