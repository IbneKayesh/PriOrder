using Aio.Db.Client.Entrance;
using Aio.Model;
using Aio.Utility.Data;
using PriOrder.App.Models;
using PriOrder.App.ModelsView;
using PriOrder.App.Services;
using System;
using System.Collections.Generic;
using System.Data;
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
        

        public ActionResult Login(USER_LOGIN obj)
        {
            Tuple<DataTable, string> _tpl = Table.Filter(LoginService.getDistInfo(obj.USER_ID, obj.USER_PASSWORD));
            if (_tpl.Item2 == AppKeys.PostSuccess)
            {

            }
            else
            {

            }
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
            return View(new CATEGORIES().getCategories());
        }
        public ActionResult Products(int? id)
        {
            return View(new ITEMS_MASTER().getItems());
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

        public ActionResult Statements()
        {
            return View();
        }
        public ActionResult Banks()
        {
            return View();
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        public ActionResult Damages()
        {
            return View();
        }

        public ActionResult ProductView()
        {
            return View();
        }
    }
}