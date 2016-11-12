using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskIt.Data.Interfaces
{
    public interface IRepository<T>
    {
        bool Create(T item);
        List<T> GetList();
        T Get(int? id);
        T Edit(int? id);
        bool Delete(int? id);
    }
}
