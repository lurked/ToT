using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;


namespace ToT
{
    public class Level
    {
        public LevelType LType;
        public string Name;
        public string Params;
        public List<Decor> Decors;
        public Dictionary<Vector2, Tile> Stage;
        public Dictionary<ResourceType, int> Resources;
        public List<Enemy> Enemies;
        public int Turn;

        public Level()
        {
            Decors = new List<Decor>();
            Stage = new Dictionary<Vector2, Tile>();
            Resources = new Dictionary<ResourceType, int>();
            Enemies = new List<Enemy>();
        }
        
        public void GenerateLevel(LevelType type, string name, string lParams)
        {
            LType = type;
            Name = name;
            Params = lParams;
            Turn = 1;

            if (name == "base01")
            {

                AddRoom(Vector2.Zero, 0);
                AddRoom(new Vector2(1, 0), 1);
                AddRoom(new Vector2(1, 1), 1);
                AddRoom(new Vector2(2, 0), 1);
                AddRoom(new Vector2(3, 0), 2);
                //AddRoom(new Vector2(3, 3));

                AddRoom(new Vector2(0, 1), 1);
                AddRoom(new Vector2(-1, 1), 1);
                AddRoom(new Vector2(0, 2), 1);
                AddRoom(new Vector2(0, 3), 2);
                //AddRoom(new Vector2(-3, 3));

                AddRoom(new Vector2(-1, 0), 1);
                AddRoom(new Vector2(1, -1), 1);
                AddRoom(new Vector2(-2, 0), 1);
                AddRoom(new Vector2(-3, 0), 2);
                //AddRoom(new Vector2(-3, -3));

                AddRoom(new Vector2(0, -1), 1);
                AddRoom(new Vector2(-1, -1), 1);
                AddRoom(new Vector2(0, -2), 1);
                AddRoom(new Vector2(0, -3), 2);
                //AddRoom(new Vector2(3, -3));

                Stage[Vector2.Zero].IsActive = true;
            }
        }

        public void AddRoom(Vector2 roomIndex, int lvl)
        {
            Tile tRoom = new Tile();
            tRoom.Initialize();
            tRoom.GenerateRoom(roomIndex, lvl);
            Stage.Add(roomIndex, tRoom);
            ScreenManager.GGPScreen.CurrentTileLevel = ScreenManager.GGPScreen.GetCurrentTileLevel();
        }

        public void Save()
        {
            string sLevel = FileManager.SerializeLevel(this);
            FileManager.SaveToFile(sLevel, ScreenManager.SAVESPATH + "ToT" + DateTime.Now.ToShortDateString().Replace("/", "") + "T" + Turn.ToString() + FileManager.GAMEFILES_EXT_LEVEL);
        }
    }
}
