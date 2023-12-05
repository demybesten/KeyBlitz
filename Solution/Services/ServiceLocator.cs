using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Services
{
    public class ServiceLocator
    {
        private static ITextUpdater? _textUpdater;

        public static void RegisterTextUpdater(ITextUpdater textUpdater)
        {
            _textUpdater = textUpdater;
        }

        public static ITextUpdater? GetTextUpdater()
        {
            return _textUpdater;
        }
    }
}
