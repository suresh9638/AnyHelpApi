using anyhelp.Data.DataContext;
using DashTechEmailing;
using DashTechEmailing.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

using RestSharp;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Security.Principal;
using System.Security.AccessControl;
using System.Reflection.Metadata;

namespace anyhelp.Service.Models
{
     public static class coreclass  
    {
        public static string GetMD5HashData(string data)
        {
            //create new instance of md5
            MD5 md5 = MD5.Create();

            //convert the input text to array of bytes
            byte[] hashData = md5.ComputeHash(Encoding.Default.GetBytes(data));

            //create new instance of StringBuilder to save hashed data
            StringBuilder returnValue = new StringBuilder();

            //loop for each byte and add it to StringBuilder
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }

            // return hexadecimal string
            return returnValue.ToString();

        }

        
        public static string Base64Decode(string base64EncodedText)
        {
            if (String.IsNullOrEmpty(base64EncodedText))
            {
                return base64EncodedText;
            }

            base64EncodedText = base64EncodedText.Replace('-', '+');
            base64EncodedText = base64EncodedText.Replace('_', '/');
            base64EncodedText = base64EncodedText.Replace('=', '/');
            base64EncodedText = base64EncodedText.Replace(':', '/');
            byte[] base64EncodedBytes = ConvertFromBase64String(base64EncodedText);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        private static byte[] ConvertFromBase64String(string input)
        {
            string working = input.Replace('-', '+').Replace('_', '/');
            while (working.Length % 3 != 0)
            {
                working += '=';
            }
            try
            {
                return Convert.FromBase64String(working);
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);

                try
                {
                    return Convert.FromBase64String(input.Replace('-', '+').Replace('_', '/'));
                }
                catch (Exception ex1)
                {
                    ExceptionLogging.SendErrorToText(ex1);
                }
                try
                {
                    return Convert.FromBase64String(input.Replace('-', '+').Replace('_', '/') + "=");
                }
                catch (Exception ex1)
                {
                    ExceptionLogging.SendErrorToText(ex1);
                }
                try
                {
                    return Convert.FromBase64String(input.Replace('-', '+').Replace('_', '/') + "==");
                }
                 catch (Exception ex1)
            {
                ExceptionLogging.SendErrorToText(ex1); }

                return null;
            }
        }


        public static string GenerateToken(string emailid,bool isadmin)
        {

            AppConfiguration appsetting = new AppConfiguration();
           
            string securityKey= appsetting.securityKey;
            string validIssuer = appsetting.validIssuer;
            string validAudience = appsetting.validAudience;
            int expiryInMinutes = appsetting.expiryInMinutes;
            
            var mySecret = securityKey;
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));

            var myIssuer = validIssuer;
            var myAudience = validAudience;

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, emailid.ToString()),
                     new Claim("uniqueid", Guid.NewGuid().ToString()),
                     new Claim("Isadmin", Convert.ToString( isadmin)),
                }),
                Expires = DateTime.UtcNow.AddMinutes(expiryInMinutes),
                Issuer = myIssuer,
                Audience = myAudience,
                
                SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        
        public static async Task<bool>  ValidateCurrentToken(string token)
        {
            try
            {
                var tokendetail = token.Split('.')[1];
                string tokendecode = coreclass.Base64Decode(tokendetail);

                TokenDetail tddata = JsonConvert.DeserializeObject<TokenDetail>(tokendecode);
                AppConfiguration appsetting = new AppConfiguration();
                
                string response = "true";
                try
                {
                    string EndPoint = appsetting.APIUrl + "session/get-cache?key=" + tddata.uniqueid;
                    var httpClientHandler = new HttpClientHandler();
                    httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
                    {
                        return true;
                    };
                    var httpClient = new HttpClient(httpClientHandler) { BaseAddress = new Uri(EndPoint) };


                    

                    HttpResponseMessage result = await httpClient.GetAsync(EndPoint);
                    if (result.IsSuccessStatusCode)
                    {
                         response = await result.Content.ReadAsStringAsync();
                    }
                }
                catch(Exception ex) { ExceptionLogging.SendErrorToText(ex); }
                if (Convert.ToBoolean(response))
                {
                    return false;
                }
                string securityKey = appsetting.securityKey;
                string validIssuer = appsetting.validIssuer;
                string validAudience = appsetting.validAudience;


                var mySecret = securityKey;
                var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));

                var myIssuer = validIssuer;
                var myAudience = validAudience;

                var tokenHandler = new JwtSecurityTokenHandler();
                try
                {
                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = myIssuer,
                        ValidAudience = myAudience,
                        IssuerSigningKey = mySecurityKey
                    }, out SecurityToken validatedToken);
                }
                catch (Exception ex)
                {
                    ExceptionLogging.SendErrorToText(ex);

                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
                return false; }
        }

        //public static string SendMailWithoutAttachment(string email, string body, string subject)
        //{
        //    try
        //    {
        //        AppConfiguration appsetting = new AppConfiguration();
        //        EmailSetup emailSetup = new EmailSetup
        //        {

                   
        //        IsFreeEmail = true,
        //            SMTPServer = appsetting.SmtpHost,
        //            PortId =Convert.ToInt32( appsetting.SmtpPort),
        //            IsEnableSsl = Convert.ToBoolean( appsetting.enableSsl),
        //            IsUseDefaultCredentials = true,
        //            MailUserName = appsetting.SmtpUsername,
        //            MailUserPassword = appsetting.SmtpPassword,
        //            FromEmailId = appsetting.SmtpUsername,
        //            FromEmailHeader = appsetting.SmtpUsername,
        //            ToEmailId = email,
        //            MailSubject = subject,
        //            IsBodyHtml = true,
        //            MailBody = body,
                   
        //            IsAutoLogFileName = true,
        //            ValidateEmail=true
        //        };
        //        Emailing emailing = new Emailing();
        //        var mailResult = emailing.SendEmailUsingFreeMailServer(emailSetup);
        //        if (mailResult != null)
        //        {
        //            return mailResult.MailMessage.ToString();
        //                           }
        //        else
        //        {
        //            return "Sending mail failure!";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLogging.SendErrorToText(ex);
        //        return  ex.Message.ToString();
        //    }
        //}

        //public static  string getadminimage(string image)
        //{
        //    string img ="";
        //    try
        //    {
        //        AppConfiguration appsetting = new AppConfiguration();

        //        string path = (appsetting.uploadfilepath + "/admin/"+image).ToLower();
               

        //        if (File.Exists(path))
        //        {
        //            byte[] imgBytes = File.ReadAllBytes(path);
        //            img = "data:image/png;base64," + Convert.ToBase64String(imgBytes);

        //        }
        //        return img;
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLogging.SendErrorToText(ex);
        //        return img;
        //    }
        //}
        //public static string GetUserCoverImage(long UserId,string image)
        //{
            
        //        AppConfiguration appsetting = new AppConfiguration();

        //    string path = (appsetting.uploadfilepath + "/" + UserId.ToString() + "/coverphoto/" + image).ToLower();

        //    string url = (appsetting.PostImageURl + "/"+ UserId.ToString() + "/coverphoto/" + image).ToLower();


        //    Uri uriResult;
           
        //    if (File.Exists(path))
        //        {
        //            return url;
        //        }
        //        else
        //        {
        //            return null;
        //        }
               
        //}
        //public static string GetUserProfileImage(long UserId, string image)
        //{

        //    AppConfiguration appsetting = new AppConfiguration();
        //    string path = (appsetting.uploadfilepath + "/" + UserId.ToString() + "/profilephoto/" + image).ToLower();
        //    string url = (appsetting.PostImageURl + "/" + UserId.ToString() + "/profilephoto/" + image).ToLower();

        //    if (File.Exists(path))
        //    {
        //        return url;
        //    }
        //    else
        //    {
        //        return null;
        //    }

        //}

        //public static string GetPostFeedbackRecording(long PostOwnerId, long PostId, string file)
        //{

        //    AppConfiguration appsetting = new AppConfiguration();
        //    string path = (appsetting.uploadfilepath + "/" + PostOwnerId.ToString() + "/"+Convert.ToString( FileSegment.Post)+"/"+PostId.ToString()+"/"+Convert.ToString( FileSubSegment.Recording)+"/" + file).ToLower();
        //    string url = (appsetting.PostImageURl + "/" + PostOwnerId.ToString() + "/" + Convert.ToString(FileSegment.Post) + "/" + PostId.ToString() + "/" + Convert.ToString(FileSubSegment.Recording) + "/" + file).ToLower();

        //    if (File.Exists(path))
        //    {
        //        return url;
        //    }
        //    else
        //    {
        //        return null;
        //    }

        //}
        //public static string GetGroupCoverPhoto(long GroupId,  string file)
        //{

        //    AppConfiguration appsetting = new AppConfiguration();
        //    string path = (appsetting.uploadfilepath + "/Group/" + GroupId.ToString() + "/" + Convert.ToString(FileSegment.CoverPhoto) + "/" +  file).ToLower();
        //    string url = (appsetting.PostImageURl + "/Group/" + GroupId.ToString() + "/" + Convert.ToString(FileSegment.CoverPhoto) + "/" +  file).ToLower();

        //    if (File.Exists(path))
        //    {
        //        return url;
        //    }
        //    else
        //    {
        //        return null;
        //    }

        //}
    }


   
    

   
}
