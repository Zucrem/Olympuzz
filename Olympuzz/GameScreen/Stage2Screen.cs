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

namespace Olympuzz.GameScreen
{
    class Stage2Screen : PlayScreen
    {
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
            spriteBatch.Draw(bossSkill, Vector2.Zero, Color.Black);
            spriteBatch.Draw(boardBGPic, new Vector2(332, 54), Color.White);
            spriteBatch.Draw(stageBGPic, Vector2.Zero, Color.White);

            spriteBatch.Draw(holderDeathPic, new Vector2(410, 606), Color.White);

            shooter.Draw(spriteBatch); 
            base.Draw(spriteBatch);

            if (isHell && eventScreen != EventScreen.PAUSE && eventScreen != EventScreen.WIN && eventScreen != EventScreen.LOSE)
            {
                spriteBatch.Draw(bossSkill, Vector2.Zero, Color.Red * 0.2f);
            }
        }
    }
}
