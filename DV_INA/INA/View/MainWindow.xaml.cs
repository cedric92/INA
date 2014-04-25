﻿using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using INA.ViewModel;
using System.IO;

namespace INA
{
    
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    /// 
    // ---------- VIEW ---------------------------------------
    public partial class MainWindow : Window
    {
        #region Members
        ViewModel.ViewModel _ViewModel;
        #endregion
       
        public MainWindow()
        {
            InitializeComponent();
            _ViewModel = new ViewModel.ViewModel();
        }

        public INA.ViewModel.ViewModel ViewModel
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
               //get absolute file path
                string filename = dlg.FileName;

                //call method splitFile which splits the chosen file according to the fileName
                _ViewModel.splitFile(filename);
            }
        }
    }
}
