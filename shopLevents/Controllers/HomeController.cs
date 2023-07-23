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
    public class HomeController : Controller
    {
        private DBSportStore_5Entities db = new DBSportStore_5Entities();
        public ActionResult Index(int? page)
        {
            int pageSize = 9;
            int pageNum = (page ?? 1);
            var productList = db.Products.ToList();
            return View(productList.ToPagedList(pageNum, pageSize));
        }
    }
}
