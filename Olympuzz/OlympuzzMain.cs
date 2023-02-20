using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Olympuzz.GameObjects;
using System.Diagnostics;
using Olympuzz.Managers;

namespace Olympuzz
{
    public class OlympuzzMain : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
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
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = (int)Singleton.Instance.Dimensions.X;
            _graphics.PreferredBackBufferHeight = (int)Singleton.Instance.Dimensions.Y;
            _graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
            Window.IsBorderless = true;// make window borderless
            position = new Vector2(100, 100);//use to set position of reddot button
            size = new Vector2(100, 100);//use to set size of reddot button
            
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            //song
            /*BGM = Content.Load<Song>("Audios/Spirit_of_the_Dead");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = Singleton.Instance.bgMusicVolume;
            MediaPlayer.Play(BGM);*/
            
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
                //Position = new Vector2(Singleton.Instance.Dimensions.X / 2 - shooterTexture.Width / 2, 700 - shooterTexture.Height),
                //Position = new Vector2(640, 720),
                color = Color.White,
                IsActive = true,
            };
            
            //ScreenManager.Instance.LoadContent(Content);
            //Arial = Content.Load<SpriteFont>("Fonts/Arial");
         }
         
        /*protected override void UnloadContent()
        {
            ScreenManager.Instance.UnloadContent();
        }*/

        protected override void Update(GameTime gameTime)
        {
            //ScreenManager.Instance.Update(gameTime);
            Singleton.Instance.IsFullScreen = _graphics.IsFullScreen;
            //MediaPlayer.Volume = Singleton.Instance.bgMusicVolume;

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (Singleton.Instance.cmdExit)
            {
                Exit();
            }

            shooter.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            _spriteBatch.Begin();

            /*//fade in and out screen
            ScreenManager.Instance.Draw(_spriteBatch);
            if (Singleton.Instance.cmdShowFPS)
            {
                float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                FrameCounter.Instance.Update(deltaTime);
                _spriteBatch.DrawString(Arial, string.Format("FPS: " + FrameCounter.Instance.AverageFramesPerSecond.ToString("F")), new Vector2(1090, 10), Color.Yellow);
            }*/

            // TODO: Add your drawing code here

            shooter.Draw(_spriteBatch);
            b.Draw(_spriteBatch);
            _spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}