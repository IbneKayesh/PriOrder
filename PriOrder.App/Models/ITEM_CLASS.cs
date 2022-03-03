using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriOrder.App.Models
{
    public class ITEM_CLASS
    {
        public int ITEM_CLASS_ID { get; set; }
        public string ITEM_CLASS_NAME { get; set; }
        public int ITEMS_COUNT { get; set; }
        public int CATEGORY_ID { get; set; }

        public List<ITEM_CLASS> getClass()
        {
            List<ITEM_CLASS> objList = new List<ITEM_CLASS>();
            objList.Add(new ITEM_CLASS { ITEM_CLASS_ID = 1, ITEM_CLASS_NAME = "Iron", ITEMS_COUNT = 2, CATEGORY_ID = 1 });
            objList.Add(new ITEM_CLASS { ITEM_CLASS_ID = 2, ITEM_CLASS_NAME = "Air Condition", ITEMS_COUNT = 2, CATEGORY_ID = 1 });
            return objList;
        }
    }
}