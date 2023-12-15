using Contracts;
using MassTransit;

namespace AuctionService;

//* We have to specify the message type passed to the IConsumer
//* In this case it is a 'Fault'
//* Within Fault, we have to pass the contract. In this case, it is AuctionCreated

//! Becuase this is a new consumer within Auction Service, we need to tell Mass Transit about this consumer (update program.cs file inside the Auction Service)

public class AuctionCreatedFaultConsumer : IConsumer<Fault<AuctionCreated>>
{
    //? The context passed to the Consume method wil contain the fault message.
    //? 
    public async Task Consume(ConsumeContext<Fault<AuctionCreated>> context)
    {
        Console.WriteLine("Consuming faulty creation ");

        var exception = context.Message.Exceptions.First();

        // The following code will only run if the exception type matched "System.ArgumentException"
        if(exception.ExceptionType == "System.ArgumentException") {

            //? We are goign to take the message inside context
            context.Message.Message.Model = "FooBar";


            //? Since we are inside the contex of a Consumer, we have access to the publish method.
            await context.Publish(context.Message.Message);
            
        } else {
                Console.WriteLine("Not an argument exception.");
        }

    }
}
