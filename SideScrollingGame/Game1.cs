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

        // Draw Level
        Texture2D level;

        // Draw Background
        Texture2D background;

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

        // Create our spawn time
        double maxSpawn = 1000;

        // Check max shell
        int maxShell;

        // Create our Enemy list
        public List<Enemy> enemyList;

        // Max enemy timer
        int maxEnemyTime;

        // Add our timer stuff
        float elapsedTime;
        public double counter;

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

            // Load level
            level = Content.Load<Texture2D>("images/level");

            // Load Background
            background = Content.Load<Texture2D>("images/background");


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

                    // Increase our ticker after a second
                    counter += gameTime.ElapsedGameTime.TotalMilliseconds;


                    if (counter >= maxSpawn)
                    {
                        score++;
                        maxSpawn = counter + 1000;

                        // This will run every 50 score. (E.G 50, 100, 150)
                        if (score % 50 == 0 && score != 0)
                        {
                            maxEnemy *= 2;
                        }

                        //counter = 0;
                    }

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
                        // Update our enemy
                        enemyList[i].Update(this);

                        // Check if our enemy goes off screen and delete
                        if (enemyList[i].OffScreen(this) == true)
                        {
                            enemyList.Remove(enemyList[i]);
                            continue;
                        }

                        // Check if enemy has collieded with player
                        if (enemyList[i].HasCollided(player.player, this) == true)
                        {
                            player.Lives--;
                            enemyList.Remove(enemyList[i]);
                        }
                    }

                    // make sure a shell exists
                    if (player.shellList.Count != 0)
                    {
                        // Loop Through all our shells
                        for (int i = 0; i < player.shellList.Count; i++)
                        {
                            // Update our bullet
                            player.shellList[i].Update();

                            // Check if the bullet goes off screen
                            if (player.shellList[i].OffScreen(this) == true)
                            {
                                player.shellList.Remove(player.shellList[i]);
                            }

                            // Check the same for enemies
                            if (enemyList.Count != 0)
                            {
                                maxEnemyTime = enemyList.Count - 1;
                            }
                            else
                            {
                                maxEnemyTime = enemyList.Count;
                            }

                            //Check if shell has collieded with the enemy
                            if (player.shellList.Count != 0)
                            {
                                for (int index = 0; index < maxEnemyTime; index++)
                                {
                                    if (player.shellList[i].HasCollided(enemyList[index].enemy) == true)
                                    {
                                        enemyList.Remove(enemyList[index]);
                                        player.shellList.Remove(player.shellList[i]);
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    // If lives hit 0 then change to Game Over screen
                    if (player.Lives <= 0)
                    {
                        state = GameState.LOST;
                    }

                    break;

                case GameState.LOST:
                    {

                        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                        {
                            enemyList.Clear();
                            player.Lives = 3;
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
            GraphicsDevice.Clear(Color.Black);

            // Our game states to switch between
            switch (state)
            {
                case GameState.MENU:
                    spriteBatch.Begin();
                    spriteBatch.Draw(background, new Rectangle(0, 0, screenWidth, screenHight), Color.White);
                    spriteBatch.End();
                    exitButton.Draw(spriteBatch);
                    startButton.Draw(spriteBatch);
                    break;

                case GameState.PLAYING:
                    // Draw objects not in classes
                    spriteBatch.Begin();
                    spriteBatch.Draw(level, new Rectangle(0, 0, screenWidth, screenHight), Color.White);
                    spriteBatch.DrawString(arialFont, "Lives : " + player.Lives.ToString(), new Vector2(20, 20), Color.Black, 0, new Vector2(0, 0), 3, SpriteEffects.None, 0);

                    // Get our enemie as a string, meausre it, then draw
                    string enemyString = "Enimies : " + maxEnemy.ToString();
                    Vector2 enemySize = arialFont.MeasureString(enemyString);
                    spriteBatch.DrawString(arialFont, enemyString, new Vector2(screenWidth / 2 - 70, 0), Color.Black, 0, new Vector2(0, 0), 3, SpriteEffects.None, 0);

                    // Get our score as a string, meausre it, then draw
                    string scoreString = "Score : " + score;
                    Vector2 scoreSize = arialFont.MeasureString(scoreString);
                    spriteBatch.DrawString(arialFont, scoreString, new Vector2(screenWidth - scoreSize.X * 3 - 20, 0 + scoreSize.Y), Color.Black, 0, new Vector2(0, 0), 3, SpriteEffects.None, 0);

                    spriteBatch.End();

                    // Call our classes draw
                    player.Draw(spriteBatch);

                    // Draw Our enemys
                    foreach (Enemy enemy in enemyList)
                    {
                        enemy.Draw(spriteBatch);
                    }

                    // Draw our shells
                    foreach (Shell shell in player.shellList)
                    {
                        shell.Draw(spriteBatch);
                    }

                    break;
                default:
                    break;

                case GameState.LOST:
                    spriteBatch.Begin();
                    spriteBatch.Draw(gameOver, new Rectangle((screenWidth / 2) - (gameOver.Width / 2), (screenHight / 2) - (gameOver.Height / 2), gameOver.Width, gameOver.Height), Color.White);
                    spriteBatch.DrawString(arialFont, "Press Esc to return to menu", new Vector2(0, 0), Color.Red, 0, new Vector2(0, 0), 3, SpriteEffects.None, 0);
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
