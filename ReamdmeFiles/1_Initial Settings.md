# Micro Services using .NET and Next.js

**-- Check the SDK Files:**

> dotnet --info

**-- Create new template:**

> dotnet new list

**-- Create new Solution file:**

> dotnet new sln

**-- Create new Web API with an output directory:**

> dotnet new webapi -o src/AuctionService

**-- Add solution file to the src directory:**

> dotnet sln add src/AuctionService/

---

# H3 Launch Settings

**-- Auction Service -> Properies:**

Profiles:

1. http
2. https
3. IIS Express

We are removing iis settings

Remove lauch url for http
Turn of browser lauching for http

Remove https and IIS EXPRESS profiles

# H3 App Setting

**-- Auction Service -> appsetting.json:**

In the development file, change the log level -> Microsoft.AspNetCore from warnign to Information

# H3 Running the Application

**Can be done a couple of ways:**

1. Using the terminal

- Navigate to the src directory
- Navigate to AuctionService
- Then run: **dotnet watch**
