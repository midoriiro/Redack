using System;
using System.Linq;
using System.Linq.Expressions;
using Redack.DatabaseLayer.DataAccess;
using System.Net.Http;
using System.Reflection;
using System.Collections.Generic;

namespace Redack.ServiceLayer.Models.Request.Uri
{
	public class ExpressionParameter : QueryParameter<Expression<Func<dynamic, dynamic>>>
	{
		public string Expression { get; private set; }

		public ExpressionParameter(QueryBuilder builder, HttpRequestMessage request) : base(builder, request)
		{
		}

		public ExpressionParameter(Expression<Func<dynamic, dynamic>> value) : base(value)
		{
		}

		public override IQueryable Execute(string key, IQueryable queryable)
		{
			if (this.Builder.RegisteredExpressionMethods.ContainsKey(key))
			{
				var methodName = this.Builder.RegisteredExpressionMethods[key];
				var method = typeof(QueryableExtensions).GetMethod(methodName);

				if (method == null)
					throw new MissingMethodException($"{methodName} does not exists in QueryableExtensions class");

				return (IQueryable)method.Invoke(queryable, new object[] { queryable, this.Expression });
			}

			throw new KeyNotFoundException($"Expression method {key} is not allowed");
		}

		public override void FromQuery(string value)
		{
			this.Expression = value;

			if (this.Builder.ExcludeExpressionKeywords.Any(this.Expression.Contains))
				throw new UnauthorizedAccessException();
		}

		public override string ToQuery()
		{
			string body = this.Value.Body.ToString();

			body = body.Replace(this.Value.Parameters[0].Name + ".", "");

			return body;
		}		
	}
}