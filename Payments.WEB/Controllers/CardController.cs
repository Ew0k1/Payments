using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Payments.BLL.DTO;
using Payments.BLL.Infrastructure;
using Payments.BLL.Interfaces;
using Payments.WEB.Models.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Payments.WEB.Controllers
{
    [Authorize (Roles = "user")]
    public class CardController : Controller
    {
        private ICardService CardService { get; set; }

        private IUserService UserService { get; set; }

        private IAccountService AccountService { get; set; }

        public CardController(ICardService cardService, IUserService userService, IAccountService accountService)
        {
            CardService = cardService;
            UserService = userService;
            AccountService = accountService;
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        [HttpGet]
        public ActionResult AddCard()
        {
            return View("AddCard");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCard(AddCardViewModel model)
        {
            if (ModelState.IsValid)
            {
                var mapper = new MapperConfiguration(
                    cfg => cfg.CreateMap<AddCardViewModel, CreditCardDTO>()).CreateMapper();
                var card = mapper.Map<AddCardViewModel, CreditCardDTO>(model);
                var userId = AuthenticationManager.User.Identity.GetUserId();

                OperationDetails operationDetails = CardService.Create(card, userId);

                if (operationDetails.Succedeed)
                {
                    return Redirect("/Account/UserProfile");
                }
                else
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult DeleteCard(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var user = UserService.FindUserById(User.Identity.GetUserId());
            var card = user.Cards.Find(c => c.Id == id);
            if (card == null)
            {
                return HttpNotFound();
            }
            var mapper = new MapperConfiguration(
                    cfg => cfg.CreateMap<CreditCardDTO, CardViewModel>()).CreateMapper();
            var cardViewModel = mapper.Map<CreditCardDTO, CardViewModel>(card);

            return PartialView(cardViewModel);
        }

        [HttpPost, ActionName("DeleteCard")]
        public ActionResult DeleteConfirm(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var user = UserService.FindUserById(User.Identity.GetUserId());
            var card = user.Cards.Find(c => c.Id == id);
            if (card == null)
            {
                return HttpNotFound();
            }
            CardService.DeleteCard(card.Id);
            return RedirectToAction("UserProfile", "Account", null);
        }

        [HttpGet]
        public ActionResult BlockAccount(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var user = UserService.FindUserById(User.Identity.GetUserId());
            var account = user.Accounts.Find(c => c.Id == id);
            if (account == null)
            {
                return HttpNotFound();
            }
            var mapper = new MapperConfiguration(
                    cfg => cfg.CreateMap<AccountDTO, AccountViewModel>()).CreateMapper();
            var accountViewModel = mapper.Map<AccountDTO, AccountViewModel>(account);

            return PartialView(accountViewModel);
        }

        [HttpPost, ActionName("BlockAccount")]
        public ActionResult BlockConfirm(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var user = UserService.FindUserById(User.Identity.GetUserId());
            var account = user.Accounts.Find(c => c.Id == id);
            if (account == null)
            {
                return HttpNotFound();
            }
            AccountService.BlockAccount(account.Id);
            return RedirectToAction("Accounts");
        }

        [HttpGet]
        public ActionResult ChangeLimit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var account = AccountService.FindUserAccounts(User.Identity.GetUserId())
                .Where(a => a.Id == id).FirstOrDefault();
            if (account != null)
            {
                var mapper = new MapperConfiguration(
                   cfg => cfg.CreateMap<AccountDTO, AccountViewModel>()).CreateMapper();
                var accountViewModel = mapper.Map<AccountDTO, AccountViewModel>(account);
                return View(accountViewModel);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost, ActionName("ChangeLimit")]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeLimitConfirm(AccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var details = AccountService.ChangeLimit(model.Id, model.Limit);
                if (details.Succedeed)
                {
                    return RedirectToAction("Accounts");
                }
                else
                {
                    ModelState.AddModelError(details.Property, details.Message);
                }
            }
            return View(model);
            
              
           

        }
        [HttpGet]
        public ActionResult AddAccount()
        {
            var cards = CardService.FindUserCards(User.Identity.GetUserId()).ToList();
            SelectList cardsSelectList = new SelectList(cards, "Id", "Number");
            var model = new AddAccountViewModel
            {
                Cards = cardsSelectList
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAccount(AddAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var acc = new AccountDTO()
                {
                    Name = model.Name,
                    CreditCardId = model.SelectedCreditCardId,
                    UserProfileId = User.Identity.GetUserId()
                };
                AccountService.Create(acc);
                return RedirectToAction("Accounts");
            }
            else
            {
                var cards = CardService.FindUserCards(User.Identity.GetUserId()).ToList();
                model.Cards = new SelectList(cards, "Id", "Number");
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult Accounts(string sortOrder)
        {
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.StatusSortParam = String.IsNullOrEmpty(sortOrder) ? "status_desc" : "";
            ViewBag.NameSortParam = sortOrder == "Name" ? "name_desc" : "Name";
            ViewBag.BalanceSortParam = sortOrder == "Balance" ? "balance_desc" : "Balance";
            ViewBag.NumberSortParam = sortOrder == "Number" ? "number_desc" : "Number";

            var accounts = AccountService.FindUserAccounts(User.Identity.GetUserId()).ToList();
            var mapper = new MapperConfiguration(
                    cfg => cfg.CreateMap<AccountDTO, AccountViewModel>()).CreateMapper();
            var accountsViewModel = mapper.Map<List<AccountDTO>, List<AccountViewModel>>(accounts);

            switch (sortOrder)
            {
                case "Name":
                    accountsViewModel = accountsViewModel.OrderBy(s => s.Name).ToList();
                    break;
                case "name_desc":
                    accountsViewModel = accountsViewModel.OrderByDescending(s => s.Name).ToList();
                    break;
                case "Balance":
                    accountsViewModel = accountsViewModel.OrderBy(s => s.Balance).ToList();
                    break;
                case "balance_desc":
                    accountsViewModel = accountsViewModel.OrderByDescending(s => s.Balance).ToList();
                    break;
                case "Number":
                    accountsViewModel = accountsViewModel.OrderBy(s => s.Number).ToList();
                    break;
                case "number_desc":
                    accountsViewModel = accountsViewModel.OrderByDescending(s => s.Number).ToList();
                    break;
                case "status_desc":
                    accountsViewModel = accountsViewModel.OrderByDescending(s => s.IsBlocked).ToList();
                    break;
                default:
                    accountsViewModel = accountsViewModel.OrderBy(s => s.IsBlocked).ToList();
                    break;
            }

            return View(accountsViewModel);
        }



        //private IEnumerable<SelectListItem> GetCards()
        //{
        //    var cards = CardService.FindUserCards(User.Identity.GetUserId()).ToList();

        //    return new SelectList(cards, "Value", "Text");
        //}
    }
}