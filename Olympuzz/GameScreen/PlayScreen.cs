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
        protected Texture2D pausePopUpPic, continueButtonPic, restartButtonPic, exitButtonPic, continueButtonPic2, restartButtonPic2, exitButtonPic2, nextButtonPic;//for pause setting hovering
        protected Texture2D noConfirmPic, yesConfirmPic;
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
        protected Button settingButton, continueButton, restartButton, nextButton, exitButton;
        //button for setting
        protected Button arrowLeftBGButton, arrowRightBGButton, arrowLeftSFXButton, arrowRightSFXButton;//setting button
        //button for confirm exit
        protected Button noConfirmButton, yesConfirmButton;

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
        protected bool pauseEvent = false;
        protected bool isEven = true;
        protected bool fadeFinish = false;
        protected bool gameOver = false;
        protected bool gameWin = false;
        protected bool confirmExit = false;

        //ตัวแปรของ sound
        protected float masterBGM = Singleton.Instance.bgMusicVolume;
        protected float masterSFX = Singleton.Instance.soundMasterVolume;

        public void Initial()
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
            //create button on pause and win or lose screen
            continueButton = new Button(continueButtonPic, new Vector2((Singleton.Instance.Dimensions.X / 2) - 107, 370), new Vector2(215, 50));
            restartButton = new Button(restartButtonPic, new Vector2((Singleton.Instance.Dimensions.X / 2) - 82, 455), new Vector2(165, 50));
            exitButton = new Button(exitButtonPic, new Vector2((Singleton.Instance.Dimensions.X / 2) - 50, 540), new Vector2(100, 50));
            nextButton = new Button(nextButtonPic, new Vector2((Singleton.Instance.Dimensions.X / 2) - 50, 370), new Vector2(100, 50));//create Button after win


            //setting button
            arrowLeftBGButton = new Button(arrowLeftBGPic, new Vector2(570, 200), new Vector2(40, 40));
            arrowRightBGButton = new Button(arrowRightBGPic, new Vector2(675, 200), new Vector2(40, 40));
            arrowLeftSFXButton = new Button(arrowLeftSFXPic, new Vector2(570, 305), new Vector2(40, 40));
            arrowRightSFXButton = new Button(arrowRightSFXPic, new Vector2(675, 305), new Vector2(40, 40));

            //confirm exit button
            yesConfirmButton = new Button(yesConfirmPic, new Vector2(315, 420), new Vector2(300, 70));
            noConfirmButton = new Button(noConfirmPic, new Vector2(685, 420), new Vector2(300, 70));

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
           
            //all button on playscreen
            pauseButtonPic = content.Load<Texture2D>("Stag_1/pause but");

            //pause screen
            pausePopUpPic = content.Load<Texture2D>("Pause/pausePopUpBG");
            //all button on pausescreen or lose or win
            continueButtonPic = content.Load<Texture2D>("Pause/Continue");
            exitButtonPic = content.Load<Texture2D>("Pause/Exit");
            restartButtonPic = content.Load<Texture2D>("Pause/Restart");
            continueButtonPic2 = content.Load<Texture2D>("Pause/ContinueGlow");
            exitButtonPic2 = content.Load<Texture2D>("Pause/ExitGlow");
            restartButtonPic2 = content.Load<Texture2D>("Pause/RestartGlow");

            //add button when win
            nextButtonPic = content.Load<Texture2D>("PlayScreen/Fire");

            arrowLeftBGPic = content.Load<Texture2D>("SettingScreen/ArrowButton");
            arrowRightBGPic = content.Load<Texture2D>("SettingScreen/ArrowRButton");
            arrowLeftSFXPic = content.Load<Texture2D>("SettingScreen/ArrowButton");
            arrowRightSFXPic = content.Load<Texture2D>("SettingScreen/ArrowRButton");

            //confirmExit pic
            yesConfirmPic = content.Load<Texture2D>("PlayScreen/Fire");
            noConfirmPic = content.Load<Texture2D>("PlayScreen/Wind");

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
            if (!gameOver && !gameWin && !notPlay)
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
                        gameOver = true; 
                        notPlay = true;
                        pauseEvent = false;
                        Singleton.Instance.lastClickTime = (int)gameTime.TotalGameTime.TotalMilliseconds;
                        MediaPlayer.Stop();
                        //Singleton.Instance.BestScore = Singleton.Instance.Score.ToString();
                        //Singleton.Instance.BestTime = Timer.ToString("F");
                    }
                }
                if (pauseButton.IsClicked(Mouse.GetState(), gameTime))
                {
                    notPlay = true;
                    pauseEvent = true;
                    MediaPlayer.Pause();
                }

                for (int i = 1; i <= 11; i++)
                {
                    for (int j = 1; j < ((bubble[i, j] != null && bubble[i, j].isEven) ? 9 : 8); j++)
                    {
                        if (bubble[i, j] != null)
                        {
                            bubble[i, j].marked = false;
                        }
                    }
                }

                ////Check ball floating alone
                //for (int i = 1; i <= 11; i++)
                //{
                //    for (int j = 1; j < ((bubble[i, j] != null && bubble[i, j].isEven) ? 9 : 8); j++)
                //    {
                //        if (bubble[i, j] != null && bubble[i, j].isEven && !bubble[i, j].marked)
                //        {
                //            if ((bubble[i - 1, j - 1] == null || bubble[i - 1, j - 1].marked) && (bubble[i - 1, j] == null || bubble[i - 1, j].marked) && (bubble[i, j + 1] == null || bubble[i, j + 1].marked) && (bubble[i, j - 1] == null || bubble[i, j - 1].marked))
                //            {
                //                bubble[i, j] = null;
                //            }
                //            if ((bubble[i - 1, 0] == null || bubble[i - 1, 0].marked) && (bubble[i, 1] == null || bubble[i, 1].marked))
                //            {
                //                bubble[i, 0] = null;
                //            }
                //            if ((bubble[i - 1, 8] == null || bubble[i - 1, 8].marked) && (bubble[i, 8] == null || bubble[i, 8].marked))
                //            {
                //                bubble[i, 9] = null;
                //            }

                //        }
                //        else 
                //        {
                //            if ((bubble[i - 1, j] == null || bubble[i - 1, j].marked) && (bubble[i - 1, j + 1] == null || bubble[i - 1, j + 1].marked) && (bubble[i, j + 1] == null || bubble[i, j + 1].marked) && (bubble[i, j - 1] == null || bubble[i, j - 1].marked))
                //            {
                //                bubble[i, j] = null;
                //            }
                //            if ((bubble[i, 1] == null || bubble[i, 1].marked) && (bubble[i - 1, 0] == null || bubble[i - 1, 0].marked) && (bubble[i - 1, 1] == null || bubble[i - 1, 1].marked))
                //            {
                //                bubble[i, 0] = null;
                //            }
                //            if ((bubble[i, 7] == null || bubble[i, 7].marked) && (bubble[i - 1, 9] == null || bubble[i - 1, 9].marked) && (bubble[i - 1, 8] == null || bubble[i - 1, 8].marked))
                //            {
                //                bubble[i, 8] = null;
                //            }
                //        }

                //        if (bubble[i, j] != null)
                //        {
                //            bubble[i, j].marked = true;
                //        }

                //    }
                //}
                
                _scrollTime += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
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
                                gameOver = true;
                                pauseEvent = false;
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
            if (notPlay)
            {
                pauseButton.setCantHover(true);
                Singleton.Instance.Shooting = false;

                // Click Arrow BGM buttonif (pauseEvent)
                if (pauseEvent)
                {
                    //if click back
                    if (continueButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        notPlay = false;
                        pauseButton.setCantHover(false);
                        pauseEvent = false;
                        MediaPlayer.Resume();
                    }
                }
                //only if gamewin
                if (gameWin)
                {
                    //if go next level
                    if (nextButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        notPlay = false;
                        pauseEvent = false;
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
                }
                if (!confirmExit)
                {
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
                    //if click restart
                    if (restartButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        Singleton.Instance.Score = 0;
                        notPlay = false;
                        pauseEvent = false;
                        MediaPlayer.Stop();
                        ScreenManager.Instance.LoadScreen(ScreenManager.GameScreenName.PlayScreen);
                    }

                    //if click exit
                    if (exitButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        //settingEvent = false;
                        confirmExit = true;
                        MediaPlayer.Pause();
                    }
                }

                else if (confirmExit)
                {
                    if (yesConfirmButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        Singleton.Instance.Score = 0;
                        notPlay = false;
                        pauseEvent = false;
                        MediaPlayer.Stop();
                        ScreenManager.Instance.LoadScreen(ScreenManager.GameScreenName.MenuScreen);
                    }
                    else if (noConfirmButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
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
                    masterSFX = 0.1f;
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

            shooter.Draw(spriteBatch);
            pauseButton.Draw(spriteBatch);

            /*spriteBatch.DrawString(Arcanista, "Score : " + Singleton.Instance.Score, new Vector2(1060, 260), color);
            spriteBatch.DrawString(Arcanista, "Time : " + Timer.ToString("F"), new Vector2(20, 260), color);
            spriteBatch.DrawString(Arcanista, "Next Time : " + (tickPerUpdate - _scrollTime).ToString("F"), new Vector2(20, 210), color);*/

            if (notPlay)
            {
                spriteBatch.Draw(blackScreenPic, Vector2.Zero, new Color(255, 255, 255, 210));
                spriteBatch.Draw(pausePopUpPic, new Vector2((Singleton.Instance.Dimensions.X - pausePopUpPic.Width) / 2, (Singleton.Instance.Dimensions.Y - pausePopUpPic.Height) / 2), new Color(255, 255, 255, 255));
                if (pauseEvent)
                {
                    fontSize = mediumfonts.MeasureString("Pause");
                    spriteBatch.DrawString(mediumfonts, "Pause", new Vector2(((Singleton.Instance.Dimensions.X - fontSize.X) / 2), 70), Color.DarkGray);
                }
                //only if gameover
                if (gameOver)
                {
                    fontSize = mediumfonts.MeasureString("Lose");
                    spriteBatch.DrawString(mediumfonts, "Lose", new Vector2(((Singleton.Instance.Dimensions.X - fontSize.X) / 2), 70), Color.DarkGray);
                }
                //only if gamewin
                if (gameWin)
                {
                    fontSize = mediumfonts.MeasureString("Win");
                    spriteBatch.DrawString(mediumfonts, "Win", new Vector2((Singleton.Instance.Dimensions.X - fontSize.X) / 2, 70), Color.DarkGray);
                }

                //only for if still playing
                if (!confirmExit)
                {
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
                    // Click Arrow BGM button
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
                    restartButton.Draw(spriteBatch, restartButtonPic2);
                    exitButton.Draw(spriteBatch, exitButtonPic2);

                    //normal for pause
                    if (pauseEvent)
                    {
                        continueButton.Draw(spriteBatch, continueButtonPic2);
                    }
                    //only if gamewin
                    if (gameWin)
                    {
                        switch (Singleton.Instance.levelState)
                        {
                            case LevelState.POSEIDON:
                                nextButton.Draw(spriteBatch);
                                break;
                            case LevelState.HADES:
                                nextButton.Draw(spriteBatch);
                                break;
                            case LevelState.ZEUS:
                                fontSize = mediumfonts.MeasureString("World is save!");
                                spriteBatch.DrawString(mediumfonts, "World is save!", new Vector2(((Singleton.Instance.Dimensions.X - fontSize.X) / 2), 370), Color.Gray);
                                break;
                        }
                    }
                }
                else if (confirmExit)
                {
                    fontSize = mediumfonts.MeasureString("Are you sure");
                    spriteBatch.DrawString(mediumfonts, "Are you sure", new Vector2((Singleton.Instance.Dimensions.X - fontSize.X) / 2, 165), Color.DarkGray);
                    fontSize = mediumfonts.MeasureString("you want to exit?");
                    spriteBatch.DrawString(mediumfonts, "you want to exit?", new Vector2((Singleton.Instance.Dimensions.X - fontSize.X) / 2, 260), Color.DarkGray);

                    noConfirmButton.Draw(spriteBatch);
                    yesConfirmButton.Draw(spriteBatch); 
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
