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
    public partial class EditToDoPage : ContentPage
    {
        public EditToDoPage()
        {
            InitializeComponent();

            MessagingCenter.Subscribe<EditToDoViewModel, string>(this,"Edit", OnEditComplete);

        }

        private void OnEditComplete(EditToDoViewModel arg1, string arg2)
        {
            Navigation.PopAsync();
        }
    }
}