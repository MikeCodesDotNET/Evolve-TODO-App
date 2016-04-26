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

            //TODO Create our client (1)
            //Create our client
            MobileService = new MobileServiceClient(Helpers.Keys.AzureServiceUrl, null)
            {
                SerializerSettings = new MobileServiceJsonSerializerSettings()
                {
                    CamelCasePropertyNames = true
                }
            };

            //TODO Create our database store & define a table. (2)
            var store = new MobileServiceSQLiteStore("todo.db");
            store.DefineTable<ToDoItem>();

            await MobileService.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());

            //Get our sync table that will call out to azure
            todoTable = MobileService.GetSyncTable<ToDoItem>();

            isInitialized = true;
        }

        public async Task SyncToDos()
        {
            //TODO Lets double check we're connected to the internets. No point in throwing errors if not. (3)
            var connected = await CrossConnectivity.Current.IsReachable(Helpers.Keys.AzureServiceUrl);
            if (connected == false)
                return;

            try
            {
                //TODO Push and Pull our data (4)
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

            //TODO Insert item into todoTable (5)
            await todoTable.InsertAsync(item);

            //Synchronize todos
            await SyncToDos();
            return item;
        }

        public async Task<ToDoItem> UpdateItem(ToDoItem item)
        {
            await Initialize();

            //TODO Update item (6)
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
                //TODO Delete item and Sync (7)
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