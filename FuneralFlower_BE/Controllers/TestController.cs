using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FuneralFlower_BE.Controllers
{
    public class TestController : BaseController
    {
        [HttpGet]
        public JsonResponse Test(string a)
        {
            return Success(data: a);
        }
    }
}
