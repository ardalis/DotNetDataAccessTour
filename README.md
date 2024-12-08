# Dot Net Data Access Tour

A tour of different data access approaches in .NET 9+.

**Note:** This is meant to demonstrate different ways to make data access calls, not best practices (or even reasonably good practices) for building ASP.NET Core Web APIs.

## Related Videos

- [Custom Specifications with EF Core](https://www.youtube.com/watch?v=i5FvDLsSrn0) @ardalis, 2024
- [Improving Data Access with Abstractions](https://www.youtube.com/watch?v=g6cjCbxq54Y) Stir Trek, 2023

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

Run this command to get ef core tools:

```powershell
dotnet tool install --global dotnet-ef
```

```powershell
dotnet ef database update
```

## Running Seq in a local Docker instance

Run this command:

```powershell
docker run --name seqlogger -d -p 5341:5341 -p 5342:80 -e ACCEPT_EULA=Y datalust/seq
```

Verify Seq is running: http://localhost:5342

## Running k6 scripts

Install k6:

```powershell
choco install k6
```

Run scripts from the k6 folder.

```powershell
k6 run data-strategy-01.js
```
