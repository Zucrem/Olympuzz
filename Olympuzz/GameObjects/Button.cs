using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olympuzz.GameObjects
{
    public class Button
    {
        private Texture2D texture;
        private Vector2 position;
        private Rectangle bounds;
        private bool isPressed;

        private const int MAX_CLICK_DELAY_MS = 300;
        private int lastClickTime;

        public Button(Texture2D texture, Vector2 position , Vector2 size)
        {
            this.texture = texture;
            this.position = position;
            this.bounds = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            this.isPressed = false;
            this.lastClickTime = 0;
        }

        public bool IsClicked(MouseState mouseState, GameTime gameTime)
        {
            bool wasPressed = isPressed;
            isPressed = bounds.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed;

            if (isPressed && !wasPressed)
            {
                int elapsedMs = (int)gameTime.TotalGameTime.TotalMilliseconds - lastClickTime;
                if (elapsedMs > MAX_CLICK_DELAY_MS)
                {
                    lastClickTime = (int)gameTime.TotalGameTime.TotalMilliseconds;
                    return true;
                }
            }

            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, bounds, Color.White);
        }
    }
}
