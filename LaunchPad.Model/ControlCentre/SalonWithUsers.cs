using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIAADataModels.Transfer.ControlCentre
{
	public class SalonWithUsers :Base.SalonBase
	{
		public Guid DistributorId { get; set; }
		public List<IIAADataModels.Transfer.ControlCentre.SalonUser> Users { get; set; }
	}
}
