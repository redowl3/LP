using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIAADataModels.Transfer.Survey
{
	public class FormQuestionResponse
	{
		public Guid QuestionId { get; set; }
		public string Answer { get; set; }
		public string Notes { get; set; }
	}
}
