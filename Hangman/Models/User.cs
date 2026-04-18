using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman.Models
{
    public class User
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }
        public Dictionary<string, int> GamesWonByCategory { get; set; } = new Dictionary<string, int>();

        public User() { }
    }
}
