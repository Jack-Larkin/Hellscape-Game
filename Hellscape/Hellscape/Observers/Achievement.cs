using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hellscape.Observers
{
    //Each acheivement consists of a graphic, name and data. once data has surpassed
    // the unlock threshold, the achievement is unlocked and its graphic is played as an animation
    class Achievement
    {
        int unlockRequirement;
        int currentData = 0;
        bool isUnlocked = false;
        String achievementName;
        Texture2D texture;
        Animation animation;
        Vector2 screenPos;
        
        public Achievement(String name, int requirement,Texture2D image)
        {
            achievementName = name;
            unlockRequirement = requirement;

            texture = image;
            animation = new Animation(image, 500, 300, 0, 2, 3.0f);
            screenPos = new Vector2(MainGame.graphics.PreferredBackBufferWidth-image.Width,MainGame.graphics.PreferredBackBufferHeight - image.Height);
        }

        public void update(int newData)
        {
            currentData = newData;
            if(currentData >= unlockRequirement)
            {
                unlock();
            }
        }

        public void increment()
        {
            currentData++;
            update(currentData);
        }

        void unlock()
        {
            if (!isUnlocked)
            {
                isUnlocked = true;

                
            }
        }

        public bool getUnlocked()
        {
            return isUnlocked;
        }

        public void draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            animation.draw(spriteBatch, gameTime, screenPos);
        }

    }
}
