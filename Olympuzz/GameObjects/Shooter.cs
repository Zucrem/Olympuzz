using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Olympuzz.GameScreen;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Olympuzz.GameObjects
{
    internal class Shooter : _GameObject
    {
        private Random random = new Random();//ใช้สุ่มสีลูกบอล
        private static Texture2D[] bubbleTexture;//รูปลูกบอลทั้งหมด
        public static Bubble bubble, bubbleNext;//ลูกบอลบนหน้าไม้
        public static bool canRotate = false;
        private float angle;
        private Texture2D _base;

        private Vector2 rotationPoint;
        private Vector2 centerRotate;

        //public SoundEffectInstance _deadSFX, _stickSFX;
        public Shooter(Texture2D texture, Texture2D[] bubble , Texture2D _base) : base(texture)
        {
            bubbleTexture = bubble;
            this._base = _base;

            startGenBubble();
        }
        
        public Bubble GetBubble()
        {
            return bubble;
        }

        public override void Update(GameTime gameTime, Bubble[,] bubbles)
        {

            rotationPoint = new Vector2(583, 702);

            if (IsActive)
            {
                if (Singleton.Instance.MouseCurrent.Y < 702 && Singleton.Instance.MouseCurrent.Y > 52 && Singleton.Instance.MouseCurrent.X > 333 && Singleton.Instance.MouseCurrent.X < 837)//mouseต้องสูงกว่าขนาดเท่านี้
                {

                    angle = (float)Math.Atan2(Singleton.Instance.MouseCurrent.Y - rotationPoint.Y, Singleton.Instance.MouseCurrent.X - rotationPoint.X);//มุม = ตำแหน่งที่ยิง - สถานะmouse x y
                                                                                                                                                        //ถ้าไม่ได้ยิง และ กดเม้าซ้าย และเม้าก่อนหน้าปล่อยอยู่
                    if (!Singleton.Instance.Shooting && Singleton.Instance.MouseCurrent.LeftButton == ButtonState.Pressed && Singleton.Instance.MousePrevious.LeftButton == ButtonState.Released)
                    {
                        //If have GameScreen change this to set bubble Angle and speed only and Don't create bubble in this anymore because GameScreen had already generate Orb for shooting
                        bubble.IsActive = true;
                        bubble.Angle = angle + MathHelper.Pi;
                        Singleton.Instance.Shooting = true;
                    }
                    if (!Singleton.Instance.Shooting && !bubble.IsActive && !canRotate)
                    {
                        centerRotate = new Vector2(rotationPoint.X - Singleton.Instance.MouseCurrent.X, rotationPoint.Y - Singleton.Instance.MouseCurrent.Y);
                        centerRotate.Normalize();
                        bubble.Position = rotationPoint - (centerRotate * 50);
                        bubble.Angle = (float)Math.Atan2(Singleton.Instance.MouseCurrent.Y - rotationPoint.Y, Singleton.Instance.MouseCurrent.X - rotationPoint.X) + MathHelper.ToRadians(90f);
                    }
                }//ถ้าหน้าไม้อยู่ในสถานะยิง
                if (Singleton.Instance.Shooting)
                {
                    //bubble.Angle = angle + MathHelper.Pi;
                    bubble.Update(gameTime, bubbles);
                }
                    
            }
        }

        private void startGenBubble()
        {
            bubbleNext = new Bubble(bubbleTexture)
            {
                Name = "Bubble",
                Position = new Vector2(483, 645),
                //deadSFX = _deadSFX,
                //stickSFX = _stickSFX,
                IsActive = false,
            };

            nextBubble();

        }

        public static void nextBubble()
        {
            bubble = bubbleNext;
            bubble.Position = new Vector2(583, 645);
            canRotate = false;
            

            bubbleNext = new Bubble(bubbleTexture)
            {
                Name = "Bubble",
                Position = new Vector2(480, 678),
                //deadSFX = _deadSFX,
                //stickSFX = _stickSFX,
                IsActive = false,
            };
        }
        

        public void Draw(SpriteBatch spriteBatch , bool isBallHolderDie)
        {
            spriteBatch.Draw(_base, Position, null, Color.White, 0, new Vector2(_texture.Width / 2, _texture.Height), 1f, SpriteEffects.None, 0f);

            if (!isBallHolderDie)
            {
                bubbleNext.Draw(spriteBatch);
            }

            if (angle == 0)
            {
                spriteBatch.Draw(_texture, Position, null, Color.White, angle, new Vector2(_texture.Width / 2, _texture.Height), 1f, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(_texture, Position, null, Color.White, angle + MathHelper.ToRadians(90f), new Vector2(_texture.Width / 2, _texture.Height), 1f, SpriteEffects.None, 0f); 
            }

            bubble.Draw(spriteBatch);
            
            

            if (Singleton.Instance.Shooting)//ถ้ายังไม่ได้อยู่ในสถานะยิงให้วาดรูปลูกบอล
                bubble.IsActive = true;
        }

    }
}
