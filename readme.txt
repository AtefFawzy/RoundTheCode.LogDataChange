You will need the following installed:

- ASP.NET Core SDK 3.1.5+ (https://dotnet.microsoft.com/download/dotnet-core/3.1)
- Visual Studio 2019 Professional (Upgrade to latest version)
- SQL Server 2017+
- Postman (download at postman.com)

Databases available in the "databases" folder. Restore them using SQL Server Management Studio.

- Games (The "data" database)
- Games-Change (The "change" database)

You will need to update appsettings.json in "/Web/RoundTheCode.LogDataChange.Web.Api" so the connection strings point to your database.

Open up RoundTheCode.LogDataChange.sln at root level which will open up the project in Visual Studio 2019. Run the application which will run the API at https://localhost:2001.

Use Postman to test the methods. You can import the Postman collection by going into the "Postman" folder.