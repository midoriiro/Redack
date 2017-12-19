using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Redack.BridgeLayer.Messages.Uri;
using Redack.ConsumeLayer;
using Redack.DomainLayer.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Redacktion.Models;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Redacktion
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class NodeListView : Page
	{
		public Model<List<Node>> Model;
		public string HeaderLabel { get; set; }
		public MainPage root { get; set; }

		public NodeListView()
		{
			this.InitializeComponent();

			this.Pagination.FirstPageRequested += this.OnChangePageRequested;
			this.Pagination.PreviousPageRequested += this.OnChangePageRequested;
			this.Pagination.NextPageRequested += this.OnChangePageRequested;
			this.Pagination.LastPageRequested += this.OnChangePageRequested;

			this.HeaderLabel = "Nodes";

			this.Model = new Model<List<Node>>();
			this.Model.PromiseOnLoad = () =>
			{
				var pagination = this.Pagination;
				var listview = this.ListView;

				var builder = new QueryBuilder();
				builder.Add("metadata", new BoolParameter(true));
				builder.Add("order", new ExpressionParameter<Node, int>(e => e.Id));
				builder.Add("paginate", new PageParameter(pagination.CurrentPageNumber, pagination.PageSize));

				var loadRequest = new RestRequest(HttpMethod.Get, "nodes");
				loadRequest.SetQuery(builder);

				return new RestPromise(loadRequest)
					.Done(async (response) =>
					{
						var content = await response.Content.ReadAsStringAsync();

						var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);

						var metadata = ((JObject)data["metadata"]).ToObject<Dictionary<string, string>>();

						pagination.ParseMetadata(metadata);

						this.Model.Data = ((JArray)data["records"]).ToObject<List<Node>>();

						listview.Items.Clear();

						foreach (var item in this.Model.Data)
							listview.Items.Add(item.Name);
					});
			};

			this.ErrorMessage.ReloadRequested += this.OnReloadRequested;

			this.ListViewContainer.Visibility = Visibility.Collapsed;
		}

		public event EventHandler<BoardPageRequestedEventArgs> NodeRequested;

		public async void OnChangePageRequested(object sender, EventArgs args)
		{
			await this.LoadingResources();
		}

		public async void OnReloadRequested(object sender, EventArgs args)
		{
			this.ErrorMessage.Visibility = Visibility.Collapsed;

			await this.LoadingResources();
		}

		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			var root = (MainPage)e.Parameter;
			this.root = root;
			this.NodeRequested += root.OnNodeRequested;

			await this.LoadingResources();
		}

		public async Task LoadingResources()
		{
			try
			{
				this.LoadingIndicator.IsActive = true;

				await this.Model.LoadingData();

				this.ListViewContainer.Visibility = Visibility.Visible;
			}
			catch (HttpRequestException)
			{
				this.ListViewContainer.Visibility = Visibility.Collapsed;
				this.ErrorMessage.Visibility = Visibility.Visible;
			}
			finally
			{
				this.LoadingIndicator.IsActive = false;
			}
		}

		private void OnItemClick(object sender, ItemClickEventArgs args)
		{
			var item = (string)args.ClickedItem;

			var node =  this.Model.Data.Find(e => e.Name == item);

			if (this.NodeRequested != null)
				this.NodeRequested(this, new BoardPageRequestedEventArgs() { Id = node.Id, Root = this.root });
		}
	}
}
