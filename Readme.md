# BuyMyHouse

**BuyMyHouse** is a real estate management system built with ASP.NET Core Web API and Azure Functions, using Azure services for storage (Blob, Queue, Table) and SQL Server for relational data.

---

## Table of Contents

- [Project Structure](#project-structure)  
- [Features](#features)  
- [Prerequisites](#prerequisites)  
- [Setup & Local Development](#setup--local-development)  
- [Run Azure Functions](#azure-functions)  
- [Testing](#testing)

---

## Project Structure

BuyMyHouse/

├─ BuyMyHouse.sln

├─ src/

    ├─ BuyMyHouse.Api/ # ASP.NET Core Web API (Controllers, Startup)

    ├─ BuyMyHouse.Domain/ # Entities, Interfaces, Domain Services

    ├─ BuyMyHouse.Infrastructure/ # Database Context, Repositories, Azure Storage 

    ├─ BuyMyHouse.AzureFunctions/ # Serverless functions

├─ tests/

    ├─ BuyMyHouse.Tests/ # Unit and integration tests

├─ docker/

    ├─ docker-compose.yml


---

## Features

- CRUD operations for mortgage applications and users.  
- Azure Blob Storage for storing mortgage documents.  
- Azure Queue Storage for notifications.  
- Azure Table Storage for income records.  
- EF Core with SQL Server for relational data.  
- RESTful API with Swagger documentation.  
- Unit testing with xUnit.

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)  
- [Visual Studio 2022 / VS Code](https://visualstudio.microsoft.com/)  
- [Docker Desktop](https://www.docker.com/) (optional, for local SQL Server)  
- Azure Subscription (Student Subscription works fine)/ Azurite

---

## Setup & Local Development

1. **Clone the repository**

```bash
git clone https://github.com/Anthony-Shoshi/buy-my-house-api/tree/main/src/BuyMyHouse.Api
```

2. **Change directory**
```
cd BuyMyHouse
```

3. **Go to docker folder to run sql server locally**

```bash
cd docker
```

4. **Run this to start sql server**
```bash
docker-compose up -d
```

5. **Update appsettings.Development.json**
```bash 
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=BuyMyHouseDB;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True",
    "BlobStorage": "UseDevelopmentStorage=true",
    "QueueStorage": "UseDevelopmentStorage=true",
    "TableStorage": "UseDevelopmentStorage=true"
  }
}
```

6. **Apply EF Core migrations**
```bash
cd src/BuyMyHouse.Infrastructure

dotnet ef database update --startup-project ../BuyMyHouse.Api
```

7. **Run the Web API**
```bash
cd ../BuyMyHouse.Api

dotnet run
```

This will seed some data and Swagger will be available at: https://localhost:5001/swagger

## Azure Functions

**Run all Azure Functions:**
```bash
cd src/BuyMyHouse.AzureFunctions
dotnet func start
```

## Testing

**Run all tests with:**
```bash
cd tests/BuyMyHouse.Tests

dotnet test
```


