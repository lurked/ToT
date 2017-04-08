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

using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision;
using FarseerPhysics.Factories;

namespace Rooms
{
    public class GameplayScreen : GameScreen
    {
        public Level CurrentLevel;
        public Player Player1;

        protected World world;

        public GameplayScreen()
        {

        }

        public void Initialize()
        {
            world = new World(new Vector2(0, 8192));

            CurrentLevel = new Level();
            CurrentLevel.GenerateLevel(world, LevelType.Static, "base01", "");

            Player1 = new Player(world, "Noob", ThingType.Player, new Vector2(384, 284), "player01", "A noob player");
            Player1.Initialize();
        }

        public override void Update(GameTime gameTime, InputManager input)
        { 
            world.Step(Math.Min((float) gameTime.ElapsedGameTime.TotalSeconds, (1f / 60f)));
            Player1.Update(gameTime);
            base.Update(gameTime, input);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            foreach (Thing tT in CurrentLevel.Decors)
                if (tT.ToDraw)
                {
                    if (ScreenManager.DebugMode)
                    {
                        ScreenManager.Sprites.DrawString(ScreenManager.Fonts[Font.debug01.ToString()], tT.Position.ToString(), tT.Position, Color.Red);
                    }
                    //ScreenManager.Sprites.Draw(ScreenManager.Textures2D[tT.ImageName], new Rectangle((int)tT.Position.X, (int)tT.Position.Y, 70, 25), Color.White);
                    ScreenManager.Sprites.Draw(ScreenManager.Textures2D[tT.ImageName], tT.ThingBody.Position, null, Color.White, tT.ThingBody.Rotation, new Vector2(tT.Rect.Width / 2, tT.Rect.Height / 2), 1f, SpriteEffects.None, 0f);
                }
            //ScreenManager.Sprites.Draw(ScreenManager.Textures2D[Player1.ImageName], Player1.ThingBody.Position, null, Color.White, Player1.ThingBody.Rotation, new Vector2(Player1.Rect.Width / 2, Player1.Rect.Height / 2), 1f, SpriteEffects.None, 0f);
            ScreenManager.Sprites.Draw(ScreenManager.Textures2D[Player1.ImageName], Player1.ThingBody.Position, null, Color.White, 0f, new Vector2(Player1.Rect.Width / 2, Player1.Rect.Height / 2), 1f, SpriteEffects.None, 0f);
            ScreenManager.Sprites.DrawString(ScreenManager.Fonts[Font.debug01.ToString()], Player1.Velocity.ToString(), Player1.Position - new Vector2(0, 12), Color.Red);
            if (ScreenManager.DebugMode)
            {
                ScreenManager.Sprites.DrawString(ScreenManager.Fonts[Font.debug01.ToString()], Player1.Position.ToString(), Player1.Position - new Vector2(0, 12), Color.Red);
                ScreenManager.Sprites.DrawString(ScreenManager.Fonts[Font.debug01.ToString()], Player1.ThingBody.Position.ToString(), Player1.ThingBody.Position - new Vector2(0, 24), Color.Red);
            }
        }
    }
}
