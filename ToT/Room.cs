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

        public Tile()
        { 
            Position = Vector2.Zero;
            //RoomPosition = Vector2.Zero;
            Decors = new List<Decor>();
            Resources = new Dictionary<ResourceType, int>();
            Upgrades = new Dictionary<UpgradeType, int>();
            IsActive = false;
        }
        public Vector2 RoomPosition
        {
            get { return Position * (ScreenManager.TileSize+new Vector2(1, 1)); }
        }
        public void Initialize()
        {
            ImageName = "pal_green_" + ScreenManager.TSize;            
        }

        public void GenerateRoom(Vector2 position, int lvl)
        {
            Position = position;
            Lvl = lvl;
            string tName = "BasicRoom:" + Lvl;
            Decor tD = new Decor(tName, ThingType.Decor, Position, ImageName, tName);
            Decors.Add(tD);
            Decors[Decors.Count - 1].Initialize();
            GeneratePods();
        }

        public void GeneratePods()
        {
            switch(Lvl)
            {
                case 0:
                    Resources.Add(ResourceType.Gold, 1);
                    Resources.Add(ResourceType.Food, 1);
                    Resources.Add(ResourceType.Wood, 1);
                    Resources.Add(ResourceType.Production, 1);
                    Resources.Add(ResourceType.Energy, 1);
                    break;
                case 1:
                    AddRandomResources(ResourceType.Gold, 0, 1);
                    AddRandomResources(ResourceType.Food, 0, 1);
                    AddRandomResources(ResourceType.Wood, 0, 1);
                    AddRandomResources(ResourceType.Production, 0, 1);
                    AddRandomResources(ResourceType.Energy, 0, 1);
                    break;
                case 2:
                    AddRandomResources(ResourceType.Gold, 0, 2);
                    AddRandomResources(ResourceType.Food, 0, 2);
                    AddRandomResources(ResourceType.Wood, 0, 2);
                    AddRandomResources(ResourceType.Production, 0, 2);
                    AddRandomResources(ResourceType.Energy, 0, 2);
                    break;
                case 3:
                    AddRandomResources(ResourceType.Gold, 0, 3);
                    AddRandomResources(ResourceType.Food, 0, 3);
                    AddRandomResources(ResourceType.Wood, 0, 3);
                    AddRandomResources(ResourceType.Production, 0, 3);
                    AddRandomResources(ResourceType.Energy, 0, 3);
                    break;
                case 4:
                    AddRandomResources(ResourceType.Gold, 0, 4);
                    AddRandomResources(ResourceType.Food, 0, 4);
                    AddRandomResources(ResourceType.Wood, 0, 4);
                    AddRandomResources(ResourceType.Production, 0, 4);
                    AddRandomResources(ResourceType.Energy, 0, 4);
                    break;
                case 5:
                    AddRandomResources(ResourceType.Gold, 0, 5);
                    AddRandomResources(ResourceType.Food, 0, 5);
                    AddRandomResources(ResourceType.Wood, 0, 5);
                    AddRandomResources(ResourceType.Production, 0, 5);
                    AddRandomResources(ResourceType.Energy, 0, 5);
                    break;
                default:
                    AddRandomResources(ResourceType.Gold, 0, 1);
                    AddRandomResources(ResourceType.Food, 0, 1);
                    AddRandomResources(ResourceType.Wood, 0, 1);
                    AddRandomResources(ResourceType.Production, 0, 1);
                    AddRandomResources(ResourceType.Energy, 0, 1);
                    break;
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
