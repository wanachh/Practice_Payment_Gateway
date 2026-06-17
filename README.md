# ZenPay: Mock Payment Gateway

ZenPay is a mock practice project designed to help understand the core mechanics of a Payment Gateway and the structure of professional, enterprise-level .NET applications.

This project was built interactively with the help of AI (Gemini) to guide the development process, explain complex code, and establish industry-standard best practices.

---

## 🎯 Purpose and Concepts

The primary goal of this practice is **learning through implementation**. By building a simplified version of a system that handles financial transactions, we explore critical software engineering concepts:

### Key Architectural Concepts Learned:
1. **Separation of Concerns (Layered Architecture)**: Splitting the application into logical layers so that the Web API doesn't talk directly to the database.
2. **Data Transfer Objects (DTOs)**: Using dedicated classes (`CreateTransactionRequest`, `TransactionResponse`) to control exactly what data is sent by the user and returned by the API, protecting the internal database schema.
3. **The Service Layer**: Moving business logic (like simulating bank approvals and generating transaction numbers) out of the Controllers and into a reusable `TransactionService`.
4. **Dependency Injection**: Injecting services (like `ITransactionService` and `ZenPayDbContext`) into controllers securely and efficiently.
5. **Entity Framework Core (EF Core)**: Using Code-First migrations to generate and interact with an SQLite database.
6. **API Documentation**: Using modern OpenAPI tools (Scalar) to visualize and test endpoints.

---

## 🏗️ Project Structure

The solution is divided into two main projects to enforce the Separation of Concerns:

### 1. `ZenPay.Core` (The Engine Room)
This library contains all the business logic, data models, and rules. It has no idea that the internet or HTTP requests exist.
- **`Models/`**: The exact representation of the database tables (e.g., `Transaction.cs`).
- **`Data/`**: The `ZenPayDbContext` which acts as the bridge to the SQLite database.
- **`DTOs/`**: Objects used to safely transfer data between the user and the API.
- **`Services/`**: The core business logic implementations (`TransactionService.cs`).
- **`ENUM/`**: Enums for transaction statuses (Pending, Captured, Failed, etc.).

### 2. `ZenPay.Api` (The Entry Point)
This is the Web API project. Its only job is to receive HTTP requests, hand them off to `ZenPay.Core`, and return HTTP responses.
- **`Controllers/`**: `TransactionController.cs` maps URLs to specific C# methods.
- **`Program.cs`**: The startup file that configures Dependency Injection, Database connections, and the OpenAPI (Scalar) documentation UI.

---

## 🚀 How to Run

### Prerequisites
- .NET 10 SDK (or compatible version)
- `dotnet-ef` CLI tools (for database migrations)

### 1. Setup the Database
Because this project uses SQLite, the database file (`ZenPay.db`) is generated locally. To create the tables, apply the Entity Framework migrations by running this command in the root folder:

```bash
dotnet ef database update --project ZenPay.Core --startup-project ZenPay.Api
```

### 2. Run the API
Start the API using the HTTP profile to avoid local certificate warnings:

```bash
dotnet run --project ZenPay.Api --launch-profile "http"
```

### 3. Test the Endpoints
Once the application is running, open your web browser and navigate to:
👉 **`http://localhost:5214/scalar`**

This will open a beautiful, interactive API dashboard where you can test the following endpoints:

* **`POST /Transaction`**: Create a new transaction (Requires `Amount` and `Currency`). It will be created in a `Pending` state.
* **`GET /Transaction`**: Retrieve a list of all transactions in the database.
* **`GET /Transaction/{id}`**: Retrieve the details of a specific transaction.
* **`POST /Transaction/{id}/process`**: Simulates sending the transaction to a bank. It has an 80% chance to succeed (`Captured`) and a 20% chance to fail (`Failed` with `ERR_INSUFFICIENT_FUNDS`).