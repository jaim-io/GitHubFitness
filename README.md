# Spartan Fitness

- [Spartan Fitness](#spartan-fitness)
  - [1. API](#1-api)
    - [Libraries](#libraries)
    - [Concepts and principles](#concepts-and-principles)
  - [2. ClientApp](#2-clientapp)
  - [3. DevOps](#3-devops)
 
## 1. API
The API was created with `.NET 6` in combination with `ASP.NET Core`, `Entity Framework Core 6` and `XUnit`. `MS SQL Server` is used for it's relational database. `JWT` and `JWT Bearer` are used for authentication and (role-based) authorization.

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
Version Control System: `Git`
Source Code Mangement: `GitHub`
CI/CD: `GitHub Actions`
Deployment: `Docker` and `Kubernetes`
Scripting: `Bash` and `Powershell Core 7`