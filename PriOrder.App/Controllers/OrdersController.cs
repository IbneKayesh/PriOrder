using Aio.Model;
using PriOrder.App.Models;
using PriOrder.App.ModelsView;
using PriOrder.App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PriOrder.App.Controllers
{
    public class OrdersController : Controller
    {
        // GET: Orders
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Cart()
        {
            string distId = "123"; //Session["userId"];
            Tuple<List<WO_ORDER_CART>, string> _tpl = OrderService.getCartByDistId_TEMP(distId);
            if (_tpl.Item2 == AppKeys.PostSuccess)
            {
                return View(_tpl.Item1);
            }
            else
            {
                ViewBag.ErrorMessages = "Cart is empty";
                return View(new List<WO_ORDER_CART>());
            }
        }
        public ActionResult SubmitCart(List<WO_ORDER_CART> objList)
        {
            string distId = "123"; //Session["userId"];
            string result = "Success";// OrderService.OrderSubmit(distId,objList);
            var rslt = new ALERT_BOX
            {
                ALERT_MESSAGES = result
            };
            return Json(rslt);
        }
    }
}