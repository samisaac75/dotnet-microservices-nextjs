
// Creating a web application
using AuctionService.Data;
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