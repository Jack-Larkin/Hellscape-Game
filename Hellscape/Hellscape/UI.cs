using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hellscape
{
    //UI which contains information the user should know about ie player health and their score
    class UI
    {
        int playerCurrentHealth;
        int playerMaxHealth;
        double healthRatio = 1;
        int currentScore = 0;
        int currentFloor = 1;

        Texture2D healthFrame;
        Texture2D healthBar;
        SpriteFont scoreFont;
        Rectangle healthRect;

        String scoreString;
        String floorString;
        String healthString;

        Vector2 scorePosition;
        Vector2 healthFramePosition;
        Vector2 healthStringPosition;
        Vector2 floorStringPosition;

        public UI(int maxHealth, SpriteFont _scoreFont, Texture2D healthFrameSprite, Texture2D whiteRect) {
            scoreFont = _scoreFont;
            playerCurrentHealth = maxHealth;
            playerMaxHealth = maxHealth;

            scorePosition = new Vector2(0, MainGame.graphics.PreferredBackBufferHeight * 7/8);
            healthFramePosition = new Vector2( 0, 0);
            healthStringPosition = new Vector2( healthFramePosition.X + healthFrameSprite.Width + 10, healthFramePosition.Y);
            floorStringPosition = new Vector2(7*( MainGame.graphics.PreferredBackBufferWidth/8), 0);

            healthFrame = healthFrameSprite;
            healthBar = whiteRect;

            healthRect = new Rectangle((int)healthFramePosition.X + (int)healthFrame.Width/5,(int)(healthFramePosition.Y + healthFrame.Height*1/8), (int)(healthFrame.Width * 6/8 * healthRatio), (healthFrame.Height*6/8));
        }

        public void updateUI(int currentHealth, int score, int floor)
        {
            playerCurrentHealth = currentHealth;
            currentScore = score;
            currentFloor = floor;

            scoreString = "Score: " + currentScore.ToString();
            floorString = floor + "F";
            //scale healthbar to match portion of health
            healthRatio = (double)playerCurrentHealth/ (double)playerMaxHealth;
            healthString = (healthRatio * 100 + "%");
            //healthRatio = 0.8;
            healthRect = new Rectangle((int)healthFramePosition.X + (int)healthFrame.Width / 5, (int)(healthFramePosition.Y + healthFrame.Height * 1 / 8), (int)(healthFrame.Width * 6 / 8 * healthRatio), (healthFrame.Height * 6 / 8));
        }

        public void draw (SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(scoreFont, scoreString, scorePosition, Color.White);
            spriteBatch.DrawString(scoreFont, floorString, floorStringPosition, Color.White);
            spriteBatch.DrawString(scoreFont, healthString, healthStringPosition, Color.White);

            spriteBatch.Draw(healthBar, healthRect, Color.PaleGreen);
            spriteBatch.Draw(healthFrame, healthFramePosition, Color.White);
            
        }

    }
}
