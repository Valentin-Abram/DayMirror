using DayMirror.Database;
using DayMirror.Models;
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

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            listView.ItemsSource = await GetActionList(DateTime.Now);
        }

        async void OnShowReportDateSelected(object sender, DateChangedEventArgs e)
        {
            listView.ItemsSource = await GetActionList(e.NewDate);
        }

        private async Task<List<ActionInfoView>> GetActionList(DateTime dateTime)
        {
            List<ActionInfoView> actionsViewList = new List<ActionInfoView>();

            var actions = await App.Database.GetDayActionsAsync(DateTime.Now.Date);
            var contexts = await App.Database.GetActionContextsAsync();

            foreach (var item in actions.Where(a => a.Date.Date == dateTime.Date).OrderBy(a => a.StartTime))
            {
                var listItem = new ActionInfoView(item);
                var context = contexts.FirstOrDefault(c => c.ID == item.UserActionContextId);

                if (context != null)
                {
                    listItem.Title += $" [ {context.Title} ]";
                }

                actionsViewList.Add(listItem);
            }

            return actionsViewList;
        }
    }

   

    class ActionInfoView
    {
        public string Title { get; set; }
        public string FullTimeInfo { get; set; }

        public ActionInfoView(UserAction action)
        {
            this.Title = action.Title;

            this.FullTimeInfo = $"{action.StartTime.ToString(@"hh\:mm\:ss")}" +
                $" - {action.EndTime.ToString(@"hh\:mm\:ss")}" +
                $" Total time {action.EndTime.Subtract(action.StartTime).ToString(@"hh\:mm\:ss")}";
        }

    }
}