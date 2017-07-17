using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Rooms
{
    public static class StaticRandom
    {
        private static int seed;

        private static ThreadLocal<Random> threadLocal = new ThreadLocal<Random>
            (() => new Random(Interlocked.Increment(ref seed)));

        static StaticRandom()
        {
            seed = Environment.TickCount;
        }

        public static Random Instance { get { return threadLocal.Value; } }

    }

    public static class Tools
    {
        public static bool Intersects(Vector2 pointToCheck, Rectangle rectToCheck)
        {
            bool intersects = false;

            if (pointToCheck.X >= rectToCheck.X && pointToCheck.X <= rectToCheck.X + rectToCheck.Width
                && pointToCheck.Y >= rectToCheck.Y && pointToCheck.Y <= rectToCheck.Y + rectToCheck.Height)
                intersects = true;

            return intersects;
        }
    }
}