using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using EvolveTODO.Models;

namespace EvolveTODO.Services
{
    public class MockService : IService
    {
        List<ToDoItem> items { get; set; } = new List<ToDoItem>();

        public MockService()
        {
            if (items.Count == 0)
                items = ToDos();
        }

        public Task<ToDoItem> AddToDo(string text, bool complete)
        {
            var item = new ToDoItem
            {
                Text = text,
                Complete = complete
            };

            items.Add(item);
            return Task.FromResult(item);
        }

        public Task<ToDoItem> UpdateItem(ToDoItem item)
        {
            var todo = items.FirstOrDefault(x => x.Id == item.Id);
            items.Remove(todo);
            items.Add(item);
            return Task.FromResult(item);
        }

        public Task<bool> DeleteItem(ToDoItem item)
        {
            items.Remove(item);
            return Task.FromResult(true);
        }

        public Task<IEnumerable<ToDoItem>> GetToDos()
        {
            IEnumerable<ToDoItem> todos = items.AsEnumerable();
            return Task.FromResult(todos);
        }

        public Task Initialize()
        {
            return null;
        }

        public Task SyncToDos()
        {
            return null;
        }

        List<ToDoItem> ToDos()
        {
            var items = new List<ToDoItem>();

            var todo1 = new ToDoItem
            {
                Text = "Pack bags",
                Complete = false
            };
            items.Add(todo1);

            var todo2 = new ToDoItem
            {
                Text = "Finish up presentation",
                Complete = true
            };
            items.Add(todo2);

            return items;

        }
    }
}

