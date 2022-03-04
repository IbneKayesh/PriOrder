using Aio.Model;
using PriOrder.App.DataModels;
using PriOrder.App.Models;
using PriOrder.App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace PriOrder.App.Areas.BackOffice.Controllers
{
    [RouteArea("BackOffice")]
    public class SetupController : Controller
    {
        // GET: BackOffice/Setup
        public ActionResult Index()
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
        public ActionResult Category(string id)
        {
            WO_ITEM_CATEGORY obj = new WO_ITEM_CATEGORY();
            obj.CATEGORY_ID = "Cat1";
            obj.CATEGORY_NAME = "Cat 1";

            return View(obj);
        }
        [HttpPost]
        public ActionResult Category()
        {
            return View();
        }
    }
}