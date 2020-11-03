using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Payments.WEB.Models.Card
{
    public class ChangeLimitViewModel
    {
        [Required]
        public int AccountId { get; set; }

        [Required]
        [Display(Name ="Limit")]
        [Range(0,10000)]
        public int Limit { get; set; }
    }
}