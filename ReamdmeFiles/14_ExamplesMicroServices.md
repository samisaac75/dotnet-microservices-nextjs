# How Microservcies Work?

**Auction Service and Search Service**

- A POST request is made to the Auction Service

- The Acution Service adds the data to PostGreSQL

- The Auction Service will add the message to the Serive Bus.

- The Search Service is going to consume the message from the service Bus

- The Search Service then updates the MongoDB database.

## DATA CONSISTENCY

**The 5 Different Items in these two services are**

1. Auction Service
2. PostGres
3. Service Bus (RabbitMQ)
4. Search Service
5. MongoDB

What happens if one of these goes down, will the data be consistent?

### Scenario 1 (No Inconsistensies? ✅)

1. Auction Service ❌
2. PostGres
3. Service Bus (RabbitMQ)
4. Search Service
5. MongoDB

If the Auction Service is down, the client cannot create an auction, it cannot update PostGres, You cannot search for data that doesn't exist and you cannot update MongoDB.

### Scenario 2 (No Inconsistensies ? ✅)

1. Auction Service
2. PostGres ❌
3. Service Bus (RabbitMQ)
4. Search Service
5. MongoDB

If our PostGres database is down, it is going to throw an exception and the request will stop exceution at that point and the auction is never created.

### Scenario 3 (No Inconsistensies? ✅)

1. Auction Service
2. PostGres
3. Service Bus (RabbitMQ)
4. Search Service ❌
5. MongoDB

We will have consistent data because, the auction will be created, but the search service comes back up, it will read all the messages that has been queued in the service bus and it will consume it.

### Scenario 4 (No Inconsistensies? 🛑)

1. Auction Service
2. PostGres
3. Service Bus (RabbitMQ)
4. Search Service
5. MongoDB ❌

We are going to have data inconsistency. The data cannot be saved to MongoDB. It is not going to retry.

### Scenario 5 (No Inconsistensies? 🛑)

1. Auction Service
2. PostGres
3. Service Bus (RabbitMQ) ❌
4. Search Service
5. MongoDB

No we do not have data consistency. The messages cannot be queued in the Bus once PostGres is updated.

### What if there is data inconsistency? - How to think about fixing it?

What if the Email exchnage is down and you are sinding an email? Where does the email go? It goes to the outbox and once the exchange is back up the email is then sent out from the outbox.

This is the strategy that we have in RabbitMQ.

Creating Migrations in Auction service for the outbox, run the following command in the terminal:

> dotnet ef migrations add outbox

Check Auction Service -> Migrations -> Outbox -> Here you will see an outbox migration. There is code here to create tables that stores the data to be sent to the Service Bus once it comes back online.

