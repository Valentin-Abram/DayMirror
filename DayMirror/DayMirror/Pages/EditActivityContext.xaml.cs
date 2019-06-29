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
    public partial class EditActivityContext : ContentPage
    {
        public EditActivityContext()
        {
            InitializeComponent();
        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            await App.Database.CreateOrUpdateActionContextAsync((UserActionContext)BindingContext);

            await Navigation.PopAsync();
        }
    }
}