using Aio.Model;
using PriOrder.App.Models;
using PriOrder.App.ModelsView;
using PriOrder.App.Services;
using PriOrder.App.Utility;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PriOrder.App.Controllers
{
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
                TempData["mesg"] = SweetMessages.Failed("Cart is empty");
                return View(new List<WO_ORDER_CART>());
            }
        }

        public ActionResult DeleteFromCart(string itemId)
        {

            apply js messages
            string distId = Session["userId"].ToString();
            EQResult result = OrderService.DelMyCartItem(distId, itemId);

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
            string result = "Success";// OrderService.OrderSubmit(distId,objList);
            var rslt = new ALERT_MESG
            {
                messages = result
            };
            return Json(rslt);
        }
    }
}