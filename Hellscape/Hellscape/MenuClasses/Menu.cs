using Hellscape.MenuClasses;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hellscape
{
    //menu class that contains buttons and other graphics
    //is a subject such that it could be observed by sound system to play sound effects on selecting different options

    class Menu : Subject
    {
        ResumeButton resumeButton;
        ExitButton exitButton;
        

        Button selectedButton;
        Texture2D menuBackground;

        Rectangle bounds = new Rectangle(MainGame.graphics.PreferredBackBufferWidth /4, 0 , MainGame.graphics.PreferredBackBufferWidth/2, MainGame.graphics.PreferredBackBufferHeight);
        const int padding = 50;
        const int buttonHeight = 300;
        public bool isActive = false;

        Color activeColour = Color.Crimson;
        List<Button> buttonList;

        public Menu(Texture2D menuTexture, Texture2D buttonTexture,SpriteFont font)
        {
            menuBackground = menuTexture;
            resumeButton = new ResumeButton(this, buttonTexture, font, "resume", new Rectangle(bounds.X + padding, bounds.Y + padding, bounds.Width - padding *2, buttonHeight));
            exitButton = new ExitButton( buttonTexture, font, "Exit", new Rectangle(bounds.X + padding, bounds.Y + padding*2 + buttonHeight, bounds.Width - padding*2, buttonHeight));

            buttonList = new List<Button> { 
            resumeButton,
            exitButton
            };

            selectedButton = buttonList[0];
        }

        public void incrementSelection()
        {
            if (buttonList.IndexOf(selectedButton) != buttonList.Count() - 1)
            {
                selectedButton = buttonList[buttonList.IndexOf(selectedButton) + 1];
            }
        }

        public void decrementSelection()
        {
            if (buttonList.IndexOf(selectedButton) != 0)
            {
                selectedButton = buttonList[buttonList.IndexOf(selectedButton) - 1];
            }
        }

        public void selectButton()
        {
            selectedButton.execute();
        }


        public void draw(SpriteBatch spriteBatch)
        {
            //draw brackground
            spriteBatch.Draw(menuBackground, bounds, Color.White);
            //draw buttons (draw selected button with gray hue)
            foreach(Button button in buttonList)
            {
                if (button.Equals(selectedButton))
                {
                    button.draw(spriteBatch, activeColour);
                }
                else
                {
                    button.draw(spriteBatch, Color.White);
                }
            }
        }
    }
}
