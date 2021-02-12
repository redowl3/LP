using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIAADataModels.Transfer
{
	public class DistributorSearchResult : SearchResult
	{
		public List<Distributor> Items { get; set; }
	}
}
