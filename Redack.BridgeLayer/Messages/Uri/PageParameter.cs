﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using Redack.DatabaseLayer.DataAccess;

namespace Redack.BridgeLayer.Messages.Uri
{
	public class PageParameter : QueryParameter<Dictionary<string, int>>
	{
		private readonly int DefaultPageIndex = 1;
		private readonly int DefaultPageSize = 10;
		private readonly int MinPageSize = 1;
		private readonly int MaxPageSize = 100;

		public Dictionary<string, Dictionary<string, int>> Metadata;

		public PageParameter(QueryBuilder builder, HttpRequestMessage request) : base(builder, request)
		{
			this.IsUnique = false;

			this.Metadata = new Dictionary<string, Dictionary<string, int>>();
		}

		public PageParameter(int index = 1, int? size = null) : base(null)
		{
			this.Value = new Dictionary<string, int>()
			{
				{ "index", index },
				{ "size", size ?? this.DefaultPageSize },
			};

			this.Normalize(this.Value);
		}

		private void Normalize(Dictionary<string, int> value)
		{
			 if (!value.ContainsKey("index"))
				 value["index"] = this.DefaultPageIndex;
			else if (value["index"] < this.DefaultPageIndex)
				 value["index"] = this.DefaultPageIndex;

			if (!value.ContainsKey("size"))
				value["size"] = this.DefaultPageSize;
			else if (value["size"] > MaxPageSize)
				value["size"] = this.MaxPageSize;
			else if (value["size"] < MinPageSize)
				value["size"] = this.MinPageSize;
		}

		public override IQueryable Execute(string key, IQueryable queryable)
		{
			if(this.Builder.IsMetadataRequested())
				this.GenerateMetaData(((IQueryable<dynamic>)queryable).Count());

			return queryable.Paginate(this.Value["index"], this.Value["size"]);
		}

		private void GenerateMetaData(int total)
		{
			int size = this.Value["size"];
			int mod = total % size;

			int currentIndex = this.Value["index"];
			int firstIndex = this.DefaultPageIndex;
			int lastIndex = total / size + (mod == 0 ? 0 : 1);
			int nextIndex = currentIndex + 1;
			int previousIndex = currentIndex - 1;

			// f: 1, p: 2, c: 3, n: 4, l: 10 | f,p,n,l
			// f: 1, p: 1, c: 2, n: 3, l: 10 | p,n,l
			// f: 1, p: 0, c: 1, n: 2, l: 10 | n,l

			// f: 1, p: 99008, c: 99009, n: 99010, l: 99008 | f,l
			// f: 1, p: 99007, c: 99008, n: 99009, l: 99008 | f,p
			// f: 1, p: 99006, c: 99007, n: 99008, l: 99008 | f,p,l
			// f: 1, p: 99005, c: 99006, n: 99007, l: 99008 | f,p,n,l

			var first = new Dictionary<string, int>
			{
				{"index", firstIndex},
				{"size", size},
			};

			var last = new Dictionary<string, int>
			{
				{"index", lastIndex},
				{"size", size},
			};

			var previous = new Dictionary<string, int>
			{
				{"index", previousIndex},
				{"size", size},
			};

			var next = new Dictionary<string, int>
			{
				{"index", nextIndex},
				{"size", size},
			};

			var self = new Dictionary<string, int>
			{
				{"index", currentIndex},
				{"size", size},
			};

			if (currentIndex > lastIndex)
			{
				this.Metadata.Add("first", first);
				this.Metadata.Add("last", last);
			}
			else
			{
				this.Metadata.Add("first", first);
				this.Metadata.Add("last", last);

				if (firstIndex <= previousIndex)
				{
					this.Metadata.Add("previous", previous);
				}

				if (nextIndex < lastIndex)
				{
					this.Metadata.Add("next", next);
				}

				this.Metadata.Add("self", self);
			}
		}

		public override void FromQuery(string value)
		{
			this.Value = JsonConvert.DeserializeObject<Dictionary<string, int>>(value);

			this.Normalize(this.Value);
		}

		public override string ToQuery()
		{
			return JsonConvert.SerializeObject(this.Value);
		}
	}
}