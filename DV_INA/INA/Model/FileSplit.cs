using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace INA.Model
{

    class FileSplit : QueueManagement
    {

        #region Members
        LogFile _Logfile;
        MultiTasking _MultiTasking;
        ProgressBarControl _ProgressBarControl;

        private int numberOfFiles = 0;
        #endregion

        #region Constructor

        public FileSplit(LogFile f, MultiTasking m, ProgressBarControl pbc)
        {
            this._Logfile = f;
            this._MultiTasking = m;
            this._ProgressBarControl = pbc;
        }

        #endregion

        #region Getter/setter

        #endregion

        #region Methods

        // split file into lines
        public void splitFile(List<string> loadedFilePaths)
        {
            // file id for  queue
            int id = 0;
            numberOfFiles = loadedFilePaths.Count();

            // write to log file
            /*   _Logfile.writeToFile("### Start Import: "
                   + DateTime.Now.ToShortTimeString().ToString() + ", "
                   + DateTime.Now.ToString("dd-MM-yyyy")
                   + Environment.NewLine);*/

            /*Info für Parallel:
             * http://stackoverflow.com/questions/5009181/parallel-foreach-vs-task-factory-startnew
             * */
            var loopResult =
            Task.Factory.StartNew(() =>
            Parallel.ForEach<string>(loadedFilePaths, path => readFile(path, id++))
            );

            /*
                        // fuer anhalten button (kommt noch)
                        if (!loopResult.IsCompleted && !loopResult.LowestBreakIteration.HasValue)
                        {
                            // write to log file
                            _Logfile.writeToFile("### Import aborted ############" + Environment.NewLine);
                        }*/
        }

        // import and check files
        public void readFile(string filePath, int id)
        {
            _Logfile.writeToFile("###Started to import file " + filePath+"###\n");

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


                    _Logfile.writeToFile("###File successfully imported###\n");


                    _ProgressBarControl.setProgressStatus(numberOfFiles);
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
                string[] transaction = line.Value.Split(' ');
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

        #endregion

        public void startTasks()
        {
            _MultiTasking.startTasks();
        }
    }


}
