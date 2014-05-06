using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Windows;


namespace INA.Model
{

    class FileSplit
    {
        #region Members

        //defines the file id
        private int ID = 0;

        QueueManagement _QueueManagement = new QueueManagement();
        // list for imported lines

        //completeTransactions will be 
        List<KeyValuePair<string, string>> completeTransactions = new List<KeyValuePair<string, string>>();

        #endregion

        public FileSplit()
        {

        }

        #region Getter/Setter

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
            List<Task> tasks = new List<Task>();
            
            foreach (var path in loadedFilePaths)
            {
                tasks.Add(Task.Factory.StartNew(() => readFiles(path), TaskCreationOptions.LongRunning));
            }


            //Task.WaitAll(tasks.ToArray());

            //finally call startMessageQueue
            _QueueManagement.startMessageQueue(completeTransactions);

        }
        #endregion

        public void readFiles(string filePath)
        {
            ID++;
            //string contains the file name
            //int contains the transaction value
            List<KeyValuePair<string, string>> transactions = new List<KeyValuePair<string, string>>();

            int sum = 0;

            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line = "";


                    List<string> entry = new List<string>();

                    transactions.Add(new KeyValuePair<string, string>(ID.ToString(), "header"));

                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Contains('#'))
                        {
                           // so schonmal nicht :o)
                            if (checkTextLines(entry))
	                        {
		                        transactions.
	                        }
                            
                            entry.Clear();
                        }
                        else
                        {
                            entry.Add(deleteFirstLetters(line, " "));


                                // transactions.Add(value);
                                transactions.Add(new KeyValuePair<string, string>(ID.ToString(), value.ToString()));

                        
                                //clear transactions because the file is corrupt
                                transactions.Clear();

                                MessageBox.Show("Ungültiger Wert in Datei\nWert: " + line + " in Zeile " + 1);

                                //set sum = 0 to avoid last message box
                                sum = 0;

                                //stop loop
                                break;
                            
                        }

                    }

                    transactions.Add(new KeyValuePair<string, string>(ID.ToString(), ("footer#" + ID)));

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
            catch (ArgumentOutOfRangeException)
            {

            }
            catch (Exception)
            {


            }

        }

        private bool checkTextLines(List<string> line)
        {

           if (line.Contains('#'))
           {
               return false;
           }
                        else
                        {
                            line = deleteFirstLetters(line, " ");

                            int value = 0;

                            //check if the transaction is a number
                            //try to parse the variable line to an int
                            if (int.TryParse(line, out value))
                            {
                                // transactions.Add(value);
                                transactions.Add(new KeyValuePair<string, string>(ID.ToString(), value.ToString()));

                                //add value to sum to check if the file is balanced
                                sum += value;
                            }
                            else
                            {
                                //clear transactions because the file is corrupt
                                transactions.Clear();

                                MessageBox.Show("Ungültiger Wert in Datei\nWert: " + line + " in Zeile " + 1);

                                //set sum = 0 to avoid last message box
                                sum = 0;

                                //stop loop
                                break;
                            }
                        }

                    }

                    transactions.Add(new KeyValuePair<string, string>(ID.ToString(), ("footer#" + ID)));

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

            return true;
       
}

        private string deleteFirstLetters(string line, string delSign)
        {
            bool delPosOver = false;

            for (int i = 0; i < line.Length; i++)
            {
                if ((line[0] != delSign[0]) & !delPosOver)
                {
                    line = line.Remove(0, 1);
                }
                else
                {
                    delPosOver = true;
                }
            }

            line = line.Remove(0, 1);
            return line;
        }
    }


}
