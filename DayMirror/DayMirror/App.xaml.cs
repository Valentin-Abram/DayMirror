
using DayMirror.Database;
using SQLite;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
