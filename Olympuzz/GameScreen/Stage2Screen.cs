using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Olympuzz.GameObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Olympuzz.Singleton;

namespace Olympuzz.GameScreen
{
    class Stage2Screen : PlayScreen
    {
        //hades skill texture
        protected Texture2D athenaSkillPic2, athenaReadyPic2, hermesSkillPic2, hermesReadyPic2, dionysusSkillPic2, dionysusReadyPic2, hephaestusSkillPic2, hephaestusReadyPic2;//all god skill texture
        //song and sfx
        protected Song hadesTheme;

        private bool skillActive = false;

        private float skillTime1 = 1f; // Time of skill is in Active
        private float bossSkillChance1 = 10f; // Chance of skill that will Active

        private Random rand = new Random();

        public override void Initial()
        {
            //all button
            pauseButton = new Button(pauseButtonPic, new Vector2(89, 50), new Vector2(148, 60));//create button object on playscreen

            _scrollSpd = 4f;
            
            timeAttack = 180f;
            base.Initial();
        }
        public override void LoadContent()
        {
            base.LoadContent();

            //stage map
            stageBGPic = content.Load<Texture2D>("Stage2/HadesStage");
            boardBGPic = content.Load<Texture2D>("Stage2/Board2");
            //all button on playscreen
            pauseButtonPic = content.Load<Texture2D>("Stage2/Pause2");

            //all good god skill
            athenaSkillPic2 = content.Load<Texture2D>("GodSkill/AthenaSkill2");
            athenaReadyPic2 = content.Load<Texture2D>("GodSkill/AthenaReady2");
            hermesSkillPic2 = content.Load<Texture2D>("GodSkill/HermesSkill2");
            hermesReadyPic2 = content.Load<Texture2D>("GodSkill/HermesReady2");
            dionysusSkillPic2 = content.Load<Texture2D>("GodSkill/DionysusSkill2");
            dionysusReadyPic2 = content.Load<Texture2D>("GodSkill/DionysusReady2");
            hephaestusSkillPic2 = content.Load<Texture2D>("GodSkill/HephaestusSkill2");
            hephaestusReadyPic2 = content.Load<Texture2D>("GodSkill/HephaestusReady2");

            //bg music
            hadesTheme = content.Load<Song>("Sounds/HadesTheme");
            MediaPlayer.Play(hadesTheme);

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

                if (!dionysusSkilled)
                {
                    bossSkillChance1 -= (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                    if (bossSkillChance1 < 0 && !skillActive)
                    {
                        int chance = rand.Next(3);
                        if (chance == 1)
                        {
                            isHell = true;
                            skillActive = true;
                        }
                    }
                }
                

                if (skillActive) // skill was Active now this is not skillCool
                {
                    skillTime1 -= (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                }

                if (skillTime1 < 0 ) //if 
                {
                    Singleton.Instance.speed = -1400;
                    isHell = false;
                    skillActive = false;
                    skillTime1 = 1;
                    bossSkillChance1 = 10;
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bossSkillPic, Vector2.Zero, Color.Black);
            spriteBatch.Draw(boardBGPic, new Vector2(332, 54), Color.White);
            spriteBatch.Draw(stageBGPic, Vector2.Zero, Color.White);


            if (dionysusSkilled)
            {
                shooter.GetBubbleNext().Draw(spriteBatch);
                spriteBatch.Draw(holderAlivePic, new Vector2(410, 606), Color.White);
            }
            else
            {
                spriteBatch.Draw(holderDeathPic, new Vector2(410, 606), Color.White);
            }

            if (skillCooldown || athenaSkilled)
            {
                switch (Singleton.Instance.charState)
                {
                    case CharState.ATHENA:
                        spriteBatch.Draw(athenaSkillPic2, new Vector2(106, 466), Color.White);
                        break;
                    case CharState.HERMES:
                        spriteBatch.Draw(hermesSkillPic2, new Vector2(106, 466), Color.White);
                        break;
                    case CharState.DIONYSUS:
                        spriteBatch.Draw(dionysusSkillPic2, new Vector2(106, 466), Color.White);
                        break;
                    case CharState.HEPHAESTUS:
                        spriteBatch.Draw(hephaestusSkillPic2, new Vector2(106, 466), Color.White);
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
                        spriteBatch.Draw(athenaReadyPic2, new Vector2(106, 466), Color.White);
                        break;
                    case CharState.HERMES:
                        spriteBatch.Draw(hermesReadyPic2, new Vector2(106, 466), Color.White);
                        break;
                    case CharState.DIONYSUS:
                        spriteBatch.Draw(dionysusSkillPic2, new Vector2(106, 466), Color.White);
                        break;
                    case CharState.HEPHAESTUS:
                        spriteBatch.Draw(hephaestusSkillPic2, new Vector2(106, 466), Color.White);
                        break;
                }
            }

            shooter.Draw(spriteBatch); 
            base.Draw(spriteBatch);

            if (isHell && eventScreen != EventScreen.PAUSE && eventScreen != EventScreen.WIN && eventScreen != EventScreen.LOSE)
            {
                spriteBatch.Draw(bossSkillPic, Vector2.Zero, Color.Red * 0.2f);
            }
        }
    }
}
