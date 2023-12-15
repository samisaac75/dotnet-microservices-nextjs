# Asynchronous Messaging

Using RabbitMQ, we are using an event bus for asynchronous messaging.

What if the event bus goes down? Have we introduced a single point failure by using this model?

The answer is Yes and No.

Yes, it will fail but even if it fails, the inidividual servies can respond to requests that comes externally to that service. They simply cannot communicate with each other.

That is to say that the Service Bus is not important. It should be treated as a first-calss citizen in our application.

- It should be given high availability.
- It should be clustered.
- It should be full tolerant.
- it should have persistant storage
- Should it fail, and a new one is replaced in its place, it can grab any messages that are persisted and recover from that failure.

### What is RabbitMQ?

- It accepts and forwards messages - Its accepts a messgae nto an exchange and forwards a message into a queue.
- It uses the Producer and Consumer model (Pub/Sub - Publish/Subscribe).
- We will be using Mass Transits to connect to RabbitMQ.
- Messages are stored in queue (message buffer).
- Can use persistant storage.
- It can be used for routing functionality.
- It uses AMQP (Advanced Message Queing Protocol).

**There are 4 exchanges:**

- Direct.
- Fanout (We are going to focus on this one).
- Topic.
- Header.

**Direct**

In case of a direct exchnage, it delivers message based on a routing key. Its like unicast messaging. It is only going to route to a single queue.

**Fanout**

For example, the AuctionService event publishes am auction created event to an exchange. This exchnage has one ore more queues bound to that exchange which will place the messages in a queue and wait for a consumer to pick up and consume that message.

We can have multiple queues consuming from the same queue or multiple queues as well.

**Topic**

Topic is same as Fanout but it can have a routing key going to more than one queue.

**Header**

Header type of exchange allows us to specifiy a header that the exchange can use and publish it various queues.

## Notes

We are using a Mass Transit to abstract the RabbitMQ package. A lot of things are going to happen in RabbitMQ by convention.

When we create a class (we are going to these classes as contracts), we are going to see the contracts appear under the Exchange tab.
