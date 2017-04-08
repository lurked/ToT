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
    public class Level
    {
        public LevelType LType;
        public string Name;
        public string Params;
        public List<Decor> Decors;
        public Room MainRoom;
        public List<Room> RoomsNorth;
        public List<Room> RoomsEast;
        public List<Room> RoomsSouth;
        public List<Room> RoomsWest;

        public Level()
        {
            Decors = new List<Decor>();
            MainRoom = new Room();
            RoomsNorth = new List<Room>();
            RoomsEast = new List<Room>();
            RoomsSouth = new List<Room>();
            RoomsWest = new List<Room>();
        }

        public void AddRooms(World world, ElementType eType, List<Room> dRooms, int qty, Cardinals ori)
        {

            for (int i = 0; i < qty; i++)
            {
                Room room = new Room();
                room.Initialize(eType, dRooms.Count + 1);
                if (dRooms.Count > 0)
                {
                    switch(ori)
                    {
                        case Cardinals.North:
                            room.Position = new Vector2(dRooms[dRooms.Count - 1].Position.X + (((dRooms[dRooms.Count - 1].Size.X - room.Size.X) / 2) * ScreenManager.TileSize.X), dRooms[dRooms.Count - 1].Position.Y - (room.Size.Y * ScreenManager.TileSize.Y));
                            break;
                        case Cardinals.East:
                            room.Position = new Vector2(dRooms[dRooms.Count - 1].Position.X + (dRooms[dRooms.Count - 1].Size.X * ScreenManager.TileSize.X), dRooms[dRooms.Count - 1].Position.Y + (((dRooms[dRooms.Count - 1].Size.Y - room.Size.Y) / 2) * ScreenManager.TileSize.Y));
                            break;
                        case Cardinals.South:
                            room.Position = new Vector2(dRooms[dRooms.Count - 1].Position.X + (((dRooms[dRooms.Count - 1].Size.X - room.Size.X) / 2) * ScreenManager.TileSize.X), dRooms[dRooms.Count - 1].Position.Y + (dRooms[dRooms.Count - 1].Size.Y * ScreenManager.TileSize.Y));
                            break;
                        case Cardinals.West:
                            room.Position = new Vector2(dRooms[dRooms.Count - 1].Position.X - (room.Size.X * ScreenManager.TileSize.X), dRooms[dRooms.Count - 1].Position.Y + (((dRooms[dRooms.Count - 1].Size.Y - room.Size.Y) / 2) * ScreenManager.TileSize.Y));
                            break;
                    }
                }
                else
                {
                    switch (ori)
                    {
                        case Cardinals.North:
                            room.Position = new Vector2(MainRoom.Position.X + (((MainRoom.Size.X - room.Size.X) / 2) * ScreenManager.TileSize.X), MainRoom.Position.Y - (room.Size.Y * ScreenManager.TileSize.Y));
                            break;
                        case Cardinals.East:
                            room.Position = new Vector2(MainRoom.Position.X + (MainRoom.Size.X * ScreenManager.TileSize.X), MainRoom.Position.Y + (((MainRoom.Size.Y - room.Size.Y) / 2) * ScreenManager.TileSize.Y));
                            break;
                        case Cardinals.South:
                            room.Position = new Vector2(MainRoom.Position.X + (((MainRoom.Size.X - room.Size.X) / 2) * ScreenManager.TileSize.X), MainRoom.Position.Y + (MainRoom.Size.Y * ScreenManager.TileSize.Y));
                            break;
                        case Cardinals.West:
                            room.Position = new Vector2(MainRoom.Position.X - (room.Size.X * ScreenManager.TileSize.X), MainRoom.Position.Y + (((MainRoom.Size.Y - room.Size.Y) / 2) * ScreenManager.TileSize.Y));
                            break;
                    }
                    
                }

                room.GenerateRoom(world, room.Position);
                dRooms.Add(room);

                switch (ori)
                {
                    case Cardinals.North:
                        if (dRooms.Count > 1)
                            dRooms[dRooms.Count - 2].OpenSide(Cardinals.North, world);
                        else
                            MainRoom.OpenSide(Cardinals.North, world);

                        dRooms[dRooms.Count - 1].OpenSide(Cardinals.South, world);

                        break;
                    case Cardinals.East:
                        if (dRooms.Count > 1)
                            dRooms[dRooms.Count - 2].OpenSide(Cardinals.East, world);
                        else
                            MainRoom.OpenSide(Cardinals.East, world);

                        dRooms[dRooms.Count - 1].OpenSide(Cardinals.West, world);

                        break;
                    case Cardinals.South:
                        if (dRooms.Count > 1)
                            dRooms[dRooms.Count - 2].OpenSide(Cardinals.South, world);
                        else
                            MainRoom.OpenSide(Cardinals.South, world);

                        dRooms[dRooms.Count - 1].OpenSide(Cardinals.North, world);

                        break;
                    case Cardinals.West:
                        if (dRooms.Count > 1)
                            dRooms[dRooms.Count - 2].OpenSide(Cardinals.West, world);
                        else
                            MainRoom.OpenSide(Cardinals.West, world);

                        dRooms[dRooms.Count - 1].OpenSide(Cardinals.East, world);

                        break;
                }
            }
        }

        public void GenerateLevel(World world, LevelType type, string name, string lParams)
        {
            LType = type;
            Name = name;
            Params = lParams;

            if (name == "base01")
            {
                MainRoom.Initialize(ElementType.Neutral, 4);
                MainRoom.GenerateRoom(world, Vector2.Zero);

                AddRooms(world, ElementType.Air, RoomsNorth, 3, Cardinals.North);
                AddRooms(world, ElementType.Fire, RoomsEast, 3, Cardinals.East);
                AddRooms(world, ElementType.Earth, RoomsSouth, 3, Cardinals.South);
                AddRooms(world, ElementType.Water, RoomsWest, 3, Cardinals.West);
            }
        }
    }
}
