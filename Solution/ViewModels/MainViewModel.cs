using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.ViewModels
{
    public class MainViewModel : BaseViewModel {
        private object _currentViewModel;
        public object CurrentViewModel {
            get {
                return _currentViewModel;
            }
            set {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public MainViewModel() {
            _currentViewModel = new NewTestViewModel();
        }
    }
}
