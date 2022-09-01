using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Hellscape
{
    /*
     Command class used to encapsulate movement and attacks as objects,
     such that these can be stored in listsd and that these requests can have attributes

        ** EXAMPLE OF CODE FEATURE BEYOND TAUGHT EXAMPLES (Command pattern)
        

     */



    public class Command
    {
        

        public bool isAttack = false;
        public bool isLight = false;

        public bool isMove = false;

        public Vector2 range = new Vector2(1,1);

        protected MainGame.Direction direction;

        

        public Command(int dir = 0)
        {
            direction = (MainGame.Direction)dir;
        }

        public Command() { }


        // this is ovverriden by subclasses and called to perform the object function
        public virtual void execute() { }

        public virtual void execute(Entity entity)
        {
            
        }

        public virtual void execute(Entity entity, Entity[] targetList) { }

        public int getDirection()
        {
            return (int)direction;
        }

        public void setRange(Vector2 newRange)
        {
            range = newRange;
        }
    }
}
