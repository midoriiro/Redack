using Redack.DomainLayer.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Redack.DatabaseLayer.DataAccess
{
	public static class QueryableExtensions
	{
		public static IQueryable Paginate(
			this IQueryable source,
			int index,
			int size)
		{
			if (index < 1)
				throw new ArgumentOutOfRangeException(
					nameof(index),
					$"index have to be equal or greater than 1");

			return source.Skip((index - 1) * size).Take(size);
		}

		public static IQueryable Query(this IQueryable source, string predicate) 
		{		
			return source.Where(predicate);
		}

		public static IQueryable Inclose(this IQueryable source, string path)
		{
			return source.Include(path);
		}

		public static IQueryable Reshape(this IQueryable source, string selector)
		{
			return source.Select(selector);
		}

		public static IQueryable Order(this IQueryable source, string ordering)
		{
			return source.OrderBy(ordering);
		}
	}
}
