using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using API.DataContract;
using API.Modules;
using API.Services;
using API.Tests.Lib;
using NUnit.Framework;
using Raven.Client;
using StoryQ;

namespace API.Tests.Spec
{
    [TestFixture]
    public class FooSpecification : ServiceSpecificationBase<FooService, FooModule>
    {
        protected override void SetupContainer()
        {
            base.SetupContainer();
            Container.Rebind<IDocumentStore>().ToMethod(context => RavenSetup.GetRavenEmbeddadedInMemoryDocumentStore());
        }

        private Bar _barInput;
        private IEnumerable<Bar> _barOutputs;
        private InternalError _responseError;
        private const string PostUrl = "";
        private const string GetUrl = "";
        private const string Myval = "MyVal";

        public Feature InitStory()
        {
            return new Story("STORY")
                .InOrderTo("IN ORDER TO")
                .AsA("AS A")
                .IWant("I WANT");
        }

        [Test]
        public void Add()
        {
            InitStory()
                .WithScenario("PUT SCENARIO")
                .Given(Input)
                .When(PostAction)
                .Then(ExpectSuccess)
                .ExecuteWithReport(MethodBase.GetCurrentMethod());
        }

        [Test]
        public void Get()
        {
            InitStory()
                .WithScenario("GET SCENARIO")
                .Given(Input)
                 .And(PostAction)
                .When(GetAction)
                .Then(ExpectOutput)
                .ExecuteWithReport(MethodBase.GetCurrentMethod());
        }

        #region given
        private void Input()
        {
            _barInput = new Bar { Val = Myval };
        }
        #endregion

        #region when
        private void PostAction()
        {
            _responseError = CallHttpPostWithResponse<Bar, InternalError>(PostUrl, _barInput);
        }

        private void GetAction()
        {
            _barOutputs = CallHttp<IEnumerable<Bar>>(GetUrl);
        }
        #endregion

        #region then
        private void ExpectSuccess()
        {
            Assert.IsNull(_responseError);
        }

        private void ExpectOutput()
        {
            Assert.IsNotNull(_barOutputs);
            Assert.AreEqual(1,_barOutputs.Count());
            var firstOrDefault = _barOutputs.FirstOrDefault();
            if (firstOrDefault != null) Assert.AreEqual(_barInput.Val, firstOrDefault.Val);
            Assert.IsNotNull(firstOrDefault);
        }
        #endregion




    }
}