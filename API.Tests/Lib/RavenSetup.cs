using Raven.Client.Embedded;

namespace API.Tests.Lib
{
    public class RavenSetup
    {
        public static EmbeddableDocumentStore GetRavenEmbeddadedInMemoryDocumentStore()
        {
            var documentStore = new EmbeddableDocumentStore
            {
                DataDirectory = "Data",
                RunInMemory = true
            };
            documentStore.Initialize();
            return documentStore;
        }
    }
}