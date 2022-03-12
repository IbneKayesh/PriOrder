using Aio.Model;
using PriOrder.App.DataModels;
using PriOrder.App.Models;
using PriOrder.App.ModelsView;
using PriOrder.App.Services;
using PriOrder.App.Utility;
using System;
using System.Collections.Generic;
using System.Web.Caching;
using System.Web.Mvc;

namespace PriOrder.App.Controllers
{
    [AioAuthorization]
    public class OrdersController : Controller
    {
        public ActionResult Cart()
        {
            string distId = Session["userId"].ToString();
            Tuple<List<WO_ORDER_CART>, EQResult> _tpl = OrderService.getCartByDistId(distId);
            if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
            {
                return View(_tpl.Item1);
            }
            else
            {
                TempData["mesg"] = SweetMessages.Info("Cart is empty");
                return View(new List<WO_ORDER_CART>());
            }
        }

        public ActionResult DeleteFromCart(string itemId)
        {
            string distId = Session["userId"].ToString();
            EQResult result = OrderService.DelMyCartItem(distId, itemId);

            if (result.SUCCESS)
            {
                int CartCount = OrderService.getCartItemsCount(distId);
                HttpContext.Cache.Insert(distId + ApplData.CART_COUNT_CACHE, CartCount, null, DateTime.Now.AddMinutes(ApplData.CHACHE_TIME), Cache.NoSlidingExpiration);
            }

            var rslt = new ALERT_MESG
            {
                success = result.SUCCESS,
                messages = result.SUCCESS == true ? $"Product: {itemId} removed from cart" : "Product removed failed, try again!"
            };
            return Json(rslt);
        }
        public ActionResult SubmitCart(List<WO_ORDER_CART> objList)
        {
            string distId = Session["userId"].ToString();

            string Order = "OR#" + new Random().Next(100, 9999999) +" Placed successfully. please deposit your payment";

            var obj = MessageService.AddNewSMS("Order", "0", "0", "0", distId, Order);

            string result = "Order Saved success, Check SMS";// OrderService.OrderSubmit(distId,objList);
            var rslt = new ALERT_MESG
            {
                messages = result
            };
            return Json(rslt);
        }

       
    }
}