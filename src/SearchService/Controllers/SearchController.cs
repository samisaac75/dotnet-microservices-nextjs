
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace SearchService;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Item>>> SeacrhItems([FromQuery] SearchParams searchParams) {

        //? 1. We create the query which is a page search
        // Page Search for the Item
        var query = DB.PagedSearch<Item, Item>();


        //? 2. If search term is not empty then we Match it
        // Check if search term is null or an empty string
        if(!string.IsNullOrEmpty(searchParams.SearchTerm)) {
            
            // Sorting the search term by test score
            query.Match(Search.Full, searchParams.SearchTerm).SortByTextScore();

        }
        
        //? 3. The query will be ordered based on their selection
        // The default search is set by adding an underscore at the end of the list
        query = searchParams.OrderBy switch
        {
            "make" => query.Sort(x => x.Ascending(a => a.Make)).Sort(x => x.Ascending(a => a.Model)),
            "new" => query.Sort(x => x.Descending(a => a.CreatedAt)),
            _ => query.Sort(x => x.Ascending(a => a.AuctionEnd))
        };
        
        //? 4. The query will be filtered based on their selection
        // The default filter by is defined by adding an underscore to the end of the list
        query = searchParams.FilterBy switch 
        {
          "finished" => query.Match(x => x.AuctionEnd < DateTime.UtcNow),
          "endingSoon" => query.Match(x => x.AuctionEnd < DateTime.UtcNow.AddHours(6) && x.AuctionEnd > DateTime.UtcNow),
          _ => query.Match(x => x.AuctionEnd > DateTime.UtcNow)         
        };
        
        //? 5. If there is a seller, it will be matched on the user that is selling
        // Filter the Seller
        if(!string.IsNullOrEmpty(searchParams.Seller)) {
            query.Match(x => x.Seller == searchParams.Seller);
        }
        
        //? 6. If there is a winner, it will match the winner.
        if(!string.IsNullOrEmpty(searchParams.Winner)) {
            query.Match(x => x.Winner == searchParams.Winner);
        }

        //! The get request fails if the page number and page size is not passed as a query string parameter
        //! Think about whether or not to add default values for page number and size if the values are missing.
        // Setting the page number for the query
        query.PageNumber(searchParams.PageNumber);

        // Setting the Page Size for the query
        query.PageSize(searchParams.PageSize);

        var result = await query.ExecuteAsync();

        // Return results, page count and total count
        return Ok(new {
            results = result.Results,
            pageCount = result.PageCount,
            totalCount = result.TotalCount
        });

    }

}
