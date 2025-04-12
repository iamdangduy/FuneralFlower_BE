using FuneralFlower_BE.Models;
using FuneralFlower_BE.Services;
using Microsoft.AspNetCore.Mvc;

namespace FuneralFlower_BE.Controllers
{
    public class UserTokenController : BaseController
    {
        [HttpGet]
        public JsonResponse CheckUserByCookie(string cookie)
        {
            try
            {
                using (var connect = BaseService.Connect())
                {
                    connect.Open();
                    using (var transaction = connect.BeginTransaction())
                    {
                        var userService = new UserService();
                        User? user = userService.GetUserByToken(cookie);
                        if (user == null) return Unauthorized();
                        else return Success();
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
