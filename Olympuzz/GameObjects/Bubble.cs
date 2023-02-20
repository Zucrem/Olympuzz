using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olympuzz.GameObjects
{
    public class Bubble : _GameObject
    {
        public Random random = new Random();
        public float speed; // speed of bubble
        public float Angle; // angle of cannon use to set where bubble will fly to
        Texture2D bubbleTexture;
        Texture2D[] allTexture;

        public Bubble(Texture2D[] allTexture) : base (allTexture)
        {
            this.allTexture = allTexture;
            bubbleTexture = RandomBubble();
        }

        public override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                Velocity.X = (float)Math.Cos(Angle) * speed; //direction of bubble to go in axis x
                Velocity.Y = (float)Math.Sin(Angle) * speed; // direction of bubble to go in axis y
                Position += Velocity * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond; // position of bubble that will increase to direction that canon point to

                if (Position.Y < 200) speed = 1000;
                else if (Position.Y > 600) speed = -1000;
            }
        }

        public override void Draw(SpriteBatch _spriteBatch)
        {
            //_spriteBatch.Draw(bubbleTexture,Position,Color.White);
            _spriteBatch.Draw(bubbleTexture, Position, null, Color.White, 0, new Vector2(bubbleTexture.Width / 2, bubbleTexture.Height), 1f, SpriteEffects.None, 0f);

        }

        public Texture2D RandomBubble()
        {
            int randNum = random.Next(allTexture.Length);
            return allTexture[randNum];
        }
    }
}
