using System;
using System.Net;
using System.ServiceModel.Activation;
using System.Web.Routing;
using API.Modules;
using API.Services;
using Ninject;
using Ninject.Extensions.Wcf;
using Ninject.Modules;
using Ninject.Web.Common;

namespace API
{
    public class Global : NinjectHttpApplication
    {
        protected override void OnApplicationStarted()
        {
            base.OnApplicationStarted();
            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            RouteTable.Routes.Add(new ServiceRoute("account", new NinjectWebServiceHostFactory(), typeof(FooService)));
        }

        protected override IKernel CreateKernel()
        {
            var modules = new INinjectModule[]
                {
                    new FooModule()
                };
            return new StandardKernel(modules); ;
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            UseUnauthorizedResponseWhenRedirectingToLoginPage();
        }

        /// <summary>
        /// When an unauthorized request occurs, we want to send back an Unauthorized response, instead of letting forms authentication redirect to the login page.
        /// This solution was found via [http://social.msdn.microsoft.com/Forums/en-NZ/wcf/thread/b6b0cd09-a95a-483e-8ad3-48a90d66d11c ].
        /// </summary>
        private void UseUnauthorizedResponseWhenRedirectingToLoginPage()
        {
            bool isRedirectingToLoginPage =
                Response.StatusCode == (int)HttpStatusCode.Redirect
                && Response.RedirectLocation.ToLower().Contains("login.aspx");

            if (isRedirectingToLoginPage)
            {
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
        }
    }
}
