using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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

        private void callVM(string message)
        {
            this._vm._textBoxInfo = message;
        }
        #endregion
    }
}
