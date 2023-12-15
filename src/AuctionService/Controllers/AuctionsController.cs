using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entites;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers;

[ApiController]
[Route("api/auctions")]
public class AuctionsController : ControllerBase
{
    private readonly AuctionDbContext _context;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    //? We are injecting from MassTrisit to inject the message through IPublishEndpoint
    public AuctionsController(AuctionDbContext context, 
    IMapper mapper, 
    IPublishEndpoint publishEndpoint)

    {
        _context = context;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    [HttpGet]
    public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions(string date) {

        var query = _context.Auctions.OrderBy(x => x.Item.Make).AsQueryable();

        if(!string.IsNullOrEmpty(date)) {
            query = query.Where(x => x.UpdatedAt.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);
        }

        //? *** Explanation of the return type: ***
            //? Task is an function that does a operation asnchronously.
            //? The Task returns an Action Result. Example Status 200 or 403 etc.
            //? Action Result returns a List of ActionDto

        return await query.ProjectTo<AuctionDto>(_mapper.ConfigurationProvider).ToListAsync();

        //? The code below was changed to the above while working with synchronous http requets

        // var auctions = await _context.Auctions
        //     .Include(x => x.Item)
        //     .OrderBy(x => x.Item.Make)
        //     .ToListAsync();

        // return _mapper.Map<List<AuctionDto>>(auctions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id) {

        var auction = await _context.Auctions
            .Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id == id);

        if(auction == null) return NotFound();  

        return _mapper.Map<AuctionDto>(auction); 

    }

    [HttpPost]
    public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto auctionDto) {

        // Mapping Auction DTO to Entity
        var auction = _mapper.Map<Auction>(auctionDto);

        //Getting the current user
        //TODO: Add current user as seller
        auction.Seller = "test";

        //* ***** LINE 1, 2 and 3 will be treated like a transaction. Either they all work or none of them work.

        //? LINE 1
        // Adding the mapped entity for auction to the database
        _context.Auctions.Add(auction);

        //! Line 1 and LINE 2 were move above LINE 3 after we added the code for the Outbox for RabbitMQ

        //? LINE 2 - Where do use the IPublishEnpoint? Do we do it after we save chnages?
        //* We have to wait until after we have the ID from te created auction.
        var newAuction = _mapper.Map<AuctionDto>(auction);

        //? LINE 3 - Using the IPublish Endpoint we can publish this new auction.
        //* We are mapping the new auction into an AuctionCreated object.
        //* We user _mapper.Map and then we pass n the new auction.
        await _publishEndpoint.Publish(_mapper.Map<AuctionCreated>(newAuction));

        // Save changes is a required step
        //? LINE 4 - if a zero is returned, that means nothing was saved in the database.
        var result = await _context.SaveChangesAsync() > 0;


        // If the data could not be saved, return a custom message.
        if(!result) return BadRequest("Could not save chnages to the database!");

        // If data saving was successful, then return the name of he created auction. We are using en existing endpoint to get the details of the newly created auction
        return CreatedAtAction(nameof(GetAuctionById), new {auction.Id}, newAuction);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAuction (Guid id, UpdateAuctionDto updateAuctionDto) {

        var auction = await _context.Auctions.Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id == id);

        if(auction == null) return NotFound();

        //TODO: Check Seller name is same as the username

        auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
        auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
        auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
        auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;
        auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;

        var result = await _context.SaveChangesAsync() > 0;

        if(result) return Ok();

        return BadRequest("Could not save changes and update.");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAuction(Guid id) {

        var auction = await _context.Auctions.FindAsync(id);

        if(auction == null) return NotFound();

        //TODO: Check seller is same as username

        _context.Auctions.Remove(auction);

        var result = await _context.SaveChangesAsync() > 0;

        if(!result) return BadRequest("Could not update database"); 

        return Ok();

    }

}