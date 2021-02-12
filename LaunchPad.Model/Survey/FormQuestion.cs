using System;
using System.Collections.Generic;

namespace IIAADataModels.Transfer.Survey
{
	public class FormQuestion
	{
		public Guid Id { get; set; }
		public string QuestionType { get; set; }
		public int Sort { get; set; }
		public List<FormQuestion> ChildQuestions { get; set; }
		public string QuestionData { get; set; }
		public List<FormQuestionConfig> Config { get; set; }
	}
}
