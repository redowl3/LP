using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIAADataModels.Transfer
{
	public class Therapist
	{
		public Guid Id { get; set; }
		public string Username { get; set; }
		public string PasswordHash { get; set; }
		public string Firstname { get; set; }
		public string Surname { get; set; }
		public string ImageUrl { get; set; }
		public string Salt { get; set; }
	}
}
