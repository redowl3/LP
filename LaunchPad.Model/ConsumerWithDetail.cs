using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IIAADataModels.Transfer
{
	public class ConsumerWithDetail : Consumer
	{
		//public ConsumerWithDetail()
		//{

		//}
		//public ConsumerWithDetail(Consumer Consumer)
		//{
			
			
		//	this.Id = Consumer.Id;
		//	this.Firstname = Consumer.Firstname;
		//	this.Lastname = Consumer.Lastname;
		//	this.Email = Consumer.Email;
		//	this.Mobile = Consumer.Mobile;
		//	this.Salon = Consumer.Salon;
		//	//this.SalonName = Consumer.SalonName;
		//	this.BillingAddress = Consumer.BillingAddress;
		//	this.ShippingAddress = Consumer.ShippingAddress;
		//	//this.ShippingAddress = Consumer.;
		//	/*this. = Consumer.;
		//	this. = Consumer.;
		//	this. = Consumer.;
		//	this. = Consumer.;
		//	this. = Consumer.;
		//	this. = Consumer.;
		//	this. = Consumer.;*/
		//}

		
		public List<ConsumerAddress> Addresses { get; set; }

		
		public IIAADataModels.Transfer.Salon SalonDetails { get; set; }

		
		public List<Survey.FormResponseNoAnswers> PreviousSurveyResponses { get; set; }

		//public List<IIAADataModels.Transfer.ConsumerTermsAcceptance> TermsAcceptance {get;set;}


		public List<IIAADataModels.Transfer.ConsumerFavourite> Favourites { get; set; }

		//public Consumer GetConsumer()
		//{
		//	return new Consumer() { 
		//		BillingAddress=this.BillingAddress,
		//		Email=this.Email,
		//		Firstname=this.Firstname,
		//		Id=this.Id,
		//		Lastname=this.Lastname,
		//		Mobile=this.Mobile,
		//		Salon=this.Salon,
		//		//SalonName=this.SalonName,
		//		ShippingAddress=this.ShippingAddress				
		//	};
		//}
	}
}
