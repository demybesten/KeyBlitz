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
        public NewTestViewModel() {
            _textLength = 20;
            ComplexityLevels.Add("basic");
            ComplexityLevels.Add("average");
            ComplexityLevels.Add("advanced");
            TextTypes.Add("story");
            TextTypes.Add("sentences");
            TextTypes.Add("words");
            Languages.Add("english");
            Languages.Add("dutch");
            Languages.Add("german");
            Languages.Add("french");
        }

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

        private ObservableCollection<string> _textTypes = new ObservableCollection<string>();
        public ObservableCollection<string> TextTypes {
            get {
                return _textTypes;
            }
            set {
                _textTypes = value;
                OnPropertyChanged(nameof(TextTypes));
            }
        }

        private string _textType = "story";
        public string TextType {
            get {
                return _textType;
            }
            set {
                _textType = value;
                OnPropertyChanged(nameof(TextType));
            }
        }

        private ObservableCollection<string> _languages = new ObservableCollection<string>();
        public ObservableCollection<string> Languages {
            get {
                return _languages;
            }
            set {
                _languages = value;
                OnPropertyChanged(nameof(Languages));
            }
        }

        private string _language = "english";
        public string Language {
            get {
                return _language;
            }
            set {
                _language = value;
                OnPropertyChanged(nameof(Language));
            }
        }

        private string _textSubject = "random";
        public string TextSubject {
            get {
                return _textSubject;
            }
            set {
                _textSubject = value;
                OnPropertyChanged(nameof(TextSubject));
            }
        }
    }
}
