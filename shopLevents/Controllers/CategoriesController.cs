using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using shopLevents.Models;

namespace shopLevents.Controllers
{
    public class CategoriesController : Controller
    {
        DBSportStore_5Entities database = new DBSportStore_5Entities(); // Khai báo database object là
                                                                        // đối tượng dùng để tạo kết nối và tương tác xuống CSDL
        // GET: Categories
        public ActionResult Index()
        {
            var categories = database.Categories.ToList();         // Dùng database.Categories để lấy danh sách
                                                                   // các Categories ở dưới CDSL xong gắn cho biến categories
            return View(categories);    // Truyền danh sách các categories này 
                                        // vào View để hiển thị dữ liệu 
        }

        /* GET và POST là hai phương thức của giao thức HTTP, đều là gửi dữ liệu về server xử lí
           sau khi người dùng nhập thông tin vào form và thực hiện submit.
           Trước khi gửi thông tin, nó sẽ được mã hóa bằng cách sử dụng một giản đồ gọi là url encoding.
           Giản đồ này là các cặp name/value được kết hợp với các kí hiệu = và các kí hiệu khác nhau được ngăn cách bởi dấu &
           name=value1&name1=value2&name2=value3 
           - GET: (Kiểu như để cho ta xem thôi)
                Phương thức GET gửi thông tin người dùng đã được mã hóa thêm vào trên yêu cầu trang: 
                           http://www.example.com/index.htm?name=value1&name1=value1
           - POST: (Xử lý: xóa, tạo mới, ...)
                Phương thức POST truyền thông tin thông qua HTTP header, thông tin này được mã hóa như phương thức GET. Dữ liệu được gửi 
                bởi phương thức POST rất bảo mật vì dữ liệu được gửi ngầm, không đưa lên URL, bằng việc sử dụng Secure HTTP
            => Khi lấy dữ liệu nên dùng GET để truy xuất và xử lý nhanh hơn 
               Khi tạo dữ liệu nên dùng POST để bảo mật dữ liệu hơn
        */

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Category category)
        {
            try
            {
                database.Categories.Add(category);
                database.SaveChanges(); // Lưu thay đổi
                return RedirectToAction("Index");
            }
            catch
            {
                return Content("LỖI TẠO MỚI CATEGORY");
            }
        }
        public ActionResult Details(int id)
        {
            var category = database.Categories.Where(c => c.Id == id).FirstOrDefault(); // Where(c => c.Id == id): cú pháp Điều kiện
                                                                                        // truy vấn SQL (LinQ to SQL)
            return View(category);      // Ta truyền vào id của category cần xem thông tin chi tiết, sau đó dùng LinQ để
                                        // truy vấn category có Id
                                        // trùng với id truyền vào, đưa category này vào View để HIỂN THỊ THÔNG TIN 
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var category = database.Categories.Where(c => c.Id == id).FirstOrDefault();
            return View(category);      // Ta truyền vào id của category cần chỉnh sửa, sau đó dùng LinQ để load category có Id
                                        // trùng với id truyền vào, đưa category này vào View để HIỂN THỊ THÔNG TIN CẦN CHỈNH SỬA
        }
        [HttpPost]
        public ActionResult Edit(int id, Category category)
        {
            database.Entry(category).State = System.Data.Entity.EntityState.Modified;
            database.SaveChanges(); // lưu thay đổi 
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var category = database.Categories.Where(c => c.Id == id).FirstOrDefault();
            if(category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var category = database.Categories.Where(c => c.Id == id).FirstOrDefault();
                database.Categories.Remove(category);
                database.SaveChanges(); // lưu thay đổi 
                return RedirectToAction("Index");
            }
            catch
            {
                return Content("Không xóa được do có liên quan đến bảng khác");
            }
        }
    }
}