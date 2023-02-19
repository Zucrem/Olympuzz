using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Olympuzz.GameObjects;
using System.Diagnostics;

namespace Olympuzz
{
    public class OlympuzzMain : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Vector2 position , size;
        private Texture2D reddot;
        private Button b;

        private Song BGM;
        private SpriteFont Arial;

        public OlympuzzMain()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = (int)Singleton.Instance.Dimensions.X;
            graphics.PreferredBackBufferHeight = (int)Singleton.Instance.Dimensions.Y;
            graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
            Window.Position = new Point((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2) - (graphics.PreferredBackBufferWidth / 2), (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2) - (graphics.PreferredBackBufferHeight / 2));
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            //song
            BGM = Content.Load<Song>("Audios/Spirit_of_the_Dead");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = Singleton.Instance.bgMusicVolume;
            MediaPlayer.Play(BGM);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            position = new Vector2(100,100);
            reddot = Content.Load<Texture2D>("Basic_red_dot");
            size = new Vector2(100,100);
            b = new Button(reddot, position,size);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ScreenManager.Instance.LoadContent(Content);
            Arial = Content.Load<SpriteFont>("Fonts/Arial");
        }

        protected override void UnloadContent()
        {
            ScreenManager.Instance.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            ScreenManager.Instance.Update(gameTime);
            Singleton.Instance.IsFullScreen = graphics.IsFullScreen;
            MediaPlayer.Volume = Singleton.Instance.BGM_MasterVolume;

            if (Singleton.Instance.cmdExit)
            {
                Exit();
            }
            if (Singleton.Instance.cmdFullScreen)
            {
                graphics.ToggleFullScreen();
                graphics.ApplyChanges();
                Singleton.Instance.cmdFullScreen = false;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();

            //fade in and out screen
            ScreenManager.Instance.Draw(spriteBatch);
            if (Singleton.Instance.cmdShowFPS)
            {
                float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                FrameCounter.Instance.Update(deltaTime);
                spriteBatch.DrawString(Arial, string.Format("FPS: " + FrameCounter.Instance.AverageFramesPerSecond.ToString("F")), new Vector2(1090, 10), Color.Yellow);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}