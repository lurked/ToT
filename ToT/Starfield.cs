using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ToT
{
    public class Starfield
    {
        private List<Star> stars = new List<Star>();
        private int ScreenWidth { get; set; }
        private int ScreenHeight { get; set; }
        private Random rand = new Random();
        public Vector2 StarfieldPosition { get; set; }
        private Color[] colors = {
            Color.White, Color.Yellow, Color.Red
        };

        public Starfield(int screenWidth, int screenHeight, int starCount, Vector2 starVelocity, Texture2D texture, Rectangle initialFrame)
        {
            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;

            for (int x = 0; x < starCount; x++)
            {
                stars.Add(
                    new Star(
                        new Vector2(rand.Next(0, screenWidth),
                            rand.Next(0, screenHeight)),
                        texture,
                        initialFrame,
                        starVelocity)
                );
            }
            foreach (Star star in stars)
            {
                star.TintColor = colors[rand.Next(0, colors.Length)];
                star.TintColor *= (float)(rand.Next(30, 80) / 100f);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (Star star in stars)
            {
                star.Update(gameTime);
                if (star.Location.Y > ScreenHeight)
                {
                    star.Location = new Vector2(rand.Next(0, ScreenWidth), 0);
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Star star in stars)
            {
                star.Draw(spriteBatch, StarfieldPosition);
            }
        }
    }
}
