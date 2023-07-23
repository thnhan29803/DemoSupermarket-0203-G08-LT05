using shopLevents.Models;
using System.Linq;
public class CartItem
{
    // Trong một giỏ hàng sẽ gồm các mặt hàng được mua, thông thường mỗi mặt hàng được mua cần lưu trữ các thông tin cơ bản:
    // Mã mặt hàng, Tên mặt hàng, Hình ảnh, Giá, Số lượng mua 
    DBSportStore_5Entities db = new DBSportStore_5Entities();
    public int ProductID { get; set; }
    public string NamePro { get; set; }
    public string ImagePro { get; set; }
    public decimal Price { get; set; }
    public int Number { get; set; }
    //Tính FinalPrice = Price * Number
    public decimal FinalPrice()
    {
        return Number * Price;
    }
    public CartItem(int ProductID)
    {
        this.ProductID = ProductID;
        var productDB = db.Products.Single(s => s.ProductID == this.ProductID);
        this.NamePro = productDB.NamePro;
        this.ImagePro = productDB.ImagePro;
        this.Price = (decimal)productDB.Price;
        this.Number = 1;
    }
}