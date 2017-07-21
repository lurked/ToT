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

        }

        public Enemy(string name, ThingType kind, Vector2 position, string imageName = "creature_robot_32", string tooltip = "")
        {
            Name = name;
            Kind = kind;
            Position = position;
            ImageName = imageName;
            Tooltip = tooltip;
            Rect = ScreenManager.Textures2D[ImageName].Bounds;

            InitEnemy(name);
        }

        public void InitEnemy(string template = "base_1")
        {
            switch(template)
            {
                case "base_1":
                    stats.Add("hp", 2f);
                    stats.Add("+hp", 0f);
                    stats.Add("movespeed", 1f);
                    stats.Add("+movespeed", 0f);
                    ImageName = "creature_robot_32";
                    break;
                default:
                    stats.Add("hp", 2f);
                    stats.Add("+hp", 0f);
                    stats.Add("movespeed", 1f);
                    stats.Add("+movespeed", 0f);
                    ImageName = "creature_robot_32";
                    break;
            }
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
