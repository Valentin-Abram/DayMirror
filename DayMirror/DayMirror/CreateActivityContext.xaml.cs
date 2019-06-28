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
    public partial class CreateActivityContext : ContentPage
    {
        public CreateActivityContext()
        {
            InitializeComponent();
        }

        async void OnSaveActionContextButtonClicked(object sender, EventArgs e)
        {
            var actionContext = (UserActionContext)BindingContext;
            await App.Database.CreateOrUpdateActionContextAsync(actionContext);

            await Navigation.PopAsync();
        }
    }
}