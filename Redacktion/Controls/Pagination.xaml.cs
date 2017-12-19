using Newtonsoft.Json;
using Redack.BridgeLayer.Messages.Uri;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Redacktion.Controls
{
	public sealed partial class Pagination : UserControl
	{
		public int PageSize { get; set; } 
		public int CurrentPageNumber { get; set; }
		public int LastPageNumber { get; set; }

		public string PageCount 
		{ 
			get
			{
				return $"{this.CurrentPageNumber} / {this.LastPageNumber}";
			}
		}

		public event EventHandler FirstPageRequested;
		public event EventHandler PreviousPageRequested;
		public event EventHandler NextPageRequested;
		public event EventHandler LastPageRequested;

		public Pagination()
		{
			this.InitializeComponent();

			this.PageSize = 20;
			this.CurrentPageNumber = 1;
			this.LastPageNumber = 1;
		}

		private void OnFirstPageRequested(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			this.CurrentPageNumber = 1;

			this.FirstPageRequested?.Invoke(this, new EventArgs());
		}

		private void OnPreviousPageRequested(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			if(this.CurrentPageNumber > 1)
			{
				this.CurrentPageNumber--;
			}
			else
			{
				this.CurrentPageNumber = 1;
			}

			this.PreviousPageRequested?.Invoke(this, new EventArgs());
		}

		private void OnNextPageRequested(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			if (this.CurrentPageNumber < this.LastPageNumber)
			{
				this.CurrentPageNumber++;
			}
			else
			{
				this.CurrentPageNumber = this.LastPageNumber;
			}

			this.NextPageRequested?.Invoke(this, new EventArgs());
		}

		private void OnLastPageRequested(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			this.CurrentPageNumber = this.LastPageNumber;

			this.LastPageRequested?.Invoke(this, new EventArgs());
		}

		public void ParseMetadata(Dictionary<string, string> metadata)
		{
			if(metadata.ContainsKey("last"))
			{
				var uri = new UriBuilder(metadata["last"]);

				var values = HttpUtility.ParseQueryString(uri.Query);
				var value = values["paginate"];

				var lastPage = JsonConvert.DeserializeObject<Dictionary<string, int>>(value);

				this.LastPageNumber = lastPage["index"];
			}

			if (metadata.ContainsKey("self"))
			{
				var uri = new UriBuilder(metadata["self"]);

				var values = HttpUtility.ParseQueryString(uri.Query);
				var value = values["paginate"];

				var page = JsonConvert.DeserializeObject<Dictionary<string, int>>(value);

				this.CurrentPageNumber = page["index"];
				this.PageSize = page["size"];
			}

			this.Bindings.Update();

			if(this.CurrentPageNumber == 1)
			{
				this.FirstPage.IsEnabled = false;
				this.PreviousPage.IsEnabled = false;
			}
			else if (this.CurrentPageNumber > 1)
			{
				this.FirstPage.IsEnabled = true;
				this.PreviousPage.IsEnabled = true;
			}

			if (this.CurrentPageNumber == this.LastPageNumber)
			{
				this.NextPage.IsEnabled = false;
				this.LastPage.IsEnabled = false;
			}
			else if (this.CurrentPageNumber < this.LastPageNumber)
			{
				this.NextPage.IsEnabled = true;
				this.LastPage.IsEnabled = true;
			}
		}
	}
}
