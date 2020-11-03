using Microsoft.AspNet.Identity.EntityFramework;


namespace Payments.DAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public virtual bool IsProfileFilled { get; set; }

        public virtual string Role { get; set; }

        public virtual ClientProfile ClientProfile { get; set; }
    }
}
