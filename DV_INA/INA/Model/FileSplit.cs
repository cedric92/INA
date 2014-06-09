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

    class FileSplit : QueueManagement
    {
    
    #region Members

        MultiTasking _MultiTasking;
        LogFile _Logfile;

    #endregion

    #region Constructor

        public FileSplit(LogFile f)
        {
          this._Logfile = f;
          _MultiTasking = new MultiTasking(f);
        }

    #endregion

    #region Methods

        // split file into lines
        public void splitFile(List<string> loadedFilePaths)
        {
            // file id for  queue
            int id = 0;

            // write to log file
            _Logfile.writeToFile("### Start Import: "
                + DateTime.Now.ToShortTimeString().ToString() + ", "
                + DateTime.Now.ToString("dd-MM-yyyy")
                + Environment.NewLine);

            /*Info für Parallel:
             * http://stackoverflow.com/questions/5009181/parallel-foreach-vs-task-factory-startnew
             * */
            var loopResult = Parallel.ForEach<string>(loadedFilePaths, path => readFile(path, id++));

            // fuer anhalten button (kommt noch)
            if (!loopResult.IsCompleted && !loopResult.LowestBreakIteration.HasValue)
            {
                // write to log file
                _Logfile.writeToFile("### Import aborted ############" + Environment.NewLine);
            }

            // if loop successful
            if (loopResult.IsCompleted)
            {
                // write to log file
                _Logfile.writeToFile("### Import successful ############" + Environment.NewLine);
            }
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
                    startMessageQueue((new KeyValuePair<int, string>(id, "Header")).ToString());
                    
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
                                    foreach (var tline in transactionBlock)
                                    {
                                        _Logfile.writeToFile("ERROR: " + Path.GetFileName(filePath) + ", "
                                            + tline.ToString() + Environment.NewLine);
                                    }
                                    transactionBlock.Clear();
                                }
                                else
                                {
                                    foreach (var t in transactionBlock)
	                                {
                                        count++;
                                        // send string to queue
                                        startMessageQueue(t.ToString());
	                                }
                                    // clear transactionBlock
                                    transactionBlock.Clear(); 
                                }
                            }
                        }
                    }

                    // send footer to queue, add count
                   startMessageQueue((new KeyValuePair<int, string>(id, "Footer " + count)).ToString());

                    count = 0; 
                }

            }
            catch (ArgumentOutOfRangeException)
            {
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        //check if the entries in the list are valid. 
        // fileName is used to show errormessages
        private bool checkTextLines(List<KeyValuePair<int, string>> transactionBlock, string filePath)
        {
            int sum = 0;
            int value = 0;

            bool check = true;

            foreach (var line in transactionBlock)
            {
                // split line.Value: acc no, amount
                string[] transaction  = line.Value.Split(' ');
                //try to parse to an int
                if (int.TryParse(transaction[1], out value) && check)
                    {
                        sum += value;
                    }
                    else
                    {
                        check = false;
                   }

            }
            //check if the transaction block is balanced (sum = 0)
            if (sum != 0)
            {
                check = false;
            }

            return check;
        }

        #region Methods
      
        #endregion
    
    #endregion

        public void startTasks()
        {
            _MultiTasking.startTasks();
        }
    }


}
