# Dot Net Data Access Tour

A tour of different data access approaches in .NET 5+.

## Data Access Options (CRUD + Queries)

- ADO.NET Custom SQL
- ADO.NET Stored Procs
- Dapper Custom SQL
- Dapper Stored Procs
- EF Core + Custom Queries

## Additional Options

- Repository layer
- Application layer

## Running the App - Create the Database

EF and its Migrations are not required for all of these variants, but it does provide a quick and easy way to get your data in place if you want to run these samples. Just run this command from the `WebDataDemo` folder:

```powershell
dotnet ef database update
```
