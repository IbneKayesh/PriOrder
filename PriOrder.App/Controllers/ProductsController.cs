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
            var objchCategories = HttpContext.Cache.Get("chCategories") as List<WO_ITEM_TYPE>;
            if (objchCategories == null)
            {
                string distId = Session["userId"].ToString();
                Tuple<List<WO_ITEM_TYPE>, EQResult> _tpl = ProductService.getCategoryListByDistId(distId);
                //Tuple<List<WO_ITEM_CATEGORY>, string> _tpl = ProductService.getCategoryListByDistGroup_TEMP(groupId);
                if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
                {
                    objchCategories = _tpl.Item1;
                    HttpContext.Cache.Insert("chCategories", objchCategories, null, DateTime.Now.AddMinutes(ApplData.CHACHE_TIME), Cache.NoSlidingExpiration);
                }
                else
                {
                    ViewBag.ErrorMessages = "No Category found";
                    return View(new List<WO_ITEM_TYPE>());
                }
            }
            return View(objchCategories);
        }


        public ActionResult ProductClass(string id)
        {
            string distId = Session["userId"].ToString();
            CLASS_CATEGORY objList = new CLASS_CATEGORY();

            var objchCategories = HttpContext.Cache.Get("chCategories") as List<WO_ITEM_TYPE>;
            if (objchCategories == null)
            {
                //Get Categories

                Tuple<List<WO_ITEM_TYPE>, EQResult> _tpl_cat = ProductService.getCategoryListByDistId(distId);
                if (_tpl_cat.Item2.SUCCESS && _tpl_cat.Item2.ROWS > 0)
                {
                    objList.CATEGORIES = _tpl_cat.Item1;
                    HttpContext.Cache.Insert("chCategories", objchCategories, null, DateTime.Now.AddMinutes(ApplData.CHACHE_TIME), Cache.NoSlidingExpiration);
                }
            }
            else
            {
                objList.CATEGORIES = objchCategories;
            }

            //create variable for product Class
            Dictionary<string, List<WO_ITEM_CLASS>> dynVar = new Dictionary<string, List<WO_ITEM_CLASS>>();
            dynVar.Add(id, HttpContext.Cache.Get(id) as List<WO_ITEM_CLASS>);

            if (dynVar.FirstOrDefault().Value == null)
            {
                //Get Classes by Category Id
                Tuple<List<WO_ITEM_CLASS>, EQResult> _tpl = ProductService.getClassByCategoryId(distId, id);
                if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
                {
                    objList.CLASS = _tpl.Item1;

                    HttpContext.Cache.Insert(dynVar.FirstOrDefault().Key, _tpl.Item1, null, DateTime.Now.AddMinutes(ApplData.CHACHE_TIME), Cache.NoSlidingExpiration);
                }
                else
                {
                    objList.CLASS = new List<WO_ITEM_CLASS>();
                    ViewBag.ErrorMessages = "No Class found";
                }
            }
            else
            {
                objList.CLASS = dynVar.FirstOrDefault().Value;
            }
            return View(objList);
        }


        public ActionResult Products(string className, string catName)
        {
            if (Request.UrlReferrer != null)
            {
                ViewBag.BackPages = Request.UrlReferrer.ToString();
            }
            Tuple<List<WO_ITEMS>, EQResult> _tpl = ProductService.getProductsByClassId(className, catName);
            if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
            {
                return View(_tpl.Item1);
            }
            else
            {
                ViewBag.ErrorMessages = "No Items found";
                return View(new List<WO_ITEMS>());
            }
        }










        public ActionResult AddToCart(string id, string qt)
        {
            string distId = Session["userId"].ToString();
            EQResult result = ProductService.AddToCart(distId, id, qt, "", "");

            var rslt = new ALERT_BOX
            {
                ALERT_MESSAGES = result.MESSAGES
            };

            return Json(rslt);
        }

    }
}