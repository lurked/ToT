using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace Rooms
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
        public Room ActiveRoom;
        protected Dictionary<Vector2, Room> Stage;


        public Thing()
        {
            Kind = ThingType.Decor;
            Position = Vector2.Zero;
            stats = new Dictionary<string, float>();
            Coords = Vector2.Zero;
            ActiveRoom = new Room();
        }
        
        public void SetStage(Dictionary<Vector2, Room> stage)
        {
            Stage = stage;
        }

        public void SetActiveRoom(Room currentRoom)
        {
            ActiveRoom.IsActive = false;
            ActiveRoom = currentRoom;
            Position = currentRoom.Position * ScreenManager.TileSize;
            ActiveRoom.IsActive = true;
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
