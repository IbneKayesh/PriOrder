using Aio.Model;
using PriOrder.App.DataModels;
using PriOrder.App.Models;
using PriOrder.App.ModelsView;
using PriOrder.App.Services;
using PriOrder.App.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
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
            updateBalance(distId);

            Tuple<List<WO_ORDER_CART>, EQResult> _tpl = OrderService.getCartByDistId(distId);
            if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
            {
                ViewBag.CART_ITEMS = _tpl.Item1.ConvertAll(a =>
                                      {
                                          return new SelectListItem()
                                          {
                                              Text = a.ITEM_NAME,
                                              Value = a.ITEM_ID,
                                          };
                                      });

                ViewBag.WO_NOTE = ProductService.getItemNotes();

                return View(_tpl.Item1);
            }
            else
            {
                ViewBag.CART_ITEMS = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.WO_NOTE = new SelectList(Enumerable.Empty<SelectListItem>());
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

            string Order = "OR#" + new Random().Next(100, 9999999) + " Placed successfully. please deposit your payment (Test)";
            var obj = MessageService.AddNewSMS("Order", "0", "0", "0", distId, Order);
            string result = "Order succeed! But this is test Only";// OrderService.OrderSubmit(distId,objList);

            //reset cart count
            //HttpContext.Cache.Insert(distId + ApplData.CART_COUNT_CACHE, 0, null, DateTime.Now.AddMinutes(ApplData.CHACHE_TIME), Cache.NoSlidingExpiration);

            var rslt = new ALERT_MESG
            {
                messages = result
            };
            return Json(rslt);
        }

        [HttpPost]
        public ActionResult ChangeOrderNote(string itmCod, string noId, string noVal)
        {
            string distId = Session["userId"].ToString();
            EQResult result = OrderService.ChanageOrderNote(distId, itmCod, noId, noVal);
            var rslt = new ALERT_MESG
            {
                success = result.SUCCESS,
                messages = result.SUCCESS ? result.ROWS + " Items note has been updated" : "Note update failed"
            };

            return Json(rslt);
        }


        private void updateBalance(string distId)
        {
            if (Session["userBalnace"] == null)
            {
                //Get Balance
                var obj = new T_DSMA();
                Tuple<List<T_DSMA_BAL>, EQResult> _tpl_bal = AccountService.getDistBalance(distId);
                if (_tpl_bal.Item2.SUCCESS && _tpl_bal.Item2.ROWS == 1)
                {
                    obj.T_DSMA_BAL = _tpl_bal.Item1.FirstOrDefault();
                    Session["userBalnace"] = obj.T_DSMA_BAL.DBAL_ABAL;
                }
                //End Get Balance 
            }
        }
    }
}