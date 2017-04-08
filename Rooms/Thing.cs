using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;


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
        protected Body thingBody;
        protected Dictionary<string, float> stats;
        public Rectangle Rect;
        public Vector2 Coords;

        public Body ThingBody
        {
            get { return thingBody; }
        }

        public Thing()
        {
            Kind = ThingType.Decor;
            Position = Vector2.Zero;
            stats = new Dictionary<string, float>();
            Coords = Vector2.Zero;
        }

        //public Thing(World world)
        //{
        //    Kind = ThingType.Decor;
        //    Position = Vector2.Zero;
        //}

        //public Thing(World world, string name)
        //{
        //    Kind = ThingType.Decor;
        //    Position = Vector2.Zero;
        //    Name = name;
        //}
        public virtual void Update(GameTime gameTime)
        {

        }

        public Thing(World world, string name, ThingType kind, Vector2 position, string imageName = "", string tooltip = "")
        {
            Name = name;
            Kind = kind;
            Position = position;
            ImageName = imageName;
            Tooltip = tooltip;
            Rect = ScreenManager.Textures2D[ImageName].Bounds;
            thingBody = BodyFactory.CreateRectangle(world, Rect.Width, Rect.Height, 1f, position);
            Coords = Vector2.Zero;
        }
    }
}
