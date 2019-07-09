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
   public class Button
    {
        // Create our texture
        Texture2D buttonSprite;

        // Create our button type
        int type;
        string stringType;

        // Create our rectange for our gun
        public Rectangle button;

        public Button()
        {

        }

        public void Load(ContentManager content, int X, int Y, string imageName, int Type)
        {
            type = Type;
            // Load our sprite
            buttonSprite = content.Load<Texture2D>("images/" + imageName);

            // Load our gun Rectangle
            button = new Rectangle(X, Y, buttonSprite.Width - 20, buttonSprite.Height - 60);

            // Check and set what type was given
            if (type == 0)
            {
                stringType = "start";
            }
            else if (type == 1)
            {
                stringType = "exit";
            }
        }

        public void Update(GameTime gameTime, Game1 game)
        {
            // Get our mouseState every update
            MouseState mouseState = Mouse.GetState();

            if (mouseState.X < button.X + buttonSprite.Width &&
                    mouseState.X > button.X &&
                    mouseState.Y < button.Y + buttonSprite.Height &&
                    mouseState.Y > button.Y)
            {
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    if (type == 0)
                    {
                        game.setState();
                    }

                    if (type == 1)
                    {
                        game.Quit();
                    }
                }
            }
        }

        public void Draw(SpriteBatch a_spriteBatch)
        {
            a_spriteBatch.Begin();
            a_spriteBatch.Draw(buttonSprite, button, Color.White); //gunSprite, gun, gunSprite.Bounds ,Color.White, 50f, origin.ToVector2, SpriteEffects.None, 1
            a_spriteBatch.End();
        }
    }
}
