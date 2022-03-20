using System;
using System.ComponentModel.DataAnnotations;

namespace PriOrder.App.Models
{
    public class V_CHOITALY
    {
        [Display(Name = "From")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime SPMS_FDAT { get; set; }

        [Display(Name = "To")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime SPMS_TDAT { get; set; }

        [Display(Name = "Slab")]
        public decimal SPMS_NSLB { get; set; }

        [Display(Name = "Item")]
        public int SMPL_ITEM { get; set; }

        [Display(Name = "Name")]
        public string ITEM_NAME  { get; set; }

        [Display(Name = "Qty")]
        public int SPGM_GQTY  { get; set; }
        
        public string GRP1 { get; set; }

        [Display(Name = "Group")]
        public string ITEM_GROUP { get; set; }

        [Display(Name = "Slab For")]
        public string SPMS_ITTY { get; set; }
    }
}
