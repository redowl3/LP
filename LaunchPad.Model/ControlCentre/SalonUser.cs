using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIAADataModels.Transfer.ControlCentre
{	
	public class SalonUser
	{
		public Guid Id { get; set; }
		public string Firstname { get; set; }
		public string Lastname { get; set; }
		public string ImageUrl { get; set; }
		public string PasswordHash { get; set; }
		public Guid SalonId { get; set; }
		public int UserType { get; set; }
		public string Email { get; set; }
		public string Salt { get; set; }
		public bool Active { get; set; }
	}
}
