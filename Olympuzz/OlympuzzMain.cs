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
        private Texture2D reddot,shooterTexture, baseTexture;
        private readonly Texture2D[] bubleAllTexture = new Texture2D[5];
        private Button b;
        private Shooter shooter;

        //private Song BGM;
        //private SpriteFont Arial;

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
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            ScreenManager.Instance.LoadContent(Content);
        }
         
        protected override void UnloadContent()
        {
            ScreenManager.Instance.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            ScreenManager.Instance.Update(gameTime);
            Singleton.Instance.IsFullScreen = _graphics.IsFullScreen;

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (Singleton.Instance.cmdExit)
            {
                Exit();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.White);
            _spriteBatch.Begin();
            //fade in and out screen
            ScreenManager.Instance.Draw(_spriteBatch);
            _spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}