using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIAADataModels.Transfer.Base
{
	public class ProductBase
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public string Summary { get; set; }

		public string Classification { get; set; }
		public string LevelType { get; set; }

		public string Level { get; set; }
		public List<ProductVariant> Variants { get; set; }

		public decimal LowestPrice()
		{
			if (this.Variants != null)
			{
				if (this.Variants.Any())
				{
					return this.Variants.Min(e => e.Price);
				}
			}
			return 9999999.99M;
		}

		public bool HasOffers()
		{
			if (this.Variants != null)
			{
				if (this.Variants.Any())
				{

					return this.Variants.Any(e => e.Price < e.RRP);
				}
			}
			return false;
		}
	}
}
