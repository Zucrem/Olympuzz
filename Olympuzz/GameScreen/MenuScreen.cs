using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Microsoft.Xna.Framework.Audio;
using Olympuzz.Managers;
using Olympuzz.GameObjects;
using System.Threading;
using System.Timers;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;
using static Olympuzz.Singleton;

namespace Olympuzz.GameScreen
{
    class MenuScreen : _GameScreen
    {
        //color for fade in
        private Color _Color = new Color(250, 250, 250, 0);

        //all picture
        private Texture2D backgroundPic, blackScreenPic, settingScreenPic;//background
        private Texture2D logoPic, startPic, settingPic, exitPic;//mainmenu pic
        private Texture2D arrowLeftBGPic, arrowRightBGPic, arrowLeftSFXPic, arrowRightSFXPic, backPic;//setting pic
        private Texture2D yesPic, noPic;//exit confirmed pic

        //all button
        private Button startButton, settingButton, exitButton;
        private Button arrowLeftBGButton, arrowRightBGButton, arrowLeftSFXButton, arrowRightSFXButton, backButton;
        private Button yesButton, noButton;

        //private SpriteFont Alagan
        private SpriteFont smallfonts, mediumfonts, bigfonts;//กำหนดชื่อ font
        private Vector2 fontSize;//ขนาด font จาก SpriteFont

        //bg and sfx song
        private Song MainmenuTheme;

        //check if go next page or fade finish
        private bool fadeFinish = false;
        private bool mainScreen = true;
        private bool showSetting = false;
        private bool confirmQuit = false;

        //timer and alpha for fade out screen
        private float _timer = 0.0f;
        private float timerPerUpdate = 0.03f;
        private int alpha = 255;

        //ตัวแปรของหน้า Option
        private float masterBGM = Singleton.Instance.bgMusicVolume;
        private float masterSFX = Singleton.Instance.soundMasterVolume;

        public void Initial()
        {
            //main menu button
            startButton = new Button(startPic, new Vector2(293, 293), new Vector2(160, 80));
            settingButton = new Button(settingPic, new Vector2(293, 410), new Vector2(160, 60));
            exitButton = new Button(exitPic, new Vector2(293, 505), new Vector2(100, 45));

            //setting button
            arrowLeftBGButton = new Button(arrowLeftBGPic, new Vector2(525, 320), new Vector2(40, 40));
            arrowRightBGButton = new Button(arrowRightBGPic, new Vector2(705, 320), new Vector2(40, 40));
            arrowLeftSFXButton = new Button(arrowLeftSFXPic, new Vector2(525, 440), new Vector2(40, 40));
            arrowRightSFXButton = new Button(arrowRightSFXPic, new Vector2(705, 440), new Vector2(40, 40));

            backButton = new Button(backPic, new Vector2(980, 570), new Vector2(150, 60));

            //confirm Exit button
            yesButton = new Button(yesPic, new Vector2(315, 420), new Vector2(300, 70));
            noButton = new Button(noPic, new Vector2(685, 420), new Vector2(300, 70));
        }
        public override void LoadContent()
        {
            base.LoadContent();
            // Texture2D รูปต่างๆ
            //allbackground
            logoPic = content.Load<Texture2D>("GameLogo");
            backgroundPic = content.Load<Texture2D>("MenuScreen/MainMenuBG");
            blackScreenPic = content.Load<Texture2D>("blackScreen");
            settingScreenPic = content.Load<Texture2D>("SettingScreen/SettingBG");

            //all pic for button
            //mainscreen pic
            startPic = content.Load<Texture2D>("MenuScreen/PlayButton");
            settingPic = content.Load<Texture2D>("MenuScreen/SettingButton");
            exitPic = content.Load<Texture2D>("MenuScreen/ExitButton");
            //setting pic
            backPic = content.Load<Texture2D>("SettingScreen/DoneButton");

            arrowLeftBGPic = content.Load<Texture2D>("SettingScreen/ArrowButton");
            arrowRightBGPic = content.Load<Texture2D>("SettingScreen/ArrowRButton");
            arrowLeftSFXPic = content.Load<Texture2D>("SettingScreen/ArrowButton");
            arrowRightSFXPic = content.Load<Texture2D>("SettingScreen/ArrowRButton");

            //confirmQuit pic
            yesPic = content.Load<Texture2D>("PlayScreen/Fire");
            noPic = content.Load<Texture2D>("PlayScreen/Wind");

            // Fonts
            smallfonts = content.Load<SpriteFont>("Alagard");
            mediumfonts = content.Load<SpriteFont>("AlagardMedium");
            bigfonts = content.Load<SpriteFont>("AlagardBig");

            //song and sfx
            MainmenuTheme = content.Load<Song>("MenuScreen/MainmenuTheme");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(MainmenuTheme);

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
            MediaPlayer.Volume = Singleton.Instance.bgMusicVolume;

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
                // Click Exit
                if (exitButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                {
                    confirmQuit = true;
                    mainScreen = false;
                }
            }
            else
            {
                if (showSetting)
                {
                    // Click Arrow BGM button
                    switch (Singleton.Instance.bgmState)
                    {
                        case AudioState.MUTE:
                            if (arrowLeftBGButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                            {
                                Singleton.Instance.bgmState = AudioState.FULL;
                                //setSoundStatus();
                            }
                            else if (arrowRightBGButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                            {
                                Singleton.Instance.bgmState = AudioState.MEDIUM;
                                //setSoundStatus();
                            }
                            break;
                        case AudioState.MEDIUM:
                            if (arrowLeftBGButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                            {
                                Singleton.Instance.bgmState = AudioState.MUTE;
                                //setSoundStatus();
                            }
                            else if (arrowRightBGButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                            {
                                Singleton.Instance.bgmState = AudioState.FULL;
                                //setSoundStatus();
                            }
                            break;
                        case AudioState.FULL:
                            if (arrowLeftBGButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                            {
                                Singleton.Instance.bgmState = AudioState.MEDIUM;
                                //setSoundStatus();
                            }
                            else if (arrowRightBGButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                            {
                                Singleton.Instance.bgmState = AudioState.MUTE;
                                //setSoundStatus();
                            }
                            break;
                    }
                    // Click Arrow SFX button
                    switch (Singleton.Instance.sfxState)
                    {
                        case AudioState.MUTE:
                            if (arrowLeftSFXButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                            {
                                Singleton.Instance.sfxState = AudioState.FULL;
                                //setSoundStatus();
                            }
                            else if (arrowRightSFXButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                            {
                                Singleton.Instance.sfxState = AudioState.MEDIUM;
                                //setSoundStatus();
                            }
                            break;
                        case AudioState.MEDIUM:
                            if (arrowLeftSFXButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                            {
                                Singleton.Instance.sfxState = AudioState.MUTE;
                                //setSoundStatus();
                            }
                            else if (arrowRightSFXButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                            {
                                Singleton.Instance.sfxState = AudioState.FULL;
                                //setSoundStatus();
                            }
                            break;
                        case AudioState.FULL:
                            if (arrowLeftSFXButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                            {
                                Singleton.Instance.sfxState = AudioState.MEDIUM;
                                //setSoundStatus();
                            }
                            else if (arrowRightSFXButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                            {
                                Singleton.Instance.sfxState = AudioState.MUTE;
                                //setSoundStatus();
                            }
                            break;
                    }

                    // Click back
                    if (backButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        mainScreen = true;
                        showSetting = false;
                    }
                }
                if (confirmQuit)
                {
                    if (noButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        mainScreen = true;
                        confirmQuit = false;
                    }
                    else if (yesButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        Singleton.Instance.cmdExit = true;
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

            //set sound status everytime
            switch (Singleton.Instance.bgmState)
            {
                case AudioState.MUTE:
                    masterBGM = 0f;
                    Singleton.Instance.bgMusicVolume = masterBGM;
                    break;
                case AudioState.MEDIUM:
                    masterBGM = 0.5f;
                    Singleton.Instance.bgMusicVolume = masterBGM;
                    break;
                case AudioState.FULL:
                    masterBGM = 1f;
                    Singleton.Instance.bgMusicVolume = masterBGM;
                    break;
            }

            switch (Singleton.Instance.sfxState)
            {
                case AudioState.MUTE:
                    masterSFX = 0f;
                    Singleton.Instance.soundMasterVolume = masterSFX;
                    break;
                case AudioState.MEDIUM:
                    masterSFX = 0.5f;
                    Singleton.Instance.soundMasterVolume = masterSFX;
                    break;
                case AudioState.FULL:
                    masterSFX = 1f;
                    Singleton.Instance.soundMasterVolume = masterSFX;
                    break;
            }

            //update button
            startButton.Update(gameTime);
            settingButton.Update(gameTime);
            exitButton.Update(gameTime);

            arrowLeftBGButton.Update(gameTime);
            arrowRightBGButton.Update(gameTime);
            arrowLeftSFXButton.Update(gameTime);
            arrowRightSFXButton.Update(gameTime);
            backButton.Update(gameTime);
            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            //Draw UI in Main Menu
            if (mainScreen)
            {
                spriteBatch.Draw(backgroundPic, Vector2.Zero, Color.White);
                spriteBatch.Draw(logoPic, new Vector2(865, 130), new Color(255, 255, 255, 255));
                startButton.Draw(spriteBatch);
                settingButton.Draw(spriteBatch);
                exitButton.Draw(spriteBatch);
            }
            // Draw UI when is NOT MainMenu
            else
            {
                spriteBatch.Draw(blackScreenPic, Vector2.Zero, new Color(255, 255, 255, 210));
                // Draw Option Screen
                if (showSetting)
                {
                    spriteBatch.Draw(settingScreenPic, Vector2.Zero, Color.White);
                    backButton.Draw(spriteBatch);
                    spriteBatch.DrawString(bigfonts, "Setting", new Vector2(550, 165), Color.Gray);

                    //BGM
                    spriteBatch.DrawString(mediumfonts, "Musics", new Vector2(300, 324), Color.Gray);

                    arrowLeftBGButton.Draw(spriteBatch);
                    arrowRightBGButton.Draw(spriteBatch);

                    //SFX
                    spriteBatch.DrawString(mediumfonts, "Sounds", new Vector2(300, 444), Color.Gray);

                    arrowLeftSFXButton.Draw(spriteBatch);
                    arrowRightSFXButton.Draw(spriteBatch);
                    // Click Arrow BGM button
                    switch (Singleton.Instance.bgmState)
                    {
                        case AudioState.MUTE:
                            spriteBatch.DrawString(mediumfonts, "MUTE", new Vector2(585, 324), Color.Gray);
                            break;
                        case AudioState.MEDIUM:
                            spriteBatch.DrawString(mediumfonts, "MED", new Vector2(585, 324), Color.Gray);
                            break;
                        case AudioState.FULL:
                            spriteBatch.DrawString(mediumfonts, "FULL", new Vector2(585, 324), Color.Gray);
                            break;
                    }
                    // Click Arrow SFX button
                    switch (Singleton.Instance.sfxState)
                    {
                        case AudioState.MUTE:
                            spriteBatch.DrawString(mediumfonts, "MUTE", new Vector2(580, 444), Color.Gray);
                            break;
                        case AudioState.MEDIUM:
                            spriteBatch.DrawString(mediumfonts, "MED", new Vector2(580, 444), Color.Gray);
                            break;
                        case AudioState.FULL:
                            spriteBatch.DrawString(mediumfonts, "FULL", new Vector2(580, 445), Color.Gray);
                            break;
                    }
                }
                else if (confirmQuit)
                {
                    fontSize = bigfonts.MeasureString("Are you sure");
                    spriteBatch.DrawString(bigfonts, "Are you sure", new Vector2((Singleton.Instance.Dimensions.X - fontSize.X) / 2, 165), Color.White);
                    fontSize = bigfonts.MeasureString("you want to quit?");
                    spriteBatch.DrawString(bigfonts, "you want to quit?", new Vector2((Singleton.Instance.Dimensions.X - fontSize.X) / 2, 260), Color.White);

                    noButton.Draw(spriteBatch);
                    yesButton.Draw(spriteBatch);
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
