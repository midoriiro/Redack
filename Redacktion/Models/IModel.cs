using Redack.ConsumeLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redacktion.Models
{
	interface IModel<T>
	{
		T Data { get; set; }
		Func<RestPromise> PromiseOnLoad { get; set; }
		Func<RestPromise> PromiseOnUpdate { get; set; }

		event EventHandler DataChanged;

		void OnDataUpdated(object sender, EventArgs args);
		void OnReloadingRequested(object sender, EventArgs arg);

		Task LoadingData();
		Task UpdatingData();
	}
}
