using System.Collections.Generic;
using System.Threading.Tasks;

using EvolveTODO.Models;

namespace EvolveTODO.Services
{
    public interface IService
    {
        Task Initialize();

        Task<IEnumerable<ToDoItem>> GetToDos();

        Task<ToDoItem> AddToDo(string text, bool complete);

        Task<ToDoItem> UpdateItem(ToDoItem item);

        Task<bool> DeleteItem(ToDoItem item);

        Task SyncToDos();
    }
}

