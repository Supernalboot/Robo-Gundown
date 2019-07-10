using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace SideScrollingGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>

    // Create our global variable for our game states
    public enum GameState { MENU, PLAYING, LOST };

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Create our delta time
       public float deltaTime;

        // Create our Gravity
        public Vector2 gravity = new Vector2(0, 10);

        // Create our enum for our game states
        public GameState state = GameState.MENU;

        // Create our button class's
        Button exitButton = new Button();
        Button startButton = new Button();

        // Draw Background
        Texture2D backdrop;

        // Draw Game Over Screen
        public Texture2D gameOver;

        // Create our player class
        Player player = new Player();

        // Create our Asteroid class
        Enemy enemy = new Enemy();

        // Create our score
        public int score = 0;

        // Create our sprite Font
        SpriteFont arialFont;

        // Screen Width and Height
        public int screenWidth;
        public int screenHight;

        // Create our Max enemy range
        int maxEnemy = 5;

        // Create our Enemy list
        public List<Enemy> enemyList;

        // Add our timer stuff
        float elapsedTime;

        // Create our game states
        public GameState setState()
        {
            return state = GameState.PLAYING;
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            gameOver = null;
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
            // Create our game as a fullscreen
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            // Get our screen height and width
            screenHight = GraphicsDevice.DisplayMode.Height;
            screenWidth = GraphicsDevice.DisplayMode.Width;

            // Make our mouse visible
            this.IsMouseVisible = true;

            // Create our enemy list
            enemyList = new List<Enemy>();

            // Add our enemy to the list
            for (int i = 0; i < maxEnemy; i++)
            {
                    enemy = new Enemy();
                    enemyList.Add(enemy);
            }


            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            // Load our buttons
            exitButton.Load(this.Content, screenWidth / 3, screenHight / 2 - exitButton.button.Height, "exit", 1);
            startButton.Load(this.Content, screenWidth / 3, screenHight / 3, "start", 0);

            // TODO: use this.Content to load your game content here
            player.Load(this.Content, screenWidth / 2 - player.player.Width, screenHight / 2 - player.player.Height);

            // Load backhground
            backdrop = Content.Load<Texture2D>("images/level");

            // Load our font
            arialFont = Content.Load<SpriteFont>("Arial");

            // Load in our enemy
            foreach (Enemy enemy in enemyList)
            {
                enemy.Load(this.Content, this);
            }

            // Load in Game Over Screen
            gameOver = Content.Load<Texture2D>("images/gameOver");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Set our escape key
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Set our delta times
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Set our game states
            switch (state)
            {
                case GameState.MENU:
                    exitButton.Update(gameTime, this);
                    startButton.Update(gameTime, this);
                    break;

                case GameState.PLAYING:

                    // Increase our timer
                    elapsedTime += deltaTime;
                    
                    // Fill in our enemies if they die
                    if (enemyList.Count < maxEnemy)
                    {
                            Random rand = new Random(Guid.NewGuid().GetHashCode());
                        float maxTime = (float)rand.NextDouble();

                            // Run only when the elapsed time has passed
                            if (elapsedTime > maxTime)
                            {
                                enemy = new Enemy();

                                enemyList.Add(enemy);
                                enemyList[enemyList.Count - 1].Load(this.Content, this);

                                elapsedTime = 0f;
                            }
                    }

                    // Update our classes
                    player.Update(deltaTime, this);

                    // Update each Enemy
                    for (int i = 0; i < enemyList.Count; i++)
                    {
                        Console.WriteLine("[" + i + "] " + "X: " + enemyList[i].enemy.X + " Y: " + enemyList[i].enemy.X);
                        // Update our enemy
                        enemyList[i].Update(this);

                        // Check if our enemy goes off screen and delete
                        if (enemyList[i].OffScreen(this) == true)
                        {
                            Console.WriteLine("enemy [" + i + "] OffScreen");
                            enemyList.Remove(enemyList[i]);
                            continue;
                        }

                        if (enemyList[i].HasCollided(player.player, this) == true)
                        {
                            Console.WriteLine("Hit Player");
                            player.Lives--;
                            enemyList.Remove(enemyList[i]);
                        }
                    }

                    // If lives hit 0 then change to Game Over screen
                    if(player.Lives <= 0)
                    {
                        state = GameState.LOST;
                    }

                    break;

                case GameState.LOST:
                    {
                        // Get Keyboard State
                        KeyboardState keyState = Keyboard.GetState();

                        // Returns to Menu
                        if(keyState.IsKeyDown(Keys.Escape))
                        {
                            player.Lives = 5;
                            score = 0;
                            state = GameState.MENU;
                        }

                        break;
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Our game states to switch between
            switch (state)
            {
                case GameState.MENU:
                    exitButton.Draw(spriteBatch);
                    startButton.Draw(spriteBatch);
                    break;

                case GameState.PLAYING:
                    // Draw objects not in classes
                    spriteBatch.Begin();
                    spriteBatch.Draw(backdrop, new Rectangle(0, 0, screenWidth, screenHight), Color.White);
                    spriteBatch.DrawString(arialFont, "Lives : " + player.Lives.ToString(), new Vector2(20, 20), Color.LightGreen, 0, new Vector2(0, 0), 3, SpriteEffects.None, 0);
                    spriteBatch.End();

                    // Call our classes draw
                    player.Draw(spriteBatch);

                    for (int i = 0; i < enemyList.Count; i++)
                    {
                        // Draw Our enemy
                        enemyList[i].Draw(spriteBatch);
                    }

                    break;
                default:
                    break;

                case GameState.LOST:
                    spriteBatch.Begin();
                    spriteBatch.Draw(gameOver, new Rectangle(screenWidth / 3 - gameOver.Width, screenHight / 3 - gameOver.Height, screenWidth / 2, screenHight / 2), Color.White);
                    spriteBatch.End();
                    break;
            }

            base.Draw(gameTime);
        }


        // A function for quitting the game
        public void Quit()
        {
            this.Exit();
        }
    }
}
