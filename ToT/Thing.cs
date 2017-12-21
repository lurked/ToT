using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace ToT
{
    public class Thing
    {
        public ThingType Kind;
        public string Name;
        public string Tooltip;
        public string ImageName;
        public Vector2 Position;
        public bool ToDraw = true;
        protected Dictionary<string, float> stats;
        public Rectangle Rect;
        public Vector2 Coords;
        public Vector2 Size;
        public Tile ActiveRoom;
        protected Dictionary<Vector2, Tile> Stage;

        public Thing()
        {
            Kind = ThingType.Decor;
            Position = Vector2.Zero;
            stats = new Dictionary<string, float>();
            Coords = Vector2.Zero;
            ActiveRoom = new Tile();
        }
        
        public void SetStage(Dictionary<Vector2, Tile> stage)
        {
            Stage = stage;
        }

        public float GetStat(string stat)
        {
            float tStat = 0;

            if (stats.ContainsKey(stat))
                tStat = stats[stat];

            if (stats.ContainsKey("+" + stat))
                tStat += stats["+" + stat];

            return tStat;
        }

        public void SetActiveRoom(Tile currentRoom)
        {
            ActiveRoom.IsActive = false;
            ActiveRoom = currentRoom;
            Position = currentRoom.RoomPosition;
            ActiveRoom.IsActive = true;

            RefreshExpendUIs();
            ScreenManager.TogTileSheet(false);
            ScreenManager.TogBuildUI(false);
            ScreenManager.TogImproveUI(false);
        }

        public void RefreshExpendUIs()
        {
            CheckIfDrawTileExpend(UITemplate.tileExpendNorth, new Vector2(ActiveRoom.Position.X, ActiveRoom.Position.Y - 1));
            CheckIfDrawTileExpend(UITemplate.tileExpendEast, new Vector2(ActiveRoom.Position.X + 1, ActiveRoom.Position.Y));
            CheckIfDrawTileExpend(UITemplate.tileExpendSouth, new Vector2(ActiveRoom.Position.X, ActiveRoom.Position.Y + 1));
            CheckIfDrawTileExpend(UITemplate.tileExpendWest, new Vector2(ActiveRoom.Position.X - 1, ActiveRoom.Position.Y));
        }

        public void CheckIfDrawTileExpend(UITemplate uiTemplate, Vector2 position)
        {
            if (!ScreenManager.GGPScreen.CurrentLevel.Stage.ContainsKey(position))
            {
                ScreenManager.GameUIs[uiTemplate].ToDraw = true;
                ScreenManager.GameUIs[uiTemplate].Items[0].ActionText = position.X + ":" + position.Y;
                string tTT = "(" + ScreenManager.GGPScreen.GetTileLevelCostString() + ") Discover a new tile - ";
                switch (uiTemplate)
                {
                    case UITemplate.tileExpendNorth:
                        tTT += "North";
                        break;
                    case UITemplate.tileExpendEast:
                        tTT += "East";
                        break;
                    case UITemplate.tileExpendSouth:
                        tTT += "South";
                        break;
                    case UITemplate.tileExpendWest:
                        tTT += "West";
                        break;

                }
                ScreenManager.GameUIs[uiTemplate].TTText = tTT;
            }
            else
                ScreenManager.GameUIs[uiTemplate].ToDraw = false;
                
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public Thing(string name, ThingType kind, Vector2 position, string imageName = "", string tooltip = "")
        {
            Name = name;
            Kind = kind;
            Position = position;
            ImageName = imageName;
            Tooltip = tooltip;
            Rect = ScreenManager.Textures2D[ImageName].Bounds;
            Coords = Vector2.Zero;
        }
    }
}
