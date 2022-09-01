using Hellscape.Observers;
using Hellscape.Subjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System.Xml.Schema;

namespace Hellscape
{
    /*
     * main game class
     * CONTROLS
     * WASD TO MOVE
     * K = LIGHT ATTACK AND SELECT ON MENU
     * L = HEAVY ATTACK (DIFFERENT RANGE)
     */
    public class MainGame : Game
    {
        
        public static bool isExiting = false;
        public static bool GameOVer = false;
        public static bool isRestarting = false;
        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        static Camera2D camera;
        Texture2D tileTexture;
        Texture2D playerSprite;
        Texture2D enemySprite;
        Texture2D stairsTexture;
        Texture2D background;
        Texture2D healthPotion;
        Texture2D damagePotion;
        Texture2D healthFrame;
        Texture2D menuTexture;
        Texture2D buttonTexture;
        Texture2D moveGraphic;
        Texture2D killGraphic;
        Texture2D comboGraphic;
        //empty texture for drawing coloured rectangles
        Texture2D whiteTexture;

        SpriteFont mainFont;
        


        UI ui;

        AchievementSystem achievementSystem;
        SoundSystem soundSystem;
        InputHandler inputHandler;
        GameAI AI;
        Menu pauseMenu;

        List<Item> itemList;
        int maxItems = 5;
        int maxLevelWidth = 30;
        int maxLEvelHeight = 30;

        
        Player player;
        int maxEnemy = 3;
        

        int score = 0;
        int floor = 0;
        int floorScoreIncrement = 1000;
        int timeScoreIncrement = 1;
        
        List<Enemy> enemyList = new List<Enemy>();
       
        
        Random r = new Random();
        Level currentLevel = null;
        Color bg = Color.CornflowerBlue;

        static int FPS = 30;

        
        int inputBufferTime = FPS/3;
        int lastBufferTime = 0;
        int deltaTime = 0;

        int enemyBufferTime = FPS;
        int lastEnemyBuffer = 0;

        public static int unitWidthHeight = 64;

        public enum Direction
        {
            LEFT,
            RIGHT,
            UP,
            DOWN,
        }

        

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            camera = new Camera2D(GraphicsDevice.Viewport);
            
            player = new Player(new Vector2(3,3),100);
            


            inputHandler = new InputHandler();
            AI = new GameAI();
            
            //_event = new Event(1,0);
            this.IsFixedTimeStep = true;//false;
            this.TargetElapsedTime = TimeSpan.FromSeconds(1d / FPS); //60);

            

            whiteTexture = new Texture2D(this.GraphicsDevice, 1, 1);
            whiteTexture.SetData(new Color[] { Color.White });

            base.Initialize();
        }

     
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            tileTexture = Content.Load<Texture2D>("floorTile");
            playerSprite = Content.Load<Texture2D>("goblin");
            enemySprite = Content.Load<Texture2D>("demonSpriteSheet");
            stairsTexture = Content.Load<Texture2D>("stairs");
            background = Content.Load<Texture2D>("background");
            mainFont = Content.Load<SpriteFont>("mainFont");
            healthPotion = Content.Load<Texture2D>("healthPotion");
            damagePotion = Content.Load<Texture2D>("attackPotion");
            healthFrame = Content.Load<Texture2D>("healthFrame");
            buttonTexture = Content.Load<Texture2D>("buttonBackground");
            menuTexture = Content.Load<Texture2D>("menuBackground");
            moveGraphic = Content.Load<Texture2D>("moveAchievement");
            comboGraphic = Content.Load<Texture2D>("comboAchievement");
            killGraphic = Content.Load<Texture2D>("killAchievement");
            soundSystem = new SoundSystem(Content.Load<Song>("Carpenter Brut - Turbo Killer"),Content.Load<SoundEffect>("footStep"), Content.Load<SoundEffect>("attack1"));
            player.addObserver(soundSystem);

            pauseMenu = new Menu(menuTexture,buttonTexture,mainFont);
            ui = new UI(player.getHealth(),mainFont,healthFrame,whiteTexture);
            player.initaliseGraphics(playerSprite,0,3,1,2);
            
            achievementSystem = new AchievementSystem(moveGraphic,killGraphic,comboGraphic);
            player.addObserver(achievementSystem);
           

            newLevel();
        }

 
        protected override void UnloadContent()
        {

        }
        //Main update loop of the game, gets called once per frame
        protected override void Update(GameTime gameTime)
        {
            if (isExiting)
            {
                Exit();
            }

            if (isRestarting)
            {
                isRestarting = false;
                restart();
            }

            if (GameOVer)
            {
                pauseMenu.isActive = true;
            }

            if (!player.isBuffering)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Escape) && !GameOVer)
                {
                    pauseMenu.isActive = !pauseMenu.isActive;
                }
            }

            

            deltaTime += 1;

            if (pauseMenu.isActive)
            {
                if (player.isBuffering)
                {
                    if (deltaTime >= inputBufferTime + lastBufferTime)
                    {
                        player.isBuffering = false;
                    }
                }

                if (!player.isBuffering)
                {
                    Command command = inputHandler.checkForInput();
                    if (command != null)
                    {
                        if (command.getDirection() == (int)Direction.DOWN)
                        {
                            //shift selected button upwards
                            pauseMenu.incrementSelection();
                        }
                        else if (command.getDirection() == (int)Direction.UP)
                        {
                            pauseMenu.decrementSelection();
                        }

                        

                    }
                    lastBufferTime = deltaTime;

                    player.isBuffering = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.K))
                        {
                            pauseMenu.selectButton();
                        }
            }


            if (!pauseMenu.isActive && !player.isDead)
            {

                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    newLevel();
                }

                ui.updateUI(player.getHealth(), score, floor);

                
                score += timeScoreIncrement;

                foreach (Item item in itemList)
                {
                    if (player.getPosition() == item.getPosition())
                    {
                        item.onPickup(player);
                    }
                }

                if (player.isBuffering)
                {
                    if (deltaTime >= inputBufferTime + lastBufferTime)
                    {
                        player.isBuffering = false;
                    }
                }
               

                if (!player.isBuffering)
                {
                    Command command = inputHandler.checkForInput();

                    if (command != null)
                    {
                        
                        player.setFacing(command.getDirection());

                        if (command.isMove && !currentLevel.tileCollisionCheck(player, command.getDirection()) && !enemyCollisionCheck((Direction)command.getDirection()))
                        {
                            command.execute(player);

                        }

                        else if (command.isAttack)
                        {

                            player.attack();
                            Rectangle boundingRect;
                            if (player.getFacing() == Direction.LEFT)
                            {
                                boundingRect = new Rectangle((int)player.getPosition().X - (int)command.range.X, (int)player.getPosition().Y - (int)command.range.Y + 1,
                                    (int)command.range.X, (int)command.range.Y + (int)command.range.Y - 1);
                            }
                            else if (player.getFacing() == Direction.RIGHT)
                            {
                                boundingRect = new Rectangle((int)player.getPosition().X + (int)command.range.X, (int)player.getPosition().Y - (int)command.range.Y + 1,
                                    (int)command.range.X, (int)command.range.Y + (int)command.range.Y - 1);
                            }
                            else if (player.getFacing() == Direction.UP)
                            {
                                boundingRect = new Rectangle((int)player.getPosition().X + 1 - (int)command.range.Y, (int)player.getPosition().Y - (int)command.range.X,
                                     (int)command.range.Y + (int)command.range.Y - 1, (int)command.range.X);
                            }
                            else
                            {
                                boundingRect = new Rectangle((int)player.getPosition().X + 1 - (int)command.range.Y, (int)player.getPosition().Y + (int)command.range.X,
                                     (int)command.range.Y + (int)command.range.Y - 1, (int)command.range.X);
                            }

                           

                            foreach (Enemy enemy in enemyList)
                            {
                                if (new Rectangle(new Point((int)enemy.getPosition().X, (int)enemy.getPosition().Y), new Point(1, 1)).Intersects(boundingRect))
                                {
                                    enemy.takeDamage(player.getAttackDamage());

                                }
                            }

                            

                        }

                        lastBufferTime = deltaTime;

                        player.isBuffering = true;
                    }
                }

               


                foreach (Enemy enemy in enemyList)
                {
                    if (!enemy.isDead)
                    {
                        if (!enemy.isBuffering)
                        {
                            AI.setEntity(enemy);
                            AI.takeEnemyTurn(player, getSurroundingTiles(enemy.getPosition()));
                            enemy.isBuffering = true;
                            lastEnemyBuffer = deltaTime;

                            enemy.isBuffering = true;
                        }
                        else
                        {
                            if (deltaTime >= enemyBufferTime + lastEnemyBuffer)
                            {
                                enemy.isBuffering = false;
                            }
                        }
                    }
                }

                //keep camera centred on player
                camera.lockToPlayer(player);

                if (player.getPosition() == currentLevel.getStairsPosition())
                {
                    newLevel();
                    floor++;
                    score += floorScoreIncrement;
                }
            }
            

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            if (pauseMenu.isActive)
            {
                spriteBatch.Begin();
                pauseMenu.draw(spriteBatch);
                spriteBatch.End();
            }

            if (!pauseMenu.isActive)
            {
                GraphicsDevice.Clear(bg);
                drawBackground();

                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.Default, RasterizerState.CullNone, null, camera.GetViewMatrix());
               
                if (currentLevel != null)
                {
                    currentLevel.drawLevel(spriteBatch, camera);
                }

                if (itemList != null)
                {
                    foreach (Item item in itemList)
                    {
                        item.draw(spriteBatch, gameTime);
                    }
                }

                player.draw(spriteBatch, gameTime);

                //testEnemy.draw(spriteBatch);

                foreach (Enemy enemy in enemyList)
                {
                    if (!enemy.isDead)
                    {
                        enemy.draw(spriteBatch, gameTime);
                    }
                }
           //spriteBatch.DrawString(mainFont, test, new Vector2(200, 300), Color.White);
                spriteBatch.End();
                 drawUI(gameTime);
            }
            //spriteBatch.End();
            base.Draw(gameTime);
        }

        //draw elements that do not move with the camera
        void drawBackground()
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.Gray);
            spriteBatch.End();
        }
        void drawUI(GameTime gameTime)
        {
            spriteBatch.Begin();
            ui.draw(spriteBatch);
            achievementSystem.draw(spriteBatch, gameTime);
            spriteBatch.End();        
        }

        
        public Level getlevel()
        {
            return currentLevel;
        }

        

        void placeEntity(Entity entity)
        {
            entity.setPosition(currentLevel.getRandomRoomTile().position);
        }

        void newLevel()
        {
            //gradually make level bigger, increasing difficulty
            maxLevelWidth += 2;
            maxLEvelHeight += 2;
            maxEnemy += 2;
            currentLevel = new Level(tileTexture, stairsTexture,maxLevelWidth,maxLEvelHeight);
            //place items
            itemList = new List<Item>();
            for (int i = 0; i <= r.Next(maxItems); i++)
            {
                int n = r.Next(2);
                Item newItem;
                if(n == 0)
                {
                    newItem = new Item(true, 5, currentLevel.getRandomRoomTile().position, healthPotion);
                }
                else
                {
                    newItem = new Item(false, 1, currentLevel.getRandomRoomTile().position, damagePotion);
                }
                itemList.Add(newItem);
            }
            //place enemy

            addEnemies();
            //place player
            placeEntity(player);
            
        }

        //places enemies
        void addEnemies()
        {
            enemyList = new List<Enemy>();
            for(int i = 0; i <= maxEnemy; i++)
            {
                Enemy newEnemy = new Enemy();
                placeEntity(newEnemy);
                newEnemy.addObserver(achievementSystem);
                newEnemy.initaliseGraphics(enemySprite,0,2,1);
                enemyList.Add(newEnemy);
            }
        }
        //returns list of tiles surrounding a position, in form
        /*
         * [1][2][3]
         * [4]---[5]
         * [6][7][8]
         * 
         */
        List<Tile> getSurroundingTiles(Vector2 targetPosition)
        {
            List<Tile> tileList = new List<Tile>
            {
                currentLevel.findTile((int)targetPosition.X - 1,(int)targetPosition.Y - 1),
                currentLevel.findTile((int)targetPosition.X ,(int)targetPosition.Y - 1),
                currentLevel.findTile((int)targetPosition.X + 1,(int)targetPosition.Y - 1),
                currentLevel.findTile((int)targetPosition.X - 1,(int)targetPosition.Y ),
                currentLevel.findTile((int)targetPosition.X + 1,(int)targetPosition.Y ),
                currentLevel.findTile((int)targetPosition.X - 1,(int)targetPosition.Y + 1),
                currentLevel.findTile((int)targetPosition.X ,(int)targetPosition.Y + 1),
                currentLevel.findTile((int)targetPosition.X + 1,(int)targetPosition.Y + 1),
            };

            return tileList;
            
            
        }

        
        
        //restart game
        public void restart()
        {
            GameOVer = false;
            isRestarting = false;
            score = 0;
            floor = 0;
            player = new Player(new Vector2(0, 0), 100);
            player.initaliseGraphics(playerSprite, 0, 3, 1, 2);
            player.addObserver(achievementSystem);
            player.addObserver(soundSystem);
            pauseMenu.isActive = false;


            newLevel();
        }

        bool enemyCollisionCheck(Direction direction)
        {
            foreach (Enemy enemy in enemyList) {
                if (direction == Direction.LEFT && !enemy.isDead)
                {
                    if (player.getPosition().X == enemy.getPosition().X + 1 &&
                        player.getPosition().Y == enemy.getPosition().Y)
                    {
                        return true;
                    }
                }
                if (direction == Direction.RIGHT && !enemy.isDead)
                {
                    if (player.getPosition().X == enemy.getPosition().X - 1 &&
                        player.getPosition().Y == enemy.getPosition().Y)
                    {
                        return true;
                    }
                }
                if (direction == Direction.UP && !enemy.isDead)
                {
                    if (player.getPosition().X == enemy.getPosition().X &&
                        player.getPosition().Y == enemy.getPosition().Y + 1)
                    {
                        return true;
                    }
                }
                if (direction == Direction.DOWN && !enemy.isDead)
                {
                    if (player.getPosition().X == enemy.getPosition().X &&
                        player.getPosition().Y == enemy.getPosition().Y - 1)
                    {
                        return true;
                    }
                }


            }


            return false;
        }
    }
}
