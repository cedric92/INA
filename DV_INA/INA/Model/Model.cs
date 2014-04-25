using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA.Model
{
    
    class Model
    {
        #region Members
        FileSplit _FileSplit;
        QueueManagement _QueueManagement;
        #endregion
        
        public Model()
        {
            _FileSplit = new FileSplit();
            _QueueManagement = new QueueManagement();
        }

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
    }
}
