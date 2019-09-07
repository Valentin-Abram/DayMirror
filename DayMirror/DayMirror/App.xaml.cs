using DayMirror.Database;
using DayMirror.ViewModels;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DayMirror
{
    public partial class App : Application
    {
        static DayMirrorSqlDb _database;
        public static DayMirrorSqlDb Database
        {
            get
            {
                if (_database == null)
                {
                    _database = new DayMirrorSqlDb(Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DayMirrorDb.db3"
                        ));
                }

                return _database;
            }
        }

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            TryRestoreRunningAction();

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        
        protected override void OnResume()
        {
            TryRestoreRunningAction();
        }

        private async Task TryRestoreRunningAction()
        {
            var actions = await App.Database.GetDayActionsAsync(DateTime.Now);

            var runningAction = actions
                .Where(a => a.Status == Models.UserActionStatus.Running)
                .FirstOrDefault();

            if (runningAction != null && MainPage.Navigation.NavigationStack.Count == 1)
            {
                var actionModel = await UserActionViewModel.FromAction(runningAction);

                await MainPage.Navigation.PushAsync(new RunningActionPage()
                {
                    BindingContext = actionModel
                });
            }

        }

    }
}
