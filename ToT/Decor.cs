using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ToT
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
            Rect = new Rectangle(0, 0, (int)(Size.X * ScreenManager.TileSize.X), (int)(Size.Y * ScreenManager.TileSize.Y));
        }

        public void Initialize()
        {
            Coords = Vector2.Zero;
        }

    }
}
