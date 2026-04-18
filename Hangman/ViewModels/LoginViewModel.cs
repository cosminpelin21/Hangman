using Hangman.Helpers;
using Hangman.Models;
using Hangman.Services;
using Hangman.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Hangman.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly UserService _userService;
        private User _selectedUser;

        public ObservableCollection<User> Users { get; set; }

        public User SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));

                (PlayCommand as RelayCommand)?.RaiseCanExecuteChanged();
                (DeleteUserCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public ICommand PlayCommand { get; }
        public ICommand AddUserCommand { get; }
        public ICommand DeleteUserCommand { get; set; }
        public ICommand CancelCommand { get; }
        public ICommand PreviousUserCommand { get; }
        public ICommand NextUserCommand { get; }

        public LoginViewModel()
        {
            _userService = new UserService();
            Users = new ObservableCollection<User>(_userService.GetAllUsers());

            PlayCommand = new RelayCommand(_ => ExecutePlay(), _ => SelectedUser != null);
            AddUserCommand = new RelayCommand(_ => ExecuteAddUser());
            DeleteUserCommand = new RelayCommand(_ => ExecuteDeleteUser(), _ => SelectedUser != null);

            CancelCommand = new RelayCommand(_ =>
            {
                SelectedUser = null;
            });

            PreviousUserCommand = new RelayCommand(_ => ExecuteNavigateUser(-1), _ => Users.Count > 0);
            NextUserCommand = new RelayCommand(_ => ExecuteNavigateUser(1), _ => Users.Count > 0);
        }

        private void ExecutePlay()
        {
            if (SelectedUser != null)
            {
                var gameWindow = new Hangman.Views.GameWindow(SelectedUser, "All categories");
                gameWindow.Show();

                var loginWindow = System.Windows.Application.Current.Windows
                    .OfType<System.Windows.Window>()
                    .FirstOrDefault(w => w is LoginWindow);

                if (loginWindow != null)
                {
                    loginWindow.Close();
                }
            }
        }

        private void ExecuteAddUser()
        {
            var newUserWindow = new NewUserWindow();
            if (newUserWindow.ShowDialog() == true)
            {
                Users = new ObservableCollection<User>(_userService.GetAllUsers());
                OnPropertyChanged(nameof(Users));
            }
        }

        private void ExecuteDeleteUser()
        {
            if (SelectedUser != null)
            {
                var result = MessageBox.Show($"Are you sure you want to delete user {SelectedUser.Name}?",
                                           "Confirm Delete", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    var storageService = new GameStorageService();
                    var allSessions = storageService.GetAllSaves();

                    _userService.DeleteUsers(SelectedUser, allSessions);

                    Users.Remove(SelectedUser);
                    SelectedUser = null;

                    MessageBox.Show("User successfully deleted!");
                }
            }
        }

        private void ExecuteNavigateUser(int direction)
        {
            if (Users == null || Users.Count == 0) return;

            int currentIndex = -1;
            if (SelectedUser != null)
            {
                currentIndex = Users.IndexOf(SelectedUser);
            }

            int newIndex = (currentIndex + direction + Users.Count) % Users.Count;
            SelectedUser = Users[newIndex];
        }
    }
}
