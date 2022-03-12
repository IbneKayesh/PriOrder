using Aio.Model;
using PriOrder.App.DataModels;
using PriOrder.App.Models;
using PriOrder.App.ModelsView;
using PriOrder.App.Services;
using PriOrder.App.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace PriOrder.App.Controllers
{
    public class ProductsController : Controller
    {
        //Cache supported
        public ActionResult Favorite()
        {
            string distId = Session["userId"].ToString();
            var objList = new List<WO_ITEMS>();

            if (ApplData.CHACHE_ENABLED)
            {
                objList = HttpContext.Cache.Get(distId + "chFav") as List<WO_ITEMS>;
            }
            if (objList == null || objList.Count == 0)
            {
                Tuple<List<WO_ITEMS>, EQResult> _tpl = ProductService.getFavoriteProductsByDistid(distId);
                if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
                {
                    objList = _tpl.Item1;
                    if (ApplData.CHACHE_ENABLED)
                    {
                        HttpContext.Cache.Insert(distId + "chFav", objList, null, DateTime.Now.AddMinutes(ApplData.CHACHE_TIME), Cache.NoSlidingExpiration);
                    }
                }
                else
                {
                    TempData["mesg"] = SweetMessages.Info("No Favorite products found");
                }
            }
            return View(objList);
        }
        //Cache supported
        public ActionResult Categories()
        {
            string distId = Session["userId"].ToString();
            var objList = new List<WO_ITEM_TYPE>();

            if (ApplData.CHACHE_ENABLED)
            {
                objList = HttpContext.Cache.Get(distId + "chCat") as List<WO_ITEM_TYPE>;
            }
            if (objList == null || objList.Count==0)
            {
                Tuple<List<WO_ITEM_TYPE>, EQResult> _tpl = ProductService.getCategoryListByDistId(distId);
                if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
                {
                    objList = _tpl.Item1;
                    if (ApplData.CHACHE_ENABLED)
                    {
                        HttpContext.Cache.Insert(distId + "chCat", objList, null, DateTime.Now.AddMinutes(ApplData.CHACHE_TIME), Cache.NoSlidingExpiration);
                    }
                }
                else
                {
                    TempData["mesg"] = SweetMessages.Info("No Products Category found");
                }
            }
            return View(objList);
        }
        //Cache supported
        public ActionResult ProductClass(string id)
        {
            if (Request.UrlReferrer == null)
            {
                TempData["mesg"] = SweetMessages.Failed("What a joke!");
                return RedirectToAction(nameof(Categories));
            }

            string distId = Session["userId"].ToString();
            CLASS_CATEGORY objList = new CLASS_CATEGORY();
            var catList = new List<WO_ITEM_TYPE>();
            var clsList = new List<WO_ITEM_CLASS>();
            if (ApplData.CHACHE_ENABLED)
            {
                catList = HttpContext.Cache.Get(distId + "chCat") as List<WO_ITEM_TYPE>;
                clsList = HttpContext.Cache.Get(distId + id) as List<WO_ITEM_CLASS>;
            }

            if (catList == null ||catList.Count==0)
            {
                Tuple<List<WO_ITEM_TYPE>, EQResult> _tpl = ProductService.getCategoryListByDistId(distId);
                if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
                {
                    catList = _tpl.Item1;
                    if (ApplData.CHACHE_ENABLED)
                    {
                        HttpContext.Cache.Insert(distId + "chCat", objList, null, DateTime.Now.AddMinutes(ApplData.CHACHE_TIME), Cache.NoSlidingExpiration);
                    }
                }
                else
                {
                    TempData["mesg"] = SweetMessages.Info("No Products Category found");
                }
            }
            if (clsList == null|| clsList.Count==0)
            {
                //Get Classes by Category Id
                Tuple<List<WO_ITEM_CLASS>, EQResult> _tpl = ProductService.getClassByCategoryId(distId, id);
                if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
                {
                    clsList = _tpl.Item1;
                    if (ApplData.CHACHE_ENABLED)
                    {
                        HttpContext.Cache.Insert(distId + id, clsList, null, DateTime.Now.AddMinutes(ApplData.CHACHE_TIME), Cache.NoSlidingExpiration);
                    }
                }
                else
                {
                    TempData["mesg"] = SweetMessages.Info("No Products Class found");
                }
            }

            //assign cat and class
            objList.CATEGORIES = catList;
            objList.CLASS = clsList;
            return View(objList);
        }
        //Cache not supported
        public ActionResult Products(string className, string catName)
        {
            if (Request.UrlReferrer == null)
            {
                TempData["mesg"] = SweetMessages.Failed("What a joke!");
                return RedirectToAction(nameof(Categories));
            }
            //Create Link for Back Button
            ViewBag.BackPages = Request.UrlReferrer.ToString();

            string distId = Session["userId"].ToString();

            Tuple<List<WO_ITEMS>, EQResult> _tpl = ProductService.getProductsByClassId(className, catName, distId);
            if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
            {
                return View(_tpl.Item1);
            }
            else
            {
                TempData["mesg"] = SweetMessages.Info("No Products found");
                return View(new List<WO_ITEMS>());
            }
        }


        public ActionResult AddToCart(string id, string qt)
        {
            if (Request.UrlReferrer == null)
            {
                TempData["mesg"] = SweetMessages.Failed("What a joke!");
                return RedirectToAction(nameof(Categories));
            }

            string distId = Session["userId"].ToString();
            EQResult result = ProductService.AddToCart(distId, id, qt, "", "");

            var rslt = new ALERT_MESG
            {
                success = result.SUCCESS,
                messages = result.SUCCESS == true ? $"Product: {id} Qty: {qt} added to cart" : "Product added failed, try again!"
            };
            return Json(rslt);
        }




        public ActionResult AddToFav(string id)
        {
            string distId = Session["userId"].ToString();
            EQResult result = ProductService.AddToFav(distId, id);

            var rslt = new ALERT_MESG
            {
                success = result.SUCCESS,
                messages = result.ROWS == 1 ? $"Product: {id} added to Favorite" : "Product already added, try another product!"
            };
            return Json(rslt);
        }
        public ActionResult DelFromFav(string id)
        {
            string distId = Session["userId"].ToString();
            EQResult result = ProductService.DelFromFav(distId, id);

            var rslt = new ALERT_MESG
            {
                success = result.SUCCESS,
                messages = result.ROWS == 1 ? $"Product: {id} removed from Favorite" : "Product is already removed!"
            };
            return Json(rslt);
        }
    }
}


//create variable for product Class
//Dictionary<string, List<WO_ITEM_CLASS>> dynVar = new Dictionary<string, List<WO_ITEM_CLASS>>();
//dynVar.Add(id, HttpContext.Cache.Get(distId + id) as List<WO_ITEM_CLASS>);
//clsList = dynVar?.FirstOrDefault().Value;