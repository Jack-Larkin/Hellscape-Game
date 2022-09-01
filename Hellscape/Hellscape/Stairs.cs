using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hellscape
{
    class Stairs : Tile
    {
        bool touched = false;
        
        public Stairs(Vector2 pos, Texture2D texture): base(pos, texture, true)
        {

            isPassable = true;
        }


    }
}
