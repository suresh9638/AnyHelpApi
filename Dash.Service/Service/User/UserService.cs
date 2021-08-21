using AutoMapper;
using anyhelp.Data.DataContext;
using anyhelp.Service.Interface;
using anyhelp.Service.Models;
using Dapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static anyhelp.Service.Helper.ServiceResponse;
using Microsoft.AspNetCore.SignalR;
using NET5SignalR.Models;
using anyhelp.Data.Entities;
using System.Net.Http;
using System.Data;
using Newtonsoft.Json;

namespace anyhelp.Service.Service
{
    public class UserService : IUserService
    {

        private readonly anyhelpContext db;

        private readonly IMapper _mapper;
        private readonly IOptions<SqlConnectionFactory> _sqlConnectionFactory;
        private readonly IHubContext<NotificationHub> _notificationHub;


        public UserService(anyhelpContext Context, IMapper mapper, IOptions<SqlConnectionFactory> sqlConnectionFactory, IHubContext<NotificationHub> notificationHub)
        {
            db = Context;
            _mapper = mapper;
            _sqlConnectionFactory = sqlConnectionFactory;
            _notificationHub = notificationHub;
        }
      
        public async Task<ExecutionResult<NotificationModel>> GetAllNotification(string id_token)
        {
            return new ExecutionResult<NotificationModel>(() =>
                {
                    if (!string.IsNullOrEmpty(id_token))
                    {
                        string encodedate = "";
                        if (id_token.Split('.').Length > 1)
                        {
                            encodedate = coreclass.Base64Decode(id_token.Split('.')[1]);

                            TokenDetails tokenDetails = new TokenDetails();

                            tokenDetails = JsonConvert.DeserializeObject<TokenDetails>(encodedate);

                            if (!string.IsNullOrEmpty(tokenDetails.phoneno))
                            {
                                NotificationModel notificationModel = new NotificationModel();
                                List<BuyernotificationModel> buyernotificationList = new List<BuyernotificationModel>();
                                TblSellerregister tblSellerregister = new TblSellerregister();
                                List<SellernotificationModel> SellernotificationList = new List<SellernotificationModel>();
                                List<TblBuyerinquiry> tblBuyerinquiryList = new List<TblBuyerinquiry>();

                                var dictionaryParameters = new Dictionary<string, object>
            {
              { "@phoneno", tokenDetails.phoneno }  }
                    ;
                                var parameters = new DynamicParameters(dictionaryParameters);
                                var multipleObject = _sqlConnectionFactory.Value.CreateConnection().QueryMultiple("Sp_Get_AllNotifications", parameters, commandType: CommandType.StoredProcedure);

                                buyernotificationList = multipleObject.Read<BuyernotificationModel>().ToList();
                                tblSellerregister = multipleObject.Read<TblSellerregister>().FirstOrDefault();
                                SellernotificationList = multipleObject.Read<SellernotificationModel>().ToList();
                                tblBuyerinquiryList = multipleObject.Read<TblBuyerinquiry>().ToList();


                                tblBuyerinquiryList.ForEach(a => {
                                    a.BuyerinquiryPhoneno = coreclass.Decrypt(a.BuyerinquiryPhoneno);
                                });

                                buyernotificationList.ForEach(a => {
                                    a.SellerPhone = coreclass.Decrypt(a.SellerPhone);
                                });
                                SellernotificationList.ForEach(a => {
                                    a.Buyerinquiry = tblBuyerinquiryList.FirstOrDefault(v=>v.BuyerinquiryId==a.SellernotificationInqueryid);
                                });
                                if (tblSellerregister != null)
                                {
                                    tblSellerregister.SellerregisterPhoneno = coreclass.Decrypt(tblSellerregister.SellerregisterPhoneno);
                                }
                                notificationModel.IsSeller = Convert.ToBoolean(tokenDetails.isseller);
                                notificationModel.SellernotificationModelList = SellernotificationList;
                                notificationModel.BuyernotificationModelList = buyernotificationList;
                                notificationModel.tblSellerregister = tblSellerregister;
                                //var tblBuyerList = _sqlConnectionFactory.Value.CreateConnection().Query<TblBuyerinquiry>("select * from tblbuyerinquiry").ToList();
                                // //var s = coreclass.SendMailWithoutAttachment("sureshvadadodiya@gmail.com", "test", "test");
                                // var e = coreclass.Encrypt("7874531931");
                                //// var d = coreclass.Decrypt(e);

                                return notificationModel;

                            }
                        }
                    }
                    return null;
                });
        }

        public async Task<ExecutionResult<string>> CreateToken(UserCreateTokenModel Model)
        {
            return new  ExecutionResult<string>(() =>
            {
                //var client = new RestClient("http://2factor.in/API/V1/354d27f2-fdd2-11eb-a13b-0200cd936042/SMS/VERIFY/"+ Model.sessionId + "/"+ Model.otp + "");
                //var request = new RestRequest(Method.GET);
                //request.AddHeader("content-type", "application/x-www-form-urlencoded");
                //var restResponse = await client.ExecuteAsync(request);
                string EndPoint = "https://2factor.in/API/V1/354d27f2-fdd2-11eb-a13b-0200cd936042/SMS/VERIFY/" + Model.sessionId + "/" + Model.otp + "";

                var httpClientHandler = new HttpClientHandler();
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
                {
                    return true;
                };
                var httpClient = new HttpClient(httpClientHandler) { BaseAddress = new Uri(EndPoint) };

                var response = string.Empty;

                HttpResponseMessage result = httpClient.GetAsync(EndPoint).GetAwaiter().GetResult();
                if (result.IsSuccessStatusCode)
                {
                    response = result.Content.ReadAsStringAsync().GetAwaiter().GetResult(); ;
                }

                twofactorModel factor = JsonConvert.DeserializeObject<twofactorModel>(response);
                string token = null;
                if (factor.Details.ToLower().Contains("otp matched"))
                { 

                string encrypt = coreclass.Encrypt(Model.phoneno);

               
                var dictionaryParameters = new Dictionary<string, object>
            {
              { "@phoneno", encrypt } }
                            ;
                var parameters = new DynamicParameters(dictionaryParameters);


                var Sellerregister = _sqlConnectionFactory.Value.CreateConnection().Query<TblSellerregister>("Sp_Get_Sellerregisterinfo", parameters, commandType: CommandType.StoredProcedure).Count();


                token = coreclass.GenerateToken(encrypt, Sellerregister == 0 ? false : true);

            }

                return  token;


            });
        }
        public async Task<ExecutionResult<List<Category>>> GetSearchCaltegory(string Search)
        {
            return new ExecutionResult<List<Category>>(() =>
            {

                var dictionaryParameters = new Dictionary<string, object>
            {
              { "@search", Search } }
                            ;
                var parameters = new DynamicParameters(dictionaryParameters);


                var CategoryList = _sqlConnectionFactory.Value.CreateConnection().Query<Category>("Sp_GetCategory", parameters, commandType: CommandType.StoredProcedure).ToList();



                return CategoryList;


            });
        }

        
        //public async Task<ExecutionResult<AdminUserModel>> AdminLogin(string emailId, string password)
        //{
        //    try
        //    {

        //        string LatestPassword = coreclass.GetMD5HashData(password);
        //        var adminuser = new AdminUser();
        //        using (var dbContext = new ChronicleBitsContext())
        //        {

        //            adminuser = dbContext.AdminUsers.FirstOrDefault(a => a.Emailid.ToLower().Equals(emailId.ToLower()) && a.Password.Equals(LatestPassword) && (a.Isactive.HasValue && a.Isactive.Value));
        //        }

        //        if (adminuser != null)
        //        {
        //            string fileextension = "";
        //            try

        //            {
        //                fileextension = Path.GetExtension(adminuser.Image);
        //            }
        //            catch (Exception ex)
        //            {
        //                ExceptionLogging.SendErrorToText(ex);
        //            }
        //            AdminUserModel usermodel = new AdminUserModel();
        //            usermodel = _mapper.Map<AdminUserModel>(adminuser);
        //            usermodel.Image = coreclass.getadminimage(usermodel.Image);

        //            return new ExecutionResult<AdminUserModel>(usermodel);
        //        }
        //        else
        //        {
        //            return new ExecutionResult<AdminUserModel>(new ErrorInfo("Incorrect email id or password!"));
        //        }



        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionLogging.SendErrorToText(ex);
        //        return new ExecutionResult<AdminUserModel>(new ErrorInfo("Incorrect email id or password!"));
        //    }
        //}

        //public ServiceResponseGeneric<List<UserModel>>   GetUser()
        //{
        //    return new ServiceResponseGeneric<List<UserModel>>(() =>
        //    {
        //        var usermodel = new List<UserModel>();
        //        using (var dbContext = new anyhelpContext())
        //        {
        //            List<User> UserList = dbContext.Users.ToList().OrderByDescending(a => a.Id).ToList();
        //            usermodel = _mapper.Map <List<UserModel>>(UserList);
        //            usermodel.ForEach(a => {
        //                a.ProfilePhotoUrl = coreclass.GetUserProfileImage(a.Id, a.ProfilePhotoUrl);
        //                a.CoverPhotoUrl = coreclass.GetUserProfileImage(a.Id, a.CoverPhotoUrl);
        //            }) ;

        //            return usermodel;
        //        }
        //    });


        //}
        //public ServiceResponseGeneric<bool>   UpdateUserStatus(long userid)
        //{
        //    return new ServiceResponseGeneric<bool>(() =>
        //    {

        //        User user = db.Users.FirstOrDefault(a => a.Id==userid);
        //        user.Isactive = user.Isactive ? false : true;               
        //        db.Entry(user).State = EntityState.Modified;
        //        db.SaveChangesAsync();

        //        return true;


        //    });

        //}


        //public ServiceResponseGeneric<bool>  UserLogout(long UserId)
        //{


        //    return new ServiceResponseGeneric<bool>(() =>
        //    {
        //        using (ADOConn AdoConnection = new ADOConn())
        //        {
        //            string Status = UserCurrentStatus.OffLine.ToString(); ;

        //            Dictionary<object, object> ParameterList = new Dictionary<object, object>();
        //            ParameterList.Add("UserId", UserId);
        //            ParameterList.Add("Status", Status);
        //            var id = AdoConnection.StoreProcedureExecuteNonQuery("Sp_UpdateUserOnlineStatus", ParameterList);

        //            return true;
        //        }
        //    });


        //}

    }
}
