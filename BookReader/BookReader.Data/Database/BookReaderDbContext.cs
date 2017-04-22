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
		public DbSet<Author> Authors { get; set; }
		public DbSet<Book> Books { get; set; }
		public DbSet<UserBook> UserBooks { get; set; }
		public DbSet<Genre> Genres { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>();
			modelBuilder.Entity<Role>();
			modelBuilder.Entity<Author>();
			modelBuilder.Entity<Book>();
			modelBuilder.Entity<UserBook>();
			modelBuilder.Entity<Genre>();
		}
	}
}
