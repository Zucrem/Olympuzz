using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Olympuzz.GameObjects;
using System.Diagnostics;

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

        public OlympuzzMain()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();
            Window.IsBorderless = true;
            position = new Vector2(100, 100);
            size = new Vector2(100, 100);


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
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            if (b.IsClicked(Mouse.GetState(),gameTime)) 
            {
                Debug.WriteLine("GG");
            }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            shooter.Draw(_spriteBatch);
            b.Draw(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}