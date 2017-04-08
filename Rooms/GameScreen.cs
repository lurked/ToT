using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Rooms
{
    public class GameScreen
    {
        public bool IsActive = true;
        public bool IsPopup = false;
        public Color BackgroundColor = Color.Black;
        public List<Menu> Menus;
        public string MenuAction;

        public virtual void LoadAssets()
        {
            Menus = new List<Menu>();

        }
        public virtual void Update(GameTime gameTime, InputManager input)
        {

            if (Menus == null)
                Menus = new List<Menu>();
            foreach (Menu tM in Menus)
                if (tM.IsActive)
                    tM.Update(gameTime, input);
        }
        public virtual void Draw(GameTime gameTime)
        {
            if (Menus == null)
                Menus = new List<Menu>();
            foreach (Menu tM in Menus)
                if (tM.IsActive)
                    tM.Draw(gameTime);
        }
        public virtual void UnloadAssets()
        {
            Menus.Clear();
        }
    }
}
