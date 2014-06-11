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
        LogFile _Logfile;
        ProgressBarControl _ProgressBarControl;
        #endregion

        public ViewModel()
        {
            _Logfile = new LogFile(this);
            _ProgressBarControl = new ProgressBarControl(this);
            _Model = new Model.Model(_Logfile, _ProgressBarControl);
            
        }

        #region Getter/Setter

        public string _textBoxInfo
        {
            get { return _Model._textBoxInfo; }
            set
            {
                _Model._textBoxInfo = value;
                OnPropertyChanged("_textBoxInfo");
            }
        }

        public ObservableCollection<string> _loadedFiles
        {
            get { return _Model._loadedFiles; }
            set
            {
                _Model._loadedFiles = value;
                OnPropertyChanged("_loadedFiles");
            }
        }

        public string setloadedFilesWithAbsolutePath
        {
            set { _Model.setloadedFilesWithAbsolutePath = value; }
        }

        public double ProgressStatus
        {
            get { return _Model._progressStatus; }
            set
            {
                _Model._progressStatus = value;
                OnPropertyChanged("ProgressStatus");
            }
        }
        #endregion

        #region Methods

        public void splitFiles()
        {
            _Model.splitFiles();

        }
        public void startTasks()
        {
            _Model.startTasks();
        }
        public bool compareFilePath(string s)
        {
            return _Model.compareFilePath(s);
        }
        public void clearFilePath(int index)
        {
            _Model.clearFilePath(index);
            OnPropertyChanged("_loadedFiles");
        }
        #endregion
    }
}
