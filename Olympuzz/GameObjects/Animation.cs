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

        //update 2
        private bool animationStop = true;
        private bool re;

        public Animation(Texture2D texture, int sizeX, int sizeY) : base(texture)
        {
            this.texture = texture;

            //get size of picture
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.tsizeX = sizeX;
            this.tsizeY = sizeY;
        }

        public Animation(Texture2D texture, int sizeX, int sizeY, int tsizeX, int tsizeY) : base(texture)
        {
            this.texture = texture;

            //get size of picture
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.tsizeX = tsizeX;
            this.tsizeY = tsizeY;

        }

        public void Initialize()
        {
            this.posX = (int)Position.X;
            this.posY = (int)Position.Y;
            destRect = new Rectangle(posX, posY, tsizeX, tsizeY);
            allframes = sourceRectVector.Count;

            re = false;
        }
        public override void Update(GameTime gameTime)
        {
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
        }
        public void UpdateHermesSkill(GameTime gameTime,float d)
        {
            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsed >= d)
            {
                if (frames >= allframes - 1 && !re)
                {
                    re = true;
                }
                else if (!re)
                {
                    frames++;
                }
                else if (frames == 0 && re)
                {
                    animationStop = true;
                    return;
                }
                else if (re)
                {
                    frames--;
                }
                elapsed = 0;
            }

            sourceRect = new Rectangle((int)sourceRectVector[frames].X, (int)sourceRectVector[frames].Y, sizeX, sizeY);
        }

        public void UpdateAthenaSkill(GameTime gameTime, float d , bool isSkill)
        {
            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsed >= d)
            {
                if (!isSkill)
                {
                    if (frames == 6)
                    {
                        frames = 5;
                    }
                    else if (frames < 6)
                    {
                        frames++;
                    }
                }
                else if (isSkill)
                {
                    if (frames == 0)
                    {
                        frames = 0;
                    }
                    else if (frames > 0)
                    {
                        frames--;
                    }
                }
                elapsed = 0;
            }

            sourceRect = new Rectangle((int)sourceRectVector[frames].X, (int)sourceRectVector[frames].Y, sizeX, sizeY);
        }

        public void UpdateHephaestusSkill(GameTime gameTime)
        {
            elapsed += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsed >= 300)
            {
                if (frames >= allframes - 1)
                {
                    return;
                }
                else
                {
                    frames++;
                }
                elapsed = 0;
            }

            sourceRect = new Rectangle((int)sourceRectVector[frames].X, (int)sourceRectVector[frames].Y, sizeX, sizeY);
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, destRect, sourceRect, Color.White);
        }

        public void AddVector(Vector2 sourceVector2)
        {
            sourceRectVector.Add(sourceVector2);
        }

        public bool GetAnimationStop()
        {
            return animationStop;
        }

        public void SetAnimationStop(bool b)
        {
            animationStop = b;
        }
        
        public int GetAllFrame()
        { 
            return allframes;
        }
        
        public int GetFrames()
        {
            return frames;
        }

        public void ResetFrame()
        {
            frames = 0;
        }

        public void ResetAnimation()
        {
            frames = 0;
            animationStop = true;
            elapsed = 0;
            re = false;
        }
    }
}
