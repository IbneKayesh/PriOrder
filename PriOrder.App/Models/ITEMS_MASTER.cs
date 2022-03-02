using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriOrder.App.Models
{
    public class ITEMS_MASTER
    {
        public int ITEM_ID { get; set; }
        public string ITEM_NAME { get; set; }
        public string ITEM_IMAGE { get; set; }
        public decimal ITEM_RATE { get; set; }
        public int ITEM_CLASS_ID { get; set; }
        public int ITEMS_GROUP_ID { get; set; }

        public List<ITEMS_MASTER> getItems()
        {
            List<ITEMS_MASTER> objList = new List<ITEMS_MASTER>();
            objList.Add(new ITEMS_MASTER { ITEM_ID = 1, ITEM_NAME = "Iron 1", ITEM_IMAGE = "/Assets/images/items/1.jpg", ITEM_RATE = 500, ITEM_CLASS_ID = 1, ITEMS_GROUP_ID = 1 });
            objList.Add(new ITEMS_MASTER { ITEM_ID = 2, ITEM_NAME = "Iron 2", ITEM_IMAGE = "/Assets/images/items/2.jpg", ITEM_RATE = 540, ITEM_CLASS_ID = 1, ITEMS_GROUP_ID = 1 });
            return objList;
        }
    }
}