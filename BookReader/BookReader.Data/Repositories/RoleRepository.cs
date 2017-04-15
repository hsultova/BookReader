using BookReader.Data.Database;
using BookReader.Data.Models;

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
