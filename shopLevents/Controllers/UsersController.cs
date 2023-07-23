using shopLevents.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace shopLevents.Controllers
{
    public class UsersController : Controller
    {
        private DBSportStore_5Entities database = new DBSportStore_5Entities();

        // GET: Users
        // đã xóa hàm Index có sẵn
        [HttpGet]
        public ActionResult Register()  
        {
            return View();
        }

        // Xử lý khi người dùng điền đầy đủ thông tin của form Register và bấm nút "Tạo".
        // Phải đảm bảo user nhập đủ các trường dữ liệu và không trùng name với các user khác 
        [HttpPost]
        public ActionResult Register(Customer cust)
        {
            if (ModelState.IsValid) // Kiểm tra xem có nhập dữ liệu hay không 
            {
                if (string.IsNullOrEmpty(cust.NameCus)) // Kiểm tra data có lỗi hay không, nếu có thì chèn lỗi hiển thị ra ngoài 
                    ModelState.AddModelError(string.Empty, "Tên đăng ký không được để trống");
                if (string.IsNullOrEmpty(cust.PassCus))
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");
                if (string.IsNullOrEmpty(cust.EmailCus))
                    ModelState.AddModelError(string.Empty, "Email không được để trống");
                if (string.IsNullOrEmpty(cust.PhoneCus))
                    ModelState.AddModelError(string.Empty, "Điện thoại không được để trống");

                // Kiểm tra xem có người nào đã đăng kí với tên đăng nhập này hay chưa
                var khachhang = database.Customers.FirstOrDefault(k => k.NameCus == cust.NameCus);
                if (khachhang != null)
                    ModelState.AddModelError(string.Empty, "Đã có người đăng kí tên này");
                if (ModelState.IsValid)
                {
                    database.Customers.Add(cust);
                    database.SaveChanges(); // lưu thay đổi
                }
                else
                {
                    return View();
                }
            }
            return RedirectToAction("Login");
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        // Trường hợp tìm thấy khách hàng trong CSDL thì ta gửi thông báo thành công khi đăng nhập, đồng thời tạo 
        // Session["Taikhoan"] để lưu thông tin của khác hàng (không phải đăng nhập lại nhiều lần) 
        // Kiểm tra xem tên của khách hàng có phải là "admin" không nếu phải lưu Session["IsAdmin"] = "1" (để xử lý thêm _MaterLayout)
        // và return RedirectToAction("Index", "Products");
        [HttpPost]
        public ActionResult Login(Customer cust)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(cust.NameCus))
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập không được để trống");
                if (string.IsNullOrEmpty(cust.PassCus))
                    ModelState.AddModelError(string.Empty, "Mật khẩu không được để trống");
                if (ModelState.IsValid)
                {
                    // Tìm khách hàng có tên đăng nhập và password hợp lệ trong CSDL
                    var khachhang = database.Customers.FirstOrDefault(k => k.NameCus == cust.NameCus && k.PassCus == cust.PassCus);
                    if (khachhang != null)
                    {
                        ViewBag.ThongBao = "Đăng nhập thành công. Hệ thống đang chuyển đến trang chủ ...";
                        // Lưu vào session 
                        Session["Taikhoan"] = khachhang;

                        if (cust.NameCus == "admin")
                        {
                            Session["IsAdmin"] = "1";
                            return RedirectToAction("Index", "Products");
                        }
                        else
                        {
                            Session["IsAdmin"] = "0";
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                        ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            // Hủy session 
            Session["Taikhoan"] = null;
            Session["IsAdmin"] = null;
            Session["GioHang"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}