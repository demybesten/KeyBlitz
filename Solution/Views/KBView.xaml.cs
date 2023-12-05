
﻿    using Solution.ViewModels;
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

            /*Spatie functie en backspace functie verder neergezet via code behind om werking van spatie en backspace te laten werken*/
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
                e.Handled = true;
            }
            /*Controleer of back is ingetoetst*/

            if (e.Key == Key.Back)
                {
                    if (viewModel != null)
                    {
                        viewModel.DeleteCharacter();
                    MyTextBox.Select(MyTextBox.Text.Length, 0);

                    }
            }
        }
        }
    }
