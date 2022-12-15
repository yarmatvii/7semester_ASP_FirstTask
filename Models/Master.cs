namespace _7semester_ASP_FirstTask.Models
{
	public class Master
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public List<Slave> Slaves { get; set; } = new();
	}
}
