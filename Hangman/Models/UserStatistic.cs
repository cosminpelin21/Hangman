using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman.Models
{
    public class UserStatistic
    {
        public string UserName { get; set; }
        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }
        public Dictionary<string, int> WinsByCategory { get; set; } = new Dictionary<string, int>();
        public UserStatistic() { }
    }
}
