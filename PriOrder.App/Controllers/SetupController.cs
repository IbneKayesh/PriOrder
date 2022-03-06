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
            var objchCategories = HttpContext.Cache.Get("chCategories") as List<WO_ITEM_TYPE>;
            if (objchCategories == null)
            {
                Tuple<List<WO_ITEM_TYPE>, EQResult> _tpl = SetupService.getCategoryList();
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
        public ActionResult Category(string id)
        {
            Tuple<List<WO_ITEM_TYPE>, EQResult> _tpl = SetupService.getCategoryById(id);
            if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
            {
                return View(_tpl.Item1.FirstOrDefault());
            }
            else
            {
                return RedirectToAction(nameof(Categories));
            }

        }
        [HttpPost]
        public ActionResult Category(WO_ITEM_TYPE obj)
        {
            string err = "";
            if (obj.ITEM_TYPE_ID != null && obj.ITEM_TYPE_IMAGE != null)
            {
                if (obj.ITEM_TYPE_IMAGE.ContentLength > 0 && obj.ITEM_TYPE_IMAGE.ContentLength < (1024 * 50))
                {
                    string fileExtension = Path.GetExtension(obj.ITEM_TYPE_IMAGE.FileName).ToLower();
                    if (fileExtension == ".jpg")
                    {
                        string filePath = "~/Images/Category/";
                        string serverPath = System.IO.Path.Combine(Server.MapPath(filePath), obj.ITEM_TYPE_ID + "-.jpg");
                        obj.ITEM_TYPE_IMAGE.SaveAs(serverPath);
                        return RedirectToAction(nameof(Categories));
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
            return View();
        }





        public ActionResult ProductClass(string id)
        {
            List<WO_ITEM_CLASS> objProdClass = new List<WO_ITEM_CLASS>();
            //create variable for product Class
            Dictionary<string, List<WO_ITEM_CLASS>> dynVar = new Dictionary<string, List<WO_ITEM_CLASS>>();
            dynVar.Add(id, HttpContext.Cache.Get(id) as List<WO_ITEM_CLASS>);

            if (dynVar.FirstOrDefault().Value == null)
            {
                //Get Classes by Category Id
                Tuple<List<WO_ITEM_CLASS>, EQResult> _tpl = SetupService.getClassByCategoryId(id);
                if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
                {
                    objProdClass = _tpl.Item1;
                    HttpContext.Cache.Insert(dynVar.FirstOrDefault().Key, _tpl.Item1, null, DateTime.Now.AddMinutes(ApplData.CHACHE_TIME), Cache.NoSlidingExpiration);
                }
                else
                {
                    objProdClass = new List<WO_ITEM_CLASS>();
                    ViewBag.ErrorMessages = "No Class found";
                }
            }
            else
            {
                objProdClass = dynVar.FirstOrDefault().Value;
            }
            return View(objProdClass);
        }


        public ActionResult Class(string id)
        {
            Tuple<List<WO_ITEM_CLASS>, EQResult> _tpl = SetupService.getClassById(id);
            if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
            {
                return View(_tpl.Item1.FirstOrDefault());
            }
            else
            {
                return RedirectToAction(nameof(Categories));
            }
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





        public ActionResult Products(string id, string catId)
        {
            Tuple<List<WO_ITEMS>, EQResult> _tpl = SetupService.getProductsByClassId(id, catId);
            if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
            {
                return View(_tpl.Item1);
            }
            else
            {
                ViewBag.ErrorMessages = "No Item found";
                return View(new List<WO_ITEMS>());
            }

        }
        public ActionResult Product(string id)
        {
            Tuple<List<WO_ITEMS>, EQResult> _tpl = SetupService.getProductById(id);
            if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
            {
                return View(_tpl.Item1.FirstOrDefault());
            }
            else
            {
                return RedirectToAction(nameof(Categories));
            }
        }

        [HttpPost]
        public ActionResult Product(WO_ITEMS obj)
        {
            string err = "";
            if (obj.ITEMS_IMAGE != null && obj.ITEM_ID != null)
            {
                if (obj.ITEMS_IMAGE.ContentLength > 0 && obj.ITEMS_IMAGE.ContentLength < (1024 * 50))
                {
                    string fileExtension = Path.GetExtension(obj.ITEMS_IMAGE.FileName).ToLower();
                    if (fileExtension == ".jpg")
                    {
                        string filePath = "~/Images/Products/";
                        string serverPath = System.IO.Path.Combine(Server.MapPath(filePath), obj.ITEM_ID + "-.jpg");
                        obj.ITEMS_IMAGE.SaveAs(serverPath);
                        return RedirectToAction(nameof(Products), new { id = obj.ITEM_ID });
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
    }
}