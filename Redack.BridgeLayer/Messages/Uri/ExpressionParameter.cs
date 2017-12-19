using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text.RegularExpressions;
using Redack.DomainLayer.Models;

namespace Redack.BridgeLayer.Messages.Uri
{
	public class ExpressionParameter<TEntity, TValue> : QueryParameter<Expression<Func<dynamic, dynamic>>> where TEntity: Entity

	{
	public string Expression { get; private set; }

	public ExpressionParameter(QueryBuilder builder, HttpRequestMessage request) : base(builder, request)
	{
	}

	public ExpressionParameter(string expression) : base(null)
	{
		this.Expression = expression;
	}

	public ExpressionParameter(Expression<Func<TEntity, TValue>> expression) : base(null)
	{
		string body = expression.Body.ToString();

		body = body.Replace(expression.Parameters[0].Name + ".", "");

		this.Expression = body;
	}

	/*public ExpressionParameter(Expression<Func<TEntity, object>> expression) : base(null)
	{
		string body = expression.Body.ToString();

		body = body.Replace(expression.Parameters[0].Name + ".", "");

		var pattern = @"Convert\(Convert\(\w\)\.(?<path>.*)\)";

		var regex = new Regex(pattern);
		var match = regex.Match(body);

		if (match.Success)
		{
			var path = match.Groups["path"].Value;
			body = Regex.Replace(body, pattern, path);
		}

		pattern = @"Convert\((?<path>.*)\)";

		regex = new Regex(pattern);
		match = regex.Match(body);

		if (match.Success)
		{
			var path = match.Groups["path"].Value;
			body = Regex.Replace(body, pattern, path);
		}

		this.Expression = body;
	}*/

	public override IQueryable Execute(string key, IQueryable queryable)
	{
		if (this.Builder.Policies.IsRegisteredExpressionMethod(key))
		{
			var method = this.Builder.Policies.GetExpressionMethod(key);

			var match = Regex.Match(this.Expression, @"^((\w+,)+(\w+)+)");

			if (match.Success)
			{
				var subexpressions = this.Expression.Split(',');

				foreach (var expression in subexpressions)
				{
					queryable = method(queryable, expression);
				}

				return queryable;
			}
			else
			{
				return method(queryable, this.Expression);
			}
		}

		throw new QueryBuilderException($"{key} is not allowed as expression method");
	}

	public override void FromQuery(string value)
	{
		this.Expression = value;

		if (this.Builder.Policies.HasExcludedExpressionKeyword(this.Expression))
			throw new QueryBuilderException($"{this.Expression} contains excluded keyword(s)");
	}

	public override string ToQuery()
	{
		return this.Expression;
	}
	}
}