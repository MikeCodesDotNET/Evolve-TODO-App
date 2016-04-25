using System;
using System.Collections.Generic;
using EvolveTODO.ViewModels;
using Xamarin.Forms;

namespace EvolveTODO.Pages
{
    public partial class ToDosPage : ContentPage
    {
        ToDosViewModel viewModel;
        public ToDosPage()
        {
            InitializeComponent();
            viewModel = new ToDosViewModel();
            BindingContext = viewModel;
        }

        public void OnDelete(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            DisplayAlert("Delete Context Action", mi.CommandParameter + " delete context action", "OK");
        }
    }
}

