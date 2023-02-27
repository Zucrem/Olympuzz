using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Olympuzz.GameObjects
{
    internal class Shooter : _GameObject
    {
        private Random random = new Random();//ใช้สุ่มสีลูกบอล
        private Texture2D[] bubbleTexture;//รูปลูกบอลทั้งหมด
        private Bubble bubble;//ลูกบอลบนหน้าไม้
        private float angle;
        private Texture2D _base;

        //public SoundEffectInstance _deadSFX, _stickSFX;
        public Shooter(Texture2D texture, Texture2D[] bubble , Texture2D _base) : base(texture)
        {
            bubbleTexture = bubble;
            this._base = _base;
            
            Debug.WriteLine(angle);

        }
        
        public override void Update(GameTime gameTime, Bubble[,] bubbles)
        {
            Singleton.Instance.MousePrevious = Singleton.Instance.MouseCurrent;//เก็บสถานะmouseก่อนหน้า
            Singleton.Instance.MouseCurrent = Mouse.GetState();//เก็บสถานะmouseปัจจุบัน

            if (Singleton.Instance.MouseCurrent.Y < 400 && Singleton.Instance.MouseCurrent.X < 900)//mouseต้องสูงกว่าขนาดเท่านี้
            {
                angle = (float)Math.Atan2(Singleton.Instance.MouseCurrent.Y - Singleton.Instance.Dimensions.Y,Singleton.Instance.MouseCurrent.X - (Singleton.Instance.Dimensions.X/2));//มุม = ตำแหน่งที่ยิง - สถานะmouse x y
                //ถ้าไม่ได้ยิง และ กดเม้าซ้าย และเม้าก่อนหน้าปล่อยอยู่
                if (!Singleton.Instance.Shooting && Singleton.Instance.MouseCurrent.LeftButton == ButtonState.Pressed && Singleton.Instance.MousePrevious.LeftButton == ButtonState.Released)
                {
                    //If have GameScreen change this to set bubble Angle and speed only and Don't create bubble in this anymore because GameScreen had already generate Orb for shooting
                    bubble = new Bubble(bubbleTexture)
                    {
                        Name = "Bubble",
                        Position = Position,
                        //deadSFX = _deadSFX,
                        //stickSFX = _stickSFX,
                        IsActive = true,
                        Angle = angle + MathHelper.Pi,
                        speed = -1000,
                    };
                    Singleton.Instance.Shooting = true;
                }
            }//ถ้าหน้าไม้อยู่ในสถานะยิง
            if (Singleton.Instance.Shooting)
                bubble.Update(gameTime, bubbles);
        } 

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_base, Position,null, Color.White, 0, new Vector2(_texture.Width / 2, _texture.Height), 1f, SpriteEffects.None, 0f);
            if (angle == 0)
            {
                spriteBatch.Draw(_texture, Position, null, Color.White, angle, new Vector2(_texture.Width / 2, _texture.Height), 1f, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(_texture, Position, null, Color.White, angle + MathHelper.ToRadians(90f), new Vector2(_texture.Width / 2, _texture.Height), 1f, SpriteEffects.None, 0f); 
            }

            if (Singleton.Instance.Shooting)//ถ้ายังไม่ได้อยู่ในสถานะยิงให้วาดรูปลูกบอล
                bubble.Draw(spriteBatch);
        }

    }
}
