using Newtonsoft.Json;
using Redack.ConsumeLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Redacktion.Models
{
	public class Model<T> : IModel<T>
	{
		public T Data { get; set; }
		public Func<RestPromise> PromiseOnLoad { get; set; }
		public Func<RestPromise> PromiseOnUpdate { get; set; }

		public event EventHandler DataChanged;

		public async void OnDataUpdated(object sender, EventArgs args)
		{
			await Task.Run(() => this.UpdatingData());
		}

		public async void OnReloadingRequested(object sender, EventArgs arg)
		{
			await Task.Run(() => this.LoadingData());
		}

		public async Task LoadingData()
		{
			try
			{
				var promise = this.PromiseOnLoad();

				var client = RestClient.GetInstance();
				var response = await client.SendAsync(promise);

				promise.Execute(response);
			}
			catch (HttpRequestException)
			{
				throw;
			}
		}

		public async Task UpdatingData()
		{
			try
			{
				var promise = this.PromiseOnUpdate();

				var client = RestClient.GetInstance();
				var response = await client.SendAsync(promise);

				promise.Execute(response);
			}
			catch (HttpRequestException)
			{
				throw;
			}
		}
	}
}
