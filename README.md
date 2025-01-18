# Spartan Fitness

- [Spartan Fitness](#spartan-fitness)
  - [1. API](#1-api)
    - [Libraries](#libraries)
    - [Concepts and principles](#concepts-and-principles)
  - [2. ClientApp](#2-clientapp)
  - [3. DevOps](#3-devops)
  - [4. Configuration](#4-configuration)
    - [API](#api)
    - [ClientApp](#clientapp)
    - [Docker compose](#docker-compose)

## 1. API

The API was created with `.NET 7` in combination with `ASP.NET Core`, `Entity Framework Core 7` and `XUnit`. `MS SQL Server` is used for it's relational database. `JWT` and `JWT Bearer` are used for authentication and (role-based) authorization.

### Libraries

- ErrorOr (Error handling)
- MediatR (CQRS)
- FluentValidation (Application layer validation)
- Mapster (Object mapping)
- EntityFrameworkCore (ORM)
- Swagger (OpenAPI documentation)

### Concepts and principles

- Domain Driven Design (DDD)
- Clean Architecture
- Command and Query Responsibility Segregation (CQRS)

## 2. ClientApp

The client app was created with `React 18`, `Typescript`, `SWS` and `Vite`.

## 3. DevOps

Version Control System: `Git`<br/>
Source Code Mangement: `GitHub`<br/>
CI/CD: `GitHub Actions`<br/>
Deployment: `Docker` and `Kubernetes`<br/>
Scripting: `Bash` and `Powershell Core 7`<br/>

## 4. Configuration

### API

The `appsettings.json`, `appsettings.Development.json` or `appsettings.Production.json` file in `~/src/SpartanFitness.Api` should be populated with the following data.

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "JwtSettings": {
    "Secret": "<SUPER-SECRET-KEY>",
    "ExpiryMinutes": 60,
    "RefreshTokenExpiryMonths": 6,
    "Issuer": "SpartanFitness",
    "Audience": "SpartanFitness"
  },
  "ConnectionStrings": {
    "SpartanFitness": "Server=<SERVER>;Database=<DATABASE>;User Id=<UID>;Password=<PWD>;TrustServerCertificate=true;"
  },
  "EmailSettings": {
    "Secret": "<SUPER-SECRET-KEY>",
    "MailGunApiKey": "<MAIL-GUN-API-KEY>",
    "MailGunDomain": "<MAIL-GUN-DOMAIN>"
  },
  "FrontendSettings": {
    "ApplicationUrl": "<APPLICATION_URL>"
  },
  "PasswordResetSettings": {
    "Secret": "<SUPER-SECRET-KEY>",
    "ExpiryMinutes": 60
  },
  "CoachCreationSettings" : {
    "Secret": "<SUPER-SECRET-KEY>"
  }
}
```

### ClientApp

The `~/src/SpartanFitness.ClientApp/.env` file should contain the following data.

```env
VITE_API_URL=<API-URL> // Example: http://localhost:8001
VITE_API_BASE=<API-BASE> // Example: http://localhost:8001/api/v1
```

### Docker compose

When using docker compose the `~/.db.env` file should also be specified.

```env 
ACCEPT_EULA=Y
<UID>_PASSWORD=<PWD>
```
