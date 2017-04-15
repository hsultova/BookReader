using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BookReader.Data.Database;
using BookReader.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BookReader.Data.Repositories
{
	public class Repository<EntityType> : IRepository<EntityType>
		where EntityType : ModelBase, new()
	{
		private BookReaderDbContext DbContext;

		public Repository(BookReaderDbContext context)
		{
			DbContext = context;
		}

		protected DbSet<EntityType> EntitySet
		{
			get
			{
				return DbContext.Set<EntityType>();
			}
		}

		public IList<EntityType> LoadList()
		{
			return EntitySet.ToList();
		}

		public EntityType Load(int id, params Expression<Func<EntityType, object>>[] includeGraph)
		{
			IQueryable<EntityType> query = EntitySet;

			foreach (var includeGraphItem in includeGraph)
			{
				query = query.Include(includeGraphItem);
			}

			return query.First(x => x.Id == id);
		}

		public IList<EntityType> LoadList(Expression<Func<EntityType, bool>> predicate, params Expression<Func<EntityType, object>>[] includeGraph)
		{
			IQueryable<EntityType> query = EntitySet;

			foreach (var includeGraphItem in includeGraph)
			{
				query = query.Include(includeGraphItem);
			}

			if (predicate != null)
			{
				query = query.Where(predicate);
			}

			return query.ToList();
		}

		public void Save(EntityType entity)
		{
			EntitySet.Attach(entity);
			DbContext.SaveChanges();
		}

		public void Add(EntityType entity)
		{
			EntitySet.Add(entity);
			DbContext.SaveChanges();
		}

		public void Remove(EntityType entity)
		{
			EntitySet.Remove(entity);
			DbContext.SaveChanges();
		}
	}
}
