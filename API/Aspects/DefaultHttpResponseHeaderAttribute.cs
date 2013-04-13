using System;
using System.ServiceModel.Web;
using PostSharp.Aspects;

namespace API.Aspects
{
    [Serializable]
    public class DefaultHttpResponseHeaderAttribute : OnMethodBoundaryAspect
    {
        public override void OnExit(MethodExecutionArgs args)
        {
            var context = WebOperationContext.Current;

            if (context != null)
            {
                if (HasNotSetResponseHeader(context, "Access-Control-Allow-Origin"))
                {
                    string origin = "*";
                    string requestOrigin = context.IncomingRequest.Headers.Get("Origin");
                    if (!string.IsNullOrWhiteSpace(requestOrigin))
                    {
                        origin = requestOrigin;
                    }
                    context.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", origin);
                }

                if (HasNotSetResponseHeader(context, "Access-Control-Allow-Methods"))
                {
                    context.OutgoingResponse.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
                }

                if (HasNotSetResponseHeader(context, "Access-Control-Allow-Headers"))
                {
                    // Accept: Content-Type is required for the AJAX POST's
                    context.OutgoingResponse.Headers.Add("Access-Control-Allow-Headers", "Accept, Content-Type");
                }

                if (HasNotSetResponseHeader(context, "Access-Control-Allow-Credentials"))
                {
                    // Access-Control-Allow-Credentials must be true if we want to allow cookies to be used.
                    context.OutgoingResponse.Headers.Add("Access-Control-Allow-Credentials", "true");
                }
            }
            base.OnExit(args);
        }

        private bool HasNotSetResponseHeader(WebOperationContext context, string headerName)
        {
            return string.IsNullOrEmpty(context.OutgoingResponse.Headers.Get(headerName));
        }
    }
}