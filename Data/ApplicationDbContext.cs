using Microsoft.EntityFrameworkCore;
using EmployeeDatabase.Models.Entities;
namespace EmployeeDatabase.Data
{
	public class ApplicationDbContext : DbContext
	{

		public ApplicationDbContext(DbContextOptions options) : base(options)
		{

		}

		public DbSet<EmployeeDetails> EmployeeDetails { get; set; }
	}
}
