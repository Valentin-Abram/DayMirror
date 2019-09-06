﻿using DayMirror.Models;
using DayMirror.Pages.Report;
using DayMirror.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
