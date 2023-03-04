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
        public float soundMasterVolume = 1f;
        public int Score = 0;
        public bool Shooting = false;
        public List<Vector2> removeBubble = new();
        public bool cmdExit = false;
        public int speed = -700; // -700
        public int lastClickTime = 0;

        public AudioState bgmState = AudioState.FULL;
        public AudioState sfxState = AudioState.FULL;


        public MouseState MousePrevious, MouseCurrent;

        private static Singleton instance;

        public enum AudioState
        {
            MUTE,
            MEDIUM,
            FULL
        }
        public static Singleton Instance
        {
            get
            {
#pragma warning disable IDE0074 // Use compound assignment
                if (instance == null)
                {
                    instance = new Singleton();
                }
#pragma warning restore IDE0074 // Use compound assignment
                return instance;
            }
        }
    }
}
