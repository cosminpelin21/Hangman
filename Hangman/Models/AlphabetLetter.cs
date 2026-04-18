using Hangman.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hangman.Models
{
    public class AlphabetLetter : BaseViewModel
    {
        private string _letter;
        public string Letter { get => _letter; set { _letter = value; OnPropertyChanged(nameof(Letter)); } }

        private bool _isActive = true;
        public bool IsActive { get => _isActive; set { _isActive = value; OnPropertyChanged(nameof(IsActive)); } }
    }
}
