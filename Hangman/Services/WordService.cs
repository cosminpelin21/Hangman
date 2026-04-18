using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman.Services
{
    public class WordService
    {
        public List<string> GetWordsByCategory(string category)
        {
            if (category == "All categories")
            {
                var allWords = new List<string>();
                string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Words");

                if (Directory.Exists(folderPath))
                {
                    var files = Directory.GetFiles(folderPath, "*.txt");
                    foreach (var file in files)
                    {
                        allWords.AddRange(File.ReadAllLines(file));
                    }
                }
                return allWords;
            }

            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Words", $"{category}.txt");
            return File.Exists(path) ? File.ReadAllLines(path).ToList() : new List<string>();
        }

        public string GetRandomWord(string category)
        {
            var words=GetWordsByCategory(category);
            return words[new Random().Next(words.Count)];
        }
    }
}
