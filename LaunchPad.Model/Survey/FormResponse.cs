using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIAADataModels.Transfer.Survey
{
	public class FormResponse : FormResponseNoAnswers
	{
		public List<FormQuestionResponse> Answers { get; set; }
	}
}
