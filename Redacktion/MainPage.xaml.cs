using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Redacktion
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

		public void OnNodeRequested(object sender, BoardPageRequestedEventArgs args)
		{
			this.ContentFrame.Navigate(typeof(ThreadListView), args);
		}

		public void OnThreadRequested(object sender, BoardPageRequestedEventArgs args)
		{
			this.ContentFrame.Navigate(typeof(MessageListView), args);
		}

		private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
			var panel = (StackPanel)args.InvokedItem;
			var item = (NavigationViewItem)panel.Parent;

			switch(item.Tag)
			{
				case "root":
					this.ContentFrame.Navigate(typeof(SignUpPage));
					break;
				case "nodes":
					this.ContentFrame.Navigate(typeof(NodeListView), this);
					break;
				case "threads":
					this.ContentFrame.Navigate(typeof(ThreadListView), this);
					break;
				case "messages":
					this.ContentFrame.Navigate(typeof(MessageListView), this);
					break;
			}
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            //throw new NotImplementedException();
        }

        private void NavigationView_Loaded(object sender, RoutedEventArgs e)
        {
			this.ContentFrame.Navigate(typeof(SignUpPage));
		}

		private void NavigationView_Loading(FrameworkElement sender, object args)
		{

		}
	}
}
