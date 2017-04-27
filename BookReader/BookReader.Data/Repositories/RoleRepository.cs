using BookReader.Data.Database;
using BookReader.Data.Models;
using BookReader.Data.Repositories.Abstract;

namespace BookReader.Data.Repositories
{
	public class RoleRepository : Repository<Role>, IRoleRepository
	{
		public RoleRepository(BookReaderDbContext context)
			: base(context)
		{
		}
	}
}
