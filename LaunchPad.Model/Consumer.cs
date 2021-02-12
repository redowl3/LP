using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIAADataModels.Transfer
{
	public class Consumer
    {
				
		public Guid Id { get; set; }
		public string Firstname  { get; set; }
		public string Lastname { get; set; }
		public string Email { get; set; }
		public string  Mobile { get; set; }
		public Guid Salon { get; set; }
		public Guid BillingAddress { get; set; }
		public Guid ShippingAddress { get; set; }
		public string SalonName { get; set; }

		public string Source { get; set; }
		public DateTime Created { get; set; }
		public DateTime Updated { get; set; }
		public DateTime DateOfBirth { get; set; }
		public int CurrentLoyaltyPoints { get; set; }
	}
}
