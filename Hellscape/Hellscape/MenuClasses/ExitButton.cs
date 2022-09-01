using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hellscape.MenuClasses
{
    //subclass of button to trigger game exit
    class ExitButton : Button
    {
        public ExitButton( Texture2D buttonTexture, SpriteFont buttonFont, String buttonText, Rectangle rect) : base(buttonTexture, buttonFont, buttonText, rect)
        {
            
        }

        public override void execute()
        {
            MainGame.isExiting = true;
        }

    }
}
