using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriOrder.App.Models
{
    public class ITEMS_GROUP
    {
        public int ITEMS_GROUP_ID { get; set; }
        public string ITEMS_GROUP_NAME { get; set; }

        public List<ITEMS_GROUP> getGroups()
        {
            List<ITEMS_GROUP> objList = new List<ITEMS_GROUP>();
            objList.Add(new ITEMS_GROUP { ITEMS_GROUP_ID = 1, ITEMS_GROUP_NAME = "Electronics" });
            objList.Add(new ITEMS_GROUP { ITEMS_GROUP_ID = 2, ITEMS_GROUP_NAME = "Wood" });
            return objList;
        }
    }
}