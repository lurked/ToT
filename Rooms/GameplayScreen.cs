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
            

            Player1 = new Player(world, "Noob", ThingType.Player, (ScreenManager.RoomSizes[0] * ScreenManager.TileSize) / 2, "player01", "A noob player");
            Player1.Initialize();
        }

        public override void Update(GameTime gameTime, InputManager input)
        { 
            world.Step(Math.Min((float) gameTime.ElapsedGameTime.TotalSeconds, 1f / 60f));
            Player1.Update(gameTime);
            base.Update(gameTime, input);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            DrawRoom(CurrentLevel.MainRoom);
            DrawRooms(CurrentLevel.RoomsNorth);
            DrawRooms(CurrentLevel.RoomsEast);
            DrawRooms(CurrentLevel.RoomsSouth);
            DrawRooms(CurrentLevel.RoomsWest);
            ScreenManager.Sprites.Draw(ScreenManager.Textures2D[Player1.ImageName], Player1.ThingBody.Position, null, Color.White, 0f, new Vector2(Player1.Rect.Width / 2, Player1.Rect.Height / 2), 1f, SpriteEffects.None, 0f);
            if (ScreenManager.DebugMode)
            {
                ScreenManager.Sprites.DrawString(ScreenManager.Fonts[Font.debug01.ToString()], (int)Player1.ThingBody.Position.X + ":" + (int)Player1.ThingBody.Position.Y, Player1.ThingBody.Position - new Vector2(24, 24), Color.Red, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
            }
        }

        public void DrawRooms(List<Room> rooms)
        {
            foreach (Room tR in rooms)
                DrawRoom(tR);
        }

        private void DrawRoom(Room room)
        {

            foreach (Thing tT in room.Decors)
                if (tT.ToDraw)
                {
                    ScreenManager.Sprites.Draw(ScreenManager.Textures2D[tT.ImageName], tT.ThingBody.Position, null, Color.White, tT.ThingBody.Rotation, new Vector2(tT.Rect.Width / 2, tT.Rect.Height / 2), 1f, SpriteEffects.None, 0f);                    //if (ScreenManager.DebugMode)
                    if (ScreenManager.DebugMode)
                        ScreenManager.Sprites.DrawString(ScreenManager.Fonts[Font.debug02.ToString()], (int)tT.Coords.X + ":" + (int)tT.Coords.Y, (tT.ThingBody.Position - new Vector2(15, 15)), Color.DarkRed, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
                }
            if (ScreenManager.DebugMode)
                ScreenManager.Sprites.DrawString(ScreenManager.Fonts[Font.debug01.ToString()], room.Position.ToString(), (room.Position - new Vector2(15, 15)), Color.Blue);
        }
    }
}
