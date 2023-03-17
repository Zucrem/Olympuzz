using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Olympuzz.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static Olympuzz.Managers.ScreenManager;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Olympuzz.GameScreen
{
    class SplashScreen : _GameScreen
    {
        private Vector2 fontSize;//ขนาด font ที่เอามา
        private Color color; //เพื่อupdate ค่าความโปร่งสี
        private SpriteFont smallfonts, mediumfonts, bigfonts;//กำหนดชื่อ font
        private Texture2D LogoPic, blackScreen;//กำหนด ภาพของหน้า splashscreen
        private int alpha; // ค่าความโปร่งสี
        private int displayIndex; // order of index to display splash screen
        private float timer; // Elapsed time in game 
        private float timePerUpdate; // Will do update function when _timer > _timePerUpdate
        private bool Show; // true will fade in and false will fade out

        //bg and sfx sound
        private Song openningTheme;

        public SplashScreen()
        {
            Show = true;
            timePerUpdate = 0.05f;
            displayIndex = 0;
            alpha = 0;
            color = new Color(255, 255, 255, alpha);
        }
        public override void LoadContent()
        {
            base.LoadContent();
            smallfonts = content.Load<SpriteFont>("Alagard");
            bigfonts = content.Load<SpriteFont>("AlagardBig");
            mediumfonts = content.Load<SpriteFont>("AlagardMedium");
            LogoPic = content.Load<Texture2D>("GameLogo");
            blackScreen = content.Load<Texture2D>("blackScreen");

            //song and sfx
            openningTheme = content.Load<Song>("Sounds/OpenningTheme");
            MediaPlayer.Play(openningTheme);
        }
        public override void UnloadContent() { base.UnloadContent(); }
        public override void Update(GameTime gameTime)
        {
            Singleton.Instance.MouseCurrent = Mouse.GetState();
            if (Keyboard.GetState().IsKeyDown(Keys.Space) || Singleton.Instance.MouseCurrent.LeftButton == ButtonState.Pressed)
            {
                ScreenManager.Instance.LoadScreen(ScreenManager.GameScreenName.MenuScreen);
            }
            // Add elapsed time to _timer
            timer += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
            if (timer >= timePerUpdate)
            {
                if (Show)
                {
                    //fade in
                    alpha += 5;
                    // when fade in finish
                    if (alpha >= 250)
                    {
                        Show = false;

                        // transition screen
                        if (displayIndex == 4)
                        {
                            ScreenManager.Instance.LoadScreen(ScreenManager.GameScreenName.MenuScreen);
                        }
                    }
                }
                else
                {
                    // fade out
                    alpha -= 20;
                    // whene fade out finish
                    if (alpha <= 0)
                    {
                        Show = true;
                        // Change display index and set next display
                        displayIndex++;
                        //change 
                        if (displayIndex == 0)
                        {
                            color = Color.Wheat;
                            timePerUpdate -= 0.015f;
                        }
                        else if (displayIndex == 1)
                        {
                            timePerUpdate += 0.03f;
                            color = Color.SaddleBrown;
                        }
                        else if (displayIndex == 2)
                        {
                            color = Color.Wheat;
                            timePerUpdate -= 0.015f;
                        }
                        else if (displayIndex == 3)
                        {
                            timePerUpdate += 0.03f;
                            color = Color.SaddleBrown;
                        }
                        else if (displayIndex == 4)
                        {
                            timePerUpdate -= 0.035f;
                        }
                    }
                }
                timer -= timePerUpdate;
                color.A = (byte)alpha;
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            switch (displayIndex)
            {
                case 0:
                    fontSize = smallfonts.MeasureString("Press spacebar to skip");
                    spriteBatch.DrawString(smallfonts, "Press spacebar to skip", new Vector2((Singleton.Instance.Dimensions.X - fontSize.X), (Singleton.Instance.Dimensions.Y - fontSize.Y)), color);
                    break;
                case 1:
                    fontSize = smallfonts.MeasureString("Teletubbie's Group Present");
                    spriteBatch.DrawString(smallfonts, "Teletubbie's Group Present", new Vector2((Singleton.Instance.Dimensions.X - fontSize.X) / 2, 450), color);
                    break;
                case 2:
                    spriteBatch.Draw(LogoPic, new Vector2((Singleton.Instance.Dimensions.X - LogoPic.Width) / 2, 200), color);
                    break;
                case 3:
                    fontSize = mediumfonts.MeasureString("The Best Bubble Shooter Game Ever");
                    spriteBatch.DrawString(mediumfonts, "The Best Bubble Shooter Game Ever", new Vector2((Singleton.Instance.Dimensions.X - fontSize.X) / 2, 530), color);
                    break;
                case 4:
                    spriteBatch.Draw(blackScreen, Vector2.Zero, color);
                    break;
            }
        }
    }
}
