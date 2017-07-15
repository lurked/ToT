using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;


namespace Rooms
{
    public class Level
    {
        public LevelType LType;
        public string Name;
        public string Params;
        public List<Decor> Decors;
        public Dictionary<Vector2, Room> Stage;

        public Level()
        {
            Decors = new List<Decor>();
            Stage = new Dictionary<Vector2, Room>();
        }

        public void GenerateLevel(LevelType type, string name, string lParams)
        {
            LType = type;
            Name = name;
            Params = lParams;

            if (name == "base01")
            {

                AddRoom(Vector2.Zero);
                AddRoom(new Vector2(1, 0));
                AddRoom(new Vector2(2, 0));
                AddRoom(new Vector2(3, 0));
                //AddRoom(new Vector2(3, 3));
                AddRoom(new Vector2(0, 1));
                AddRoom(new Vector2(0, 2));
                AddRoom(new Vector2(0, 3));
                //AddRoom(new Vector2(-3, 3));
                AddRoom(new Vector2(-1, 0));
                AddRoom(new Vector2(-2, 0));
                AddRoom(new Vector2(-3, 0));
                //AddRoom(new Vector2(-3, -3));
                AddRoom(new Vector2(0, -1));
                AddRoom(new Vector2(0, -2));
                AddRoom(new Vector2(0, -3));
                //AddRoom(new Vector2(3, -3));

                Stage[Vector2.Zero].IsActive = true;
            }
        }

        private void AddRoom(Vector2 roomIndex)
        {
            Room tRoom = new Room();
            tRoom.Initialize(ElementType.Neutral);
            tRoom.GenerateRoom(roomIndex);
            Stage.Add(roomIndex, tRoom);
        }
    }
}
