using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Redacktion
{
	public class PageChangedEventArgs : EventArgs
	{
		public int CurrentPageNumber { get; set; }
		public int LastPageNumber { get; set; }
	}
}
