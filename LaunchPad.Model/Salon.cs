using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIAADataModels.Transfer
{	
	public class Salon : Base.SalonBase
	{
		public List<Transfer.Therapist> Therapists { get; set; }
		public List<Transfer.ProductCategory> ProductCategories { get; set; }

		public List<Transfer.Survey.Form> Surveys { get; set; }
	}
}
