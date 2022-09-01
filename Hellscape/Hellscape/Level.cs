using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace Hellscape
{
    /*
     * Class responsible for random proceedural level generation
     * generates rooms and tiles to link them
     * 
     * Example of sophisticated code/ non-trivial problem
     * 
     */
    
    public class Level
    {
        int levelWidth; 
        int levelHeight;
        int padding = 1;
        List<Room> roomList = new List<Room>();
        List<Tile> tileList = new List<Tile>();
        Random r = new Random();
        int maxRooms = 15;
        int minRooms = 3;
        int maxRoomWidth = 11;
        int maxRoomHeight = 11;
        
        int maxWidth;
        int maxHeight;
        Stairs stairs;

        Texture2D tileTexture;
        Texture2D stairsTexture;

        //directions
        const Directions LEFT = Directions.LEFT;
        const Directions RIGHT = Directions.RIGHT;
        const Directions UP = Directions.UP;
        const Directions DOWN = Directions.DOWN;

        public enum Directions { 
        LEFT = MainGame.Direction.LEFT,
        RIGHT = MainGame.Direction.RIGHT,
        UP = MainGame.Direction.UP,
        DOWN = MainGame.Direction.DOWN

        }

        public Level(Texture2D tileText, Texture2D stairsText,int widthMax, int heightMax)
        {
            tileTexture = tileText;
            stairsTexture = stairsText;
            setWidthHeight(widthMax, heightMax);
            generateLevel();


        }

        void generateLevel()
        {
            //generate tiles
            for(int a = 0; a < levelHeight; a++)
            {
                for(int b = 0; b < levelWidth; b++)
                {
                    tileList.Add(new Tile (b, a,tileTexture));
                }
            }

            //place random non-overlapping rooms
            while (roomList.Count() <= minRooms || roomList == null)
            {
                //reset room list
                roomList = null;
                for (int c = 0; c <= maxRooms; c++)
                {
                    
                    
                    
                    Room tempRoom = new Room(r.Next(0 + padding, (levelWidth - maxRoomWidth - padding)), r.Next(0 + padding, (levelHeight - maxRoomHeight - padding)),maxRoomWidth,maxRoomHeight);
                    if (roomList == null)
                    {
                        roomList = new List<Room> {
                        tempRoom
                        };
                    }
                    else if(!overlapCheck(tempRoom))
                    {
                        roomList.Add(tempRoom);
                    }

                }
                if(roomList.Count() >= minRooms)
                {
                    break;
                }

            }
            //make tiles in rooms passable
            foreach(Room room in roomList)
            {
                for(int d=(int)room.position.Y; d <= (int)room.position.Y + room.height ; d++)
                {
                    for(int e = (int)room.position.X; e <= (int)room.position.X + room.width;e++)
                    {
                        
                        findTile(e, d).setPassable(true);

                    }
                }
            }

            //connect rooms

            foreach(Room room in roomList)
            {
                Room room1 = room;
                Room room2;
                while (true)
                {
                    room2 = roomList[r.Next(0,roomList.Count)];
                    if (room1 != room2) break;
                }

                //connect rooms
                connectRooms(room1, room2);
            }

            placeStairs();
            Debug.WriteLine("stairs is at position " + stairs.position);
            

        }

        //returns true if rooms overlap
        bool overlapCheck(Room room1, Room room2)
        {
            //if room 1 intersects room 2
            if (room1.position.X <= room2.position.X + room2.width && room1.position.X + room1.width >= room2.position.X &&
    room1.position.Y < room2.position.Y + room2.height && room1.position.Y + room1.height > room2.position.Y)
            {
                return true;
            }
            //if room 2 intersects room 1
            else if (room2.position.X <= room1.position.X + room1.width && room2.position.X + room2.width >= room1.position.X &&
    room2.position.Y < room1.position.Y + room1.height && room2.position.Y + room2.height > room1.position.Y)
            {
                return true;
            }
            else { return false; }
        }
        //return true if rooms overlap
        bool overlapCheck(Room room)
        {
            if (roomList != null)
            {
                foreach (Room listRoom in roomList)
                {
                    if (overlapCheck(room, listRoom))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //generate straight line of tiles
        void generateCorridor(Tile startTile, Directions startDirection, int length = 1)
        {
            //from start tile, make next tile in direction the current tile 
            //change tile to passable
            // do this until next tile is already passable
            Tile currentTile = startTile;
            //0 = left; 1= right; 2 = up ; 3 = down
            Directions direction = startDirection;
            int tilesChanged = 0;
            while (true)
            {
                currentTile = adjacentTile(direction,currentTile);

                currentTile.setPassable(true);
                tilesChanged++;

                //break if complete or no adjacent tile

                if (tilesChanged >= length) break;

                //check if there is an adjacent tile
                if(direction == LEFT)
                {
                    if (currentTile.position.X == 0) break;
                }
                else if(direction == RIGHT)
                {
                    if (currentTile.position.X == levelWidth - 1) break;
                }
                else if (direction == UP)
                {
                    if (currentTile.position.Y == 0) break;
                }
                else if (direction == DOWN)
                {
                    if (currentTile.position.Y == levelHeight  - 1) break;
                }


            }
        }

        void connectRooms(Room room1, Room room2)
        {

            
            //vertical movement
                //If room 1 is below room 2
                if(room1.centreTile.position.Y > room2.centreTile.position.Y)
                {
                    generateCorridor(room1.centreTile, UP, (int)(room1.centreTile.position.Y - room2.centreTile.position.Y));
                }
                // if room 1 above room 2
                else if(room1.centreTile.position.Y < room2.centreTile.position.Y)
                {
                    generateCorridor(room1.centreTile, DOWN, (int)(room2.centreTile.position.Y - room1.centreTile.position.Y));
                }
                //else rooms are parallel in y axis and nothing needed

                //horizontal movement
                //If room 1 to the right of room 2
                if (room1.centreTile.position.X > room2.centreTile.position.X)
                {
                    generateCorridor(room2.centreTile, RIGHT, (int)(room1.centreTile.position.X - room2.centreTile.position.X));
                }
                else if (room1.centreTile.position.X < room2.centreTile.position.X)
                {
                    generateCorridor(room2.centreTile, LEFT, (int)(room2.centreTile.position.X - room1.centreTile.position.X));
                }
                //else do nothing as parallel



            
            
        }

        

        Tile adjacentTile(Directions direction, Tile targetTile)
        {
            if(direction == LEFT)
            {
                

                return findTile((int)targetTile.position.X - 1, (int)targetTile.position.Y);
            }

            else if (direction == RIGHT)
            {
                
              return findTile((int)targetTile.position.X + 1, (int)targetTile.position.Y);
                
            }

            else if (direction == UP)
            {
                
                return findTile((int)targetTile.position.X, (int)targetTile.position.Y - 1);
            }

            else if (direction == DOWN)
            {
                
                return findTile((int)targetTile.position.X, (int)targetTile.position.Y + 1);
            }

            return null;
        }
       

        public Tile findTile(int x, int y)
        {
            int tileNumber = x + (y * levelWidth);
            //if tile doesnt exist
            
            return tileList[tileNumber];
        }

        public bool tileCollisionCheck(Entity entity, int direction)
        {
            Tile currentTile = findTile((int)entity.getPosition().X, (int)entity.getPosition().Y);

            Tile targetTile = adjacentTile((Directions)direction, currentTile);

            if (!targetTile.getPassable())
            {
                return true;
            }

            return false;
        }


        public void drawLevel(SpriteBatch spriteBatch, Camera2D camera)
        {
           
            foreach(Tile tile in tileList)
            {
                tile.drawTile(spriteBatch);
                
            }
            stairs.drawTile(spriteBatch);
            
        }

        public Tile getRandomRoomTile()
        {
            int roomNumber = r.Next(0, roomList.Count - 1);

            return findTile(r.Next((int)roomList[roomNumber].position.X, (int)roomList[roomNumber].position.X + roomList[roomNumber].width) , r.Next((int)roomList[roomNumber].position.Y, (int)roomList[roomNumber].position.Y + roomList[roomNumber].height));
        }

        void placeStairs()
        {
            stairs = new Stairs(getRandomRoomTile().position, stairsTexture);
        }

        public Vector2 getStairsPosition()
        {
            return stairs.position;
        }

        public void setWidthHeight(int width, int height)
        {
            maxWidth = Math.Max(30, width);
            maxHeight = Math.Max(30, height);
            levelWidth = r.Next(29, maxWidth);
            levelHeight = r.Next(29, maxHeight);
            
        }
    }
}
