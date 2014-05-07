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
        #endregion

        public FileSplit()
        {

        }

        #region Getter/Setter

        #endregion

        

        #region Methods
        // split file into lines
        public void splitFile(List<string> loadedFilePaths)
        {
            List<Task> tasks = new List<Task>();
            
            foreach (var path in loadedFilePaths)
            {
                tasks.Add(Task.Factory.StartNew(() => readFile(path), TaskCreationOptions.LongRunning));
            }


            //Task.WaitAll(tasks.ToArray());

        }
        #endregion

        public void readFile(string filePath)
        {
            ID++;

            //first string defines the file id
            //2nd string defines the transaction value
            List<KeyValuePair<string, string>> transactionsForQueue = new List<KeyValuePair<string, string>>();

            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {  
                    string line = "";
                    List<string> transactionBlock = new List<string>();

                    transactionsForQueue.Add(new KeyValuePair<string, string>(ID.ToString(), "header"));

                    while ((line = sr.ReadLine()) != null)
                    {
                        
                        if (!line.Contains('#'))
                        {
                            string s = deleteFirstLetters(line, " ");
                            transactionBlock.Add(s);

                           /* //fügt transaktions-wert + blz hinzu
                            transactionsForQueue.Add(new KeyValuePair<string, string>(ID.ToString(), line));
                            */
                            
                            //fügt nur den transaktions-wert hinzu (ohne blz)
                            transactionsForQueue.Add(new KeyValuePair<string, string>(ID.ToString(), s));
                             
                        }
                        else
                        {

                            if (transactionBlock.Count > 0)
                            {
                                if (! checkTextLines(transactionBlock, filePath))
                                {
                                    transactionBlock.Clear();
                                    transactionsForQueue.Clear();
                                    break;                     
                                }
                                else
                                {
                                    
                                } 
                            }
                        }
                    }

                    if (transactionsForQueue.Count()>0)
                    {
                        transactionsForQueue.Add(new KeyValuePair<string, string>(ID.ToString(), ("footer#" + ID)));

                        _QueueManagement.startMessageQueue(transactionsForQueue);
                        transactionsForQueue.Clear();  
                    }
                      
                }
            }
            catch (ArgumentOutOfRangeException)
            {
            }
            catch (Exception)
            {
            }
        }

        //check if the given entries in the list are valid. fileName is used to show errormessages
        private bool checkTextLines(List<string> transactionBlock, string fileName)
        {
            int sum = 0;
            int value = 0;
            foreach (var item in transactionBlock)
            {
                //check if the line is a number
                if (int.TryParse(item, out value))
                {
                    sum += value;
                }
                else
                {
                    MessageBox.Show("Error: file "+getFileNameFromPath(fileName)+" contains not a number");
                    return false;
                }
            }
            //check if the transaction block is balanced ( sum = 0)
            if (sum != 0)
            {
                MessageBox.Show("Error: file " + getFileNameFromPath(fileName) + " is not balanced");
                return false;
            }
            else
            {
                return true;
            }
        }

        //delete first letters of the given string until the given param delSign
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

        //returns only the file name according to the given param filePath
        private string getFileNameFromPath(string filePath)
        {
            char c = '\\';
            //get last position of \ in absolute path
            int pos = filePath.LastIndexOf(c);
            //cut the path at the last pos of \ => shows only the file name without absolute path
            string sub = filePath.Substring(pos + 1);
            //set text for loaded files => databinding

            return sub;
        }
    }


}
