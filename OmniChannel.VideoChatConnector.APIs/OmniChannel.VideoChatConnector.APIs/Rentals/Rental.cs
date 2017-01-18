using System.Linq;

namespace OmniChannel.VideoChatConnector.APIs.Rentals
{
	using System.Collections.Generic;
	using MongoDB.Bson;
	using MongoDB.Bson.Serialization.Attributes;

	public class Rental
	{
	    public Rental()
	    {
	            
	    }
	    public Rental(PostRental postRental)
	    {
	        Description = postRental.Description;
	        NumberOfRooms = postRental.NumberOfRooms;
	        Price = postRental.Price;
	        Address = (postRental.Address ?? string.Empty).Split('\n').ToList();
	    }
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		public string Description { get; set; }
		public int NumberOfRooms { get; set; }
		public List<string> Address = new List<string>();
		
		[BsonRepresentation(BsonType.Double)]
		public decimal Price { get; set; }

        public List<PriceAdjustment> Adjustments= new List<PriceAdjustment>();

	    public void AdjustPrice(AdjustPrice adjustPrice)
	    {
            var priceAdjustment = new PriceAdjustment(adjustPrice,Price);
            Adjustments.Add(priceAdjustment);
	        Price = adjustPrice.NewPrice;
	    }
	}
}