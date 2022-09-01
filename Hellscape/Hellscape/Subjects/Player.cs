using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hellscape
{
    // subclass of Entity to describe player specific implementations of certain functions
    public class Player : Entity
    {
        
        Animation deathAnimation;
        Animation attackLeft;
        Animation attackRight;
        Animation attackUp;
        Animation attackDown;

        public Player(Vector2 pos,  int hp) {
            isPlayer = true;
            position = pos;
            health = hp;
            maxHealth = hp;
            screenPosition = new Vector2(position.X * MainGame.unitWidthHeight, position.Y * MainGame.unitWidthHeight);
            attackDamage = 5;
            //sprite = _sprite;
        }

        public void initaliseGraphics(Texture2D _sprite, int downRow, int leftRow, int rightRow, int upRow)
        {
            sprite = _sprite;
            walkDown = new Animation(sprite, 64, 64, downRow, 8, 0.1f, true);
            attackDown = new Animation(sprite, 64, 64, downRow, 3, 0.2f, false, 8);
            walkUp = new Animation(sprite, 64, 64, upRow, 8, 0.1f, true);
            attackUp = new Animation(sprite, 64, 64, upRow, 3, 0.2f, false, 8);
            walkLeft = new Animation(sprite, 64, 64, leftRow, 8, 0.1f, true);
            attackLeft = new Animation(sprite, 64, 64, leftRow, 3, 0.2f, false, 8);
            walkRight = new Animation(sprite, 64, 64, rightRow, 8, 0.1f, true);
            attackRight = new Animation(sprite, 64, 64, rightRow, 3, 0.2f, false, 8);
            deathAnimation = new Animation(sprite, 64, 64, 5, 3, 0.4f);
        }

        public override void move(int direction)
        {
            //make move event
            notify(this, new Event(Event.EventTypes.MOVE, 1));
            base.move(direction);
        }

        public override void attack()
        {
            if(facing == MainGame.Direction.LEFT)
            {
                currentAnimation = attackLeft;
            }
            else if (facing == MainGame.Direction.RIGHT)
            {
                currentAnimation = attackRight;
            }
            else if (facing == MainGame.Direction.UP)
            {
                currentAnimation = attackUp;
            }
            else if (facing == MainGame.Direction.DOWN)
            {
                currentAnimation = attackDown;
            }
            currentAnimation.reset();
            base.attack();
        }


        public void draw(SpriteBatch spriteBatch,GameTime gameTime)
        {
            updateScreenPostion();
            if (currentAnimation != null)
            {
                currentAnimation.draw(spriteBatch, gameTime, screenPosition);
            }
            else
            {
                spriteBatch.Draw(sprite, new Rectangle((int)screenPosition.X, (int)screenPosition.Y, screenWidth, screenHeight), new Rectangle(0, 0, 64, 64), Color.White);
            }
        }

        public override void die()
        {
            currentAnimation = deathAnimation;
            MainGame.GameOVer = true;
            base.die();
        }
    }
}
