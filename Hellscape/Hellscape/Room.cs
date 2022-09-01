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
    //describes dimensions of the room and the centre tile, used to connect rooms with corridors in level generation
    class Room
    {
        //position on map
        public Vector2 position;
        // centre of room
        public Tile centreTile;
        int maxWidth; int maxHeight;
        public int width; public int height;
        Random r = new Random();

        public Room(int posX, int posY, int mWidth, int mHeight)
        {
            position.X = posX;
            position.Y = posY;
            maxWidth = mWidth;
            maxHeight = mHeight;
            width = r.Next(3, maxWidth);
            height = r.Next(3, maxHeight);
            centreTile = new Tile((int)(position.X + width / 2), (int)(position.Y + height / 2));
        }
    }
}
