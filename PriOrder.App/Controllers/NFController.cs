using Aio.Model;
using ClosedXML.Excel;
using PriOrder.App.DataModels;
using PriOrder.App.Models;
using PriOrder.App.Services;
using PriOrder.App.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PriOrder.App.Controllers
{
    [AioAuthorization]
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
                    TempData["mesg"] = SweetMessages.Success("Your request is Submitted!");
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
                return RedirectToAction("MyDistributors");
            }
            else
            {
                TempData["mesg"] = SweetMessages.Info("You already applied!");
            }
            return RedirectToAction("MyDistributors");
        }


        public ActionResult AddNominee()
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
            DropdownFor_AddNominee();
            return View(obj);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddNominee(NIF_NOMI obj)
        {
            if (ModelState.IsValid)
            {
                string distId = Session["userId"].ToString();
                EQResult result = NFService.AddNominee(obj, distId);
                if (result.SUCCESS && result.ROWS == 1)
                {
                    TempData["mesg"] = SweetMessages.Success("Your is request Submitted!");
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["mesg"] = SweetMessages.Info("You already applied!");
                }
            }

            DropdownFor_AddNominee();
            return View(obj);
        }
        private void DropdownFor_AddNominee()
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
        public List<SelectListItem> getNomineeNumber()
        {
            string distId = Session["userId"].ToString();
            Tuple<List<NIF_NOMI>, EQResult> objList1 = NFService.getNomineeNumber(distId);
            if (objList1.Item2.ROWS == 0)
            {
                var objList = new List<SelectListItem>
                {
                new SelectListItem { Selected = true, Text = "Nominee 1", Value ="1"},
                new SelectListItem { Selected = false, Text = "Nominee 2", Value = "2"},
                };
                return objList;
            }
            else if (objList1.Item2.ROWS == 1)
            {
                int NomNo = objList1.Item1.First().NOMI_ID == 1 ? 2 : 1;

                var objList = new List<SelectListItem>
                {
                new SelectListItem { Selected = true, Text = $@"Nominee {NomNo}", Value =$"{NomNo}"}
                };
                return objList;
            }
            else
            {
                return new List<SelectListItem>();
            }
        }


        public ActionResult ViewApplication()
        {
            string distId = Session["userId"].ToString();
            var obj = NFService.ViewAppl(distId);
            if (obj == null || obj.NIF_APPL == null)
            {
                TempData["mesg"] = SweetMessages.Info("No Application found, Please apply first");
                return RedirectToAction("Index");
            }
            return View(obj);
        }


        public ActionResult UploadImage()
        {
            string distId = Session["userId"].ToString();
            var obj = new NIF_IMGS();
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
            DropdownFor_UploadImage();
            return View(obj);
        }

        [HttpPost]
        public ActionResult UploadImage(NIF_IMGS obj)
        {
            DropdownFor_UploadImage();
            string err = "Session expired! Try with new Login";
            if (ModelState.IsValid)
            {
                if (obj.ITEM_IMAGE_TYPE != 0 && obj.ITEM_IMAGE != null)
                {
                    if (obj.ITEM_IMAGE.ContentLength > 0 && obj.ITEM_IMAGE.ContentLength < ApplData.NF_IMG_SIZE)
                    {
                        string dirName = "", filName = "";
                        if (obj.ITEM_IMAGE_TYPE == 1 || obj.ITEM_IMAGE_TYPE == 3 || obj.ITEM_IMAGE_TYPE == 5)
                        {
                            dirName = "NFNID";
                            filName = "APPNID";
                            if (obj.ITEM_IMAGE_TYPE == 3)
                            {
                                filName = "NOM1NID";
                            }
                            else if (obj.ITEM_IMAGE_TYPE == 5)
                            {
                                filName = "NOM2NID";
                            }
                        }
                        else if (obj.ITEM_IMAGE_TYPE == 2 || obj.ITEM_IMAGE_TYPE == 4 || obj.ITEM_IMAGE_TYPE == 6)
                        {
                            dirName = "NFPIC";
                            filName = "APPPIC";
                            if (obj.ITEM_IMAGE_TYPE == 4)
                            {
                                filName = "NOM1PIC";
                            }
                            else if (obj.ITEM_IMAGE_TYPE == 6)
                            {
                                filName = "NOM2PIC";
                            }
                        }
                        string fileExtension = Path.GetExtension(obj.ITEM_IMAGE.FileName).ToLower();
                        if (fileExtension == ".jpg")
                        {
                            string ExtfilePath = "~/Images/Distributor/" + dirName;
                            string filePath = System.IO.Path.Combine(Server.MapPath(ExtfilePath));
                            //create directory
                            if (!Directory.Exists(filePath))
                            {
                                Directory.CreateDirectory(filePath);
                            }
                            //delete if already exists
                            string serverPath = System.IO.Path.Combine(Server.MapPath(ExtfilePath), obj.APPL_NID + "_" + filName + ".jpg");
                            try
                            {
                                if (System.IO.File.Exists(serverPath))
                                {
                                    System.IO.File.Delete(serverPath);
                                }
                            }
                            catch (Exception ex)
                            {
                                Response.Write(ex.Message);
                            }
                            obj.ITEM_IMAGE.SaveAs(serverPath);

                            string distId = Session["userId"].ToString();
                            EQResult result = NFService.UpdatePicture(obj, distId);

                            TempData["mesg"] = SweetMessages.SuccessPop("Picture successfully updated");
                            return RedirectToAction(nameof(UploadImage));
                        }
                        else
                        {
                            err = "File type must be JPG format";
                        }
                    }
                    else
                    {
                        err = $"File size must be less than {ApplData.NF_IMG_SIZE / 1024}KB and 80 x 80 pixel";
                    }
                }
                else
                {
                    err = "Invalid file";
                }
            }
            ModelState.AddModelError("", errorMessage: err);
            TempData["mesg"] = SweetMessages.Failed(err);
            return View(obj);
        }
        private void DropdownFor_UploadImage()
        {
            ViewBag.ITEM_IMAGE_TYPE = getImageType();
        }
        public static List<SelectListItem> getImageType()
        {
            var objList = new List<SelectListItem>
              {
                new SelectListItem { Selected = false, Text = "Applicant NID", Value ="1"},
                new SelectListItem { Selected = false, Text = "Applicant Picture", Value = "2"},
                new SelectListItem { Selected = false, Text = "Nominee (1) NID", Value ="3"},
                new SelectListItem { Selected = false, Text = "Nominee (1) Picture", Value = "4"},
                new SelectListItem { Selected = false, Text = "Nominee (2) NID", Value ="5"},
                new SelectListItem { Selected = false, Text = "Nominee (2) Picture", Value = "6"},
            };
            return objList;
        }


        public ActionResult DownloadAll()
        {
            var objList = NFService.ViewAllAppl();
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(objList.Table);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= NFAll.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            return RedirectToAction("Index");
            //var gv = new GridView();
            //gv.DataSource = objList.Table;
            //gv.DataBind();
            //Response.ClearContent();
            //Response.Buffer = true;
            //Response.AddHeader("content-disposition", "attachment; filename=NFApplicationListAll.xlsx");
            //Response.ContentType = "application/ms-excel";
            //Response.Charset = "";
            //StringWriter objStringWriter = new StringWriter();
            //HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            //gv.RenderControl(objHtmlTextWriter);
            //Response.Output.Write(objStringWriter.ToString());
            //Response.Flush();
            //Response.End();

        }
    }
}