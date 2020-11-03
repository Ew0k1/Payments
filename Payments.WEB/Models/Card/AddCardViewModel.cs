using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Payments.WEB.Models.Card
{
    public class AddCardViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.CreditCard)]
        [Display(Name = "Card Number")]
        [RegularExpression("^(?:4[0-9]{12}(?:[0-9]{3})?|[25][1-7][0-9]{14}|6(?:011|5[0-9][0-9])[0-9]{12}|3[47][0-9]{13}|3(?:0[0-5]|[68][0-9])[0-9]{11}|(?:2131|1800|35\\d{3})\\d{11})$",
            ErrorMessage = "Invalid card")]
        public string Number { get; set; }

        [Required]
        [RegularExpression("^(0[1-9]|1[0-2])$", ErrorMessage = "Invalid month")]
        public string Month { get; set; }

        [Required]
        [RegularExpression("^([1-9][0-9])$", ErrorMessage = "Invalid year")]
        public string Year { get; set; }

        [Required]
        [Display(Name = "CVV")]
        [RegularExpression("^[0-9]{3}$", ErrorMessage = "Invalid security code")]
        public string SecurityCode { get; set; }
    }
}