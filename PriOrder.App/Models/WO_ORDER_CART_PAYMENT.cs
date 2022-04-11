using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PriOrder.App.Models
{
    public class WO_ORDER_CART_PAYMENT
    {
        [Display(Name = "Status")]
        public bool IS_VALID { get; set; } = true;

        [Display(Name = "Total")]
        public decimal TOTAL { get; set; } = 0;

        public virtual T_DSMA_BAL T_DSMA_BAL { get; set; }

        public virtual T_MBDO_INCV T_MBDO_INCV { get; set; }

        public virtual List<T_MBDO> T_MBDO { get; set; }
    }
}