using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Rooms
{
    public class Decor : Thing
    {
        public Decor(string name, ThingType kind, Vector2 position, string imageName = "", string tooltip = "")
        {
            Name = name;
            Kind = kind;
            Position = position;
            ImageName = imageName;
            Tooltip = tooltip;
            //Rect = ScreenManager.Textures2D[ImageName].Bounds;
            Rect = new Rectangle(0, 0, (int)(Size.X * ScreenManager.TileSize.X), (int)(Size.Y * ScreenManager.TileSize.Y));
            //thingBody = BodyFactory.CreateRectangle(world, Rect.Width, Rect.Height, 1f, Position);
        }

        public void Initialize()
        {
            Coords = Vector2.Zero;
            //thingBody.BodyType = BodyType.Static;
            //thingBody.Friction = 1000f;
            //thingBody.IsStatic = true;
            //thingBody.CollisionCategories = Category.Cat1; //assigning the entity to a category
            //thingBody.CollidesWith = Category.All; //which category will the entity collide with? i pick all in this case
            //thingBody.UserData = this; // just leave this be as it is for now
            //thingBody.Position = Position; // Sets the position of the object
        }

    }
}
