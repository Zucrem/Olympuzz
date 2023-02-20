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
        private Texture2D reddot,shooterTexture,bubbleTexture, baseTexture;
        private readonly Texture2D[] bubleAllTexture = new Texture2D[5];
        private Button b;
        private Shooter shooter;
        private Bubble bubble;

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
            Window.IsBorderless = true;
            position = new Vector2(100, 100);
            size = new Vector2(100, 100);
            
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            //song
            BGM = Content.Load<Song>("Audios/Spirit_of_the_Dead");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = Singleton.Instance.bgMusicVolume;
            MediaPlayer.Play(BGM);
            
            // TODO: Add your initialization logic here
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            shooterTexture = Content.Load<Texture2D>("bow only");
            bubleAllTexture[0] = Content.Load<Texture2D>("Earth");
            bubleAllTexture[1] = Content.Load<Texture2D>("Fire");
            bubleAllTexture[2] = Content.Load<Texture2D>("Thunder");
            bubleAllTexture[3] = Content.Load<Texture2D>("Water");
            bubleAllTexture[4] = Content.Load<Texture2D>("Wind");
            baseTexture = Content.Load<Texture2D>("base");

            reddot = Content.Load<Texture2D>("Basic_red_dot");


            b = new Button(reddot, position,size);
            shooter = new Shooter(shooterTexture, bubleAllTexture, baseTexture)
            {
                Name = "Shooter",
                Position = new Vector2(_graphics.PreferredBackBufferWidth/2, _graphics.PreferredBackBufferHeight),
                //Position = new Vector2(640, 720),
                color = Color.White,
                IsActive = true,
            };
            
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

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            shooter.Draw(_spriteBatch);
            b.Draw(_spriteBatch);
            _spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}