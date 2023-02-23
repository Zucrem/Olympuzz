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

namespace Olympuzz.GameScreen
{
    class PlayScreen : _GameScreen
    {
        private Texture2D background, blackScreen;
        //private SpriteFont Arial, Arcanista;
        private Bubble[,] bubble = new Bubble[9, 10];
        private Color color;
        private Shooter shooter;
        private Vector2 fontSize;
        private float _timer = 0f;
        private float __timer = 0f;
        private float Timer = 0f;
        private float timerPerUpdate = 0.05f;
        private float tickPerUpdate = 30f;
        private int alpha = 255;
        private bool fadeFinish = false;
        private bool gameOver = false;
        private bool gameWin = false;
        private SoundEffectInstance BubbleSFX_stick, BubbleSFX_dead;
        private SoundEffectInstance Click;


        private Texture2D shooterTexture, baseTexture;
        private readonly Texture2D[] bubleAllTexture = new Texture2D[5];

        //border = 640,684
        
        public void Initial()
        {
            color = new Color(255, 255, 255, alpha);
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 10 - (i % 2); j++)
                {
                    bubble[i, j] = new Bubble(bubleAllTexture)
                    {
                        Name = "Bubble", 
                        Position = new Vector2((j * 50) + ((i % 2) == 0 ? 414 : 439), (i * 45) + 85), // what x cordition is the best 414 or 415
                        IsActive = false,
                    };
                }
            }
            /*Click.Volume = Singleton.Instance.bgMusicVolume;
            BubbleSFX_stick.Volume = Singleton.Instance.bgMusicVolume;
            BubbleSFX_dead.Volume = Singleton.Instance.bgMusicVolume;*/
            shooter = new Shooter(shooterTexture, bubleAllTexture, baseTexture)
            {
                Name = "Shooter",
                Position = new Vector2(Singleton.Instance.Dimensions.X / 2, Singleton.Instance.Dimensions.Y),
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
            background = content.Load<Texture2D>("gud room");
            blackScreen = content.Load<Texture2D>("blackScreen");
            shooterTexture = content.Load<Texture2D>("bow only");
            bubleAllTexture[0] = content.Load<Texture2D>("Earth");
            bubleAllTexture[1] = content.Load<Texture2D>("Fire");
            bubleAllTexture[2] = content.Load<Texture2D>("Thunder");
            bubleAllTexture[3] = content.Load<Texture2D>("Water");
            bubleAllTexture[4] = content.Load<Texture2D>("Wind");
            baseTexture = content.Load<Texture2D>("base");
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
                shooter.Update(gameTime);
                Timer += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                for (int i = 0; i < 8; i++)
                {
                    if (bubble[8, i] != null)
                    {
                        gameOver = true;
                        //Singleton.Instance.BestScore = Singleton.Instance.Score.ToString();
                        //Singleton.Instance.BestTime = Timer.ToString("F");
                    }
                }
                //Check ball flying
                for (int i = 1; i < 9; i++)
                {
                    for (int j = 1; j < 7 - (i % 2); j++)
                    {
                        if (i % 2 != 0)
                        {
                            if (bubble[i - 1, j] == null && bubble[i - 1, j + 1] == null)
                            {
                                bubble[i, j] = null;
                            }
                            if (bubble[i, 1] == null && bubble[i - 1, 0] == null && bubble[i - 1, 1] == null)
                            {
                                bubble[i, 0] = null;
                            }
                            if (bubble[i, 5] == null && bubble[i - 1, 7] == null && bubble[i - 1, 6] == null)
                            {
                                bubble[i, 6] = null;
                            }
                        }
                        else
                        {
                            if (bubble[i - 1, j - 1] == null && bubble[i - 1, j] == null)
                            {
                                bubble[i, j] = null;
                            }
                            if (bubble[i - 1, 0] == null && bubble[i, 1] == null)
                            {
                                bubble[i, 0] = null;
                            }
                            if (bubble[i - 1, 6] == null && bubble[i, 6] == null)
                            {
                                bubble[i, 7] = null;
                            }
                        }
                    }
                }

                __timer += (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                if (__timer >= tickPerUpdate)
                {
                    // Check game over before scroll
                    for (int i = 6; i < 9; i++)
                    {
                        for (int j = 0; j < 8 - (i % 2); j++)
                        {
                            if (bubble[i, j] != null)
                            {
                                gameOver = true;
                                //Singleton.Instance.BestScore = Singleton.Instance.Score.ToString();
                                //Singleton.Instance.BestTime = Timer.ToString("F");
                            }
                        }
                    }
                    // Scroll position 
                    for (int i = 5; i >= 0; i--)
                    {
                        for (int j = 0; j < 8 - (i % 2); j++)
                        {
                            bubble[i + 2, j] = bubble[i, j];
                        }
                    }
                    // Draw new scroll position
                    for (int i = 0; i < 9; i++)
                    {
                        for (int j = 0; j < 8 - (i % 2); j++)
                        {
                            if (bubble[i, j] != null)
                            {
                                bubble[i, j].Position = new Vector2((j * 80) + ((i % 2) == 0 ? 320 : 360), (i * 70) + 40);
                            }
                        }
                    }
                    //Random ball after scroll
                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < 8 - (i % 2); j++)
                        {
                            bubble[i, j] = new Bubble(bubleAllTexture)
                            {
                                Name = "Bubble",
                                Position = new Vector2((j * 80) + ((i % 2) == 0 ? 320 : 360), (i * 70) + 40),
                                IsActive = false,
                            };
                        }
                    }

                    __timer -= tickPerUpdate;
                }

                gameWin = CheckWin(bubble);

            }
            else
            {
                Singleton.Instance.MousePrevious = Singleton.Instance.MouseCurrent;
                Singleton.Instance.MouseCurrent = Mouse.GetState();
                if (Singleton.Instance.MouseCurrent.LeftButton == ButtonState.Pressed && Singleton.Instance.MousePrevious.LeftButton == ButtonState.Released)
                {
                    Singleton.Instance.Score = 0;
                    ScreenManager.Instance.LoadScreen(ScreenManager.GameScreenName.MenuScreen);
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
                    color.A = (byte)alpha;
                }
            }

            shooter.Update(gameTime);

            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            for (int i = 0; i < 9; i++) // Bubble in Even Line
            {
                for (int j = 0; j < 10; j++) //Bubble in Odd Line
                {
                    if (bubble[i, j] != null)
                        bubble[i, j].Draw(spriteBatch);
                }
            }
            shooter.Draw(spriteBatch);

            /*spriteBatch.DrawString(Arcanista, "Score : " + Singleton.Instance.Score, new Vector2(1060, 260), color);
            spriteBatch.DrawString(Arcanista, "Time : " + Timer.ToString("F"), new Vector2(20, 260), color);
            spriteBatch.DrawString(Arcanista, "Next Time : " + (tickPerUpdate - __timer).ToString("F"), new Vector2(20, 210), color);*/

            if (gameOver)
            {
                spriteBatch.Draw(blackScreen, Vector2.Zero, new Color(255, 255, 255, 210));
                /*fontSize = Arial.MeasureString("GameOver !!");
                spriteBatch.DrawString(Arial, "GameOver !!", Singleton.Instance.Dimensions / 2 - fontSize / 2, color);*/
            }

            if (gameWin)
            {
                spriteBatch.Draw(blackScreen, Vector2.Zero, new Color(255, 255, 255, 210));
                /*fontSize = Arial.MeasureString("GameWin !!");
                spriteBatch.DrawString(Arial, "GameWin !!", Singleton.Instance.Dimensions / 2 - fontSize / 2, color);*/
            }

            // Draw fade out
            if (!fadeFinish)
            {
                spriteBatch.Draw(blackScreen, Vector2.Zero, color);
            }
        }
    }
}
