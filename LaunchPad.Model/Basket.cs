using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIAADataModels.Transfer
{
	public class Basket
	{
		public Guid Id { get; set; }
		public List<BasketItem> Items { get; set; }
	}
}
