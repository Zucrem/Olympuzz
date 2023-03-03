using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Olympuzz.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Olympuzz.GameObjects
{
    public class Button
    {
        private Texture2D texture;
        private Vector2 position;
        private Rectangle bounds;
        private bool isHovered;
        private bool isPressed;

        private SoundEffectInstance clickSound, whileHoveringSound;

        private const int MAX_CLICK_DELAY_MS = 200;

        //sound
        //private SoundEffectInstance soundClickButton, soundEnterGame, soundSelectButton;

        public Button(Texture2D texture, Vector2 position , Vector2 size)
        {
            this.texture = texture;
            this.position = position;
            this.bounds = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            isHovered = false;
            this.isPressed = false;
        }

        public void LoadContent()
        {
            ContentManager content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content"); ;
            // Sounds
            clickSound = content.Load<SoundEffect>("Sounds/clicksound").CreateInstance();
            whileHoveringSound = content.Load<SoundEffect>("Sounds/whilehoveringsound").CreateInstance();
        }

        public void Update(GameTime gameTime)
        {
            //click sound
            clickSound.Volume = Singleton.Instance.soundMasterVolume;
            whileHoveringSound.Volume = Singleton.Instance.soundMasterVolume;
        }

        public bool isWhileHovering(MouseState mouseState)
        {
            //whileHoveringSound.Play();
            isHovered = bounds.Contains(mouseState.Position);
            return isHovered;
        }
        public bool IsClicked(MouseState mouseState, GameTime gameTime)
        {
            clickSound.Play();
            bool wasPressed = isPressed;

            // bound.Contains use to active only if mouse is in Position && check if mouse was left click
            isPressed = bounds.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed; 

            //if left mouse was click && check if mouse was holding
            if (isPressed && !wasPressed)
            {
                //count time from lastClickTime
                //ex. Program was run for 5 sec (5000 ms) and user doesn't click = 0 lastClickTime , then elapsedMs = 5000 - 0
                int elapsedMs = (int)gameTime.TotalGameTime.TotalMilliseconds - Singleton.Instance.lastClickTime;
                // if pass max click delay time since LastClickTime then lastClickTime was that Time
                if (elapsedMs > MAX_CLICK_DELAY_MS)
                {
                    //lastclickTime = TotalTime of program that time
                    //soundClickButton.Play();
                    Singleton.Instance.lastClickTime = (int)gameTime.TotalGameTime.TotalMilliseconds;
                    return true;
                }
            }

            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isWhileHovering(Singleton.Instance.MouseCurrent))
            {
                //soundHoverButton.Play();
                spriteBatch.Draw(texture, bounds, Color.LightGray);
            }
            else
            {
                spriteBatch.Draw(texture, bounds, Color.White);
            }
        }
    }
}
