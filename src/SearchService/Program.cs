using System.Net;
using Contracts;
using MassTransit;
using MongoDB.Driver;
using MongoDB.Entities;
using Polly;
using Polly.Extensions.Http;
using SearchService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

//? Adding AutoMapper to the services
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//? Creating an HTTP Client to add the Auction Service.
builder.Services.AddHttpClient<AuctionSvcHttpClient>().AddPolicyHandler(GetPolicy());

//? 

// Adding Masstransit to the service
builder.Services.AddMassTransit(x => {

    x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();

    //? If AuctionCreatedConsumer is being used in another service, there will be a cash. hence we are adding the configurationbelow:

    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search", false));

	x.UsingRabbitMq((context, cfg) => {

        //? e is the endpoint formatter. 
        cfg.ReceiveEndpoint("search-auction-created", e => {

            //? First attribute (5) is the number of retries
            //? Second attribute (5) is the time between each retries
            e.UseMessageRetry(r => r.Interval(5,5));

            //? We have to add the consumner that we are configuring this for
            e.ConfigureConsumer<AuctionCreatedConsumer>(context);

        });

		//? Check the documentation for the ConfigureEndpoints method
		cfg.ConfigureEndpoints(context);
	});
});

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Lifetime.ApplicationStarted.Register(async () => {
    try
    {
        await DbInitializer.InitDb(app);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
});


app.Run();

//? This static method which creates a policy and we can handle the response based on what happens
// This policy retries every 3 seconds.

static IAsyncPolicy<HttpResponseMessage> GetPolicy()
    => HttpPolicyExtensions
        .HandleTransientHttpError()
        .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
        .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(3));