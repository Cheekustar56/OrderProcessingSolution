Order Processing Solution - Small demo app (Web + Background service)
------------------------------------------------------------------

What's included:
- OrderWeb (ASP.NET Core MVC) : web UI to create and list orders.
- OrderProcessor (Worker Service) : background worker that processes orders marked 'Ready'.
- Shared OrderDbContext and Order model.
- appsettings.json in projects uses LocalDB by default:
     Server=(localdb)\\MSSQLLocalDB;Database=OrderDb;Trusted_Connection=True;

How to run locally:
1. Extract the folder and open OrderProcessingSolution.sln in Visual Studio 2022+ or use `dotnet` CLI.
2. From solution folder, run: dotnet restore
3. To run the web app:
     dotnet run --project OrderWeb
   Browse to http://localhost:5000 (or https://localhost:5001)
4. To run the worker:
     dotnet run --project OrderProcessor
   Worker will poll DB every 30 seconds and process orders with Status='Ready'.

Database:
- The apps call db.Database.EnsureCreated() so the DB/table will be created automatically on startup.
- If you prefer, run the SQL script at sql/InitialCreate.sql against your SQL Server to create the Orders table.

Deploying to AWS:
- Update appsettings.json connection strings in both projects to point to your AWS SQL Server/RDS.
- Publish the projects and deploy:
  - Web app -> IIS (copy published files to C:\inetpub\wwwroot\OrderWeb)
  - Worker -> register as Windows Service (NSSM recommended) using the published executable.

Next steps (CI/CD):
- We'll create a Jenkins pipeline that builds on Jenkins master and deploys to an appserver agent (IIS + NSSM service).
