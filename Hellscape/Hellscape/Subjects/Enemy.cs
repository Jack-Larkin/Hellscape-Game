using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hellscape.Subjects
{
    class Enemy : Entity
    {
        /*
         *subclass of entity to describe enemies and their implementations of virtual functions
         */
        public Enemy()
        {

        }

        public Enemy(Vector2 pos, int hp)
        {
            position = pos;
            health = hp;
            screenPosition = new Vector2(position.X * MainGame.unitWidthHeight, position.Y * MainGame.unitWidthHeight);
            //sprite = _sprite;
        }

        public void initaliseGraphics(Texture2D spriteSheet, int downRow, int leftrightRow, int upRow)
        {
            sprite = spriteSheet;
            walkDown = new Animation(spriteSheet,64,64,downRow,4,0.1f,true);
            walkUp = new Animation(spriteSheet, 64, 64, upRow, 4, 0.1f, true);
            walkRight = new Animation(spriteSheet, 64, 64, leftrightRow, 4, 0.1f, true);
            walkLeft = new Animation(spriteSheet, 64, 64, leftrightRow, 4, 0.1f, true);
            //no left walk on spritesheet so right walk is mirrored
            walkLeft.setMirror(true);
        }

        public override void die()
        {
            notify(this, new Event(Event.EventTypes.DIE, 1));
            
            base.die();
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
    }
}
