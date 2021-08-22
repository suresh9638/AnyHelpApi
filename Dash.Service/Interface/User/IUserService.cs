
using System.Collections.Generic;
using static anyhelp.Service.Helper.ServiceResponse;

using anyhelp.Service.Models;
using System.Threading.Tasks;
using anyhelp.Data.Entities;
using System;

namespace anyhelp.Service.Interface
{
    public interface IUserService 
    {
        Task<ExecutionResult<NotificationModel>> GetAllNotification(string id_token);
        Task<ExecutionResult<List<Category>>> GetSearchCaltegory(string Search);
        Task<ExecutionResult<string>> CreateToken(UserCreateTokenModel Model);
        Task<ExecutionResult<bool>> CreateInquiry(CreateInquiryModel model);
        Task<ExecutionResult<bool>> SetReadNotification(long Notificationid, bool Isserivce);
        Task<ExecutionResult<DateTime>> test();
        //ServiceResponseGeneric<List<UserModel>> GetUser();
        //ServiceResponseGeneric<bool> UpdateUserStatus(long userid);
        //ServiceResponseGeneric<bool> UserLogout(long UserId);

    }
}
