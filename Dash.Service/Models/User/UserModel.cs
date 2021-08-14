using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace anyhelp.Service.Models
{
    public class UserModel
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public long CreatedBy { get; set; }
        public long UpdatedBy { get; set; }
        public bool Isactive { get; set; }
        public string Status { get; set; }
        public bool Isadmin { get; set; }
        public string Connection { get; set; }
        public string ProfilePhotoUrl { get; set; }        
        public string CoverPhotoUrl { get; set; }
        public string ProfileShareMode { get; set; }
        public string UserCurrentStatus { get; set; }
        public DateTime? LastOnlineOn { get; set; }
        public long FriendsCount { get; set; }
        public long NotificationCount { get; set; }
    }

   

   
 
 

    

}
