using Hangman.Helpers;
using Hangman.Models;
using Hangman.Services;
using Hangman.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Hangman.ViewModels
{
    public class GameViewModel : BaseViewModel
    {
        private User _currentUser;
        private string _category;
        private string _targetWord;
        private string _hiddenWord;
        private string _currentHangmanImage;
        private int _mistakes;
        private int _currentLevel = 1;
        private int _secondsLeft = 30;

        private DispatcherTimer _timer;
        private WordService _wordService = new WordService();
        private GameStorageService _storageService = new GameStorageService();

        public string UserName => _currentUser.Name;
        public string UserLogo => _currentUser.ImagePath;
        public string CurrentCategory => _category;

        public string TargetWord
        {
            get => _targetWord;
            set => _targetWord = value;
        }

        public string HiddenWord
        {
            get => _hiddenWord;
            set { _hiddenWord = value; OnPropertyChanged(nameof(HiddenWord)); }
        }

        public string CurrentHangmanImage
        {
            get => _currentHangmanImage;
            set { _currentHangmanImage = value; OnPropertyChanged(nameof(CurrentHangmanImage)); }
        }

        public int Mistakes
        {
            get => _mistakes;
            set { _mistakes = value; OnPropertyChanged(nameof(Mistakes)); }
        }

        public int CurrentLevel
        {
            get => _currentLevel;
            set { _currentLevel = value; OnPropertyChanged(nameof(CurrentLevel)); }
        }

        public int SecondsLeft
        {
            get => _secondsLeft;
            set { _secondsLeft = value; OnPropertyChanged(nameof(SecondsLeft)); }
        }

        public ObservableCollection<AlphabetLetter> Alphabet { get; set; } = new ObservableCollection<AlphabetLetter>();
        public ObservableCollection<string> MistakesCollection { get; set; } = new ObservableCollection<string>();

        public ICommand GuessLetterCommand { get; }
        public ICommand ChangeCategoryCommand { get; }
        public ICommand NewGameCommand { get; }
        public ICommand SaveGameCommand { get; }
        public ICommand OpenGameCommand { get; }
        public ICommand ShowStatsCommand { get; }
        public ICommand AboutCommand { get; }
        public ICommand ExitCommand { get; }

        public GameViewModel(User currentUser, string category)
        {
            _currentUser = currentUser;
            _category = category;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += Timer_Tick;

            GuessLetterCommand = new RelayCommand(param =>
            {
                string letter = param as string;
                if (!string.IsNullOrEmpty(letter))
                    ExecuteGuess(letter);
            });

            ChangeCategoryCommand = new RelayCommand(param =>
            {
                if (param is string newCategory)
                {
                    _category = newCategory;
                    StartNewLevel();
                    OnPropertyChanged(nameof(CurrentCategory));
                }
            });

            NewGameCommand = new RelayCommand(_ => ExecuteNewGame());
            SaveGameCommand = new RelayCommand(_ => ExecuteSave());
            OpenGameCommand = new RelayCommand(_ => ExecuteOpen());
            ShowStatsCommand = new RelayCommand(_ => ExecuteShowStats());
            ExitCommand = new RelayCommand(_ => ExecuteExit());
            AboutCommand = new RelayCommand(_ => MessageBox.Show("Nume: Pelin Cosmin\nGrupa: 10LF243\nSpecializare: Informatica"));

            RegisterGameStarted();
            StartNewLevel();
        }

        private void StartNewLevel()
        {
            SecondsLeft = 30;
            Mistakes = 0;
            MistakesCollection.Clear();

            Alphabet.Clear();
            for (char c = 'A'; c <= 'Z'; c++)
                Alphabet.Add(new AlphabetLetter { Letter = c.ToString() });

            TargetWord = _wordService.GetRandomWord(_category).ToUpper();
            HiddenWord = string.Join(" ", Enumerable.Repeat("_", TargetWord.Length));

            UpdateHangmanImage();
            _timer.Start();

            OnPropertyChanged(nameof(UserName));
            OnPropertyChanged(nameof(UserLogo));
            OnPropertyChanged(nameof(CurrentCategory));
            OnPropertyChanged(nameof(CurrentHangmanImage));
        }

        private void ExecuteGuess(string letter)
        {
            if (TargetWord.Contains(letter))
            {
                UpdateHiddenWord(letter);
            }
            else
            {
                Mistakes++;
                MistakesCollection.Add("X");
                UpdateHangmanImage();
            }

            var pressedLetter = Alphabet.FirstOrDefault(l => l.Letter == letter);
            if (pressedLetter != null)
                pressedLetter.IsActive = false;

            CheckGameState();
        }

        private void UpdateHiddenWord(string letter)
        {
            char[] current = HiddenWord.ToCharArray();
            for (int i = 0; i < TargetWord.Length; i++)
            {
                if (TargetWord[i].ToString().Equals(letter, StringComparison.OrdinalIgnoreCase))
                {
                    current[i * 2] = TargetWord[i];
                }
            }
            HiddenWord = new string(current);
        }

        private void CheckGameState()
        {
            if (!HiddenWord.Contains("_"))
            {
                _timer.Stop();

                if (CurrentLevel < 3)
                {
                    MessageBox.Show($"Congratulations! You have reached level {CurrentLevel + 1}.");
                    CurrentLevel++;
                    StartNewLevel();
                }
                else
                {
                    MessageBox.Show("Congratulations! You won the whole game!");
                    FinalizeGame(true);
                    StartNewLevel();
                }
            }
            else if (Mistakes >= 7)
            {
                _timer.Stop();
                DisableAllLetters();
                HandleLoss();
            }
        }

        private void HandleLoss()
        {
            _timer.Stop();
            DisableAllLetters();
            MessageBox.Show($"Game Over! The word was: {TargetWord}");
            FinalizeGame(false);
            CurrentLevel = 1;
            OnPropertyChanged(nameof(CurrentLevel));
            StartNewLevel();
        }

        private void FinalizeGame(bool isWin)
        {
            var statsService = new StatisticService();
            statsService.UpdateStatistics(_currentUser.Name, _category, isWin);
            CurrentLevel = 1;
            OnPropertyChanged(nameof(CurrentLevel));
        }

        private void RegisterGameStarted()
        {
            var statsService = new StatisticService();
            statsService.UpdateStatistics(_currentUser.Name, _category, false, true);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            SecondsLeft--;
            if (SecondsLeft <= 0)
            {
                _timer.Stop();
                HandleLoss();
            }
        }

        private void ExecuteNewGame()
        {
            var result = MessageBox.Show("Are you sure you want to start a new game? Your current progress will be lost.",
                                         "New Game", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _timer.Stop();
                CurrentLevel = 1;
                StartNewLevel();
                RegisterGameStarted();
                OnPropertyChanged(nameof(CurrentLevel));
            }
        }

        private void ExecuteSave()
        {
            _timer.Stop();

            var session = new GameSession
            {
                UserName = _currentUser.Name,
                Category = _category,
                CurrentWord = TargetWord,
                RevealWord = HiddenWord,
                Mistakes = Mistakes,
                TimeRemaining = SecondsLeft,
                CurrentLevel = CurrentLevel
            };

            _storageService.SaveGame(session);
            MessageBox.Show("Game saved successfully! Return to Login screen.");

            var loginWindow = new LoginWindow();
            loginWindow.Show();

            var currentWindow = Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w is Views.GameWindow);

            currentWindow?.Close();
        }

        private void ExecuteOpen()
        {
            var saves = _storageService.GetSavesForUser(_currentUser.Name);
            var currentSave = saves.FirstOrDefault(s => s.Category == _category);

            if (currentSave != null)
            {
                _timer.Stop();

                TargetWord = currentSave.CurrentWord;
                HiddenWord = currentSave.RevealWord;
                Mistakes = currentSave.Mistakes;
                SecondsLeft = currentSave.TimeRemaining;
                CurrentLevel = currentSave.CurrentLevel;

                Alphabet.Clear();
                for (char c = 'A'; c <= 'Z'; c++)
                {
                    string letterStr = c.ToString();
                    bool isGuessed = HiddenWord.Contains(letterStr, StringComparison.OrdinalIgnoreCase);
                    Alphabet.Add(new AlphabetLetter { Letter = letterStr, IsActive = !isGuessed });
                }

                UpdateHangmanImage();
                _timer.Start();
                MessageBox.Show("Save loaded!");
            }
            else
            {
                MessageBox.Show("There is no save for this category.");
            }
        }

        private void ExecuteShowStats()
        {
            _timer.Stop();
            var statsVM = new StatsViewModel(_currentUser.Name);
            var statsWindow = new Views.StatsWindow();
            statsWindow.DataContext = statsVM;

            statsWindow.ShowDialog();
            _timer.Start();
        }

        private void ExecuteExit()
        {
            _timer.Stop();
            var loginWindow = new LoginWindow();
            loginWindow.Show();

            var currentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is Views.GameWindow);
            currentWindow?.Close();
        }

        private void UpdateHangmanImage()
        {
            CurrentHangmanImage = $"/Resources/stage{Mistakes}.jpeg";
        }

        private void DisableAllLetters()
        {
            foreach (var letter in Alphabet)
            {
                letter.IsActive = false;
            }
        }
    }
}
