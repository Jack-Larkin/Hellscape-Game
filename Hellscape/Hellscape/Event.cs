using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hellscape
{

    // Contains information about events that are sent to observer classes
    public class Event
    {
        
        int numericalData = 0;
        public enum EventTypes
        {
            //event types here: EVENT_FALL, EVENT_MOVE etc
            MOVE,
            COMBO,
            DIE,
            ATTACK,

        }
        public EventTypes type;
        public Event(EventTypes _type, int data = 0)

        {
            EventTypes type = _type;
            numericalData = data;
        }
    }
}
