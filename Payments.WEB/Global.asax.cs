using Ninject;
using Ninject.Modules;
using Ninject.Web.Mvc;
using Payments.BLL.Infrastructure;
using Payments.WEB.Util;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Payments.WEB
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            NinjectModule userModule = new UserModule();
            NinjectModule serviceModule = new ServiceModule("MyConnection");
            var kernel = new StandardKernel(userModule, serviceModule);
            kernel.Unbind<ModelValidatorProvider>();
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }
    }
}
