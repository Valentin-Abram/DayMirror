using DayMirror.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DayMirror.ViewModels
{
    class DisplayToDoViewModel
    {
        public ObservableCollection<UserActionDisplayModel> ToDoList { get; set; } = new ObservableCollection<UserActionDisplayModel>();

        public ICommand LoadDataCommand { get; set; }

        public DisplayToDoViewModel()
        {
            LoadData();
        }

        public async Task LoadData()
        {
            var list = await App.Database.GetToDoListAsync();
            var contextList = await App.Database.GetActionContextsAsync();

            foreach (var item in list)
            {
                ToDoList.Add(
                    new UserActionDisplayModel(
                        item,
                        contextList.FirstOrDefault(c => c.ID == item?.UserActionContextId))
                    );
            }
        }

    }
}
