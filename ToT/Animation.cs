using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ToT
{
    public class Animation
    {
        private Texture2D image;
        private string text;
        private SpriteFont font;
        private Color color, drawColor;
        private Rectangle sourceRect;
        //private float axis;
        private float rotation, scale;
        private Vector2 origin, position;
        private ContentManager content;
        private bool isActive;
        private float alpha;
        private Vector2 frames, currentFrame;

        public Color DrawColor
        {
            set { drawColor = value; }
            get { return drawColor; }
        }
        public Texture2D Image
        {
            get { return image; }
        }

        public Rectangle SourceRect
        {
            set { sourceRect = value; }
        }

        public Vector2 Frames
        {
            set { frames = value; }
        }

        public Vector2 CurrentFrame
        {
            set { currentFrame = value; }
            get { return currentFrame; }
        }

        public int FrameWidth
        {
            get { return image.Width / (int)frames.X; }
        }

        public int FrameHeight
        {
            get { return image.Height / (int)frames.Y; }
        }

        public virtual float Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }

        public bool IsActive
        {
            set { isActive = value; }
            get { return isActive; }
        }

        public float Scale
        {
            set { scale = value; }
        }

        public SpriteFont Font
        {
            get { return font; }
            set { font = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public void LoadContent(ContentManager Content, Texture2D image,
            string text, Vector2 position)
        {
            content = new ContentManager(Content.ServiceProvider, "Content");
            this.image = image;
            this.text = text;
            this.position = position;
            if (text != String.Empty)
            {
                font = this.content.Load<SpriteFont>("Font1");
                //color = new Color(0, 160, 210);
                color = Color.White;
            }

            rotation = 0.0f;
            //axis = 0.0f;
            scale = alpha = 1.0f;
            isActive = false;
            drawColor = Color.White;

            currentFrame = new Vector2(0, 0);
            if (image != null && frames != Vector2.Zero)
                sourceRect = new Rectangle((int)currentFrame.X * FrameWidth, (int)currentFrame.Y * FrameHeight, FrameWidth, FrameHeight);
            else
                sourceRect = new Rectangle(0, 0, image.Width, image.Height);
        }

        public void UnloadContent()
        {
            //image = position = font = color = sourceRect = rotation = axis = scale = null;
            content.Unload();
            text = String.Empty;
            position = Vector2.Zero;
            sourceRect = Rectangle.Empty;
            image = null;
            color = Color.Black;
            isActive = false;

        }

        public virtual void Update(GameTime gameTime, ref Animation a)
        {

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (image != null)
            {
                origin = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);
                spriteBatch.Draw(image, position + origin, sourceRect,
                    drawColor * alpha, rotation, origin, scale,
                    SpriteEffects.None, 0.0f);
            }

            if (text != String.Empty)
            {
                origin = new Vector2(font.MeasureString(text).X / 2,
                        font.MeasureString(text).Y / 2);
                spriteBatch.DrawString(font, text, position + origin,
                    color * alpha, rotation, origin, scale, SpriteEffects.None, 0.0f);
            }

        }
    }
}
