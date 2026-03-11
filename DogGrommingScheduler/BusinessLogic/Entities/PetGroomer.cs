namespace BusinessLogic.Entities
{
	public class PetGroomer
	{
		public Guid Id { get; set; }
		public string UserId { get; set; } = string.Empty;
		public User User { get; set; } = null!;
		public List<Schedule> Schedules { get; set; } = new();
	}
}