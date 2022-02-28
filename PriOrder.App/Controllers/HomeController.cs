using PriOrder.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PriOrder.App.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MyProfile()
        {
            return View();
        }

        public ActionResult Login()
        {
            ViewBag.Title = "Login";
            return View();
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            return RedirectToAction(nameof(Login));
        }

        public ActionResult Categories()
        {
            List<CATEGORIES> objList = new List<CATEGORIES>();
            objList.Add(new CATEGORIES { ID = 1, CATEGORY_NAME = "Electronic items", CATEGORY_IMAGE = "/Assets/images/icons/category-blue/cpu.svg", ITEMS_COUNT = 440 });
            objList.Add(new CATEGORIES { ID = 2, CATEGORY_NAME = "Home equipments", CATEGORY_IMAGE = "/Assets/images/icons/category-blue/homeitem.svg", ITEMS_COUNT = 346 });
            objList.Add(new CATEGORIES { ID = 3, CATEGORY_NAME = "Toys and kids", CATEGORY_IMAGE = "/Assets/images/icons/category-blue/toy.svg", ITEMS_COUNT = 145 });
            objList.Add(new CATEGORIES { ID = 4, CATEGORY_NAME = "Accessories", CATEGORY_IMAGE = "/Assets/images/icons/category-blue/watch.svg", ITEMS_COUNT = 650 });
            objList.Add(new CATEGORIES { ID = 5, CATEGORY_NAME = "Jewelleries", CATEGORY_IMAGE = "/Assets/images/icons/category-blue/diamond.svg", ITEMS_COUNT = 90 });
            return View(objList);
        }
        public ActionResult Products(int? id)
        {
            return View();
        }
        public ActionResult Messages()
        {
            List<MESSAGES> objList = new List<MESSAGES>();
            objList.Add(new MESSAGES { ID = 1, MESSAGES_DATE_TIME = "01-January-2022 09:45:18 AM", MESSAGES_BODY = "Your Order:1234 Successfully Delivered", IS_READ = false });
            objList.Add(new MESSAGES { ID = 2, MESSAGES_DATE_TIME = "02-January-2022 11:17:29 AM", MESSAGES_BODY = "Your Order:8746 Successfully Confirmed, Delivered Soon", IS_READ = true });
            objList.Add(new MESSAGES { ID = 3, MESSAGES_DATE_TIME = "03-January-2022 14:18:35 AM", MESSAGES_BODY = "Your Order:3546 Successfully Cancelled", IS_READ = false });
            return View(objList);
        }

        public ActionResult Support()
        {
            List<SUPPORT_MESSAGES> objList = new List<SUPPORT_MESSAGES>();
            objList.Add(new SUPPORT_MESSAGES { ID = 1, MESSAGES_DATE_TIME = "01-January-2022 09:45:18 AM", MESSAGES_BODY = "Order:1234 Please Refund", IS_READ = false });
            objList.Add(new SUPPORT_MESSAGES { ID = 2, MESSAGES_DATE_TIME = "01-January-2022 09:45:18 AM", MESSAGES_BODY = "Your Order:1234 Successfully Refund", REPLIED_BY = "Mr. Support", IS_READ = false });
            objList.Add(new SUPPORT_MESSAGES { ID = 1, MESSAGES_DATE_TIME = "01-January-2022 09:45:18 AM", MESSAGES_BODY = "Thank you", IS_READ = false });
            return View(objList);
        }

        public ActionResult Cart()
        {
            return View();
        }
        public ActionResult OrderSuccess()
        {
            return View();
        }
        public ActionResult Favorite()
        {
            return View();
        }
        public ActionResult MyOrders()
        {
            return View();
        }
    }
}