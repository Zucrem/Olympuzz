using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Olympuzz
{
    internal class Singleton
    {
        public Vector2 Dimensions = new(1280, 720);
        public float bgMusicVolume = 1f;
        public float soundMasterVolume = 0.6f;
        public int Score = 0;
        public bool Shooting = false;
        public List<Vector2> removeBubble = new();
        public bool cmdExit = false;
        public int speed = -700; // -700
        public int lastClickTime = 0;


        //Audio State
        public AudioState bgmState = AudioState.FULL;
        public AudioState sfxState = AudioState.FULL;

        //Level State
        public LevelState levelState = LevelState.NULL;

        //Char State
        public CharState charState = CharState.NULL;

        //Mouse State
        public MouseState MousePrevious, MouseCurrent;

        private static Singleton instance;

        public enum AudioState
        {
            MUTE,
            MEDIUM,
            FULL
        }
        public enum LevelState
        {
            NULL,
            POSEIDON,
            HADES,
            ZEUS,
            ENDLESS
        }
        public enum CharState
        {
            NULL,
            ATHENA,
            HERMES,
            DIONYSUS,
            HEPHAESTUS
        }
        /*public enum SkillState
        {
            MUTE,
            MEDIUM,
            FULL
        }*/
        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Singleton();
                }
                return instance;
            }
        }
    }
}
