using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hellscape
{
    public class Observer
    {
        /*
         * Base class for observers. subclasses implement the onNotify
         * which calls functions depending on the event recieved and the associated entity
         * 
         * **Example of code feature beyond taught example (Observer pattern)
         */
        public virtual void onNotify( Entity entity, Event _event)
        {

        }
    }
}
