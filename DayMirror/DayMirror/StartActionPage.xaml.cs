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
    public partial class StartActionPage : ContentPage
    {
        public StartActionPage()
        {
            InitializeComponent();
        }

        async void OnStartActivityButtonClicked(object sender, EventArgs e)
        {
            var action = (UserAction)BindingContext;

            if (string.IsNullOrEmpty(action.Title))
            {
                await DisplayAlert("", "Please enter action name", "Cancel");
                return;
            }

            action.StartTime = DateTime.Now.TimeOfDay;
            action.Date = DateTime.Now;

            await Navigation.PushAsync(new RunningActionPage()
            {
                BindingContext = action
            });
        }

        async void OnAddContextButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SelectActivityContext()
            {
                BindingContext = new UserAction()
            });
        }

    }
}