using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace anyhelp.Data.DataContext
{
   public class AppConfiguration
    {
       public string PortalUrl { get; set; }
        public string APIUrl { get; set; }
        public string SqlConnectonString { get; set; }
        
      
        public string securityKey { get; set; }
        public string validIssuer { get; set; }
        public string validAudience { get; set; }
        public int expiryInMinutes { get; set; }
        public string SmtpFrom { get; set; }
        public string SmtpUsername { get; set; }
        public string SmtpPassword { get; set; }
        public string SmtpHost { get; set; }
        public string SmtpPort { get; set; }

        public string enableSsl { get; set; }
        public string publickey { get; set; }
        public string privatekey { get; set; }



        public AppConfiguration()
        {
            var configBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configBuilder.AddJsonFile(path, false);
            var root = configBuilder.Build();
            var appSettings = root.GetSection("AppSettings:ConnectionStrings:DefaultConnection");
            SqlConnectonString = appSettings.Value;
            PortalUrl = root.GetSection("AppSettings:PortalUrl").Value;
            APIUrl = root.GetSection("AppSettings:APIUrl").Value;

            privatekey = root.GetSection("AppSettings:privatekey").Value;
            publickey = root.GetSection("AppSettings:publickey").Value;
            securityKey = root.GetSection("AppSettings:TokenSettings:securityKey").Value;

            validIssuer = root.GetSection("AppSettings:TokenSettings:validIssuer").Value;
            validAudience = root.GetSection("AppSettings:TokenSettings:validAudience").Value;

            expiryInMinutes =Convert.ToInt32( root.GetSection("AppSettings:TokenSettings:expiryInMinutes").Value);

            SmtpFrom = root.GetSection("AppSettings:SmtpSettings:SmtpFrom").Value;

            SmtpUsername = root.GetSection("AppSettings:SmtpSettings:SmtpUsername").Value;

            SmtpPassword = root.GetSection("AppSettings:SmtpSettings:SmtpPassword").Value;
            SmtpHost = root.GetSection("AppSettings:SmtpSettings:SmtpHost").Value;
            SmtpPort = root.GetSection("AppSettings:SmtpSettings:SmtpPort").Value;
            enableSsl = root.GetSection("AppSettings:SmtpSettings:enableSsl").Value;

        }

}
}
