# BuyMyHouse

**BuyMyHouse** is a real estate management system built with ASP.NET Core Web API and Azure Functions, using Azure services for storage (Blob, Queue, Table) and SQL Server for relational data.

---

## Table of Contents

- [Project Structure](#project-structure)  
- [Features](#features)  
- [Prerequisites](#prerequisites)  
- [Setup & Local Development](#setup--local-development)  
- [Azure Configuration](#azure-configuration)  
- [Deployment](#deployment)  
- [Testing](#testing)  
- [Environment Variables / App Settings](#environment-variables--app-settings)  
- [License](#license)

---

## Project Structure

BuyMyHouse/

├─ BuyMyHouse.sln

├─ src/

    │ ├─ BuyMyHouse.Api/ # ASP.NET Core Web API (Controllers, Startup)

    │ ├─ BuyMyHouse.Domain/ # Entities, Interfaces, Domain Services

    │ ├─ BuyMyHouse.Infrastructure/ # Database Context, Repositories, Azure Storage 

    │ ├─ BuyMyHouse.AzureFunctions/ # Serverless functions

├─ tests/

    │ ├─ BuyMyHouse.Tests/ # Unit and integration tests

├─ docker/

    │ ├─ docker-compose.yml


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
- [Docker](https://www.docker.com/) (optional, for local SQL Server)  
- Azure Subscription (Student Subscription works fine)

---

## Setup & Local Development

1. **Clone the repository**

```bash
git clone https://github.com/Anthony-Shoshi/buy-my-house-api/tree/main/src/BuyMyHouse.Api

cd BuyMyHouse/docker/


```bash
docker-compose up -d


