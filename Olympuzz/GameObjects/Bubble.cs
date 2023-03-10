using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization.Formatters;

namespace Olympuzz.GameObjects
{
    public class Bubble : _GameObject
    {
        private Random random = new Random();
        public float Angle; // angle of cannon use to set where bubble will fly to
        private Texture2D bubbleTexture;
        Texture2D[] allTexture;
        public bool isEven;
        public bool marked = false;
        List<Vector2> clusterBubble = new List<Vector2>();
        public bool isRemovable = false;

        public Bubble(Texture2D[] allTexture) : base (allTexture)
        {
            this.allTexture = allTexture;
            bubbleTexture = RandomBubble();
            //ranRotate = random.Next(2);

        }

        public void Cheat()
        {
            bubbleTexture = allTexture[2];
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

                //for (int j = 0; j < bubbles.GetLength(1); j++)
                //{
                //    if (bubbles[1, j] != null)
                //    {
                //        isEven = bubbles[1, j].isEven;
                //        break;
                //    }
                //}

                //if (ranRotate == 0)
                //{
                //    rotateBubble += ((float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond) * 5;
                //}
                //else
                //{
                //    rotateBubble -= ((float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond) * 5;
                //}


                for (int j = 0;j < (((bubbles[1, j] != null) && (bubbles[1, j].isEven))? 9 : 8); j++)
                {
                    if (Position.Y <= 79 && bubbles[1, j] != null && bubbles[1, j].isEven)
                    {
                        if (Position.X < 373)
                        {
                            bubbles[0, 0] = this;
                            bubbles[0, 0].isEven = true;
                            bubbles[0, 0].Position = new Vector2((0 * 49) + 363 , 79);
                            Shooter.canRotate = true;
                            Shooter.nextBubble();
                            bubbles[0, 0].Angle = 0;
                            RemoveBubble(bubbles, new Vector2(0, 0), bubbles[0, 0].bubbleTexture);
                        }
                        else if (Position.X < 422)
                        {
                            bubbles[0, 1] = this;
                            bubbles[0, 1].isEven = true;
                            bubbles[0, 1].Position = new Vector2((1 * 49) + 363, 79);
                            Shooter.canRotate = true;
                            Shooter.nextBubble();
                            bubbles[0, 1].Angle = 0;
                            RemoveBubble(bubbles, new Vector2(1, 0), bubbles[0, 1].bubbleTexture);
                        }
                        else if (Position.X < 471)
                        {
                            bubbles[0, 2] = this;
                            bubbles[0, 2].isEven = true;
                            bubbles[0, 2].Position = new Vector2((2 * 49) + 363, 79);
                            Shooter.canRotate = true;
                            Shooter.nextBubble();
                            bubbles[0, 2].Angle = 0;
                            RemoveBubble(bubbles, new Vector2(2, 0), bubbles[0, 2].bubbleTexture);
                        }
                        else if (Position.X < 520)
                        {
                            bubbles[0, 3] = this;
                            bubbles[0, 3].isEven = true;
                            bubbles[0, 3].Position = new Vector2((3 * 49) + 363, 79);
                            Shooter.canRotate = true;
                            Shooter.nextBubble();
                            bubbles[0, 3].Angle = 0;
                            RemoveBubble(bubbles, new Vector2(3, 0), bubbles[0, 3].bubbleTexture);
                        }
                        else if (Position.X < 569)
                        {
                            bubbles[0, 4] = this;
                            bubbles[0, 4].isEven = true;
                            bubbles[0, 4].Position = new Vector2((4 * 49) + 363, 79);
                            Shooter.canRotate = true;
                            Shooter.nextBubble();
                            bubbles[0, 4].Angle = 0;
                            RemoveBubble(bubbles, new Vector2(4, 0), bubbles[0, 4].bubbleTexture);
                        }
                        else if (Position.X < 618)
                        {
                            bubbles[0, 5] = this;
                            bubbles[0, 5].isEven = true;
                            bubbles[0, 5].Position = new Vector2((5 * 49) + 363, 79);
                            Shooter.canRotate = true;
                            Shooter.nextBubble();
                            bubbles[0, 5].Angle = 0;
                            RemoveBubble(bubbles, new Vector2(5, 0), bubbles[0, 5].bubbleTexture);
                        }
                        else if (Position.X < 667)
                        {
                            bubbles[0, 6] = this;
                            bubbles[0, 6].isEven = true;
                            bubbles[0, 6].Position = new Vector2((6 * 49) + 363, 79);
                            Shooter.canRotate = true;
                            Shooter.nextBubble();
                            bubbles[0, 6].Angle = 0;
                            RemoveBubble(bubbles, new Vector2(6, 0), bubbles[0, 6].bubbleTexture);
                        }
                        else if (Position.X < 716)
                        {
                            bubbles[0, 7] = this;
                            bubbles[0, 7].isEven = true;
                            bubbles[0, 7].Position = new Vector2((7 * 49) + 363, 79);
                            Shooter.canRotate = true;
                            Shooter.nextBubble();
                            bubbles[0, 7].Angle = 0;
                            RemoveBubble(bubbles, new Vector2(7, 0), bubbles[0, 7].bubbleTexture);
                        }
                        else if (Position.X < 765)
                        {
                            bubbles[0, 8] = this;
                            bubbles[0, 8].isEven = true;
                            bubbles[0, 8].Position = new Vector2((8 * 49) + 363, 79);
                            Shooter.canRotate = true;
                            Shooter.nextBubble();
                            bubbles[0, 8].Angle = 0;
                            RemoveBubble(bubbles, new Vector2(8, 0), bubbles[0, 8].bubbleTexture);
                        }
                        else if (Position.X < 814)
                        {
                            bubbles[0, 9] = this;
                            bubbles[0, 9].isEven = true;
                            bubbles[0, 9].Position = new Vector2((9 * 49) + 363, 79);
                            Shooter.canRotate = true;
                            Shooter.nextBubble();
                            bubbles[0, 9].Angle = 0;
                            RemoveBubble(bubbles, new Vector2(9, 0), bubbles[0, 9].bubbleTexture);
                        }

                        IsActive = false;
                        Singleton.Instance.Shooting = false;

                        break;

                        //stickSFX.Volume = Singleton.Instance.SFX_MasterVolume;
                        //stickSFX.Play();
                    }
                    else if (Position.Y <= 79 && bubbles[1, j] != null && !bubbles[1, j].isEven)
                    {
                        if (Position.X < 398)
                        {
                            bubbles[0, 0] = this;
                            bubbles[0, 0].isEven = false;
                            bubbles[0, 0].Position = new Vector2((0 * 49) + 388, 79);
                            Shooter.canRotate = true;
                            Shooter.nextBubble();
                            bubbles[0, 0].Angle = 0;
                            RemoveBubble(bubbles, new Vector2(0, 0), bubbles[0, 0].bubbleTexture);
                        }
                        else if (Position.X < 447)
                        {
                            bubbles[0, 1] = this;
                            bubbles[0, 1].isEven = false;
                            bubbles[0, 1].Position = new Vector2((1 * 49) + 388, 79);
                            Shooter.canRotate = true;
                            Shooter.nextBubble();
                            bubbles[0, 1].Angle = 0;
                            RemoveBubble(bubbles, new Vector2(1, 0), bubbles[0, 1].bubbleTexture);
                        }
                        else if (Position.X < 496)
                        {
                            bubbles[0, 2] = this;
                            bubbles[0, 2].isEven = false;
                            bubbles[0, 2].Position = new Vector2((2 * 49) + 388, 79);
                            Shooter.canRotate = true;
                            Shooter.nextBubble();
                            bubbles[0, 2].Angle = 0;
                            RemoveBubble(bubbles, new Vector2(2, 0), bubbles[0, 2].bubbleTexture);
                        }
                        else if (Position.X < 545)
                        {
                            bubbles[0, 3] = this;
                            bubbles[0, 3].isEven = false;
                            bubbles[0, 3].Position = new Vector2((3 * 49) + 388, 79);
                            Shooter.canRotate = true;
                            Shooter.nextBubble();
                            bubbles[0, 3].Angle = 0;
                            RemoveBubble(bubbles, new Vector2(3, 0), bubbles[0, 3].bubbleTexture);
                        }
                        else if (Position.X < 594)
                        {
                            bubbles[0, 4] = this;
                            bubbles[0, 4].isEven = false;
                            bubbles[0, 4].Position = new Vector2((4 * 49) + 388, 79);
                            Shooter.canRotate = true;
                            Shooter.nextBubble();
                            bubbles[0, 4].Angle = 0;
                            RemoveBubble(bubbles, new Vector2(4, 0), bubbles[0, 4].bubbleTexture);
                        }
                        else if (Position.X < 643)
                        {
                            bubbles[0, 5] = this;
                            bubbles[0, 5].isEven = false;
                            bubbles[0, 5].Position = new Vector2((5 * 49) + 388, 79);
                            Shooter.canRotate = true;
                            Shooter.nextBubble();
                            bubbles[0, 5].Angle = 0;
                            RemoveBubble(bubbles, new Vector2(5, 0), bubbles[0, 5].bubbleTexture);
                        }
                        else if (Position.X < 692)
                        {
                            bubbles[0, 6] = this;
                            bubbles[0, 6].isEven = false;
                            bubbles[0, 6].Position = new Vector2((6 * 49) + 388, 79);
                            Shooter.canRotate = true;
                            Shooter.nextBubble();
                            bubbles[0, 6].Angle = 0;
                            RemoveBubble(bubbles, new Vector2(6, 0), bubbles[0, 6].bubbleTexture);
                        }
                        else if (Position.X < 741)
                        {
                            bubbles[0, 7] = this;
                            bubbles[0, 7].isEven = false;
                            bubbles[0, 7].Position = new Vector2((7 * 49) + 388, 79);
                            Shooter.canRotate = true;
                            Shooter.nextBubble();
                            bubbles[0, 7].Angle = 0;
                            RemoveBubble(bubbles, new Vector2(7, 0), bubbles[0, 7].bubbleTexture);
                        }
                        else if (Position.X < 790)
                        {
                            bubbles[0, 8] = this;
                            bubbles[0, 8].isEven = false;
                            bubbles[0, 8].Position = new Vector2((8 * 49) + 388, 79);
                            Shooter.canRotate = true;
                            Shooter.nextBubble();
                            bubbles[0, 8].Angle = 0;
                            RemoveBubble(bubbles, new Vector2(8, 0), bubbles[0, 8].bubbleTexture);
                        }

                        IsActive = false;
                        Singleton.Instance.Shooting = false;

                        break;

                        //stickSFX.Volume = Singleton.Instance.SFX_MasterVolume;
                        //stickSFX.Play();
                    }
                }

                

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
                                Shooter.canRotate = true;
                                Shooter.nextBubble();
                                bubbles[i, j + 1].Angle = 0;
                                RemoveBubble(bubbles, new Vector2(j + 1, i), bubbles[i, j + 1].bubbleTexture);
                            }
                            else if ((Position.Y - 20) <= bubbles[i, j].Position.Y) // Set side ball in left side of previous ball
                            {
                                bubbles[i, j - 1] = this;
                                bubbles[i, j - 1].isEven = bubbles[i, j].isEven;
                                bubbles[i, j - 1].Position = new Vector2(((j - 1) * 49) + (bubbles[i, j].isEven ? 363 : 388), (i * 42) + 79);
                                Shooter.canRotate = true;
                                Shooter.nextBubble();
                                bubbles[i, j - 1].Angle = 0;
                                RemoveBubble(bubbles, new Vector2(j - 1, i), bubbles[i, j - 1].bubbleTexture);
                            }
                            else if (Position.X >= bubbles[i, j].Position.X && Position.Y > bubbles[i, j].Position.Y) // Set under ball 
                            {
                                if (bubbles[i, j].isEven)
                                {
                                    if (j == 9)
                                    {
                                        bubbles[i + 1, j - 1] = this;
                                        bubbles[i + 1, j - 1].isEven = !bubbles[i, j].isEven;
                                        bubbles[i + 1, j - 1].Position = new Vector2(((j - 1) * 49) + (bubbles[i, j].isEven ? 388 : 363), ((i + 1) * 42) + 79);
                                        Shooter.canRotate = true;
                                        Shooter.nextBubble();
                                        bubbles[i + 1, j - 1].Angle = 0;
                                        RemoveBubble(bubbles, new Vector2(j - 1, i + 1), bubbles[i + 1, j - 1].bubbleTexture);
                                    }
                                    else
                                    {
                                        bubbles[i + 1, j] = this;
                                        bubbles[i + 1, j].isEven = !bubbles[i, j].isEven;
                                        bubbles[i + 1, j].Position = new Vector2((j * 49) + (bubbles[i, j].isEven ? 388 : 363), ((i + 1) * 42) + 79);
                                        Shooter.canRotate = true;
                                        Shooter.nextBubble();
                                        bubbles[i + 1, j].Angle = 0;
                                        RemoveBubble(bubbles, new Vector2(j, i + 1), bubbles[i + 1, j].bubbleTexture);
                                        Debug.WriteLine("DD");
                                        Debug.WriteLine(bubbles[i + 1, j].isEven);
                                    }
                                }
                                else
                                {
                                    bubbles[i + 1, j + 1] = this;
                                    bubbles[i + 1, j + 1].isEven = !bubbles[i, j].isEven;
                                    bubbles[i + 1, j + 1].Position = new Vector2(((j + 1) * 49) + (bubbles[i, j].isEven ? 388 : 363), ((i + 1) * 42) + 79);
                                    Shooter.canRotate = true;
                                    Shooter.nextBubble();
                                    bubbles[i + 1, j + 1].Angle = 0;
                                    RemoveBubble(bubbles, new Vector2(j + 1, i + 1), bubbles[i + 1, j + 1].bubbleTexture);
                                    Debug.WriteLine("AA");
                                    Debug.WriteLine(" I : "+ i + " J : " + j + " Bubbble ij : " + bubbles[i, j].isEven + " Bubble IJ1 : " + bubbles[i + 1, j + 1].isEven);
                                }
                            }
                            else if (Position.Y >= bubbles[i, j].Position.Y) //set Left under ball 
                            {
                                if (bubbles[i, j].isEven) //if ball is Even ball will set this as Odd Line
                                {
                                    bubbles[i + 1, j - 1] = this;
                                    bubbles[i + 1, j - 1].isEven = !bubbles[i, j].isEven;
                                    bubbles[i + 1, j - 1].Position = new Vector2(((j - 1) * 49) + (bubbles[i, j].isEven ? 388 : 363), ((i + 1) * 42) + 79);
                                    Shooter.canRotate = true;
                                    Shooter.nextBubble();
                                    bubbles[i + 1, j - 1].Angle = 0;
                                    RemoveBubble(bubbles, new Vector2(j - 1, i + 1), bubbles[i + 1, j - 1].bubbleTexture);
                                    Debug.WriteLine("SS");
                                    Debug.WriteLine(bubbles[i + 1, j - 1].isEven);
                                }
                                else
                                {
                                    bubbles[i + 1, j] = this;
                                    bubbles[i + 1, j].isEven = !bubbles[i, j].isEven;
                                    bubbles[i + 1, j].Position = new Vector2((j * 49) + (bubbles[i, j].isEven ? 388 : 363), ((i + 1) * 42) + 79);
                                    Shooter.canRotate = true;
                                    Shooter.nextBubble();
                                    bubbles[i + 1, j].Angle = 0;
                                    RemoveBubble(bubbles, new Vector2(j, i + 1), bubbles[i + 1, j].bubbleTexture);
                                }
                            }
                            IsActive = false;
                            Singleton.Instance.Shooting = false;
                            if (Singleton.Instance.removeBubble.Count >= 3)
                            {
                                isRemovable = false;
                                foreach (Vector2 vectorRemove in Singleton.Instance.removeBubble)
                                {
                                    bubbles[(int)vectorRemove.Y, (int)vectorRemove.X] = null;
                                }

                                foreach (Vector2 vectorRemove in Singleton.Instance.removeBubble)
                                {
                                    CheckFloating(bubbles, new Vector2(vectorRemove.X - 1,vectorRemove.Y));
                                    
                                    if (isRemovable)
                                    {
                                        clusterBubble.Clear();
                                        isRemovable = false;
                                    }

                                    foreach (Vector2 vectorRemoved in clusterBubble)
                                    {
                                        bubbles[(int)vectorRemoved.Y, (int)vectorRemoved.X] = null;
                                    }

                                    //removeCluster = clusterBubble
                                    CheckFloating(bubbles, new Vector2(vectorRemove.X + 1, vectorRemove.Y));
                                    
                                    if (isRemovable)
                                    {
                                        clusterBubble.Clear();
                                        isRemovable = false;
                                    }

                                    foreach (Vector2 vectorRemoved in clusterBubble)
                                    {
                                        bubbles[(int)vectorRemoved.Y, (int)vectorRemoved.X] = null;
                                    }

                                    CheckFloating(bubbles, new Vector2(vectorRemove.X, vectorRemove.Y - 1));

                                    if (isRemovable)
                                    {
                                        clusterBubble.Clear();
                                        isRemovable = false;
                                    }

                                    foreach (Vector2 vectorRemoved in clusterBubble)
                                    {
                                        bubbles[(int)vectorRemoved.Y, (int)vectorRemoved.X] = null;
                                    }
                                    //removeCluster.AddRange(clusterBubble);

                                    if (bubbles[(int)vectorRemove.Y, (int)vectorRemove.X] != null && bubbles[(int)vectorRemove.Y, (int)vectorRemove.X].isEven)
                                    {
                                        CheckFloating(bubbles, new Vector2(vectorRemove.X - 1, vectorRemove.Y - 1));

                                        if (isRemovable)
                                        {
                                            clusterBubble.Clear();
                                            isRemovable = false;
                                        }

                                        foreach (Vector2 vectorRemoved in clusterBubble)
                                        {
                                            bubbles[(int)vectorRemoved.Y, (int)vectorRemoved.X] = null;
                                        }

                                        CheckFloating(bubbles, new Vector2(vectorRemove.X - 1, vectorRemove.Y + 1));

                                        if (isRemovable)
                                        {
                                            clusterBubble.Clear();
                                            isRemovable = false;
                                        }

                                        foreach (Vector2 vectorRemoved in clusterBubble)
                                        {
                                            bubbles[(int)vectorRemoved.Y, (int)vectorRemoved.X] = null;
                                        }
                                    }
                                    else
                                    {
                                        CheckFloating(bubbles, new Vector2(vectorRemove.X + 1, vectorRemove.Y - 1));

                                        if (isRemovable)
                                        {
                                            clusterBubble.Clear();
                                            isRemovable = false;
                                        }

                                        foreach (Vector2 vectorRemoved in clusterBubble)
                                        {
                                            bubbles[(int)vectorRemoved.Y, (int)vectorRemoved.X] = null;
                                        }

                                        CheckFloating(bubbles, new Vector2(vectorRemove.X + 1, vectorRemove.Y + 1));

                                        if (isRemovable)
                                        {
                                            clusterBubble.Clear();
                                            isRemovable = false;
                                        }

                                        foreach (Vector2 vectorRemoved in clusterBubble)
                                        {
                                            bubbles[(int)vectorRemoved.Y, (int)vectorRemoved.X] = null;
                                        }
                                    }
                                }
                            }

                            Singleton.Instance.removeBubble.Clear();

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

        

        public void RemoveBubble(Bubble[,] bubbles, Vector2 bubbleRemoved, Texture2D targetTexture)
        {
            if ((bubbleRemoved.Y > 11 || bubbleRemoved.Y < 0) || (bubbleRemoved.X < 0 || bubbleRemoved.X > 9) || bubbles[(int)bubbleRemoved.Y, (int)bubbleRemoved.X] == null  || (!bubbles[(int)bubbleRemoved.Y, (int)bubbleRemoved.X].bubbleTexture.Equals(targetTexture)) || Singleton.Instance.removeBubble.Contains(bubbleRemoved))
            {
                return;
            }

            if (!(Singleton.Instance.removeBubble.Contains(bubbleRemoved)))
            {
                Singleton.Instance.removeBubble.Add(bubbleRemoved);
            }

            RemoveBubble(bubbles, new Vector2(bubbleRemoved.X, bubbleRemoved.Y - 1), bubbles[(int)bubbleRemoved.Y, (int)bubbleRemoved.X].bubbleTexture); // check up bubble
            RemoveBubble(bubbles, new Vector2(bubbleRemoved.X + 1, bubbleRemoved.Y), bubbles[(int)bubbleRemoved.Y, (int)bubbleRemoved.X].bubbleTexture); // check right bubble
            RemoveBubble(bubbles, new Vector2(bubbleRemoved.X - 1, bubbleRemoved.Y), bubbles[(int)bubbleRemoved.Y, (int)bubbleRemoved.X].bubbleTexture); // check left bubble

            RemoveBubble(bubbles, new Vector2(bubbleRemoved.X, bubbleRemoved.Y + 1), bubbles[(int)bubbleRemoved.Y, (int)bubbleRemoved.X].bubbleTexture); // check left bubble



            if (bubbles[(int)bubbleRemoved.Y, (int)bubbleRemoved.X].isEven)
            {
                RemoveBubble(bubbles, new Vector2(bubbleRemoved.X - 1, bubbleRemoved.Y - 1), bubbles[(int)bubbleRemoved.Y, (int)bubbleRemoved.X].bubbleTexture); //if this is Even line will check up left bubble
                RemoveBubble(bubbles, new Vector2(bubbleRemoved.X - 1, bubbleRemoved.Y + 1), bubbles[(int)bubbleRemoved.Y, (int)bubbleRemoved.X].bubbleTexture); // if this is Even line will check down left bubble
            }
            else
            {
                RemoveBubble(bubbles, new Vector2(bubbleRemoved.X + 1, bubbleRemoved.Y - 1), bubbles[(int)bubbleRemoved.Y, (int)bubbleRemoved.X].bubbleTexture); //if this is odd line will check up right bubble
                RemoveBubble(bubbles, new Vector2(bubbleRemoved.X + 1, bubbleRemoved.Y + 1), bubbles[(int)bubbleRemoved.Y, (int)bubbleRemoved.X].bubbleTexture); // if this is odd line will check  down right bubble
            }
        }

        public void CheckFloating(Bubble[,] bubbles, Vector2 targetCluster)
        {
            if ((targetCluster.Y > 11 || targetCluster.Y < 0) || (targetCluster.X < 0 || targetCluster.X > 9) || bubbles[(int)targetCluster.Y, (int)targetCluster.X] == null  || clusterBubble.Contains(targetCluster))
            {
                return;
            }

            if (!clusterBubble.Contains(targetCluster))
            {
                clusterBubble.Add(targetCluster);
            }

            if (!isRemovable)
            {
                CheckFloating(bubbles, new Vector2(targetCluster.X, targetCluster.Y - 1)); // check up bubble
                CheckFloating(bubbles, new Vector2(targetCluster.X + 1, targetCluster.Y)); // check right bubble
                CheckFloating(bubbles, new Vector2(targetCluster.X - 1, targetCluster.Y)); // check left bubble
                CheckFloating(bubbles, new Vector2(targetCluster.X, targetCluster.Y + 1)); // check down bubble

                if (bubbles[(int)targetCluster.Y, (int)targetCluster.X].isEven)
                {
                    CheckFloating(bubbles, new Vector2(targetCluster.X - 1, targetCluster.Y - 1)); //if this is Even line will check up left bubble
                    CheckFloating(bubbles, new Vector2(targetCluster.X - 1, targetCluster.Y + 1)); // if this is Even line will check down left bubble
                }
                else
                {
                    CheckFloating(bubbles, new Vector2(targetCluster.X + 1, targetCluster.Y - 1)); //if this is odd line will check up right bubble
                    CheckFloating(bubbles, new Vector2(targetCluster.X + 1, targetCluster.Y + 1)); // if this is odd line will check  down right bubble
                }
            }

            if (targetCluster.Y == 0)
            {
                isRemovable = true;
            }
        }

        public override void Draw(SpriteBatch _spriteBatch)
        {
            //draw(texture,position,reactangle,color,angle,origin,scale,effect,orderInlayer) buble at Position and origin of Image is middle bottom of image
            _spriteBatch.Draw(bubbleTexture, Position, null, Color.White, Angle, new Vector2(bubbleTexture.Width / 2, bubbleTexture.Height/2), 1f, SpriteEffects.None, 0f);
        }

        public Texture2D RandomBubble()
        {
            int randNum = random.Next(allTexture.Length);
            return allTexture[randNum];
        }
    }
}
