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
        private Texture2D backgroundPic, blackScreenPic, settingScreenPic, StageBGPic, CharBGPic , howToPlayBG1Pic, howToPlayBG2Pic;//background
        private Texture2D logoPic, startPic, settingPic, exitPic;//mainmenu pic
        private Texture2D arrowLeftBGPic, arrowRightBGPic, arrowLeftSFXPic, arrowRightSFXPic, backSettingPic, selectStagePic;//setting pic
        private Texture2D stage1Pic1, stage2Pic1, stage3Pic1, stageEndlessPic1, stage1Pic2, stage2Pic2, stage3Pic2, stageEndlessPic2, howToPlayPic, backSelectLevelPic;//select level pic
        private Texture2D char1Pic1, char1Pic2, char2Pic1, char3Pic1, char4Pic1, char2Pic2, char3Pic2, char4Pic2, selectCharPic, backSelectCharPic, arrowLeftCharPic, arrowRightCharPic;//select character pic
        private Texture2D arrowLeftHowToPlayPic, arrowRightHowToPlayPic, backHowToPlayPic;//how to play pic
        private Texture2D confirmQuitPopUpPic, yesConfirmQuitPic1, noConfirmQuitPic1, yesConfirmQuitPic2, noConfirmQuitPic2;//exit confirmed pic

        //all button
        private Button startButton, settingButton, exitButton;//mainmenu button
        private Button arrowLeftBGButton, arrowRightBGButton, arrowLeftSFXButton, arrowRightSFXButton, backSettingButton;//setting button
        private Button stage1Button, stage2Button, stage3Button, stageEndlessButton, howToPlayButton, backSelectLevelButton, selectStageButton;//select level button
        private Button char1Button, char2Button, char3Button, char4Button, selectCharButton, backSelectCharButton, arrowLeftCharButton, arrowRightCharButton;//select charactor button
        private Button arrowLeftHowToPlayButton, arrowRightHowToPlayButton, backHowToPlayButton;
        private Button yesButton, noButton;//exit confirmed pic

        //private SpriteFont Alagan
        private SpriteFont smallfonts, mediumfonts, bigfonts;//กำหนดชื่อ font
        private Vector2 fontSize;//ขนาด font จาก SpriteFont

        //bg and sfx sound
        private Song MainmenuTheme;

        //check if go next screen or fade finish
        private bool fadeFinish = false;
        private StateScreen screen = StateScreen.MAINSCREEN;
        private enum StateScreen
        {
            MAINSCREEN,
            SETTINGSCREEN,
            QUITSCREEN,
            HOWTOPLAYSCREEN,
            SELECTSTAGESCREEN,
            SELECTCHARSCREEN
        }

        //check if press arrow at selectCharScreen
        private int charSlide = 1;
        //check if press arrow at howtoplay screen
        private int howToPlaySlide = 1;

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
            stage1Button = new Button(stage1Pic1, new Vector2(146, 189), new Vector2(220, 342));
            stage2Button = new Button(stage2Pic1, new Vector2(401, 189), new Vector2(220, 342));
            stage3Button = new Button(stage3Pic1, new Vector2(656, 189), new Vector2(220, 342));
            stageEndlessButton = new Button(stageEndlessPic1, new Vector2(911, 189), new Vector2(220, 342));
            backSelectLevelButton = new Button(backSelectLevelPic, new Vector2(30, 40), new Vector2(100, 35));
            howToPlayButton = new Button(howToPlayPic, new Vector2(100, 610), new Vector2(200, 70));
            selectStageButton = new Button(selectStagePic, new Vector2(960, 610), new Vector2(130, 50));

            //select charactor button
            char1Button = new Button(char1Pic1, new Vector2(215, 165), new Vector2(248, 342));
            char2Button = new Button(char2Pic1, new Vector2(502, 165), new Vector2(248, 342));
            char3Button = new Button(char3Pic1, new Vector2(790, 165), new Vector2(248, 342));
            char4Button = new Button(char4Pic1, new Vector2(790, 165), new Vector2(248, 342));
            arrowLeftCharButton = new Button(arrowLeftCharPic, new Vector2(68, 302), new Vector2(64, 90));
            arrowRightCharButton = new Button(arrowRightCharPic, new Vector2(1174, 302), new Vector2(64, 90));
            backSelectCharButton = new Button(backSelectCharPic, new Vector2(26, 506), new Vector2(174, 72));
            selectCharButton = new Button(selectCharPic, new Vector2(1086, 506), new Vector2(174, 72));

            //howtoplay button
            arrowLeftHowToPlayButton = new Button(arrowLeftHowToPlayPic, new Vector2(68, 302), new Vector2(64, 90));
            arrowRightHowToPlayButton = new Button(arrowRightHowToPlayPic, new Vector2(1174, 302), new Vector2(64, 90));
            backHowToPlayButton = new Button(backHowToPlayPic, new Vector2(490, 630), new Vector2(300, 70));

            //confirm Exit button
            yesButton = new Button(yesConfirmQuitPic1, new Vector2(495, 390), new Vector2(120, 60));
            noButton = new Button(noConfirmQuitPic1, new Vector2(660, 390), new Vector2(120, 60));
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
            StageBGPic = content.Load<Texture2D>("SelectStage/StageSelectScreen");
            stage1Pic1 = content.Load<Texture2D>("SelectStage/PoseidonStage");
            stage2Pic1 = content.Load<Texture2D>("SelectStage/HadesStage");
            stage3Pic1 = content.Load<Texture2D>("SelectStage/PoseidonStage");
            stageEndlessPic1 = content.Load<Texture2D>("SelectStage/PoseidonStage");
            stage1Pic2 = content.Load<Texture2D>("PlayScreen/Water");
            stage2Pic2 = content.Load<Texture2D>("PlayScreen/Water");
            stage3Pic2 = content.Load<Texture2D>("PlayScreen/Water");
            stageEndlessPic2 = content.Load<Texture2D>("PlayScreen/Water");
            backSelectLevelPic = content.Load<Texture2D>("SelectStage/BackToMainmenu");
            howToPlayPic = content.Load<Texture2D>("PlayScreen/Wind");
            selectStagePic = content.Load<Texture2D>("SelectStage/SelectThatStage");

            //select charactor pic
            CharBGPic = content.Load<Texture2D>("SelectChar/SelectGodBG");
            char1Pic1 = content.Load<Texture2D>("SelectChar/AthenaPickPic");
            char2Pic1 = content.Load<Texture2D>("SelectChar/HermesPickPic");
            char3Pic1 = content.Load<Texture2D>("SelectChar/DionysusPickPic");
            char4Pic1 = content.Load<Texture2D>("SelectChar/HephaestusPickPic");
            char1Pic2 = content.Load<Texture2D>("SelectChar/AthenaPic");
            char2Pic2 = content.Load<Texture2D>("SelectChar/HermesPic");
            char3Pic2 = content.Load<Texture2D>("SelectChar/DionysusPic");
            char4Pic2 = content.Load<Texture2D>("SelectChar/HephaestusPic");
            arrowLeftCharPic = content.Load<Texture2D>("SelectChar/ArrowLeft");
            arrowRightCharPic = content.Load<Texture2D>("SelectChar/ArrowRight");
            backSelectCharPic = content.Load<Texture2D>("SelectChar/BackButton");
            selectCharPic = content.Load<Texture2D>("SelectChar/SelectButton");

            //howtoplay pic
            howToPlayBG1Pic = content.Load<Texture2D>("HowToPlayScreen/HowToPlayBG");
            howToPlayBG2Pic = content.Load<Texture2D>("blackScreen");
            arrowLeftHowToPlayPic = content.Load<Texture2D>("HowToPlayScreen/ArrowLeft");
            arrowRightHowToPlayPic = content.Load<Texture2D>("HowToPlayScreen/ArrowRight");
            backHowToPlayPic = content.Load<Texture2D>("HowToPlayScreen/ContinueButton");

            //confirmQuit pic
            confirmQuitPopUpPic = content.Load<Texture2D>("ConfirmExit/ConfirmQuitPopUp");
            yesConfirmQuitPic1 = content.Load<Texture2D>("ConfirmExit/Yes");
            noConfirmQuitPic1 = content.Load<Texture2D>("ConfirmExit/No");
            yesConfirmQuitPic2 = content.Load<Texture2D>("ConfirmExit/YesGlow");
            noConfirmQuitPic2 = content.Load<Texture2D>("ConfirmExit/NoGlow");

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
            switch (screen)
            {
                case StateScreen.MAINSCREEN:
                    // Click start game
                    if (startButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        if (Singleton.Instance.firsttime)
                        {
                            screen = StateScreen.HOWTOPLAYSCREEN;
                            Singleton.Instance.firsttime = false;
                        }
                        else
                        {
                            screen = StateScreen.SELECTSTAGESCREEN;
                        }
                    }
                    // Click setting
                    if (settingButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        screen = StateScreen.SETTINGSCREEN;
                    }
                    // Click Exit
                    if (exitButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        screen = StateScreen.QUITSCREEN;
                    }
                    break;
                case StateScreen.SELECTSTAGESCREEN:
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
                        screen = StateScreen.MAINSCREEN;
                        Singleton.Instance.levelState = LevelState.NULL;
                    }
                    //selectStage
                    if (selectStageButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        if (Singleton.Instance.levelState != LevelState.NULL)
                        {
                            screen = StateScreen.SELECTCHARSCREEN;
                        }
                    }
                    //how to play
                    if (howToPlayButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        screen = StateScreen.HOWTOPLAYSCREEN;
                    }
                    break;
                case StateScreen.SELECTCHARSCREEN:
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
                        Singleton.Instance.charState = CharState.NULL;
                        charSlide = 2;
                    }
                    if (arrowLeftCharButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        Singleton.Instance.charState = CharState.NULL;
                        charSlide = 1;
                    }

                    //click back
                    if (backSelectCharButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        Singleton.Instance.levelState = LevelState.NULL;
                        Singleton.Instance.charState = CharState.NULL;
                        charSlide = 1;
                        screen = StateScreen.SELECTSTAGESCREEN;
                    }

                    //click play
                    if (selectCharButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        if ((Singleton.Instance.levelState != LevelState.NULL) && (Singleton.Instance.charState != CharState.NULL))
                        {
                            ScreenManager.Instance.LoadScreen(ScreenManager.GameScreenName.PlayScreen);
                        }
                    }
                    break;
                case StateScreen.HOWTOPLAYSCREEN:
                    //click arrow to change how to play
                    if (arrowRightHowToPlayButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        howToPlaySlide = 2;
                    }
                    if (arrowLeftHowToPlayButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        howToPlaySlide = 1;
                    }
                    //click continue
                    if (backHowToPlayButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        Singleton.Instance.levelState = LevelState.NULL;
                        screen = StateScreen.SELECTSTAGESCREEN;
                    }
                    break;
                case StateScreen.SETTINGSCREEN:
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
                            }
                            else if (arrowRightSFXButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                            {
                                Singleton.Instance.sfxState = AudioState.MEDIUM;
                            }
                            break;
                        case AudioState.MEDIUM:
                            if (arrowLeftSFXButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                            {
                                Singleton.Instance.sfxState = AudioState.MUTE;
                            }
                            else if (arrowRightSFXButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                            {
                                Singleton.Instance.sfxState = AudioState.FULL;
                            }
                            break;
                        case AudioState.FULL:
                            if (arrowLeftSFXButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                            {
                                Singleton.Instance.sfxState = AudioState.MEDIUM;
                            }
                            else if (arrowRightSFXButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                            {
                                Singleton.Instance.sfxState = AudioState.MUTE;
                            }
                            break;
                    }

                    // Click back
                    if (backSettingButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        screen = StateScreen.MAINSCREEN;
                    }
                    break;
                case StateScreen.QUITSCREEN:
                    if (noButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        screen = StateScreen.MAINSCREEN;
                    }
                    else if (yesButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        Singleton.Instance.cmdExit = true;
                    }
                    break;
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
                    masterSFX = 0.3f;
                    Singleton.Instance.soundMasterVolume = masterSFX;
                    break;
                case AudioState.FULL:
                    masterSFX = 0.6f;
                    Singleton.Instance.soundMasterVolume = masterSFX;
                    break;
            }
            //update
            SetStageButtonStatus();
            SetCharButtonStatus();

            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            //draw in each screen
            switch (screen)
            {
                case StateScreen.MAINSCREEN:
                    spriteBatch.Draw(backgroundPic, Vector2.Zero, Color.White);//bg
                    spriteBatch.Draw(logoPic, new Vector2(865, 130), new Color(255, 255, 255, 255));
                    startButton.Draw(spriteBatch);
                    settingButton.Draw(spriteBatch);
                    exitButton.Draw(spriteBatch);
                    break;
                case StateScreen.SELECTSTAGESCREEN:
                    spriteBatch.Draw(StageBGPic, Vector2.Zero, Color.White);//bg
                    stage1Button.Draw(spriteBatch);
                    stage2Button.Draw(spriteBatch);
                    stage3Button.Draw(spriteBatch);
                    stageEndlessButton.Draw(spriteBatch);
                    howToPlayButton.Draw(spriteBatch);
                    selectStageButton.Draw(spriteBatch);
                    backSelectLevelButton.Draw(spriteBatch);
                    break;
                case StateScreen.HOWTOPLAYSCREEN:
                    switch (howToPlaySlide)
                    {
                        case 1:
                            spriteBatch.Draw(howToPlayBG1Pic, Vector2.Zero, new Color(255, 255, 255, 210));
                            arrowRightHowToPlayButton.Draw(spriteBatch);
                            break;
                        case 2:
                            spriteBatch.Draw(howToPlayBG2Pic, Vector2.Zero, new Color(255, 255, 255, 210));
                            arrowLeftHowToPlayButton.Draw(spriteBatch);
                            break;
                    }
                    backHowToPlayButton.Draw(spriteBatch);
                    break;
                case StateScreen.SELECTCHARSCREEN:
                    spriteBatch.Draw(CharBGPic, Vector2.Zero, Color.White);//bg

                    //draw if slide 1 or 2
                    switch (charSlide)
                    {
                        case 1:
                            char1Button.SetPosition(new Vector2(215, 165));
                            char2Button.SetPosition(new Vector2(502, 165));
                            char3Button.SetPosition(new Vector2(790, 165));
                            char1Button.Draw(spriteBatch);
                            char2Button.Draw(spriteBatch);
                            char3Button.Draw(spriteBatch);
                            arrowRightCharButton.Draw(spriteBatch);
                            break;
                        case 2:
                            char1Button.SetPosition(new Vector2(0, 0));
                            char2Button.SetPosition(new Vector2(215, 165));
                            char3Button.SetPosition(new Vector2(502, 165));
                            arrowLeftCharButton.Draw(spriteBatch);
                            char2Button.Draw(spriteBatch);
                            char3Button.Draw(spriteBatch);
                            char4Button.Draw(spriteBatch);
                            break;
                    }
                    selectCharButton.Draw(spriteBatch);
                    backSelectCharButton.Draw(spriteBatch);
                    break;
                case StateScreen.SETTINGSCREEN:
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
                    break;
                case StateScreen.QUITSCREEN:
                    spriteBatch.Draw(blackScreenPic, Vector2.Zero, new Color(255, 255, 255, 210));
                    spriteBatch.Draw(confirmQuitPopUpPic, new Vector2((Singleton.Instance.Dimensions.X - confirmQuitPopUpPic.Width) / 2, (Singleton.Instance.Dimensions.Y - confirmQuitPopUpPic.Height) / 2), new Color(255, 255, 255, 255));
                    fontSize = mediumfonts.MeasureString("Are you sure");
                    spriteBatch.DrawString(mediumfonts, "Are you sure", new Vector2((Singleton.Instance.Dimensions.X - fontSize.X) / 2, 255), Color.DarkGray);
                    fontSize = mediumfonts.MeasureString("you want to quit?");
                    spriteBatch.DrawString(mediumfonts, "you want to quit?", new Vector2((Singleton.Instance.Dimensions.X - fontSize.X) / 2, 310), Color.DarkGray);

                    noButton.Draw(spriteBatch, noConfirmQuitPic2);
                    yesButton.Draw(spriteBatch, yesConfirmQuitPic2);
                    break;
            }

            // Draw fade out
            if (!fadeFinish)
            {
                spriteBatch.Draw(blackScreenPic, Vector2.Zero, _Color);
            }
        }
        public void SetStageButtonStatus()
        {
            switch (Singleton.Instance.levelState)
            {
                case LevelState.NULL:
                    stage1Button.SetTexture(stage1Pic1);
                    stage2Button.SetTexture(stage2Pic1);
                    stage3Button.SetTexture(stage3Pic1);
                    stageEndlessButton.SetTexture(stageEndlessPic1);

                    stage1Button.SetCantHover(false);
                    stage2Button.SetCantHover(false);
                    stage3Button.SetCantHover(false);
                    stageEndlessButton.SetCantHover(false);
                    selectStageButton.SetCantHover(true);
                    break;
                case LevelState.POSEIDON:
                    stage1Button.SetTexture(stage1Pic2);
                    stage2Button.SetTexture(stage2Pic1);
                    stage3Button.SetTexture(stage3Pic1);
                    stageEndlessButton.SetTexture(stageEndlessPic1);

                    stage1Button.SetCantHover(true);
                    stage2Button.SetCantHover(false);
                    stage3Button.SetCantHover(false);
                    stageEndlessButton.SetCantHover(false);
                    selectStageButton.SetCantHover(false);
                    break;
                case LevelState.HADES:
                    stage1Button.SetTexture(stage1Pic1);
                    stage2Button.SetTexture(stage2Pic2);
                    stage3Button.SetTexture(stage3Pic1);
                    stageEndlessButton.SetTexture(stageEndlessPic1);

                    stage1Button.SetCantHover(false);
                    stage2Button.SetCantHover(true);
                    stage3Button.SetCantHover(false);
                    stageEndlessButton.SetCantHover(false);
                    selectStageButton.SetCantHover(false);
                    break;
                case LevelState.ZEUS:
                    stage1Button.SetTexture(stage1Pic1);
                    stage2Button.SetTexture(stage2Pic1);
                    stage3Button.SetTexture(stage3Pic2);
                    stageEndlessButton.SetTexture(stageEndlessPic1);

                    stage1Button.SetCantHover(false);
                    stage2Button.SetCantHover(false);
                    stage3Button.SetCantHover(true);
                    stageEndlessButton.SetCantHover(false);
                    selectStageButton.SetCantHover(false);
                    break;
                case LevelState.ENDLESS:
                    stage1Button.SetTexture(stage1Pic1);
                    stage2Button.SetTexture(stage2Pic1);
                    stage3Button.SetTexture(stage3Pic1);
                    stageEndlessButton.SetTexture(stageEndlessPic2);

                    stage1Button.SetCantHover(false);
                    stage2Button.SetCantHover(false);
                    stage3Button.SetCantHover(false);
                    stageEndlessButton.SetCantHover(true);
                    selectStageButton.SetCantHover(false);
                    break;
            }
        }
        public void SetCharButtonStatus()
        {
            switch (Singleton.Instance.charState)
            {
                case CharState.NULL:
                    char1Button.SetTexture(char1Pic1);
                    char2Button.SetTexture(char2Pic1);
                    char3Button.SetTexture(char3Pic1);
                    char4Button.SetTexture(char4Pic1);

                    char1Button.SetCantHover(false);
                    char2Button.SetCantHover(false);
                    char3Button.SetCantHover(false);
                    char4Button.SetCantHover(false);
                    selectCharButton.SetCantHover(true);
                    break;
                case CharState.ATHENA:
                    char1Button.SetTexture(char1Pic2);
                    char2Button.SetTexture(char2Pic1);
                    char3Button.SetTexture(char3Pic1);
                    char4Button.SetTexture(char4Pic1);

                    char1Button.SetCantHover(true);
                    char2Button.SetCantHover(false);
                    char3Button.SetCantHover(false);
                    char4Button.SetCantHover(false);
                    selectCharButton.SetCantHover(false);
                    break;
                case CharState.HERMES:
                    char1Button.SetTexture(char1Pic1);
                    char2Button.SetTexture(char2Pic2);
                    char3Button.SetTexture(char3Pic1);
                    char4Button.SetTexture(char4Pic1);

                    char1Button.SetCantHover(false);
                    char2Button.SetCantHover(true);
                    char3Button.SetCantHover(false);
                    char4Button.SetCantHover(false);
                    selectCharButton.SetCantHover(false);
                    break;
                case CharState.DIONYSUS:
                    char1Button.SetTexture(char1Pic1);
                    char2Button.SetTexture(char2Pic1);
                    char3Button.SetTexture(char3Pic2);
                    char4Button.SetTexture(char4Pic1);

                    char1Button.SetCantHover(false);
                    char2Button.SetCantHover(false);
                    char3Button.SetCantHover(true);
                    char4Button.SetCantHover(false);
                    selectCharButton.SetCantHover(false);
                    break;
                case CharState.HEPHAESTUS:
                    char1Button.SetTexture(char1Pic1);
                    char2Button.SetTexture(char2Pic1);
                    char3Button.SetTexture(char3Pic1);
                    char4Button.SetTexture(char4Pic2);

                    char1Button.SetCantHover(false);
                    char2Button.SetCantHover(false);
                    char3Button.SetCantHover(false);
                    char4Button.SetCantHover(true);
                    selectCharButton.SetCantHover(false);
                    break;
            }
        }
    }
}
