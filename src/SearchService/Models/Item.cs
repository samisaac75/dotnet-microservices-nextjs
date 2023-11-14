using MongoDB.Entities;

namespace SearchService;

// Entity class provides an unique identifier for the Item.
// Therefroe we do not need to specify a unique id like we did for Auction DTO, where we specified GUID
public class Item : Entity
{
	public int ReservePrice { get; set; } 
	public string Seller { get; set; }
	public string Winner { get; set; }
	public int SoldAmount { get; set; }
	public int CurrentHighBid { get; set; }
	public DateTime CreatedAt { get; set; } 
	public DateTime UpdatedAt { get; set; } 
	public DateTime AuctionEnd { get; set; } 
	public string Status { get; set; }
	public string Make { get; set; }
    public string Model { get; set; }
	public int Year { get; set; }
	public string Color { get; set; }
	public int Mileage { get; set; }
	public string Image { get; set; }
}
