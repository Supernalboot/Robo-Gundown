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

        public Shell(string whoShot)
        {
            type = whoShot;
        }

        // Create our Shell Rectangle
        Rectangle shell;

        // set our speed and timers
        int speed = 10;

        //Create our Gun Class
        Player player;

        //Who fired billet
        string type;

        // Create our Texture
        Texture2D shellSprite;

        // Set our fired bool
        bool fired = false;

        // Create vector 2s for our direction and target position
        public Vector2 position;

        // Set our Start Positon
        void SetStartPos<T>(T character)
        {
            //position.X = character.player.Width / 2;
            //position.Y = -character.player.Height;
        }

        // Load our Sprite and Shell
        public void Load<T>(ContentManager content, T character)
        {
            if(type == "player") shellSprite = content.Load<Texture2D>("images/playerBullet");
            if (type == "enemy") shellSprite = content.Load<Texture2D>("images/enemyBullet");
            
            SetStartPos(character);

            shell = new Rectangle(100, 100, shellSprite.Width - 20, shellSprite.Height - 20);
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
            if (type == "player") shell.Y -= speed;
            if (type == "enemy") shell.Y += speed;
        }

        // Collision Detection
        bool HasCollided(Rectangle a_intersect)
        {
            return true;
        }
    }
}
