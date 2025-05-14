using Dapper;
using FuneralFlower_BE.Models;
using System.Data;
using static FuneralFlower_BE.Controllers.BaseController;

namespace FuneralFlower_BE.Services
{
    public class NewsService : BaseService
    {
        public NewsService() : base() { }

        public NewsService(IDbConnection db) : base(db) { }

        public List<News> GetListNews(IDbTransaction? transaction = null)
        {
            string query = "select * from [news] order by CreateTime desc";
            return this._connection.Query<News>(query, transaction).ToList();
        }

        public void InsertNews(News news, IDbTransaction? transaction = null)
        {
            string query = "INSERT INTO [dbo].[news] ([Id], [Title], [TitleImageUrl], [NewsContent], [CreateTime]) " +
                "VALUES (@Id, @Title, @TitleImageUrl, @NewsContent, @CreateTime)";
            int status = this._connection.Execute(query, news, transaction);
            if (status <= 0) throw new Exception(JsonResponse.Message.ERROR_SYSTEM);
        }

        public News? GetById(string Id, IDbTransaction? transaction = null)
        {
            string query = "select * from [news] where Id = @Id";
            return this._connection.Query<News>(query, new { Id = Id }, transaction).FirstOrDefault();
        }

        public void UpdateNews(News news, IDbTransaction? transaction = null)
        {
            string query = "UPDATE [dbo].[news] SET [Title] = @Title, [TitleImageUrl] = @TitleImageUrl, [NewsContent] = @NewsContent WHERE [Id] = @Id";
            int status = this._connection.Execute(query, news, transaction);
            if (status <= 0) throw new Exception(JsonResponse.Message.ERROR_SYSTEM);
        }

        public void DeleteNews(string Id)
        {
            string query = "delete from [news] where Id = @Id";
            var status = this._connection.Execute(query, new { Id });
            if (status <= 0) throw new Exception(JsonResult.Message.ERROR_SYSTEM);
        }
    }
}
