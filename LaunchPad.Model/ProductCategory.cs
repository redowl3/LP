using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIAADataModels.Transfer
{
	public class ProductCategory
	{
		public string Name { get; set; }
		public string Subtitle { get; set; }
		public List<ProductCategory> Subcategories { get; set; }

		public List<Product> Products { get; set; }
	}
}
