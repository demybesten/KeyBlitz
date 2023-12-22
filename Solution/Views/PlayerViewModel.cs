using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Views
{
    public class PlayerViewModel
    {
        private string _playerName;

        public string PlayerName
        {
            get { return _playerName; }
            set
            {
                if (_playerName != value)
                {
                    _playerName = value;
                    OnPropertyChanged(nameof(PlayerName));
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
