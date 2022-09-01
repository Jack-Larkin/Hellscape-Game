using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hellscape
{

    //Main class to describe commonalities between the player and enemies
    public class Entity : Subject
    {
        public bool isPlayer;
        protected int health = 5;
        protected int maxHealth = 5;

        protected int attackDamage = 5;

        int sightRadius = 3;
        protected Vector2 position;
        protected Vector2 screenPosition;
        protected static int width = 1;
        protected static int height = 1;
        protected static int screenWidth = width * MainGame.unitWidthHeight;
        protected static int screenHeight = height * MainGame.unitWidthHeight;
        protected Texture2D sprite;
        public bool isDead = false;
        protected MainGame.Direction facing;

        protected Animation walkDown;
        protected Animation walkUp;
        protected Animation walkLeft;
        protected Animation walkRight; 
        protected Animation currentAnimation;

        public bool isBuffering = false;

        public Entity(){}

        public Entity(Vector2 pos ) {
            position = pos;
        }

        public Entity(Vector2 pos, int hp) {
            position = pos;
            
            updateScreenPostion();
            health = hp;
            maxHealth = hp;
            
        }

        public void initaliseGraphics(Texture2D _sprite)
        {
            sprite = _sprite;

        }

        public virtual void move(int direction)
        {
            setFacing(direction);
            //move in direction
            if (direction == (int)MainGame.Direction.UP)
            {
                currentAnimation = walkUp;
                position.Y -= 1;
            }
            if (direction == (int)MainGame.Direction.DOWN)
            {
                currentAnimation = walkDown;
                position.Y += 1;
            }
            if (direction == (int)MainGame.Direction.LEFT)
            {
                currentAnimation = walkLeft;
                position.X -= 1;
            }
            if (direction == (int)MainGame.Direction.RIGHT)
            {
                currentAnimation = walkRight;
                position.X += 1;
            }

        }
        public virtual void attack()
        {
            
        }
        

        protected void updateScreenPostion()
        {
            screenPosition = new Vector2(position.X * MainGame.unitWidthHeight, position.Y * MainGame.unitWidthHeight);
        }

        public Vector2 getPosition()
        {
            return position;
        }

        public void setPosition(Vector2 pos)
        {
            position = pos;
        }

        public Vector2 getScreenPosition()
        {
            return screenPosition;
        }

        // take damage, die , heal etc

        public void setFacing(MainGame.Direction direction)
        {
            facing = direction;
            changeWalkAnimation();
        }

        public void setFacing(int direction)
        {
            facing = (MainGame.Direction)direction;
            changeWalkAnimation();
        }

        public MainGame.Direction getFacing()
        {
            return facing;
        }

        public int getSight()
        {
            return sightRadius;
        }

        public int getAttackDamage()
        {
            return attackDamage;
        }

        public void increaseAttackDamage(int amount)
        {
            attackDamage += amount;
        }

        public void takeDamage(int damage)
        {
            //play animation
            notify(this, new Event(Event.EventTypes.ATTACK,0));
            health -= damage;
            Debug.WriteLine("health is: " + health);
            if (health <= 0)
            {
                health = 0;
                die();
            }
        }

        public void increaseHealth(int amount)
        {
            health += amount;
            if(health > maxHealth)
            {
                health = maxHealth;
            }
        }

        public int getHealth()
        {
            return health;
        }

        public virtual void die()
        {
            //play animation?
            isDead = true;
            notify(this, new Event(Event.EventTypes.DIE, 1));
        }

        void changeWalkAnimation()
        {
            if (facing == MainGame.Direction.LEFT)
            {
                currentAnimation = walkLeft;
            }
            else if (facing == MainGame.Direction.RIGHT)
            {
                currentAnimation = walkRight;
            }
            else if (facing == MainGame.Direction.UP)
            {
                currentAnimation = walkUp;
            }
            else if (facing == MainGame.Direction.DOWN)
            {
                currentAnimation = walkDown;
            }
        }
    }
}
