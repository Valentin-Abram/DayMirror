using DayMirror.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DayMirror.Pages.ToDo
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
            MessagingCenter.Subscribe<CreateToDoViewModel, string>(this, "Validation", ShowValidationMessage);
            MessagingCenter.Subscribe<CreateToDoViewModel, string>(this, "Creation", PopPage);
        }

        private void PopPage(CreateToDoViewModel sender, string message)
        {
            if (message == "Ok")
            {
                Navigation.PopAsync();
            }
            else
            {
                DisplayAlert("Error", message, "Ok");
            }
        }

        private void ShowValidationMessage(object sender, string message)
        {
            DisplayAlert("Validation error", message, "Ok");
        }
    }
}