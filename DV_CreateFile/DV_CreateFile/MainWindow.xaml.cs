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
using System.IO;
using System.Xml;


namespace DV_CreateFile
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void btCreate_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog saveDialog = new Microsoft.Win32.SaveFileDialog();
            saveDialog.FileName = "Document"; // Default file name
            saveDialog.DefaultExt = ".txt"; // Default file extension
            saveDialog.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            string filename = null;

            // Show save file dialog box
            Nullable<bool> result = saveDialog.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                filename = saveDialog.FileName;
            } 
            
            int lines = 0;

            if (Convert.ToInt32(txtLines.Text) > 0)
            {
                 lines = Convert.ToInt32(txtLines.Text);
            }
            else
            {
                lines = 100;
                MessageBox.Show("Es wurde eine Datei mit 100 Zeilen erstellt.", "Information");
            }

            string extension = Path.GetExtension(saveDialog.FileName);

            if (extension == ".txt")
            {            
                Random rnd_sum = new Random();
               // Random rnd_acc = new Random();

                int sum = 0;
                int value = 0;

                if (chbBuggy.IsChecked == true)
                {
                    Random rnd_checked = new Random();
                    int bug = rnd_checked.Next(1, lines-1);

                    using (StreamWriter sw = File.CreateText(filename))
                    {
                        for (int i = 0; i < lines - 1; i++)
                        {
                            if (i == bug)
                            {
                                sw.WriteLine("BUG");
                            }
                            else
                            {
                                value = rnd_sum.Next(-1000, 1000);
                                sum += value;

                                // sw.Write(rnd_acc.Next(1, 100)); 
                                // sw.WriteLine(" " + value.ToString());

                                sw.WriteLine(value.ToString());
                            }

                        }

                        if (chbWrongamount.IsChecked == true)
                        {
                            sw.WriteLine(rnd_sum.Next(-1000, 1000));
                        }
                        else
                        {
                            // sw.Write(rnd_acc.Next(1, 100));
                            // sw.WriteLine(" " + sum * -1);

                            sw.WriteLine(sum * -1);
                        }

                    }
                }
                else
                {
                    using (StreamWriter sw = File.CreateText(filename))
                    {
                        for (int i = 0; i < lines - 1; i++)
                        {
                            value = rnd_sum.Next(-1000, 1000);
                            sum += value;

                            // sw.Write(rnd_acc.Next(1, 100)); 
                            // sw.WriteLine(" " + value.ToString());

                            sw.WriteLine(value.ToString());

                        }

                        if (chbWrongamount.IsChecked == true)
                        {
                            sw.WriteLine(rnd_sum.Next(-1000, 1000));

                        }
                        else
                        {
                            // sw.Write(rnd_acc.Next(1, 100));
                            // sw.WriteLine(" " + sum * -1);

                            sw.WriteLine(sum * -1);
                        }

                    }

                }


                    MessageBox.Show("Die Datei wurde erfolgreich erstellt.\nPfad: " + filename, "Information");

            }
       /*     else if (extension == ".xml")
            {
                // experimental

                XmlDocument doc = new XmlDocument();
                XmlNode myRoot, myNode;
               // XmlAttribute myAttribute;

                Random rnd_sum = new Random();
                Random rnd_acc = new Random();

                int sum = 0;
                int value = 0;

                // creare root
                myRoot = doc.CreateElement("AccountList");
                doc.AppendChild(myRoot);

                for (int i = 0; i < lines - 1; i++)
                {
                    value = rnd_sum.Next(-1000, 1000);
                    sum += value; 
                    
                    myNode = doc.CreateElement("Amount");

                    myNode.InnerText = value.ToString();

                  //  myAttribute = doc.CreateAttribute("accountNo");
                  //  myAttribute.InnerText = rnd_acc.Next(1, 100).ToString();
                  //  myNode.Attributes.Append(myAttribute);

                    myRoot.AppendChild(myNode);  

                }

                myNode = doc.CreateElement("Amount");

                myNode.InnerText = (sum * -1).ToString();

                // myAttribute = doc.CreateAttribute("accountNo");
                // myAttribute.InnerText = rnd_acc.Next(1, 100).ToString();
                // myNode.Attributes.Append(myAttribute);

                myRoot.AppendChild(myNode);  

                doc.Save(filename);

                MessageBox.Show("Die Datei wurde erfolgreich erstellt.\nPfad: " + filename, "Information");

            }
*/
        }

        private void cbFileformat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
