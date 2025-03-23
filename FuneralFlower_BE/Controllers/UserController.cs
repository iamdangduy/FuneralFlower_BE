using FuneralFlower_BE.Models;
using FuneralFlower_BE.Models.UserInfo;
using FuneralFlower_BE.Providers;
using FuneralFlower_BE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FuneralFlower_BE.Controllers
{
    [AllowAnonymous]
    public class UserController : BaseController
    {
        [HttpPost]
        [AllowAnonymous]
        public JsonResponse Login(UserLoginModel model)
        {
            try
            {
                using (var connect = BaseService.Connect())
                {
                    connect.Open();
                    using (var transaction = connect.BeginTransaction())
                    {
                        if (string.IsNullOrEmpty(model.UserName) || string.IsNullOrEmpty(model.Password)) return Error(JsonResponse.Message.LOGIN_ACCOUNT_OR_PASSWORD_EMPTY);
                        UserService userService = new UserService(connect);

                        User userLogin = userService.GetUserByOrPhoneOrAccount(model.UserName, transaction);
                        if (userLogin == null) throw new Exception(JsonResponse.Message.LOGIN_ACCOUNT_OR_PASSWORD_INCORRECT);

                        string password = SecurityProvider.EncodePassword(userLogin.Id, model.Password);
                        if (!userLogin.Password.Equals(password)) return Error(JsonResponse.Message.LOGIN_ACCOUNT_OR_PASSWORD_INCORRECT);

                        string deviceId = Guid.NewGuid().ToString().ToLower();
                        string token = SecurityProvider.CreateToken(userLogin.Id, userLogin.Password, deviceId);

                        DateTime now = DateTime.Now;
                        UserToken userToken = new UserToken();
                        userToken.CreateTime = HelperProvider.GetSeconds(now);
                        userToken.ExpireTime = HelperProvider.GetSeconds(now.AddDays(7));
                        userToken.Token = token;
                        userToken.UserId = userLogin.Id;
                        userToken.Id = Guid.NewGuid().ToString();
                        userService.InsertUserToken(userToken, transaction);
                        transaction.Commit();
                        return Success(new { token, deviceId });
                    }
                }
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResponse Register(User userRequest)
        {
            try
            {
                using (var connect = BaseService.Connect())
                {
                    connect.Open();
                    using (var transaction = connect.BeginTransaction())
                    {

                        UserService userService = new UserService(connect);
                        if (string.IsNullOrEmpty(userRequest.UserName)) return Error("Họ và tên không được để trống.");
                        if (string.IsNullOrEmpty(userRequest.Password)) return Error("Mật khẩu không được để trống.");

                        User? checkAccount = userService.GetUserByAccount(userRequest.UserName, transaction);
                        if (checkAccount != null) return Error("Tên tài khoản đã có người sử dụng");

                        User user = new User();
                        user.Id = Guid.NewGuid().ToString();
                        user.Password = SecurityProvider.EncodePassword(user.Id, userRequest.Password);

                        if (!string.IsNullOrEmpty(user.Email))
                        {
                            user.Email = userRequest.Email.Trim();
                        }

                        if (!string.IsNullOrEmpty(user.Email))
                        {
                            user.Phone = userRequest.Phone.Trim();
                        }

                        userService.InsertUser(user, transaction);


                        transaction.Commit();
                        return Success();
                    }
                }
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
    }
}
