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
    public class ProductsController : Controller
    {
        // GET: Products
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Categories()
        {
            string groupId = ""; //Session["userGroup"];
            Tuple<List<WO_ITEM_CATEGORY>, string> _tpl = ProductService.getCategoryListByDistGroup(groupId);
            if (_tpl.Item2 == AppKeys.PostSuccess)
            {
                return View(_tpl.Item1);
            }
            else
            {
                ViewBag.ErrorMessages = "No Category found";
                return View(new List<WO_ITEM_CATEGORY>());
            }
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

            Tuple<List<T_ITMA>, string> _tpl = ProductService.getProductsByClassId(id);
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

        public ActionResult AddCart(string id,string qt)
        {
            string distId = "123"; //Session["userId"];
            string result = ProductService.AddToCart(distId, id, qt, "", "");
            ViewBag.ErrorMessages = result;
            return View();
        }

    }
}