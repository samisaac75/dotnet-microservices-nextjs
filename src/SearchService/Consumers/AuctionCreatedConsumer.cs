using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;

namespace SearchService;

public class AuctionCreatedConsumer : IConsumer<AuctionCreated>
{
    private readonly IMapper _mapper;

    public AuctionCreatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<AuctionCreated> context)
    {
        Console.WriteLine("---> Consuming Auction Created. " + context.Message.Id);
    
        var item = _mapper.Map<Item>(context.Message);

        //? Excercise - We are not accepting any cars named "Foo"
        //* We are going to throw an exception if we receive a car that has the model name of Foo

        if(item.Model == "Foo") throw new ArgumentException("Cannot sell cars with the name of foo");
        //* We can look at the faulty 

        await item.SaveAsync();
    }
}