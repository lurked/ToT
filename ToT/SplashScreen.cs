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
    public class SplashScreen : GameScreen
    {
        private Vector2 logoPosition;

        public override void LoadAssets()
        {
            logoPosition = new Vector2(10, 10);
        }

        public override void Update(GameTime gameTime, InputManager input)
        {
            base.Update(gameTime, input);

        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.Sprites.DrawString(ScreenManager.Fonts[Font.logo01.ToString()], "Tales of Tiles", logoPosition, Color.CornflowerBlue);
        }
    }
}
