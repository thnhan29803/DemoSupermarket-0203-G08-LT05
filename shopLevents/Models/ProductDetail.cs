using System;
using System.Collections.Generic;
using shopLevents.Models;

namespace shopLevents.Models
{
    public class ProductDetail
    {
        public int ProductID { get; set; }
        public string NamePro { get; set; }
        public string DecriptionPro { get; set; }
        public string Category { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string ImagePro { get; set; }
        public string ImageBehind { get; set; }
        public List<ProductImage> listProductImage { get; set; }
        public List<ProductImage> listColorImage { get; set; }
    }
}