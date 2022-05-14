using Aio.Model;
using PriOrder.App.Models;
using PriOrder.App.Services;
using PriOrder.App.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PriOrder.App.Controllers
{
    public class NFController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Apply()
        {
            string distId = Session["userId"].ToString();
            Tuple<List<NIF_DIST>, EQResult> _tpl = NFService.getNIF_DIST(distId);
            if (_tpl.Item2.ROWS > 0)
            {
                TempData["mesg"] = SweetMessages.Info("You already applied!");
                return RedirectToAction("Index");
            }
            DropdownFor_Apply();
            NIF_APPL obj = new NIF_APPL();
            return View(obj);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Apply(NIF_APPL obj)
        {
            if (obj.AS_PRESENT == 1)
            {
                obj.HOUSE_ROAD2 = obj.HOUSE_ROAD;
                obj.VILLAGE_NAME2 = obj.VILLAGE_NAME;
                obj.UNION_NAME2 = obj.UNION_NAME;
                obj.POLICE_STATION2 = obj.POLICE_STATION;
                obj.DISTRICT2 = obj.DISTRICT;

                //Re-IsValid
                ModelState.Clear();
            }
            //ModelState["obj.HOUSE_ROAD2"].Errors.Clear();
            //ModelState["obj.VILLAGE_NAME2"].Errors.Clear();
            //ModelState["obj.HOUSE_ROAD2"].Errors.Clear();
            //ModelState["obj.POLICE_STATION2"].Errors.Clear();
            //ModelState["obj.DISTRICT2"].Errors.Clear();
            //UpdateModel(obj);
            if (ModelState.IsValid)
            {
                string distId = Session["userId"].ToString();
                EQResult result = NFService.ApplicationCreate(obj, distId);
                if (result.SUCCESS)
                {
                    TempData["mesg"] = SweetMessages.Success("Your is request Submitted!");
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["mesg"] = SweetMessages.Info("You already applied!");
                }
            }
            DropdownFor_Apply();
            return View(obj);
        }

        private void DropdownFor_Apply()
        {
            Tuple<List<T_DTDM>, EQResult> objList = NFService.getDistrict();
            if (objList.Item2.SUCCESS && objList.Item2.ROWS > 0)
            {
                ViewBag.DISTRICT = new SelectList(objList.Item1, "DTDM_TEXT", "DTDM_NAME");
                ViewBag.DISTRICT2 = new SelectList(objList.Item1, "DTDM_TEXT", "DTDM_NAME");
            }
            else
            {
                ViewBag.DISTRICT = new SelectList(Enumerable.Empty<SelectListItem>());
                ViewBag.DISTRICT2 = new SelectList(Enumerable.Empty<SelectListItem>());
            }
            ViewBag.POLICE_STATION = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.POLICE_STATION2 = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.UNION_NAME = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.UNION_NAME2 = new SelectList(Enumerable.Empty<SelectListItem>());
        }
        [AllowAnonymous]
        public ActionResult DropDownFor_Thana(string district)
        {
            Tuple<List<T_DTNM>, EQResult> objList = NFService.getThanaByDistrId(district);
            if (objList.Item2.SUCCESS && objList.Item2.ROWS > 0)
            {
                var obj = new SelectList(objList.Item1, "DTNM_TEXT", "DTNM_NAME");
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
        [AllowAnonymous]
        public ActionResult DropDownFor_Union(string thana)
        {
            Tuple<List<T_DITHUN>, EQResult> objList = NFService.getUnionByThanaId(thana);
            if (objList.Item2.SUCCESS && objList.Item2.ROWS > 0)
            {
                var obj = new SelectList(objList.Item1, "UNION_TEXT", "UNION_NAME");
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult TandC()
        {
            return View();
        }

        public ActionResult MyDistributors()
        {
            string distId = Session["userId"].ToString();
            var obj = new List<NIF_DIST>();
            Tuple<List<NIF_DIST>, EQResult> _tpl = AccountService.getMyBusiness(distId);
            if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
            {
                ViewBag.dist_nid = _tpl.Item1.First().APPL_NID;
                obj = _tpl.Item1;
            }
            else
            {
                TempData["mesg"] = SweetMessages.Info("No Distributor information found");
            }            
            return View(obj);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult MyDistributors(string dist_nid, string dist_id)
        {
            if (string.IsNullOrWhiteSpace(dist_nid) || string.IsNullOrWhiteSpace(dist_id))
            {
                TempData["mesg"] = SweetMessages.Info("No Distributor information found");
                return RedirectToAction("MyDistributors");
            }
            EQResult result = NFService.AddDistributor(dist_nid, dist_id);
            if (result.SUCCESS)
            {
                TempData["mesg"] = SweetMessages.Success("Your is request Submitted!");
                return RedirectToAction("Index");
            }
            else
            {
                TempData["mesg"] = SweetMessages.Info("You already applied!");
            }
            return RedirectToAction("MyDistributors");
        }

        
        public ActionResult MyNominee()
        {
            string distId = Session["userId"].ToString();
            var obj = new NIF_NOMI();
            Tuple<List<NIF_DIST>, EQResult> _tpl = AccountService.getMyBusiness(distId);
            if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
            {
                obj.APPL_NID = _tpl.Item1.First().APPL_NID;
            }
            else
            {
                TempData["mesg"] = SweetMessages.Info("No Application found, Please apply first");
                return RedirectToAction("Index");
            }

            DropdownFor_MyNominee();
            return View(obj);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult MyNominee(NIF_NOMI obj)
        {
            if (ModelState.IsValid)
            {
                string distId = Session["userId"].ToString();
                EQResult result = NFService.AddNominee(obj, distId);
                if (result.SUCCESS && result.ROWS==1)
                {
                    TempData["mesg"] = SweetMessages.Success("Your is request Submitted!");
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["mesg"] = SweetMessages.Info("You already applied!");
                }
            }

            DropdownFor_MyNominee();
            return View(obj);
        }
        private void DropdownFor_MyNominee()
        {
            Tuple<List<T_DTDM>, EQResult> objList = NFService.getDistrict();
            if (objList.Item2.SUCCESS && objList.Item2.ROWS > 0)
            {
                ViewBag.DISTRICT = new SelectList(objList.Item1, "DTDM_TEXT", "DTDM_NAME");
            }
            else
            {
                ViewBag.DISTRICT = new SelectList(Enumerable.Empty<SelectListItem>());
            }
            ViewBag.POLICE_STATION = new SelectList(Enumerable.Empty<SelectListItem>());
            ViewBag.UNION_NAME = new SelectList(Enumerable.Empty<SelectListItem>());

            ViewBag.NOMI_ID = getNomineeNumber();
        }
        public static List<SelectListItem> getNomineeNumber()
        {
            var objList = new List<SelectListItem>
              {
                new SelectListItem { Selected = true, Text = "First", Value ="1"},
                new SelectListItem { Selected = false, Text = "Second", Value = "2"},
            };
            return objList;
        }
    }
}