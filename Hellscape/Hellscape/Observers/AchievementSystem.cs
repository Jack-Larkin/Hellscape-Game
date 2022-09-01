using Hellscape.Observers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hellscape
{
    /* Contains achievements and observes subjects to check for events that could unlock these achievements
     * 
     */
    public class AchievementSystem : Observer
    {
        Achievement move;
        Achievement kill;
        Achievement combo;
        List<Achievement> achievementList;

        Texture2D moveGraphic;
        Texture2D killGraphic;
        Texture2D comboGraphic;

        public AchievementSystem(Texture2D moveImage, Texture2D killImage, Texture2D comboImage)
        {
            moveGraphic = moveImage;
            killGraphic = killImage;
            comboGraphic = comboImage;


            initialise();
            achievementList = new List<Achievement>
            {
                move,
                kill,
                combo
            };
        }

        override public void onNotify(Entity entity,Event _event){
            switch (_event.type)
            {
             

                case Event.EventTypes.DIE:
                    kill.increment();
                    break;

                case Event.EventTypes.COMBO:
                    combo.increment();
                    break;
            }
            //only need move events from player
            if (entity.isPlayer && _event.type == Event.EventTypes.MOVE)
            {
                move.increment();
            }
        }

        void initialise()
        {
            move = new Achievement("Move", 1, moveGraphic);
            kill = new Achievement("Kill 2 enemies", 1, killGraphic);
            combo = new Achievement("Do a combo attack", 1, comboGraphic);
        }

        public void draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            foreach(Achievement achievement in achievementList)
            {
                if (achievement.getUnlocked())
                {
                    achievement.draw(spriteBatch, gameTime);
                }
            }
        }
    }
}
