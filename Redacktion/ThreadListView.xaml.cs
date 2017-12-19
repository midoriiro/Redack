using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Redack.BridgeLayer.Messages.Uri;
using Redack.ConsumeLayer;
using Redack.DomainLayer.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
	public sealed partial class ThreadListView : Page
	{
		public ObservableCollection<Thread> Threads { get; set; }
		public Model<List<Thread>> Model;
		public string HeaderLabel { get; set; }
		public int? FromNode { get; set; } = null;
		public MainPage Root { get; set; }

		public event EventHandler<BoardPageRequestedEventArgs> ThreadRequested;

		public ThreadListView()
		{
			this.Threads = new ObservableCollection<Thread>();

			this.InitializeComponent();

			this.ListView.ItemsSource = this.Threads;

			this.Pagination.FirstPageRequested += this.OnChangePageRequested;
			this.Pagination.PreviousPageRequested += this.OnChangePageRequested;
			this.Pagination.NextPageRequested += this.OnChangePageRequested;
			this.Pagination.LastPageRequested += this.OnChangePageRequested;

			this.HeaderLabel = "Threads";

			this.Model = new Model<List<Thread>>();
			this.Model.PromiseOnLoad = () =>
			{
				var pagination = this.Pagination;
				var listview = this.ListView;

				var builder = new QueryBuilder();
				builder.Add("metadata", new BoolParameter(true));

				if (this.FromNode != null)
				{
					builder.Add("inclose", new ExpressionParameter<Thread, Node>(e => e.Node));
					builder.Add("query", new ExpressionParameter<Thread, bool>($"Node.Id == {this.FromNode}"));
				}

				builder.Add("order", new ExpressionParameter<Thread, int>(e => e.Id));
				builder.Add("paginate", new PageParameter(pagination.CurrentPageNumber, pagination.PageSize));

				var loadRequest = new RestRequest(HttpMethod.Get, "threads");
				loadRequest.SetQuery(builder);

				return new RestPromise(loadRequest)
					.Done(async (response) =>
					{
						var content = await response.Content.ReadAsStringAsync();

						var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);

						var metadata = ((JObject)data["metadata"]).ToObject<Dictionary<string, string>>();

						pagination.ParseMetadata(metadata);

						this.Model.Data = ((JArray)data["records"]).ToObject<List<Thread>>();

						this.Threads.Clear();

						foreach (var item in this.Model.Data)
							this.Threads.Add(item);
					});
			};

			this.ErrorMessage.ReloadRequested += this.OnReloadRequested;

			this.ListViewContainer.Visibility = Visibility.Collapsed;
		}

		public async void OnChangePageRequested(object sender, EventArgs args)
		{
			await this.LoadingResources();
		}

		public async void OnReloadRequested(object sender, EventArgs args)
		{
			this.ErrorMessage.Visibility = Visibility.Collapsed;

			await this.LoadingResources();
		}

		protected override async void OnNavigatedTo(NavigationEventArgs args)
		{
			base.OnNavigatedTo(args);

			if (args.Parameter != null)
			{
				if(args.Parameter.GetType() == typeof(BoardPageRequestedEventArgs))
				{
					var boardargs = (BoardPageRequestedEventArgs)args.Parameter;
					this.FromNode = boardargs.Id;
					this.Root = boardargs.Root;

					this.ThreadRequested += this.Root.OnThreadRequested;
				}
				else
				{
					var root = (MainPage)args.Parameter;
					this.ThreadRequested += root.OnThreadRequested;
				}				
			}				
			else
			{
				this.FromNode = null;
			}

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
			var thread = (Thread)args.ClickedItem;

			if (this.ThreadRequested != null)
				this.ThreadRequested(this, new BoardPageRequestedEventArgs() { Id = thread.Id, Root = this.Root });
		}
	}
}
