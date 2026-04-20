# EmployeeDatabase — ASP.NET Core Web API

A complete Employee Management REST API built with **ASP.NET Core**, **Entity Framework Core**, and **SQL Server** — end-to-end CRUD, DTO-based input validation, EF Core migrations, and interactive Swagger documentation.

This repo accompanies a step-by-step video tutorial. Whether you clone it to learn from the code or follow along with the video, the goal is the same: a working, production-style Web API you can build, run, and extend.

📺 **Watch the full tutorial on YouTube:** [https://youtu.be/VIYj55lvGtA](https://youtu.be/VIYj55lvGtA)

---

![.NET](https://img.shields.io/badge/.NET-10-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-Web_API-5C2D91?style=flat-square)
![Entity Framework](https://img.shields.io/badge/EF_Core-10.0.6-68217A?style=flat-square)
![SQL Server](https://img.shields.io/badge/SQL_Server-Express-CC2927?style=flat-square&logo=microsoftsqlserver&logoColor=white)
![Swagger](https://img.shields.io/badge/Swagger-OpenAPI-85EA2D?style=flat-square&logo=swagger&logoColor=black)
![License](https://img.shields.io/badge/License-MIT-green?style=flat-square)

---

## Table of contents

- [What this project does](#what-this-project-does)
- [Tech stack](#tech-stack)
- [Project structure](#project-structure)
- [Prerequisites](#prerequisites)
- [Getting started](#getting-started)
- [Configuration](#configuration)
- [Database migrations](#database-migrations)
- [API endpoints](#api-endpoints)
- [Testing the API](#testing-the-api)
- [Architecture overview](#architecture-overview)
- [Known issues & next improvements](#known-issues--next-improvements)
- [Troubleshooting](#troubleshooting)
- [Learning resources](#learning-resources)
- [License](#license)

---

## What this project does

This is a RESTful Web API for managing employee records. It exposes six HTTP endpoints for full CRUD operations, persists data to a SQL Server database via Entity Framework Core (code-first migrations), and ships with interactive Swagger UI for in-browser testing.

**Core features:**

- Full CRUD: Create, Read, Update, Delete employees
- Two flavors of update: full replace (standard `PUT`) and partial update (`PUT /v2/{id}`)
- Guid-based primary keys with automatic generation
- DTOs for input, entities for persistence — clean separation of concerns
- Dependency injection for `DbContext`
- Interactive Swagger UI with XML documentation comments
- Code-first EF Core migrations

---

## Tech stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core Web API (.NET 10) |
| ORM | Entity Framework Core 10 |
| Database | SQL Server Express |
| API documentation | Swagger / OpenAPI (Swashbuckle) |
| Testing | Swagger UI, Postman |
| IDE | Visual Studio 2026 |

### NuGet packages

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="10.0.6" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="10.0.6" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="10.0.6" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="10.0.6" />
<PackageReference Include="Swashbuckle.AspNetCore.SwaggerGen" Version="10.1.7" />
<PackageReference Include="Swashbuckle.AspNetCore.SwaggerUI" Version="10.1.7" />
```

**What each package does:**

- **EntityFrameworkCore** — Core ORM. Provides `DbContext`, `DbSet`, and LINQ-to-SQL translation.
- **EntityFrameworkCore.SqlServer** — SQL Server provider. Enables `options.UseSqlServer(...)` in `Program.cs`.
- **EntityFrameworkCore.Design** — Design-time package. Powers scaffolding and migrations.
- **EntityFrameworkCore.Tools** — Package Manager Console cmdlets (`Add-Migration`, `Update-Database`).
- **Swashbuckle.AspNetCore.SwaggerGen** — Generates the OpenAPI spec from your controllers.
- **Swashbuckle.AspNetCore.SwaggerUI** — Renders the interactive `/swagger` HTML UI.

---

## Project structure

```
EmployeeDatabase/
├── Controllers/
│   ├── EmployeesController.cs        # All Employee CRUD endpoints
│   └── WeatherForecastController.cs  # Default template (safe to delete)
├── Data/
│   └── ApplicationDbContext.cs       # EF Core DbContext with DbSet<EmployeeDetails>
├── Migrations/
│   ├── 20260418014835_InitialCreate.cs
│   ├── 20260418014835_InitialCreate.Designer.cs
│   └── ApplicationDbContextModelSnapshot.cs
├── Models/
│   ├── AddEmployeeDto.cs             # Input DTO for POST
│   ├── UpdateEmployeeDto.cs          # Input DTO for PUT
│   └── Entities/
│       └── EmployeeDetails.cs        # Employee entity mapped to DB table
├── appsettings.json                  # Connection string & logging config
├── Program.cs                        # DI, middleware, startup
└── EmployeeDatabase.csproj
```

---

## Prerequisites

Make sure you have the following installed before running the project:

- **.NET 10 SDK** — [Download](https://dotnet.microsoft.com/download)
- **SQL Server Express** — [Download](https://www.microsoft.com/sql-server/sql-server-downloads) (free)
- **SQL Server Management Studio (SSMS)** — [Download](https://learn.microsoft.com/sql/ssms/download-sql-server-management-studio-ssms) (optional but recommended)
- **EF Core CLI tools:**
  ```bash
  dotnet tool install --global dotnet-ef
  ```
- **Visual Studio 2026** (Community Edition is free) or **VS Code with the C# extension**
- **Postman** (optional, for testing) — [Download](https://www.postman.com/downloads/)

---

## Getting started

### 1. Clone the repository

```bash
git clone https://github.com/gitpackage/dotnet.git
cd EmployeeDatabase
```

### 2. Restore packages

```bash
dotnet restore
```

### 3. Configure the connection string

Open `appsettings.json` and update the `DefaultConnection` to point to your local SQL Server instance (see the [Configuration](#configuration) section below).

### 4. Apply the database migration

```bash
dotnet ef database update
```

This creates the `EmployeesData` database and the `EmployeeDetails` table.

### 5. Run the application

```bash
dotnet run
```

The app will start and print the URLs it's listening on. Open a browser to:

```
https://localhost:<port>/swagger
```

You should see the interactive Swagger UI listing the Employee and WeatherForecast endpoints.

---

## Configuration

The connection string lives in `appsettings.json` under `ConnectionStrings:DefaultConnection`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "server=YOUR_SERVER\\SQLEXPRESS;Database=EmployeesData;Trusted_connection=True;TrustServerCertificate=true;MultipleActiveResultSets=true"
  }
}
```

**What each parameter means:**

| Parameter | Purpose |
|---|---|
| `server` | SQL Server instance name (replace `YOUR_SERVER` with your machine name) |
| `Database` | Database name — EF Core creates this on first migration |
| `Trusted_connection=True` | Uses Windows authentication (your current Windows user) |
| `TrustServerCertificate=true` | Skips TLS certificate validation (fine for local dev; do not use in production) |
| `MultipleActiveResultSets=true` | Allows multiple open result sets on a single connection |

> ⚠️ **Security note:** Never commit a production connection string with credentials to a public repo. For production, use environment variables, Azure Key Vault, or `dotnet user-secrets`.

---

## Database migrations

This project uses EF Core's code-first migrations.

### Apply all pending migrations

```bash
dotnet ef database update
```

### Add a new migration (after changing the entity model)

```bash
dotnet ef migrations add YourMigrationName
dotnet ef database update
```

### Roll back the last migration

```bash
dotnet ef database update PreviousMigrationName
```

### Drop the database (start fresh)

```bash
dotnet ef database drop --force
dotnet ef database update
```

---

## API endpoints

Base URL: `https://localhost:<port>/api/employees`

| Method | Route | Description | Request body | Response |
|---|---|---|---|---|
| `GET` | `/api/employees` | List all employees | — | `200 OK` + `EmployeeDetails[]` |
| `GET` | `/api/employees/{id}` | Get one employee by Guid | — | `200 OK` or `404 Not Found` |
| `POST` | `/api/employees` | Create a new employee | `AddEmployeeDto` | `201 Created` + entity |
| `PUT` | `/api/employees/{id}` | Full replace — overwrites all fields | `UpdateEmployeeDto` | `200 OK` or `404 Not Found` |
| `PUT` | `/api/employees/v2/{id}` | Partial update — only provided fields are changed | `UpdateEmployeeDto` | `200 OK` or `404 Not Found` |
| `DELETE` | `/api/employees/delete/{id}` | Delete an employee | — | `200 OK` or `404 Not Found` |

### Example request/response

**POST /api/employees**

```json
{
  "fName": "Jane",
  "lName": "Doe",
  "email": "jane.doe@example.com"
}
```

**Response — 201 Created**

```json
{
  "id": "3f8a9c12-4b5e-4d7a-8f2c-1b3d5e6f7a89",
  "firstName": "Jane",
  "lastName": "Doe",
  "email": "jane.doe@example.com"
}
```

---

## Testing the API

### Option 1: Swagger UI (easiest)

1. Run the app with `dotnet run`
2. Open `https://localhost:<port>/swagger` in your browser
3. Expand any endpoint, click **Try it out**, fill in the fields, and click **Execute**

### Option 2: Postman

1. Create a new Postman request
2. Set the URL (e.g., `https://localhost:<port>/api/employees`)
3. Set the HTTP method and request body
4. Send the request
5. If you get an SSL error, disable SSL certificate verification in Postman settings for localhost

### Option 3: cURL

```bash
# GET all employees
curl -k https://localhost:<port>/api/employees

# POST a new employee
curl -k -X POST https://localhost:<port>/api/employees \
  -H "Content-Type: application/json" \
  -d '{"fName":"Jane","lName":"Doe","email":"jane@example.com"}'
```

The `-k` flag skips certificate validation for the local dev certificate.

---

## Architecture overview

Request flow for `POST /api/employees`:

```
┌─────────────────────┐     HTTP POST      ┌──────────────────────────┐
│  Client             │ ─────────────────▶ │  EmployeesController     │
│  (Swagger/Postman)  │                    │  [HttpPost]              │
└─────────────────────┘                    └────────────┬─────────────┘
                                                        │
                                                        ▼
                                           ┌──────────────────────────┐
                                           │  AddEmployeeDto          │
                                           │  (input DTO)             │
                                           └────────────┬─────────────┘
                                                        │  map to entity
                                                        ▼
                                           ┌──────────────────────────┐
                                           │  EmployeeDetails         │
                                           │  (entity)                │
                                           └────────────┬─────────────┘
                                                        │
                                                        ▼
                                           ┌──────────────────────────┐
                                           │  ApplicationDbContext    │
                                           │  dbContext.Add()         │
                                           │  dbContext.SaveChanges() │
                                           └────────────┬─────────────┘
                                                        │  INSERT
                                                        ▼
                                           ┌──────────────────────────┐
                                           │  SQL Server              │
                                           │  EmployeeDetails table   │
                                           └──────────────────────────┘
```

---

## Known issues & next improvements

### Known issue: `required` modifier on `UpdateEmployeeDto`

The `v2/{id}` PUT endpoint is designed for partial updates, but `FName` and `LName` are marked `required` on `UpdateEmployeeDto`. This means callers still have to send both fields — the partial-update behavior doesn't fully work as intended.

**Fix:** make `FName` and `LName` nullable (`string?`) on `UpdateEmployeeDto`, or create a separate `PatchEmployeeDto` with all nullable properties.

### Ideas for next improvements

- [ ] Convert all database calls to async (`ToListAsync`, `FindAsync`, `SaveChangesAsync`)
- [ ] Return `CreatedAtAction` from the POST endpoint to set a proper `Location` header
- [ ] Add input validation with `FluentValidation` or data annotations (email format, name length)
- [ ] Add pagination to the `GET` all endpoint (`Skip`, `Take`)
- [ ] Add logging via `ILogger<EmployeesController>`
- [ ] Add global exception handling middleware with `ProblemDetails` responses
- [ ] Add unit tests with xUnit and an in-memory database provider
- [ ] Add authentication with JWT or ASP.NET Core Identity
- [ ] Remove the default `WeatherForecastController` and `WeatherForecast.cs` template files

---

## Troubleshooting

### "A network-related or instance-specific error occurred while establishing a connection to SQL Server"

- Confirm SQL Server Express is running (check **Services** → `SQL Server (SQLEXPRESS)`)
- Verify the server name in your connection string matches your machine name
- Try connecting in SSMS first to confirm the instance is reachable

### "Unable to create an object of type 'ApplicationDbContext'"

- Make sure `dotnet-ef` is installed: `dotnet tool install --global dotnet-ef`
- Run EF commands from the project root (the folder with `.csproj`)
- Confirm `Microsoft.EntityFrameworkCore.Design` is referenced in the `.csproj`

### "The SSL connection could not be established"

- Trust the local dev certificate: `dotnet dev-certs https --trust`
- In Postman, disable SSL certificate verification under **Settings → General**

### Swagger UI doesn't show endpoint descriptions

- In `EmployeeDatabase.csproj`, enable XML documentation generation:
  ```xml
  <PropertyGroup>
    <GenerateDocumentationFile>true</GenerateDocumentationFile>
    <NoWarn>$(NoWarn);1591</NoWarn>
  </PropertyGroup>
  ```

---

## Learning resources

- [ASP.NET Core documentation](https://learn.microsoft.com/aspnet/core/)
- [Entity Framework Core documentation](https://learn.microsoft.com/ef/core/)
- [EF Core migrations guide](https://learn.microsoft.com/ef/core/managing-schemas/migrations/)
- [REST API best practices (Microsoft)](https://learn.microsoft.com/azure/architecture/best-practices/api-design)
- [Swashbuckle / Swagger docs](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)

---

## License

This project is licensed under the MIT License — see the [LICENSE](LICENSE) file for details.

You're free to use, modify, and distribute this code for learning or your own projects.

---

## Author

Built by [@gitpackage](https://github.com/gitpackage)

If this tutorial helped you, please ⭐ the repo and subscribe on YouTube — it's the best way to support more content like this.

📺 **Watch the tutorial:** [https://youtu.be/VIYj55lvGtA](https://youtu.be/VIYj55lvGtA)
