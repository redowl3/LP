using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIAADataModels.Transfer
{
	public class Category
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public List<Category> Subcategories { get; set; }
		
		public Guid DistributorId { get; set; }
		public string Subtitle { get; set; }
		public int Sort { get; set; }
		public Guid ParentId { get; set; }
	}
}
