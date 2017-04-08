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

        public Level()
        {
            Decors = new List<Decor>();
        }

        public void GenerateLevel(World world, LevelType type, string name, string lParams)
        {
            LType = type;
            Name = name;
            Params = lParams;

            if (name == "base01")
            {
                Decors.Add(new Decor(world, "Starting Platform", ThingType.Decor, new Vector2(0, 580), "grassHalfMid", "Base Starting Platform"));
                Decors[Decors.Count - 1].Initialize();
                Decors.Add(new Decor(world, "Starting Platform", ThingType.Decor, new Vector2(35, 555), "grassHalfMid", "Base Starting Platform"));
                Decors[Decors.Count - 1].Initialize();
                Decors.Add(new Decor(world, "Starting Platform", ThingType.Decor, new Vector2(70, 530), "grassHalfMid02", "Base Starting Platform"));
                Decors[Decors.Count - 1].Initialize();
                Decors.Add(new Decor(world, "Starting Platform", ThingType.Decor, new Vector2(105, 505), "grassHalfMid02", "Base Starting Platform"));
                Decors[Decors.Count - 1].Initialize();
                Decors.Add(new Decor(world, "Starting Platform", ThingType.Decor, new Vector2(140, 480), "grassHalfMid", "Base Starting Platform"));
                Decors[Decors.Count - 1].Initialize();
                Decors.Add(new Decor(world, "Starting Platform", ThingType.Decor, new Vector2(175, 455), "grassHalfMid", "Base Starting Platform"));
                Decors[Decors.Count - 1].Initialize();
                Decors.Add(new Decor(world, "Starting Platform", ThingType.Decor, new Vector2(210, 430), "grassHalfMid02", "Base Starting Platform"));
                Decors[Decors.Count - 1].Initialize();
                Decors.Add(new Decor(world, "Starting Platform", ThingType.Decor, new Vector2(235, 405), "grassHalfMid02", "Base Starting Platform"));
                Decors[Decors.Count - 1].Initialize();
                Decors.Add(new Decor(world, "Starting Platform", ThingType.Decor, new Vector2(280, 380), "grassHalfMid", "Base Starting Platform"));
                Decors[Decors.Count - 1].Initialize();
                Decors.Add(new Decor(world, "Starting Platform", ThingType.Decor, new Vector2(350, 380), "grassHalfMid", "Base Starting Platform"));
                Decors[Decors.Count - 1].Initialize();
                Decors.Add(new Decor(world, "Starting Platform", ThingType.Decor, new Vector2(420, 380), "grassHalfMid", "Base Starting Platform"));
                Decors[Decors.Count - 1].Initialize();
                Decors.Add(new Decor(world, "Starting Platform", ThingType.Decor, new Vector2(490, 380), "grassHalfMid", "Base Starting Platform"));
                Decors[Decors.Count - 1].Initialize();
                Decors.Add(new Decor(world, "Starting Platform", ThingType.Decor, new Vector2(560, 380), "grassHalfMid", "Base Starting Platform"));
                Decors[Decors.Count - 1].Initialize();
                Decors.Add(new Decor(world, "Starting Platform", ThingType.Decor, new Vector2(630, 380), "grassHalfMid", "Base Starting Platform"));
                Decors[Decors.Count - 1].Initialize();
                Decors.Add(new Decor(world, "Starting Platform", ThingType.Decor, new Vector2(700, 440), "grassHalfMid", "Base Starting Platform"));
                Decors[Decors.Count - 1].Initialize();
                Decors.Add(new Decor(world, "Starting Platform", ThingType.Decor, new Vector2(770, 480), "grassHalfMid", "Base Starting Platform"));
                Decors[Decors.Count - 1].Initialize();
                Decors.Add(new Decor(world, "Starting Platform", ThingType.Decor, new Vector2(840, 530), "grassHalfMid", "Base Starting Platform"));
                Decors[Decors.Count - 1].Initialize();
            }
        }
    }
}
