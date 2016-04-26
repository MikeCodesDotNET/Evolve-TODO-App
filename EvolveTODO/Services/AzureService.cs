using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;

using EvolveTODO.Models;
using Plugin.Connectivity;

namespace EvolveTODO.Services
{
    public class AzureService : IService
    {
        public MobileServiceClient MobileService { get; set; }
        IMobileServiceSyncTable<ToDoItem> todoTable;

        bool isInitialized;
        public async Task Initialize()
        {
            if (isInitialized)
                return;

            //TODO 1: Create our client


            //TODO 2: Create our database store & define a table.
           

            isInitialized = true;
        }

        public async Task SyncToDos()
        {
            //TODO 3: Lets double check we're connected to the internets. No point in throwing errors if not.

            try
            {
                //TODO 4: Push and Pull our data

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to sync items, that is alright as we have offline capabilities: " + ex);
            }
        }

        public async Task<IEnumerable<ToDoItem>> GetToDos()
        {
            await Initialize();
            await SyncToDos();
            return await todoTable.ToEnumerableAsync();
        }

        public async Task<ToDoItem> AddToDo(string text, bool complete)
        {
            await Initialize();
            var item = new ToDoItem
            {
                Text = text,
                Complete = complete
            };

            //TODO 5: Insert item into todoTable


            //Synchronize todos
            await SyncToDos();
            return item;
        }

        public async Task<ToDoItem> UpdateItem(ToDoItem item)
        {
            await Initialize();

            //TODO 6: Update item


            //Synchronize todos
            await SyncToDos();
            return item;
        }

        public async Task<bool> DeleteItem(ToDoItem item)
        {
            await Initialize();
            try
            {
                //TODO 7: Delete item and Sync


                return true;
            }
            catch
            {
                return false;
            }
        }


    }
}