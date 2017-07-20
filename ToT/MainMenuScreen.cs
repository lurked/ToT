using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ToT
{
    public class MainMenuScreen : GameScreen
    {
        private Vector2 logoPosition;
        //private Menu mainMenu;
        //private int mainMenuIndex;

        public override void LoadAssets()
        {
            base.LoadAssets();
            
            logoPosition = new Vector2(10, 10);
            //mainMenu = new Menu();
            //mainMenu.Items.Add(new Menu.MenuItem("New Game", MenuType.Text, Color.White, "newgame"));
            //mainMenu.Items.Add(new Menu.MenuItem("Load Game", MenuType.Text, Color.White, "loadgame"));
            //mainMenu.Items.Add(new Menu.MenuItem("Options", MenuType.Text, Color.White, "options"));
            //mainMenu.Items.Add(new Menu.MenuItem("Exit Game", MenuType.Text, Color.White, "exit"));
            //mainMenu.Position = new Vector2(10, ScreenManager.Resolution.Y - (mainMenu.Items.Count * 24));
            //mainMenu.CurrentItem = 0;
            //mainMenu.IsSelectable = true;
            //Menus.Add(mainMenu);
            //mainMenuIndex = Menus.Count - 1;

            ScreenManager.GameUIs.Add(UITemplate.mainNew, ScreenManager.GenerateUI(UITemplate.mainNew));
            ScreenManager.GameUIs.Add(UITemplate.mainLoad, ScreenManager.GenerateUI(UITemplate.mainLoad));
            ScreenManager.GameUIs.Add(UITemplate.mainLoadSaves, ScreenManager.GenerateUI(UITemplate.mainLoadSaves));
            ScreenManager.GameUIs.Add(UITemplate.mainOptions, ScreenManager.GenerateUI(UITemplate.mainOptions));
            ScreenManager.GameUIs.Add(UITemplate.mainExit, ScreenManager.GenerateUI(UITemplate.mainExit));
        }

        public override void Update(GameTime gameTime, InputManager input)
        {
            base.Update(gameTime, input);

            //if (Menus[mainMenuIndex].CurrentAction != "" && Menus[mainMenuIndex].CurrentAction != null)
            //{
            //    MenuAction = Menus[mainMenuIndex].CurrentAction.ToLower();
            //    
            //
            //    Menus[mainMenuIndex].CurrentAction = "";
            //}
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            ScreenManager.Sprites.DrawString(ScreenManager.Fonts[Font.logo01.ToString()], "Tales of Tiles", logoPosition, Color.CornflowerBlue);
        }
    }
}
