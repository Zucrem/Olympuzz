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
        private Texture2D bgLevel1, blackScreenPic, shooterTexture, baseTexture, pauseButtonPic, board, settingButtonPic, continueButtonPic, restartButtonPic, nextButtonPic, exitButtonPic;
        private Texture2D bgmMuteSoundPic, bgmMediumSoundPic, bgmFullSoundPic, sfxMuteSoundPic, sfxMediumSoundPic, sfxFullSoundPic, backPic;
        private Texture2D bgmMuteSoundPic2, bgmMediumSoundPic2, bgmFullSoundPic2, sfxMuteSoundPic2, sfxMediumSoundPic2, sfxFullSoundPic2;//for setting button not hovering
        private Texture2D noConfirmPic, yesConfirmPic;
        private readonly Texture2D[] bubleAllTexture = new Texture2D[5];

        private Color color;

        //private SpriteFont Alagan
        private SpriteFont smallfonts, mediumfonts, bigfonts;//กำหนดชื่อ font
        private Vector2 fontSize;

        //all playscreen
        private Bubble[,] bubble = new Bubble[15, 10];
        private Shooter shooter;
        private Button pauseButton;
        //button at pause screen
        private Button settingButton, continueButton, restartButton, nextButton, exitButton;
        //button for setting
        private Button bgmMuteSoundButton, bgmMediumSoundButton, bgmFullSoundButton, sfxMuteSoundButton, sfxMediumSoundButton, sfxFullSoundButton, backButton;
        //button for confirm exit
        private Button noConfirmButton, yesConfirmButton;

        //timer
        private float _timer = 0f;
        private float _scrollTime = 0f;
        private float Timer = 0f;
        private float timerPerUpdate = 0.05f;
        private float tickPerUpdate = 30f;
        private int alpha = 255;

        //song and sfx
        private Song poseidonTheme;

        private SoundEffectInstance BubbleSFX_stick, BubbleSFX_dead;
        private SoundEffectInstance Click;

        //check if go next page or fade finish
        private bool notPlay = false;
        private bool pauseEvent = false;
        private bool isEven = true;
        private bool fadeFinish = false;
        private bool gameOver = false;
        private bool gameWin = false;
        private bool confirmExit = false;

        //check if go setting
        private bool settingEvent = false;
        //ตัวแปรของ sound
        private float masterBGM = Singleton.Instance.bgMusicVolume;
        private float masterSFX = Singleton.Instance.soundMasterVolume;

        //select level
        private int level = 1;

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
            continueButton = new Button(continueButtonPic, new Vector2(490, 250), new Vector2(300, 70));
            restartButton = new Button(restartButtonPic, new Vector2(490, 360), new Vector2(300, 70));
            settingButton = new Button(settingButtonPic, new Vector2(490, 470), new Vector2(300, 70));
            exitButton = new Button(exitButtonPic, new Vector2(490, 580), new Vector2(300, 70));
            nextButton = new Button(nextButtonPic, new Vector2(490, 250), new Vector2(300, 70));//create Button after win

            //setting button
            bgmMuteSoundButton = new Button(bgmMuteSoundPic, new Vector2(475, 250), new Vector2(175, 50));
            bgmMediumSoundButton = new Button(bgmMediumSoundPic, new Vector2(650, 250), new Vector2(175, 50));
            bgmFullSoundButton = new Button(bgmFullSoundPic, new Vector2(825, 250), new Vector2(175, 50));
            sfxMuteSoundButton = new Button(sfxMuteSoundPic, new Vector2(475, 430), new Vector2(175, 50));
            sfxMediumSoundButton = new Button(sfxMediumSoundPic, new Vector2(650, 430), new Vector2(175, 50));
            sfxFullSoundButton = new Button(sfxFullSoundPic, new Vector2(825, 430), new Vector2(175, 50));

            backButton = new Button(backPic, new Vector2(490, 609), new Vector2(300, 70));

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
            //bgLevel1 picture add
            blackScreenPic = content.Load<Texture2D>("blackScreen");
            //lvl 1 map
            bgLevel1 = content.Load<Texture2D>("Stag_1/Poseidon Stage");
            board = content.Load<Texture2D>("Stag_1/board");

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
            
            //all button on pausescreen or lose or win
            continueButtonPic = content.Load<Texture2D>("PlayScreen/Earth");
            settingButtonPic = content.Load<Texture2D>("PlayScreen/Wind");
            exitButtonPic = content.Load<Texture2D>("PlayScreen/Water");
            restartButtonPic = content.Load<Texture2D>("PlayScreen/Fire");
            
            //add button when win
            nextButtonPic = content.Load<Texture2D>("PlayScreen/Fire");

            //setting button
            bgmMuteSoundPic = content.Load<Texture2D>("PlayScreen/Fire");
            bgmMediumSoundPic = content.Load<Texture2D>("PlayScreen/Wind");
            bgmFullSoundPic = content.Load<Texture2D>("PlayScreen/Earth");
            sfxMuteSoundPic = content.Load<Texture2D>("PlayScreen/Fire");
            sfxMediumSoundPic = content.Load<Texture2D>("PlayScreen/Wind");
            sfxFullSoundPic = content.Load<Texture2D>("PlayScreen/Earth");
            backPic = content.Load<Texture2D>("PlayScreen/Water");

            //if it cant hovering
            bgmMuteSoundPic2 = content.Load<Texture2D>("PlayScreen/Water");
            bgmMediumSoundPic2 = content.Load<Texture2D>("PlayScreen/Water");
            bgmFullSoundPic2 = content.Load<Texture2D>("PlayScreen/Water");
            sfxMuteSoundPic2 = content.Load<Texture2D>("PlayScreen/Water");
            sfxMediumSoundPic2 = content.Load<Texture2D>("PlayScreen/Water");
            sfxFullSoundPic2 = content.Load<Texture2D>("PlayScreen/Water");

            //confirmExit pic
            yesConfirmPic = content.Load<Texture2D>("PlayScreen/Fire");
            noConfirmPic = content.Load<Texture2D>("PlayScreen/Wind");

            // Fonts
            smallfonts = content.Load<SpriteFont>("Alagard");
            mediumfonts = content.Load<SpriteFont>("AlagardMedium");
            bigfonts = content.Load<SpriteFont>("AlagardBig");

            //song and sfx
            poseidonTheme = content.Load<Song>("Stag_1/PoseidonTheme");
            MediaPlayer.IsRepeating = true;

            switch (level)
            {
                case 1:
                    MediaPlayer.Play(poseidonTheme);
                    break;
            }

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
                if (settingEvent)
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

                    //if click back in setting page
                    if (backButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        settingEvent = false;
                    }
                }
                else if (confirmExit)
                {
                    if (yesConfirmButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        Singleton.Instance.Score = 0;
                        notPlay = false;
                        pauseEvent = false;
                        settingEvent = false;
                        MediaPlayer.Stop();
                        ScreenManager.Instance.LoadScreen(ScreenManager.GameScreenName.MenuScreen);
                    }
                    else if (noConfirmButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                    {
                        confirmExit = false;
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
                            pauseButton.setCantHover(false);
                            pauseEvent = false;
                            MediaPlayer.Resume();
                        }
                    }
                    else
                    {
                        //if go next level
                        if (nextButton.IsClicked(Singleton.Instance.MouseCurrent, gameTime))
                        {
                            if (level < 4)
                            {
                                notPlay = false;
                                pauseEvent = false;
                                settingEvent = false;
                                level += 1;
                                MediaPlayer.Stop();
                                ScreenManager.Instance.LoadScreen(ScreenManager.GameScreenName.PlayScreen);
                            }

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
                        settingEvent = false;
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

            //all update
            //set sound status everytime
            setSoundStatus();

            shooter.Update(gameTime, bubble);

            //update button
            continueButton.Update(gameTime);
            nextButton.Update(gameTime);
            settingButton.Update(gameTime);
            restartButton.Update(gameTime);
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
            switch (level)
            {
                case 1://bgLevel1
                    spriteBatch.Draw(board, new Vector2(336, 54), Color.White);
                    spriteBatch.Draw(bgLevel1, Vector2.Zero, Color.White);
                    break;
                case 2://bgLevel2
                    //spriteBatch.Draw(board, new Vector2(336, 54), Color.White);
                    //spriteBatch.Draw(bgLevel1, Vector2.Zero, Color.White);
                    break;
                case 3://bgLevel3
                    //spriteBatch.Draw(board, new Vector2(336, 54), Color.White);
                    //spriteBatch.Draw(bgLevel1, Vector2.Zero, Color.White);
                    break;
            }

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
                if (settingEvent)
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
                else if (confirmExit)
                {
                    fontSize = smallfonts.MeasureString("Are you sure");
                    spriteBatch.DrawString(bigfonts, "Are you sure", new Vector2(((Singleton.Instance.Dimensions.X - fontSize.X) / 2) - 90, 165), Color.White);
                    fontSize = smallfonts.MeasureString("you want to Exit?");
                    spriteBatch.DrawString(bigfonts, "you want to Exit?", new Vector2(((Singleton.Instance.Dimensions.X - fontSize.X) / 2) - 100, 260), Color.White);

                    noConfirmButton.Draw(spriteBatch);
                    yesConfirmButton.Draw(spriteBatch);
                }
                else
                {
                    //normal for pause
                    restartButton.Draw(spriteBatch);
                    settingButton.Draw(spriteBatch);
                    exitButton.Draw(spriteBatch);
                    //only for if still playing
                    if (pauseEvent)
                    {
                        fontSize = bigfonts.MeasureString("Pause");
                        spriteBatch.DrawString(bigfonts, "Pause", new Vector2(555, 130), color);
                        continueButton.Draw(spriteBatch);
                    }
                    //only if gameover
                    else if (gameOver)
                    {
                        fontSize = bigfonts.MeasureString("GameOver");
                        spriteBatch.DrawString(bigfonts, "GameOver", new Vector2(547, 130), color);
                    }
                    //only if gamewin
                    else if (gameWin)
                    {
                        nextButton.Draw(spriteBatch);
                        fontSize = bigfonts.MeasureString("GameWin");
                        spriteBatch.DrawString(bigfonts, "GameWin", new Vector2(547, 130), color);
                    }
                }
            }

            // Draw fade out
            if (!fadeFinish)
            {
                spriteBatch.Draw(blackScreenPic, Vector2.Zero, color);
            }
        }

        //set Audio Button for Setting
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
