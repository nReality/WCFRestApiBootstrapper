using API.Implementation;
using API.Interface;
using Ninject.Modules;
using Raven.Client;
using Raven.Client.Document;

namespace API.Modules
{
    public class FooModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDocumentStore>().ToMethod(context =>
                {
                    var documentStore = new DocumentStore
                    {
                        ConnectionStringName = "ConnString"
                    };
                    documentStore.Initialize();
                    return documentStore;
                });

            Bind<IFoo>().To<Foo>();

        }
    }
}