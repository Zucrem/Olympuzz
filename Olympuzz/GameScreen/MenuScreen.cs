using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Audio;
using Olympuzz.Managers;
using Olympuzz.GameObjects;

namespace Olympuzz.GameScreen
{
    class MenuScreen : _GameScreen
    {
        
        private Color _Color = new Color(250, 250, 250, 0);
        private Texture2D backgroundPic, blackScreenPic;//background
        //all picture
        private Texture2D startPic, howToPlayPic, settingPic, exitPic, checkBoxBGM, checkBoxSFX, apply, back, ArrowLeftBGM, ArrowRightBGM, ArrowLeftSFX, ArrowRightSFX;

        //all button
        Button startButton, howToPlayButton, settingButton, exitButton, backButton, applySettingButton, arrowLbgmButton, arrowRbgmButton, arrowLsfxButton, arrowRsfxButton;

        //private SpriteFont Arial, Arcanista, KM;
        private Vector2 fontSize;


        //check if go next page or fade finish
        private bool fadeFinish = false;
        private bool mainScreen = true;
        private bool showSetting = false, showHowToPlay = false;

        //timer and alpha for fade out screen
        private float _timer = 0.0f;
        private float timerPerUpdate = 0.03f;
        private int alpha = 255;

        //ตัวแปรของหน้า Option
        private int masterBGM  = Singleton.Instance.bgMusicVolume;
        private int masterSFX = Convert.ToInt32(Singleton.Instance.soundMasterVolume * 100);



        public void Initial()
        {
            //main menu button
            startButton = new Button(startPic, new Vector2(490, 389), new Vector2(300, 70));
            howToPlayButton = new Button(howToPlayPic, new Vector2(490, 490), new Vector2(300, 70));
            settingButton = new Button(settingPic, new Vector2(600, 609), new Vector2(300, 70));
            //settingButton = new Button(settingPic, new Vector2(490, 609), new Vector2(300, 70));
            exitButton = new Button(exitPic, new Vector2(700, 100), new Vector2(300, 70));

            //setting and how2play button
            backButton = new Button(back, new Vector2(490, 609), new Vector2(300, 70));
            arrowLbgmButton = new Button(ArrowLeftBGM, new Vector2(700, 240), new Vector2(70, 50));
            arrowRbgmButton = new Button(ArrowRightBGM, new Vector2(900, 240), new Vector2(70, 50));
            arrowLsfxButton = new Button(ArrowLeftSFX, new Vector2(700, 315), new Vector2(70, 50));
            arrowRsfxButton = new Button(ArrowRightSFX, new Vector2(900, 315), new Vector2(70, 50));
            applySettingButton = new Button(apply, new Vector2(745, 510), new Vector2(300, 70));
        }
        public override void LoadContent()
        {
            base.LoadContent();
            // Texture2D รูปต่างๆ
            //allbackground
            backgroundPic = content.Load<Texture2D>("gud room");
            blackScreenPic = content.Load<Texture2D>("blackScreen");

            //all button
            //mainscreen button
            startPic = content.Load<Texture2D>("Water");
            howToPlayPic = content.Load<Texture2D>("Water");
            settingPic = content.Load<Texture2D>("Water");
            exitPic = content.Load<Texture2D>("Water");
            //setting and how2play button
            /*checkBoxBGM = content.Load<Texture2D>("Fire");
            checkBoxSFX = content.Load<Texture2D>("Fire");*/
            apply = content.Load<Texture2D>("Fire");
            back = content.Load<Texture2D>("Fire");
            ArrowLeftBGM = content.Load<Texture2D>("Wind");
            ArrowRightBGM = content.Load<Texture2D>("Earth");
            ArrowLeftSFX = content.Load<Texture2D>("Wind");
            ArrowRightSFX = content.Load<Texture2D>("Earth");
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
                if (startButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                {
                    ScreenManager.Instance.LoadScreen(ScreenManager.GameScreenName.PlayScreen);
                    mainScreen = false;
                }
                // Click setting
                if (settingButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                {
                    showSetting = true;
                    mainScreen = false;
                }
                // Click How to Play
                if (howToPlayButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                {
                    showHowToPlay = true;
                    mainScreen = false;
                }
                // Click Exit
                if (exitButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                {
                    Singleton.Instance.cmdExit = true;
                }
            }
            else
            {
                // Click Back
                if (backButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                {
                    mainScreen = true;
                    showHowToPlay = false;
                    showSetting = false;
                    //soundClickButton.Play();
                }
                else if (showSetting)
                {

                    // Click Arrow BGM Left and RIght
                    if (applySettingButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        if (masterBGM > 0)
                        {
                            masterBGM -= 5;
                        }
                    }
                    else if (applySettingButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        if (masterBGM < 100)
                        {
                            masterBGM += 5;
                        }
                    }
                    // Click Arrow SFX Left and RIght
                    if (applySettingButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        if (masterSFX > 0)
                        {
                            masterSFX -= 5;
                        }
                    }
                    else if (applySettingButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        if (masterSFX < 100)
                        {
                            masterSFX += 5;
                        }
                    }
                    // Apply setting to Game (this is real changing to apply to singleton)
                    if (applySettingButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        /*Singleton.Instance.bgMusicVolume = masterBGM ;
                        Singleton.Instance.soundMasterVolume = masterSFX / 100f;*/
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
            //Draw UI in Main Menu
            if (mainScreen)
            {
                spriteBatch.Draw(backgroundPic, Vector2.Zero, Color.White);
                startButton.Draw(spriteBatch);
                howToPlayButton.Draw(spriteBatch);
                settingButton.Draw(spriteBatch);
                exitButton.Draw(spriteBatch);
            }
            // Draw UI when is NOT MainMenu
            else
            {
                spriteBatch.Draw(blackScreenPic, Vector2.Zero, new Color(255, 255, 255, 210));
                backButton.Draw(spriteBatch);
                // Draw Option Screen
                if (showSetting)
                {
                    /*fontSize = KM.MeasureString("Setting");
                    spriteBatch.DrawString(KM, "Setting", new Vector2(Singleton.Instance.Dimensions.X / 2 - fontSize.X / 2, 125), Color.White);

                    //BGM
                    spriteBatch.DrawString(Arcanista, "BGM Volume", new Vector2(300, 250), Color.White);
                    spriteBatch.Draw(Arrow, new Vector2(700, 240), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0f);
                    spriteBatch.DrawString(Arcanista, masterBGM .ToString(), new Vector2(800, 250), Color.White);
                    spriteBatch.Draw(Arrow, new Vector2(900, 240), Color.White);

                    //SFX
                    spriteBatch.DrawString(Arcanista, "SFX Volume", new Vector2(300, 325), Color.White);
                    spriteBatch.Draw(Arrow, new Vector2(700, 315), null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0f);
                    spriteBatch.DrawString(Arcanista, masterSFX.ToString(), new Vector2(800, 325), Color.White);
                    spriteBatch.Draw(Arrow, new Vector2(900, 315), Color.White);
                    */

                    //BGM
                    arrowLbgmButton.Draw(spriteBatch);
                    arrowRbgmButton.Draw(spriteBatch);

                    //SFX
                    arrowLsfxButton.Draw(spriteBatch);
                    arrowRsfxButton.Draw(spriteBatch);

                    applySettingButton.Draw(spriteBatch);
                }
                // Draw About Screen
                else if (showHowToPlay)
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
