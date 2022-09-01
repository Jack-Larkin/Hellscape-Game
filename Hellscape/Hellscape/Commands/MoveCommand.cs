using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hellscape
{

    // Subclass of Command used to encapsulate movement requests from enetities
    public class MoveCommand : Command
    {
        
        

        enum Direction
        {
            LEFT = MainGame.Direction.LEFT,
            RIGHT = MainGame.Direction.RIGHT,
            UP = MainGame.Direction.UP,
            DOWN = MainGame.Direction.DOWN
        }

        

        public MoveCommand(int _direction) : base(_direction)
        {
            direction = (MainGame.Direction)_direction;
           
            isMove = true;
        }

        

        public override void execute(Entity entity)
        {
            entity.setFacing((int)direction);
           
            
            entity.move((int)direction);
            
        }

        
    }
}
