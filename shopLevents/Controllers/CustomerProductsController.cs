using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using shopLevents.Models;
using System.Net;
using PagedList;
using PagedList.Mvc;

namespace shopLevents.Controllers
{
    public class CustomerProductsController : Controller
    {
        private DBSportStore_5Entities db = new DBSportStore_5Entities();

        public ActionResult Index(string category, int? page, double min = double.MinValue, double max = double.MaxValue)
        {
            int pageSize = 6; // 
            int pageNum = (page ?? 1); // Hiện số trang 
            if (category == null) // Load hết tất cả sản phẩm 
            {
                var productList = db.Products.OrderByDescending(x => x.NamePro); // lấy Products trong CSDL
                                                                                 // sắp xếp theo NamePro x => x.NamePro lấy cột NamePro trong CSDL
                return View(productList.ToPagedList(pageNum, pageSize));
            }
            else // Load những loại sản phẩm nhất định 
            {
                var productList = db.Products.OrderByDescending(x => x.NamePro).Where(p => p.Category == category);
                return View(productList.ToPagedList(pageNum, pageSize));
            }
        }
        public ActionResult Details(int? id) // int? id: khi ta click vào một sản phẩm
                                             // thì ta sẽ truyền id vào hàm Details
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);

            List<ProductImage> listProductImage = db.ProductImages.Where(x => x.ProductID == id && x.TYPE == "CHI_TIET").ToList();
            List<ProductImage> listColorImage = db.ProductImages.Where(x => x.ProductID == id && x.TYPE == "MAU").ToList();

            ProductDetail productdetail = new ProductDetail();
            productdetail.ProductID = product.ProductID;
            productdetail.NamePro = product.NamePro;
            productdetail.DecriptionPro = product.DecriptionPro;
            productdetail.Category = product.Category;
            productdetail.Price = product.Price;
            productdetail.ImagePro = product.ImagePro;
            productdetail.ImageBehind = product.ImageBehind;
            productdetail.listProductImage = listProductImage;
            productdetail.listColorImage = listColorImage;

            if (productdetail == null)
            {
                return HttpNotFound();
            }
            return View(productdetail);
        }
        public ActionResult GetProductsByCategory()
        {
            var categories = db.Categories.ToList();
            return PartialView("CategoriesPartialView", categories);
        }
        public ActionResult GetProductsByCateId(int id, int? page)
        {
            int pageSize = 6;
            int pageNum = (page ?? 1);
            var products = db.Products.Where(p => p.Category1.Id == id).ToList();
            ViewBag.CateID = id;
            return View("Index", products.ToPagedList(pageNum, pageSize));
        }
    }
}