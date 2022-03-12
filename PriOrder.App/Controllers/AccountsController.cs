using Aio.Model;
using PriOrder.App.DataModels;
using PriOrder.App.Models;
using PriOrder.App.ModelsView;
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
    public class AccountsController : Controller
    {
        public ActionResult MyProfile()
        {
            string distId = Session["userId"].ToString();
            var obj = new T_DSMA();

            if (ApplData.CHACHE_ENABLED)
            {
                obj = HttpContext.Cache.Get(distId + "chProfile") as T_DSMA;
            }
            if (obj == null || obj.DSMA_DSID == null)
            {
                Tuple<List<T_DSMA>, EQResult> _tpl = AccountService.getDistProfile(distId);
                if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
                {
                    obj = _tpl.Item1.FirstOrDefault();
                    if (ApplData.CHACHE_ENABLED)
                    {
                        HttpContext.Cache.Insert(distId + "chProfile", obj, null, DateTime.Now.AddMinutes(ApplData.CHACHE_TIME), Cache.NoSlidingExpiration);
                    }
                }
                else
                {
                    TempData["mesg"] = SweetMessages.Failed("No Profile information found");
                }
            }
            return View(obj);
        }

        public ActionResult Banks()
        {
            Tuple<List<T_BANKCOL>, EQResult> _tpl = AccountService.getBankList();
            if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
            {
                return View(_tpl.Item1);
            }
            else
            {
                TempData["mesg"] = SweetMessages.Info("Bank account list is empty");
                return View(new List<T_BANKCOL>());
            }
        }

        public ActionResult MRStatus()
        {
            string distId = Session["userId"].ToString();
            Tuple<List<MR_STATUS>, EQResult> _tpl = AccountService.getMRStatusByDistId(distId);
            if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
            {
                return View(_tpl.Item1);
            }
            else
            {
                TempData["mesg"] = SweetMessages.Info("MR list is empty");
                return View(new List<MR_STATUS>());
            }
        }


        public ActionResult BusinessPartner()
        {
            string distId = Session["userId"].ToString();
            var obj = new List<NIF_DIST>();
            Tuple<List<NIF_DIST>, EQResult> _tpl = AccountService.getMyBusiness(distId);
            if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
            {
                obj = _tpl.Item1;
            }
            else
            {
                TempData["mesg"] = SweetMessages.Info("No Partner information found");
            }
            return View(obj);
        }


        public ActionResult ChangePassword()
        {

            return View(new USER_PASSWORD());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(USER_PASSWORD obj)
        {
            string distId = Session["userId"].ToString();
            if (ModelState.IsValid)
            {
                EQResult objN = AccountService.changePassword(obj, distId);
                if (objN.SUCCESS)
                {
                    TempData["mesg"] = SweetMessages.Success(objN.MESSAGES);
                    return RedirectToAction(nameof(MyProfile));
                }
                else
                {
                    TempData["mesg"] = SweetMessages.Failed(objN.MESSAGES);
                }
            }else
            {
                TempData["mesg"] = SweetMessages.Failed("All password is required");
            }
            return View(obj);
        }

        public ActionResult ChangeProfilePicture()
        {
            string distId = Session["userId"].ToString();
            USER_LOGIN obj = new USER_LOGIN();
            obj.USER_ID = distId;
            obj.USER_IMAGE = null;
            return View(obj);
        }
        [HttpPost]
        public ActionResult ChangeProfilePicture(USER_LOGIN obj)
        {
            string err = "";
            if (obj.USER_ID != null && obj.USER_IMAGE != null)
            {
                if (obj.USER_IMAGE.ContentLength > 0 && obj.USER_IMAGE.ContentLength < ApplData.PRO_IMG_SIZE)
                {
                    string fileExtension = Path.GetExtension(obj.USER_IMAGE.FileName).ToLower();
                    if (fileExtension == ".jpg")
                    {
                        string filePath = "~/Images/Distributor/Profile/";
                        string serverPath = System.IO.Path.Combine(Server.MapPath(filePath), obj.USER_ID + "-.jpg");
                        obj.USER_IMAGE.SaveAs(serverPath);

                        TempData["mesg"] = SweetMessages.SuccessPop("Picture successfully updated");
                        return RedirectToAction(nameof(MyProfile));
                    }
                    else
                    {
                        err = "File type must be JPEG/JPG format";
                    }
                }
                else
                {
                    err = $"File size must be less than {ApplData.PRO_IMG_SIZE}KB and 80 x 80 pixel";
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
    }
}