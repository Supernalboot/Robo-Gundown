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
    public class Shell
    {

        public Shell()
        {
        }

        // Create our Shell Rectangle
        Rectangle shellRect;

        // set our speed and timers
        int speed = 10;

        // Create our Texture
        Texture2D shellSprite;

        // Create vector 2s for our direction and target position
        public Vector2 position;

        // Set our Start Positon
        void SetStartPos(Player player)
        {
            position.X = player.playerRect.X + player.playerRect.Width - (shellSprite.Width / 2);// - shell.Width / 2;
            position.Y = player.playerRect.Y - shellRect.Height;
        }

        // Load our Sprite and Shell
        public void Load(ContentManager content, Player player)
        {
            // Load the Sprite
            shellSprite = content.Load<Texture2D>("images/playerBullet");

            // Set our Starting Position
            SetStartPos(player);

            // Create our new shell rectangle
            shellRect = new Rectangle((int)position.X, (int)position.Y, shellSprite.Width - 20, shellSprite.Height - 20);
        }

        // Draw our Sprite and Shell
        public void Draw(SpriteBatch a_spriteBatch)
        {
            a_spriteBatch.Begin();
            a_spriteBatch.Draw(shellSprite, shellRect, Color.White);
            a_spriteBatch.End();
        }

        public void Update()
        {
            shellRect.Y -= speed;
        }

        public bool OffScreen(Game1 game)
        {
            if (shellRect.Y + shellRect.Height < 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Collision Detection
        public bool HasCollided(Rectangle a_intersect)
        {
            if (shellRect.Intersects(a_intersect)) // rectange.intersects
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
