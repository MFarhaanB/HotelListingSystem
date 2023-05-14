using HotelListingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Security.Cryptography;
using HotelListingSystem.Controllers;
using Microsoft.AspNet.Identity.Owin;


namespace HotelListingSystem.Engines
{
   
    public static class CustomerApis
    {
        private static ApplicationDbContext core = new ApplicationDbContext();
        public static HotelUsers GetUserLoginDetails(string Username, string Password)
        {
            HotelUsers User = new HotelUsers();
            var AspNetUser = GetUserInfo(new ApplicationDbContext(), Username.Trim());
            if(AspNetUser != null)
            {
                var HashedLoginPassword = SecurityEncryption.EncryptPassword(Password);
                int PasswordIsValid = string.Compare(core.HotelUsers.Find(AspNetUser.HotelUserId).MobileAppPassword, HashedLoginPassword);
                if (PasswordIsValid > 0)
                    return core.HotelUsers.Find(AspNetUser.HotelUserId);
            }
            return User;
        }

        public static SystemUser GetUserInfo(ApplicationDbContext core, string Username)
            => core.Users.FirstOrDefault(x => x.UserName == Username);
    }

    public class UserLoginViewModel
    {
        public HotelUsers User { get; set; }
             
    }

}