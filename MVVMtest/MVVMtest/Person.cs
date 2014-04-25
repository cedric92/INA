using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMtest
{
    // ---------- MODEL ---------------------------------------
    public class Person
    {
        public string Name { get; set; }
        public int Alter { get; set; }

        public static Person CreatePerson()
        {
            return new Person { Name = "Franz", Alter = 45 };
        }
    }
}
