using System;
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

        protected void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
                //NotifyPropertyChanged("name");
            }
        }

        #endregion

        #region Members
        Model.Model _Model;
        #endregion

        public ViewModel()
        {
            _Model = new Model.Model();
        }

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
        
        public void splitFile(string fileName)
        {
            _Model.splitFile(fileName);
           
        }
        #endregion
    }
}
