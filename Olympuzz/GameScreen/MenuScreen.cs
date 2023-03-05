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
        private Texture2D arrowLeftBGPic, arrowRightBGPic, arrowLeftSFXPic, arrowRightSFXPic, backSettingPic, selectStagePic;//setting pic
        private Texture2D stage1Pic1, stage2Pic1, stage3Pic1, stageEndlessPic1, stage1Pic2, stage2Pic2, stage3Pic2, stageEndlessPic2, howToPlayPic, backSelectLevelPic;//select level pic
        private Texture2D char1Pic1, char1Pic2, char2Pic1, char3Pic1, char4Pic1, char2Pic2, char3Pic2, char4Pic2, selectCharPic, backSelectCharPic, arrowLeftCharPic, arrowRightCharPic;//select character pic
        private Texture2D backHowToPlayPic;
        private Texture2D yesPic, noPic;//exit confirmed pic

        //all button
        private Button startButton, settingButton, exitButton;//mainmenu button
        private Button arrowLeftBGButton, arrowRightBGButton, arrowLeftSFXButton, arrowRightSFXButton, backSettingButton;//setting button
        private Button stage1Button, stage2Button, stage3Button, stageEndlessButton, howToPlayButton, backSelectLevelButton, selectStageButton;//select level button
        private Button char1Button, char2Button, char3Button, char4Button, selectCharButton, backSelectCharButton, arrowLeftCharButton, arrowRightCharButton;//select charactor button
        private Button backHowToPlayButton;
        private Button yesButton, noButton;//exit confirmed pic

        //private SpriteFont Alagan
        private SpriteFont smallfonts, mediumfonts, bigfonts;//กำหนดชื่อ font
        private Vector2 fontSize;//ขนาด font จาก SpriteFont

        //bg and sfx sound
        private Song MainmenuTheme;

        //check if go next page or fade finish
        private bool fadeFinish = false;
        private bool mainScreen = true;
        private bool showSettingScreen = false;
        private bool selectStageScreen = false;
        private bool selectCharScreen = false;
        private bool howToPlayScreen = false;
        private bool confirmQuit = false;

        //check if press arrow at selectCharScreen
        private int charSlide = 1;

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
            startButton = new Button(startPic, new Vector2(273, 293), new Vector2(160, 80));
            settingButton = new Button(settingPic, new Vector2(273, 410), new Vector2(160, 60));
            exitButton = new Button(exitPic, new Vector2(273, 505), new Vector2(100, 45));

            //setting button
            arrowLeftBGButton = new Button(arrowLeftBGPic, new Vector2(525, 320), new Vector2(40, 40));
            arrowRightBGButton = new Button(arrowRightBGPic, new Vector2(705, 320), new Vector2(40, 40));
            arrowLeftSFXButton = new Button(arrowLeftSFXPic, new Vector2(525, 440), new Vector2(40, 40));
            arrowRightSFXButton = new Button(arrowRightSFXPic, new Vector2(705, 440), new Vector2(40, 40));

            backSettingButton = new Button(backSettingPic, new Vector2(980, 570), new Vector2(150, 60));

            //select level button
            stage1Button = new Button(stage1Pic1, new Vector2(215, 200), new Vector2(275, 275));
            stage2Button = new Button(stage2Pic1, new Vector2(502, 200), new Vector2(275, 275));
            stage3Button = new Button(stage3Pic1, new Vector2(790, 200), new Vector2(275, 275));
            stageEndlessButton = new Button(stageEndlessPic1, new Vector2(215, 497), new Vector2(850, 70));
            backSelectLevelButton = new Button(backSelectLevelPic, new Vector2(215, 610), new Vector2(300, 70));
            //howToPlayButton = new Button(howToPlayPic, new Vector2(765, 610), new Vector2(300, 70));
            selectStageButton = new Button(selectCharPic, new Vector2(765, 610), new Vector2(300, 70));

            //select charactor button
            char1Button = new Button(char1Pic1, new Vector2(215, 200), new Vector2(275, 350));
            char2Button = new Button(char2Pic1, new Vector2(502, 200), new Vector2(275, 350));
            char3Button = new Button(char3Pic1, new Vector2(790, 200), new Vector2(275, 350));
            char4Button = new Button(char4Pic1, new Vector2(790, 200), new Vector2(275, 350));
            arrowLeftCharButton = new Button(arrowLeftCharPic, new Vector2(121, 337), new Vector2(75, 75));
            arrowRightCharButton = new Button(arrowRightCharPic, new Vector2(1092, 337), new Vector2(75, 75));
            backSelectCharButton = new Button(backSelectCharPic, new Vector2(215, 610), new Vector2(300, 70));
            selectCharButton = new Button(selectCharPic, new Vector2(765, 610), new Vector2(300, 70));

            //howtoplay button
            backHowToPlayButton = new Button(selectCharPic, new Vector2(490, 610), new Vector2(300, 70));

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
            backSettingPic = content.Load<Texture2D>("SettingScreen/DoneButton");

            arrowLeftBGPic = content.Load<Texture2D>("SettingScreen/ArrowButton");
            arrowRightBGPic = content.Load<Texture2D>("SettingScreen/ArrowRButton");
            arrowLeftSFXPic = content.Load<Texture2D>("SettingScreen/ArrowButton");
            arrowRightSFXPic = content.Load<Texture2D>("SettingScreen/ArrowRButton");

            //select level pic
            stage1Pic1 = content.Load<Texture2D>("PlayScreen/Earth");
            stage2Pic1 = content.Load<Texture2D>("PlayScreen/Earth");
            stage3Pic1 = content.Load<Texture2D>("PlayScreen/Earth");
            stageEndlessPic1 = content.Load<Texture2D>("PlayScreen/Earth");
            stage1Pic2 = content.Load<Texture2D>("PlayScreen/Water");
            stage2Pic2 = content.Load<Texture2D>("PlayScreen/Water");
            stage3Pic2 = content.Load<Texture2D>("PlayScreen/Water");
            stageEndlessPic2 = content.Load<Texture2D>("PlayScreen/Water");
            backSelectLevelPic = content.Load<Texture2D>("PlayScreen/Fire");
            howToPlayPic = content.Load<Texture2D>("PlayScreen/Wind");
            selectStagePic = content.Load<Texture2D>("PlayScreen/Thunder");

            //select charactor pic
            char1Pic1 = content.Load<Texture2D>("PlayScreen/Earth");
            char2Pic1 = content.Load<Texture2D>("PlayScreen/Wind");
            char3Pic1 = content.Load<Texture2D>("PlayScreen/Water");
            char4Pic1 = content.Load<Texture2D>("PlayScreen/Earth");
            char1Pic2 = content.Load<Texture2D>("PlayScreen/Thunder");
            char2Pic2 = content.Load<Texture2D>("PlayScreen/Thunder");
            char3Pic2 = content.Load<Texture2D>("PlayScreen/Thunder");
            char4Pic2 = content.Load<Texture2D>("PlayScreen/Thunder");
            arrowLeftCharPic = content.Load<Texture2D>("PlayScreen/Water");
            arrowRightCharPic = content.Load<Texture2D>("PlayScreen/Water");
            backSelectCharPic = content.Load<Texture2D>("PlayScreen/Fire");
            selectCharPic = content.Load<Texture2D>("PlayScreen/Wind");

            //howtoplay pic
            backHowToPlayPic = content.Load<Texture2D>("PlayScreen/Wind");

            //confirmQuit pic
            yesPic = content.Load<Texture2D>("PlayScreen/Fire");
            noPic = content.Load<Texture2D>("PlayScreen/Wind");

            // Fonts
            smallfonts = content.Load<SpriteFont>("Alagard");
            mediumfonts = content.Load<SpriteFont>("AlagardMedium");
            bigfonts = content.Load<SpriteFont>("AlagardBig");

            //song and sfx
            MainmenuTheme = content.Load<Song>("Sounds/MainmenuTheme");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(MainmenuTheme);

            //reset Stage and Char
            Singleton.Instance.levelState = LevelState.NULL;
            Singleton.Instance.charState = CharState.NULL;

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
                    selectStageScreen = true;
                    mainScreen = false;
                }
                // Click setting
                if (settingButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                {
                    showSettingScreen = true;
                    mainScreen = false;
                }
                // Click Exit
                if (exitButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                {
                    confirmQuit = true;
                    mainScreen = false;
                }
            }
            if (selectStageScreen)
            {
                //select stage1
                if (stage1Button.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                {
                    Singleton.Instance.levelState = LevelState.POSEIDON;
                }
                //select stage2
                if (stage2Button.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                {
                    Singleton.Instance.levelState = LevelState.HADES;
                }
                //select stage3
                if (stage3Button.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                {
                    Singleton.Instance.levelState = LevelState.ZEUS;
                }
                //select endless
                /*if (stageEndlessButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                {
                    Singleton.Instance.levelState = LevelState.ENDLESS;
                }*/
                //back
                if (backSelectLevelButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                {
                    selectStageScreen = false;
                    mainScreen = true;
                    Singleton.Instance.levelState = LevelState.NULL;
                }
                //selectStage
                if (selectStageButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                {
                    if (Singleton.Instance.levelState != LevelState.NULL)
                    {
                        selectStageScreen = false;
                        selectCharScreen = true;
                    }
                }
                //how to play
                /*if (howToPlayButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                {
                    selectStageScreen = false;
                    howToPlayScreen = true;
                }*/
            }
            if (selectCharScreen)
            {
                //select that char
                if (char1Button.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                {
                    Singleton.Instance.charState = CharState.ATHENA;
                }
                if (char2Button.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                {
                    Singleton.Instance.charState = CharState.HERMES;
                }
                if (char3Button.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                {
                    Singleton.Instance.charState = CharState.DIONYSUS;
                }
                if (char4Button.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                {
                    Singleton.Instance.charState = CharState.HEPHAESTUS;
                }
                //click arrow
                if (arrowRightCharButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                {
                    char1Button.setPosition(new Vector2(0, 0));
                    char2Button.setPosition(new Vector2(215, 200));
                    char3Button.setPosition(new Vector2(502, 200));
                    charSlide = 2;
                    Singleton.Instance.charState = CharState.NULL;
                }
                if (arrowLeftCharButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                {
                    char1Button.setPosition(new Vector2(215, 200));
                    char2Button.setPosition(new Vector2(502, 200));
                    char3Button.setPosition(new Vector2(790, 200));
                    charSlide = 1;
                    Singleton.Instance.charState = CharState.NULL;
                }

                //click back
                if (backSelectCharButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                {
                    selectCharScreen = false;
                    selectStageScreen = true;
                    Singleton.Instance.levelState = LevelState.NULL;
                    Singleton.Instance.charState = CharState.NULL;
                }

                //click play
                if (selectCharButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                {
                    if ((Singleton.Instance.levelState != LevelState.NULL) && (Singleton.Instance.charState != CharState.NULL))
                    {
                        selectCharScreen = false;
                        ScreenManager.Instance.LoadScreen(ScreenManager.GameScreenName.PlayScreen);
                    }
                }
            }
            if (howToPlayScreen)
            {
                //click continue
                if (selectCharButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                {
                    howToPlayScreen = false;
                    mainScreen = true;
                }
            }
            else
            {
                if (showSettingScreen)
                {
                    // Click Arrow BGM button
                    switch (Singleton.Instance.bgmState)
                    {
                        case AudioState.MUTE:
                            if (arrowLeftBGButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                            {
                                Singleton.Instance.bgmState = AudioState.FULL;
                            }
                            else if (arrowRightBGButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                            {
                                Singleton.Instance.bgmState = AudioState.MEDIUM;
                            }
                            break;
                        case AudioState.MEDIUM:
                            if (arrowLeftBGButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                            {
                                Singleton.Instance.bgmState = AudioState.MUTE;
                            }
                            else if (arrowRightBGButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                            {
                                Singleton.Instance.bgmState = AudioState.FULL;
                            }
                            break;
                        case AudioState.FULL:
                            if (arrowLeftBGButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                            {
                                Singleton.Instance.bgmState = AudioState.MEDIUM;
                            }
                            else if (arrowRightBGButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                            {
                                Singleton.Instance.bgmState = AudioState.MUTE;
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
                    if (backSettingButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        mainScreen = true;
                        showSettingScreen = false;
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
                    masterSFX = 0.1f;
                    Singleton.Instance.soundMasterVolume = masterSFX;
                    break;
                case AudioState.FULL:
                    masterSFX = 0.6f;
                    Singleton.Instance.soundMasterVolume = masterSFX;
                    break;
            }
            //update
            setStageButtonStatus();
            setCharButtonStatus();

            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            //draw bg
            if (mainScreen || selectStageScreen || selectCharScreen)
            {
                spriteBatch.Draw(backgroundPic, Vector2.Zero, Color.White);
            }
            //Draw UI in Main Menu
            if (mainScreen)
            {
                spriteBatch.Draw(logoPic, new Vector2(865, 130), new Color(255, 255, 255, 255));
                startButton.Draw(spriteBatch);
                settingButton.Draw(spriteBatch);
                exitButton.Draw(spriteBatch);
            }
            if (selectStageScreen)
            {
                fontSize = bigfonts.MeasureString("Select Stage");
                spriteBatch.DrawString(bigfonts, "Select Stage", new Vector2((Singleton.Instance.Dimensions.X - fontSize.X) / 2, 93), Color.Yellow);
                stage1Button.Draw(spriteBatch);
                stage2Button.Draw(spriteBatch);
                stage3Button.Draw(spriteBatch);
                stageEndlessButton.Draw(spriteBatch);
                //howToPlayButton.Draw(spriteBatch); 
                selectStageButton.Draw(spriteBatch);
                backSelectLevelButton.Draw(spriteBatch);
            }
            if (selectCharScreen)
            {
                fontSize = bigfonts.MeasureString("Select Your Character");
                spriteBatch.DrawString(bigfonts, "Select Your Character", new Vector2((Singleton.Instance.Dimensions.X - fontSize.X) / 2, 93), Color.Yellow);
                if (charSlide == 1)
                {
                    char1Button.Draw(spriteBatch);
                    char2Button.Draw(spriteBatch);
                    char3Button.Draw(spriteBatch);
                    arrowRightCharButton.Draw(spriteBatch);
                }
                if(charSlide == 2)
                {
                    char2Button.Draw(spriteBatch);
                    char3Button.Draw(spriteBatch);
                    char4Button.Draw(spriteBatch);
                    arrowLeftCharButton.Draw(spriteBatch);
                }
                selectCharButton.Draw(spriteBatch);
                backSelectCharButton.Draw(spriteBatch);
            }
            // Draw UI when is NOT MainMenu
            else
            {
                // Draw Option Screen
                if (showSettingScreen)
                {
                    spriteBatch.Draw(settingScreenPic, Vector2.Zero, Color.White);
                    backSettingButton.Draw(spriteBatch);
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
                    spriteBatch.Draw(blackScreenPic, Vector2.Zero, new Color(255, 255, 255, 210));
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
        public void setStageButtonStatus()
        {
            switch (Singleton.Instance.levelState)
            {
                case LevelState.NULL:
                    stage1Button.setTexture(stage1Pic1);
                    stage2Button.setTexture(stage2Pic1);
                    stage3Button.setTexture(stage1Pic1);
                    stageEndlessButton.setTexture(stageEndlessPic1);

                    stage1Button.setCantHover(false);
                    stage2Button.setCantHover(false);
                    stage3Button.setCantHover(false);
                    stageEndlessButton.setCantHover(false);
                    selectStageButton.setCantHover(true);
                    break;
                case LevelState.POSEIDON:
                    stage1Button.setTexture(stage1Pic2);
                    stage2Button.setTexture(stage2Pic1);
                    stage3Button.setTexture(stage1Pic1);
                    stageEndlessButton.setTexture(stageEndlessPic1);

                    stage1Button.setCantHover(true);
                    stage2Button.setCantHover(false);
                    stage3Button.setCantHover(false);
                    stageEndlessButton.setCantHover(false);
                    selectStageButton.setCantHover(false);
                    break;
                case LevelState.HADES:
                    stage1Button.setTexture(stage1Pic1);
                    stage2Button.setTexture(stage2Pic2);
                    stage3Button.setTexture(stage1Pic1);
                    stageEndlessButton.setTexture(stageEndlessPic1);

                    stage1Button.setCantHover(false);
                    stage2Button.setCantHover(true);
                    stage3Button.setCantHover(false);
                    stageEndlessButton.setCantHover(false);
                    selectStageButton.setCantHover(false);
                    break;
                case LevelState.ZEUS:
                    stage1Button.setTexture(stage1Pic1);
                    stage2Button.setTexture(stage2Pic1);
                    stage3Button.setTexture(stage1Pic2);
                    stageEndlessButton.setTexture(stageEndlessPic1);

                    stage1Button.setCantHover(false);
                    stage2Button.setCantHover(false);
                    stage3Button.setCantHover(true);
                    stageEndlessButton.setCantHover(false);
                    selectStageButton.setCantHover(false);
                    break;
                case LevelState.ENDLESS:
                    stage1Button.setTexture(stage1Pic1);
                    stage2Button.setTexture(stage2Pic1);
                    stage3Button.setTexture(stage1Pic1);
                    stageEndlessButton.setTexture(stageEndlessPic2);

                    stage1Button.setCantHover(false);
                    stage2Button.setCantHover(false);
                    stage3Button.setCantHover(false);
                    stageEndlessButton.setCantHover(true);
                    selectStageButton.setCantHover(false);
                    break;
            }
        }
        public void setCharButtonStatus()
        {
            switch (Singleton.Instance.charState)
            {
                case CharState.NULL:
                    char1Button.setTexture(char1Pic1);
                    char2Button.setTexture(char2Pic1);
                    char3Button.setTexture(char3Pic1);
                    char4Button.setTexture(char4Pic1);

                    char1Button.setCantHover(false);
                    char2Button.setCantHover(false);
                    char3Button.setCantHover(false);
                    char4Button.setCantHover(false);
                    selectCharButton.setCantHover(true);
                    break;
                case CharState.ATHENA:
                    char1Button.setTexture(char1Pic2);
                    char2Button.setTexture(char2Pic1);
                    char3Button.setTexture(char3Pic1);
                    char4Button.setTexture(char4Pic1);

                    char1Button.setCantHover(true);
                    char2Button.setCantHover(false);
                    char3Button.setCantHover(false);
                    char4Button.setCantHover(false);
                    selectCharButton.setCantHover(false);
                    break;
                case CharState.HERMES:
                    char1Button.setTexture(char1Pic1);
                    char2Button.setTexture(char2Pic2);
                    char3Button.setTexture(char3Pic1);
                    char4Button.setTexture(char4Pic1);

                    char1Button.setCantHover(false);
                    char2Button.setCantHover(true);
                    char3Button.setCantHover(false);
                    char4Button.setCantHover(false);
                    selectCharButton.setCantHover(false);
                    break;
                case CharState.DIONYSUS:
                    char1Button.setTexture(char1Pic1);
                    char2Button.setTexture(char2Pic1);
                    char3Button.setTexture(char3Pic2);
                    char4Button.setTexture(char4Pic1);

                    char1Button.setCantHover(false);
                    char2Button.setCantHover(false);
                    char3Button.setCantHover(true);
                    char4Button.setCantHover(false);
                    selectCharButton.setCantHover(false);
                    break;
                case CharState.HEPHAESTUS:
                    char1Button.setTexture(char1Pic1);
                    char2Button.setTexture(char2Pic1);
                    char3Button.setTexture(char3Pic1);
                    char4Button.setTexture(char4Pic2);

                    char1Button.setCantHover(false);
                    char2Button.setCantHover(false);
                    char3Button.setCantHover(false);
                    char4Button.setCantHover(true);
                    selectCharButton.setCantHover(false);
                    break;
            }
        }
    }
}
