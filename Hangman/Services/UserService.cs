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
    public class UserService
    {
        private readonly string _filePath = "users.json";

        public List<User> GetAllUsers()
        {
            if (!File.Exists(_filePath))
                return new List<User>();
            string json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<User>>(json);
        }

        public void SaveUsers(List<User> users)
        {
            string json = JsonSerializer.Serialize(users);
            File.WriteAllText(_filePath, json);
        }

        public void DeleteUsers(User userToDelete, List<GameSession> allSessions) 
        {
            var allUsers = GetAllUsers();

            var userInList = allUsers.FirstOrDefault(u => u.Name == userToDelete.Name);
            if (userInList != null) 
            {
                allUsers.Remove(userInList);
                SaveUsers(allUsers);
            }
            
            var remainingSessions=allSessions.Where(s=>s.UserName!=userToDelete.Name).ToList();
            var gameStorageService = new GameStorageService();
            gameStorageService.SaveAllSessions(remainingSessions);
            var statisticService = new StatisticService();
            statisticService.DeleteStatisticsForUser(userToDelete.Name);
        }
    }
}
