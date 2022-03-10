using PriOrder.App.Models;
using PriOrder.App.Services;
using PriOrder.App.Utility;
using System.Web.Mvc;

namespace PriOrder.App.Controllers
{
    public class MessagesController : Controller
    {
        // PUSH messages
        public ActionResult Sms()
        {
            string distId = Session["userId"].ToString();
            var objList = MessageService.getSms(distId);
            return View(objList);
        }
        //New Support
        public ActionResult SupportRequest()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SupportRequest(WO_SUP_MSG obj)
        {
            return View();
        }

        //All Suppport
        public ActionResult Support()
        {
            string distId = "1234";
            var objList = MessageService.getSupport(distId);
            return View(objList);
        }
        public ActionResult SupportReply(string replyId)
        {
            if (Request.UrlReferrer == null)
            {
                TempData["mesg"] = SweetMessages.Failed("This Support number is closed!");
                return RedirectToAction(nameof(Support));
            }
            var objList = MessageService.getSupportBody(replyId);
            return View(objList);
        }
        public ActionResult SupportReply(string replyId, string messagesText)
        {

            return View();
        }
    }
}