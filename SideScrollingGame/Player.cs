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
using Microsoft.Xna.Framework.Media;

namespace SideScrollingGame
{
   public class Player
    {
        // Load our class
        public Player()
        {

        }

        // Set our Movement speed
        int moveSpeed = 10;

        //
        bool spaceDown = false;

        // Set our lives
        public int Lives = 3;

        // Create our texture
        Texture2D playerSprite;

        // Create our rectange for our player
        public Rectangle playerRect;

        // Create our shell list
        public List<Shell> shellList = new List<Shell>();

        // Add the shell to the list for the game to manage
        void Shoot(Game1 game)
        {
            Shell shell = new Shell();
            shell.Load(game.Content, this);
            shellList.Add(shell);
        }

        void UpdateInput(float deltaTime, Game1 game)
        {
            //Get Our Games Gravity
            Vector2 localAcceleration = game.gravity;

            // Check for key inputs
            if (Keyboard.GetState().IsKeyDown(Keys.Left) == true || Keyboard.GetState().IsKeyDown(Keys.A) == true)
            {
                playerRect.X -= moveSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Right) == true || Keyboard.GetState().IsKeyDown(Keys.D) == true)
            {
                playerRect.X += moveSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Up) == true || Keyboard.GetState().IsKeyDown(Keys.W) == true)
            {
                playerRect.Y -= moveSpeed + (int)game.gravity.Y;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Down) == true || Keyboard.GetState().IsKeyDown(Keys.S) == true)
            {
                playerRect.Y += moveSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space) == true)
            {
                // Check if the spacebar os down
                if (shellList.Count < 2 && spaceDown == false)
                {
                    Shoot(game);
                }

                // Set it to true
                spaceDown = true;
            }

            if(Keyboard.GetState().IsKeyUp(Keys.Space) == true)
            {
                spaceDown = false;
            }

            // Apply gravity
            playerRect.Y += (int)game.gravity.Y;
        }

        void Collision(Game1 game)
        {
            if(playerRect.X + playerRect.Width > game.screenWidth)
            {
                playerRect.X = game.screenWidth - playerRect.Width;
            }

            if (playerRect.X < 0)
            {
                playerRect.X = 0;
            }

            if (playerRect.Y + playerRect.Height > game.screenHight + (playerRect.Height * 2))
            {
                // game.state = GameState.LOST;
            }

            if (playerRect.Y < 0)
            {
                playerRect.Y = 0;
            }
        }

        public void Update(float deltaTime, Game1 game)
        {
            // Check if player goes off screen
            if(OffScreen(game) == true)
            {
                game.state = GameState.LOST;
            }

            // Update our player
            UpdateInput(deltaTime, game);

            // Update Bullets
            foreach (Shell bullet in shellList)
            {
                bullet.Update();
            }

            // Check for collision
            Collision(game);
        }

        // Load our class
        public void Load(ContentManager content, int X, int Y)
        {
            // Load our sprite
            playerSprite = content.Load<Texture2D>("images/player");

            // Load our player Rectangle
            playerRect = new Rectangle(X, Y, playerSprite.Width - 190, playerSprite.Height - 90);
        }

        // Draw our class
        public void Draw(SpriteBatch a_spriteBatch)
        {

            // Draw our class
            a_spriteBatch.Begin();
            a_spriteBatch.Draw(playerSprite, playerRect, Color.White);
            a_spriteBatch.End();
        }

        bool OffScreen(Game1 game)
        {
            if (playerRect.Y > game.screenHight + playerRect.Height)
            {
                MediaPlayer.Stop();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
