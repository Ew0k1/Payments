using Ninject.Modules;
using Payments.BLL.Interfaces;
using Payments.BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Payments.WEB.Util
{
    public class UserModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUserService>().To<UserService>();
            Bind<ICardService>().To<CardService>();
            Bind<IAccountService>().To<AccountService>();
            Bind<IPaymentService>().To<PaymentService>();
            Bind<IAdministratorService>().To<AdministratorService>();
        }
    }
}