using System.Collections.Generic;
using API.DataContract;

namespace API.Interface
{
    public interface IFoo
    {
        IEnumerable<Bar> GetAll();
        void AddOrUpdate(Bar bar);
    }
}