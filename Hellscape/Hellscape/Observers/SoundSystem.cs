using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hellscape.Observers
{

    //observes subjects to play appropraite sound effects on certain events
    // Example of monogame feature beyond what has been taught (importing and using sound effects as oppsed to songs)
    class SoundSystem : Observer
    {
        SoundEffect footstep;
        SoundEffect attack;

        Song BGM;

        public SoundSystem(Song background, SoundEffect walkSound, SoundEffect attackSound)
        {
            BGM = background;
            footstep = walkSound;
            attack = attackSound;


            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.1f;
            MediaPlayer.Play(BGM);
            
            
        }

        override public void onNotify(Entity entity, Event _event)
        {
            switch (_event.type)
            {
                case Event.EventTypes.MOVE:
                    footstep.Play();
                    break;

                case Event.EventTypes.DIE:
                    attack.Play();
                    break;

                case Event.EventTypes.ATTACK:
                    attack.Play();
                    break;
            }
        }
    }
}
