using System.Data;
using Microsoft.Data.SqlClient;
using FuneralFlower_BE.Models;
using FuneralFlower_BE.Providers;

namespace FuneralFlower_BE.Services
{
    public class BaseService
    {
        protected IDbConnection _connection;

        public BaseService()
        {
            AppSettingModel? appSetting = HelperProvider.GetAppSetting("DefaultConnection");
            if (appSetting == null) throw new Exception("Không tìm thấy thông tin kết nối cơ sở dữ liệu.");
            _connection = new SqlConnection(appSetting.DefaultConnection);
        }

        public BaseService(IDbConnection? connection)
        {

            if (connection == null)
            {
                AppSettingModel? appSetting = HelperProvider.GetAppSetting("DefaultConnection");
                if (appSetting == null) throw new Exception("Không tìm thấy thông tin kết nối cơ sở dữ liệu.");
                _connection = new SqlConnection(appSetting.DefaultConnection);
            }
            else
            {
                this._connection = connection;
            }
        }


        public static IDbConnection Connect()
        {
            AppSettingModel? appSetting = HelperProvider.GetAppSetting("DefaultConnection");
            if (appSetting == null) throw new Exception("Không tìm thấy thông tin kết nối cơ sở dữ liệu.");
            return new SqlConnection(appSetting.DefaultConnection);
        }

    }
}
