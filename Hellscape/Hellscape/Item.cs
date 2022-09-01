using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hellscape
{
    //Item class used to enchance player stats
    public class Item
    {
        int effectAmount;
        bool isHealth;
        bool isDamage;
        bool collected = false;

        Vector2 position;
        Texture2D spriteSheet;
        Animation animation;

        public Item(bool type, int amount, Vector2 pos, Texture2D sprite )
        {
            if (type)
            {
                isHealth = true;
            }
            else
            {
                isDamage = true;
            }

            effectAmount = amount;
            position = pos;
            spriteSheet = sprite;
            animation = new Animation(spriteSheet, 64, 64, 0, 10, 0.2f, true);
        }

        public void onPickup(Entity entity)
        {
            if (!collected)
            {
                if (isHealth)
                {
                    entity.increaseHealth(effectAmount);
                }
                else
                {
                    entity.increaseAttackDamage(effectAmount);
                }
                collected = true;
            }
        }

        public Vector2 getPosition()
        {
            return position;
        }

        public void draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (!collected)
            {
                animation.draw(spriteBatch, gameTime, position * MainGame.unitWidthHeight);
            }
        }

    }
}
