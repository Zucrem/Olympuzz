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
    class Stage3Screen : PlayScreen
    {
        //song and sfx
        protected Song zeusTheme;
        public override void Initial()
        {
            //all button
            pauseButton = new Button(pauseButtonPic, new Vector2(86, 60), new Vector2(148, 50));//create button object on playscreen
            base.Initial();
        }
        public override void LoadContent()
        {
            base.LoadContent();

            //stage map
            stageBGPic = content.Load<Texture2D>("Stage3/ZeusStage");
            boardBGPic = content.Load<Texture2D>("Stage3/Board3");

            //all button on playscreen
            pauseButtonPic = content.Load<Texture2D>("Stage3/Pause3");

            //bg music
            zeusTheme = content.Load<Song>("Sounds/ZeusTheme");
            MediaPlayer.Play(zeusTheme);

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
