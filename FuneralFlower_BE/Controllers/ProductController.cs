using FuneralFlower_BE.Models;
using FuneralFlower_BE.Providers;
using FuneralFlower_BE.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.Extensions.Hosting;

namespace FuneralFlower_BE.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProductController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        //public string GetImagePath(string filename)
        //{
        //    var path = Path.Combine(_hostingEnvironment.ContentRootPath, Constant.PATH.PRODUCT_IMAGE_PATH, filename);
        //    return path;
        //}

        [HttpGet]
        public JsonResponse GetListProduct()
        {
            try
            {
                ProductService productService = new ProductService();
                return Success(productService.GetListProduct());
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        [HttpPost]
        public JsonResponse InsertProduct(Product model)
        {
            try
            {
                UserService userService = new UserService();
                string token = Request.Headers.Authorization.ToString();
                User? user = userService.GetUserByToken(token);
                if (user == null) return Unauthorized();
                ProductService productService = new ProductService();
                Product product = new Product();
                product.Id = Guid.NewGuid().ToString();
                product.ProductName = model.ProductName;
                product.ProductOldPrice = model.ProductOldPrice;
                product.ProductNewPrice = model.ProductNewPrice;
                product.Description = model.Description;
                //if (!string.IsNullOrEmpty(model.ProductImageUrl))
                //{
                //    string filename = Guid.NewGuid().ToString() + ".jpg";
                //    var path = GetImagePath(Constant.PATH.PRODUCT_IMAGE_PATH + filename);
                //    HelperProvider.Base64ToImage(model.ProductImageUrl, path);
                //    product.ProductImageUrl = Constant.PATH.PRODUCT_IMAGE_URL + filename;
                //}

                if (!string.IsNullOrEmpty(model.ProductImageUrl))
                {
                    string filename = Guid.NewGuid().ToString() + ".jpg";

                    // Sử dụng IHostingEnvironment để lấy đường dẫn
                    var path = Path.Combine(_hostingEnvironment.ContentRootPath, Constant.PATH.PRODUCT_IMAGE_PATH, filename);

                    // Gọi hàm chuyển đổi Base64 sang hình ảnh
                    HelperProvider.Base64ToImage(model.ProductImageUrl, path);

                    // Kiểm tra và xóa file cũ
                    if (!HelperProvider.DeleteFile(product.ProductImageUrl, _hostingEnvironment))
                    {
                        return Error();
                    }

                    // Cập nhật đường dẫn hình ảnh
                    product.ProductImageUrl = Constant.PATH.PRODUCT_IMAGE_PATH + filename;
                }
                product.CreateTime = HelperProvider.GetSeconds();
                productService.InsertProduct(product);

                return Success();
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        [HttpPost]
        public JsonResponse UpdateProduct(Product model)
        {
            try
            {
                UserService userService = new UserService();
                string token = Request.Headers.Authorization.ToString();
                User? user = userService.GetUserByToken(token);
                if (user == null) return Unauthorized();
                ProductService productService = new ProductService();
                var oldProduct = productService.GetById(model.Id);

                oldProduct.ProductName = model.ProductName;
                oldProduct.ProductOldPrice = model.ProductOldPrice;
                oldProduct.ProductNewPrice = model.ProductNewPrice;
                oldProduct.Description = model.Description;
                oldProduct.ProductImageUrl = model.ProductImageUrl;
                oldProduct.ProductCategoryId = model.ProductCategoryId;

                productService.UpdateProduct(oldProduct);

                return Success();
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        [HttpGet]
        public JsonResponse DeleteProduct(string Id)
        {
            try
            {
                UserService userService = new UserService();
                string token = Request.Headers.Authorization.ToString();
                User? user = userService.GetUserByToken(token);
                if (user == null) return Unauthorized();

                ProductService productService = new ProductService();
                productService.DeleteProduct(Id);
                return Success(null, "Xoá dữ liệu thành công!");
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
    }
}
