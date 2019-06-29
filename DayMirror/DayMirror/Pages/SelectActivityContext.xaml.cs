using DayMirror.Models;
using DayMirror.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DayMirror
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectActivityContext : ContentPage
    {
        public SelectActivityContext()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            UpdateActivityContextList();
        }

        async void UpdateActivityContextList()
        {
            listView.ItemsSource = await App.Database.GetActionContextsAsync();
        }

        async void ActivityContextSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var actionContext = e.SelectedItem as UserActionContext;

            ((UserActionViewModel)BindingContext).ActionContext = actionContext;
            await Navigation.PushAsync(new StartActionPage()
            {
                BindingContext = this.BindingContext
            });
        }

        async void OnEdit(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;

            await Navigation.PushAsync(new EditActivityContext()
            {
                BindingContext = menuItem.CommandParameter
            });
        }

        async void OnDelete(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var actionContext = menuItem.CommandParameter as UserActionContext;

            await App.Database.DeleteActionContextAsync(actionContext);

            UpdateActivityContextList();
        }

        async void OnAddNewActionContextButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(NewActionContextName.Text))
            {
                await DisplayAlert("","You can't add empty context","Cancel");
                return;
            }

            var actionContext = new UserActionContext()
            {
                Title = NewActionContextName.Text
            };

            await App.Database.CreateOrUpdateActionContextAsync(actionContext);

            // Clear editor
            NewActionContextName.Text = string.Empty;

            UpdateActivityContextList();
        }
    }
}