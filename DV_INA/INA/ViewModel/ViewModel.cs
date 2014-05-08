using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INA.Model;
using System.ComponentModel;
using System.Collections.ObjectModel;



namespace INA.ViewModel
{
    // ---------- VIEWMODEL ---------------------------------------
    public class ViewModel : INotifyPropertyChanged
    {
        #region NotifiyPropertyChanged

       
        public event PropertyChangedEventHandler PropertyChanged;

        protected internal void OnPropertyChanged(string propertyname)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyname));
        }

        #endregion

        #region Members
        Model.Model _Model;
        #endregion

        #region Getter/Setter
        public ViewModel()
        {
            _Model = new Model.Model();
        }
      
        //to delete
        public ObservableCollection<string> _testList
        {
            get { return _Model._testList; }
            set
            {
                _Model._testList = value;
                OnPropertyChanged("_testList");
            }
        }
      
        
        public string setloadedFilesWithAbsolutePath
        {
            set { _Model.setloadedFilesWithAbsolutePath = value; }
        }
        #endregion
       

        internal INA.Model.Model Model
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
        
        public void splitFiles()
        {
            _Model.splitFiles();
           
        }
        public bool compareFilePath(string s)
        {
            return _Model.compareFilePath(s);
        }
        #endregion
    }
}
