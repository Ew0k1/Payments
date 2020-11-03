using Microsoft.AspNet.Identity;
using Payments.BLL.Infrastructure;
using Payments.BLL.Interfaces;
using Payments.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.BLL.Services
{
    public class AdministratorService : IAdministratorService
    {
        IUnitOfWork Database { get; set; }

        public AdministratorService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public OperationDetails BlockUser(string id)
        {
            var user = Database.UserManager.FindById(id);

            if (user==null)
            {
                return new OperationDetails(false, "User does not exist", "Id");
            }
            if (user.ClientProfile.IsBlocked)
            {
                return new OperationDetails(false, "User is already blocked", "Id");
            }
            Database.UserManager.AddToRole(user.Id, "blockedUser");
            Database.UserManager.RemoveFromRole(user.Id, "user");
            user.Role = "blockedUser";
            Database.UserManager.Update(user);
            Database.ClientRepository.Block(id);
            Database.Save();
            return new OperationDetails(true, "User is successfully blocked,", "");
        }

        public OperationDetails UnblockUser(string id)
        {
            var user = Database.UserManager.FindById(id);

            if (user == null)
            {
                return new OperationDetails(false, "User does not exist", "Id");
            }
            if (!user.ClientProfile.IsBlocked)
            {
                return new OperationDetails(false, "User is not blocked", "Id");
            }
            Database.UserManager.AddToRole(user.Id, "user");
            Database.UserManager.RemoveFromRole(user.Id, "blockedUser");
            user.Role = "user";
            Database.UserManager.Update(user);
            Database.ClientRepository.Unblock(id);
            Database.Save();
            return new OperationDetails(true, "User is successfully blocked,", "");
        }
    }
}
