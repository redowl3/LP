using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIAADataModels.Transfer.ControlCentre
{
	public class ProductProperty
	{
		public Guid Id { get; set; }
		public Guid ProductPropertyInformationId { get; set; }
		public int Sort { get; set; }
		public string Detail { get; set; }
		public string ImageUrl { get; set; }

		public IIAADataModels.Transfer.ProductProperty ToTransfer()
		{
			return new Transfer.ProductProperty()
			{
				Detail = this.Detail,
				ImageUrl = this.ImageUrl
			};
		}
	}
}
