using System;
using System.Collections.Generic;
using System.Linq;
using Redack.DomainLayer.Models;

namespace Redack.BridgeLayer.Messages.Uri
{
	public sealed class QueryBuilderPolicies
	{
		private readonly Dictionary<string, Type> _registeredKeywords;
		private readonly Dictionary<string, Func<IQueryable, string, IQueryable>> _registeredExpressionMethods;
		private readonly List<string> _excludedExpressionKeywords;
		public bool RequiredPagination;

		public QueryBuilderPolicies()
		{
			this._registeredKeywords = new Dictionary<string, Type>();
			this._registeredExpressionMethods = new Dictionary<string, Func<IQueryable, string, IQueryable>>();
			this._excludedExpressionKeywords = new List<string>();
			this.RequiredPagination = true;
		}

		public void RegisterKeyword(string keyword, Type type)
		{
			this._registeredKeywords.Add(keyword, type);
		}

		public void RegisterExpression(string keyword, Func<IQueryable, string, IQueryable> method)
		{
			if(!this._registeredKeywords.ContainsKey(keyword))
				throw new QueryBuilderException($"{keyword} does not exists as keyword");
			if(this._registeredKeywords[keyword] != typeof(ExpressionParameter<Entity, dynamic>))
				throw new QueryBuilderException(
					"To register an expression method " +
					"you have to first register a keyword with a type " +
					$"{typeof(ExpressionParameter<Entity, dynamic>).Name}");

			this._registeredExpressionMethods.Add(keyword, method);
		}

		public void ExcludeExpressionKeyword(string keyword)
		{
			this._excludedExpressionKeywords.Add(keyword);
		}

		public bool IsRegisteredKeyword(string keyword, Type type)
		{
			return 
				this._registeredKeywords.ContainsKey(keyword) && 
				this._registeredKeywords[keyword] == type;
		}

		public bool IsRegisteredExpressionMethod(string keyword)
		{
			return this._registeredExpressionMethods.ContainsKey(keyword);
		}

		public bool HasExcludedExpressionKeyword(string expression)
		{
			return this._excludedExpressionKeywords.Any(expression.Contains);
		}

		public Type GetRegisteredKeyword(string keyword)
		{
			return this._registeredKeywords[keyword];
		}

		public Func<IQueryable, string, IQueryable> GetExpressionMethod(string keyword)
		{
			return this._registeredExpressionMethods[keyword];
		}
	}
}