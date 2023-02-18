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
        public float speed; // speed of bubble
        public float Angle; // angle of cannon use to set where bubble will fly to

        public Bubble(Texture2D texture) : base(texture)
        {
        }

        public override void Update(GameTime gameTime, Bubble[,] gameObject)
        {

            if (IsActive)
            {
                Velocity.X = (float)Math.Cos(Angle) * speed; //direction of bubble to go in axis x
                Velocity.Y = (float)Math.Sin(Angle) * speed; // direction of bubble to go in axis y
                Position += Velocity * gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond; // position of bubble that will increase to direction that canon point to
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
