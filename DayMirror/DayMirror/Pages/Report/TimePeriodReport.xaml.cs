using DayMirror.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DayMirror.Pages.Report
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimePeriodReport : ContentPage
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public TimePeriodReport()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            listView.ItemsSource = await GetStatisticsAsync();
        }

        public async Task<List<StatisticData>> GetStatisticsAsync()
        {
            return await App.Database.GetUserActionStatistic(DateFrom, DateTo);
        }

    }
}