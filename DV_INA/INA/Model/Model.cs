using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace INA.Model
{
    // ---------- MODEL ---------------------------------------
    class Model
    {
        #region Members
        FileSplit _FileSplit;
        
        //QueueManagement _QueueManagement
        private List<string> loadedFilesWithAbsolutePath = new List<string>();
       
        private ObservableCollection<string> loadedFiles = new ObservableCollection<string>();

        string[] info = new string[7];
        
        int infoCount = 0;

        #endregion
        
        public Model(LogFile _Logfile)
        {
            _FileSplit = new FileSplit(_Logfile);
            info[0] = " ";
            info[1] = " ";
            info[2] = " ";
            info[3] = " ";
            info[4] = " ";
            info[5] = " ";
            info[6] = " ";
        }
        #region Getter/Setter
        public string _textBoxInfo
        {
            get {
                string s = "";
                for (int i = 0; i < info.Count(); i++)
                {
                    s += (info[i].ToString()+"\n");
                }
                return s; 
            }
            set
            {              
                info[infoCount] = value;

                infoCount++;
                if (infoCount == 7)
                {
                    infoCount = 0;
                }

            }
        }
        public ObservableCollection<string> _loadedFiles
        {
            get { return loadedFiles; }
            set { loadedFiles = value; }
        }

        public string setloadedFilesWithAbsolutePath
        {
            set { this.loadedFilesWithAbsolutePath.Add(value); }
        }
    
        #endregion
        

   /*    
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
    */
        #region Methods

        public void splitFiles()
        {

            _FileSplit.splitFile(loadedFilesWithAbsolutePath);
        }
        //check if the given parameter path already exists in the file path list
        public bool compareFilePath(string path)
        {
            for (int i = 0; i < loadedFilesWithAbsolutePath.Count; i++)
            {
                if (loadedFilesWithAbsolutePath.ElementAt(i).Equals(path))
                {
                   return true;
                }
            }
            return false;
        }

        #endregion

        public void startTasks()
        {
            _FileSplit.startTasks();
        }

        public void clearFilePath(int index)
        {
            
            string s1 = loadedFiles.ElementAt(index);
            loadedFiles.Remove(s1);

            string s2 = loadedFilesWithAbsolutePath.ElementAt(index);
            loadedFilesWithAbsolutePath.Remove(s2);
        }
    }
}
