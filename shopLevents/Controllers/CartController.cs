using shopLevents.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace shopLevents.Controllers
{
    public class CartController : Controller
    {
        public List<CartItem> GetCart()
        {
            List<CartItem> myCart = Session["GioHang"] as List<CartItem>; // ép kiểu Session["GioHang"] về dạng List<CartItem>
                                                                          // (Session["GioHang"] là 1 kiểu object)
            //Nếu giỏ hàng chưa tồn tại thì tạo mới và đưa vào Session
            if (myCart == null)
            {
                myCart = new List<CartItem>();
                Session["GioHang"] = myCart;
            }
            return myCart;
        }

        // Hàm thêm một sản phẩm vào giỏ hàng 
        public ActionResult AddToCart(int id)
        {
            if (Session["TaiKhoan"] == null) //Chưa đăng nhập
                return RedirectToAction("Login", "Users");
            //Lấy giỏ hàng hiện tại
            List<CartItem> myCart = GetCart();
            CartItem currentProduct = myCart.FirstOrDefault(p => p.ProductID == id);
            if (currentProduct == null)
            {
                currentProduct = new CartItem(id);
                myCart.Add(currentProduct);
            }
            else
            {
                currentProduct.Number++; //Sản phẩm đã có trong giỏ thì tăng số lượng lên 1
            }
            return RedirectToAction("Index", "CustomerProducts", new { id = id });
        }

        // Hàm tính tổng số lượng mặt hàng được mua 
        private int GetTotalNumber()
        {
            int totalNumber = 0;
            List<CartItem> myCart = GetCart();
            if (myCart != null)
                totalNumber = myCart.Sum(sp => sp.Number);
            return totalNumber;
        }

        // Hàm tính tổng tiền của các sản phẩm được mua 
        private decimal GetTotalPrice()
        {
            decimal totalPrice = 0;
            List<CartItem> myCart = GetCart();
            if (myCart != null)
                totalPrice = myCart.Sum(sp => sp.FinalPrice());
            return totalPrice;
        }

        // Hàm hiển thị thông tin bên trong giỏ hàng 
        public ActionResult GetCartInfo()
        {
            List<CartItem> myCart = GetCart();
            //Nếu giỏ hàng trống thì trả về trang ban đầu
            if (myCart == null || myCart.Count == 0)
            {
                return RedirectToAction("Index", "CustomerProducts");
            }
            ViewBag.TotalNumber = GetTotalNumber(); // giống ViewBag.Title
            ViewBag.TotalPrice = GetTotalPrice();
            return View(myCart); //Trả về View hiển thị thông tin giỏ hàng
        }

        // Hiện thông tin giỏ hàng qua Partial View
        public ActionResult CartPartial()
        {
            ViewBag.TotalNumber = GetTotalNumber();
            ViewBag.TotalPrice = GetTotalPrice();
            return PartialView();
        }

        public ActionResult DeleteCartItem(int id)
        {
            List<CartItem> myCart = GetCart();
            //Lấy sản phẩm trong giỏ hàng
            var currentProduct = myCart.FirstOrDefault(p => p.ProductID == id);
            if (currentProduct != null)
            {
                myCart.RemoveAll(p => p.ProductID == id);
                return RedirectToAction("GetCartInfo"); //Quay về trang giỏ hàng
            }
            if (myCart.Count == 0) //Quay về trang sản phẩm nếu giỏ hàng không có gì
                return RedirectToAction("Index", "CustomerProducts");
            return RedirectToAction("GetCartInfo"); //Quay về trang giỏ hàng
        }

        public ActionResult UpdateCartItem(int id, int Number)
        {
            List<CartItem> myCart = GetCart();
            //Lấy sản phẩm trong giỏ hàng
            var currentProduct = myCart.FirstOrDefault(p => p.ProductID == id);
            if (currentProduct != null)
            {
                //Cập nhật lại số lượng tương ứng
                //Lưu ý số lượng phải lớn hơn hoặc bằng 1
                currentProduct.Number = Number;
            }
            return RedirectToAction("GetCartInfo"); //Quay về trang giỏ hàng
        }

        public ActionResult ConfirmCart()
        {
            if (Session["TaiKhoan"] == null) //Chưa đăng nhập
                return RedirectToAction("Login", "Users");
            List<CartItem> myCart = GetCart();
            if (myCart == null || myCart.Count == 0) //Chưa có giỏ hàng hoặc chưa có sp
                return RedirectToAction("Index", "CustomerProducts");
            ViewBag.TotalNumber = GetTotalNumber();
            ViewBag.TotalPrice = GetTotalPrice();
            return View(myCart); //Trả về View xác nhận đặt hàng
        }

        DBSportStore_5Entities database = new DBSportStore_5Entities();
        public ActionResult AgreeCart()
        {
            Customer khach = Session["TaiKhoan"] as Customer; //Khách
            List<CartItem> myCart = GetCart(); //Giỏ hàng
            OrderPro DonHang = new OrderPro(); //Tạo mới đơn đặt hàng
            DonHang.IDCus = khach.IDCus;
            DonHang.DateOrder = DateTime.Now;
            DonHang.AddressDeliverry = "PLEASE CONTACT CUSTOMER";
            database.OrderProes.Add(DonHang);
            database.SaveChanges();
            //Lần lượt thêm từng chi tiết cho đơn hàng trên
            foreach (var product in myCart)
            {
                OrderDetail chitiet = new OrderDetail();
                chitiet.IDOrder = DonHang.ID;
                chitiet.IDProduct = product.ProductID;
                chitiet.Quantity = product.Number;
                chitiet.UnitPrice = (double)product.Price;
                database.OrderDetails.Add(chitiet);
            }
            database.SaveChanges();
            //Xóa giỏ hàng
            Session["GioHang"] = null;
            return RedirectToAction("Index", "CustomerProducts");
        }

        public ActionResult Delivery()
        {
            List<CartItem> myCart = GetCart();
            //Nếu giỏ hàng trống thì trả về trang ban đầu
            if (myCart == null || myCart.Count == 0)
            {
                return RedirectToAction("Index", "CustomerProducts");
            }
            ViewBag.TotalNumber = GetTotalNumber();
            ViewBag.TotalPrice = GetTotalPrice();
            return View(myCart); //Trả về View hiển thị thông tin giỏ hàng
        }

        public ActionResult Payment()
        {
            return View();
        }
        
        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }
    }
}