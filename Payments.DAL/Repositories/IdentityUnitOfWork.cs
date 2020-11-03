using Microsoft.AspNet.Identity.EntityFramework;
using Payments.DAL.EF;
using Payments.DAL.Entities;
using Payments.DAL.Identity;
using Payments.DAL.Interfaces;
using System;
using System.Threading.Tasks;

namespace Payments.DAL.Repositories
{
    public class IdentityUnitOfWork : IUnitOfWork
    {
        private ApplicationContext db;
        private AccountRepository accountRepository;
        private CardRepository cardRepository;
        private PaymentRepository paymentRepository;
        private ClientRepository clientRepository;
        private PictureRepository pictureRepository;

        //private ResidenceRepository residenceRepository;

        public IdentityUnitOfWork(string connectionString)
        {
            db = new ApplicationContext(connectionString);
            UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            RoleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(db));
        }

        public ApplicationUserManager UserManager { get; }


        public ApplicationRoleManager RoleManager { get; }

        public AccountRepository AccountRepository
        {
            get
            {
                if (accountRepository == null)
                    accountRepository = new AccountRepository(db);
                return accountRepository;
            }
        }

        public CardRepository CardRepository
        {
            get
            {
                if (cardRepository == null)
                    cardRepository = new CardRepository(db);
                return cardRepository;
            }
        }

        public PaymentRepository PaymentRepository
        {
            get
            {
                if (paymentRepository == null)
                    paymentRepository = new PaymentRepository(db);
                return paymentRepository;
            }
        }

        public ClientRepository ClientRepository
        {
            get
            {
                if (clientRepository == null)
                    clientRepository = new ClientRepository(db);
                return clientRepository;
            }
        }

        public PictureRepository PictureRepository
        {
            get
            {
                if (pictureRepository == null)
                    pictureRepository = new PictureRepository(db);
                return pictureRepository;
            }
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (!disposed)
                {
                    if (disposing)
                    {
                        db.Dispose();
                    }
                    disposed = true;
                }
            }
        }

        
    }
}
