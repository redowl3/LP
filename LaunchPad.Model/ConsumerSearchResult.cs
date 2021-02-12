using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIAADataModels.Transfer
{
	public class ConsumerSearchResult : SearchResult
	{
		public List<Consumer> Items { get; set; }
	}
}
