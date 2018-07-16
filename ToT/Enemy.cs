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
        public float TileProgress; //Percentage of a tile made by this enemy
        public Cardinals Orientation;


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
            Size = new Vector2(32, 32);
            InitEnemy(name);
        }

        public void InitEnemy(string template = "base_1")
        {
            switch(template)
            {
                case "base_1":
                    stats.Add(StatType.HP.ToString(), 2f);
                    stats.Add("+" + StatType.HP.ToString(), 0f);
                    stats.Add(StatType.MoveSpeed.ToString(), 0.34f);
                    stats.Add("+" + StatType.MoveSpeed.ToString(), 0f);
                    ImageName = "creature_robot_32";
                    break;
                default:
                    stats.Add(StatType.HP.ToString(), 2f);
                    stats.Add("+" + StatType.HP.ToString(), 0f);
                    stats.Add(StatType.MoveSpeed.ToString(), 0.34f);
                    stats.Add("+" + StatType.MoveSpeed.ToString(), 0f);
                    ImageName = "creature_robot_32";
                    break;
            }
            Size = new Vector2(ScreenManager.Textures2D[ImageName].Width, ScreenManager.Textures2D[ImageName].Height);
            UpdateOrientation();
        }

        public Vector2 DrawPosition()
        {
            Vector2 tDP = Position;

            switch(Orientation)
            {
                case Cardinals.North:
                    tDP += new Vector2(0, TileProgress);
                    tDP = tDP * (ScreenManager.TSize + 1);
                    break;
                case Cardinals.South:
                    tDP += new Vector2(0, 1 - TileProgress);
                    tDP = tDP * (ScreenManager.TSize + 1) - new Vector2(0, Size.Y);
                    break;
                case Cardinals.West:
                    tDP += new Vector2(TileProgress, 0);
                    tDP = tDP * (ScreenManager.TSize + 1);
                    break;
                case Cardinals.East:
                    tDP += new Vector2(1 - TileProgress, 0);
                    tDP = tDP * (ScreenManager.TSize + 1) - new Vector2(Size.X, 0);
                    break;
            }

            
            return tDP;
        }

        public void UpdateOrientation()
        {
            if (Position.X == 0)
            {
                if (Position.Y > 0)
                    Orientation = Cardinals.South;
                else
                    Orientation = Cardinals.North;
            }
            else
            {
                if (Position.X > 0)
                    Orientation = Cardinals.East;
                else
                    Orientation = Cardinals.West;
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
