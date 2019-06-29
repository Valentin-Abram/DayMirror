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

namespace DayMirror
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
            listView.ItemsSource = await GetActionViewModels(date);
        }

        void OnReportItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null) return;
            ((ListView)sender).SelectedItem = null;
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