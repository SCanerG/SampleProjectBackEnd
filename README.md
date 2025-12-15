# SampleProjectBackEnd

SampleProjectBackEnd is a robust .NET Web API project built using **Clean Architecture (Onion Architecture)** principles. It provides a solid foundation for scalable enterprise applications with built-in support for Identity, JWT Authentication, and Repository Pattern.

## 🚀 Features

- **Clean Architecture**: Separation of concerns into Domain, Application, Infrastructure, and API layers.
- **Authentication & Authorization**:
  - Integrated **ASP.NET Core Identity**.
  - **JWT (JSON Web Token)** based authentication.
  - **Refresh Token** mechanism with database storage and rotation checks.
- **Advanced Data Access**:
  - **Entity Framework Core** with Code-First approach.
  - **Repository & Unit of Work** patterns.
- **DTOs & Mapping**: AutoMapper integration for clean separation between Entities and API Models.
- **Validation**: FluentValidation support (implied in structure).
- **Environment Management**: flexible `appsettings.json` configuration for Development/Production.

## 🛠 Technology Stack

- **.NET 8** (or relevant version based on `csproj`)
- **ASP.NET Core Web API**
- **Entity Framework Core** (SQL Server)
- **AutoMapper**
- **S.O.L.I.D Principles**

## 📂 Project Structure

The solution consists of the following projects:

- **SampleProjectBackEnd.Domain**: Contains enterprise-wide logic and types (Entities, Enums). No dependencies.
- **SampleProjectBackEnd.Application**: Contains business logic, interfaces (Service/Repository contracts), DTOs, and Mappers. Depends on Domain.
- **SampleProjectBackEnd.Infrastructure**: Implements the interfaces defined in Application (EF Core, Identity, Sending Emails, etc.). Depends on Application.
- **SampleProjectBackEnd.Api**: The entry point of the application (Controllers, Middleware, Configuration). Depends on Application and Infrastructure.

## ⚙️ Getting Started

### Prerequisites
- .NET SDK installed.
- SQL Server (LocalDB or full instance).

### Installation

1.  **Clone the repository**
    ```bash
    git clone https://github.com/yourusername/SampleProjectBackEnd.git
    cd SampleProjectBackEnd
    ```

2.  **Configure Database**
    Update the `ConnectionStrings` in `SampleProjectBackEnd.Api/appsettings.json`:
    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=(localdb)\\ScgLocalDB;Database=SampleProject;Trusted_Connection=True;"
    }
    ```

3.  **Run Migrations**
    Open a terminal in the solution folder and run:
    ```bash
    dotnet ef database update --project SampleProjectBackEnd.Infrastructure --startup-project SampleProjectBackEnd.Api
    ```

4.  **Run the Project**
    ```bash
    dotnet run --project SampleProjectBackEnd.Api
    ```

## 🔐 Auth Endpoints

The API includes endpoints for comprehensive token management:
- `POST /api/auth/login`: Authenticate and receive Access + Refresh Tokens.
- `POST /api/auth/register`: Create a new user.
- `POST /api/auth/refresh-token`: Renew access token using a valid Refresh Token.
- `POST /api/auth/revoke`: Revoke a Refresh Token.

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.
