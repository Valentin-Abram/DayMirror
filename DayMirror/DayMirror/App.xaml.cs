using DayMirror.Database;
using DayMirror.Enums.UserAction;
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
            // if app state is restored successfully
            if (MainPage.Navigation.NavigationStack.Count > 1)
            {
                return;
            }

            var actions = await App.Database.GetDayActionsAsync(DateTime.Now);

            var runningAction = actions
                .Where(a => a.Status == UserActionStatus.Running)
                .FirstOrDefault();

            if (runningAction != null )
            {
                var actionContext = await App.Database.GetActionContextAsync(runningAction.UserActionContextId);

                await MainPage.Navigation.PushAsync(new Pages.ActionStates.RunningActionPage()
                {
                    BindingContext = new ViewModels.ActionStates.RunningActionViewModel(runningAction, actionContext)
                }) ;
            }

        }

    }
}
