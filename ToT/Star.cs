using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ToT
{
    public class Star
    {
        private Texture2D Texture { get; set; }
        public Color TintColor { get; set; }
        public Vector2 Location { get; set; }
        private Vector2 Velocity { get; set; }
        private Rectangle InitialFrame { get; set; }
        private Vector2 StarfieldPosition { get; set; }
        private float Scale { get; set; }
        
        public Star(Vector2 location, Texture2D texture, Rectangle initialFrame, Vector2 velocity)
        {
            Location = location;
            Texture = texture;
            InitialFrame = initialFrame;
            Velocity = velocity;
        }

        public Rectangle Destination
        {
            get
            {
                return new Rectangle(
                    (int)(Location.X + StarfieldPosition.X), 
                    (int)(Location.Y + StarfieldPosition.Y), 
                    InitialFrame.Width, 
                    InitialFrame.Height);
            }
        }
        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Location += (Velocity * elapsed);
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 starfieldPosition)
        {
            StarfieldPosition = starfieldPosition;
            spriteBatch.Draw(Texture, Destination, InitialFrame, TintColor);
        }
    }
}
