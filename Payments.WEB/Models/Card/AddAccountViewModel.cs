using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Payments.WEB.Models.Card
{
    public class AddAccountViewModel
    {
        [Required]
        [Display(Name = "Card name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Credit card")]
        public int SelectedCreditCardId { get; set; }
        public IEnumerable<SelectListItem> Cards { get; set; }
    }
}