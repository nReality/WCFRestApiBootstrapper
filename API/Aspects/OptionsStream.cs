using System.IO;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace API.Aspects
{
    public class OptionsStream
    {
        public static Stream CreateOptionsStream()
        {
            //
            // Note: This method exists to enable the preflight request in CORS, and the only way I could get it to work
            //       was by following the blog post http://blog.joelchristner.com/2012/06/hi-allim-writing-this-post-to-hopefully.html
            //
            var context = WebOperationContext.Current;
            if (context == null)
            {
                return null;
            }
            var operationContext = OperationContext.Current;

            var response = context.OutgoingResponse;
            response.StatusDescription = "OK";
            response.StatusCode = HttpStatusCode.OK;
            response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, X-Requested-With, Accept");
            response.Headers.Add("Access-Control-Expose-Headers", "Content-Type, X-Requested-With, Accept");
            response.Headers.Add("Accept", "*/*");
            response.Headers.Add("Accept-Language", "en-US, en");
            response.Headers.Add("Accept-Charset", "ISO-8859-1, utf-8");
            response.Headers.Add("Connection", "keep-alive");

            string url = operationContext.IncomingMessageHeaders.To.ToString();
            url = url.Replace(operationContext.IncomingMessageHeaders.To.PathAndQuery, "");
            response.Headers.Add("Host", url);

            return null;
        }
    }
}