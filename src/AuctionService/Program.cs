
// Creating a web application
using AuctionService;
using AuctionService.Data;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add Controller provides the services that we need when creating a Web API Controller.
builder.Services.AddControllers();


builder.Services.AddDbContext<AuctionDbContext>(opt => {
	opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//? The Get Assemblies will specify where the mapping profiles are. 
//? This provides the assemly this application is running in
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Adding Masstransit to the service
builder.Services.AddMassTransit(x => {

	x.AddEntityFrameworkOutbox<AuctionDbContext>(o => {

		// Retry from outbox every 10 seconds
		o.QueryDelay = TimeSpan.FromSeconds(10);

		// We are letting the outbox know to use PostGres Database.
		o.UsePostgres();

		o.UseBusOutbox();

	});

	x.AddConsumersFromNamespaceContaining<AuctionCreatedFaultConsumer>();
	x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("auction", false));

	x.UsingRabbitMq((context, cfg) => {

		//? Check the documentation for the ConfigureEndpoints method
		cfg.ConfigureEndpoints(context);
	});
});

// Building out Application
var app = builder.Build();

// Configure the HTTP request pipeline.
//? This acts like a Middleware when a request comes into the server and when the response goes out of the server
//? Typically we would add Authentication and Authorisation here

app.UseAuthorization();

//? This middleware allows the framework to direct the http request to the correct API endpoint
app.MapControllers();

//? 
try
{
	DbInitializer.InitDb(app);
}
catch (Exception e)
{
	Console.WriteLine(e);
}

app.Run();


//! The code below were removed for tutorial purposes

// if (app.Environment.IsDevelopment())
// {

// }

// app.UseHttpsRedirection();