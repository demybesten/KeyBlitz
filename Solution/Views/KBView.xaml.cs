using Solution.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Solution.Views
{
    /// <summary>
    /// Interaction logic for KBView.xaml
    /// </summary>
    public partial class KBView : UserControl
    {
        public KBView()
        {
            InitializeComponent();
            DataContext = new KBViewModel();
           
        }

        /*Spatie functie verder neergezet via code behind om werking van spatie te laten werken*/     
        



        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            KBViewModel viewModel = DataContext as KBViewModel;

            /*Controleer of spatie is ingetoetst*/
            if (e.Key == Key.Space)
            {


                if (viewModel != null)
                {
                    viewModel.Space();
                }
                e.Handled = true; // Voorkom dat het evenement verder wordt verwerkt

            }
            if (e.Key == Key.LeftShift || e.Key == Key.RightShift || e.KeyboardDevice.IsKeyDown(Key.LeftShift) && e.Key == Key.Space || e.KeyboardDevice.IsKeyDown(Key.RightShift) && e.Key == Key.Space)
            {
                // Negeer de Shift-toetsen zelf
                return;
            }
            if (e.KeyboardDevice.IsKeyDown(Key.LeftShift) || e.KeyboardDevice.IsKeyDown(Key.RightShift))
            {
                // Shift is ingedrukt, voeg hoofdletter toe aan de stack
                viewModel.AddToKeyStack(e.Key.ToString().ToUpper());
            }
           
        }







    }
}
