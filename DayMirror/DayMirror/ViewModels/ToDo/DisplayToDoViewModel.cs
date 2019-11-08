using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DayMirror.ViewModels
{
    class DisplayToDoViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<UserActionDisplayModel> ToDoList { get; set; } = new ObservableCollection<UserActionDisplayModel>();
        private UserActionDisplayModel _selectedItem;
        public UserActionDisplayModel SelectedItem
        {
            get 
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                NotifyPropertyChanged();
            }
        }


        public ICommand RunActionCommand { get; set; }
        public ICommand EditActionCommand { get; set; }
        public ICommand DeleteActionCommand { get; set; }

        public DisplayToDoViewModel()
        {
            RegisterCommands();
        }

        private void RegisterCommands()
        {
            RunActionCommand = new Command<object>(RunAction);
            EditActionCommand = new Command<object>(EditAction);
            DeleteActionCommand = new Command<object>(DeleteAction);
        }

        public async Task LoadData()
        {
            ToDoList.Clear();

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

        public void RunAction(object data)
        {
            SendMessage("RunAction", data);
        }

        public void EditAction(object data)
        {
            SendMessage("EditAction", data);
        }

        public void DeleteAction(object data)
        {
            SendMessage("DeleteAction", data);
        }

        private void SendMessage(string messageName, object data)
        {
            MessagingCenter.Send<DisplayToDoViewModel, object>(this, messageName, data);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
