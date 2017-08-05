using BookReader.Data.Database;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BookReader.Web.Helpers
{
	public class TransactionFilterAttribute : ActionFilterAttribute
	{
		private BookReaderDbContext _context;

		public TransactionFilterAttribute(BookReaderDbContext context)
		{
			_context = context;
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			_context.Database.BeginTransaction();
		}

		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			if (_context.Database.CurrentTransaction != null)
			{
				if (filterContext.Exception == null)
				{
					_context.Database.CommitTransaction();
				}
				else
				{
					_context.Database.RollbackTransaction();
				}
			}
		}

	}
}
