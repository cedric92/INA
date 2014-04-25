using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace INA.Model
{
    class FileSplit
    {
        #region Members

        QueueManagement _QueueManagement = new QueueManagement();
        // list for imported lines
        List<int> transactions = new List<int>();

        #endregion

        public FileSplit()
        {

        }

        #region Getter/Setter

        public List<int> getTransactions
        {
        get{return transactions;}
        }

        #endregion

        internal QueueManagement QueueManagement
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        #region Methods
        // split file into lines
        public void splitFile(string fileName)
        {
            
            int sum = 0;
            int linecount = 0;

            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    string line = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        linecount++;
                        int value = 0;

                        //try to parse the variable line to an int
                        if (int.TryParse(line, out value))
                        {
                            transactions.Add(value);
                            //add value to sum to check if the file is balanced
                            sum += value;
                        }
                        else
                        {
                            //clear transactions because the file is corrupt
                            transactions.Clear();

                            MessageBox.Show("Ungültiger Wert in Datei\nWert: "+line+" in Zeile "+linecount);

                            //set sum = 0 to avoid last message box
                            sum = 0;

                            //stop loop
                            break;
                        }
                       
                    }

                    // if accordings arent balanced
                    if (sum != 0)
                    {
                        MessageBox.Show("Buchung nicht ausgeglichen");
                    }
                    // if everything is ok, put into messagequeue
                    else
                    {
                        _QueueManagement.startMessageQueue(transactions);

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion
    }
}
