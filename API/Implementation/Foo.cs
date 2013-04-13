using System.Collections.Generic;
using System.Linq;
using API.DataContract;
using API.Interface;
using Raven.Client;

namespace API.Implementation
{
    public class Foo : IFoo
    {
        private readonly IDocumentStore _documentStore;

        public Foo(IDocumentStore documentStore)
        {
            _documentStore = documentStore;
        }

        public IEnumerable<Bar> GetAll()

        {
            using (var session = _documentStore.OpenSession())
            {
                return session.Query<Bar>().ToList();
            }
        }

        public void AddOrUpdate(Bar bar)
        {
            using (var session = _documentStore.OpenSession())
            {
                session.Store(bar);
                session.SaveChanges();
            }
        }
    }
}