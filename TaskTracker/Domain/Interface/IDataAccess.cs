using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.Models;

namespace TaskTracker.Domain.Interface
{
    public interface IDataAccess
    {
        List<CheckItems> GetTaskData(string user);
        bool CheckIfExists(int id, string user);
        void Insert(int id, string user);
    }
}
