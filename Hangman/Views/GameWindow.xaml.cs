using Hangman.Models;
using Hangman.ViewModels;
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
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        public GameWindow(User user, string category)
        {
            InitializeComponent();
            this.DataContext = new GameViewModel(user, category);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key >= Key.A && e.Key <= Key.Z)
            {
                string pressedKey = e.Key.ToString();
                var viewModel = this.DataContext as GameViewModel;

                if (viewModel != null && viewModel.GuessLetterCommand.CanExecute(pressedKey))
                {
                    viewModel.GuessLetterCommand.Execute(pressedKey);
                }
            }
        }
    }
}
