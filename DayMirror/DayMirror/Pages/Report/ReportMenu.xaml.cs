using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DayMirror.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DayMirror.Pages.Report
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportMenu : ContentPage
    {
        public ReportMenu()
        {
            InitializeComponent();
        }

        public void OnDayReportButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new DayReportPage());
        }

        public void OnWeekReportButtonClicked(object sender, EventArgs e)
        {
            var timePeriodPage = new TimePeriodReport();
            timePeriodPage.DateFrom = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
            timePeriodPage.DateTo = DateTime.Now;

            Navigation.PushAsync(timePeriodPage);
        }

        public void OnMonthReportButtonClicked(object sender, EventArgs e)
        {
            var startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            var timePeriodPage = new TimePeriodReport();
            timePeriodPage.DateFrom = startDate;
            timePeriodPage.DateTo = DateTime.Now;

            Navigation.PushAsync(timePeriodPage);
        }

        public void OnGetReportButtonClicked(object sender, EventArgs e)
        {
            var timePeriodPage = new TimePeriodReport();
            timePeriodPage.DateFrom = DateFrom.Date;
            timePeriodPage.DateTo = DateTo.Date;

            Navigation.PushAsync(timePeriodPage);
        }
    }
}