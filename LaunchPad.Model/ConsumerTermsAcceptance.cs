using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIAADataModels.Transfer
{	
	public class ConsumerTermsAcceptance
	{
		public Guid Id { get; set; }
		public Guid TermsId { get; set; }
		public Guid ConsumerId { get; set; }
		public DateTime AcceptedDate { get; set; }
	}
}
