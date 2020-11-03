using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Payments.WEB.Models.Payment
{
    public class CreatePaymentViewModel
    {
        [Required]
        [Display(Name = "Payment title")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Display(Name = "Details")]
        public string Details { get; set; }

        [Required]
        [Display(Name = "Recipient")]
        public int RecipientNumber { get; set; }

        [Required]
        [Display(Name = "Account")]
        public int SelectedAccountId { get; set; }

        public IEnumerable<SelectListItem> Accounts { get; set; }
    }
}