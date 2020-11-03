using Payments.WEB.Models.Card;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Payments.WEB.Models.Account
{
    public class UserProfileViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string MiddleName { get; set; }

        public DateTime BirthDate { get; set; }

        public PictureViewModel Picture { get; set; }

        public string Guid { get; set; }

        public List<CardViewModel> Cards { get; set; }

        public List<AccountViewModel> Accounts { get; set; }

        public bool IsBlocked { get; set; }

        public string GetStatus(bool state)
        {
            if (state)
            {
                return "Blocked";
            }
            else
            {
                return "Active";
            }
        }

        public List<AccountViewModel> GetAccountsPreview(int amount)
        {
            return Accounts.OrderBy(i => i.IsBlocked).TakeWhile(x => x.IsBlocked == false).Take(5).ToList();
        }
    }
}