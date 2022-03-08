using PriOrder.App.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PriOrder.App.Services
{
    public class MessageService
    {
        public static List<WO_PUSH_MSG> getSms(string distId)
        {
            List<WO_PUSH_MSG> objList = new List<WO_PUSH_MSG>();
            objList.Add(new WO_PUSH_MSG { MSG_ID = "a", CAT_ID = "General", BODY_TEXT = "this is sample message", CREATE_DATE = DateTime.Now, IS_READ = 0 });
            objList.Add(new WO_PUSH_MSG { MSG_ID = "b", CAT_ID = "General", BODY_TEXT = "this is sample message 2", CREATE_DATE = DateTime.Now, IS_READ = 0 });
            objList.Add(new WO_PUSH_MSG { MSG_ID = "b", CAT_ID = "General", BODY_TEXT = "this is sample message 3", CREATE_DATE = DateTime.Now, IS_READ = 0 });
            return objList;
        }
        public static List<WO_SUP_MSG> getSupport(string distId, bool IsOpen = true)
        {
            List<WO_SUP_MSG> objList = new List<WO_SUP_MSG>();
            objList.Add(new WO_SUP_MSG { SUP_NUMBER = "abc", SUP_TYPE = "Common", DIST_ID = distId, IS_ACTIVE = false, CREATE_DATE = DateTime.Now, UPDATE_DATE = DateTime.Now, CLOSED_BY = "Mr.X", CLOSED_NOTE = "Solved" });
            objList.Add(new WO_SUP_MSG { SUP_NUMBER = "zsd", SUP_TYPE = "Common", DIST_ID = distId, IS_ACTIVE = true, CREATE_DATE = DateTime.Now, UPDATE_DATE = DateTime.Now, CLOSED_BY = "Mr.X", CLOSED_NOTE = "Solved" });
            objList.Add(new WO_SUP_MSG { SUP_NUMBER = "35s", SUP_TYPE = "Common", DIST_ID = distId, IS_ACTIVE = false, CREATE_DATE = DateTime.Now, UPDATE_DATE = DateTime.Now, CLOSED_BY = "Mr.X", CLOSED_NOTE = "Solved" });
            objList.Add(new WO_SUP_MSG { SUP_NUMBER = "sdf", SUP_TYPE = "Common", DIST_ID = distId, IS_ACTIVE = true, CREATE_DATE = DateTime.Now, UPDATE_DATE = DateTime.Now, CLOSED_BY = "Mr.X", CLOSED_NOTE = "Solved" });
            return objList;
        }

        public static List<WO_SUP_MSG_BODY> getSupportBody(string supId)
        {
            List<WO_SUP_MSG_BODY> objList = new List<WO_SUP_MSG_BODY>();
            objList.Add(new WO_SUP_MSG_BODY { SUP_NUMBER = supId, BODY_TEXT = "Hello", CREATE_DATE = DateTime.Now, SUP_BY = "0" });
            objList.Add(new WO_SUP_MSG_BODY { SUP_NUMBER = supId, BODY_TEXT = "How can I help you", CREATE_DATE = DateTime.Now, SUP_BY = "Mr Y" });
            objList.Add(new WO_SUP_MSG_BODY { SUP_NUMBER = supId, BODY_TEXT = "Need help about products", CREATE_DATE = DateTime.Now, SUP_BY = "0" });
            return objList;
        }
    }
}