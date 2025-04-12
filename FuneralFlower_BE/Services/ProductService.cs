using Dapper;
using FuneralFlower_BE.Models;
using System.Data;
using static FuneralFlower_BE.Controllers.BaseController;

namespace FuneralFlower_BE.Services
{
    public class ProductService : BaseService
    {
        public ProductService() : base() { }
        public ProductService(IDbConnection db) : base(db) { }

        public List<Product> GetListProduct(IDbTransaction? transaction = null)
        {
            string query = "select * from [product]";
            return this._connection.Query<Product>(query, transaction).ToList();
        }
        
        public List<Product> GetListPreviewProduct(IDbTransaction? transaction = null)
        {
            string query = "select TOP (10) * from [product]";
            return this._connection.Query<Product>(query, transaction).ToList();
        }

        public void InsertProduct(Product product, IDbTransaction? transaction = null)
        {
            string query = "INSERT INTO [dbo].[product] ([Id], [ProductName], [ProductOldPrice], [ProductNewPrice], [Description], [ProductImageUrl], [ProductCategoryId]) " +
                "VALUES (@Id, @ProductName, @ProductOldPrice, @ProductNewPrice, @Description, @ProductImageUrl, @ProductCategoryId)";
            int status = this._connection.Execute(query, product, transaction);
            if (status <= 0) throw new Exception(JsonResponse.Message.ERROR_SYSTEM);
        }

        public Product? GetById(string Id, IDbTransaction? transaction = null)
        {
            string query = "select * from [product] where Id = @Id";
            return this._connection.Query<Product>(query, new { Id = Id }, transaction).FirstOrDefault();
        }

        public void UpdateProduct(Product product, IDbTransaction? transaction = null)
        {
            string query = "UPDATE [dbo].[product] SET [ProductName] = @ProductName, [ProductOldPrice] = @ProductOldPrice, [ProductNewPrice] = @ProductNewPrice, [Description] = @Description, [ProductImageUrl] = @ProductImageUrl, [ProductCategoryId] = @ProductCategoryId WHERE [Id] = @Id";
            int status = this._connection.Execute(query, product, transaction);
            if (status <= 0) throw new Exception(JsonResponse.Message.ERROR_SYSTEM);
        }

        public void DeleteProduct(string Id)
        {
            string query = "delete from [product] where Id = @Id";
            var status = this._connection.Execute(query, new { Id });
            if (status <= 0) throw new Exception(JsonResult.Message.ERROR_SYSTEM);
        }
    }
}
