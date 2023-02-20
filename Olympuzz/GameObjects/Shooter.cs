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

        private Vector2 d;

        //public SoundEffectInstance _deadSFX, _stickSFX;
        public Shooter(Texture2D texture, Texture2D[] bubble , Texture2D _base) : base(texture)
        {
            bubbleTexture = bubble;
            this._base = _base;
            Position = new Vector2(960, 500);
            //bubble = new Bubble(bubbleTexture)
            //{
            //    Name = "Bubble",
            //    Position = new Vector2(960,500),
            //    //deadSFX = _deadSFX,
            //    //stickSFX = _stickSFX,
            //    //color = _color,
            //    IsActive = true,
            //    Angle = 10,
            //    speed = 500,
            //};

        }
        
        public override void Update(GameTime gameTime)
        {
            Singleton.Instance.MousePrevious = Singleton.Instance.MouseCurrent;//เก็บสถานะmouseก่อนหน้า
            Singleton.Instance.MouseCurrent = Mouse.GetState();//เก็บสถานะmouseปัจจุบัน

            if (Singleton.Instance.MouseCurrent.Y < 625)//mouseต้องสูงกว่าขนาดเท่านี้
            {
                angle = (float)Math.Atan2(Singleton.Instance.MouseCurrent.Y - 500,Singleton.Instance.MouseCurrent.X - 960);//มุม = ตำแหน่งที่ยิง - สถานะmouse x y
                //ถ้าไม่ได้ยิง และ กดเม้าซ้าย และเม้าก่อนหน้าปล่อยอยู่
                if (!Singleton.Instance.Shooting && Singleton.Instance.MouseCurrent.LeftButton == ButtonState.Pressed && Singleton.Instance.MousePrevious.LeftButton == ButtonState.Released)
                {
                    bubble = new Bubble(bubbleTexture)
                    {
                        Name = "Bubble",
                        //Position = new Vector2(Singleton.Instance.Dimensions.X/2,Singleton.Instance.Dimensions.Y),
                        Position = new Vector2(960, 500),
                        //deadSFX = _deadSFX,
                        //stickSFX = _stickSFX,
                        //color = _color,
                        IsActive = true,
                        Angle = angle + MathHelper.Pi,
                        speed = -1000,
                    };
                    Singleton.Instance.Shooting = true;
                }
            }//ถ้าหน้าไม้อยู่ในสถานะยิง
            if (Singleton.Instance.Shooting)
                bubble.Update(gameTime);
        } 

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(_texture, Position + new Vector2(50, 50), null, Color.White, angle + MathHelper.ToRadians(-90f), new Vector2(50, 50), 1.5f, SpriteEffects.None, 0f);//วาดหน้าไม้
            spriteBatch.Draw(_base, Position,null, Color.White, 0, new Vector2(_texture.Width / 2, _texture.Height), 1f, SpriteEffects.None, 0f);
            //spriteBatch.Draw(_texture, Position, null, Color.White, angle + MathHelper.ToRadians(45f), new Vector2(_texture.Width / 2, _texture.Height), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(_texture, Position, null, Color.White, angle + MathHelper.ToRadians(90f), new Vector2(_texture.Width / 2, _texture.Height), 1f, SpriteEffects.None, 0f);
            if (Singleton.Instance.Shooting)//ถ้ายังไม่ได้อยู่ในสถานะยิงให้วาดรูปลูกบอล
                bubble.Draw(spriteBatch);
                //spriteBatch.Draw(bubbleTexture, new Vector2(Singleton.Instance.Dimensions.X / 2 - bubbleTexture.Width / 2, 700 - bubbleTexture.Height), _color);//ตำแหน่งกลางหน้าจอ - กลางลูกบอล, ตำแหน่งหน้าไม้ - กลางลูกบอล
            //else//ถ้ายิง ให้วาดแค่หน้าไม้
            //    bubble.Draw(spriteBatch);
        }

    }
}
