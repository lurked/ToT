using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ToT
{
    public class GameScreen
    {
        public bool IsActive = true;
        public bool IsPopup = false;
        public Color BackgroundColor = Color.Black;

        public virtual void LoadAssets()
        {

        }
        public virtual void Update(GameTime gameTime, InputManager input)
        {


        }
        public virtual void Draw(GameTime gameTime)
        {

        }
        public virtual void UnloadAssets()
        {

        }
    }
}
