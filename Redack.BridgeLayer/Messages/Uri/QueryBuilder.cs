using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;

namespace Redack.BridgeLayer.Messages.Uri
{
	public sealed class QueryBuilder
	{
		private List<KeyValuePair<string, IQueryParameter>> Parameters { get; }

		public QueryBuilderPolicies Policies { get; set; }

		public QueryBuilder()
		{
			this.Parameters = new List<KeyValuePair<string, IQueryParameter>>();
			this.Policies = new QueryBuilderPolicies();
		}

		public void Add(string key, IQueryParameter parameter)
		{
			if(!parameter.IsUnique && this.Parameters.Any(
				e => e.Key == key && 
				e.Value.GetType() == parameter.GetType()))
				throw new QueryBuilderException(
					$"Key-Parameter association have to be unique");

			this.Parameters.Add(new KeyValuePair<string, IQueryParameter>(key, parameter));
		}

		public void AddRange(List<KeyValuePair<string, IQueryParameter>> parameters)
		{
			foreach (var parameter in parameters)
				this.Add(parameter.Key, parameter.Value);
		}

		public void Replace(string key, IQueryParameter parameter)
		{
			var keyValuePair = new KeyValuePair<string, IQueryParameter>(key, parameter);

			int index = this.Parameters.FindIndex(e => e.Key == key);

			this.Parameters[index] = keyValuePair;
		}

		public void Clear()
		{
			this.Parameters.Clear();
		}

		public T GetParameter<T>(string key) where T : IQueryParameter
		{
			return (T)this.Parameters.Find(e => e.Key == key).Value;
		}

		public Dictionary<string, string> GetMetadata(HttpRequestMessage request)
		{
			if (this.IsMetadataRequested())
			{
				PageParameter pagination = this.GetParameter<PageParameter>("paginate");

				if (pagination != null)
				{
					Dictionary<string, string>  result = new Dictionary<string, string>();

					var metadata = pagination.Metadata;

					var builder = new QueryBuilder();
					builder.AddRange(this.Parameters);

					foreach (var item in metadata)
					{
						int index = item.Value["index"];
						int size = item.Value["size"];

						builder.Replace("paginate", new PageParameter(index, size));

						var query = builder.ToQueryString();
						var uri = new UriBuilder(request.RequestUri);
						uri.Query = query;

						result.Add(item.Key, uri.Uri.ToString());
					}

					return result;
				}

				throw new QueryBuilderException("PageParameter does not exists in current context");
			}

			return null;
		}

		public bool IsMetadataRequested()
		{
			BoolParameter metadata = this.GetParameter<BoolParameter>("metadata");

			if (metadata != null && metadata.Value)
				return true;

			return false;
		}

		private string GetKeyPath(string classname)
		{
			string name = classname.Replace("Parameter", "");

			string[] keywords = Regex.Split(name, @"([A-Z]+[a-z]+)");

			keywords = keywords.Where(e => !string.IsNullOrEmpty(e)).ToArray();

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

		private IQueryParameter GetQueryParameter(string key, HttpRequestMessage request)
		{
			var match = Regex.Match(key, @"(?<keypath>.*)\..*");

			if (match.Success)
			{
				var keypath = match.Groups["keypath"].Value;
				string classname = this.ParseKeyPath(keypath) + "Parameter";

				Type classType = Type.GetType($"Redack.BridgeLayer.Messages.Uri.{classname}");

				if (classType != null)
					return (IQueryParameter)Activator.CreateInstance(classType, this, request);

				throw new QueryBuilderException($"Keypath does not exists: {keypath}");
			}

			throw new QueryBuilderException($"Incorrect format : {key}");
		}

		public string ToQueryString()
		{
			List<string> result = new List<string>();

			foreach (var parameter in this.Parameters)
			{
				string keypath = this.GetKeyPath(parameter.Value.GetType().Name);
				string value = parameter.Value.ToQuery();

				result.Add($"{keypath}.{parameter.Key}={value}");
			}

			return string.Join("&", result);
		}

		public void FromRequest(HttpRequestMessage request)
		{
			string query = request.RequestUri.Query;

			NameValueCollection values = HttpUtility.ParseQueryString(query);

			for (int i = 0; i < values.Count; i++)
			{
				var key = values.GetKey(i);
				var value = values[key];

				if(key == null)
					throw new QueryBuilderException($"Incorrect format at position {i + 1}");

				var parameter = this.GetQueryParameter(key, request);
				parameter.FromQuery(value);

				key = Regex.Replace(key, @".*\.", "");

				this.Add(key, parameter);
			}
		}

		public IQueryable Execute(IQueryable queryable)
		{
			bool isPaginated = this.Policies.RequiredPagination;
			bool hasPagination =
				this.Parameters.Count == 0 ||
				this.Parameters.Last().Value.GetType() != typeof(PageParameter);

			if (isPaginated && hasPagination)
				throw new QueryBuilderException("Pagination is required at end of each query string");

			foreach (var parameter in this.Parameters)
			{
				var key = parameter.Key;
				var value = parameter.Value;

				if (this.Policies.IsRegisteredKeyword(key, value.GetType()))
				{
					try
					{
						queryable = value.Execute(key, queryable);
					}
					catch (NotImplementedException)
					{
					}
				}
				else
				{
					throw new QueryBuilderException($"{key} is not allowed as a keyword");
				}
			}

			return queryable;
		}
	}
}