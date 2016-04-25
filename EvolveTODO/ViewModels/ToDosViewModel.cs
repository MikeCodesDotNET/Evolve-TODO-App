using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using EvolveTODO.Helpers;
using EvolveTODO.Models;
using EvolveTODO.Services;
using Xamarin.Forms;

namespace EvolveTODO.ViewModels
{
    public class ToDosViewModel : ViewModelBase
    {
        IService azureService;

        public ToDosViewModel()
        {
            Title = "ToDo";
            azureService = ServiceLocator.Instance.Resolve<IService>();

            ExecuteRefreshCommand();
            MessagingCenter.Subscribe<ToDoDetailViewModel>(this, "ItemsChanged", (sender) =>
            {
                ExecuteRefreshCommand();
            });
        }

        ObservableCollection<ToDoItem> toDoItems = new ObservableCollection<ToDoItem>();
        public ObservableCollection<ToDoItem> ToDoItems
        {
            get { return toDoItems; }
            set
            {
                toDoItems = value;
                OnPropertyChanged("ToDoItems");
            }
        }

        private ToDoItem selectedToDoItem;

        public ToDoItem SelectedToDoItem
        {
            get { return selectedToDoItem; }
            set
            {
                selectedToDoItem = value;
                OnPropertyChanged("SelectedItem");

                if (selectedToDoItem != null)
                {
                    var navigation = Application.Current.MainPage as NavigationPage;
                    navigation.PushAsync(new Pages.ToDoDetail(SelectedToDoItem));
                    SelectedToDoItem = null;
                }
            }
        }

        Command refreshCommand;
        public Command RefreshCommand
        {
            get { return refreshCommand ?? (refreshCommand = new Command(async () => await ExecuteRefreshCommand())); }
        }

        async Task ExecuteRefreshCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var todos = await azureService.GetToDos();
                ToDoItems.Clear();
                foreach (var todo in todos)
                {
                    ToDoItems.Add(todo);
                }
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

        Command deleteCommand;
        public Command DeleteCommand
        {
            get { return deleteCommand ?? (deleteCommand = new Command(async () => await ExecuteDeleteCommand())); }
        }

        async Task ExecuteDeleteCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var sucessfullDeletion = await azureService.DeleteItem(SelectedToDoItem);
                if (sucessfullDeletion)
                {
                    var todos = await azureService.GetToDos();
                    ToDoItems.Clear();
                    foreach (var todo in todos)
                    {
                        ToDoItems.Add(todo);
                    }
                }
                else
                {
                    throw new Exception("Failed to delete item");
                }
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

        Command addNewItemCommand;
        public Command AddNewItemCommand
        {
            get { return addNewItemCommand ?? (addNewItemCommand = new Command(async () => await ExecuteAddNewItemCommand())); }
        }

        async Task ExecuteAddNewItemCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                await Application.Current.MainPage.Navigation.PushAsync(new Pages.ToDoDetail());
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
