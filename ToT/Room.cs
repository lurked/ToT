using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
namespace ToT
{
    public class Tile
    {
        //public Vector2 RoomPosition;
        public Vector2 Position;
        public List<Decor> Decors;
        public string ImageName;
        public int Lvl;
        public bool IsActive;
        public Dictionary<ResourceType, int> Resources;
        public Dictionary<UpgradeType, int> Upgrades;
        public bool IsSpawn;
        public Building TileBuilding;

        public Tile()
        { 
            Position = Vector2.Zero;
            //RoomPosition = Vector2.Zero;
            Decors = new List<Decor>();
            Resources = new Dictionary<ResourceType, int>();
            Upgrades = new Dictionary<UpgradeType, int>();
            IsActive = false;
            IsSpawn = false;
        }
        public Vector2 RoomPosition
        {
            get { return Position * (ScreenManager.TileSize + new Vector2(1, 1)); }
        }

        public Vector2 BuildingPosition
        {
            get { return RoomPosition + ScreenManager.TileSize/2 - new Vector2(64, 64)/2; }
        }

        public void Initialize()
        {
            ImageName = "cotton_green_" + ScreenManager.TSize;
        }

        public void GenerateRoom(Vector2 position, int lvl)
        {
            Position = position;
            Lvl = lvl;
            string tName = "BasicRoom:" + Lvl;

            GeneratePods();

            switch (Lvl)
            {
                case 0:
                    ImageName = "main_01_" + ScreenManager.TSize;
                    break;
                default:
                    ImageName = "cotton_green_" + ScreenManager.TSize;
                    break;
            }
            if (ImageName != "main_01_" + ScreenManager.TSize)
                if (Position.X == 0 || Position.Y == 0)
                    ImageName = "dirt_" + ScreenManager.TSize;

            Decor tD = new Decor(tName, ThingType.Decor, Position, ImageName, tName);
            Decors.Add(tD);
            Decors[Decors.Count - 1].Initialize();
        }

        public void AddResources(int qty, ResourceType res)
        {
            if (Resources.ContainsKey(res))
                Resources[res] += qty;
            else
                Resources.Add(res, qty);
            ScreenManager.GGPScreen.RefreshIncome();
        }

        public void AddThing(int qty, ThingType tType)
        {
            switch(tType)
            {
                case ThingType.Tower:
                    SetBuilding(BuildingType.Tower_Normal, qty);
                    break;
            }
        }

        public void SetBuilding(BuildingType tType, int lvl)
        {
            TileBuilding = new Building
            {
                TType = tType,
                Level = lvl
            };
        }

        public void GeneratePods()
        {
            if (Position.X == 0 && Position.Y == 0)
            {
                Resources.Add(ResourceType.Gold, 1);
                Resources.Add(ResourceType.Food, 1);
                Resources.Add(ResourceType.Production, 1);
                Resources.Add(ResourceType.Energy, 1);
            }
            else if (Position.X == 0 || Position.Y == 0)
                MaybeSetAsSpawn(Lvl);
            else
                switch (Lvl)
                {
                    case 0:

                        break;
                    case 1:
                        AddRandomResources(ResourceType.Gold, 0, 1);
                        AddRandomResources(ResourceType.Food, 0, 1);
                        AddRandomResources(ResourceType.Production, 0, 1);
                        AddRandomResources(ResourceType.Energy, 0, 1);
                        break;
                    case 2:
                        AddRandomResources(ResourceType.Gold, 0, 2);
                        AddRandomResources(ResourceType.Food, 0, 2);
                        AddRandomResources(ResourceType.Production, 0, 2);
                        AddRandomResources(ResourceType.Energy, 0, 2);
                        break;
                    case 3:
                        AddRandomResources(ResourceType.Gold, 0, 3);
                        AddRandomResources(ResourceType.Food, 0, 3);
                        AddRandomResources(ResourceType.Production, 0, 3);
                        AddRandomResources(ResourceType.Energy, 0, 3);
                        break;
                    case 4:
                        AddRandomResources(ResourceType.Gold, 0, 4);
                        AddRandomResources(ResourceType.Food, 0, 4);
                        AddRandomResources(ResourceType.Production, 0, 4);
                        AddRandomResources(ResourceType.Energy, 0, 4);
                        break;
                    case 5:
                        AddRandomResources(ResourceType.Gold, 0, 5);
                        AddRandomResources(ResourceType.Food, 0, 5);
                        AddRandomResources(ResourceType.Production, 0, 5);
                        AddRandomResources(ResourceType.Energy, 0, 5);
                        break;
                    default:
                        AddRandomResources(ResourceType.Gold, 0, 1);
                        AddRandomResources(ResourceType.Food, 0, 1);
                        AddRandomResources(ResourceType.Production, 0, 1);
                        AddRandomResources(ResourceType.Energy, 0, 1);
                        break;
                }
        }

        private void MaybeSetAsSpawn(int Lvl)
        {
            int breakpoint;
            switch (Lvl)
            {
                case 0:
                    breakpoint = 100;
                    break;
                case 1:
                    breakpoint = 100;
                    break;
                case 2:
                    breakpoint = 80;
                    break;
                case 3:
                    breakpoint = 75;
                    break;
                case 4:
                    breakpoint = 70;
                    break;
                case 5:
                    breakpoint = 65;
                    break;
                case 6:
                    breakpoint = 60;
                    break;
                case 7:
                    breakpoint = 55;
                    break;
                case 8:
                    breakpoint = 50;
                    break;
                case 9:
                    breakpoint = 45;
                    break;
                case 10:
                    breakpoint = 40;
                    break;
                case 11:
                    breakpoint = 35;
                    break;
                default:
                    breakpoint = 100;
                    break;
            }
            int iRand = StaticRandom.Instance.Next(0, 100);
            if (iRand >= breakpoint)
            {
                IsSpawn = true;
                SetBuilding(BuildingType.Spawn_Enemy_Basic, Lvl);
            }
        }

        private int AddRandomResources(ResourceType res, int minRes, int maxRes)
        {
            int iRand = StaticRandom.Instance.Next(minRes, maxRes + 1);

            if (iRand > 0)
                Resources.Add(res, iRand);
            return iRand;
        }
    }
}
