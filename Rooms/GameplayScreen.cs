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


namespace Rooms
{
    public class GameplayScreen : GameScreen
    {
        public Level CurrentLevel;
        public Player Player1;
        public static int Turn = 0;

        public GameplayScreen()
        {

        }

        public void Initialize()
        {

            CurrentLevel = new Level();
            CurrentLevel.GenerateLevel(LevelType.Static, "base01", "");

            Player1 = new Player("Noob", ThingType.Player, ScreenManager.TileSize / 2, "player01", "A noob player");
            Player1.Initialize();
            Player1.SetStage(CurrentLevel.Stage);
            Player1.SetActiveRoom(CurrentLevel.Stage[Vector2.Zero]);
            

        }

        public override void Update(GameTime gameTime, InputManager input)
        { 
            //Player1.Update(gameTime);
            base.Update(gameTime, input);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            foreach (KeyValuePair<Vector2, Room> entry in CurrentLevel.Stage)
                DrawRoom(entry.Value);

            ScreenManager.Sprites.Draw(ScreenManager.Textures2D[Player1.ImageName], Player1.Position + (ScreenManager.TileSize / 2), null, Color.White, 0f, new Vector2(Player1.Rect.Width / 2, Player1.Rect.Height / 2), 1f, SpriteEffects.None, 0f);
            if (ScreenManager.DebugMode)
            {
                ScreenManager.Sprites.DrawString(ScreenManager.Fonts[Font.debug01.ToString()], (int)Player1.Position.X + ":" + (int)Player1.Position.Y, Player1.Position - new Vector2(24, 24), Color.Red, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
            }
        }

        private void DrawRoom(Room room)
        {

            foreach (Thing tT in room.Decors)
                if (tT.ToDraw)
                {
                    Color tCol = Color.White;
                    if (room.IsActive)
                        tCol = Color.Blue;
                    //ScreenManager.Sprites.Draw(ScreenManager.Textures2D[tT.ImageName], tT.Position + (room.Position * ScreenManager.TSize), null, Color.White, 0f, new Vector2(tT.Rect.Width / 2, tT.Rect.Height / 2), 1f, SpriteEffects.None, 0f);                    //if (ScreenManager.DebugMode)
                    ScreenManager.Sprites.Draw(ScreenManager.Textures2D[tT.ImageName], tT.Position + (room.Position * ScreenManager.TSize), null, tCol);                    //if (ScreenManager.DebugMode)
                    //if (ScreenManager.DebugMode)
                    //    ScreenManager.Sprites.DrawString(ScreenManager.Fonts[Font.debug02.ToString()], (int)tT.Coords.X + ":" + (int)tT.Coords.Y, (tT.Position - new Vector2(15, 15)) + (room.Position * ScreenManager.TSize), Color.DarkRed, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
                }
            if (ScreenManager.DebugMode)
                ScreenManager.Sprites.DrawString(ScreenManager.Fonts[Font.debug02.ToString()], (int)room.Position.X + ":" + (int)room.Position.Y, (room.Position - new Vector2(15, 15)) + (room.Position * ScreenManager.TSize), Color.DarkRed, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
        }
    }
}
