using DayMirror.Models;
using DayMirror.Models.UserAction;
using DayMirror.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DayMirror.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditUserAction : ContentPage
    {
        public EditUserAction()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            ActionContextPicker.ItemsSource = await App.Database.GetActionContextsAsync();
            ActionContextPicker.SelectedItem = ((List<UserActionContext>)ActionContextPicker.ItemsSource)
                .First(ac => ac.ID == GetUserActionViewModel().ActionContext.ID);
        }

        private UserActionViewModel GetUserActionViewModel()
        {
            return ((UserActionViewModel)BindingContext);
        }

        private async void Save_Button_Clicked(object sender, EventArgs e)
        {
            var userAction = GetUserActionViewModel();
            userAction.ActionContext = ((UserActionContext)ActionContextPicker.SelectedItem);

            await App.Database.UpdateUserAction(userAction.GetAction());
            await Navigation.PopAsync();
        }

        private async void Cancel_Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}