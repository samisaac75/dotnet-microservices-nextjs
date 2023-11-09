using System.ComponentModel.DataAnnotations.Schema;
using AuctionService.Entites;

namespace AuctionService;

[Table("Items")]
public class Item
{
	public Guid Id { get; set; }
	public string Model { get; set; }
	public string Make { get; set; }
	public int Year { get; set; }
	public string Color { get; set; }
	public int Mileage { get; set; }
	public string Image { get; set; }

//* Nav Properties
	//? The following poperties is for establishing on-to-one relationship between Item Entity and Auction Entity
	public Auction Auction { get; set; }
	public Guid AuctionId { get; set; }

}
