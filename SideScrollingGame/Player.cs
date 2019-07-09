using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SideScrollingGame
{
    class Player
    {
        // Load our class
        public Player()
        {

        }

        // Set our Movement speed
        int moveSpeed = 10;

        // Set our lives
        public int Lives = 3;

        // Create our texture
        Texture2D playerSprite;

        // Create our rectange for our player
        public Rectangle player;

        // Create our shell list
        List<Shell> shellList;

        // Add the shell to the list for the game to manage
        void Shoot(Game1 game)
        {
            //Shell shell = new Shell("player");
            //shellList.Add(shell);
            //shellList[shellList.Capacity - 1].Load<Player>(game.Content, this);
        }

        void UpdateInput(float deltaTime, Game1 game)
        {
            //Get Our Games Gravity
            Vector2 localAcceleration = game.gravity;


            if (Keyboard.GetState().IsKeyDown(Keys.Left) == true || Keyboard.GetState().IsKeyDown(Keys.A) == true)
            {
                player.X -= moveSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right) == true || Keyboard.GetState().IsKeyDown(Keys.D) == true)
            {
                player.X += moveSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up) == true || Keyboard.GetState().IsKeyDown(Keys.W) == true)
            {
                player.Y -= moveSpeed + (int)game.gravity.Y;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down) == true || Keyboard.GetState().IsKeyDown(Keys.S) == true)
            {
                player.Y += moveSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space) == true)
            {
                Shoot(game);
            }

            // Apply gravity
            player.Y += (int)game.gravity.Y;
        }

        void Collision(Game1 game)
        {
            if(player.X + player.Width > game.screenWidth)
            {
                player.X = game.screenWidth - player.Width;
            }

            if (player.X < 0)
            {
                player.X = 0;
            }

            if (player.Y + player.Height > game.screenHight + (player.Height * 2))
            {
                //game.state = GameState.LOST;
            }

            if (player.Y < 0)
            {
                player.Y = 0;
            }
        }

        public void Update(float deltaTime, Game1 game)
        {
            // Update our player
            UpdateInput(deltaTime, game);

            // Update Bullets
            //foreach(Shell bullet in shellList)
            //{
            //    bullet.Update();
            //}

            // Check for collision
            Collision(game);
        }

        // Load our class
        public void Load(ContentManager content, int X, int Y)
        {
            // Load our sprite
            playerSprite = content.Load<Texture2D>("images/player");

            // Load our player Rectangle
            player = new Rectangle(X, Y, playerSprite.Width - 150, playerSprite.Height - 70);
        }

        // Draw our class
        public void Draw(SpriteBatch a_spriteBatch)
        {

            // Draw our class
            a_spriteBatch.Begin();
            a_spriteBatch.Draw(playerSprite, player, Color.White);
            a_spriteBatch.End();
        }
    }
}
