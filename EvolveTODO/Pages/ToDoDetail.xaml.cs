using System;
using System.Collections.Generic;
using EvolveTODO.Models;

using Xamarin.Forms;

namespace EvolveTODO.Pages
{
    public partial class ToDoDetail : ContentPage
    {
        public ToDoDetail(ToDoItem item = null)
        {
            InitializeComponent();
            if(item != null)
                BindingContext = new ViewModels.ToDoDetailViewModel(item);
            else
                BindingContext = new ViewModels.ToDoDetailViewModel();

        }
    }
}

