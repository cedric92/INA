﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INA.Model;
using System.ComponentModel;



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
        public string loadedFiles
        {
            get { return _Model._loadedFiles; }
            set {
                _Model._loadedFiles = value;
                OnPropertyChanged("loadedFiles");         
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