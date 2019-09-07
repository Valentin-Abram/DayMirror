using DayMirror.Pages.Report;
using DayMirror.ViewModels;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace DayMirror
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        async void OnCreateActionButtonClicked(object sender, EventArgs e)
        {
            
            await Navigation.PushAsync(new StartActionPage()
            {
                BindingContext = new UserActionViewModel()
            });
        }

        async void OnDayMirrorButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ReportMenu());
        }


    }
}
