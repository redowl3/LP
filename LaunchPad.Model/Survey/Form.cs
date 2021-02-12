using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIAADataModels.Transfer.Survey
{
	public class Form
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public DateTime Created { get; set; }
		public DateTime LastUpdate { get; set; }
		public List<FormPage> Pages { get; set; }

		public int Version { get; set; }
		//public List<FormQuestion> Questions{ get; set; }
	}
}
