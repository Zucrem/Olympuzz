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
    class Stage2Screen : PlayScreen
    {
        //song and sfx
        protected Song hadesTheme;

        public override void Initial()
        {
            //all button
            pauseButton = new Button(pauseButtonPic, new Vector2(89, 50), new Vector2(148, 60));//create button object on playscreen
            base.Initial();
        }
        public override void LoadContent()
        {
            base.LoadContent();

            //stage map
            stageBGPic = content.Load<Texture2D>("Stage2/HadesStage");
            boardBGPic = content.Load<Texture2D>("Stage2/Board2");
            //all button on playscreen
            pauseButtonPic = content.Load<Texture2D>("Stage2/Pause2");

            //bg music
            hadesTheme = content.Load<Song>("Sounds/HadesTheme");
            MediaPlayer.Play(hadesTheme);

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
