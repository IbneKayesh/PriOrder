using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriOrder.App.Models
{
    public class CATEGORIES
    {
        public int ID { get; set; }
        public string CATEGORY_NAME { get; set; }
        public string CATEGORY_IMAGE { get; set; }
        public int ITEMS_COUNT { get; set; }
    }
}