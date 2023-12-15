

using AuctionService.Entites;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Data;

public class AuctionDbContext : DbContext
{
    public AuctionDbContext(DbContextOptions options) : base(options)
    {


        
    }

    public DbSet<Auction> Auctions { get; set; }

    //? We need to override the OnModelCreating using the AuctionDBContext
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {   
        //? This is not mandatory but we are just covering all bases.
        base.OnModelCreating(modelBuilder);

        //TODO: Need to create new migrations because these will create tables in the database.
        //* Migrations created
        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }

}
