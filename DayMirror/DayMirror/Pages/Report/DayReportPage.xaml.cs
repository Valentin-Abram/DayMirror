using DayMirror.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DayMirror.Pages.Report
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DayReportPage : ContentPage
    {
        public DayReportPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            UpdateDayReportActionList(DateTime.Now);
        }

        void OnShowReportDateSelected(object sender, DateChangedEventArgs e)
        {
            UpdateDayReportActionList(e.NewDate);
        }

        async void UpdateDayReportActionList(DateTime date)
        {
            statisticListView.ItemsSource = await App.Database.GetUserActionStatistic(date, date);
            listView.ItemsSource = await GetActionViewModels(date);
        }

        void OnReportItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;
            ((ListView)sender).SelectedItem = null;
        }

        async void OnEdit(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var actionViewModel = menuItem.CommandParameter as UserActionViewModel;

            await Navigation.PushAsync(new EditUserAction() {
                BindingContext = actionViewModel
            });
        }

        async void OnDelete(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var actionViewModel = menuItem.CommandParameter as UserActionViewModel;

            App.Database.DeleteAction(actionViewModel.GetAction()).Wait();
            UpdateDayReportActionList(DateTime.Now);
        }

        private async Task<List<UserActionViewModel>> GetActionViewModels(DateTime date)
        {
            var userActions = await App.Database.GetDayActionsAsync(date);

            List<UserActionViewModel> actionViewModels = new List<UserActionViewModel>();

            foreach (var item in userActions)
            {
                actionViewModels.Add(new UserActionViewModel
                {
                    Id = item.ID,
                    Title = item.Title,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    ActionContext = await App.Database.GetActionContextAsync(item.UserActionContextId),
                    Date = item.Date,
                    Status = item.Status
                });
            }

            return actionViewModels;
        }

    }
}