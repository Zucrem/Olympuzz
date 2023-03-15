using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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
    public class Animation : _GameObject
    {
        private Texture2D texture;
        private int posX, posY;

        private int frameSize;
        private int spriteX, spriteY;
        private int sizeX, sizeY;
        private int tsizeX, tsizeY;
        private Rectangle destRect, sourceRect;

        private float elapsed;
        private float delay = 300f;
        private int frames = 0;

        public Animation(Texture2D texture, int spriteX, int spriteY, int sizeX, int sizeY, int tsizeX, int tsizeY, int frameSize) : base(texture)
        {
            this.texture = texture;

            this.frameSize = frameSize;
            //get amout sprite per row and column
            this.spriteX = spriteX;
            this.spriteY = spriteY;
            //get size of texture
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            //get size of sprite texture
            this.tsizeX = tsizeX;
            this.tsizeY = tsizeY;

        }

        public Animation(Texture2D texture, int sizeX, int sizeY, int tsizeX, int tsizeY, int frameSize) : base(texture)
        {
            this.texture = texture;

            this.frameSize = frameSize;
            //get size of picture
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            //get size of sprite texture
            this.tsizeX = tsizeX;
            this.tsizeY = tsizeY;

        }

        public void Initialize()
        {
            this.posX = (int)Position.X;
            this.posY = (int)Position.Y;
            destRect = new Rectangle(posX, posY, sizeX, sizeY);
        }
        public override void Update(GameTime gameTime)
        {
            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            
            if(elapsed >= delay)
            {
                if (frames >= frameSize-1)
                {
                    frames = 0;
                }
                else
                {
                    frames++;
                }
                elapsed = 0;
            }

            sourceRect = new Rectangle(tsizeX * frames, 0, tsizeX,tsizeY);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, destRect, sourceRect, Color.White);
            //spriteBatch.Draw(texture, new Rectangle(posX, posY, sizeX, sizeY), sourceRect, Color.White);
            //spriteBatch.Draw(texture, new Rectangle(100,100,128,360), Color.White);
            //spriteBatch.Draw(texture, destRect, new Rectangle(tsizeX * 2, 0, tsizeX, tsizeY), Color.White);
        }
    }
}
