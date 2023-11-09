using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entites;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers;

[ApiController]
[Route("api/auctions")]
public class AuctionsController : ControllerBase
{
    private readonly AuctionDbContext _context;
    private readonly IMapper _mapper;

    public AuctionsController(AuctionDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions() {

        //? *** Explanation of the return type: ***
            //? Task is an function that does a operation asnchronously.
            //? The Task returns an Action Result. Example Status 200 or 403 etc.
            //? Action Result returns a List of ActionDto

        var auctions = await _context.Auctions
            .Include(x => x.Item)
            .OrderBy(x => x.Item.Make)
            .ToListAsync();

        return _mapper.Map<List<AuctionDto>>(auctions);
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

        // Adding the mapped entity for auction to the database
        _context.Auctions.Add(auction);

        // Save changes i required step
        //? if a zero is returned, that means nothing was saved in the database.
        var result = await _context.SaveChangesAsync() > 0;

        // If the data could not be saved, return a custom message.
        if(!result) return BadRequest("Count not save chnages to the database!");

        // if data saving was successful, then rtuen the name of he created auction
        return CreatedAtAction(nameof(GetAuctionById), new {auction.Id}, _mapper.Map<AuctionDto>(auctionDto));
    }
}
