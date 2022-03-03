using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriOrder.App.Models
{
    public class CATEGORIES
    {
        public int CATEGORY_ID { get; set; }
        public string CATEGORY_NAME { get; set; }
        public string CATEGORY_IMAGE { get; set; }
        public int ITEM_CLASS_COUNT { get; set; }
        public int ITEMS_COUNT { get; set; }

        public List<CATEGORIES> getCategories()
        {
            List<CATEGORIES> objList = new List<CATEGORIES>();
            objList.Add(new CATEGORIES { CATEGORY_ID = 1, CATEGORY_NAME = "Electronic items", CATEGORY_IMAGE= "/Assets/images/icons/category-blue/cpu.svg",ITEM_CLASS_COUNT=2,ITEMS_COUNT=30 });
            objList.Add(new CATEGORIES { CATEGORY_ID = 2, CATEGORY_NAME = "Home equipments", CATEGORY_IMAGE = "/Assets/images/icons/category-blue/homeitem.svg", ITEM_CLASS_COUNT = 2, ITEMS_COUNT = 30 });
            objList.Add(new CATEGORIES { CATEGORY_ID = 3, CATEGORY_NAME = "Toys and kids", CATEGORY_IMAGE = "/Assets/images/icons/category-blue/toy.svg", ITEM_CLASS_COUNT = 2, ITEMS_COUNT = 30 });
            objList.Add(new CATEGORIES { CATEGORY_ID = 4, CATEGORY_NAME = "Accessories", CATEGORY_IMAGE = "/Assets/images/icons/category-blue/watch.svg", ITEM_CLASS_COUNT = 2, ITEMS_COUNT = 30 });
            objList.Add(new CATEGORIES { CATEGORY_ID = 4, CATEGORY_NAME = "Jewelleries", CATEGORY_IMAGE = "/Assets/images/icons/category-blue/diamond.svg", ITEM_CLASS_COUNT = 2, ITEMS_COUNT = 30 });
            return objList;
        }
    }
}