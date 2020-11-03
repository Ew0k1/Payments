using Payments.DAL.Identity;
using Payments.DAL.Repositories;
using System;
using System.Threading.Tasks;

namespace Payments.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationUserManager UserManager { get; }

        ClientRepository ClientRepository { get; }

        ApplicationRoleManager RoleManager { get; }

        CardRepository CardRepository { get; }

        AccountRepository AccountRepository { get; }

        PaymentRepository PaymentRepository { get; }

        PictureRepository PictureRepository { get; }

        Task SaveAsync();

        void Save();
    }
}
