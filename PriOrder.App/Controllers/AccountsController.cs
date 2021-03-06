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
    [AioAuthorization]
    public class AccountsController : Controller
    {
        public ActionResult MyProfile()
        {
            string distId = Session["userId"].ToString();

            var obj = new T_DSMA();
            obj.T_DSMA_BAL = new T_DSMA_BAL();
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
                    //Get Balance
                    //Tuple<List<T_DSMA_BAL>, EQResult> _tpl_bal = AccountService.getDistBalance(distId);
                    Tuple<List<T_DSMA_BAL>, EQResult> _tpl_bal = AccountService.getDistBalance("WAUTO", distId, "GET_BALANCE", distId, "", "");
                    if (_tpl_bal.Item2.SUCCESS && _tpl_bal.Item2.ROWS == 1)
                    {
                        obj.T_DSMA_BAL = _tpl_bal.Item1.FirstOrDefault();
                        Session["userBalnace"] = obj.T_DSMA_BAL.DBAL_ABAL;
                    }
                    //End Get Balance



                    //get Target
                    Tuple<List<T_TARGETT>, EQResult> _tpl_target = AccountService.getDistTarget(distId);
                    if (_tpl_target.Item2.SUCCESS && _tpl_target.Item2.ROWS == 1)
                    {
                        obj.T_TARGETT = _tpl_target.Item1.First();
                    }
		else
                    {
                        obj.T_TARGETT = new T_TARGETT();
                    }
                    //End Target

                    if (ApplData.CHACHE_ENABLED)
                    {
                        HttpContext.Cache.Insert(distId + "chProfile", obj, null, DateTime.Now.AddMinutes(ApplData.CHACHE_TIME), Cache.NoSlidingExpiration);
                    }
                }
                else
                {
                    TempData["mesg"] = SweetMessages.Failed("No Profile information found");
                    obj = new T_DSMA();
                    obj.T_DSMA_BAL = new T_DSMA_BAL();
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
            }
            else
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
                        string serverPath = System.IO.Path.Combine(Server.MapPath(filePath), obj.USER_ID + ".jpg");
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
                    err = $"File size must be less than {ApplData.PRO_IMG_SIZE / 1024}KB and 80 x 80 pixel";
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