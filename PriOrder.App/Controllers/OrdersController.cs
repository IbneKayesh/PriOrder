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

            try
            {
                foreach (WO_ORDER_CART item in objList)
                {
                    if (item.NOTE_ID == "200" && Convert.ToInt32(item.NOTE_VALUE) > 50)
                    {
                        var note200 = new ALERT_MESG
                        {
                            messages = "Enter Note Value below 50",
                            success = false
                        };
                        return Json(note200);
                    }
                }

                EQResult result = OrderService.UpdateCart(distId, objList);
                var rslt = new ALERT_MESG
                {
                    messages = "Order processed, Confirm your order",
                    success = true
                };
                return Json(rslt);
            }
            catch (Exception ex)
            {
                var rslt = new ALERT_MESG
                {
                    messages = "Somthing went wrong",
                    success = false
                };
                return Json(rslt);
            }




            //string Order = "OR#" + new Random().Next(100, 9999999) + " Placed successfully. please deposit your payment (Test)";
            //var obj = MessageService.AddNewSMS("Order", "0", "0", "0", distId, Order);


            //reset cart count
            //HttpContext.Cache.Insert(distId + ApplData.CART_COUNT_CACHE, 0, null, DateTime.Now.AddMinutes(ApplData.CHACHE_TIME), Cache.NoSlidingExpiration);


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
                Tuple<List<T_DSMA_BAL>, EQResult> _tpl_bal = AccountService.getDistBalance("WAUTO", distId, "GET_BALANCE", distId, "", "");
                if (_tpl_bal.Item2.SUCCESS && _tpl_bal.Item2.ROWS == 1)
                {
                    obj.T_DSMA_BAL = _tpl_bal.Item1.FirstOrDefault();
                    Session["userBalnace"] = obj.T_DSMA_BAL.DBAL_ABAL;
                }
                //End Get Balance 
            }
        }



        public ActionResult ConfirmOrder()
        {
            string distId = Session["userId"].ToString();
            WO_ORDER_CART_PAYMENT obj = new WO_ORDER_CART_PAYMENT();
            //Check Direct Delivery Amount >> no active button


            //get Balance
            Tuple<List<T_DSMA_BAL>, EQResult> _tpl_Bal = AccountService.getDistBalance("WAUTO", distId, "GET_BALANCE", distId, "", "1");
            obj.T_DSMA_BAL = _tpl_Bal.Item1.FirstOrDefault();

            //get Incentive
            Tuple<T_MBDO_INCV, EQResult> _tpl_Inc = OrderService.getIncentive("WAUTO", distId, "GET_INCV", distId, "", "");
            obj.T_MBDO_INCV = _tpl_Inc.Item1;

            //No rows no active button
            //Current Order
            Tuple<List<T_MBDO>, EQResult> _tpl_Curr = OrderService.getPendingActiveOrderByDistId(distId);
            if (_tpl_Curr.Item2.ROWS == 0)
            {
                //obj.IS_VALID = false;
            }
            obj.T_MBDO = _tpl_Curr.Item1;

            //Total
            obj.TOTAL = _tpl_Curr.Item1.Sum(x => x.TOTAL);

            //Inc Total Calculation
            obj.T_MBDO_INCV.NET = obj.TOTAL - obj.T_MBDO_INCV.SPV - obj.T_MBDO_INCV.INCV;

            return View(obj);
        }
        [HttpPost]
        public ActionResult ConfirmOrder(WO_ORDER_CART_PAYMENT obj)
        {
            return RedirectToAction("ConfirmOrder");
        }
    }
}