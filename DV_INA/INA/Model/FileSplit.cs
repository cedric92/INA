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

        QueueManagement _QueueManagement = new QueueManagement();

        // save corrupt account entries, KeyValuePair simplifies access to key and value
        List<KeyValuePair<int, string>> errorTransactions = new List<KeyValuePair<int, string>>();

    #endregion

    #region Constructor

        public FileSplit()
        {

        }

    #endregion

    #region Methods

        // split file into lines
        public void splitFile(List<string> loadedFilePaths)
        {
            // file id for  queue
            int id = 0;
            
            // create new task
            foreach (var path in loadedFilePaths)
            {
                Task.Factory.StartNew(() => readFile(path, id++));
            }

            // show messages from messagequeue
           // QueueManagement.ReceiveStringMessageFromQueue();
        }

        // import and check files
        public void readFile(string filePath, int id)
        {           
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line = "";
                    int count = 0;

                    // save whole transactions, KeyValuePair simplifies access to key and value (acc-no + sum)
                    List<KeyValuePair<int, string>> transactionBlock = new List<KeyValuePair<int, string>>();

                    // send header to queue
                    _QueueManagement.startMessageQueue((new KeyValuePair<int, string>(id, "Header")).ToString());

                    // read file
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (!line.Contains('#'))
                        {
                            // add line to transactionBlock
                            transactionBlock.Add((new KeyValuePair<int, string>(id, line)));
                        }
                        else
                        {
                            if (transactionBlock.Count > 0)
                            {
                                // check transactionBlock
                                if (!checkTextLines(transactionBlock, filePath))
                                {
                                    // if file is corrupt add transaction to error-list
                                    errorTransactions.AddRange(transactionBlock);
                                    transactionBlock.Clear();
                                }
                                else
                                {
                                    foreach (var t in transactionBlock)
	                                {
                                        count++;
                                        // send string to queue
                                        _QueueManagement.startMessageQueue(t.ToString());
	                                }
                                    // clear transactionBlock
                                    transactionBlock.Clear(); 
                                }
                            }
                        }
                    }

                    // send footer to queue, add count
                    _QueueManagement.startMessageQueue((new KeyValuePair<int, string>(id, "Footer#" + count)).ToString());

                    count = 0; 
                }

            }
            catch (ArgumentOutOfRangeException)
            {
            }
            catch (Exception)
            {
            }

        }

        //check if the entries in the list are valid. 
        // fileName is used to show errormessages
        private bool checkTextLines(List<KeyValuePair<int, string>> transactionBlock, string fileName)
        {
            int sum = 0;
            int value = 0;

            foreach (var line in transactionBlock)
            {
                //try to parse to an int
                if (int.TryParse(deleteFirstLetters(line.Value, " "), out value))
                {
                    sum += value;
                }
                else
                {
                 //   MessageBox.Show("Error in File " + getFileNameFromPath(fileName) + ": " + deleteFirstLetters(line.Value, " ") 
                  //      + " is not an int.\nSkipped concerned accounting record.");
                    return false;
                }
            }
            //check if the transaction block is balanced (sum = 0)
            if (sum != 0)
            {
             //   MessageBox.Show("Error in File " + getFileNameFromPath(fileName) 
             //       + ": Sum is not balanced.\nSkipped concerned accounting record.");
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
                // delete chars until delSign (space)
                if ((line[0] != delSign[0]) && !delPosOver)
                {
                    line = line.Remove(0, 1);
                }
                else
                {
                    delPosOver = true;
                }
            }

            // remove delSign (space)
            line = line.Remove(0, 1);
            
            // return new string
            return line;
        }

        //returns the file name according to the given param filePath
        private string getFileNameFromPath(string filePath)
        {
            char c = '\\';
            //get last position of \ in absolute path
            int pos = filePath.LastIndexOf(c);
            //cut the path at the last pos of \ => shows only the file name without absolute path
            string sub = filePath.Substring(pos + 1);

            return sub;
        }
    
    #endregion
    }


}
