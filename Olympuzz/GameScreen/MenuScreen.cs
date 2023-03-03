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

namespace Olympuzz.GameScreen
{
    class MenuScreen : _GameScreen
    {
        
        private Color _Color = new Color(250, 250, 250, 0);
        private Texture2D backgroundPic, blackScreenPic;//background
        //all picture
        private Texture2D logoPic, startPic, settingPic, exitPic, bgmMuteSoundPic, bgmMediumSoundPic, bgmFullSoundPic, sfxMuteSoundPic, sfxMediumSoundPic, sfxFullSoundPic, back;

        //all button
        private Button startButton, settingButton, exitButton, bgmMuteSoundButton, bgmMediumSoundButton, bgmFullSoundButton, sfxMuteSoundButton, sfxMediumSoundButton, sfxFullSoundButton, backButton;

        //private SpriteFont Alagan;
        private SpriteFont smallfonts, mediumfonts, bigfonts;//กำหนดชื่อ font

        //bg and sfx song
        private Song MainmenuTheme;


        //check if go next page or fade finish
        private bool fadeFinish = false;
        private bool mainScreen = true;
        private bool showSetting = false;

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
            startButton = new Button(startPic, new Vector2(293, 293), new Vector2(300, 70));
            settingButton = new Button(settingPic, new Vector2(293, 410), new Vector2(300, 70));
            exitButton = new Button(exitPic, new Vector2(293, 505), new Vector2(300, 70));

            //setting button
            bgmMuteSoundButton = new Button(bgmMuteSoundPic, new Vector2(475, 250), new Vector2(175, 50));
            bgmMediumSoundButton = new Button(bgmMediumSoundPic, new Vector2(650, 250), new Vector2(175, 50));
            bgmFullSoundButton = new Button(bgmFullSoundPic, new Vector2(825, 250), new Vector2(175, 50));
            sfxMuteSoundButton = new Button(sfxMuteSoundPic, new Vector2(475, 430), new Vector2(175, 50));
            sfxMediumSoundButton = new Button(sfxMediumSoundPic, new Vector2(650, 430), new Vector2(175, 50));
            sfxFullSoundButton = new Button(sfxFullSoundPic, new Vector2(825, 430), new Vector2(175, 50));
            backButton = new Button(back, new Vector2(490, 609), new Vector2(300, 70));
        }
        public override void LoadContent()
        {
            base.LoadContent();
            // Texture2D รูปต่างๆ
            //allbackground
            logoPic = content.Load<Texture2D>("GameLogo");
            backgroundPic = content.Load<Texture2D>("MenuScreen/MainMenuBG");
            blackScreenPic = content.Load<Texture2D>("blackScreen");

            //all button
            //mainscreen button
            startPic = content.Load<Texture2D>("MenuScreen/PlayButton");
            settingPic = content.Load<Texture2D>("MenuScreen/SettingButton");
            exitPic = content.Load<Texture2D>("MenuScreen/ExitButton");
            //setting button
            bgmMuteSoundPic = content.Load<Texture2D>("PlayScreen/Fire");
            bgmMediumSoundPic = content.Load<Texture2D>("PlayScreen/Wind");
            bgmFullSoundPic = content.Load<Texture2D>("PlayScreen/Earth");
            sfxMuteSoundPic = content.Load<Texture2D>("PlayScreen/Fire");
            sfxMediumSoundPic = content.Load<Texture2D>("PlayScreen/Wind");
            sfxFullSoundPic = content.Load<Texture2D>("PlayScreen/Earth");
            back = content.Load<Texture2D>("PlayScreen/Water");

            // Fonts
            smallfonts = content.Load<SpriteFont>("Alagard");
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
                // Click Exit
                if (exitButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                {
                    Singleton.Instance.cmdExit = true;
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
                            Singleton.Instance.bgMusicVolume = masterBGM ;
                        }
                    }
                    else if (bgmMediumSoundButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        masterBGM = 0.5f;
                        Singleton.Instance.bgMusicVolume = masterBGM;
                    }
                    else if (bgmFullSoundButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        if (masterBGM < 1f)
                        {
                            masterBGM = 1f;
                            Singleton.Instance.bgMusicVolume = masterBGM;
                        }
                    }

                    // Click Arrow SFX button
                    if (sfxMuteSoundButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        if (masterSFX > 0f)
                        {
                            masterSFX = 0f;
                            Singleton.Instance.soundMasterVolume = masterSFX;
                        }
                    }
                    else if (sfxMediumSoundButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                            masterSFX = 0.5f;
                            Singleton.Instance.soundMasterVolume = masterSFX;
                    }
                    else if (sfxFullSoundButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        if (masterSFX < 1f)
                        {
                            masterSFX = 1f;
                            Singleton.Instance.soundMasterVolume = masterSFX;
                        }
                    }

                    // Click Back
                    if (backButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        mainScreen = true;
                        showSetting = false;
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
                backButton.Draw(spriteBatch);
                // Draw Option Screen
                if (showSetting)
                {
                    spriteBatch.DrawString(bigfonts, "Setting", new Vector2(513, 93), Color.White);

                    //BGM
                    spriteBatch.DrawString(smallfonts, "BGM Volume", new Vector2(279, 290), Color.White);

                    bgmMuteSoundButton.Draw(spriteBatch);
                    bgmMediumSoundButton.Draw(spriteBatch);
                    bgmFullSoundButton.Draw(spriteBatch);

                    //SFX
                    spriteBatch.DrawString(smallfonts, "SFX Volume", new Vector2(279, 430), Color.White);

                    sfxMuteSoundButton.Draw(spriteBatch);
                    sfxMediumSoundButton.Draw(spriteBatch);
                    sfxFullSoundButton.Draw(spriteBatch);
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
