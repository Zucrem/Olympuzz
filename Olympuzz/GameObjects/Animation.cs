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

        private int sizeX, sizeY;
        private int tsizeX, tsizeY;
        private int posX, posY;
        private Rectangle destRect, sourceRect;

        private float elapsed;
        private float delay = 200f;
        private List<Vector2> sourceRectVector = new List<Vector2>();
        private int frames = 0;
        private int allframes;

        //choose method to do
        private int method;

        public Animation(Texture2D texture, int sizeX, int sizeY) : base(texture)
        {
            this.texture = texture;

            //get size of picture
            this.sizeX = sizeX;
            this.sizeY = sizeY;

            this.method = 1;
        }

        public Animation(Texture2D texture, int sizeX, int sizeY , String number) : base(texture)
        {
            this.texture = texture;

            //get size of picture
            this.sizeX = sizeX;
            this.sizeY = sizeY;

            this.method = 2;
        }

        public void Initialize()
        {
            this.posX = (int)Position.X;
            this.posY = (int)Position.Y;
            destRect = new Rectangle(posX, posY, sizeX, sizeY);
            allframes = sourceRectVector.Count; 
        }
        public override void Update(GameTime gameTime)
        {
            switch (method)
            {
                case 1:
                    elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                    if (elapsed >= delay)
                    {
                        if (frames >= allframes - 1)
                        {
                            frames = 0;
                        }
                        else
                        {
                            frames++;
                        }
                        elapsed = 0;
                    }

                    sourceRect = new Rectangle((int)sourceRectVector[frames].X, (int)sourceRectVector[frames].Y, sizeX, sizeY);
                    break;
                case 2:
                    elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                    Debug.WriteLine("1");
                    int loop = 0;
                    if (elapsed >= delay)
                    {
                        if (frames >= allframes - 1)
                        {
                            Debug.WriteLine("2");
                            loop = 1;
                        }
                        else if (frames < 0)
                        {
                            Debug.WriteLine("3");
                            break;
                        }
                        else
                        {
                            if (loop == 0)
                            {
                                Debug.WriteLine("4");
                                frames++;
                            }
                            else if (loop == 1)
                            {
                                Debug.WriteLine("5");
                                frames--;
                            }
                        }
                        elapsed = 0;
                    }

                    sourceRect = new Rectangle((int)sourceRectVector[frames].X, (int)sourceRectVector[frames].Y, sizeX, sizeY);
                    break;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (method == 1)
            {
                spriteBatch.Draw(texture, destRect, sourceRect, Color.White);
            }
            else if (method == 2)
            {
                spriteBatch.Draw(texture, destRect, new Rectangle((int)sourceRectVector[1].X, (int)sourceRectVector[1].Y, sizeX, sizeY), Color.White);
            }
            //spriteBatch.Draw(texture, new Rectangle(posX, posY, sizeX, sizeY), sourceRect, Color.White);
            //spriteBatch.Draw(texture, new Rectangle(100,100,128,360), Color.White);
            //spriteBatch.Draw(texture, destRect, new Rectangle((int)sourceRectVector[1].X, (int)sourceRectVector[1].Y, sizeX, sizeY), Color.White);
        }

        public void AddVector(Vector2 sourceVector2)
        {
            sourceRectVector.Add(sourceVector2);
        }
    }
}
