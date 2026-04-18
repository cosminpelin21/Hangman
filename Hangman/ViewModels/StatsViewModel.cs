using Hangman.Helpers;
using Hangman.Models;
using Hangman.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman.ViewModels
{
    public class StatsViewModel : BaseViewModel
    {
        private readonly StatisticService _statisticService;
        public ObservableCollection<UserStatistic> AllUsersStatistics { get; set; }
        private UserStatistic _currentUserStats;
        public string UserName => _currentUserStats?.UserName ?? "N/A";
        public int TotalGames => _currentUserStats?.GamesPlayed ?? 0;
        public int TotalWins => _currentUserStats?.GamesWon ?? 0;
        public List<KeyValuePair<string, int>> CategoryWins =>
            _currentUserStats?.WinsByCategory?.ToList() ?? new List<KeyValuePair<string, int>>();
        public StatsViewModel(string userName)
        {
            _statisticService = new StatisticService();

            var allStats = _statisticService.GetAllStatistics();
            _currentUserStats = allStats.FirstOrDefault(s => s.UserName == userName);

            if (_currentUserStats == null)
            {
                _currentUserStats = new UserStatistic
                {
                    UserName = userName,
                    WinsByCategory = new Dictionary<string, int>()
                };
            }
        }
    }
}
