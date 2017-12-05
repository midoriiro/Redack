using System;

namespace Redack.BridgeLayer.Messages.Uri
{
	public sealed class QueryBuilderException : Exception
	{
		public QueryBuilderException(string message) : base(message)
		{
		}
	}
}