using Microsoft.AspNet.Identity.EntityFramework;
using Payments.DAL.Entities;
using Payments.DAL.Entities.Cards;
using System.Data.Entity;

namespace Payments.DAL.EF
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(string conectionString) : base(conectionString) { }

        public ApplicationContext():base()
        {

        }

        static ApplicationContext()
        {
            Database.SetInitializer(new DbInitializer());
        }
        public DbSet<ClientProfile> ClientProfiles { get; set; }

        public DbSet<CreditCard> CreditCards { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<Picture> Pictures { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Payment>()
                    .HasRequired(m => m.Sender)
                    .WithMany(t => t.PaymentsSent)
                    .HasForeignKey(m => m.SenderId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Payment>()
                        .HasRequired(m => m.Recipient)
                        .WithMany(t => t.PaymentsRecieve)
                        .HasForeignKey(m => m.RecipientId)
                        .WillCascadeOnDelete(false);
        }
    }
}
