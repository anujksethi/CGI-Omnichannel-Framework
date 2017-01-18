namespace OmniChannel.VideoChatConnector.APIs.Rentals
{
	public class PostRental
	{
		public string Description { get; set; }
		public int NumberOfRooms { get; set; }
		public decimal Price { get; set; }
		public string Address { get; set; }
	}
}