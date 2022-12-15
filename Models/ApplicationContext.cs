using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace _7semester_ASP_FirstTask.Models
{
	public class ApplicationContext : DbContext
	{
		public DbSet<User> Users { get; set; } = null!;
		public DbSet<Master> Masters { get; set; } = null!;
		public DbSet<Slave> Slaves { get; set; } = null!;
		public ApplicationContext(DbContextOptions<ApplicationContext> options)
			: base(options)
		{
			//Database.EnsureDeleted();
			Database.EnsureCreated();

			if (!Users.Any())
			{
				Users.Add(new User("a", "a", "a", "Ukraine"));
				Users.Add(new User("b", "b", "b", "Spain"));
			}

			if (!Masters.Any())
			{
				Master window = new Master { Name = "Window" };
				Master oracle = new Master { Name = "Oracle" };

				Slave slave1 = new Slave { SkinTone = "Black", Master = oracle, Age = 18, Price = 100 };
				Slave slave2 = new Slave { SkinTone = "WhiterThanBlack", Master = oracle, Age = 14, Price = 100 };
				Slave slave3 = new Slave { SkinTone = "TotallyBlack", Master = null, Age = 16, Price = 100 };

				Masters.AddRange(oracle, window);
				Slaves.AddRange(slave1, slave2, slave3);
				Slave slave4 = new Slave { SkinTone = "Black", Master = oracle, Age = 18, Price = 100 };
				Slave slave5 = new Slave { SkinTone = "WhiterThanBlack", Master = oracle, Age = 14, Price = 100 };
				Slave slave6 = new Slave { SkinTone = "TotallyBlack", Master = null, Age = 16, Price = 100 };
				Slaves.AddRange(slave4, slave5, slave6);
				SaveChanges();
			}
		}
	}
}
