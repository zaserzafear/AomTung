# Updating Database Schema with Entity Framework Core

## Introduction

This guide provides step-by-step instructions on how to update the database schema in your .NET Core application using Entity Framework Core and the Pomelo.EntityFrameworkCore.MySql provider.

## Prerequisites

- [.NET Core SDK](https://dotnet.microsoft.com/download) installed.
- MySQL database server installed and running.
- Entity Framework Core tools installed (`dotnet tool install --global dotnet-ef`).

## Update Database Schema

To update the database schema after changing any property, follow these steps:

1. Open a terminal or command prompt.

2. Navigate to your project directory containing the `AppDbContext` and other relevant files.

3. Run the following command to scaffold the updated database context and models:

   ```bash
   dotnet ef dbcontext scaffold "Server=localhost;User=admin;Password=admin;Port=3306;Database=demo" --context AppDbContext Pomelo.EntityFrameworkCore.MySql --context-dir Data --output-dir DataModels --data-annotations --use-database-names --force --no-onconfiguring
   ```