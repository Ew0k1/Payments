using Payments.WEB.Models.Account;
using Payments.WEB.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Payments.WEB.Models.Administrator
{
    public class ClientProfilesViewModel
    {
        public List<UserProfileViewModel> UserProfiles { get; set; }

        public PageInfo PageInfo { get; set; }
    }
}