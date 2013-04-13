using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Web;
using NUnit.Framework;
using Newtonsoft.Json;
using Ninject;
using Ninject.Modules;
using RestSharp;

namespace API.Tests.Lib
{
    [TestFixture]
    public abstract class ServiceSpecificationBase<TRestService, TBootstrapperModule>
        where TBootstrapperModule : NinjectModule, new()
    {
        private const string HttpLocalhost = "http://localhost:8888";
        protected WebServiceHost Host;
        protected RestClient Client;
        protected StandardKernel Container;


        [SetUp]
        public virtual void Setup()
        {
            SetupAndCleanupData();
            SetupContainer();
            SetupHost();
            SetupClient();
        }

        protected virtual void SetupAndCleanupData()
        {
            
        }

        protected virtual void SetupContainer()
        {
            Container = new StandardKernel(new TBootstrapperModule());
        }

        public void SetupClient()
        {
            Client = new RestClient
            {
                BaseUrl = HttpLocalhost,
                Timeout = 300 * 1000
            };

        }

        protected TOutput CallHttp<TOutput>(string resource)
            where TOutput: class
        {
            var request = new RestRequest { Resource = resource };
            request.AddHeader("Content-Type", "application/json");
            var timeStart = DateTime.Now;
            var response = Client.Execute(request);
            var timeEnd = DateTime.Now;
            var timeTotalInMilliSeconds = (long)timeEnd.Subtract(timeStart).TotalMilliseconds;

            RestTraceFileGenerator.WriteRestTrace<TRestService>(resource, response, timeTotalInMilliSeconds);
            
            return JsonConvert.DeserializeObject<TOutput>(response.Content);
        }

        protected TOutput CallHttpPostWithResponse<TInput, TOutput>(string resource, TInput objectToPost)
            where TOutput: class 
        {
            var request = new RestRequest {Resource = resource, Method = Method.POST, RequestFormat = DataFormat.Json};
            request.AddBody(objectToPost);

            request.AddHeader("Content-Type", "application/json");

            var timeStart = DateTime.Now;
            var response = Client.Execute(request);
            var timeEnd = DateTime.Now;
            var timeTotalInMilliSeconds = (long)timeEnd.Subtract(timeStart).TotalMilliseconds;

            RestTraceFileGenerator.WriteRestTrace<TRestService>(resource, response, timeTotalInMilliSeconds);

            if (string.IsNullOrWhiteSpace(response.Content))
                return null;
            
            return JsonConvert.DeserializeObject<TOutput>(response.Content);
        }

        protected void CallHttpPost<TInput>(string resource, TInput objectToPost)
        {
            CallHttpPostWithResponse<TInput, object>(resource, objectToPost);
        }


        private void SetupHost()
        {
            Host = new WebServiceHost(Container.Get<TRestService>(), new Uri(HttpLocalhost));

            var behaviour = Host.Description.Behaviors.Find<ServiceBehaviorAttribute>();
            behaviour.InstanceContextMode = InstanceContextMode.Single;

            Host.Open();
        }

        [TearDown]
        public void TearDown()
        {
            try
            {
                Host.Close();

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}