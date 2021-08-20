using AutoMapper;
using anyhelp.Data;
using anyhelp.Data.ADO;
using anyhelp.Data.DataContext;

using anyhelp.Service.Helper;
using anyhelp.Service.Interface;
using anyhelp.Service.Models;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static anyhelp.Service.Helper.ServiceResponse;
using Microsoft.AspNetCore.SignalR;
using NET5SignalR.Models;
using anyhelp.Data.Entities;
using RestSharp;
using System.Net.Http;

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
        public ServiceResponseGeneric<List<UserServiceModel>> Create()
        {
            throw new System.NotImplementedException();
        }

        public ServiceResponseGeneric<List<UserServiceModel>> GetAll()
        {

            throw new System.NotImplementedException();
        }

        public ServiceResponseGeneric<UserServiceModel> GetById(int id)
        {
            throw new System.NotImplementedException();
        }

        public ServiceResponseGeneric<List<UserServiceModel>> Update()
        {
            throw new System.NotImplementedException();
        }
        public async Task<ExecutionResult<List<TblBuyerinquiry>>> GetAllInquiry()
        {
            return new ExecutionResult<List<TblBuyerinquiry>>(() =>
                {

                    var tblBuyerList = _sqlConnectionFactory.Value.CreateConnection().Query<TblBuyerinquiry>("select * from tblbuyerinquiry").ToList();
                    var s = coreclass.SendMailWithoutAttachment("sureshvadadodiya@gmail.com", "test", "test");
                   // var e = coreclass.Encrypt("9638407536");
                   // var d = coreclass.Decrypt(e);

                    return  tblBuyerList;


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
               
                HttpResponseMessage result =  httpClient.GetAsync(EndPoint).GetAwaiter().GetResult();
                if (result.IsSuccessStatusCode)
                {
                    response =  result.Content.ReadAsStringAsync().GetAwaiter().GetResult(); ;
                }


                string encrypt = coreclass.Encrypt(Model.mobileNumber);

                string token = coreclass.GenerateToken(encrypt);



                return  token;


            });
        }
        public async Task<ExecutionResult<List<TblBuyerinquiry>>> GetAllInquiry1()
        {
            return new ExecutionResult<List<TblBuyerinquiry>>(() =>
            {

                var tblBuyerList = _sqlConnectionFactory.Value.CreateConnection().Query<TblBuyerinquiry>("select buyerinquiry_id as  buyerinquiryid, buyerinquiry_fullname as  BuyerinquiryFullname, buyerinquiry_phoneno as BuyerinquiryPhoneno,buyerinquiry_categoryid as  BuyerinquiryCategoryid,buyerinquiry_latitude as  BuyerinquiryLatitude,buyerinquiry_longitude as BuyerinquiryLongitude,buyerinquiry_isdelete as BuyerinquiryIsdelete from tbl_buyerinquiry").ToList();

                throw new Exception("test error");

                return tblBuyerList;


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
