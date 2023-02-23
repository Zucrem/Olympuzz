using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Olympuzz.GameScreen;
using System.Diagnostics;

namespace Olympuzz.Managers
{
    public class ScreenManager
    {
        public ContentManager Content { private set; get; }
        public enum GameScreenName
        {
            MenuScreen,
            PlayScreen
        }
        //object gamescreen
        private _GameScreen currentScreen;

        //constructer ให้ current state == หน้าโหลด or หน้าปิด
        public ScreenManager()
        {
            currentScreen = new SplashScreen();
        }
        public void LoadScreen(GameScreenName screenName)
        {
            switch (screenName)
            {
                //ถ้าไปหน้าmenu
                case GameScreenName.MenuScreen:
                    currentScreen = new MenuScreen();
                    break;
                //ถ้าไปหน้าเล่นเกม
                case GameScreenName.PlayScreen:
                    currentScreen = new PlayScreen();
                    break;
            }
            currentScreen.LoadContent();
        }
        public void LoadContent(ContentManager Content)
        {
            this.Content = new ContentManager(Content.ServiceProvider, "Content");
            currentScreen.LoadContent();
        }
        public void UnloadContent()
        {
            currentScreen.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            currentScreen.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            currentScreen.Draw(spriteBatch);
        }
        private static ScreenManager instance;
        public static ScreenManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new ScreenManager();
                return instance;
            }
        }
    }
}
