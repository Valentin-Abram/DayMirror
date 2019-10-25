using DayMirror.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DayMirror.Pages.ToDo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ToDoListPage : ContentPage
    {
        public ToDoListPage()
        {
            InitializeComponent();
            CheckForEmptyList();
        }

        private void CheckForEmptyList()
        {
            var context = BindingContext as DisplayToDoViewModel;

            EmptyListMessage.IsVisible = context?.ToDoList?.Count > 0 ? false : true;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CreateToDoPage());
        }
    }
}