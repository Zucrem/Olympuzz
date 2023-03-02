using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olympuzz.GameObjects
{
    public class Bubble : _GameObject
    {
        public Random random = new Random();
        public float Angle; // angle of cannon use to set where bubble will fly to
        Texture2D bubbleTexture;
        Texture2D[] allTexture;
        public bool isEven;

        public Bubble(Texture2D[] allTexture) : base (allTexture)
        {
            this.allTexture = allTexture;
            bubbleTexture = RandomBubble();
        }

        public Bubble(Texture2D bubble) : base(bubble)
        {
            bubbleTexture = bubble;
        }

        public override void Update(GameTime gameTime, Bubble[,] bubbles)
        {
            if (IsActive)
            {
                Velocity.X = (float)Math.Cos(Angle) * Singleton.Instance.speed; //direction of bubble to go in axis x
                Velocity.Y = (float)Math.Sin(Angle) * Singleton.Instance.speed; // direction of bubble to go in axis y
                Position += Velocity * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond; // position of bubble that will increase to direction that canon point to
                bubbleCollision(bubbles);
                //if (Position.Y <= 40)
                //{
                //    IsActive = false;
                //    if (Position.X > 880)
                //    {
                //        bubbles[0, 7] = this;
                //        isEven = !isEven;
                //        Position = new Vector2((j * 49) + (isEven ? 363 : 388), (i * 42) + 79);
                //    }
                //    else if (Position.X > 800)
                //    {
                //        bubbles[0, 6] = this;
                //        Position = new Vector2((6 * 80) + ((0 % 2) == 0 ? 320 : 360), (0 * 70) + 40);
                //    }
                //    else if (Position.X > 720)
                //    {
                //        bubbles[0, 5] = this;
                //        Position = new Vector2((5 * 80) + ((0 % 2) == 0 ? 320 : 360), (0 * 70) + 40);
                //    }
                //    else if (Position.X > 640)
                //    {
                //        bubbles[0, 4] = this;
                //        Position = new Vector2((4 * 80) + ((0 % 2) == 0 ? 320 : 360), (0 * 70) + 40);
                //    }
                //    else if (Position.X > 560)
                //    {
                //        bubbles[0, 3] = this;
                //        Position = new Vector2((3 * 80) + ((0 % 2) == 0 ? 320 : 360), (0 * 70) + 40);
                //    }
                //    else if (Position.X > 480)
                //    {
                //        bubbles[0, 2] = this;
                //        Position = new Vector2((2 * 80) + ((0 % 2) == 0 ? 320 : 360), (0 * 70) + 40);
                //    }
                //    else if (Position.X > 400)
                //    {
                //        bubbles[0, 1] = this;
                //        Position = new Vector2((1 * 80) + ((0 % 2) == 0 ? 320 : 360), (0 * 70) + 40);
                //    }
                //    else if (Position.X > 320)
                //    {
                //        bubbles[0, 0] = this;
                //        Position = new Vector2((0 * 80) + ((0 % 2) == 0 ? 320 : 360), (0 * 70) + 40);
                //    }
                //    Singleton.Instance.Shooting = false;

                //    //stickSFX.Volume = Singleton.Instance.SFX_MasterVolume;
                //    //stickSFX.Play();
                //}
                if (Position.X < 363) 
                {   
                    Angle = -Angle;
                    Angle += MathHelper.ToRadians(180);
                    //speed = 0;
                }

                if (Position.X > 807) 
                { 
                    Angle = -Angle; 
                    Angle += MathHelper.ToRadians(180);
                    //speed = 0;
                }
            }
        }

        private void bubbleCollision(Bubble[,] bubbles)
        {
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (bubbles[i, j] != null && !bubbles[i, j].IsActive) //if bubble at that position have a bubble and exist in board
                    {
                        if (CheckCollision(bubbles[i, j]) <= 42)
                        {
                            if (Position.X >= bubbles[i, j].Position.X && (Position.Y - 20) <= bubbles[i, j].Position.Y) // Set side ball in right side of previous ball
                            {
                                bubbles[i, j + 1] = this;
                                bubbles[i, j + 1].isEven = bubbles[i, j].isEven;
                                bubbles[i, j + 1].Position = new Vector2(((j + 1) * 49) + (bubbles[i, j].isEven ? 363 : 388), (i * 42) + 79);
                            }
                            else if ((Position.Y - 20) <= bubbles[i, j].Position.Y) // Set side ball in left side of previous ball
                            {
                                bubbles[i, j - 1] = this;
                                bubbles[i, j - 1].isEven = bubbles[i, j].isEven;
                                bubbles[i, j - 1].Position = new Vector2(((j - 1) * 49) + (bubbles[i, j].isEven ? 363 : 388), (i * 42) + 79);
                            }
                            else if (Position.X >= bubbles[i, j].Position.X && Position.Y > bubbles[i,j].Position.Y) // Set under ball 
                            {
                                if (bubbles[i, j].isEven)
                                {
                                    if (j == 9)
                                    {
                                        bubbles[i + 1, j - 1] = this;
                                        bubbles[i + 1, j - 1].isEven = !bubbles[i, j].isEven;
                                        bubbles[i + 1, j - 1].Position = new Vector2(((j - 1) * 49) + (bubbles[i, j].isEven ? 388 : 363), ((i + 1) * 42) + 79);
                                    }
                                    else
                                    {
                                        bubbles[i + 1, j] = this;
                                        bubbles[i + 1, j].isEven = !bubbles[i, j].isEven;
                                        bubbles[i + 1, j].Position = new Vector2((j * 49) + (bubbles[i, j].isEven ? 388 : 363), ((i + 1) * 42) + 79);
                                    }
                                }
                                else
                                {
                                    bubbles[i + 1, j + 1] = this;
                                    bubbles[i + 1, j + 1].isEven = !bubbles[i, j].isEven;
                                    bubbles[i + 1, j + 1].Position = new Vector2(((j + 1) * 49) + (bubbles[i, j].isEven ? 388 : 363), ((i + 1) * 42) + 79);
                                }
                            }
                            else if (Position.Y >= bubbles[i, j].Position.Y)
                            {
                                if (bubbles[i, j].isEven)
                                {
                                    bubbles[i + 1, j - 1] = this;
                                    bubbles[i + 1, j - 1].isEven = !bubbles[i, j].isEven;
                                    bubbles[i + 1, j - 1].Position = new Vector2(((j - 1) * 49) + (bubbles[i, j].isEven ? 388 : 363), ((i + 1) * 42) + 79);
                                }
                                else
                                {
                                    bubbles[i + 1, j] = this;
                                    bubbles[i + 1, j].isEven = !bubbles[i, j].isEven;
                                    bubbles[i + 1, j].Position = new Vector2((j * 49) + (bubbles[i, j].isEven ? 388 : 363), ((i + 1) * 42) + 79);
                                }
                            }
                            IsActive = false;
                            Singleton.Instance.removeBubble.Clear();
                            Singleton.Instance.Shooting = false;
                            return;
                        }
                    }
                }
            }
        }

        public int CheckCollision(Bubble other)
        {
            return (int)Math.Sqrt(Math.Pow(Position.X - other.Position.X, 2) + Math.Pow(Position.Y - other.Position.Y, 2));
        }

        public override void Draw(SpriteBatch _spriteBatch)
        {
            //draw(texture,position,reactangle,color,angle,origin,scale,effect,orderInlayer) buble at Position and origin of Image is middle bottom of image
            _spriteBatch.Draw(bubbleTexture, Position, null, Color.White, 0, new Vector2(bubbleTexture.Width / 2, bubbleTexture.Height/2), 1f, SpriteEffects.None, 0f);
        }

        public Texture2D RandomBubble()
        {
            int randNum = random.Next(allTexture.Length);
            return allTexture[randNum];
        }
    }
}
