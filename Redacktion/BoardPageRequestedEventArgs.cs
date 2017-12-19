using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redacktion
{
	public class BoardPageRequestedEventArgs : EventArgs
	{
		public int Id { get; set; }
		public MainPage Root { get; set; }
	}
}
