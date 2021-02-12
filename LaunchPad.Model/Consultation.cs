using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIAADataModels.Transfer
{
	public class Consultation
	{		
		public Guid Id { get; set; }

		public Basket HealthPlan { get; set; }
		public Basket Basket { get; set; }

		public List<IIAADataModels.Transfer.Survey.FormResponse> SurveyResponses { get; set; }

	}
}
