using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Payments.BLL.DTO;
using Payments.BLL.Interfaces;
using Payments.WEB.Models.Account;
using Payments.WEB.Models.Administrator;
using Payments.WEB.Models.Card;
using Payments.WEB.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Payments.WEB.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private IUserService UserService { get; set; }

        private IAdministratorService AdministratorService { get; set; }


        public AdminController(IUserService userService, IAdministratorService adminService)
        {
            UserService = userService;
            AdministratorService = adminService;
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        [HttpGet]
        public ActionResult Index(string sortOrder, int page = 1)
        {
            ViewBag.NameSortParam = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewBag.SurnameSortParam = sortOrder == "surname_asc" ? "surname_desc" : "surname_asc";
            ViewBag.MiddleNameSortParam = sortOrder == "middleName_asc" ? "middleName_desc" : "middleName_asc";
            ViewBag.BirthDateSortParam = sortOrder == "birthDate_asc" ? "birthDate_desc" : "birthDate_asc";
            ViewBag.StatusSortParam = sortOrder == "status_asc" ? "status_desc" : "status_asc";

            int pageSize = 20;
            var users = UserService.GetAllClientProfiles(pageSize, page, sortOrder);
            int totalItems = UserService.GetNumberOfProfiles();

            List<UserProfileViewModel> profiles = new List<UserProfileViewModel>();
            foreach (var profile in users)
            {
                UserProfileViewModel profileModel = new UserProfileViewModel()
                {
                    BirthDate = profile.BirthDate,
                    Id = profile.Id,
                    IsBlocked = profile.IsBlocked,
                    MiddleName = profile.MiddleName,
                    Name=profile.Name,
                    Surname = profile.Surname
                };
                profiles.Add(profileModel);
            }
         
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = totalItems };

            

            var model = new ClientProfilesViewModel()
            {
                UserProfiles = profiles,
                PageInfo = pageInfo
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult BlockProfile(string id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var user = UserService.FindUserById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            var mapper = new MapperConfiguration(
                    cfg => cfg.CreateMap<UserDTO, UserProfileViewModel>()).CreateMapper();
            var profileViewModel = mapper.Map<UserDTO, UserProfileViewModel>(user);

            return View(profileViewModel);
        }

        [HttpPost, ActionName("BlockProfile")]
        public ActionResult BlockConfirm(string id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var user = UserService.FindUserById(id);

            if (user == null)
            {
                return HttpNotFound();
            }
            AdministratorService.BlockUser(user.Id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ViewProfile(string id)
        {
            var user = UserService.FindUserById(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreditCardDTO, CardViewModel>();
                cfg.CreateMap<UserDTO, UserProfileViewModel>();
                cfg.CreateMap<AccountDTO, AccountViewModel>();
                cfg.CreateMap<PictureDTO, PictureViewModel>();
            }).CreateMapper();
            var profileViewModel = mapper.Map<UserDTO, UserProfileViewModel>(user);
            return View(profileViewModel);
        }
    }
}