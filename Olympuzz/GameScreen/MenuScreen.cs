using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Audio;
using Olympuzz.Managers;

namespace Olympuzz.GameScreen
{
    class MenuScreen : _GameScreen
    {
        
        private Color _Color = new Color(250, 250, 250, 0);
        private Texture2D backgroundPic, blackScreenPic;//background
        private Texture2D startPic, howToPlayPic, settingPic, exitPic, checkBoxYes, checkBoxNo, apply, back, Arrow;
        //private SpriteFont Arial, Arcanista, KM;
        private Vector2 fontSize;

        private SoundEffectInstance soundClickButton, soundEnterGame, soundSelectButton;

        private bool fadeFinish = false;
        private bool showSetting = false, showHowToPlay = false;
        private bool mhStart = false, mhOption = false, mhAbout = false, mhExit = false, mhBack = false, mhApply;
        private bool mhsStart = false, mhsOption = false, mhsAbout = false, mhsExit = false;
        private bool mainScreen = true;

        private float _timer = 0.0f;
        private float timerPerUpdate = 0.03f;
        private int alpha = 255;

        //ตัวแปรของหน้า Option
        private bool FullScreen = Singleton.Instance.IsFullScreen;
        private int masterBGmusic = Singleton.Instance.bgMusicVolume;
        private int masterSFX = Convert.ToInt32(Singleton.Instance.soundMasterVolume * 100);

        public void Initial()
        {

        }
        public override void LoadContent()
        {
            base.LoadContent();
            // Texture2D รูปต่างๆ
            backgroundPic = content.Load<Texture2D>("gud room");
            blackScreenPic = content.Load<Texture2D>("blackScreen");
            startPic = content.Load<Texture2D>("Fire");
            howToPlayPic = content.Load<Texture2D>("Fire");
            settingPic = content.Load<Texture2D>("Fire");
            exitPic = content.Load<Texture2D>("Fire");
            checkBoxYes = content.Load<Texture2D>("Fire");
            checkBoxNo = content.Load<Texture2D>("Fire");
            apply = content.Load<Texture2D>("Fire");
            back = content.Load<Texture2D>("Fire");
            Arrow = content.Load<Texture2D>("Fire");
            // Fonts
            /*Arial = content.Load<SpriteFont>("Fonts/Arial");
            Arcanista = content.Load<SpriteFont>("Fonts/Arcanista");
            KM = content.Load<SpriteFont>("Fonts/KH-Metropolis");*/
            // Sounds
            /*soundClickButton = content.Load<SoundEffect>("Audios/UI_SoundPack8_Error_v1").CreateInstance();
            soundEnterGame = content.Load<SoundEffect>("Audios/transition t07 two-step 007").CreateInstance();
            soundSelectButton = content.Load<SoundEffect>("Audios/UI_SoundPack11_Select_v14").CreateInstance();*/
            // Call Init
            Initial();
        }
        public override void UnloadContent()
        {
            base.UnloadContent();
        }
        public override void Update(GameTime gameTime)
        {
            //click sound
            /*soundSelectButton.Volume = Singleton.Instance.soundMasterVolume;
            soundClickButton.Volume = Singleton.Instance.soundMasterVolume;
            soundEnterGame.Volume = Singleton.Instance.soundMasterVolume;*/

            Singleton.Instance.MousePrevious = Singleton.Instance.MouseCurrent;
            Singleton.Instance.MouseCurrent = Mouse.GetState();
            if (mainScreen)
            {
                // Click start game
                if ((Singleton.Instance.MouseCurrent.X > 517 && Singleton.Instance.MouseCurrent.Y > 166) && (Singleton.Instance.MouseCurrent.X < 780 && Singleton.Instance.MouseCurrent.Y < 514))
                {
                    mhStart = true;
                    if (!mhsStart)
                    {
                        //soundSelectButton.Play();
                        mhsStart = true;
                    }
                    if (Singleton.Instance.MouseCurrent.LeftButton == ButtonState.Pressed && Singleton.Instance.MousePrevious.LeftButton == ButtonState.Released)
                    {
                        //soundEnterGame.Play();
                        ScreenManager.Instance.LoadScreen(ScreenManager.GameScreenName.PlayScreen);
                    }
                }
                else
                {
                    mhStart = false;
                    mhsStart = false;
                }
                // Click setting
                if ((Singleton.Instance.MouseCurrent.X > 93 && Singleton.Instance.MouseCurrent.Y > 161) && (Singleton.Instance.MouseCurrent.X < 256 && Singleton.Instance.MouseCurrent.Y < 674))
                {
                    mhOption = true;
                    if (!mhsOption)
                    {
                        //soundSelectButton.Play();
                        mhsOption = true;
                    }
                    if (Singleton.Instance.MouseCurrent.LeftButton == ButtonState.Pressed && Singleton.Instance.MousePrevious.LeftButton == ButtonState.Released)
                    {
                        showSetting = true;
                        mainScreen = false;
                        //soundClickButton.Play();
                    }
                }
                else
                {
                    mhsOption = false;
                    mhOption = false;
                }
                // Click About
                if ((Singleton.Instance.MouseCurrent.X > 1044 && Singleton.Instance.MouseCurrent.Y > 178) && (Singleton.Instance.MouseCurrent.X < 1218 && Singleton.Instance.MouseCurrent.Y < 694))
                {
                    mhAbout = true;
                    if (!mhsAbout)
                    {
                        //soundSelectButton.Play();
                        mhsAbout = true;
                    }
                    if (Singleton.Instance.MouseCurrent.LeftButton == ButtonState.Pressed && Singleton.Instance.MousePrevious.LeftButton == ButtonState.Released)
                    {
                        showHowToPlay = true;
                        mainScreen = false;
                        //soundClickButton.Play();
                    }
                }
                else
                {
                    mhsAbout = false;
                    mhAbout = false;
                }
                // Click Exit
                if ((Singleton.Instance.MouseCurrent.X > 420 && Singleton.Instance.MouseCurrent.Y > 580) && (Singleton.Instance.MouseCurrent.X < 831 && Singleton.Instance.MouseCurrent.Y < 692))
                {
                    mhExit = true;
                    if (!mhsExit)
                    {
                        //soundSelectButton.Play();
                        mhsExit = true;
                    }
                    if (Singleton.Instance.MouseCurrent.LeftButton == ButtonState.Pressed && Singleton.Instance.MousePrevious.LeftButton == ButtonState.Released)
                    {
                        //soundEnterGame.Play();
                        Singleton.Instance.cmdExit = true;
                    }
                }
                else
                {
                    mhExit = false;
                    mhsExit = false;
                }
            }
            else
            {
                // Click Back
                if ((Singleton.Instance.MouseCurrent.X > (1230 - back.Width) && Singleton.Instance.MouseCurrent.Y > 50) && (Singleton.Instance.MouseCurrent.X < 1230 && Singleton.Instance.MouseCurrent.Y < (50 + back.Height)))
                {
                    mhBack = true;
                    if (Singleton.Instance.MouseCurrent.LeftButton == ButtonState.Pressed && Singleton.Instance.MousePrevious.LeftButton == ButtonState.Released)
                    {
                        mainScreen = true;
                        showHowToPlay = false;
                        showSetting = false;
                        //soundClickButton.Play();
                    }
                }
                else
                {
                    mhBack = false;
                }
                if (showSetting)
                {

                    // Click Arrow BGM
                    if ((Singleton.Instance.MouseCurrent.X > 700 && Singleton.Instance.MouseCurrent.Y > 240) && (Singleton.Instance.MouseCurrent.X < (700 + Arrow.Width) && Singleton.Instance.MouseCurrent.Y < (240 + Arrow.Height)))
                    {
                        if (Singleton.Instance.MouseCurrent.LeftButton == ButtonState.Pressed && Singleton.Instance.MousePrevious.LeftButton == ButtonState.Released)
                        {
                            if (masterBGmusic > 0) masterBGmusic -= 5;
                            soundSelectButton.Play();
                        }
                    }
                    else if ((Singleton.Instance.MouseCurrent.X > 900 && Singleton.Instance.MouseCurrent.Y > 240) && (Singleton.Instance.MouseCurrent.X < (900 + Arrow.Width) && Singleton.Instance.MouseCurrent.Y < (240 + Arrow.Height)))
                    {
                        if (Singleton.Instance.MouseCurrent.LeftButton == ButtonState.Pressed && Singleton.Instance.MousePrevious.LeftButton == ButtonState.Released)
                        {
                            if (masterBGmusic < 100) masterBGmusic += 5;
                        }
                    }
                    // Click Arrow SFX
                    if ((Singleton.Instance.MouseCurrent.X > 700 && Singleton.Instance.MouseCurrent.Y > 315) && (Singleton.Instance.MouseCurrent.X < (700 + Arrow.Width) && Singleton.Instance.MouseCurrent.Y < (315 + Arrow.Height)))
                    {
                        if (Singleton.Instance.MouseCurrent.LeftButton == ButtonState.Pressed && Singleton.Instance.MousePrevious.LeftButton == ButtonState.Released)
                        {
                            if (masterSFX > 0) masterSFX -= 5;
                        }
                    }
                    else if ((Singleton.Instance.MouseCurrent.X > 900 && Singleton.Instance.MouseCurrent.Y > 315) && (Singleton.Instance.MouseCurrent.X < (900 + Arrow.Width) && Singleton.Instance.MouseCurrent.Y < (315 + Arrow.Height)))
                    {
                        if (Singleton.Instance.MouseCurrent.LeftButton == ButtonState.Pressed && Singleton.Instance.MousePrevious.LeftButton == ButtonState.Released)
                        {
                            if (masterSFX < 100) masterSFX += 5;
                        }
                    }
                    // Apply setting to Game
                    if ((Singleton.Instance.MouseCurrent.X > (1100 - apply.Width) && Singleton.Instance.MouseCurrent.Y > 625) && (Singleton.Instance.MouseCurrent.X < 1100 && Singleton.Instance.MouseCurrent.Y < (625 + back.Height)))
                    {
                        mhApply = true;
                        if (Singleton.Instance.MouseCurrent.LeftButton == ButtonState.Pressed && Singleton.Instance.MousePrevious.LeftButton == ButtonState.Released)
                        {
                            /*soundClickButton.Play();
                            Singleton.Instance.bgMusicVolume = masterBGmusic;
                            Singleton.Instance.soundMasterVolume = masterSFX / 100f;*/
                        }
                    }
                    else
                    {
                        mhApply = false;
                    }
                }
            }

            // fade out
            if (!fadeFinish)
            {
                _timer += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                if (_timer >= timerPerUpdate)
                {
                    alpha -= 5;
                    _timer -= timerPerUpdate;
                    if (alpha <= 5)
                    {
                        fadeFinish = true;
                    }
                    _Color.A = (byte)alpha;
                }
            }
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundPic, Vector2.Zero, Color.White);
            // Draw mouse onHover button
            if (mhAbout)
            {
                spriteBatch.Draw(howToPlayPic, new Vector2(1047, 366), Color.White);
            }
            if (mhExit)
            {
                spriteBatch.Draw(exitPic, new Vector2(475, 574), Color.White);
            }
            if (mhOption)
            {
                spriteBatch.Draw(settingPic, new Vector2(98, 362), Color.White);
            }
            if (mhStart)
            {
                spriteBatch.Draw(startPic, new Vector2(551, 332), Color.White);
            }
            // Draw UI when is NOT MainMenu
            if (!mainScreen)
            {
                spriteBatch.Draw(blackScreenPic, Vector2.Zero, new Color(255, 255, 255, 210));
                if (mhBack)
                {
                    spriteBatch.Draw(back, new Vector2(1230 - back.Width, 50), Color.OrangeRed);
                }
                else
                {
                    spriteBatch.Draw(back, new Vector2(1230 - back.Width, 50), Color.White);
                }
                // Draw Option Screen
                if (showSetting)
                {
                    /*fontSize = KM.MeasureString("Setting");
                    spriteBatch.DrawString(KM, "Setting", new Vector2(Singleton.Instance.Dimensions.X / 2 - fontSize.X / 2, 125), Color.White);

                    spriteBatch.DrawString(Arcanista, "BGM Volume", new Vector2(300, 250), Color.White);
                    spriteBatch.Draw(Arrow, new Vector2(700, 240), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0f);
                    spriteBatch.DrawString(Arcanista, masterBGmusic.ToString(), new Vector2(800, 250), Color.White);
                    spriteBatch.Draw(Arrow, new Vector2(900, 240), Color.White);

                    spriteBatch.DrawString(Arcanista, "SFX Volume", new Vector2(300, 325), Color.White);
                    spriteBatch.Draw(Arrow, new Vector2(700, 315), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0f);
                    spriteBatch.DrawString(Arcanista, masterSFX.ToString(), new Vector2(800, 325), Color.White);
                    spriteBatch.Draw(Arrow, new Vector2(900, 315), Color.White);
                    */
                    if (mhApply)
                    {
                        spriteBatch.Draw(apply, new Vector2(1100 - back.Width, 625), Color.OrangeRed);
                    }
                    else
                    {
                        spriteBatch.Draw(apply, new Vector2(1100 - back.Width, 625), Color.White);
                    }
                }
                // Draw About Screen
                if (showHowToPlay)
                {
                    /*fontSize = KM.MeasureString("About");
                    spriteBatch.DrawString(KM, "About", new Vector2(Singleton.Instance.Diemensions.X / 2 - fontSize.X / 2, 125), Color.White);

                    spriteBatch.DrawString(Arcanista, "Graphics", new Vector2(200, 250), Color.NavajoWhite);
                    spriteBatch.DrawString(Arcanista, "- We create", new Vector2(160, 350), Color.White);
                    spriteBatch.DrawString(Arcanista, "All Graphics", new Vector2(150, 425), Color.White);

                    spriteBatch.DrawString(Arcanista, "Audios", new Vector2(600, 250), Color.NavajoWhite);
                    spriteBatch.DrawString(Arcanista, "- www.sonniss.com", new Vector2(520, 350), Color.White);
                    spriteBatch.DrawString(Arcanista, "Free Audios Bundle", new Vector2(510, 425), Color.White);

                    spriteBatch.DrawString(Arcanista, "Fonts", new Vector2(1000, 250), Color.NavajoWhite);
                    spriteBatch.DrawString(Arcanista, "- Arial", new Vector2(985, 350), Color.White);
                    spriteBatch.DrawString(Arcanista, "- Arcanista", new Vector2(950, 425), Color.White);
                    spriteBatch.DrawString(Arcanista, "- KH-Metropolis", new Vector2(920, 500), Color.White);*/

                }
            }
            // Draw fade out
            if (!fadeFinish)
            {
                spriteBatch.Draw(blackScreenPic, Vector2.Zero, _Color);
            }
        }
    }
}
