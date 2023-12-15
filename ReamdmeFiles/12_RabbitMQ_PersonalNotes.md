# RabbitMQ Personal Notes

After creating hooking up AuctionCreated contract to SearchService, here is my understanding:

**We have two exchanges**

1. Auction Created
2. search-auction-created

How MassTransit approches RabbitMQ is that it has exchnages that send to an exchange which are then sent to a queue.

Contracts:AuctionCreated exchange pushes the event to search-auction-created that is binded to the SeacrhService.

The search-auction-created exchange a queue which is also called search-auction-created

Now we have a queue ready to receive messages

Next we take a look at publish a message to that queue.

These exchanges as fanouts
