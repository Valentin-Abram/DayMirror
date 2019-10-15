using DayMirror.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DayMirror.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateToDoPage : ContentPage
    {
        public CreateToDoPage()
        {
            InitializeComponent();
            SubscribeToMessages();
        }

        private void SubscribeToMessages()
        {
            MessagingCenter.Subscribe<CreateToDoViewModel, string>(this, "Validation", ShowMessage);
        }

        private void ShowMessage(object sender, string message)
        {
            DisplayAlert("Validation error", message, "Ok");
        }
    }
}