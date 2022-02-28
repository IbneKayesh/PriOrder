using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PriOrder.App.Models
{
    public class MESSAGES
    {
        public int ID { get; set; }
        public string MESSAGES_DATE_TIME { get; set; }
        public string MESSAGES_BODY { get; set; }
        public bool IS_READ { get; set; }
    }
}