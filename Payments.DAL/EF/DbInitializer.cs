using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Payments.DAL.Entities;
using Payments.DAL.Entities.Cards;
using Payments.DAL.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.DAL.EF
{
    class DbInitializer : DropCreateDatabaseIfModelChanges<ApplicationContext>
    {
        protected override void Seed(ApplicationContext db)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));

            var roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(db));


            // создаем две роли
            var role1 = new ApplicationRole { Name = "admin" };
            var role2 = new ApplicationRole { Name = "user" };
            var role3 = new ApplicationRole { Name = "blockedUser" };
            var role4 = new ApplicationRole { Name = "moderator" };

            // добавляем роли в бд
            roleManager.Create(role1);
            roleManager.Create(role2);
            roleManager.Create(role3);
            roleManager.Create(role4);

            var card1 = new VisaCard()
            {
                Name = "Test Card1",
                Number = "1234567891234567",
                Month = "10",
                Year = "20",
                SecurityCodeHash = "123"
            };
            var card2 = new MasterCard()
            {
                Name = "Test Card2",
                Number = "9887655432216554",
                Month = "07",
                Year = "24",
                SecurityCodeHash = "000"
            };
            var card3 = new MasterCard()
            {
                Name = "Test Card3",
                Number = "1887655432216552",
                Month = "02",
                Year = "26",
                SecurityCodeHash = "000"
            };
            var card4 = new MasterCard()
            {
                Name = "Test Card4",
                Number = "1887655432216444",
                Month = "06",
                Year = "27",
                SecurityCodeHash = "000"
            };

            var cards = new List<CreditCard>
            {
                card1,
                card2
            };
            var cards1 = new List<CreditCard>
            {
                card3
            };
            var cards2 = new List<CreditCard>
            {
                card4
            };

            db.CreditCards.Add(card1);
            db.CreditCards.Add(card2);
            db.CreditCards.Add(card3);
            db.CreditCards.Add(card4);

            //////////////create payments///////////
            var payment1 = new Payment()
            {
                Amount = 100,
                Date = DateTime.Now,
                Details = "First Test payment 100",
                IsSent = true,
                Name = "Test Payment",
            };

            var payment2 = new Payment()
            {
                Amount = 9000,
                Date = DateTime.Now,
                Details = "First Test payment 9000",
                IsSent = true,
                Name = "Test Payment",
            };

            var payments1 = new List<Payment>
            {
                payment1
            };

            var payments2 = new List<Payment>
            {
                payment2
            };

            db.Payments.Add(payment1);
            ////////create accounts/////////////////

            var account1 = new Account()
            {
                Balance = 1000,
                CreditCard = card3,
                Number = 100000091,
                Name = "Account1",
                PaymentsRecieve = payments1,
                PaymentsSent = payments2
            };

            var account2 = new Account()
            {
                Balance = 50000,
                CreditCard = card4,
                Number = 100000099,
                Name = "Account2",
                PaymentsRecieve = payments2,
                PaymentsSent = payments1
            };


            var accounts1 = new List<Account>
            {
                account1
            };

            var accounts2 = new List<Account>
            {
                account2
            };

            db.Accounts.Add(account1);
            db.Accounts.Add(account2);

            /////////////create user profiles/////////////
          
            var profile = new ClientProfile()
            {
                Name = "Admin",
                BirthDate = DateTime.Now,
                Cards = cards,
            };

            var userProfile1 = new ClientProfile()
            {
                Name = "User1",
                BirthDate = DateTime.Now,
                Cards = cards1,
                Accounts = accounts1,
            };

            var userProfile2 = new ClientProfile()
            {
                Name = "User2",
                BirthDate = DateTime.Now,
                Cards = cards2,
                Accounts = accounts2,
            };

            // создаем пользователей
            var admin = new ApplicationUser { Email = "admin@gmail.com", UserName = "admin@gmail.com", ClientProfile = profile, Role = "admin" };
            string password = "Admin123";
            var result = userManager.Create(admin, password);

            var user1 = new ApplicationUser { Email = "user1@gmail.com", UserName = "user1@gmail.com", ClientProfile = userProfile1, Role = "user" };
            string password1 = "User123";
            var result1 = userManager.Create(user1, password1);

            var user2 = new ApplicationUser { Email = "user2@gmail.com", UserName = "user2@gmail.com", ClientProfile = userProfile2, Role = "user" };
            string password2 = "User123";
            var result2 = userManager.Create(user2, password2);

            // если создание пользователя прошло успешно
            if (result.Succeeded)
            {
                // добавляем для пользователя роль
                userManager.AddToRole(admin.Id, role1.Name);
            }

            if (result1.Succeeded)
            {
                // добавляем для пользователя роль
                userManager.AddToRole(user1.Id, role2.Name);
            }

            if (result2.Succeeded)
            {
                // добавляем для пользователя роль
                userManager.AddToRole(user2.Id, role2.Name);
            }

            base.Seed(db);
        }
    }
}
