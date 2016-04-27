using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;

using EvolveTODO.Models;
using Plugin.Connectivity;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;

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
            //Create our client
            MobileService = new MobileServiceClient(Helpers.Keys.AzureServiceUrl, null)
            {
                SerializerSettings = new MobileServiceJsonSerializerSettings()
                {
                    CamelCasePropertyNames = true
                }
            };

            //TODO 2: Create our database store & define a table.
            var store = new MobileServiceSQLiteStore("todo.db");
            store.DefineTable<ToDoItem>();

            //MobileServiceSyncHandler - Handles table operation errors and push completion results.
            await MobileService.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());

            //Get our sync table that will call out to azure
            todoTable = MobileService.GetSyncTable<ToDoItem>();

            isInitialized = true;
        }

        public async Task SyncToDos()
        {
            //TODO 3: Add connectivity check. 
            var connected = await Plugin.Connectivity.CrossConnectivity.Current.IsReachable(Helpers.Keys.AzureServiceUrl);
            if (connected == false)
                return; 

            try
            {
                //TODO 4: Push and Pull our data
                await MobileService.SyncContext.PushAsync();
                await todoTable.PullAsync("allTodoItems", todoTable.CreateQuery());
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
            await todoTable.InsertAsync(item);
            //Synchronize todos
            await SyncToDos();
            return item;
        }

        public async Task<ToDoItem> UpdateItem(ToDoItem item)
        {
            await Initialize();

            //TODO 6: Update item
            await todoTable.UpdateAsync(item);

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
                await todoTable.DeleteAsync(item);
                await SyncToDos();

                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}