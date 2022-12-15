using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace _7semester_ASP_FirstTask.Models
{
	public class Slave
	{
		//public Slave(int price, int age, string skinTone)
		//{
		//	this.Price = price;
		//	this.Age = age;
		//	this.SkinTone = skinTone;
		//}

		//public Slave() { }

		public int Id { get; set; }
		public int? Price { get; set; }
		public int? Age { get; set; }
		public string? SkinTone { get; set; }
		public int? MasterId { get; set; }
		public Master? Master { get; set; }
	}
}
