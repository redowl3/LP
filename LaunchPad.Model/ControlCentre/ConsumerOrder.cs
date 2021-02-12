using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIAADataModels.Transfer.ControlCentre
{
	public class ConsumerOrder
	{
		public Guid Id { get; set; }
		public int OrderNumber { get; set; }
		public DateTime OrderDate { get; set; }
		public Guid ConsumerId { get; set; }
		public int Status { get; set; }
		public int Substatus { get; set; }
		public string TrackingUrl { get; set; }
		public decimal ShippingCost { get; set; }
		public decimal TotalCost { get; set; }
		public string BillingAddress { get; set; }
		public string ShippingAddress { get; set; }
		public List<IIAADataModels.Transfer.ControlCentre.ConsumerOrderItem> Items {get;set;}
		public IIAADataModels.Transfer.Consumer Consumer { get; set; }
		public int Source { get; set; }

	}
}
