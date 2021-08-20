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
using System.Net.Mail;
using System.Net;

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


        public static string GenerateToken(string mobilenumber)
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
                    new Claim("mobilenumber", mobilenumber.ToString()),
                   
                    
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
                //var tokendetail = token.Split('.')[1];
                //string tokendecode = coreclass.Base64Decode(tokendetail);

                //TokenDetail tddata = JsonConvert.DeserializeObject<TokenDetail>(tokendecode);
                //AppConfiguration appsetting = new AppConfiguration();

                //string response = "true";
                //try
                //{
                //    string EndPoint = appsetting.APIUrl + "session/get-cache?key=" + tddata.uniqueid;
                //    var httpClientHandler = new HttpClientHandler();
                //    httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
                //    {
                //        return true;
                //    };
                //    var httpClient = new HttpClient(httpClientHandler) { BaseAddress = new Uri(EndPoint) };




                //    HttpResponseMessage result = await httpClient.GetAsync(EndPoint);
                //    if (result.IsSuccessStatusCode)
                //    {
                //         response = await result.Content.ReadAsStringAsync();
                //    }
                //}
                //catch(Exception ex) { ExceptionLogging.SendErrorToText(ex); }
                //if (Convert.ToBoolean(response))
                //{
                //    return false;
                //}
                AppConfiguration appsetting = new AppConfiguration();
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

        public static string Encrypt(string text)
        {
            try
            {
                AppConfiguration appConfiguration = new AppConfiguration();
                string textToEncrypt = text;
               string ToReturn = "";              
                byte[] secretkeyByte = { };
                secretkeyByte = System.Text.Encoding.UTF8.GetBytes(appConfiguration.privatekey);
                byte[] publickeybyte = { };
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(appConfiguration.publickey);
                MemoryStream ms = null;
                CryptoStream cs = null;
                byte[] inputbyteArray = System.Text.Encoding.UTF8.GetBytes(textToEncrypt);
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateEncryptor(publickeybyte, secretkeyByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    ToReturn = Convert.ToBase64String(ms.ToArray());
                }
                return ToReturn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }


        public static string Decrypt(string text)
        {
            try
            {
                AppConfiguration appConfiguration = new AppConfiguration();
                string textToDecrypt = text;
                string ToReturn = "";
              
                byte[] privatekeyByte = { };
                privatekeyByte = System.Text.Encoding.UTF8.GetBytes(appConfiguration.privatekey);
                byte[] publickeybyte = { };
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(appConfiguration.publickey);
                MemoryStream ms = null;
                CryptoStream cs = null;
                byte[] inputbyteArray = new byte[textToDecrypt.Replace(" ", "+").Length];
                inputbyteArray = Convert.FromBase64String(textToDecrypt.Replace(" ", "+"));
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateDecryptor(publickeybyte, privatekeyByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    Encoding encoding = Encoding.UTF8;
                    ToReturn = encoding.GetString(ms.ToArray());
                }
                return ToReturn;
            }
            catch (Exception ae)
            {
                throw new Exception(ae.Message, ae.InnerException);
            }
        }
        public static string SendMailWithoutAttachment(string email, string body, string subject)
        {
            try
            {
                AppConfiguration appConfiguration = new AppConfiguration();
                var smtpClient = new SmtpClient(appConfiguration.SmtpHost)
                {
                    Port = Convert.ToInt32(appConfiguration.SmtpPort),
                    Credentials = new NetworkCredential(appConfiguration.SmtpUsername, appConfiguration.SmtpPassword),
                    EnableSsl = Convert.ToBoolean(appConfiguration.enableSsl),
                };
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(appConfiguration.SmtpFrom),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(email);

                smtpClient.Send(mailMessage);
                return "Send success.";
            }
            catch
            {
                return "Send failure!";
            }
           
        }

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
