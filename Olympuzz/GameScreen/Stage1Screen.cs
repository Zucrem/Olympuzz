using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Olympuzz.GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olympuzz.GameScreen
{
    class Stage1Screen : PlayScreen
    {
        //song and sfx
        protected Song poseidonTheme;
        public override void Initial()
        {
            //all button
            pauseButton = new Button(pauseButtonPic, new Vector2(98, 50), new Vector2(148, 60));//create button object on playscreen
            base.Initial();
        }
        public override void LoadContent()
        {
            base.LoadContent();

            //stage map
            stageBGPic = content.Load<Texture2D>("Stage1/PoseidonStage");
            boardBGPic = content.Load<Texture2D>("Stage1/Board1");
            //all button on playscreen
            pauseButtonPic = content.Load<Texture2D>("Stage1/Pause1");

            //bg music
            poseidonTheme = content.Load<Song>("Sounds/PoseidonTheme");
            MediaPlayer.Play(poseidonTheme);

            Initial();
        }
        public override void UnloadContent()
        {
            base.UnloadContent();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
