using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INA.Model;

namespace INA.ViewModel
{
    public class ViewModel
    {
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
