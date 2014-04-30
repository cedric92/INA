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

        //string contains the file name
        //int contains the transaction value
        List<KeyValuePair<string, string>> transactions = new List<KeyValuePair<string, string>>();

        #endregion

        public FileSplit()
        {

        }

        #region Getter/Setter

        public List<KeyValuePair<string, string>> getTransactions
        {
            get { return transactions; }
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
        public void splitFile(List<string> loadedFilePaths)
        {
            List<KeyValuePair<string, string>> completeTransactions = new List<KeyValuePair<string, string>>();
            //defines the file id
            int fileID = 1;

            foreach (var fileName in loadedFilePaths)
            {
                //count each line
                int sum = 0;

                //count each line in file
                int linecount = 0;

                try
                {
                    using (StreamReader sr = new StreamReader(fileName))
                    {
                        string line = "";
                        transactions.Add(new KeyValuePair<string, string>(fileID.ToString(), "header"));
                        while ((line = sr.ReadLine()) != null)
                        {
                            linecount++;
                            int value = 0;

                            //try to parse the variable line to an int
                            if (int.TryParse(line, out value))
                            {
                                // transactions.Add(value);
                                transactions.Add(new KeyValuePair<string, string>(fileID.ToString(), value.ToString()));

                                //add value to sum to check if the file is balanced
                                sum += value;
                            }
                            else
                            {
                                //clear transactions because the file is corrupt
                                transactions.Clear();

                                MessageBox.Show("Ungültiger Wert in Datei\nWert: " + line + " in Zeile " + linecount);

                                //set sum = 0 to avoid last message box
                                sum = 0;

                                //stop loop
                                break;
                            }
                        }
                        transactions.Add(new KeyValuePair<string, string>(fileID.ToString(), ("footer#"+linecount)));

                        fileID++;
                        // if accordings arent balanced
                        if (sum != 0)
                        {
                            MessageBox.Show("Buchung nicht ausgeglichen.\nDie Datei kann nicht verarbeitet werden!");
                        }
                        // if everything is ok, put into messagequeue
                        else
                        {
                            completeTransactions.AddRange(transactions);
                        }
                        transactions.Clear();
                    }                   
                }

                catch (Exception)
                {
                    //nich so geil

                }


            }
            //finally call startMessageQueue
            _QueueManagement.startMessageQueue(completeTransactions);
        }
        #endregion
    }
}
