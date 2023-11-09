# Program File Configurations

1. Add a DB Context using the builder
2. Setup the Connection String to the DB

In development mode, we can add out connection string to appSetting.Development.json

**Example for postgres Conection String:**

- This goes below the "Logging" Key

  "ConnectionStrings": {
  "DefaultConnection": "Server=localhost:5432;User Id=postgres;Password=878_Pembroke;Database=auctions"
  }

**We need the following tool. To check if it already exists, run**

> dotnet tool list -g

**If it is no installed, run the following comment in the terminal**

> dotnet tool install dotnet-ef -g

To invode this tool use **dotnet-ef**

This tool will create a migration after looking at our code and creates a database schema

**To create the migrarion, run the following command:**

> donet ef migrations add "InitialCreate" -o Data/Migrations

**To update the database, run**

> dotnet ef database update

**To Drop the database, run the following command**

> dotnet ef databse drop

