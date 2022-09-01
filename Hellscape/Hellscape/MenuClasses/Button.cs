using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hellscape
{
    //Button class is another implementation fo the command class, essentially a graphical representation of a triggerable command
    class Button : Command
    {
        Texture2D texture;
        SpriteFont font;
        String text;

        Rectangle bounds;
        const int padding = 10;
        Vector2 textPosition;

        public Button(Texture2D buttonTexture, SpriteFont buttonFont,String buttonText, Rectangle rect)
        {
            texture = buttonTexture;
            font = buttonFont;
            text = buttonText;
            bounds = rect;

            textPosition = new Vector2( bounds.X + bounds.Height/2, bounds.Y + bounds.Y/2);
        }

        public virtual void execute() { }

        public void draw(SpriteBatch spriteBatch, Color colour)
        {
            spriteBatch.Draw(texture, bounds,colour);
            spriteBatch.DrawString(font, text, textPosition, Color.White);
        }
    }
}
