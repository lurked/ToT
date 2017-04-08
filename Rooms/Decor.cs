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
    public class Decor : Thing
    {
        public Decor(World world, string name, ThingType kind, Vector2 position, string imageName = "", string tooltip = "")
        {
            Name = name;
            Kind = kind;
            Position = position;
            ImageName = imageName;
            Tooltip = tooltip;
            Rect = ScreenManager.Textures2D[ImageName].Bounds;
            thingBody = BodyFactory.CreateRectangle(world, Rect.Width, Rect.Height, 1f, position);
        }

        public void Initialize(Vector2 coords)
        {
            Coords = coords;
            thingBody.BodyType = BodyType.Static;
            thingBody.Friction = 10f;
            thingBody.CollisionCategories = Category.Cat2; //assigning the entity to a category
            thingBody.CollidesWith = Category.All; //which category will the entity collide with? i pick all in this case
            thingBody.UserData = this; // just leave this be as it is for now
            thingBody.Position = Position; // Sets the position of the object
        }

    }
}
