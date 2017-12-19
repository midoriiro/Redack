using Redack.ConsumeLayer;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Redacktion.Controls
{
	public sealed partial class HttpErrorMessage : UserControl
	{
		public string Hostname { get; set; }

		public event EventHandler ReloadRequested;

		public HttpErrorMessage()
		{
			this.InitializeComponent();

			this.Hostname = RestClient.GetInstance().GetHostname();
		}

		private void OnReloadRequested(object sender, RoutedEventArgs e)
		{
			if (this.ReloadRequested != null)
				this.ReloadRequested(this, new EventArgs());
		}
	}
}
