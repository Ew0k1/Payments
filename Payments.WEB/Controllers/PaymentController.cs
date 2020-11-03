using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Payments.BLL.DTO;
using Payments.BLL.Infrastructure;
using Payments.BLL.Interfaces;
using Payments.WEB.Models.Card;
using Payments.WEB.Models.Pagination;
using Payments.WEB.Models.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Payments.WEB.Controllers
{
    [Authorize (Roles="user")]
    public class PaymentController : Controller
    {
        private IPaymentService PaymentService { get; set; }

        private IAccountService AccountService { get; set; }

        public PaymentController(IPaymentService paymentService, IAccountService accountService)
        {
            PaymentService = paymentService;
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
        public ActionResult Index(int page = 1)
        {
            int pageSize = 10;
            var paymentsSent = PaymentService.FindSentPayments(User.Identity.GetUserId(), pageSize, page).ToList();
            var paymentsReceive = PaymentService.FindReceivedPayments(User.Identity.GetUserId(), pageSize, page).ToList();
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PaymentDTO, PaymentViewModel>();
                cfg.CreateMap<AccountDTO, AccountViewModel>();
            }).CreateMapper();
            var paymentsSentViewModel = mapper.Map<List<PaymentDTO>, List<PaymentViewModel>>(paymentsSent);
            var paymentsReceiveViewModel = mapper.Map<List<PaymentDTO>, List<PaymentViewModel>>(paymentsReceive);

            PageInfo pageInfoSent = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = paymentsSentViewModel.Count };
            PageInfo pageInfoReceive = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = paymentsReceiveViewModel.Count };

            PaymentListViewModel model = new PaymentListViewModel()
            {
                PaymentsSent = paymentsSentViewModel,
                PaymentsReceived = paymentsReceiveViewModel,
                PageInfoSent = pageInfoSent,
                PageInfoReceive = pageInfoReceive
            };

            return View(model);
        }

        [HttpGet]
        public ActionResult CreatePayment()
        {
            var accounts = AccountService.FindUserAccounts(User.Identity.GetUserId()).ToList();
            SelectList cardsSelectList = new SelectList(accounts, "Id", "Name");
            var model = new CreatePaymentViewModel
            {
                Accounts = cardsSelectList
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePayment(CreatePaymentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var payment = new PaymentDTO
                {
                    Amount = model.Amount,
                    Date = DateTime.Now,
                    Details = model.Details,
                    IsSent = true,
                    Name = model.Name,
                    Recipient = AccountService.FindByNumber(model.RecipientNumber),
                    SenderId = model.SelectedAccountId,
                };
                OperationDetails operationDetails = PaymentService.Create(payment);

                if (operationDetails.Succedeed)
                {
                    return Redirect("/Payment/Index");
                }
                else
                {
                    var accounts = AccountService.FindUserAccounts(User.Identity.GetUserId()).ToList();
                    model.Accounts = new SelectList(accounts, "Id", "Name");
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
                }
                   
                return View(model);

            }
            else
            {
                var accounts = AccountService.FindUserAccounts(User.Identity.GetUserId()).ToList();
                model.Accounts = new SelectList(accounts, "Id", "Name");
                return View(model);
            }
        }

    }
}