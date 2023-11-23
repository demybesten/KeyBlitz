using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.ViewModels
{
    public class NewTestViewModel : BaseViewModel{
        private int _textLength;
        public int TextLength {
            get {
                return _textLength;
            }
            set { 
                _textLength = value; 
                OnPropertyChanged(nameof(TextLength));
            }
        }

    }
}
