using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIAADataModels.Transfer.Survey
{
	public class FormResponseNoAnswers
	{
		public Guid Id { get; set; }

		public DateTime Created { get; set; }
		public Guid FormId { get; set; }
		public int Version { get; set; }
		//public Guid ConsultationId { get; set; }

	}
}
