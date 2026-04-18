using Hangman.Models;
using Hangman.Services;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Hangman.Views
{
    /// <summary>
    /// Interaction logic for NewUserWindow.xaml
    /// </summary>
    public partial class NewUserWindow : Window
    {
        public NewUserWindow()
        {
            InitializeComponent();
        }
        private void PickImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Selectează o imagine";
            op.Filter = "Toate imaginile|*.jpg;*.jpeg;*.png|" +
                        "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                        "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                ImagePathTextBox.Text = op.FileName;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UserNameTextBox.Text))
            {
                MessageBox.Show("Enter a name!");
                return;
            }

            UserService service = new UserService();
            List<User> users = service.GetAllUsers() ?? new List<User>();

            users.Add(new User
            {
                Name = UserNameTextBox.Text,
                ImagePath = ImagePathTextBox.Text
            });

            service.SaveUsers(users);
            this.DialogResult = true;
        }
        private void Cancel_Click(object sender, RoutedEventArgs e) => this.DialogResult = false;
    }
}
