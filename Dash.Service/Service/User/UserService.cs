using AutoMapper;
using anyhelp.Data;
using anyhelp.Data.ADO;
using anyhelp.Data.DataContext;

using anyhelp.Service.Helper;
using anyhelp.Service.Interface;
using anyhelp.Service.Models;

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

namespace anyhelp.Service.Service
{
    public class UserService : IUserService
    {

        private readonly anyhelpContext db;
      
        private readonly IMapper _mapper;
        private readonly IOptions<SqlConnectionFactory> _sqlConnectionFactory;
        private readonly IHubContext<NotificationHub> _notificationHub;

       
        public UserService(anyhelpContext Context,  IMapper mapper, IOptions<SqlConnectionFactory> sqlConnectionFactory, IHubContext<NotificationHub> notificationHub)
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
