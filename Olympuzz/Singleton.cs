﻿using Microsoft.Xna.Framework;
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
        public int bgMusicVolume = 100;
        public int Score = 0;
        public float soundMasterVolume = 1f;
        public bool Shooting = false;
        public List<Vector2> removeBubble = new();
        public bool cmdExit = false; //cmdFullScreen = false, cmdShowFPS = false;
        public bool IsFullScreen;
        public int speed = -700;
        //public string BestTime, BestScore;

        public MouseState MousePrevious, MouseCurrent;

        private static Singleton instance;
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
