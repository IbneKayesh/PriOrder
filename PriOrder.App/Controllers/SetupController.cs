using Aio.Model;
using PriOrder.App.DataModels;
using PriOrder.App.Models;
using PriOrder.App.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace PriOrder.App.Controllers
{
    public class SetupController : Controller
    {
        // GET: Setup
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Categories()
        {
            //var chObjList = HttpContext.Cache.Get("chCategories") as List<WO_ITEM_TYPE>;
            //if (chObjList == null)
            //{
            //    string groupId = "";
            //    //Session["userGroup"];
            //    //Tuple<List<WO_ITEM_CATEGORY>, string> _tpl = ProductService.getCategoryListByDistGroup(groupId);
            //    Tuple<List<WO_ITEM_TYPE>, string> _tpl = ProductService.getCategoryListByDistGroup_TEMP(groupId);
            //    if (_tpl.Item2 == AppKeys.PostSuccess)
            //    {
            //        chObjList = _tpl.Item1;
            //        HttpContext.Cache.Insert("chCategories", chObjList, null, DateTime.Now.AddMinutes(ApplData.CHACHE_TIME), Cache.NoSlidingExpiration);
            //    }
            //    else
            //    {
            //        ViewBag.ErrorMessages = "No Category found";
            //        return View(new List<WO_ITEM_TYPE>());
            //    }
            //}
            return View();
        }
        public ActionResult Category(string id)
        {
            //WO_ITEM_TYPE obj = new WO_ITEM_TYPE();
            //obj.CATEGORY_ID = id;
            //obj.CATEGORY_NAME = "Cat 1";

            return View();
        }
        [HttpPost]
        public ActionResult Category(WO_ITEM_TYPE obj)
        {
            //string err = "";
            //if (obj.CATEGORY_IMAGE != null && obj.CATEGORY_ID != null)
            //{
            //    if (obj.CATEGORY_IMAGE.ContentLength > 0 && obj.CATEGORY_IMAGE.ContentLength < (1024 * 50))
            //    {
            //        string fileExtension = Path.GetExtension(obj.CATEGORY_IMAGE.FileName).ToLower();
            //        if (fileExtension == ".jpg")
            //        {
            //            string filePath = "~/Images/Category/";
            //            string serverPath = System.IO.Path.Combine(Server.MapPath(filePath), obj.CATEGORY_ID + "-.jpg");
            //            obj.CATEGORY_IMAGE.SaveAs(serverPath);
            //            return RedirectToAction(nameof(Categories));
            //        }
            //        else
            //        {
            //            err = "File type must be JPEG/JPG format";
            //        }
            //    }
            //    else
            //    {
            //        err = "File size must be less than 8KB and 80 x 80 pixel";
            //    }
            //}
            //else
            //{
            //    err = "Invalid file";
            //}
            //ModelState.AddModelError("", errorMessage: err);
            return View();
        }





        public ActionResult ProductClass(string id)
        {
            List<WO_ITEM_CLASS> objList = new List<WO_ITEM_CLASS>();

            Tuple<List<WO_ITEM_CLASS>, string> _tpl = ProductService.getClassByCategoryId_TEMP(id);
            if (_tpl.Item2 == AppKeys.PostSuccess)
            {
                objList = _tpl.Item1;
            }
            else
            {
                objList = new List<WO_ITEM_CLASS>();
                ViewBag.ErrorMessages = "No Class found";
            }
            return View(objList);
        }


        public ActionResult Class(string id)
        {
            WO_ITEM_CLASS obj = new WO_ITEM_CLASS();
            obj.ITEM_CLASS_ID = id;
            return View(obj);
        }

        [HttpPost]
        public ActionResult Class(WO_ITEM_CLASS obj)
        {
            string err = "";
            if (obj.ITEM_CLASS_IMAGE != null && obj.ITEM_CLASS_ID != null)
            {
                if (obj.ITEM_CLASS_IMAGE.ContentLength > 0 && obj.ITEM_CLASS_IMAGE.ContentLength < (1024 * 50))
                {
                    string fileExtension = Path.GetExtension(obj.ITEM_CLASS_IMAGE.FileName).ToLower();
                    if (fileExtension == ".jpg")
                    {
                        string filePath = "~/Images/ItemClass/";
                        string serverPath = System.IO.Path.Combine(Server.MapPath(filePath), obj.ITEM_CLASS_ID + "-.jpg");
                        obj.ITEM_CLASS_IMAGE.SaveAs(serverPath);
                        return RedirectToAction(nameof(ProductClass), new { id = obj.ITEM_TYPE_ID });
                    }
                    else
                    {
                        err = "File type must be JPEG/JPG format";
                    }
                }
                else
                {
                    err = "File size must be less than 8KB and 80 x 80 pixel";
                }
            }
            else
            {
                err = "Invalid file";
            }
            ModelState.AddModelError("", errorMessage: err);
            return View(obj);
        }





        public ActionResult Products(string id)
        {
            //    List<WO_ITEMS> objList = new List<WO_ITEMS>();

            //    Tuple<List<WO_ITEMS>, string> _tpl = ProductService.getProductsByClassId_TEMP(id);
            //    if (_tpl.Item2 == AppKeys.PostSuccess)
            //    {
            //        objList = _tpl.Item1;
            //    }
            //    else
            //    {
            //        objList = new List<WO_ITEMS>();
            //        ViewBag.ErrorMessages = "No Item found";
            //    }
            //    return View(objList);
            return View();

        }
        public ActionResult Product(string id)
        {
            //WO_ITEMS obj = new WO_ITEMS();
            //obj.ITMA_ITID = id;
            //return View(obj);
            return View();
        }

        [HttpPost]
        public ActionResult Product(WO_ITEMS obj)
        {
            //    string err = "";
            //    if (obj.ITMA_IMGE != null && obj.ITMA_ITID != null)
            //    {
            //        if (obj.ITMA_IMGE.ContentLength > 0 && obj.ITMA_IMGE.ContentLength < (1024 * 50))
            //        {
            //            string fileExtension = Path.GetExtension(obj.ITMA_IMGE.FileName).ToLower();
            //            if (fileExtension == ".jpg")
            //            {
            //                string filePath = "~/Images/Products/";
            //                string serverPath = System.IO.Path.Combine(Server.MapPath(filePath), obj.ITMA_ITID + "-.jpg");
            //                obj.ITMA_IMGE.SaveAs(serverPath);
            //                return RedirectToAction(nameof(Products), new { id = obj.ITMA_CLASS });
            //            }
            //            else
            //            {
            //                err = "File type must be JPEG/JPG format";
            //            }
            //        }
            //        else
            //        {
            //            err = "File size must be less than 8KB and 80 x 80 pixel";
            //        }
            //    }
            //    else
            //    {
            //        err = "Invalid file";
            //    }
            //    ModelState.AddModelError("", errorMessage: err);
            //    return View(obj);
            return View();
        }
    }
}