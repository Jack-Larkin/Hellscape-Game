using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hellscape
{

    /*
     * Base class to describe an object that can be observed by observer classes
     * Subject objects can notify which enables them to communicate with observers without being 
     * directly coupled to them
     * 
     * **Example of code feature beyond taught example (Observer pattern)
     */
    public class Subject
    {
        List<Observer> observerList = new List<Observer>();

        public void addObserver(Observer observer)
        {
            observerList.Add(observer);
        }

        public void removeObserver(Observer observer)
        {
            observerList.Remove(observer);
        }

        protected void notify(Entity entity, Event _event)
        {
            foreach( Observer observer in observerList)
            {
                observer.onNotify(entity, _event);
            }
        }
    }
}
