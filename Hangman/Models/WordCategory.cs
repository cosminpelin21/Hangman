using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman.Models
{
    public class WordCategory
    {
        public string CategoryName { get; set; }
        public List<string> Words { get; set; } = new List<string>();
    }
}
