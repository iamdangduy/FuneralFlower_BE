using FuneralFlower_BE.Models;
using Newtonsoft.Json;

namespace FuneralFlower_BE.Providers
{
    public class HelperProvider
    {
        public static AppSettingModel? GetAppSetting(string key)
        {
            try
            {
                using (StreamReader r = new StreamReader("appsettings.json"))
                {
                    string json = r.ReadToEnd();
                    return JsonConvert.DeserializeObject<AppSettingModel>(json);
                }
            }
            catch
            {

            }
            return null;
        }

        public static long GetSeconds(DateTime? dateTime = null)
        {
            try
            {
                if (!dateTime.HasValue) dateTime = DateTime.UtcNow;
                var Timestamp = new DateTimeOffset(dateTime.Value).ToUnixTimeMilliseconds();
                return Timestamp;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
