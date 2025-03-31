using FuneralFlower_BE.Models;
using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Drawing2D;

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

        public static void Base64ToImage(string base64String, string pathToSave, int maxWidth = 1024, int maxHeight = 1024)
        {
            try
            {
                // Tách đường dẫn
                List<string> listPartFolder = pathToSave.Split(Path.DirectorySeparatorChar).ToList();
                listPartFolder.RemoveAt(listPartFolder.Count - 1);
                string director = string.Join(Path.DirectorySeparatorChar.ToString(), listPartFolder);

                // Tạo thư mục nếu không tồn tại
                if (!Directory.Exists(director))
                {
                    Directory.CreateDirectory(director);
                }

                // Chuyển đổi Base64 thành byte[]
                byte[] imageBytes = Convert.FromBase64String(base64String);

                // Chuyển đổi byte[] thành Image
                using (var ms = new MemoryStream(imageBytes))
                {
                    using (var image = Image.FromStream(ms))
                    {
                        // Tính toán kích thước mới
                        var ratioX = maxWidth >= image.Width ? 1 : (double)maxWidth / image.Width;
                        var ratioY = maxHeight >= image.Height ? 1 : (double)maxHeight / image.Height;
                        var ratio = Math.Min(ratioX, ratioY);
                        var newWidth = (int)(image.Width * ratio);
                        var newHeight = (int)(image.Height * ratio);

                        // Tạo hình ảnh mới
                        using (var newImage = new Bitmap(newWidth, newHeight))
                        {
                            using (var thumbGraph = Graphics.FromImage(newImage))
                            {
                                thumbGraph.CompositingQuality = CompositingQuality.HighQuality;
                                thumbGraph.SmoothingMode = SmoothingMode.HighQuality;
                                thumbGraph.DrawImage(image, 0, 0, newWidth, newHeight);
                            }

                            // Lưu hình ảnh
                            newImage.Save(pathToSave);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error converting Base64 to image.", ex);
            }
        }

        public static bool DeleteFile(string path, IWebHostEnvironment hostingEnvironment)
        {
            try
            {
                // Lấy đường dẫn đầy đủ
                var fullPath = Path.Combine(hostingEnvironment.ContentRootPath, path);

                // Xóa ảnh cũ
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
                return true;
            }
            catch (Exception ex)
            {
                // Ghi log hoặc xử lý lỗi nếu cần
                return false;
            }
        }
    }
}
