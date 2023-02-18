using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olympuzz.GameObjects
{
    internal class Shooter : _GameObject
    {
        private Random random = new Random();//ใช้สุ่มสีลูกบอล
        private Texture2D bubbleTexture;//รูปลูกบอล
        private Bubble BubbleOnShooter;//ลูกบอลบนหน้าไม้
        private Color _color;
        private float angle;

        public SoundEffectInstance _deadSFX, _stickSFX;
        public Shooter(Texture2D texture, Texture2D bubble) : base(texture)
        {
            bubbleTexture = bubble;
            _color = GetRandomColor();
        }

        public override void Update(GameTime gameTime, Bubble[,] gameObjects)
        {
            Singleton.Instance.MousePrevious = Singleton.Instance.MouseCurrent;//เก็บสถานะmouseก่อนหน้า
            Singleton.Instance.MouseCurrent = Mouse.GetState();//เก็บสถานะmouseปัจจุบัน
            if (Singleton.Instance.MouseCurrent.Y < 625)//mouseต้องสูงกว่าขนาดเท่านี้
            {
                angle = (float)Math.Atan2((Position.Y + _texture.Height / 2) - Singleton.Instance.MouseCurrent.Y, (Position.X + _texture.Width / 2) - Singleton.Instance.MouseCurrent.X);//มุม = ตำแหน่งที่ยิง - สถานะmouse x y
                //ถ้าไม่ได้ยิง และ กดเม้าซ้าย และเม้าก่อนหน้าปล่อยอยู่
                if (!Singleton.Instance.Shooting && Singleton.Instance.MouseCurrent.LeftButton == ButtonState.Pressed && Singleton.Instance.MousePrevious.LeftButton == ButtonState.Released)
                {
                    BubbleOnShooter = new Bubble(bubbleTexture)
                    {
                        Name = "Bubble",
                        Position = new Vector2(Singleton.Instance.Dimensions.X / 2 - bubbleTexture.Width / 2, 700 - bubbleTexture.Height),
                        //deadSFX = _deadSFX,
                        //stickSFX = _stickSFX,
                        color = _color,
                        IsActive = true,
                        Angle = angle + MathHelper.Pi
                        //Speed = 1000,
                    };
                    _color = GetRandomColor();
                    Singleton.Instance.Shooting = true;
                }
            }//ถ้าหน้าไม้อยู่ในสถานะยิง
            if (Singleton.Instance.Shooting)
                BubbleOnShooter.Update(gameTime, gameObjects);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Position + new Vector2(50, 50), null, Color.White, angle + MathHelper.ToRadians(-90f), new Vector2(50, 50), 1.5f, SpriteEffects.None, 0f);//วาดหน้าไม้
            if (!Singleton.Instance.Shooting)//ถ้ายังไม่ได้อยู่ในสถานะยิงให้วาดรูปลูกบอล
                spriteBatch.Draw(bubbleTexture, new Vector2(Singleton.Instance.Dimensions.X / 2 - bubbleTexture.Width / 2, 700 - bubbleTexture.Height), _color);//ตำแหน่งกลางหน้าจอ - กลางลูกบอล, ตำแหน่งหน้าไม้ - กลางลูกบอล
            else//ถ้ายิง ให้วาดแค่หน้าไม้
                BubbleOnShooter.Draw(spriteBatch);
        }

        //method สุ่มสี
        public Color GetRandomColor()
        {
            //ไม่มีลูก
            Color _color = Color.Black;
            switch (random.Next(0, 6))
            {
                case 0:
                    _color = Color.White;
                    break;
                case 1:
                    _color = Color.Blue;
                    break;
            }
            return _color;
        }
    }
}
