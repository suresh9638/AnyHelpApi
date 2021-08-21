using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace anyhelp.Service.Models
{
    public class UserCreateTokenModel
    {
      
        public string phoneno { get; set; }
        public string sessionId { get; set; }
        public string otp { get; set; }
       
    }
       

}
