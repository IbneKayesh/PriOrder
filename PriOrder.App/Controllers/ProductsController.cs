using Aio.Model;
using PriOrder.App.DataModels;
using PriOrder.App.Models;
using PriOrder.App.ModelsView;
using PriOrder.App.Services;
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
        // GET: Products
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Favorite()
        {
            return View();
        }
        public ActionResult Categories()
        {
            var chObjList = HttpContext.Cache.Get("chCategories") as List<WO_ITEM_CATEGORY>;
            if (chObjList == null)
            {
                string groupId = "";
                //Session["userGroup"];
                //Tuple<List<WO_ITEM_CATEGORY>, string> _tpl = ProductService.getCategoryListByDistGroup(groupId);
                Tuple<List<WO_ITEM_CATEGORY>, string> _tpl = ProductService.getCategoryListByDistGroup_TEMP(groupId);
                if (_tpl.Item2 == AppKeys.PostSuccess)
                {
                    chObjList = _tpl.Item1;
                    HttpContext.Cache.Insert("chCategories", chObjList, null, DateTime.Now.AddMinutes(ApplData.CHACHE_TIME), Cache.NoSlidingExpiration);
                }
                else
                {
                    ViewBag.ErrorMessages = "No Category found";
                    return View(new List<WO_ITEM_CATEGORY>());
                }
            }
            return View(chObjList);
        }


        public ActionResult ProductClass(string id)
        {
            CLASS_CATEGORY objList = new CLASS_CATEGORY();

            //Get Categories
            string groupId = ""; //Session["userGroup"];
            Tuple<List<WO_ITEM_CATEGORY>, string> _tpl_cat = ProductService.getCategoryListByDistGroup(groupId);
            if (_tpl_cat.Item2 == AppKeys.PostSuccess)
            {
                objList.CATEGORIES = _tpl_cat.Item1;
            }
            //Get Classes by Category Id
            Tuple<List<WO_ITEM_CLASS>, string> _tpl = ProductService.getClassByCategoryId(id);
            if (_tpl.Item2 == AppKeys.PostSuccess)
            {
                objList.CLASS = _tpl.Item1;
            }
            else
            {
                objList.CLASS = new List<WO_ITEM_CLASS>();
                ViewBag.ErrorMessages = "No Class found";
            }
            return View(objList);
        }


        public ActionResult Products(string id)
        {
            if (Request.UrlReferrer != null)
            {
                ViewBag.BackPages = Request.UrlReferrer.ToString();
            }

            //Tuple<List<T_ITMA>, string> _tpl = ProductService.getProductsByClassId(id);
            Tuple<List<T_ITMA>, string> _tpl = ProductService.getProductsByClassId_TEMP(id);
            if (_tpl.Item2 == AppKeys.PostSuccess)
            {
                return View(_tpl.Item1);
            }
            else
            {
                ViewBag.ErrorMessages = "No Items found";
                return View(new List<T_ITMA>());
            }
        }

        public ActionResult AddToCart(string id, string qt)
        {
            string distId = "123"; //Session["userId"];
            string result = "Success";// ProductService.AddToCart(distId, id, qt, "", "");
            var rslt = new ALERT_BOX
            {
                ALERT_MESSAGES = result
            };
            return Json(rslt);
        }

    }
}