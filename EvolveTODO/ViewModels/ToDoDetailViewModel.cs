using System;
using System.Threading.Tasks;
using EvolveTODO.Helpers;
using EvolveTODO.Models;
using EvolveTODO.Services;
using Xamarin.Forms;

namespace EvolveTODO.ViewModels
{
    public class ToDoDetailViewModel : ViewModelBase
    {
        IService azureService;
        ToDoItem item;

        public ToDoDetailViewModel(ToDoItem item = null)
        {
            if (item != null)
            {
                Title = item.Text;
                this.item = item;
                Text = item.Text;
                Complete = item.Complete;
            }
            else
                Title = "New ToDo";

            azureService = ServiceLocator.Instance.Resolve<IService>();
        }

        public string Text { get; set;}
        public bool Complete { get; set;}


        Command saveItemCommand;
        public Command SaveItemCommand
        {
            get { return saveItemCommand ?? (saveItemCommand = new Command(async () => await ExecuteSaveItemCommand())); }
        }

        async Task ExecuteSaveItemCommand()
        {
            if (IsBusy)
                return;
            
            if (item != null)
            {
                item.Text = Text;
                item.Complete = Complete;
            }

            IsBusy = true;

            try
            {
                Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Saving new item");

                if (item == null)
                {
                    await azureService.AddToDo(Text, Complete);
                    Acr.UserDialogs.UserDialogs.Instance.ShowSuccess("Saved new item", 1500);
                }
                else
                {
                    await azureService.UpdateItem(item);
                    Acr.UserDialogs.UserDialogs.Instance.ShowSuccess("Updated item", 1500);
                }
                
                MessagingCenter.Send<ToDoDetailViewModel>(this, "ItemsChanged");
            }
            catch (Exception ex)
            {
                Acr.UserDialogs.UserDialogs.Instance.ShowError(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        Command deleteItemCommand;
        public Command DeleteItemCommand
        {
            get { return deleteItemCommand ?? (deleteItemCommand = new Command(async () => await ExecuteDeleteItemCommand())); }
        }

        async Task ExecuteDeleteItemCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (item != null)
                {
                    Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Deleting item");

                    await azureService.DeleteItem(item);
                    MessagingCenter.Send<ToDoDetailViewModel>(this, "ItemsChanged");
                    Acr.UserDialogs.UserDialogs.Instance.ShowSuccess($"Deleted {item.Text}", 1500);
                }
                else
                    Acr.UserDialogs.UserDialogs.Instance.ShowError("Item doesn't exist");
            }
            catch (Exception ex)
            {
                Acr.UserDialogs.UserDialogs.Instance.ShowError(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}

