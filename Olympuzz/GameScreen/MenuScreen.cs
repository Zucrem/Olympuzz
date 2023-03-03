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
        private Texture2D backgroundPic, blackScreenPic;//background
        private Texture2D logoPic, startPic, settingPic, exitPic;//mainmenu pic
        private Texture2D bgmMuteSoundPic, bgmMediumSoundPic, bgmFullSoundPic, sfxMuteSoundPic, sfxMediumSoundPic, sfxFullSoundPic, backPic;//setting pic
        private Texture2D bgmMuteSoundPic2, bgmMediumSoundPic2, bgmFullSoundPic2, sfxMuteSoundPic2, sfxMediumSoundPic2, sfxFullSoundPic2;//setting pic for not hovering button
        private Texture2D yesPic, noPic;//exit confirmed pic

        //all button
        private Button startButton, settingButton, exitButton;
        private Button bgmMuteSoundButton, bgmMediumSoundButton, bgmFullSoundButton, sfxMuteSoundButton, sfxMediumSoundButton, sfxFullSoundButton, backButton;
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
            bgmMuteSoundButton = new Button(bgmMuteSoundPic, new Vector2(475, 250), new Vector2(175, 50));
            bgmMediumSoundButton = new Button(bgmMediumSoundPic, new Vector2(650, 250), new Vector2(175, 50));
            bgmFullSoundButton = new Button(bgmFullSoundPic, new Vector2(825, 250), new Vector2(175, 50));
            sfxMuteSoundButton = new Button(sfxMuteSoundPic, new Vector2(475, 430), new Vector2(175, 50));
            sfxMediumSoundButton = new Button(sfxMediumSoundPic, new Vector2(650, 430), new Vector2(175, 50));
            sfxFullSoundButton = new Button(sfxFullSoundPic, new Vector2(825, 430), new Vector2(175, 50));

            backButton = new Button(backPic, new Vector2(490, 609), new Vector2(300, 70));

            //confirm Exit button
            yesButton = new Button(backPic, new Vector2(315, 420), new Vector2(300, 70));
            noButton = new Button(backPic, new Vector2(685, 420), new Vector2(300, 70));
        }
        public override void LoadContent()
        {
            base.LoadContent();
            // Texture2D รูปต่างๆ
            //allbackground
            logoPic = content.Load<Texture2D>("GameLogo");
            backgroundPic = content.Load<Texture2D>("MenuScreen/MainMenuBG");
            blackScreenPic = content.Load<Texture2D>("blackScreen");

            //all pic for button
            //mainscreen pic
            startPic = content.Load<Texture2D>("MenuScreen/PlayButton");
            settingPic = content.Load<Texture2D>("MenuScreen/SettingButton");
            exitPic = content.Load<Texture2D>("MenuScreen/ExitButton");
            //setting pic
            backPic = content.Load<Texture2D>("PlayScreen/Water");

            bgmMuteSoundPic = content.Load<Texture2D>("PlayScreen/Fire");
            bgmMediumSoundPic = content.Load<Texture2D>("PlayScreen/Wind");
            bgmFullSoundPic = content.Load<Texture2D>("PlayScreen/Earth");
            sfxMuteSoundPic = content.Load<Texture2D>("PlayScreen/Fire");
            sfxMediumSoundPic = content.Load<Texture2D>("PlayScreen/Wind");
            sfxFullSoundPic = content.Load<Texture2D>("PlayScreen/Earth");
            //if it cant hovering
            bgmMuteSoundPic2 = content.Load<Texture2D>("PlayScreen/Water");
            bgmMediumSoundPic2 = content.Load<Texture2D>("PlayScreen/Water");
            bgmFullSoundPic2 = content.Load<Texture2D>("PlayScreen/Water");
            sfxMuteSoundPic2 = content.Load<Texture2D>("PlayScreen/Water");
            sfxMediumSoundPic2 = content.Load<Texture2D>("PlayScreen/Water");
            sfxFullSoundPic2 = content.Load<Texture2D>("PlayScreen/Water");

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
                    if (bgmMuteSoundButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        if (masterBGM > 0f)
                        {
                            masterBGM = 0f;
                            Singleton.Instance.bgMusicVolume = masterBGM; 
                            Singleton.Instance.bgmState = AudioState.MUTE;
                        }
                    }
                    else if (bgmMediumSoundButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        masterBGM = 0.5f;
                        Singleton.Instance.bgMusicVolume = masterBGM;
                        Singleton.Instance.bgmState = AudioState.MEDIUM;

                    }
                    else if (bgmFullSoundButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        if (masterBGM < 1f)
                        {
                            masterBGM = 1f;
                            Singleton.Instance.bgMusicVolume = masterBGM;
                            Singleton.Instance.bgmState = AudioState.FULL;
                        }
                    }

                    // Click Arrow SFX button
                    if (sfxMuteSoundButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        if (masterSFX > 0f)
                        {
                            masterSFX = 0f;
                            Singleton.Instance.soundMasterVolume = masterSFX;
                            Singleton.Instance.sfxState = AudioState.MUTE;
                        }
                    }
                    else if (sfxMediumSoundButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        masterSFX = 0.5f;
                        Singleton.Instance.soundMasterVolume = masterSFX;
                        Singleton.Instance.sfxState = AudioState.MEDIUM;
                    }
                    else if (sfxFullSoundButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        if (masterSFX < 1f)
                        {
                            masterSFX = 1f;
                            Singleton.Instance.soundMasterVolume = masterSFX;
                            Singleton.Instance.sfxState = AudioState.FULL;
                        }
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
            setSoundStatus();

            //update button
            startButton.Update(gameTime);
            settingButton.Update(gameTime);
            exitButton.Update(gameTime);
            bgmMuteSoundButton.Update(gameTime);
            bgmMediumSoundButton.Update(gameTime);
            bgmFullSoundButton.Update(gameTime);
            sfxMuteSoundButton.Update(gameTime);
            sfxMediumSoundButton.Update(gameTime);
            sfxFullSoundButton.Update(gameTime);
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
                    backButton.Draw(spriteBatch);
                    spriteBatch.DrawString(bigfonts, "Setting", new Vector2(513, 93), Color.Yellow);

                    //BGM
                    spriteBatch.DrawString(mediumfonts, "Musics", new Vector2(276, 257), Color.Yellow);

                    bgmMuteSoundButton.Draw(spriteBatch);
                    bgmMediumSoundButton.Draw(spriteBatch);
                    bgmFullSoundButton.Draw(spriteBatch);

                    //SFX
                    spriteBatch.DrawString(mediumfonts, "Sounds", new Vector2(276, 437), Color.Yellow);

                    sfxMuteSoundButton.Draw(spriteBatch);
                    sfxMediumSoundButton.Draw(spriteBatch);
                    sfxFullSoundButton.Draw(spriteBatch);

                }
                else if (confirmQuit)
                {
                    fontSize = smallfonts.MeasureString("Are you sure");
                    spriteBatch.DrawString(bigfonts, "Are you sure", new Vector2(((Singleton.Instance.Dimensions.X - fontSize.X) / 2) -90, 165), Color.White);
                    fontSize = smallfonts.MeasureString("you want to quit?");
                    spriteBatch.DrawString(bigfonts, "you want to quit?", new Vector2(((Singleton.Instance.Dimensions.X - fontSize.X) / 2) - 100, 260), Color.White);

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
        public void setSoundStatus()
        {
            switch (Singleton.Instance.bgmState)
            {
                case AudioState.MUTE:
                    bgmMuteSoundButton.setTexture(bgmMuteSoundPic2);
                    bgmMediumSoundButton.setTexture(bgmMediumSoundPic);
                    bgmFullSoundButton.setTexture(bgmFullSoundPic);

                    bgmMuteSoundButton.setCantHover(true);
                    bgmMediumSoundButton.setCantHover(false);
                    bgmFullSoundButton.setCantHover(false);
                    break;
                case AudioState.MEDIUM:
                    bgmMuteSoundButton.setCantHover(false);
                    bgmMediumSoundButton.setCantHover(true);
                    bgmFullSoundButton.setCantHover(false);

                    bgmMuteSoundButton.setTexture(bgmMuteSoundPic);
                    bgmMediumSoundButton.setTexture(bgmMediumSoundPic2);
                    bgmFullSoundButton.setTexture(bgmFullSoundPic);
                    break;
                case AudioState.FULL:
                    bgmMuteSoundButton.setCantHover(false);
                    bgmMediumSoundButton.setCantHover(false);
                    bgmFullSoundButton.setCantHover(true);

                    bgmMuteSoundButton.setTexture(bgmMuteSoundPic);
                    bgmMediumSoundButton.setTexture(bgmMediumSoundPic);
                    bgmFullSoundButton.setTexture(bgmFullSoundPic2);
                    break;
            }

            switch (Singleton.Instance.sfxState)
            {
                case AudioState.MUTE:
                    sfxMuteSoundButton.setCantHover(true);
                    sfxMediumSoundButton.setCantHover(false);
                    sfxFullSoundButton.setCantHover(false);

                    sfxMuteSoundButton.setTexture(sfxMuteSoundPic2);
                    sfxMediumSoundButton.setTexture(sfxMediumSoundPic);
                    sfxFullSoundButton.setTexture(sfxFullSoundPic);
                    break;
                case AudioState.MEDIUM:
                    sfxMuteSoundButton.setCantHover(false);
                    sfxMediumSoundButton.setCantHover(true);
                    sfxFullSoundButton.setCantHover(false);

                    sfxMuteSoundButton.setTexture(sfxMuteSoundPic);
                    sfxMediumSoundButton.setTexture(sfxMediumSoundPic2);
                    sfxFullSoundButton.setTexture(sfxFullSoundPic);
                    break;
                case AudioState.FULL:
                    sfxMuteSoundButton.setCantHover(false);
                    sfxMediumSoundButton.setCantHover(false);
                    sfxFullSoundButton.setCantHover(true);

                    sfxMuteSoundButton.setTexture(sfxMuteSoundPic);
                    sfxMediumSoundButton.setTexture(sfxMediumSoundPic);
                    sfxFullSoundButton.setTexture(sfxFullSoundPic2);
                    break;
            }
        }
    }
}
