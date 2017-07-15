using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
namespace Rooms
{
    public class Room
    {
        public Vector2 RoomPosition;
        public Vector2 Position;
        public List<Decor> Decors;
        public ElementType Element;
        public string ImageName;
        public int Lvl;
        public bool IsActive;

        

        public Room()
        {
            Position = Vector2.Zero;
            RoomPosition = Vector2.Zero;
            Decors = new List<Decor>();
            Element = ElementType.Neutral;
            IsActive = false;
        }

        public void Initialize(ElementType element)
        {
            Element = element;
            switch(Element)
            {
                case ElementType.Air:
                    ImageName = "pal_green_" + ScreenManager.TSize;
                    break;
                case ElementType.Fire:
                    ImageName = "pal_orange_" + ScreenManager.TSize;
                    break;
                case ElementType.Earth:
                    ImageName = "pal_brown_" + ScreenManager.TSize;
                    break;
                case ElementType.Water:
                    ImageName = "pal_blue_" + ScreenManager.TSize;
                    break;
                default:
                    ImageName = "pal_green_" + ScreenManager.TSize;
                    break;
            }
        }

        public void GenerateRoom(Vector2 position)
        {
            Position = position;
            string tName = Element.ToString() + "_Top";
            Decor tD = new Decor(tName, ThingType.Decor, Position, ImageName, tName);
            Decors.Add(tD);
            Decors[Decors.Count - 1].Initialize();
        }

    }
}
