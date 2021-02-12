using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIAADataModels.Transfer.ControlCentre
{
	public class ProductWithDistributorId : Base.ProductBase
	{
		public Guid DistributorId { get; set; }

		public List<IIAADataModels.Transfer.ControlCentre.ProductSkinConcern> SkinConcerns { get; set; }

		public List<IIAADataModels.Transfer.ControlCentre.ProductAdditionalInformation> AdditionalInformation { get; set; }

		public List<IIAADataModels.Transfer.ControlCentre.ProductImage> Images { get; set; }


		public List<ProductProperty> Properties { get; set; }

	}
}
