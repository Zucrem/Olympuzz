using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Olympuzz.GameObjects;
using Olympuzz.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework.Media;
using static Olympuzz.Singleton;

namespace Olympuzz.GameScreen
{
    class PlayScreen : _GameScreen
    {
        //all pictures
        protected Texture2D stageBGPic, blackScreenPic, shooterTexture, baseTexture, pauseButtonPic, boardBGPic;
        protected Texture2D arrowLeftBGPic, arrowRightBGPic, arrowLeftSFXPic, arrowRightSFXPic;//sound pic
        protected Texture2D pausePopUpPic, continueButtonPic, restartButtonPic, exitButtonPic, continueButtonPic2, restartButtonPic2, exitButtonPic2;//for pause setting hovering
        protected Texture2D restartWLPic, nextButtonPic, backWLPic, DefeatSignPic, VictorySignPic;//win or lose
        protected Texture2D confirmExitPopUpPic, noConfirmPic1, yesConfirmPic1, noConfirmPic2, yesConfirmPic2;//for confirm
        protected readonly Texture2D[] bubleAllTexture = new Texture2D[5];

        protected Color color;

        //protected SpriteFont Alagan
        protected SpriteFont smallfonts, mediumfonts, bigfonts;//กำหนดชื่อ font
        protected Vector2 fontSize;

        //all playscreen
        protected Bubble[,] bubble = new Bubble[15, 10];

        protected Shooter shooter;
        protected Button pauseButton;
        //button at pause screen
        protected Button continueButton, restartButton, exitButton;
        //button for setting
        protected Button arrowLeftBGButton, arrowRightBGButton, arrowLeftSFXButton, arrowRightSFXButton;//setting button
        //button for confirm exit
        protected Button noConfirmButton, yesConfirmButton;
        //button for win lose
        protected Button nextButton, restartWLButton, exitWLButton;

        //timer
        protected float _timer = 0f;
        protected float _scrollTime = 0f;
        protected float Timer = 0f;
        protected float timerPerUpdate = 0.05f;
        protected float tickPerUpdate = 30f;
        protected float bubbleAngle = 0f;
        protected int alpha = 255;
        protected SoundEffectInstance BubbleSFX_stick, BubbleSFX_dead;
        protected SoundEffectInstance Click;

        //check if go next page or fade finish
        protected bool notPlay = false;
        protected bool isEven = true;
        protected bool fadeFinish = false;

        protected bool confirmExit = false;

        //ตัวแปรของ sound
        protected float masterBGM = Singleton.Instance.bgMusicVolume;
        protected float masterSFX = Singleton.Instance.soundMasterVolume;

        protected EventScreen eventScreen = EventScreen.NULL;
        protected enum EventScreen
        {
            NULL,
            PAUSE,
            WIN,
            LOSE
        }

        public virtual void Initial()
        {
            color = new Color(255, 255, 255, alpha);
            for (int i = 0; i < 5; i++) // if end in 13 line i < 5 || if end in 12 line i < 4
            {
                for (int j = 0; j < 10 - (i % 2); j++)
                {
                    bubble[i, j] = new Bubble(bubleAllTexture)
                    {
                        Name = "Bubble",
                        Position = new Vector2((j * 49) + (isEven ? 363 : 388), (i * 42) + 79), // what x cordition is the best 414 or 415
                        isEven = isEven,
                        IsActive = false,
                    };
                }
                isEven = !isEven;
            }
            /*Click.Volume = Singleton.Instance.bgMusicVolume;
            BubbleSFX_stick.Volume = Singleton.Instance.bgMusicVolume;
            BubbleSFX_dead.Volume = Singleton.Instance.bgMusicVolume;*/

            //all button
            pauseButton = new Button(pauseButtonPic, new Vector2(15, 20), new Vector2(300, 70));//create button object on playscreen
            //create button on pause
            continueButton = new Button(continueButtonPic, new Vector2((Singleton.Instance.Dimensions.X / 2) - 107, 370), new Vector2(215, 50));
            restartButton = new Button(restartButtonPic, new Vector2((Singleton.Instance.Dimensions.X / 2) - 82, 455), new Vector2(165, 50));
            exitButton = new Button(exitButtonPic, new Vector2((Singleton.Instance.Dimensions.X / 2) - 50, 540), new Vector2(100, 50));

            //create button win or lose screen
            nextButton = new Button(nextButtonPic, new Vector2(540, 430), new Vector2(200, 60));//create Button after win
            restartWLButton = new Button(restartWLPic, new Vector2(540, 510), new Vector2(200, 60));//create Button after win
            exitWLButton = new Button(backWLPic, new Vector2(540, 591), new Vector2(200, 60));//create Button after win

            //setting button
            arrowLeftBGButton = new Button(arrowLeftBGPic, new Vector2(570, 200), new Vector2(40, 40));
            arrowRightBGButton = new Button(arrowRightBGPic, new Vector2(675, 200), new Vector2(40, 40));
            arrowLeftSFXButton = new Button(arrowLeftSFXPic, new Vector2(570, 305), new Vector2(40, 40));
            arrowRightSFXButton = new Button(arrowRightSFXPic, new Vector2(675, 305), new Vector2(40, 40));

            //confirm exit button
            yesConfirmButton = new Button(yesConfirmPic1, new Vector2(495, 390), new Vector2(120, 60));
            noConfirmButton = new Button(noConfirmPic1, new Vector2(660, 390), new Vector2(120, 60));

            shooter = new Shooter(shooterTexture, bubleAllTexture, baseTexture)
            {
                Name = "Shooter",
                Position = new Vector2(583,702),
                //_deadSFX = BubbleSFX_dead,
                //_stickSFX = BubbleSFX_stick,
                IsActive = true,
            };
        }

        //public bool CheckWin(Bubble[,] bubble)
        //{
        //    for (int i = 0; i < 9; i++)
        //    {
        //        for (int j = 0; j < 8 - (i % 2); j++)
        //        {
        //            if (bubble[i, j] != null)
        //            {
        //                return false;
        //            }
        //        }
        //    }
        //    return true;
        //}

        public override void LoadContent()
        {
            base.LoadContent();
            //stageBGPic picture add
            blackScreenPic = content.Load<Texture2D>("blackScreen");

            //all object
            shooterTexture = content.Load<Texture2D>("PlayScreen/bow");
            bubleAllTexture[0] = content.Load<Texture2D>("PlayScreen/Earth");
            bubleAllTexture[1] = content.Load<Texture2D>("PlayScreen/Fire");
            bubleAllTexture[2] = content.Load<Texture2D>("PlayScreen/Thunder");
            bubleAllTexture[3] = content.Load<Texture2D>("PlayScreen/Water");
            bubleAllTexture[4] = content.Load<Texture2D>("PlayScreen/Wind");
            
            baseTexture = content.Load<Texture2D>("PlayScreen/base");

            //pause screen
            pausePopUpPic = content.Load<Texture2D>("Pause/pausePopUpBG");
            //all button on pausescreen
            continueButtonPic = content.Load<Texture2D>("Pause/Continue");
            exitButtonPic = content.Load<Texture2D>("Pause/Exit");
            restartButtonPic = content.Load<Texture2D>("Pause/Restart");
            continueButtonPic2 = content.Load<Texture2D>("Pause/ContinueGlow");
            exitButtonPic2 = content.Load<Texture2D>("Pause/ExitGlow");
            restartButtonPic2 = content.Load<Texture2D>("Pause/RestartGlow");

            arrowLeftBGPic = content.Load<Texture2D>("SettingScreen/ArrowButton");
            arrowRightBGPic = content.Load<Texture2D>("SettingScreen/ArrowRButton");
            arrowLeftSFXPic = content.Load<Texture2D>("SettingScreen/ArrowButton");
            arrowRightSFXPic = content.Load<Texture2D>("SettingScreen/ArrowRButton");

            //add button when win or lose
            nextButtonPic = content.Load<Texture2D>("WinLoseScreen/Next");
            restartWLPic = content.Load<Texture2D>("WinLoseScreen/Restart");
            backWLPic = content.Load<Texture2D>("WinLoseScreen/Mainmenu");
            DefeatSignPic = content.Load<Texture2D>("WinLoseScreen/DefeatSign");
            VictorySignPic = content.Load<Texture2D>("WinLoseScreen/VictorySign");

            //confirmExit pic
            confirmExitPopUpPic = content.Load<Texture2D>("ConfirmExit/ConfirmExitPopUp");
            yesConfirmPic1 = content.Load<Texture2D>("ConfirmExit/Yes");
            noConfirmPic1 = content.Load<Texture2D>("ConfirmExit/No");
            yesConfirmPic2 = content.Load<Texture2D>("ConfirmExit/YesGlow");
            noConfirmPic2 = content.Load<Texture2D>("ConfirmExit/NoGlow");

            // Fonts
            smallfonts = content.Load<SpriteFont>("Alagard");
            mediumfonts = content.Load<SpriteFont>("AlagardMedium");
            bigfonts = content.Load<SpriteFont>("AlagardBig");

            //song and sfx
            MediaPlayer.IsRepeating = true;

            Initial();
            
        }
        public override void UnloadContent()
        {
            base.UnloadContent();
        }
        public override void Update(GameTime gameTime)
        {
            MediaPlayer.Volume = Singleton.Instance.bgMusicVolume;
            if (!notPlay)
            {
                //create bubble on the field
                /*for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (bubble[i, j] != null)
                            bubble[i, j].Update(gameTime, bubble);
                    }
                }*/
                
                //shooter.Update(gameTime, bubble);
                shooter.Update(gameTime, bubble);
                
                Timer += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                for (int j = 0; j < 10; j++)
                {
                    if (bubble[12, j] != null)
                    {
                        notPlay = true;
                        eventScreen = EventScreen.LOSE;
                        Singleton.Instance.lastClickTime = (int)gameTime.TotalGameTime.TotalMilliseconds;
                        MediaPlayer.Stop();
                        //Singleton.Instance.BestScore = Singleton.Instance.Score.ToString();
                        //Singleton.Instance.BestTime = Timer.ToString("F");
                    }
                }
                if (pauseButton.IsClicked(Mouse.GetState(), gameTime))
                {
                    notPlay = true;
                    eventScreen = EventScreen.PAUSE;
                    MediaPlayer.Pause();
                }

                _scrollTime += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond * 100;
                if (_scrollTime >= tickPerUpdate)
                {
                    // Check game over before scroll
                    for (int i = 6; i < 13; i++)
                    {
                        for (int j = 0; j < 10 - (isEven ? 0 : 1); j++)
                        {
                            if (bubble[12, j] != null)
                            {
                                notPlay = true;
                                eventScreen = EventScreen.LOSE;
                                MediaPlayer.Stop();
                                //Singleton.Instance.BestScore = Singleton.Instance.Score.ToString();
                                //Singleton.Instance.BestTime = Timer.ToString("F");
                            }
                        }
                    }

                    // Scroll position 
                    for (int i = 11; i >= 0; i--)
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            bubble[i + 1, j] = bubble[i, j];
                            if (bubble[i, j] != null)
                                if (j == (bubble[i, j].isEven ? 9 : 8)) break;
                        }
                    }

                    // Draw new scroll position
                    for (int i = 0; i < 13; i++)
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            if (bubble[i, j] != null)
                            {
                                bubble[i, j].Position = new Vector2((j * 49) + (bubble[i,j].isEven ? 363 : 388), (i * 42) + 79);
                                if (j == (bubble[i, j].isEven ? 9 : 8)) break;
                            }

                        }
                    }

                    //Random ball after scroll
                    for (int i = 0; i < 1; i++)
                    {
                        for (int j = 0; j < 10 - (isEven ? 0 : 1); j++)
                        {
                            bubble[i, j] = new Bubble(bubleAllTexture)
                            {
                                Name = "Bubble",
                                Position = new Vector2((j * 49) + (isEven ? 363 : 388), (i * 42) + 79),
                                isEven = isEven,
                                IsActive = false,
                            };
                        }
                        isEven = !isEven;
                    }

                    _scrollTime -= tickPerUpdate;
                }
                /*notPlay = true;
                gameWin = CheckWin(bubble);*/

            }

            //if in pause, gameover , gamewin
            else if (notPlay)
            {
                pauseButton.SetCantHover(true);
                Singleton.Instance.Shooting = false;

                if (!confirmExit)
                {
                    switch (eventScreen)
                    {
                        case EventScreen.PAUSE:
                            //if click back
                            if (continueButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                            {
                                pauseButton.SetCantHover(false);
                                eventScreen = EventScreen.NULL;
                                MediaPlayer.Resume();
                                notPlay = false;
                            }

                            //if click restart
                            if (restartButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                            {
                                Singleton.Instance.Score = 0;
                                ScreenManager.Instance.LoadScreen(ScreenManager.GameScreenName.PlayScreen);
                            }

                            //if click exit
                            if (exitButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                            {
                                confirmExit = true;
                                MediaPlayer.Pause();
                            }

                            switch (Singleton.Instance.bgmState)
                            {
                                case AudioState.MUTE:
                                    if (arrowLeftBGButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                                    {
                                        Singleton.Instance.bgmState = AudioState.FULL;
                                    }
                                    if (arrowRightBGButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                                    {
                                        Singleton.Instance.bgmState = AudioState.MEDIUM;
                                    }
                                    break;
                                case AudioState.MEDIUM:
                                    if (arrowLeftBGButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                                    {
                                        Singleton.Instance.bgmState = AudioState.MUTE;
                                    }
                                    if (arrowRightBGButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
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
                            break;
                        case EventScreen.WIN:
                            //if go next level
                            if (nextButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                            {
                                MediaPlayer.Stop();
                                switch (Singleton.Instance.levelState)
                                {
                                    case LevelState.POSEIDON:
                                        Singleton.Instance.levelState = LevelState.HADES;
                                        break;
                                    case LevelState.HADES:
                                        Singleton.Instance.levelState = LevelState.ZEUS;
                                        break;
                                }
                                ScreenManager.Instance.LoadScreen(ScreenManager.GameScreenName.PlayScreen);
                            }
                            //if click restart
                            if (restartWLButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                            {
                                Singleton.Instance.Score = 0;
                                ScreenManager.Instance.LoadScreen(ScreenManager.GameScreenName.PlayScreen);
                            }

                            //if click exit
                            if (exitWLButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                            {
                                confirmExit = true;
                                MediaPlayer.Pause();
                            }
                            break;
                        case EventScreen.LOSE:
                            //if click restart
                            if (restartWLButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                            {
                                Singleton.Instance.Score = 0;
                                ScreenManager.Instance.LoadScreen(ScreenManager.GameScreenName.PlayScreen);
                            }

                            //if click exit
                            if (exitWLButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                            {
                                confirmExit = true;
                                MediaPlayer.Pause();
                            }
                            break;
                    }
                }

                else if (confirmExit)
                {
                    if (yesConfirmButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        Singleton.Instance.Score = 0;
                        ScreenManager.Instance.LoadScreen(ScreenManager.GameScreenName.MenuScreen);
                    }
                    if (noConfirmButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        confirmExit = false;
                    }
                }
            }

            if (!fadeFinish)
            {
                _timer += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                // fade out when start
                if (_timer >= timerPerUpdate)
                {
                    alpha -= 5;
                    _timer -= timerPerUpdate;
                    if (alpha <= 5)
                    {
                        fadeFinish = true;
                    }
                    color.A = (byte)alpha;
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

            //all update
            shooter.Update(gameTime, bubble);

            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(boardBGPic, new Vector2(336, 54), Color.White);
            spriteBatch.Draw(stageBGPic, Vector2.Zero, Color.White);

            for (int i = 0; i < 15; i++) // Line of bubble
            {
                for (int j = 0; j < 10; j++) //Bubble in line
                {
                    if (bubble[i, j] != null)
                    { 
                        bubble[i, j].Draw(spriteBatch);
                    }
                }
            }

            pauseButton.Draw(spriteBatch);
            shooter.Draw(spriteBatch);

            /*spriteBatch.DrawString(Arcanista, "Score : " + Singleton.Instance.Score, new Vector2(1060, 260), color);
            spriteBatch.DrawString(Arcanista, "Time : " + Timer.ToString("F"), new Vector2(20, 260), color);
            spriteBatch.DrawString(Arcanista, "Next Time : " + (tickPerUpdate - _scrollTime).ToString("F"), new Vector2(20, 210), color);*/

            if (notPlay)
            {
                spriteBatch.Draw(blackScreenPic, Vector2.Zero, new Color(255, 255, 255, 210));

                //only for if still playing
                if (!confirmExit)
                {
                    switch (eventScreen)
                    {
                        case EventScreen.PAUSE:
                            spriteBatch.Draw(pausePopUpPic, new Vector2((Singleton.Instance.Dimensions.X - pausePopUpPic.Width) / 2, (Singleton.Instance.Dimensions.Y - pausePopUpPic.Height) / 2), new Color(255, 255, 255, 255));
                            fontSize = mediumfonts.MeasureString("Pause");
                            spriteBatch.DrawString(mediumfonts, "Pause", new Vector2(((Singleton.Instance.Dimensions.X - fontSize.X) / 2), 70), new Color(33, 35, 60, 255));
                            //BGM
                            fontSize = mediumfonts.MeasureString("Musics");
                            spriteBatch.DrawString(mediumfonts, "Musics", new Vector2(((Singleton.Instance.Dimensions.X - fontSize.X) / 2), 155), Color.Gray);

                            arrowLeftBGButton.Draw(spriteBatch);
                            arrowRightBGButton.Draw(spriteBatch);

                            //SFX
                            fontSize = mediumfonts.MeasureString("Sounds");
                            spriteBatch.DrawString(mediumfonts, "Sounds", new Vector2(((Singleton.Instance.Dimensions.X - fontSize.X) / 2), 255), Color.Gray);

                            arrowLeftSFXButton.Draw(spriteBatch);
                            arrowRightSFXButton.Draw(spriteBatch);

                            continueButton.Draw(spriteBatch, continueButtonPic2);// Click Arrow BGM button
                            restartButton.Draw(spriteBatch, restartButtonPic2);
                            exitButton.Draw(spriteBatch, exitButtonPic2);
                            switch (Singleton.Instance.bgmState)
                            {
                                case AudioState.MUTE:
                                    fontSize = smallfonts.MeasureString("MUTE");
                                    spriteBatch.DrawString(smallfonts, "MUTE", new Vector2(((Singleton.Instance.Dimensions.X - fontSize.X) / 2) + 5, 208), Color.Gray);
                                    break;
                                case AudioState.MEDIUM:
                                    fontSize = smallfonts.MeasureString("MED");
                                    spriteBatch.DrawString(smallfonts, "MED", new Vector2(((Singleton.Instance.Dimensions.X - fontSize.X) / 2) + 5, 208), Color.Gray);
                                    break;
                                case AudioState.FULL:
                                    fontSize = smallfonts.MeasureString("FULL");
                                    spriteBatch.DrawString(smallfonts, "FULL", new Vector2(((Singleton.Instance.Dimensions.X - fontSize.X) / 2) + 5, 208), Color.Gray);
                                    break;
                            }
                            // Click Arrow SFX button
                            switch (Singleton.Instance.sfxState)
                            {
                                case AudioState.MUTE:
                                    fontSize = smallfonts.MeasureString("MUTE");
                                    spriteBatch.DrawString(smallfonts, "MUTE", new Vector2(((Singleton.Instance.Dimensions.X - fontSize.X) / 2) + 5, 313), Color.Gray);
                                    break;
                                case AudioState.MEDIUM:
                                    fontSize = smallfonts.MeasureString("MED");
                                    spriteBatch.DrawString(smallfonts, "MED", new Vector2(((Singleton.Instance.Dimensions.X - fontSize.X) / 2) + 5, 313), Color.Gray);
                                    break;
                                case AudioState.FULL:
                                    fontSize = smallfonts.MeasureString("FULL");
                                    spriteBatch.DrawString(smallfonts, "FULL", new Vector2(((Singleton.Instance.Dimensions.X - fontSize.X) / 2) + 5, 313), Color.Gray);
                                    break;
                            }
                            break;
                        case EventScreen.LOSE:
                            restartWLButton.SetPosition(new Vector2(540, 430));
                            exitWLButton.SetPosition(new Vector2(540, 510));
                            spriteBatch.Draw(DefeatSignPic, new Vector2(190, 82), new Color(255, 255, 255, 255));
                            restartWLButton.Draw(spriteBatch);
                            exitWLButton.Draw(spriteBatch);
                            break;
                        case EventScreen.WIN:
                            spriteBatch.Draw(VictorySignPic, new Vector2(190, 82), new Color(255, 255, 255, 255));
                            //draw only for Stage1 and Stage2
                            if (Singleton.Instance.levelState == LevelState.POSEIDON || Singleton.Instance.levelState == LevelState.HADES)
                            {
                                restartWLButton.SetPosition(new Vector2(540, 510));
                                exitWLButton.SetPosition(new Vector2(540, 591));
                                nextButton.Draw(spriteBatch);
                                restartWLButton.Draw(spriteBatch);
                                exitWLButton.Draw(spriteBatch);
                            }
                            break;
                    }
                }
                else if (confirmExit)
                {
                    spriteBatch.Draw(confirmExitPopUpPic, new Vector2((Singleton.Instance.Dimensions.X - confirmExitPopUpPic.Width) / 2, (Singleton.Instance.Dimensions.Y - confirmExitPopUpPic.Height) / 2), new Color(255, 255, 255, 255));
                    fontSize = mediumfonts.MeasureString("Exit");
                    spriteBatch.DrawString(mediumfonts, "Exit", new Vector2(((Singleton.Instance.Dimensions.X - fontSize.X) / 2), 190), new Color(33, 35, 60, 255));
                    fontSize = mediumfonts.MeasureString("Are you sure");
                    spriteBatch.DrawString(mediumfonts, "Are you sure", new Vector2((Singleton.Instance.Dimensions.X - fontSize.X) / 2, 270), Color.DarkGray);
                    fontSize = mediumfonts.MeasureString("you want to exit?");
                    spriteBatch.DrawString(mediumfonts, "you want to exit?", new Vector2((Singleton.Instance.Dimensions.X - fontSize.X) / 2, 325), Color.DarkGray);

                    noConfirmButton.Draw(spriteBatch, noConfirmPic2);
                    yesConfirmButton.Draw(spriteBatch, yesConfirmPic2);
                }
            }
            // Draw fade out
            if (!fadeFinish)
            {
                spriteBatch.Draw(blackScreenPic, Vector2.Zero, color);
            }
        }
    }
}
