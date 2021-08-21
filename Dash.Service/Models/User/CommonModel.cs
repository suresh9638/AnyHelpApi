using anyhelp.Data.Entities;
using System;
using System.Collections.Generic;
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

}
