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
    public class Enemy
    {
        // Create our enemy Rectangle
        public Rectangle enemy;

        // set our speed and timers
        int speed = 10;

        // Set our random
        Random rand = new Random(Guid.NewGuid().GetHashCode());

        // set our spawn bool
        bool spawn = false;

        // Create our Texture
        Texture2D enemySprite;

        // Create vector 2s for our direction and target position
        public Vector2 position;

        // Set our Start Positon
        void SetStartPos(Game1 game)
        {
            // randomly spawn our asteround above the screen
            position.X = rand.Next(game.screenWidth);
            position.Y = 0 - enemySprite.Height;
        }

        // Load our Sprite and enemy
        public void Load(ContentManager content, Game1 game)
        {
            // Load our image and create a rectangle for it
            enemySprite = content.Load<Texture2D>("images/enemy");

            // Set our position
            SetStartPos(game);

            enemy = new Rectangle((int)position.X, (int)position.Y, enemySprite.Width - 170, enemySprite.Height - 120);

            // Ajust Position so that they do not clip off screen
            if(enemy.X + enemy.Width > game.screenWidth) enemy.X = game.screenWidth - enemy.Width;
            if (enemy.X < 0) enemy.X = 0;

        }

        // Draw our Sprite
        public void Draw(SpriteBatch a_spriteBatch)
        {
            // Draw our sprite
            a_spriteBatch.Begin();
            a_spriteBatch.Draw(enemySprite, enemy, Color.White);
            a_spriteBatch.End();
        }

        // Our update loop
        public void Update(Game1 game)
        {
            // check if this class was spawned
            if (spawn == false)
            {
                // Spawn enemy
                spawn = true;
            }
            else
            {
                // update enemy position
                UpdateVelocity();
            }
        }

        // Collision Detection
        public bool HasCollided(Rectangle a_intersect, Game1 game)
        {
            if (enemy.Intersects(a_intersect)) // rectange.intersects
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Check if enemy goes off screen
        public bool OffScreen(Game1 game)
        {
            if (enemy.Y > game.screenHight)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Update our enemy velocity
        void UpdateVelocity()
        {
            // Move our enemy
            enemy.Y += speed;
        }
    }
}