# 🐾 Dog Grooming Scheduler — Agenda Canina

Sistema web para gestionar turnos de peluquería canina. Permite a los clientes registrarse, ver la disponibilidad de los peluqueros y reservar turnos. Los peluqueros pueden gestionar sus horarios y los administradores tienen control total del sistema.

---

## 📋 Tabla de contenidos

- [Stack tecnológico](#stack-tecnológico)
- [Arquitectura](#arquitectura)
- [Estructura del proyecto](#estructura-del-proyecto)
- [Entidades del dominio](#entidades-del-dominio)
- [API — Endpoints](#api--endpoints)
- [Configuración local](#configuración-local)
- [Despliegue en producción](#despliegue-en-producción)
- [CI/CD con GitHub Actions](#cicd-con-github-actions)
- [Variables de entorno](#variables-de-entorno)
- [Roles del sistema](#roles-del-sistema)

---

## Stack tecnológico

| Capa | Tecnología |
|---|---|
| Frontend | Blazor WebAssembly (.NET 10) |
| Backend | ASP.NET Core 10 Web API |
| Base de datos | PostgreSQL 18 |
| ORM | Entity Framework Core 9 + Npgsql |
| Autenticación | ASP.NET Core Identity + JWT |
| Jobs en background | Hangfire + PostgreSQL |
| Email | Resend API |
| Documentación API | Swagger / OpenAPI |
| Contenedores | Docker (multi-stage build) |
| Plataforma de despliegue | Coolify v4 sobre AWS EC2 t3.small |
| CI/CD | GitHub Actions |

---

## Arquitectura

```
┌─────────────────────────────────────────────────────────┐
│                     Cliente (Browser)                    │
│              Blazor WASM — nginx en Coolify              │
│         http://HASH.IP.sslip.io (puerto 80)             │
└──────────────────────────┬──────────────────────────────┘
                           │ HTTP (CORS habilitado)
┌──────────────────────────▼──────────────────────────────┐
│                  ASP.NET Core Web API                    │
│              Coolify — Docker en EC2                     │
│         http://HASH.IP.sslip.io (puerto 8080)           │
│                                                         │
│  AuthController  ReserveController  PetGroomerController│
│         ↓               ↓                  ↓            │
│     AuthService   ReserveService    PetGroomerService   │
│         ↓               ↓                  ↓            │
│              Entity Framework Core (Npgsql)              │
│                    Hangfire (jobs)                       │
└──────────────────────────┬──────────────────────────────┘
                           │
┌──────────────────────────▼──────────────────────────────┐
│                   PostgreSQL 18                          │
│            Coolify — Docker en EC2 (interno)            │
│              Host: container-name:5432                  │
└─────────────────────────────────────────────────────────┘
```

### Flujo CI/CD

```
Push a main (BlazorClient/**)
        ↓
GitHub Actions compila Blazor WASM
(en servidores de GitHub, gratis)
        ↓
Sube archivos compilados a rama: main-front
        ↓
Llama webhook de Coolify
        ↓
Coolify redespliega nginx con nuevos archivos
(sin buildear .NET en el servidor)

Push a main (backend/**)
        ↓
Coolify detecta el push vía GitHub App
        ↓
Coolify buildea Dockerfile y redespliega la API
```

---

## Estructura del proyecto

```
dog-grooming-scheduler/
├── Dockerfile                          # Build multi-stage del backend
├── Dockerfile.frontend                 # Nginx estático para el frontend
├── .github/
│   └── workflows/
│       └── deploy-frontend.yml         # CI/CD del Blazor WASM
└── DogGrommingScheduler/
    ├── Directory.Packages.props        # Versiones de paquetes NuGet centralizadas
    ├── DogGrommingScheduler.slnx       # Solución
    │
    ├── BlazorClient/                   # Frontend — Blazor WebAssembly
    │   ├── Pages/
    │   │   ├── Register.razor          # Registro de usuario
    │   │   ├── Login.razor             # Login
    │   │   ├── Reserve.razor           # Crear turno
    │   │   └── MyReserves.razor        # Mis turnos
    │   ├── Services/
    │   │   ├── PetGroomerService.cs    # Llamadas HTTP a PetGroomer
    │   │   └── ReserveService.cs       # Llamadas HTTP a Reserve
    │   ├── Handlers/Auth/
    │   │   ├── AuthService.cs          # Login/Register HTTP
    │   │   └── CustomAuthStateProvider.cs  # JWT en localStorage
    │   └── wwwroot/
    │       └── appsettings.json        # URL de la API
    │
    ├── DogGrommingScheduler/           # Backend — ASP.NET Core Web API
    │   ├── Controllers/
    │   │   ├── AuthController.cs       # POST /api/Auth/register, login
    │   │   ├── ReserveController.cs    # POST/PUT/GET /api/Reserve
    │   │   ├── PetGroomerController.cs # GET /api/PetGroomer
    │   │   └── TestEmailController.cs  # POST /api/TestEmail/prueba-confirmacion
    │   └── Program.cs                  # Configuración de la app
    │
    ├── AplicationLogic/                # Capa de aplicación (Use Cases)
    │   ├── Services/
    │   │   ├── Authentication/AuthService.cs
    │   │   ├── Email/ResendEmailService.cs
    │   │   ├── PetGroomer/PetGroomerService.cs
    │   │   └── Scheduler/
    │   │       ├── ReserveService.cs
    │   │       └── BackgroundJobService.cs
    │   └── Interfaces/
    │
    ├── BusinessLogic/                  # Dominio (Entidades + Interfaces de repositorios)
    │   ├── Entities/
    │   │   ├── User.cs
    │   │   ├── Client.cs
    │   │   ├── PetGroomer.cs
    │   │   ├── Reserve.cs
    │   │   └── Schedule.cs
    │   └── RepositoriesInterfaces/
    │
    ├── DataAccess/                     # Capa de acceso a datos (EF Core)
    │   └── Repositories/
    │       ├── ContextDB.cs
    │       ├── ClientRepositoryEF.cs
    │       ├── PetGroomerRepositoryEF.cs
    │       ├── ReserveRepositoryEF.cs
    │       └── ScheduleRepositoryEF.cs
    │
    ├── Shared/                         # DTOs compartidos entre frontend y backend
    │   └── DTOs/
    │       ├── RegisterRequestDto.cs
    │       ├── LoginRequestDto.cs
    │       ├── AuthResponseDto.cs
    │       ├── ReserveDto/
    │       └── PetGroomerDtos/
    │
    └── AplicationLogic.Tests/          # Tests unitarios (xUnit + Moq)
```

---

## Entidades del dominio

### User
Extiende `IdentityUser` de ASP.NET Core Identity.
```
Id (string, heredado)
Name (string)
Email (string, heredado)
CreatedAt (DateTime)
```

### Client
Perfil de cliente vinculado a un User.
```
Id (Guid)
UserId → User
Reservations → List<Reserve>
```

### PetGroomer
Perfil de peluquero vinculado a un User.
```
Id (Guid)
UserId → User
Schedules → List<Schedule>
```

### Schedule
Horario de trabajo de un peluquero en una fecha específica.
```
Id (Guid)
PetGroomerId → PetGroomer
Date (DateTime)
StartTime (DateTime)
EndTime (DateTime)
Reservations → List<Reserve>
```

### Reserve
Turno reservado por un cliente.
```
Id (Guid)
ReservationDate (DateTime)
TimeSlot (TimeSpan)       — hora exacta del turno (ej: 10:30)
ScheduleId → Schedule
ClientId → Client
PetSize (Enum: Small | Medium | Large)
IsCanceled (bool)
ReminderJobId (string)    — ID del job de Hangfire para recordatorio
```

---

## API — Endpoints

### Autenticación
| Método | Ruta | Descripción | Auth |
|---|---|---|---|
| POST | `/api/Auth/register` | Registrar nuevo usuario | No |
| POST | `/api/Auth/login` | Login, devuelve JWT | No |

### Turnos (Reserves)
| Método | Ruta | Descripción | Auth |
|---|---|---|---|
| POST | `/api/Reserve` | Crear un turno | No* |
| PUT | `/api/Reserve/cancel/{id}` | Cancelar un turno | No* |
| GET | `/api/Reserve/schedule/{groomerId}/{date}` | Ver disponibilidad de un peluquero en una fecha | No |
| GET | `/api/Reserve/my-reserves/{userId}` | Ver mis turnos | No* |

### Peluqueros
| Método | Ruta | Descripción | Auth |
|---|---|---|---|
| GET | `/api/PetGroomer` | Listar todos los peluqueros | No |

### Email (Testing)
| Método | Ruta | Descripción |
|---|---|---|
| POST | `/api/TestEmail/prueba-confirmacion?emailDestino=xxx` | Enviar email de prueba |

> *La autorización por roles está modelada pero no todos los endpoints tienen `[Authorize]` implementado todavía.

La documentación completa de los endpoints está disponible en `/swagger` cuando la API está corriendo.

---

## Configuración local

### Requisitos previos
- .NET 10 SDK
- PostgreSQL (local o Docker)
- Node.js 22 (para las herramientas de Blazor)

### 1. Clonar el repositorio
```bash
git clone https://github.com/MergeCraft/dog-grooming-scheduler.git
cd dog-grooming-scheduler
```

### 2. Configurar la base de datos local
```bash
# Con Docker (recomendado)
docker run -d \
  --name doggrooming-db \
  -e POSTGRES_PASSWORD=postgres \
  -e POSTGRES_DB=doggrooming \
  -p 5432:5432 \
  postgres:18-alpine
```

### 3. Configurar el appsettings.Development.json del backend
Crear o editar `DogGrommingScheduler/DogGrommingScheduler/appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=doggrooming;Username=postgres;Password=postgres"
  },
  "Jwt": {
    "Key": "una-clave-secreta-de-desarrollo-minimo-32-chars",
    "Issuer": "DogGroomingAPI",
    "Audience": "DogGroomingClients"
  },
  "Resend": {
    "ApiKey": "re_tu_api_key_de_resend"
  },
  "BlazorClient": {
    "AllowedOrigins": ["https://localhost:7034"]
  }
}
```

### 4. Correr el backend
```bash
cd DogGrommingScheduler/DogGrommingScheduler
dotnet run
# API disponible en https://localhost:5001
# Swagger en https://localhost:5001/swagger
```

### 5. Configurar el frontend
Editar `DogGrommingScheduler/BlazorClient/wwwroot/appsettings.json`:
```json
{
  "Api": {
    "BaseUrl": "https://localhost:5001/"
  }
}
```

### 6. Correr el frontend
```bash
cd DogGrommingScheduler/BlazorClient
dotnet run
# Disponible en https://localhost:7034
```

---

## Despliegue en producción

El sistema corre en **AWS EC2 t3.small** con **Coolify v4** como plataforma de despliegue.

### Infraestructura
| Componente | Detalle |
|---|---|
| Servidor | AWS EC2 t3.small (2GB RAM), Ubuntu 24.04, us-east-1 |
| Plataforma | Coolify v4 en http://IP:8000 |
| Costo estimado | ~$16.60/mes (EC2 + EBS) |

### URLs de producción
| Componente | URL |
|---|---|
| Frontend | http://lh80djb34p89iips5f125sem.100.31.44.207.sslip.io |
| API | http://ehetwcjqfemblayixtef3rgm.100.31.44.207.sslip.io |
| Swagger | http://ehetwcjqfemblayixtef3rgm.100.31.44.207.sslip.io/swagger |
| Coolify | http://100.31.44.207:8000 |

### Notas importantes de despliegue
- El frontend **no se compila en el servidor** — se compila en GitHub Actions y se sube como archivos estáticos a la rama `main-front`.
- La base de datos corre como contenedor Docker dentro de Coolify en la red interna.
- El connection string usa el **nombre del contenedor** como host (no localhost ni IP).
- El **puerto expuesto** de la API en Coolify debe ser `8080` — si queda en `3000` el proxy devuelve Bad Gateway.

Para la guía completa de despliegue desde cero ver [guia-despliegue-coolify-aws.pdf](./guia-despliegue-coolify-aws.pdf).

---

## CI/CD con GitHub Actions

### Deploy del frontend (`.github/workflows/deploy-frontend.yml`)
Se dispara automáticamente al hacer push a `main` tocando archivos en `BlazorClient/**` o `Shared/**`.

**Pasos:**
1. Checkout del código desde `main`
2. Compilar el Blazor WASM con `dotnet publish`
3. Generar un `Dockerfile` mínimo de nginx en la carpeta de publicación
4. Pushear los archivos compilados a la rama `main-front`
5. Llamar al webhook de Coolify para redesplegar el frontend

**Secret requerido:** `COOLIFY_TOKEN` — generarlo en Coolify → Keys & Tokens → API Tokens (permiso: deploy).

### Deploy del backend
Coolify detecta automáticamente los pushes a `main` a través de la GitHub App instalada en la organización MergeCraft y redespliega el backend buildeando el `Dockerfile`.

---

## Variables de entorno

### Backend (configurar en Coolify → Environment Variables)

| Variable | Descripción | Ejemplo |
|---|---|---|
| `ConnectionStrings__DefaultConnection` | Connection string de PostgreSQL | `Host=container;Port=5432;Database=doggrooming;Username=postgres;Password=xxx` |
| `Jwt__Key` | Clave secreta para JWT (mínimo 32 caracteres) | `mi-clave-super-secreta-2026-xxx` |
| `Jwt__Issuer` | Identificador del emisor del token | `DogGroomingAPI` |
| `Jwt__Audience` | Audiencia del token | `DogGroomingClients` |
| `Resend__ApiKey` | API key de Resend.com para emails | `re_xxxxxxxxxx` |
| `BlazorClient__AllowedOrigins__0` | URL del frontend (para CORS) | `http://HASH.IP.sslip.io` |
| `ASPNETCORE_ENVIRONMENT` | Entorno de ejecución | `Production` |

### Frontend
El frontend solo necesita un archivo `wwwroot/appsettings.json` con la URL de la API. Este archivo se compila dentro del WASM, por lo que cualquier cambio requiere recompilar corriendo el workflow de GitHub Actions.

---

## Roles del sistema

El sistema define 3 roles creados automáticamente al iniciar la aplicación:

| Rol | Descripción |
|---|---|
| `Client` | Usuario que puede hacer y cancelar sus propios turnos |
| `Groomer` | Peluquero que puede gestionar sus horarios |
| `Admin` | Administrador con acceso completo |

Los roles se crean automáticamente en el startup si no existen.

---

## Equipo

**MergeCraft** — Proyecto académico, Universidad ORT Uruguay, 2026.
