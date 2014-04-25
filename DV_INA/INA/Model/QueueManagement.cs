using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INA.Model
{
    class QueueManagement
    {
        #region Members
        FileSplit _FileSplit;
        #endregion

        public QueueManagement()
        {
            _FileSplit = new FileSplit();
        }

        internal MultiThreading MultiThreading
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
