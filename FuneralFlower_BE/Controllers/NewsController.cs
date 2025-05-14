namespace FuneralFlower_BE.Controllers;

using FuneralFlower_BE.Models;
using FuneralFlower_BE.Providers;
using FuneralFlower_BE.Services;
using Microsoft.AspNetCore.Mvc;

public class NewsController : BaseController
{
    private readonly IWebHostEnvironment _hostingEnvironment;
    public NewsController(IWebHostEnvironment hostingEnvironment)
    {
        _hostingEnvironment = hostingEnvironment;
    }

    [HttpGet]
    public JsonResponse GetListNews()
    {
        try
        {
            NewsService newsService = new NewsService();
            return Success(newsService.GetListNews());
        }
        catch (Exception ex)
        {
            return Error(ex.Message);
        }
    }

    [HttpGet]
    public JsonResponse GetNewsById(string Id)
    {
        try
        {
            NewsService newsService = new NewsService();
            return Success(newsService.GetById(Id));
        }
        catch (Exception ex)
        {
            return Error(ex.Message);
        }
    }

    [HttpPost]
    public JsonResponse InsertNews(News model)
    {
        try
        {
            NewsService newsService = new NewsService();
            News news = new News();
            news.Id = Guid.NewGuid().ToString();
            news.Title = model.Title;
            if (!string.IsNullOrEmpty(model.TitleImageUrl))
            {
                string filename = Guid.NewGuid().ToString() + ".jpg";
                var path = _hostingEnvironment.WebRootPath + Constant.PATH.NEWS_IMAGE_PATH + filename;
                HelperProvider.Base64ToImage(model.TitleImageUrl, path);
                news.TitleImageUrl = Constant.PATH.NEWS_IMAGE_URL + filename;
            }
            news.NewsContent = model.NewsContent;
            news.CreateTime = HelperProvider.GetSeconds();

            newsService.InsertNews(news);

            return Success();
        }
        catch (Exception ex)
        {
            return Error(ex.Message);
        }
    }
}
