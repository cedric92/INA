using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA.Model
{
    // ---------- MODEL ---------------------------------------
    class Model
    {
        #region Members
        FileSplit _FileSplit;
        //QueueManagement _QueueManagement;

        private string loadedFiles = "";
        private List<string> loadedFilesWithAbsolutePath = new List<string>();
        #endregion
        
        public Model()
        {
            _FileSplit = new FileSplit();

         
           // _QueueManagement = new QueueManagement();
        }
        #region Getter/Setter
        internal FileSplit FileSplit
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public string _loadedFiles
        {
            get { return loadedFiles; }
            set { this.loadedFiles = loadedFiles+"\n"+value; }
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
    }
}
