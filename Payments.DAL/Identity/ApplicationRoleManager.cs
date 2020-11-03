using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Payments.DAL.Entities;

namespace Payments.DAL.Identity
{
    public class ApplicationRoleManager : RoleManager<ApplicationRole>
    {
        public ApplicationRoleManager(RoleStore<ApplicationRole> store)
                    : base(store)
        { }
    }
}
