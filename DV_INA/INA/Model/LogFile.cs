using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.ObjectModel;
using System.Windows.Threading;

namespace INA.Model
{
    class LogFile
    {
        #region Member
        ViewModel.ViewModel _vm;
        #endregion

        #region ctor
        public LogFile(ViewModel.ViewModel vm)
        {
            this._vm = vm;
        }
        #endregion

        #region Methods
        public void writeToFile(string message)
        {
            callVM(message);

            string path = Path.Combine((Environment.ExpandEnvironmentVariables("%USERPROFILE%") + @"\Desktop"), "Log.txt");
            //string path = Path.GetTempPath();
            //string path = Path.Combine(Path.GetTempPath(), "SaveFile.txt");

            // This text is added only once to the file.
            if (!File.Exists(path))
            {
                // Create a file to write to.
                File.WriteAllText(path, message);
            }
            else
            {
                try
                {
                    File.AppendAllText(path, message);
                }
                catch (Exception)
                {
                    Console.WriteLine("Error in Logfile: Write To File");
                }
               
            }
        }

        static readonly object _lock = new object();
        private void callVM(string message)
        {            
              /*  ObservableCollection<string> tmp = _vm._listViewInfo;
                //tmp.Add(message);
                this._vm._listViewInfo = tmp;

                string s = _vm._tbInfo + message;*/
            lock (_lock)
            {
                _vm._tbInfo += message;
            }
                
        }
        #endregion
    }
}
