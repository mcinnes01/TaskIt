using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskIt.Data.Interfaces;

namespace TaskIt.Data
{
    public class TaskRepository : IRepository<Task>
    {
        public TaskRepository()
        {

        }

        public bool Create(Task item)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int? id)
        {
            throw new NotImplementedException();
        }

        public Task Edit(int? id)
        {
            throw new NotImplementedException();
        }

        public Task Get(int? id)
        {
            throw new NotImplementedException();
        }

        public List<Task> GetList()
        {
            throw new NotImplementedException();
        }
    }
}
