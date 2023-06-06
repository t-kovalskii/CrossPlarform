using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Net.Http;
using System.Text.Json;
using System.Numerics;

namespace Lab2.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _currentDateTime;
        private string _currentToDoItem;
        private string _currentDeviceInfo;
        private int i = 1;
 
        public string Title
        {
            get => "Welcome to.NET MAUI";
        }

        public string CurrentDateTime
        {
            get => _currentDateTime;
            set
            {
                _currentDateTime = value;
                OnPropertyChanged();
            }
        }

        public string CurrentToDoItem
        {
            get => _currentToDoItem;
            set
            {
                _currentToDoItem = value;
                OnPropertyChanged();
            }
        }

        public ICommand UpdateTimeCommand { get; }

        public ICommand UpdateToDoItemCommand { get; }

        public string CurrentDeviceinfo
        {
            get => new StringBuilder()
            .AppendLine($"Model: {DeviceInfo.Model}")
            .AppendLine($"Manufacturer: {DeviceInfo.Manufacturer}")
            .AppendLine($"Platform: {DeviceInfo.Platform}")
            .AppendLine($"OS Version: {DeviceInfo.VersionString}").ToString();
            set
            {
                _currentDeviceInfo = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            FetchDataFromApiAsync(i);
            UpdateTimeCommand = new Command(UpdateTime);
            UpdateToDoItemCommand = new Command(UpdateToDoItem);
            CurrentDateTime = DateTime.Now.ToString("F");
        }
        private void UpdateTime()
        {
            CurrentDateTime = DateTime.Now.ToString("F");
        }


        private void UpdateToDoItem()
        {
            i++;
            FetchDataFromApiAsync(i);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task FetchDataFromApiAsync(int i)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("https://jsonplaceholder.typicode.com/todos/" + i);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var toDoItem = JsonSerializer.Deserialize<ToDoItem>(json);
                CurrentToDoItem = toDoItem.title;
            }
        }
    }

    public class ToDoItem
    {
        public int userId { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public bool completed { get; set; }
    }
}
