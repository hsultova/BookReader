using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using BookReader.Data.Models;

namespace BookReader.Data.Repositories.Abstract
{
	public interface IRepository<EntityType>
		where EntityType : ModelBase, new()
	{
		IList<EntityType> LoadList();
		EntityType Load(int id, params Expression<Func<EntityType, object>>[] includeGraph);
		EntityType Load(int id, Expression<Func<EntityType, bool>> predicate, params string[] includeGraph);
		IList<EntityType> LoadList(Expression<Func<EntityType, bool>> predicate, params Expression<Func<EntityType, object>>[] includeGraph);
		void Add(EntityType entity);
		void Save(EntityType entity);
		void Remove(EntityType entity);
	}
}
