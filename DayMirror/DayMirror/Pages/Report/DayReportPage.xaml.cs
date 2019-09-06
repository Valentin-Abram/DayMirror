using DayMirror.Database;
using DayMirror.Models;
using DayMirror.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
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

        }

        async void OnDelete(object sender, EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var actionViewModel = menuItem.CommandParameter as UserActionViewModel;

            await App.Database.DeleteAction(actionViewModel.GetAction());
        }

        private async Task<List<UserActionViewModel>> GetActionViewModels(DateTime date)
        {
            var userActions = await App.Database.GetDayActionsAsync(date);

            List<UserActionViewModel> actionViewModels = new List<UserActionViewModel>();

            foreach (var item in userActions)
            {
                actionViewModels.Add(new UserActionViewModel
                {
                    Title = item.Title,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    ActionContext = await App.Database.GetActionContextAsync(item.UserActionContextId)
                });
            }

            return actionViewModels;
        }

    }
}