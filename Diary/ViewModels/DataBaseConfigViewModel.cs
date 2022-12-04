using Diary.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace Diary.ViewModels
{
    public class DataBaseConfigViewModel : ViewModelBase
    {
        public DataBaseConfigViewModel()
        {
            CloseCommand = new RelayCommand(Close);
            SaveCommand = new RelayCommand(Save);
        }

        public ICommand CloseCommand { get; set; }
        public ICommand SaveCommand { get; set; }

        private string _host;

        public string Host
        {
            get { return Properties.Settings.Default.Host; }
            set 
            { 
                Properties.Settings.Default.Host = value;
                OnPropertyChanged();
            }
        }

        private string _serverName;

        public string ServerName
        {
            get { return Properties.Settings.Default.ServerName; }
            set
            {
                Properties.Settings.Default.ServerName = value;
                OnPropertyChanged();
            }
        }

        private string _database;

        public string Database
        {
            get { return Properties.Settings.Default.Database; }
            set
            {
                Properties.Settings.Default.Database = value;
                OnPropertyChanged();
            }
        }

        private string _userId;

        public string UserId
        {
            get { return Properties.Settings.Default.UserId; }
            set
            {
                Properties.Settings.Default.UserId = value;
                OnPropertyChanged();
            }
        }

        private string _password;

        public string Password
        {
            get { return Properties.Settings.Default.Password; }
            set
            {
                Properties.Settings.Default.Password = value;
                OnPropertyChanged();
            }
        }


        private void Close(object obj)
        {
            (obj as Window).DialogResult = false;
        }


        private void Save(object obj)
        {
            Properties.Settings.Default.Save();

            (obj as Window).DialogResult = true;
        }

    }
}
