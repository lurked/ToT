using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ToT
{
    public class Enemy : Thing
    {
        public Vector2 Velocity;

        public void Initialize()
        {
            //thingBody.BodyType = BodyType.Dynamic;
            //thingBody.Friction = 100f;
            //thingBody.GravityScale = 0f;
            //thingBody.CollisionCategories = Category.Cat1; //assigning the entity to a category
            //thingBody.CollidesWith = Category.All; //which category will the entity collide with? i pick all in this case
            //thingBody.UserData = this; // just leave this be as it is for now
            //thingBody.Position = Position; // Sets the position of the object
        }

        public Enemy(string name, ThingType kind, Vector2 position, string imageName = "", string tooltip = "")
        {
            Name = name;
            Kind = kind;
            Position = position;
            ImageName = imageName;
            Tooltip = tooltip;
            Rect = ScreenManager.Textures2D[ImageName].Bounds;
            //thingBody = BodyFactory.CreateRectangle(world, Rect.Width, Rect.Height, 1f, position);
            //thingBody = BodyFactory.CreateCircle(world, Rect.Width / 2, 1f, position);
            InitEnemy();
        }

        public void InitEnemy(string template = "base01")
        {
            stats.Add("hp", 2f);
            stats.Add("+hp", 0f);
            stats.Add("movespeed", 1.5f);
            stats.Add("+movespeed", 0f);
            stats.Add("maxspeed", 1f);
            stats.Add("+maxspeed", 0f);
            Velocity = Vector2.Zero;
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 tForce = Vector2.Zero;

            UpdateMovement(gameTime);

            base.Update(gameTime);
        }

        private void UpdateMovement(GameTime gameTime)
        {

        }
    }
}
