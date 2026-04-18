using Hangman.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hangman.Services
{
    public class StatisticService
    {
        private readonly string _statsPath = "statistics.json";

        public void UpdateStatistics(string userName, string category, bool isWin, bool isNewGame=false)
        {
            var allStats = GetAllStatistics();

            var userStat = allStats.FirstOrDefault(s => s.UserName == userName);

            if (userStat == null)
            {
                userStat = new UserStatistic { UserName = userName };
                allStats.Add(userStat);
            }

            if (isNewGame)
            {
                userStat.GamesPlayed++;
            }

            if (userStat.WinsByCategory == null)
            {
                userStat.WinsByCategory = new Dictionary<string, int>();
            }

            if (isWin)
            {
                userStat.GamesWon++;
                if (!userStat.WinsByCategory.ContainsKey(category))
                    userStat.WinsByCategory[category] = 0;
                userStat.WinsByCategory[category]++;
            }
            SaveStatistics(allStats);
        }

        public List<UserStatistic> GetAllStatistics()
        {
            if (!File.Exists(_statsPath))
                return new List<UserStatistic>();
            string json = File.ReadAllText(_statsPath);
            return JsonSerializer.Deserialize<List<UserStatistic>>(json) ?? new List<UserStatistic>();
        }

        private void SaveStatistics(List<UserStatistic> stats)
        {
            string json = JsonSerializer.Serialize(stats, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_statsPath, json);
        }
        public void DeleteStatisticsForUser(string userName)
        {
            var allStats = GetAllStatistics();
            var userStat = allStats.FirstOrDefault(s => s.UserName == userName);
            if (userStat != null)
            {
                allStats.Remove(userStat);
                SaveStatistics(allStats);
            }
        }
    }
}
