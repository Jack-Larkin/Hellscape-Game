using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hellscape.MenuClasses
{

    //subclass of button to implement disabling the menu, or restarting the game if gameover
    class ResumeButton : Button
    {
        Menu menuInstance;
        public ResumeButton (Menu menu, Texture2D buttonTexture, SpriteFont buttonFont, String buttonText, Rectangle rect) : base(buttonTexture, buttonFont, buttonText, rect) 
        {
            menuInstance = menu;
        }

        //resume game by disabling menu
        public override void execute()
        {
            if (MainGame.GameOVer)
            {
                MainGame.isRestarting = true;
            }
            menuInstance.isActive = false;
            base.execute();
        }

    }
}
