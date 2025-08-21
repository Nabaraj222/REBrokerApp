README.txt

========================================
REBroker .NET 8.0 Project - Setup Guide
========================================

This solution is built using .NET 8.0 and Microsoft SQL Server 2022 (v20.2). It follows the Code-First approach with Entity Framework Core for database management. The main projects are REBroker.Web (MVC) and REBroker.API (Web API), both of which should be set as startup projects.

📌 Prerequisites
----------------
- .NET SDK 8.0
- Microsoft SQL Server 2022 (v20.2) or later
- Visual Studio 2022 (recommended) or Visual Studio Code with C# support
- Entity Framework Core Tools (install via: `dotnet tool install -g dotnet-ef`)

🚀 Setup & Run Instructions
----------------------------

1. **Restore Dependencies**
- Open terminal in the solution root and run:

2. **Update Database Connection**
- Open `appsettings.json` in the REBroker.API project (or shared configuration).
- Locate the `ConnectionStrings` section and update `DefaultConnection` to your SQL Server instance.

Example:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=REBrokerDB;User Id=sa;Password=YourStrongPassword;TrustServerCertificate=True;"
}
```

3. **Update Database**
- Open Package Manager Console and run: update-database
