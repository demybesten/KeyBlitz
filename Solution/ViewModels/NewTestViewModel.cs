using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public NewTestViewModel() {
            _textLength=20;
            ComplexityLevels.Add("basic");
            ComplexityLevels.Add("avarage");
            ComplexityLevels.Add("pro");
        }

        private ObservableCollection<string> _complexityLevels = new ObservableCollection<string>();
        public ObservableCollection<string> ComplexityLevels {
            get {
                return _complexityLevels;
            }
            set {
                _complexityLevels = value;
                OnPropertyChanged(nameof(ComplexityLevels));
            }
        }
    }
}
