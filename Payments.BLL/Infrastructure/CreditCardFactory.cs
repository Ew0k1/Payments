using Payments.BLL.DTO;
using Payments.DAL.Entities.Cards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Payments.BLL.Infrastructure
{
    public static class CreditCardFactory
    {
        private static  string masterCardPattern = @"\A5[1-5][0-9]{14}\z";

        private static  string visaCardPattern = @"\A4[0-9]{12}(?:[0-9]{3})?\z";

        public static CreditCard CreateCreditCard(CreditCardDTO card, string securityCodeHash, string userId)
        {
            if (Regex.IsMatch(card.Number.ToString(), masterCardPattern))
            {
                return new MasterCard()
                {
                    Name = card.Name,
                    Number = card.Number,
                    Month = card.Month,
                    Year = card.Year,
                    SecurityCodeHash = securityCodeHash,
                    UserProfileId = userId,
                };
            }
            if (Regex.IsMatch(card.Number.ToString(), visaCardPattern))
            {
                return new VisaCard()
                {
                    Name = card.Name,
                    Number = card.Number,
                    Month = card.Month,
                    Year = card.Year,
                    SecurityCodeHash = securityCodeHash,
                    UserProfileId = userId,
                };
            }
            return null;
        }
    }
}
