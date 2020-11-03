using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Payments.BLL.Interfaces;
using Payments.BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: OwinStartup(typeof(Payments.WEB.App_Start.Startup))]
namespace Payments.WEB.App_Start
{
    public class Startup
    {
        //IServiceCreator serviceCreator = new ServiceCreator();
        public void Configuration(IAppBuilder app)
        {
            //app.CreatePerOwinContext<IUserService>(CreateUserService);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                ExpireTimeSpan = TimeSpan.FromMinutes(5)
            });
        }

        //private IUserService CreateUserService()
        //{
        //    return serviceCreator.CreateUserService("DefaultConnection");
        //}
    }
}