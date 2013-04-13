using System;
using System.Configuration;
using System.ServiceModel.Web;
using PostSharp.Aspects;

namespace API.Aspects
{
    [Serializable]
    public class AllowCrossOriginHeaderAttribute : OnMethodBoundaryAspect
    {
        private string AllowedOrigins
        {
            get
            {
                return ConfigurationManager.AppSettings["AllowedOrigins"];
            }
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            var context = WebOperationContext.Current;
            if (context == null)
            {
                return;
            }

            var response = context.OutgoingResponse;

            var accessControlAllowOrigin = GetAccessControlAllowOriginValue(context);
            SetHeader(response, "Access-Control-Allow-Origin", accessControlAllowOrigin);

            SetHeader(response, "Access-Control-Allow-Credentials", "true");

            response.Headers.Remove("Server");
            response.Headers.Remove("X-Powered-By");

            base.OnExit(args);
        }

        private string GetAccessControlAllowOriginValue(WebOperationContext context)
        {
            var accessControlAllowOrigin = "*";
            string allowedOrigins = AllowedOrigins;
            var providedOrigin = context.IncomingRequest.Headers.Get("Origin");
            if (!string.IsNullOrWhiteSpace(providedOrigin))
            {
                if (allowedOrigins.Contains(providedOrigin))
                {
                    accessControlAllowOrigin = providedOrigin;
                }
            }
            return accessControlAllowOrigin;
        }

        private static void SetHeader(OutgoingWebResponseContext response, string headerName, string headerValue)
        {
            if (string.IsNullOrWhiteSpace(response.Headers.Get(headerName)))
            {
                response.Headers.Add(headerName, headerValue);
            }
            else
            {
                response.Headers.Set(headerName, headerValue);
            }
        }
    }
}