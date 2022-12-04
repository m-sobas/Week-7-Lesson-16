using Diary.Commands;
using Diary.Models;
using Diary.Models.Configurations;
using Diary.Models.Domains;
using Diary.Models.Wrappers;
using Diary.Properties;
using Diary.Views;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Diary.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private Repository _repository = new Repository();


        public MainViewModel()
        {
            //using (var context = new ApplicationDbContext())
            //{
            //    var students = context.Students.ToList();
            //}

            if (CheckConnection())
            {
                var splashScreenWindow = new SplashScreenView();
                splashScreenWindow.Show();

                Thread.Sleep(3000);

                splashScreenWindow.Close();
            }
            else
            {
                var dialog = MessageBox.Show("Nie można połączyć się z bazą danych.\nCzy chcesz na nowo skonfigurować połączenie?", "Błąd połączenia", MessageBoxButton.OKCancel, MessageBoxImage.Error);

                if (dialog != MessageBoxResult.OK)
                {
                    CloseApplication();
                    return;
                }

                var dbConfigWindow = new DataBaseConfigView();
                bool? dialogResult = dbConfigWindow.ShowDialog();

                if (dialogResult == true)
                {
                    RestartApplication();
                    return;
                }
                else
                {
                    CloseApplication();
                    return;
                }
            }

            AddStudentCommand = new RelayCommand(AddEditStudent);
            EditStudentCommand = new RelayCommand(AddEditStudent, CanEditDeleteStudent);
            DeleteStudentCommand = new AsyncRelayCommand(DeleteStudent, CanEditDeleteStudent);
            RefreshStudentsCommand = new RelayCommand(RefreshStudents);
            DatabaseConfigCommand = new RelayCommand(DatabaseConfig);
          
            RefreshDiary();
            InitGroups();
        }

        public ICommand AddStudentCommand { get; set; }
        public ICommand EditStudentCommand { get; set; }
        public ICommand DeleteStudentCommand { get; set; }
        public ICommand RefreshStudentsCommand { get; set; }
        public ICommand DatabaseConfigCommand { get; set; }

        private StudentWrapper _selectedStudent;

        public StudentWrapper SelectedStudent
        {
            get
            {
                return _selectedStudent;
            }
            set 
            {
                _selectedStudent = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<StudentWrapper> _students;

        public ObservableCollection<StudentWrapper> Students
        {
            get
            {
                return _students;
            }
            set
            {
                _students = value;
                OnPropertyChanged();
            }
        }


        private int _selectedGroupId;

        public int SelectedGroupId
        {
            get 
            { 
                return _selectedGroupId; 
            }
            set 
            { 
                _selectedGroupId = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Group> _group;

        public ObservableCollection<Group> Groups
        {
            get
            {
                return _group;
            }
            set
            {
                _group = value;
                OnPropertyChanged();
            }
        }

        private void RefreshStudents(object obj)
        {
            RefreshDiary();
        }


        private bool CanRefreshStudents(object obj)
        {
            return true;
        }


        private void AddEditStudent(object obj)
        {
            var addEditStudentWindow = new AddEditStudentView(obj as StudentWrapper);
            addEditStudentWindow.Closed += AddEditStudentWindow_Closed;
            addEditStudentWindow.ShowDialog();
        }


        private void AddEditStudentWindow_Closed(object sender, EventArgs e)
        {
            RefreshDiary();
        }


        private bool CanEditDeleteStudent(object obj)
        {
            return SelectedStudent != null;
        }


        private async Task DeleteStudent(object obj)
        {
            var metroWindow = Application.Current.MainWindow as MetroWindow;

            var dialog = await metroWindow.ShowMessageAsync("Usuwanie ucznia", $"Czy na pewno chcesz usunąć ucznia {SelectedStudent.FirstName} {SelectedStudent.LastName}?", 
                MessageDialogStyle.AffirmativeAndNegative);

            if (dialog != MessageDialogResult.Affirmative)
                return;

            _repository.DeleteStudent(SelectedStudent.Id);

            RefreshDiary();
        }


        private bool CanDeleteStudent(object obj)
        {
            return SelectedStudent != null;
        }


        private void RefreshDiary()
        {
            Students = new ObservableCollection<StudentWrapper>(_repository.GetStudents(SelectedGroupId));
        }


        private void InitGroups()
        {
            var groups = _repository.GetGroups();
            groups.Insert(0, new Group { Id = 1, Name = "Wszystkie" });

            Groups = new ObservableCollection<Group>(groups);

            SelectedGroupId = 0;
        }


        private void DatabaseConfig(object obj)
        {
            var dbConfigWindow = new DataBaseConfigView();
            bool? dialogResult = dbConfigWindow.ShowDialog();

            if (dialogResult == true)
            {
                RestartApplication();
                return;
            }
        }


        public bool CheckConnection()
        {
            try
            {
                using (var context = new ApplicationDbContext(DBConnection.ConnectionString))
                {
                    context.Database.Connection.Open();
                    context.Database.Connection.Close();
                }
            }
            catch (SqlException)
            {
                return false;
            }

            return true;
        }


        private void RestartApplication()
        {
            var currentExecutablePath = Process.GetCurrentProcess().MainModule.FileName;
            Process.Start(currentExecutablePath);
            Application.Current.Shutdown();
        }


        private void CloseApplication()
        {
            Application.Current.Shutdown();
        }
    }
}
