using DayMirror.Database;
using DayMirror.Models;
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
    public partial class DayReportPage : ContentPage
    {

        public DayReportPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            List<ActionInfoView> actionsView = new List<ActionInfoView>();

            var actions = await App.Database.GetDayActionsAsync(DateTime.Now.Date);

            foreach (var item in actions.Where(a => a.Date.Date == DateTime.Now.Date).OrderBy(a => a.StartTime))
            {
                actionsView.Add(new ActionInfoView(item));
            }

            listView.ItemsSource = actionsView;
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