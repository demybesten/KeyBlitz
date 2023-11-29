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
            }

            /*Controleer of enter is ingetoetst*/

            if (e.Key == Key.Enter)
                {


                    if (viewModel != null)
                    {
                        viewModel.Enter();
                    }
                    e.Handled = true; // Voorkom dat het evenement verder wordt verwerkt

                }
            /*Controleer of back is ingetoetst*/

            if (e.Key == Key.Back)
                {
                    if (viewModel != null)
                    {
                        viewModel.DeleteCharacter();
                    }
                }
               
           
            }







        }
    }
