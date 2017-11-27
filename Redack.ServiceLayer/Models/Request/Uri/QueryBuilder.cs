using Redack.DatabaseLayer.DataAccess;
using Redack.DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Compilation;

namespace Redack.ServiceLayer.Models.Request.Uri
{
	public sealed class QueryBuilder
	{
		public List<KeyValuePair<string, IQueryParameter>> Parameters { get; private set; } = new List<KeyValuePair<string, IQueryParameter>>();

		public Dictionary<string, Type> RegisteredKeywords = new Dictionary<string, Type>();
		public Dictionary<string, string> RegisteredExpressionMethods = new Dictionary<string, string>();
		public List<string> ExcludeExpressionKeywords = new List<string>();

		public string ToQueryString()
		{
			List<string> result = new List<string>();

			for (int i = 0; i < this.Parameters.Count; i++)
			{
				var parameter = this.Parameters[i];

				string keypath = this.GetKeyPath(parameter.Value.GetType().Name);
				string value = parameter.Value.ToQuery();

				result.Add($"[{i}][{keypath}].{parameter.Key}={value}");
			}

			if (this.Parameters.Count == 0 || this.Parameters.Last().Key != "paginate")
				throw new InvalidOperationException("Pagination is required at end of each query string");

			return string.Join("&", this.Parameters);
		}

		public void Parse(HttpRequestMessage request)
		{
			string query = request.RequestUri.Query;

			NameValueCollection values = HttpUtility.ParseQueryString(query);

			for (int i = 0; i < values.Count; i++)
			{
				var key = values.GetKey(i);
				var value = values[key];

				var match = Regex.Match(key, @"\[\d+\]\[(?<keypath>.*)\]");

				if(match.Success)
				{
					var keypath = match.Groups["keypath"].Value;
					string classname = this.ParseKeyPath(keypath) + "Parameter";

					Type classType = Type.GetType($"Redack.ServiceLayer.Models.Request.{classname}");

					if(classType != null)
					{
						var parameter = (IQueryParameter)Activator.CreateInstance(classType, this, request);
						parameter.FromQuery(value);

						key = Regex.Replace(key, @"\[\d+\]\[.*\]\.", "");

						this.Parameters.Add(new KeyValuePair<string, IQueryParameter>(key, parameter));
					}
					else
					{
						throw new KeyNotFoundException($"Keypath({keypath}) does not exists");
					}
				}
				else
				{
					throw new KeyNotFoundException($"Keypath({match.Value}) incorrect format");
				}
			}
		}

		public IQueryable Execute(IQueryable queryable)
		{
			foreach(var parameter in this.Parameters)
			{
				var key = parameter.Key;
				var value = parameter.Value;

				if(this.RegisteredKeywords.ContainsKey(key) && this.RegisteredKeywords[key] == value.GetType())
				{
					try
					{
						queryable = value.Execute(key, queryable);
					}
					catch(NotImplementedException)
					{
						continue;
					}
				}
				else
				{
					throw new KeyNotFoundException($"Keyword({key}) is not allowed");
				}
			}

			if (this.Parameters.Count == 0 || this.Parameters.Last().Key != "paginate")
				throw new InvalidOperationException("Pagination is required at end of each query string");

			return queryable;
		}

		public void Add(string key, IQueryParameter parameter)
		{
			this.Parameters.Add(new KeyValuePair<string, IQueryParameter>(key, parameter));
		}

		private string GetKeyPath(string classname)
		{
			string name = classname.Replace("Parameter", "");

			string[] keywords = Regex.Split(name, @"([A-Z]+[a-z]+)");

			for (int i = 0; i < keywords.Length; i++)
				keywords[i] = keywords[i].ToLower();

			return string.Join(".", keywords);
		}

		private string ParseKeyPath(string keypath)
		{
			string[] keywords = keypath.Split('.');

			for (int i = 0; i < keywords.Length; i++)
			{
				keywords[i] = char.ToUpper(keywords[i][0]) + keywords[i].Substring(1);
			}

			return string.Join("", keywords);
		}
	}
}