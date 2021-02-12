using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIAADataModels.Transfer
{
	public class BasketItem
	{
		public Guid ProductId { get; set; }
		public Guid VariantId { get; set; }
		public string PrescribingOption { get; set; }
		public string Notes { get; set; }
		public int Quantity { get; set; }
		public string BarcodeValue { get; set; }
	}
}
