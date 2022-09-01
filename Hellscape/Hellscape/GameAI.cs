using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hellscape
{

    /*
     * GAME AI class, invokes functions of the current entity to make it take an appropriate move 
     * depending on the target's position.
     * 
     * While currently only implemented to be used by enemies, the majority of this code is written to specify 
     * the entity class such that this could potentially be used to automatically complete the player turn as well
     * 
     * Example of a more sophisticated coding structure (AI)
     * 
     */
    public class GameAI
    {
        Entity currentEntity;
        bool actionTaken = false;
        public GameAI() { }


        public void takeEnemyTurn(Entity target,List<Tile> tileList)
        {
            actionTaken = false;
            //if player is within sight radius
            //attack player or move towards them
            if (isTargetInSight(target.getPosition())){
                foreach(MainGame.Direction direction in Enum.GetValues(typeof(MainGame.Direction))){
                    //if adjacent, break and attack
                    if (isTargetAdjacent(target.getPosition(), direction))
                    {
                        //attack
                        currentEntity.setFacing((int)direction);
                        AttackCommand attack = new AttackCommand((int)direction, new Vector2(1, 1));
                        attack.execute(currentEntity);
                        target.takeDamage(currentEntity.getAttackDamage());
                        actionTaken = true;
                        break;
                    }
                }
                if (!actionTaken)
                {
                    approachEntity(target.getPosition(), tileList);
                    actionTaken = true;
                }

                
            }
        }

        //changes the entity to perform actions on
        public void setEntity(Entity entity)
        {
            currentEntity = entity;
        }

        //performs collision check for tiles and then moves
        public void approachEntity(Vector2 targetPosition, List<Tile> tileList)
        {
            if (currentEntity.getPosition().X < targetPosition.X  && canMove(tileList,MainGame.Direction.RIGHT,targetPosition))
            {

                MoveCommand move = new MoveCommand((int)MainGame.Direction.RIGHT);
                
                move.execute(currentEntity);

            }
            else if (currentEntity.getPosition().X > targetPosition.X && canMove(tileList, MainGame.Direction.LEFT, targetPosition))
            {
                MoveCommand move = new MoveCommand((int)MainGame.Direction.LEFT);
                move.execute(currentEntity);
            }

            if (currentEntity.getPosition().Y < targetPosition.Y && canMove(tileList, MainGame.Direction.DOWN, targetPosition))
            {
                MoveCommand move = new MoveCommand((int)MainGame.Direction.DOWN);
                move.execute(currentEntity);
            }
            else if (currentEntity.getPosition().Y > targetPosition.Y && canMove(tileList, MainGame.Direction.UP, targetPosition))
            {
                MoveCommand move = new MoveCommand((int)MainGame.Direction.UP);
                move.execute(currentEntity);
            }

            
        }

        //checks if position is within sight radius
        bool isTargetInSight(Vector2 objectPosition)
        {
            if (objectPosition.X >= currentEntity.getPosition().X - currentEntity.getSight() && objectPosition.X <= currentEntity.getPosition().X + currentEntity.getSight()
                && objectPosition.Y >= currentEntity.getPosition().Y - currentEntity.getSight() && objectPosition.Y <= currentEntity.getPosition().Y + currentEntity.getSight())
            {
                return true;
            }

            return false;
        }

        //checks if target is next to player
        bool isTargetAdjacent(Vector2 targetPosition, MainGame.Direction dir)
        {
            if(dir == MainGame.Direction.LEFT)
            {
                if(targetPosition.X == currentEntity.getPosition().X - 1 && targetPosition.Y == currentEntity.getPosition().Y)
                {
                    return true;
                }

            }
            else if (dir == MainGame.Direction.RIGHT)
            {
                if (targetPosition.X == currentEntity.getPosition().X + 1 && targetPosition.Y == currentEntity.getPosition().Y)
                {
                    return true;
                }

            }
            else if (dir == MainGame.Direction.UP)
            {
                if (targetPosition.Y == currentEntity.getPosition().Y - 1 && targetPosition.X == currentEntity.getPosition().X)
                {
                    return true;
                }

            }
            else if (dir == MainGame.Direction.DOWN)
            {
                if (targetPosition.Y == currentEntity.getPosition().Y + 1 && targetPosition.X == currentEntity.getPosition().X)
                {
                    return true;
                }

            }

            return false;
        }
        //check to see if the current entity can move to a certain tile
        bool canMove(List<Tile> tileList, MainGame.Direction dir, Vector2 targetPosition)
        {

            if (dir == MainGame.Direction.LEFT)
            {
                if(!tileList[3].getPassable() || tileList[3].position == targetPosition)
                {
                    return false;
                }
            }

            else if (dir == MainGame.Direction.RIGHT)
            {
                if (!tileList[5].getPassable() || tileList[5].position == targetPosition)
                {
                    return false;
                }
            }

            else if (dir == MainGame.Direction.UP)
            {
                if (!tileList[2].getPassable() || tileList[2].position == targetPosition)
                {
                    return false;
                }
            }

            else if (dir == MainGame.Direction.DOWN)
            {
                if (!tileList[7].getPassable() || tileList[7].position == targetPosition)
                {
                    return false;
                }
            }

            return true;
        }

    }
}
