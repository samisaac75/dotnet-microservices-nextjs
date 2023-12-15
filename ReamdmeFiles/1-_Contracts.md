# Contracts

1. First we setup a new classlib for Contracts

> dotnet new classlib -o src/Contracts

2. Added Contracts to solution

> dotnet sln add src/Contracts

3. Add reference for Contracts to AuctionService

> cd src
> cd AuctionService
> dotnet add reference ../../src/Contracts

4. Add Contracts to Search Service

> cd src
> cd SearchService
> 