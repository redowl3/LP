using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIAADataModels.Transfer
{
	public class SalonConsumer
	{
		public Guid Id { get; set; }
		public string Firstname { get; set; }
		public string Lastname { get; set; }
		public string Email { get; set; }
		public string Mobile { get; set; }

		public DateTime DateOfBirth { get; set; }
		public List<Transfer.ConsumerAddress> Addresses { get; set; }

		public Transfer.Consultation CurrentConsultation { get; set; }

		public Guid TherapistId { get; set; }

		public List<Survey.FormResponseNoAnswers> PreviousResponses { get; set; }
	}
}
