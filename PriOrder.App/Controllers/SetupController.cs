using Aio.Model;
using PriOrder.App.DataModels;
using PriOrder.App.Models;
using PriOrder.App.Services;
using PriOrder.App.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace PriOrder.App.Controllers
{
    [AioAuthorization]
    public class SetupController : Controller
    {
        //Cache supported
        public ActionResult Categories()
        {
            var objList = new List<WO_ITEM_TYPE>();

            if (ApplData.CHACHE_ENABLED)
            {
                objList = HttpContext.Cache.Get("AllchCat") as List<WO_ITEM_TYPE>;
            }
            if (objList == null)
            {
                Tuple<List<WO_ITEM_TYPE>, EQResult> _tpl = SetupService.getCategoryList();
                if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
                {
                    objList = _tpl.Item1;
                    if (ApplData.CHACHE_ENABLED)
                    {
                        HttpContext.Cache.Insert("AllchCat", objList, null, DateTime.Now.AddMinutes(ApplData.CHACHE_TIME), Cache.NoSlidingExpiration);
                    }
                }
                else
                {
                    TempData["mesg"] = SweetMessages.Info("No Products Category found");
                }
            }
            return View(objList);
        }
        //Cache not supported
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
        //Cache not supported
        [HttpPost]
        public ActionResult Category(WO_ITEM_TYPE obj)
        {
            string err = "";
            if (obj.ITEM_TYPE_ID != null && obj.ITEM_TYPE_IMAGE != null)
            {
                if (obj.ITEM_TYPE_IMAGE.ContentLength > 0 && obj.ITEM_TYPE_IMAGE.ContentLength < ApplData.CAT_IMG_SIZE)
                {
                    string fileExtension = Path.GetExtension(obj.ITEM_TYPE_IMAGE.FileName).ToLower();
                    if (fileExtension == ".jpg")
                    {
                        string filePath = "~/Images/Category/";
                        string serverPath = System.IO.Path.Combine(Server.MapPath(filePath), obj.ITEM_TYPE_ID + "-.jpg");
                        obj.ITEM_TYPE_IMAGE.SaveAs(serverPath);

                        TempData["mesg"] = SweetMessages.SuccessPop("Image successfully added");
                        return RedirectToAction(nameof(Categories));
                    }
                    else
                    {
                        err = "File type must be JPEG/JPG format";
                    }
                }
                else
                {
                    err = $"File size must be less than {ApplData.CAT_IMG_SIZE}KB and 80 x 80 pixel";
                }
            }
            else
            {
                err = "Invalid file";
            }
            ModelState.AddModelError("", errorMessage: err);
            TempData["mesg"] = SweetMessages.Failed(err);
            return View();
        }
        //Cache supported
        public ActionResult ProductClass(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return RedirectToAction(nameof(Categories));
            }
            var objList = new List<WO_ITEM_CLASS>();

            if (ApplData.CHACHE_ENABLED)
            {
                objList = HttpContext.Cache.Get("Allch" + id) as List<WO_ITEM_CLASS>;
            }
            if (objList == null)
            {
                //Get Classes by Category Id
                Tuple<List<WO_ITEM_CLASS>, EQResult> _tpl = SetupService.getClassByCategoryId(id);
                if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
                {
                    objList = _tpl.Item1;
                    if (ApplData.CHACHE_ENABLED)
                    {
                        HttpContext.Cache.Insert("Allch" + id, objList, null, DateTime.Now.AddMinutes(ApplData.CHACHE_TIME), Cache.NoSlidingExpiration);
                    }
                }
                else
                {
                    TempData["mesg"] = SweetMessages.Info("No Class found");
                }
            }
            return View(objList);
        }
        //Cache not supported
        public ActionResult Class(string id, string catName)
        {
            Tuple<List<WO_ITEM_CLASS>, EQResult> _tpl = SetupService.getClassById(id, catName);
            if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
            {
                return View(_tpl.Item1.FirstOrDefault());
            }
            else
            {
                return RedirectToAction(nameof(Categories));
            }
        }
        //Cache not supported
        [HttpPost]
        public ActionResult Class(WO_ITEM_CLASS obj)
        {
            string err = "";
            if (obj.ITEM_CLASS_IMAGE != null && obj.ITEM_CLASS_ID != null)
            {
                if (obj.ITEM_CLASS_IMAGE.ContentLength > 0 && obj.ITEM_CLASS_IMAGE.ContentLength < ApplData.CLS_IMG_SIZE)
                {
                    string fileExtension = Path.GetExtension(obj.ITEM_CLASS_IMAGE.FileName).ToLower();
                    if (fileExtension == ".jpg")
                    {
                        string filePath = "~/Images/ItemClass/";
                        string serverPath = System.IO.Path.Combine(Server.MapPath(filePath), obj.ITEM_CLASS_ID + "-.jpg");
                        obj.ITEM_CLASS_IMAGE.SaveAs(serverPath);

                        TempData["mesg"] = SweetMessages.SuccessPop("Image successfully added");
                        return RedirectToAction(nameof(ProductClass), new { id = obj.ITEM_TYPE_ID });
                    }
                    else
                    {
                        err = "File type must be JPEG/JPG format";
                    }
                }
                else
                {
                    err = $"File size must be less than {ApplData.CLS_IMG_SIZE}KB and 80 x 80 pixel";
                }
            }
            else
            {
                err = "Invalid file";
            }
            ModelState.AddModelError("", errorMessage: err);
            TempData["mesg"] = SweetMessages.Failed(err);
            return View(obj);
        }
        //Cache not supported
        public ActionResult Products(string id, string catName)
        {
            //Create Link for Back Button
            ViewBag.BackPages = Request.UrlReferrer.ToString();

            Tuple<List<WO_ITEMS>, EQResult> _tpl = SetupService.getProductsByClassId(id, catName);
            if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
            {
                return View(_tpl.Item1);
            }
            else
            {
                TempData["mesg"] = SweetMessages.Info("No Item found");
                return View(new List<WO_ITEMS>());
            }
        }
        //Cache not supported
        public ActionResult Product(string id)
        {
            Tuple<List<WO_ITEMS>, EQResult> _tpl = SetupService.getProductById(id);
            if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
            {
                //Create Link for Back Button with Free Properties // Item Unit
                var singleProduct = _tpl.Item1.FirstOrDefault();
                var uri = new Uri(Request.UrlReferrer.ToString());
                singleProduct.ITEM_UNIT = uri.Segments[3] + "-" + HttpUtility.ParseQueryString(uri.Query).Get("catName");

                return View(singleProduct);
            }
            else
            {
                return RedirectToAction(nameof(Categories));
            }
        }
        //Cache not supported
        [HttpPost]
        public ActionResult Product(WO_ITEMS obj)
        {
            string err = "";
            if (obj.ITEMS_IMAGE != null && obj.ITEM_ID != null)
            {
                if (obj.ITEMS_IMAGE.ContentLength > 0 && obj.ITEMS_IMAGE.ContentLength < ApplData.ITM_IMG_SIZE)
                {
                    string fileExtension = Path.GetExtension(obj.ITEMS_IMAGE.FileName).ToLower();
                    if (fileExtension == ".jpg")
                    {
                        string filePath = "~/Images/Products/";
                        string serverPath = System.IO.Path.Combine(Server.MapPath(filePath), obj.ITEM_ID + "-.jpg");
                        obj.ITEMS_IMAGE.SaveAs(serverPath);

                        //return back to last pages
                        string[] parms = obj.ITEM_UNIT.Split('-');

                        TempData["mesg"] = SweetMessages.SuccessPop("Image successfully added");
                        return RedirectToAction("Products", new { id = parms[0], catName = parms[1] });
                    }
                    else
                    {
                        err = "File type must be JPEG/JPG format";
                    }
                }
                else
                {
                    err = $"File size must be less than {ApplData.ITM_IMG_SIZE}KB and 80 x 80 pixel";
                }
            }
            else
            {
                err = "Invalid file";
            }
            ModelState.AddModelError("", errorMessage: err);
            TempData["mesg"] = SweetMessages.Failed(err);
            return View(obj);
        }
    }
}


//create variable for product Class
//Dictionary<string, List<WO_ITEM_CLASS>> dynVar = new Dictionary<string, List<WO_ITEM_CLASS>>();
//dynVar.Add(id, HttpContext.Cache.Get(id) as List<WO_ITEM_CLASS>);