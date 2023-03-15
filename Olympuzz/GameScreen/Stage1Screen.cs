using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Olympuzz.GameObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olympuzz.GameScreen
{
    class Stage1Screen : PlayScreen
    {
        //song and sfx
        private Song poseidonTheme;

        private bool isBallHolderDie = false;
        private bool skillActive = false;
        private bool hasSwitched = false;
        private bool isWave = false;

        private float skillTime1 = 10f; // Time of skill is in Active
        private float bossSkillChance1 = 5f; // Chance of skill that will Active
        
        private float skillTime2 = 5f;
        private float bossSkillChance2 = 10f;

        private int switchSkill;

        private Random rand = new Random();

        private Color waveColor;

        private Texture2D waveSkill;


        public override void Initial()
        {
            //all button
            pauseButton = new Button(pauseButtonPic, new Vector2(98, 50), new Vector2(148, 60));//create button object on playscreen
            waveColor = new Color(58, 172, 212);
            base.Initial();
        }
        public override void LoadContent()
        {
            base.LoadContent();

            //stage map
            stageBGPic = content.Load<Texture2D>("Stage1/PoseidonStage");
            boardBGPic = content.Load<Texture2D>("Stage1/Board1");
            //all button on playscreen
            pauseButtonPic = content.Load<Texture2D>("Stage1/Pause1");

            waveSkill = content.Load<Texture2D>("Stage3/skill");

            //bg music
            poseidonTheme = content.Load<Song>("Sounds/PoseidonTheme");
            MediaPlayer.Play(poseidonTheme);

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

                if (!hasSwitched)
                {
                    switchSkill = rand.Next(2);
                    hasSwitched = true;
                }
                
                switch (switchSkill)
                {
                    case 0:
                        bossSkillChance1 -= (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                        if (bossSkillChance1 < 0 && !skillActive)
                        {
                            int chance = rand.Next(3);
                            if (chance == 1)
                            {
                                Singleton.Instance.speed = -300;
                                isWave = true;
                                skillActive = true;
                            }
                        }
                        break;

                    case 1:
                        bossSkillChance2 -= (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                        if (bossSkillChance2 < 0 && !skillActive)
                        {
                            int chance = rand.Next(2);
                            if (chance == 1)
                            {
                                isBallHolderDie = true;
                                skillActive = true;
                            }
                        }
                        break;
                }


                if (skillActive) // skill was Active now this is not skillCool
                {
                    switch (switchSkill)
                    {
                        case 0:
                            skillTime1 -= (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                            break;
                        
                        case 1:
                            skillTime2 -= (float)gameTime.ElapsedGameTime.Ticks / TimeSpan.TicksPerSecond;
                            break;
                    }
                }

                if (skillTime1 < 0 || skillTime2 < 0) //if 
                {
                    Singleton.Instance.speed = -1400;
                    isBallHolderDie = false;
                    skillActive = false;
                    hasSwitched = false;
                    isWave = false;
                    skillTime1 = 10f;
                    skillTime2 = 5f;
                    bossSkillChance1 = 5;
                    bossSkillChance2 = 10;
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(boardBGPic, new Vector2(336, 54), Color.White);
            spriteBatch.Draw(stageBGPic, Vector2.Zero, Color.White);

            if (!isBallHolderDie)
            {
                spriteBatch.Draw(HolderAlivePic, new Vector2(410, 606), Color.White);
            }
            else
            {
                spriteBatch.Draw(HolderDeathPic, new Vector2(410, 606), Color.White);
            }

            shooter.Draw(spriteBatch, isBallHolderDie);
            base.Draw(spriteBatch);
            if (isWave)
            {
                spriteBatch.Draw(waveSkill, Vector2.Zero, waveColor * 0.2f);
            }
        }
    }
}
