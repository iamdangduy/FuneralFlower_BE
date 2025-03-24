using Dapper;
using FuneralFlower_BE.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using static FuneralFlower_BE.Controllers.BaseController;

namespace FuneralFlower_BE.Services
{
    public class UserService : BaseService
    {
        public UserService() : base() { }
        public UserService(IDbConnection db) : base(db) { }
        public User? GetUserById(string id, IDbTransaction? transaction = null)
        {
            string query = "select * from [user] where Id = @id";
            return this._connection.Query<User>(query, new { id }, transaction).FirstOrDefault();
        }

        public User? GetUserByOrPhoneOrAccount(string account, IDbTransaction? transaction = null)
        {

            string phone = "";

            if (account.Length > 9) phone = "+84 " + account.Substring(account.Length - 9, 9);
            string query = "select top 1 * from [user] where Phone = @phone or UserName = @account";
            return this._connection.Query<User>(query, new { account, phone }, transaction).FirstOrDefault();
        }

        public void InsertUserToken(UserToken model, IDbTransaction? transaction = null)
        {
            string query = "INSERT INTO [dbo].[usertoken]([Id],[UserId],[Token],[ExpireTime],[CreateTime]) VALUES (@Id,@UserId,@Token,@ExpireTime,@CreateTime)";
            int status = this._connection.Execute(query, model, transaction);
            if (status <= 0) throw new Exception(JsonResponse.Message.ERROR_SYSTEM);
        }

        public User? GetUserByAccount(string account, IDbTransaction? transaction = null)
        {
            string query = "select top 1 * from [user] where UserName=@account";
            return this._connection.Query<User>(query, new { account }, transaction).FirstOrDefault();
        }

        public void InsertUser(User user, IDbTransaction? transaction = null)
        {
            string query = "INSERT INTO [dbo].[user] ([Id], [UserName], [Email], [Phone], [Password])" +
                " VALUES (@Id, @UserName, @Email, @Phone, @Password)";
            int status = this._connection.Execute(query, user, transaction);
            if (status <= 0) throw new Exception(JsonResponse.Message.ERROR_SYSTEM);
        }

        public void RemoveUserToken(string token, IDbTransaction? transaction = null)
        {
            string query = "update [usertoken] set Token=NULL where Token=@token";
            int status = this._connection.Execute(query, new { token = token }, transaction);
            if (status <= 0) throw new Exception(JsonResponse.Message.ERROR_SYSTEM);
        }

        public User? GetUserByToken(string Token, IDbTransaction? transaction = null)
        {
            string query = "select u.* from [user] u join [usertoken] ut on u.Id = ut.UserId where ut.Token = @Token";
            return this._connection.Query<User>(query, new { Token }, transaction).FirstOrDefault();
        }
    }
}
