using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman.Models
{
    public class GameSession
    {
        public string UserName { get; set; }
        public string CurrentWord { get; set; }
        public string RevealWord { get; set; }
        public int Mistakes { get; set; }
        public int TimeRemaining { get; set; }
        public string Category { get; set; }
        public int CurrentLevel { get; set; }

    }
}
