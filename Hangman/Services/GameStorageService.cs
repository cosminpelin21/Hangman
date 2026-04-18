using Hangman.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace Hangman.Services
{
    public class GameStorageService
    {
        private readonly string _savesPath = "saves.json";

        public void SaveGame(GameSession session)
        {
            var allSaves = GetAllSaves();
            var existingSave=allSaves.FirstOrDefault(s=>s.UserName==session.UserName
                                                    && s.Category==session.Category);
            if (existingSave != null) {
                int index = allSaves.IndexOf(existingSave);
                allSaves[index] = session;
            }
            else allSaves.Add(session);

            string json = JsonSerializer.Serialize(allSaves);
            File.WriteAllText(_savesPath,json);
        }

        public List<GameSession> GetAllSaves()
        {
            if(!File.Exists(_savesPath))
                return new List<GameSession>();
            string json=File.ReadAllText(_savesPath);
            return JsonSerializer.Deserialize<List<GameSession>>(json) ?? new List<GameSession>();
        }

        public List<GameSession> GetSavesForUser(string userName)
        {
            return GetAllSaves().Where(s=>s.UserName==userName).ToList();
        }

        public void SaveAllSessions(List<GameSession> sessions)
        {
            string json = JsonSerializer.Serialize(sessions, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_savesPath, json);
        }

        public void DeleteSavesForUser(string userName)
        {
            var remainingSaves = GetAllSaves().Where(s => s.UserName != userName).ToList();
            string json = JsonSerializer.Serialize(remainingSaves);
            File.WriteAllText(_savesPath, json);
        }
    }
}
