﻿using Microsoft.Xna.Framework.Audio;
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

namespace Olympuzz.GameScreen
{
    class PlayScreen : _GameScreen
    {
        //all pictures
        private Texture2D background, blackScreenPic, shooterTexture, baseTexture, pauseButtonPic, board, settingButtonPic, continueButtonPic, restartButtonPic, nextButtonPic, exitButtonPic;
        private Texture2D apply, back, ArrowLeftBGM, ArrowRightBGM, ArrowLeftSFX, ArrowRightSFX;
        private readonly Texture2D[] bubleAllTexture = new Texture2D[5];

        private Color color;
        private Vector2 fontSize;

        //private SpriteFont Arial, Arcanista;

        //all playscreen
        private Bubble[,] bubble = new Bubble[15, 10];
        private Shooter shooter;
        private Button pauseButton;
        //button at pause screen
        private Button settingButton, continueButton, restartButton, nextButton, exitButton;
        //button for setting
        private Button backButton, applySettingButton, arrowLbgmButton, arrowRbgmButton, arrowLsfxButton, arrowRsfxButton;

        //timer
        private float _timer = 0f;
        private float _scrollTime = 0f;
        private float Timer = 0f;
        private float timerPerUpdate = 0.05f;
        private float tickPerUpdate = 30f;
        private int alpha = 255;

        private SoundEffectInstance BubbleSFX_stick, BubbleSFX_dead;
        private SoundEffectInstance Click;

        //check if go next page or fade finish
        private bool notPlay = false;
        private bool pauseEvent = false;
        private bool isEven = true;
        private bool fadeFinish = false;
        private bool gameOver = false;
        private bool gameWin = false;
        //check if go setting
        private bool settingEvent = false;

        //border = 640,684

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
                        Position = new Vector2((j * 49) + (isEven ? 363 : 388), (i * 42) + 105), // what x cordition is the best 414 or 415
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
            //create button object on playscreen
            pauseButton = new Button(pauseButtonPic, new Vector2(100, 100), new Vector2(300, 70));
            //create button on pause and win or lose screen
            continueButton = new Button(continueButtonPic, new Vector2(490, 239), new Vector2(300, 70));
            settingButton = new Button(settingButtonPic, new Vector2(490, 389), new Vector2(300, 70));
            exitButton = new Button(exitButtonPic, new Vector2(490, 600), new Vector2(300, 70));
            restartButton = new Button(restartButtonPic, new Vector2(490, 490), new Vector2(300, 70));
            //create Button after win
            nextButton = new Button(nextButtonPic, new Vector2(490, 239), new Vector2(300, 70));
            //setting button
            //backButton = new Button(back, new Vector2(490, 609), new Vector2(300, 70));
            backButton = new Button(back, new Vector2(800, 609), new Vector2(300, 70));
            arrowLbgmButton = new Button(ArrowLeftBGM, new Vector2(700, 240), new Vector2(70, 50));
            arrowRbgmButton = new Button(ArrowRightBGM, new Vector2(900, 240), new Vector2(70, 50));
            arrowLsfxButton = new Button(ArrowLeftSFX, new Vector2(700, 315), new Vector2(70, 50));
            arrowRsfxButton = new Button(ArrowRightSFX, new Vector2(900, 315), new Vector2(70, 50));
            applySettingButton = new Button(apply, new Vector2(745, 510), new Vector2(300, 70));



            shooter = new Shooter(shooterTexture, bubleAllTexture, baseTexture)
            {
                Name = "Shooter",
                Position = new Vector2(Singleton.Instance.Dimensions.X / 2, 684),
                //_deadSFX = BubbleSFX_dead,
                //_stickSFX = BubbleSFX_stick,
                IsActive = true,
            };
        }

        public bool CheckWin(Bubble[,] bubble)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 8 - (i % 2); j++)
                {
                    if (bubble[i, j] != null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public override void LoadContent()
        {
            base.LoadContent();
            //background picture add
            blackScreenPic = content.Load<Texture2D>("blackScreen");
            background = content.Load<Texture2D>("Stag_1/Poseidon Stage");

            //all object
            shooterTexture = content.Load<Texture2D>("PlayScreen/bow_2 v2");
            bubleAllTexture[0] = content.Load<Texture2D>("PlayScreen/Earth");
            bubleAllTexture[1] = content.Load<Texture2D>("PlayScreen/Fire");
            bubleAllTexture[2] = content.Load<Texture2D>("PlayScreen/Thunder");
            bubleAllTexture[3] = content.Load<Texture2D>("PlayScreen/Water");
            bubleAllTexture[4] = content.Load<Texture2D>("PlayScreen/Wind");

            baseTexture = content.Load<Texture2D>("PlayScreen/base_2 v2");
            board = content.Load<Texture2D>("Stag_1/board");

            //all button on playscreen
            pauseButtonPic = content.Load<Texture2D>("Stag_1/pause but");
            //all button on pausescreen or lose or win
            continueButtonPic = content.Load<Texture2D>("PlayScreen/Earth");
            settingButtonPic = content.Load<Texture2D>("PlayScreen/Wind");
            exitButtonPic = content.Load<Texture2D>("PlayScreen/Water");
            restartButtonPic = content.Load<Texture2D>("PlayScreen/Fire");
            //add button when win
            nextButtonPic = content.Load<Texture2D>("PlayScreen/Fire");
            //all button on setting screen
            apply = content.Load<Texture2D>("PlayScreen/Fire");
            back = content.Load<Texture2D>("PlayScreen/Fire");
            ArrowLeftBGM = content.Load<Texture2D>("PlayScreen/Wind");
            ArrowRightBGM = content.Load<Texture2D>("PlayScreen/Earth");
            ArrowLeftSFX = content.Load<Texture2D>("PlayScreen/Wind");
            ArrowRightSFX = content.Load<Texture2D>("PlayScreen/Earth");

            /*Arial = content.Load<SpriteFont>("Fonts/Arial");
            Arcanista = content.Load<SpriteFont>("Fonts/Arcanista");
            BubbleSFX_dead = content.Load<SoundEffect>("Audios/UI_SoundPack8_Error_v1").CreateInstance();
            BubbleSFX_stick = content.Load<SoundEffect>("Audios/UI_SoundPack11_Select_v14").CreateInstance();
            Click = content.Load<SoundEffect>("Audios/transition t07 two-step 007").CreateInstance();*/
            Initial();
        }
        public override void UnloadContent()
        {
            base.UnloadContent();
        }
        public override void Update(GameTime gameTime)
        {
            if (!gameOver && !gameWin)
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
                        gameOver = true;
                        //Singleton.Instance.BestScore = Singleton.Instance.Score.ToString();
                        //Singleton.Instance.BestTime = Timer.ToString("F");
                    }
                }
                if (pauseButton.IsClicked(Mouse.GetState(), gameTime))
                {
                    notPlay = true;
                    pauseEvent = true;
                }
                //Check ball flying
                //for (int i = 1; i < 9; i++)
                //{
                //    for (int j = 1; j < 7 - (i % 2); j++)
                //    {
                //        if (i % 2 != 0)
                //        {
                //            if (bubble[i - 1, j] == null && bubble[i - 1, j + 1] == null)
                //            {
                //                bubble[i, j] = null;
                //            }
                //            if (bubble[i, 1] == null && bubble[i - 1, 0] == null && bubble[i - 1, 1] == null)
                //            {
                //                bubble[i, 0] = null;
                //            }
                //            if (bubble[i, 5] == null && bubble[i - 1, 7] == null && bubble[i - 1, 6] == null)
                //            {
                //                bubble[i, 6] = null;
                //            }
                //        }
                //        else
                //        {
                //            if (bubble[i - 1, j - 1] == null && bubble[i - 1, j] == null)
                //            {
                //                bubble[i, j] = null;
                //            }
                //            if (bubble[i - 1, 0] == null && bubble[i, 1] == null)
                //            {
                //                bubble[i, 0] = null;
                //            }
                //            if (bubble[i - 1, 6] == null && bubble[i, 6] == null)
                //            {
                //                bubble[i, 7] = null;
                //            }
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
                                bubble[i, j].Position = new Vector2((j * 49) + (bubble[i, j].isEven ? 363 : 388), (i * 42) + 105);

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
                                Position = new Vector2((j * 49) + (isEven ? 363 : 388), (i * 42) + 105),
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
                        color.A = (byte)alpha;
                    }
                }

                shooter.Update(gameTime, bubble);

                base.Update(gameTime);
            }

            //if game over or game win
            if (notPlay)
            {
                if (settingEvent)
                {
                    //if click back in setting page
                    if (backButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        settingEvent = false;
                    }
                }

                else
                {
                    //if still not win or lose
                    if (!gameOver && !gameWin)
                    {
                        //if click back
                        if (continueButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                        {
                            notPlay = false;
                            pauseEvent = false;
                        }
                    }
                    else
                    {
                        //if go next level
                        if (nextButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                        {
                        }
                    }

                    //if click seting
                    if (settingButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        settingEvent = true;
                    }

                    //if click restart
                    if (restartButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        Singleton.Instance.Score = 0;
                        notPlay = false;
                        pauseEvent = false;
                        ScreenManager.Instance.LoadScreen(ScreenManager.GameScreenName.PlayScreen);
                    }

                    //if click exit
                    if (exitButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        Singleton.Instance.Score = 0;
                        notPlay = false;
                        pauseEvent = false;
                        ScreenManager.Instance.LoadScreen(ScreenManager.GameScreenName.MenuScreen);
                    }

                    
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            //background
            spriteBatch.Draw(board, new Vector2(336,54), Color.White);
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            
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
                if (settingEvent)
                {
                    spriteBatch.Draw(blackScreenPic, Vector2.Zero, new Color(255, 255, 255, 210));
                    backButton.Draw(spriteBatch);
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
                else
                {
                    //normal for pause
                    spriteBatch.Draw(blackScreenPic, Vector2.Zero, new Color(255, 255, 255, 210));
                    restartButton.Draw(spriteBatch);
                    settingButton.Draw(spriteBatch);
                    exitButton.Draw(spriteBatch);
                    //only for if still playing
                    if (pauseEvent)
                    {
                        continueButton.Draw(spriteBatch);
                    }
                    //only if gameover
                    if (gameOver)
                    {
                        /*fontSize = Arial.MeasureString("GameOver !!");
                        spriteBatch.DrawString(Arial, "GameOver !!", Singleton.Instance.Dimensions / 2 - fontSize / 2, color);*/
                    }
                    //only if gamewin
                    if (gameWin)
                    {
                        nextButton.Draw(spriteBatch);
                        /*fontSize = Arial.MeasureString("GameWin !!");
                        spriteBatch.DrawString(Arial, "GameWin !!", Singleton.Instance.Dimensions / 2 - fontSize / 2, color);*/
                    }
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
