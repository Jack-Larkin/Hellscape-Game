using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace Hellscape
{
    //describes points within the level that may or may not be passable to entities
    public class Tile
    {
        public Vector2 position;
        Vector2 screenPosition;
        //width and height, used for drawing
        public static int widthHeight = MainGame.unitWidthHeight;
        protected bool isPassable = false;
        Texture2D tileTexture;
        

        public Tile(int x, int y, Texture2D texture = null ,bool pass = false)
        {
            position.X = x;
            position.Y = y;
            screenPosition.X = x * widthHeight;
            screenPosition.Y = y * widthHeight;
            isPassable = pass;
            tileTexture = texture;
        }

        public Tile(Vector2 pos, Texture2D texture = null, bool pass = false)
        {
            position = pos;
            screenPosition.X = position.X * widthHeight;
            screenPosition.Y = position.Y * widthHeight;
            isPassable = pass;
            tileTexture = texture;
        }


        public bool getPassable()
        {
            return isPassable;
        }
        public void setPassable(bool pass)
        {
            isPassable = pass;
        }

        public void drawTile(SpriteBatch spriteBatch)
        {
            if (isPassable)
            {
                
                spriteBatch.Draw(tileTexture, new Rectangle((int)screenPosition.X,(int)screenPosition.Y,widthHeight,widthHeight), Color.White);
                // new Rectangle((int)position.X*widthHeight, (int)position.Y*widthHeight, widthHeight, widthHeight),
            }
        }
    }
}
