using Aio.Model;
using PriOrder.App.Models;
using PriOrder.App.Services;
using PriOrder.App.Utility;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PriOrder.App.Controllers
{
    public class MessagesController : Controller
    {
        private string distId;

        // PUSH messages
        public ActionResult Sms()
        {
            string distId = Session["userId"].ToString();
            var objList = MessageService.getSms(distId);
            if (objList.Count < 1)
            {
                TempData["mesg"] = SweetMessages.Info("No SMS found");
            }
            return View(objList);
        }
        //New Support
        public ActionResult SupportRequest()
        {
            ViewBag.CTYP_TYPE = MessageService.getSupportCategory();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SupportRequest(WO_SUP_MSG obj)
        {
            if (ModelState.IsValid)
            {
                string distId = Session["userId"].ToString();
                string result = MessageService.AddNewSupport(obj.CTYP_TYPE, distId, obj.SUP_NUMBER);
                if (result != "0")
                {
                    TempData["mesg"] = SweetMessages.Info($"Your Request sent to respective concern. NO#{result}");
                    return RedirectToAction(nameof(Support));
                }
            }
            TempData["mesg"] = SweetMessages.Failed($"New support request sending failed, try again");
            ViewBag.CTYP_TYPE = MessageService.getSupportCategory();
            return View(obj);
        }

        //All Suppport
        public ActionResult Support()
        {
            string distId = Session["userId"].ToString();
            var objList = new List<WO_SUP_MSG>();
            Tuple<List<WO_SUP_MSG>, EQResult> _tpl = MessageService.getSupport(distId);
            if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
            {
                objList = _tpl.Item1;
            }
            else
            {
                TempData["mesg"] = SweetMessages.Info("No Support Messages found");
            }
            return View(objList);
        }
        public ActionResult SupportReply(string replyId)
        {
            if (Request.UrlReferrer == null)
            {
                TempData["mesg"] = SweetMessages.Failed("This Support number is closed!");
                return RedirectToAction(nameof(Support));
            }
            var objList = new List<WO_SUP_MSG_BODY>();
            Tuple<List<WO_SUP_MSG_BODY>, EQResult> _tpl = MessageService.getSupportBody(replyId);
            if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
            {
                objList = _tpl.Item1;
            }
            else
            {
                TempData["mesg"] = SweetMessages.Info("No Support Reply found");
            }
            return View(objList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SupportReply(string replyId, string messagesText)
        {
            if (string.IsNullOrWhiteSpace(replyId) || string.IsNullOrWhiteSpace(messagesText) || messagesText.Length < 2)
            {
                TempData["mesg"] = SweetMessages.Failed("Enter valid text Messages");
            }
            else
            {
                EQResult result = MessageService.SupportReply(replyId, messagesText);
                if (result.SUCCESS && result.ROWS == 1)
                {
                    TempData["mesg"] = SweetMessages.Success("Messages sent");
                }
            }
            return RedirectToAction(nameof(SupportReply), new { replyId = replyId });
        }


        //--------------------------------Back Office--------------------------------------------//

        public ActionResult Feedback()
        {
            string distId = Session["userId"].ToString();
            var objList = new List<WO_SUP_MSG>();
            Tuple<List<WO_SUP_MSG>, EQResult> _tpl = MessageService.getFeedback(distId);
            if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
            {
                objList = _tpl.Item1;
            }
            else
            {
                TempData["mesg"] = SweetMessages.Info("No pending Feedback messages found");
            }
            return View(objList);
        }
        public ActionResult FeedbackReply(string replyId)
        {
            if (Request.UrlReferrer == null)
            {
                TempData["mesg"] = SweetMessages.Failed("This Support number is closed!");
                return RedirectToAction(nameof(Feedback));
            }
            var objList = new List<WO_SUP_MSG_BODY>();
            Tuple<List<WO_SUP_MSG_BODY>, EQResult> _tpl = MessageService.getSupportBody(replyId);
            if (_tpl.Item2.SUCCESS && _tpl.Item2.ROWS > 0)
            {
                objList = _tpl.Item1;
            }
            else
            {
                TempData["mesg"] = SweetMessages.Info("No Feedback Reply found");
            }
            return View(objList);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FeedbackReply(SUP_MSG_REPL obj)
        {
            string suppName = Session["userId"].ToString();

            if (ModelState.IsValid && obj.messagesText.Length > 2)
            {
                EQResult result = MessageService.FeedbackReply(obj, suppName);
                if (result.SUCCESS && result.ROWS == 1)
                {
                    TempData["mesg"] = SweetMessages.Success("Messages sent");
                }
                if (result.SUCCESS && result.ROWS == 2)
                {
                    TempData["mesg"] = SweetMessages.Success("Messages sent with Closed");
                }
            }
            else
            {
                TempData["mesg"] = SweetMessages.Failed("Enter valid text Messages");
            }
            return RedirectToAction(nameof(FeedbackReply), new { replyId = obj.replyId });
        }
    }
}