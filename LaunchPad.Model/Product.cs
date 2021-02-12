using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIAADataModels.Transfer
{	public class Product :Base.ProductBase
	{
		

		public List<string> ImageUrls { get; set; }

		public List<ProductAdditionalInformation> AdditionalInformation { get; set; }

		public List<ProductProperty> Properties { get; set; }		

		

		public List<string> SkinConcerns { get; set; }

		
		
	}
}
