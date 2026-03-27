namespace BusinessLogic.Entities
{
	public class Client
	{
		public Guid Id { get; set; }
		public string UserId { get; set; } = string.Empty;
		public User User { get; set; } = null!;
		public List<Reserve> Reservations { get; set; } = new();
	}
}