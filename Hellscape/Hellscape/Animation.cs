using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Hellscape
{

    /*
     * Animation class which contains a spritesheet and plays an animation
     * Row number, amount of frames and the start frame are specified such that
     * only a portion of the spritesheet is drawn, meaning that only a single image is necessary for all of an objects animations
     *
     *Contributes to better Look of the game   
      */
    public class Animation
    {
        Texture2D spriteSheet;
        int numFrames;
        int currentFrameIndex = 0;
        int firstFrame = 0;
        int totalFramesPlayed = 0;
        int cellWidth;
        int cellheight;
        int cellYPos;
        bool isLooping;

        float frameTime;
        float elapsedFrameTime = 0;

        Vector2 position;
        SpriteEffects effects = SpriteEffects.None;

        public Animation(Texture2D sprites,int width,int height, int rowNumber, int frames, float time, bool loop = false,int startFrame = 0)
        {
            spriteSheet = sprites;
            cellheight = height;
            cellWidth = width;
            numFrames = frames;
            frameTime = time;
            isLooping = loop;
            cellYPos = cellheight * rowNumber;
            firstFrame = startFrame;
        }

        public void setMirror(bool isMirror)
        {
            if (isMirror)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            else { effects = SpriteEffects.None; }
        }

        public void setFrames(int newFrames)
        {
            numFrames = newFrames;
        }

        //reset for non looping animations
        public void reset()
        {
            currentFrameIndex = firstFrame;
        }

        public void setLoop(bool state)
        {
            isLooping = state;
        }

        public void draw(SpriteBatch spriteBatch, GameTime gameTime, Vector2 sourcePos)
        {
            elapsedFrameTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(elapsedFrameTime > frameTime)
            {
                totalFramesPlayed++;
                if (isLooping)
                {
                    currentFrameIndex =firstFrame + totalFramesPlayed % numFrames ; 
                }
                else
                {
                    currentFrameIndex = Math.Min(totalFramesPlayed + firstFrame, numFrames  - 1);
                }

                elapsedFrameTime = 0;
            }

            Vector2 startPosition = new Vector2(cellWidth * currentFrameIndex,cellYPos);
            Rectangle sourceRect = new Rectangle((int)startPosition.X,(int)startPosition.Y,cellWidth,cellheight);
            Rectangle destRect = new Rectangle((int)sourcePos.X, (int)sourcePos.Y, cellWidth, cellheight);
            Vector2 Origin = new Vector2(sourcePos.X + cellWidth/2,sourcePos.Y + cellheight/2);
          
            spriteBatch.Draw(spriteSheet, destRect, sourceRect, Color.White,0.0f,new Vector2(0,0),effects,0.0f);
        }

    }
}
