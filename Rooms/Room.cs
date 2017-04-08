using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
namespace Rooms
{
    public class Room
    {
        public Vector2 RoomPosition;
        public Vector2 Position;
        public Vector2 Size;
        public List<Decor> Decors;
        public ElementType Element;
        public string ImageName;
        public int Lvl;
        

        public Room()
        {
            Position = Vector2.Zero;
            RoomPosition = Vector2.Zero;
            Decors = new List<Decor>();
            Element = ElementType.Neutral;

        }

        public void Initialize(ElementType element, int lvl)
        {
            Element = element;
            Lvl = lvl;
            switch(Element)
            {
                case ElementType.Air:
                    ImageName = "pal_green_01";
                    break;
                case ElementType.Fire:
                    ImageName = "pal_orange_01";
                    break;
                case ElementType.Earth:
                    ImageName = "pal_brown_01";
                    break;
                case ElementType.Water:
                    ImageName = "pal_blue_01";
                    break;
                default:
                    ImageName = "pal_grey_01";
                    break;
            }
            Size = ScreenManager.RoomSizes[Lvl];
        }

        public void OpenSide(Cardinals ori, World world)
        {
            int iStart;
            int iEnd;
            
            switch (ori)
            {
                case Cardinals.North:
                    iStart = (int)(Size.X / 2) - 3;
                    iEnd = (int)(Size.X / 2) + 1;

                    for (int i = 0; i < Decors.Count; i++)
                    {
                        if (Decors[i].Coords.Y == 0 && Decors[i].Coords.X > iStart && Decors[i].Coords.X < iEnd)
                        {
                            world.RemoveBody(Decors[i].ThingBody);
                            Decors.RemoveAt(i);
                            i--;
                        }
                    }
                    break;
                case Cardinals.East:
                    iStart = (int)(Size.Y / 2) - 3;
                    iEnd = (int)(Size.Y / 2) + 1;


                    for (int i = 0; i < Decors.Count; i++)
                    {
                        if (Decors[i].Coords.X == Size.X - 1 && Decors[i].Coords.Y > iStart && Decors[i].Coords.Y < iEnd)
                        {
                            world.RemoveBody(Decors[i].ThingBody);
                            Decors.RemoveAt(i);
                            i--;
                        }
                    }
                    break;
                case Cardinals.South:
                    iStart = (int)(Size.X / 2) - 3;
                    iEnd = (int)(Size.X / 2) + 1;


                    for (int i = 0; i < Decors.Count; i++)
                    {
                        if (Decors[i].Coords.Y == Size.Y - 1 && Decors[i].Coords.X > iStart && Decors[i].Coords.X < iEnd)
                        {
                            world.RemoveBody(Decors[i].ThingBody);
                            Decors.RemoveAt(i);
                            i--;
                        }
                    }

                    break;
                case Cardinals.West:
                    iStart = (int)(Size.Y / 2) - 3;
                    iEnd = (int)(Size.Y / 2) + 1;

                    for (int i = 0; i < Decors.Count; i++)
                    {
                        if (Decors[i].Coords.X == 0 && Decors[i].Coords.Y > iStart && Decors[i].Coords.Y < iEnd)
                        {
                            world.RemoveBody(Decors[i].ThingBody);
                            Decors.RemoveAt(i);
                            i--;
                        }
                    }
                    break;
            }
        }
        
        public void GenerateRoom(World world, Vector2 position)
        {
            Position = position;
            for (int i = 0; i < ScreenManager.RoomSizes[Lvl].X; i++)
            {
                Vector2 tV = new Vector2(i, 0);
                Decors.Add(new Decor(world, Element.ToString() + Lvl + "_0:" + i, ThingType.Decor, Position + new Vector2(i * ScreenManager.TileSize.X, 0), ImageName, Element.ToString() + Lvl + "_0:" + i));
                Decors[Decors.Count - 1].Initialize(tV);

                tV = new Vector2(i, (Size.Y - 1));
                Decors.Add(new Decor(world, Element.ToString() + Lvl + "_" + i + ":" + (Size.Y - 1), ThingType.Decor, Position + new Vector2(i * ScreenManager.TileSize.X, ScreenManager.TileSize.Y * (Size.Y - 1)), ImageName, Element.ToString() + Lvl + "_" + i + ":" + (Size.Y - 1)));
                Decors[Decors.Count - 1].Initialize(tV);
            }

            for (int i = 1; i < ScreenManager.RoomSizes[Lvl].Y; i++)
            {
                Vector2 tV = new Vector2(0, i);
                Decors.Add(new Decor(world, Element.ToString() + Lvl + "_" + i + ":0", ThingType.Decor, Position + new Vector2(0, i * ScreenManager.TileSize.X), ImageName, Element.ToString() + Lvl + "_" + i + ":0"));
                Decors[Decors.Count - 1].Initialize(tV);

                tV = new Vector2((Size.X - 1), i);
                Decors.Add(new Decor(world, Element.ToString() + Lvl + "_" + (Size.X - 1) + ":" + i, ThingType.Decor, Position + new Vector2(ScreenManager.TileSize.X * (Size.X - 1), i * ScreenManager.TileSize.Y), ImageName, Element.ToString() + Lvl + "_" + (Size.X - 1) + ":" + i));
                Decors[Decors.Count - 1].Initialize(tV);
            }
        }

    }
}
