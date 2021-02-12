using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIAADataModels.Transfer.ControlCentre
{
	public class ConsumerOrderItem
	{
		public Guid Id { get; set; }
		public Guid ConsumerOrderId { get; set; }
		public Guid ProductId { get; set; }
		public Guid VariantId { get; set; }
		public int Quantity { get; set; }
		public decimal PricePaid { get; set; }
		public int Status { get; set; }
		public int LoyaltyPoints { get; set; }
		public string PrescribingOption { get; set; }

		public IIAADataModels.Transfer.Product Product { get; set; }
		public IIAADataModels.Transfer.ProductVariant Variant { get; set; }
	}
}
