using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Olympuzz.GameObjects;
using Olympuzz.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Olympuzz.Singleton;

namespace Olympuzz.GameScreen
{
    class Stage3Screen : PlayScreen
    {
        //song and sfx
        protected Song zeusTheme;

        private bool isBallHolderDie = false;
        private bool skillActive = false;
        private bool hasSwitched = false;       
        private bool isflash = false;           // flash screen to white when boss active skill 

        private float timeBossSkillActive = 2f; // Time of skill is in Active           // period of active skill
        private float timeCDBossSkillActive = 10f; // Chance of skill that will Active  // Time of next boss skill active 

        private float timeKillBallHolder = 5f;  // period of BallHolder die
        private float timeCDBallHoldkill = 10f; // Time of next BallHolder kill active

        private int switchSkill;

        private Random rand = new Random();

        private Texture2D flashSkill;

        //sound
        private SoundEffect thunderSound;

        public override void Initial()
        {
            //all button
            pauseButton = new Button(pauseButtonPic, new Vector2(86, 55), new Vector2(148, 58));//create button object on playscreen
            timeAttack = 240f;
            _scrollSpd = 4.6f;

            base.Initial();
        }
        public override void LoadContent()
        {
            //ContentManager content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");
            base.LoadContent();

            //stage map
            stageBGPic = content.Load<Texture2D>("Stage3/ZeusStage");
            boardBGPic = content.Load<Texture2D>("Stage3/Board3");

            //all button on playscreen
            pauseButtonPic = content.Load<Texture2D>("Stage3/Pause3");

            //bg music
            zeusTheme = content.Load<Song>("Sounds/ZeusTheme");
            MediaPlayer.Play(zeusTheme);
            //sfx
            thunderSound = content.Load<SoundEffect>("Sounds/ThunderSound");

            Initial();
        }
        public override void UnloadContent()
        {
            base.UnloadContent();
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!notPlay)
            {
                timeAttack -= (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;

                if (!hasSwitched)
                {
                    switchSkill = rand.Next(2);
                    hasSwitched = true;
                }

                if (!dionysusSkilled)
                {
                    switch (switchSkill)
                    {
                        case 0:
                            timeCDBossSkillActive -= (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                            if (timeCDBossSkillActive < 0 && !skillActive)
                            {
                                int chance = rand.Next(3);
                                if (chance == 1)
                                {
                                    isflash = true;
                                    skillActive = true;
                                }
                            }
                            break;

                        case 1:
                            timeCDBallHoldkill -= (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                            if (timeCDBallHoldkill < 0 && !skillActive)
                            {
                                int chance = rand.Next(2);
                                if (chance == 1)
                                {
                                    thunderSound.Play(volume: Singleton.Instance.soundMasterVolume, 0, 0);
                                    isBallHolderDie = true;
                                    skillActive = true;
                                }
                            }
                            break;
                    }
                }

                if (skillActive) // skill was Active now this is not skillCool
                {
                    switch (switchSkill)
                    {
                        case 0:
                            timeBossSkillActive -= (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                            break;

                        case 1:
                            timeKillBallHolder -= (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                            break;
                    }
                }

                if (timeBossSkillActive < 0 || timeKillBallHolder < 0) //if 
                {
                    Singleton.Instance.speed = -1400;
                    isBallHolderDie = false;
                    skillActive = false;
                    hasSwitched = false;
                    isflash = false;
                    timeBossSkillActive = 2f;
                    timeKillBallHolder = 5f;
                    timeCDBossSkillActive = 10;
                    timeCDBallHoldkill = 10;
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bossSkillPic, Vector2.Zero, Color.Black);
            spriteBatch.Draw(boardBGPic, new Vector2(332, 54), Color.White);
            spriteBatch.Draw(stageBGPic, Vector2.Zero, Color.White);

            if (skillCooldown || athenaSkilled)
            {
                switch (Singleton.Instance.charState)
                {
                    case CharState.ATHENA:
                        spriteBatch.Draw(athenaSkillPic, new Vector2(106, 466), Color.White);
                        break;
                    case CharState.HERMES:
                        spriteBatch.Draw(hermesSkillPic, new Vector2(106, 466), Color.White);
                        break;
                    case CharState.DIONYSUS:
                        spriteBatch.Draw(dionysusSkillPic, new Vector2(106, 466), Color.White);
                        break;
                    case CharState.HEPHAESTUS:
                        spriteBatch.Draw(hephaestusSkillPic, new Vector2(106, 466), Color.White);
                        break;
                }
                if (!godSkill.GetAnimationStop())
                {
                    godSkill.Draw(spriteBatch);
                }
            }
            else
            {
                switch (Singleton.Instance.charState)
                {
                    case CharState.ATHENA:
                        spriteBatch.Draw(athenaReadyPic, new Vector2(106, 466), Color.White);
                        break;
                    case CharState.HERMES:
                        spriteBatch.Draw(hermesReadyPic, new Vector2(106, 466), Color.White);
                        break;
                    case CharState.DIONYSUS:
                        spriteBatch.Draw(dionysusSkillPic, new Vector2(106, 466), Color.White);
                        break;
                    case CharState.HEPHAESTUS:
                        spriteBatch.Draw(hephaestusSkillPic, new Vector2(106, 466), Color.White);
                        break;
                }
            }


            if (!isBallHolderDie)
            {
                shooter.GetBubbleNext().Draw(spriteBatch);
                spriteBatch.Draw(holderAlivePic, new Vector2(410, 606), Color.White);
            }
            else
            {
                spriteBatch.Draw(holderDeathPic, new Vector2(410, 606), Color.White);
            }

            shooter.Draw(spriteBatch);
            base.Draw(spriteBatch);

            if (isflash && eventScreen != EventScreen.PAUSE && eventScreen != EventScreen.WIN && eventScreen != EventScreen.LOSE)
            {
                spriteBatch.Draw(bossSkillPic, Vector2.Zero, new Color(255,255,255) * 0.95f);
            }

        }
    }
}
