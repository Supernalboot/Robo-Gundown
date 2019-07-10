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
        Rectangle shell;

        // set our speed and timers
        int speed = 10;

        //Create our Gun Class
        Player player;

        // Create our Texture
        Texture2D shellSprite;

        // Create vector 2s for our direction and target position
        public Vector2 position;

        // Set our Start Positon
        void SetStartPos(Player player)
        {
            position.X = player.player.X + player.player.Width - (shellSprite.Width / 2);// - shell.Width / 2;
            position.Y = player.player.Y - shell.Height;
        }

        // Load our Sprite and Shell
        public void Load(ContentManager content, Player player)
        {
            // Load the Sprite
            shellSprite = content.Load<Texture2D>("images/playerBullet");

            // Set our Starting Position
            SetStartPos(player);

            // Create our new shell rectangle
            shell = new Rectangle((int)position.X, (int)position.Y, shellSprite.Width - 20, shellSprite.Height - 20);
        }

        // Draw our Sprite and Shell
        public void Draw(SpriteBatch a_spriteBatch)
        {
            a_spriteBatch.Begin();
            a_spriteBatch.Draw(shellSprite, shell, Color.White);
            a_spriteBatch.End();
        }

        public void Update()
        {
            shell.Y -= speed;
        }

        public bool OffScreen(Game1 game)
        {
            if (shell.Y + shell.Height < 0)
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
            if (shell.Intersects(a_intersect)) // rectange.intersects
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
