using System;
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

            string path = txtPath.Text;
            string format = cbFileformat.Text;
            string filename = txtFilename.Text;

            if (format == "TXT")
            {            
                Random rnd_sum = new Random();
               // Random rnd_acc = new Random();

                int sum = 0;
                int value = 0;

                    using (StreamWriter sw = File.CreateText(path + filename + "." + format))
                    {
                        for (int i = 0; i < lines-1; i++)
                        {
                            value = rnd_sum.Next(-1000, 1000);
                            sum += value;

                           // sw.Write(rnd_acc.Next(1, 100)); 
                           // sw.WriteLine(" " + value.ToString());

                            sw.WriteLine(value.ToString());
                            
                        }

                       // sw.Write(rnd_acc.Next(1, 100));
                       // sw.WriteLine(" " + sum * -1);

                        sw.WriteLine(sum * -1);
  
                    }

                    MessageBox.Show("Die Datei wurde erfolgreich erstellt.\nPfad: " + path + filename + "." + format, "Information");

            }
            else if(format == "XML")
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

                doc.Save(path + filename + "." + format);

                MessageBox.Show("Die Datei wurde erfolgreich erstellt.\nPfad: " + path + filename + "." + format, "Information");

            }
            else
            {
                MessageBox.Show("Bitte ein Dateiformat auswählen!", "Warnung");
            }
        }

        private void cbFileformat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
