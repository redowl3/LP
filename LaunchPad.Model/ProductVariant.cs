using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIAADataModels.Transfer
{
	public class ProductVariant
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Detail { get; set; }
		public int Quantity { get; set; }
		public string Unit { get; set; }
		public decimal Price { get; set; }
		public int LoyaltyPoints { get; set; }
		public string Barcode { get; set; }
		public decimal RRP { get; set; }

		public List<ProductVariantPrescribingOption> PrescribingOptions { get; set; }
	}
}
