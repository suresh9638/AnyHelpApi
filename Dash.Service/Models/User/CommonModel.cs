using anyhelp.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using System.Text;

namespace anyhelp.Service.Models
{
    
        public  class twofactorModel
        {
          public string Status { get; set; }
    public string Details { get; set; }
    }



    public class TokenDetails
    {
        public string phoneno { get; set; }
        public string isseller { get; set; }
        public int nbf { get; set; }
        public int exp { get; set; }
        public int iat { get; set; }
        public string iss { get; set; }
        public string aud { get; set; }
    }

    public  class Category
    {
        public long CategoryId { get; set; }
        public string CategoryName { get; set; }
       
    }

    public class CreateInquiryModel
    {
        public string Id_Token { get; set; }
        public long CategoryId { get; set; }
        public string Fullname { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
    }
    public class NotificationCount
    {
        public string Phoneno { get; set; }
        public long Count { get; set; }

    }
    public class PaymentRequestModel
    {
        [Required(ErrorMessage = "Id Token is required.")]
        public string Id_Token { get; set; }
        public string EmailId { get; set; }
        [Required(ErrorMessage = "Amount is required.")]
        public string Amount { get; set; }
    }
}
