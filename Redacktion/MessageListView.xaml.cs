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
	public sealed partial class MessageListView : Page
	{
		public ObservableCollection<Message> Messages { get; set; }
		public Model<List<Message>> Model;
		public string HeaderLabel { get; set; }
		public int? FromThread { get; set; } = null;
		public MainPage Root { get; set; }

		public MessageListView()
		{
			this.Messages = new ObservableCollection<Message>();

			this.InitializeComponent();

			this.ListView.ItemsSource = this.Messages;

			this.Pagination.FirstPageRequested += this.OnChangePageRequested;
			this.Pagination.PreviousPageRequested += this.OnChangePageRequested;
			this.Pagination.NextPageRequested += this.OnChangePageRequested;
			this.Pagination.LastPageRequested += this.OnChangePageRequested;

			this.HeaderLabel = "Messages";

			this.Model = new Model<List<Message>>();
			this.Model.PromiseOnLoad = () =>
			{
				var pagination = this.Pagination;
				var listview = this.ListView;

				var builder = new QueryBuilder();
				builder.Add("metadata", new BoolParameter(true));
				builder.Add("inclose", new ExpressionParameter<Message, User>(e => e.Author));

				if (this.FromThread != null)
				{
					builder.Add("inclose", new ExpressionParameter<Message, Thread>(e => e.Thread));
					builder.Add("query", new ExpressionParameter<Message, bool>($"Thread.Id == {this.FromThread}"));
				}

				builder.Add("order", new ExpressionParameter<Message, DateTime>(e => e.Date));
				builder.Add("paginate", new PageParameter(pagination.CurrentPageNumber, pagination.PageSize));

				var loadRequest = new RestRequest(HttpMethod.Get, "messages");
				loadRequest.SetQuery(builder);

				return new RestPromise(loadRequest)
					.Done(async (response) =>
					{
						var content = await response.Content.ReadAsStringAsync();

						var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);

						var metadata = ((JObject)data["metadata"]).ToObject<Dictionary<string, string>>();

						pagination.ParseMetadata(metadata);

						this.Model.Data = ((JArray)data["records"]).ToObject<List<Message>>();

						this.Messages.Clear();

						foreach (var item in this.Model.Data)
							this.Messages.Add(item);
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
				if (args.Parameter.GetType() == typeof(BoardPageRequestedEventArgs))
				{
					var boardargs = (BoardPageRequestedEventArgs)args.Parameter;
					this.FromThread = boardargs.Id;
					this.Root = boardargs.Root;
				}

			}
			else
			{
				this.FromThread = null;
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
	}
}
