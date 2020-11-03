using Microsoft.Owin.Security;
using Payments.BLL.DTO;
using System.Web.Mvc;
using Payments.BLL.Infrastructure;
using Payments.BLL.Interfaces;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using System;
using Microsoft.AspNet.Identity;
using Payments.WEB.Models.Account;
using Payments.WEB.Models.Card;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Web.Hosting;

namespace Payments.WEB.Controllers
{
    
    public class AccountController : Controller
    {
        private IUserService UserService { get; set; }

        private IMapper UserMapper { get; set; }

        public AccountController(IUserService userService)
        {
            UserService = userService;
            UserMapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreditCardDTO, CardViewModel>();
                cfg.CreateMap<UserDTO, UserProfileViewModel>();
                cfg.CreateMap<AccountDTO, AccountViewModel>();
                cfg.CreateMap<PictureDTO, PictureViewModel>();
            }).CreateMapper();
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO { Email = model.Email, Password = model.Password };
                ClaimsIdentity claim = await UserService.Authenticate(userDto);

                if (claim == null)
                {
                    ModelState.AddModelError("", "Неверный логин или пароль.");
                }
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = false
                    }, claim);
                    if (returnUrl!=null)
                    {
                        return RedirectToLocal(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                PictureDTO picture = new PictureDTO()
                {
                    Image = ConvertImage(HostingEnvironment.MapPath("/Content/Images/profile-avatar.png"))
                };
                UserDTO userDto = new UserDTO
                {
                    Email = model.Email,
                    Password = model.Password,
                    Role = "user",
                    Name = model.Name,
                    Surname = model.Surname,
                    MiddleName = model.MiddleName,
                    BirthDate = model.BirthDate,
                    Picture= picture
                };
                OperationDetails operationDetails = await UserService.CreateUser(userDto);
                if (operationDetails.Succedeed)
                {

                    ClaimsIdentity claim = await UserService.Authenticate(userDto);
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, claim);
                    return RedirectToAction("Index", "Home");

                }
                else
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(PasswordChangeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = UserService.FindUserByEmail(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "User with this email doesn't exist");
                    return View("ForgotPassword");
                }

                var code = UserService.GeneratePasswordResetToken(user.Id);

                var callbackUrl = Url.Action("ResetPassword", "Account",
                    new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                string body = "Please reset your password by clicking here: <a href=\"" + callbackUrl + "\">link</a>";
                await UserService.SendEmailAsync(user.Id, body);
                return View("ForgotPasswordConfirmation");
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = UserService.FindUserByEmail(model.Email);
                if (user == null)
                {
                    return RedirectToAction("ResetPasswordConfirmation", "Account");
                }

                if (user != null)
                {
                    var mapper = new MapperConfiguration(c => c.CreateMap<ResetPasswordViewModel, UserDTO>()).CreateMapper();
                    var userDto = mapper.Map<ResetPasswordViewModel, UserDTO>(model);
                    UserService.ResetPassword(userDto, user.Email, model.Code, user.Id);
                    return RedirectToAction("ResetPasswordConfirmation", "Account");
                }
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles ="user")]
        public ActionResult UserProfile()
        {
            var u = AuthenticationManager.User.Identity.GetUserId();
            var a = User.Identity.GetUserId();
            var user = UserService.FindUserById(User.Identity.GetUserId());
            if (user != null)
            {
                var profile = UserMapper.Map<UserDTO, UserProfileViewModel>(user);
                var list = profile.GetAccountsPreview(5);
                return View(profile);
            }
            return HttpNotFound();
        }

        public ActionResult ChangePicture(PictureDTO pic, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid && uploadImage != null)
            {
                pic.Image = ConvertImage(uploadImage);

                UserService.UploadPicture(pic, User.Identity.GetUserId());

                return RedirectToAction("Index");
            }
            return View(pic);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        private byte[] ConvertImage(HttpPostedFileBase uploadImage)
        {
            if (uploadImage!=null)
            {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                return imageData;
            }
            else
            {
                return null;
            }
        }
        private byte[] ConvertImage(string path)
        {
            if (path!=null)
            {
                Image image = Image.FromFile(path);
                MemoryStream memoryStream = new MemoryStream();
                image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                return memoryStream.ToArray();
            }
            else
            {
                return null;
            }
        }
    }
}