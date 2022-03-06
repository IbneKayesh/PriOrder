using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriOrder.App.Models
{
    public class WO_APP_MENU
    {
        public int MODULE_ID { get; set; }
        public string MODULE_NAME_EN { get; set; }
        public string MODULE_NAME_BN { get; set; }
        public string MODULE_ICON { get; set; }
        public int MODULE_ORDER { get; set; }

        public int PARENT_ID { get; set; }
        public string PARENT_NAME_EN { get; set; }
        public string PARENT_NAME_BN { get; set; }
        public string PARENT_ICON { get; set; }
        public int PARENT_ORDER { get; set; }

        public int MENU_ID { get; set; }
        public string MENU_NAME_EN { get; set; }
        public string MENU_NAME_BN { get; set; }
        public string MENU_ICON { get; set; }
        public string AREA_NAME { get; set; }
        public string CONTROLLER_NAME { get; set; }
        public string ACTION_NAME { get; set; }
        public int MENU_ORDER { get; set; }
    }
}