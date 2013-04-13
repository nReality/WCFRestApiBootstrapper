using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using API.Aspects;
using API.DataContract;
using API.Interface;

namespace API.Services
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    [ExceptionShield]
    public class FooService
    {
        private readonly IFoo _foo;

        public FooService(IFoo foo)
        {
            _foo = foo;
        }

        [DefaultHttpResponseHeader]
        [WebInvoke(Method = "POST", UriTemplate = "", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(InternalError))]
        public void AddOrUpdate(Bar bar)
        {
            _foo.AddOrUpdate(bar);
        }

        [DefaultHttpResponseHeader]
        [WebInvoke(Method = "GET", UriTemplate = "", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        public IEnumerable<Bar> Get()
        {
            return _foo.GetAll();
        }

        [OperationContract]
        [AllowCrossOriginHeader]
        [WebInvoke(Method = "OPTIONS", UriTemplate = "/*", BodyStyle = WebMessageBodyStyle.Bare)]
        public Stream Options()
        {
            return OptionsStream.CreateOptionsStream();
        }
    }
}
