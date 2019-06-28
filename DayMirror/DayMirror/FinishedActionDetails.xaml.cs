﻿using DayMirror.Models;
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
    public partial class FinishedActionDetails : ContentPage
    {
        public FinishedActionDetails()
        {
            InitializeComponent();
            
        }
       

        async void OnGoToMainMenuButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }

        async void OnCreateActionButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new StartActionPage()
            {
                BindingContext = new UserAction()
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var action = (UserAction)BindingContext;

            TimeElapsedLabel.Text =$"Tite elapsed {GetTimeElapsed().ToString(@"hh\:mm\:ss")}";
        }

        private TimeSpan GetTimeElapsed()
        {
            var action = ((UserAction)BindingContext);

            var timeTotal = new TimeSpan();

            try
            {
                timeTotal = action.EndTime.Subtract(action.StartTime);
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }

            return timeTotal;
        }
    }
}