using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIAADataModels.Transfer.Survey
{
	public class FormPage
	{
		public Guid Id { get; set; }
		public int PageNumber { get; set; }
		public DateTime Created { get; set; }
		public DateTime LastUpdate { get; set; }
		public string Title { get; set; }
		public List<FormQuestion> Questions{ get; set; }
	}
}
