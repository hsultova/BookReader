using BookReader.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BookReader.Data.Database
{
	public class BookReaderDbContext : DbContext
	{
		public BookReaderDbContext(DbContextOptions<BookReaderDbContext> options)
			: base(options)
		{
		}

		public DbSet<User> Users { get; set; }
		public DbSet<Role> Roles { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>();
			modelBuilder.Entity<Role>();
		}
	}
}
