using Solution.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Solution.Services
{
    public interface ITextUpdater
    {
        void updateText(List<Word> words, bool resetWordWrap = false);
    }
}