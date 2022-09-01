using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hellscape
{

    /*
     * Subclass of command used to represent attack requests (both from player and enemy)
     * 
     */
    public class AttackCommand : Command
    {
        //int damage;
        //origin of attack
        
        //length, width of attack
        Vector2 range = new Vector2(1,1);

        enum attackType
        {
            LIGHT,
            HEAVY
        }

        attackType type;

        public AttackCommand(int dir): base(dir)
        {
            direction = (MainGame.Direction)dir;
            isAttack = true;
        }

        public AttackCommand(int dir, Vector2 attackRange, bool isHeavy =  false) : base(dir)
        {
            direction = (MainGame.Direction)dir;
            isAttack = true;
            range = attackRange;
            isLight = !isHeavy;
            if (isHeavy)
            {
                type = attackType.HEAVY;
            }
        }

        public override void execute(Entity entity)
        {
            entity.attack();
        }

        //public override Rectangle execute(Entity entity)
        //{
        //    entity.attack();

        //    if(direction == MainGame.Direction.LEFT)
        //    {
        //        return new Rectangle((int)entity.getPosition().X - 1, (int)entity.getPosition().Y, (int)range.X, (int)range.Y);
        //    }

        //    return new Rectangle();
        //}

        public override void execute(Entity entity, Entity[] targetList)
        {
            entity.attack();
            foreach(Entity target in targetList)
            {
                target.takeDamage(entity.getAttackDamage());
            }
        }
    }
}
