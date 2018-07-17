using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ToT
{
    public class Map : UI
    {
        private List<Vector2> TilesPositions { get; set; }
        private Vector2 MapSize { get; set; }

        public Map(UIType type, string uiID, string name, Vector2 size, Vector2 position)
        {
            uiType = type;
            ID = uiID;
            Name = name;
            Size = size;
            Position = position;
            Items = new List<UIItem>();
            BackColor = Color.DarkSlateGray;
            TilesPositions = new List<Vector2>();
            MapSize = new Vector2(12, 12); //X tiles to the east and west, Y tiles to the north and south. So actually 12x12 = 25x25 minimap.


        }

        public void RefreshMap(Dictionary<Vector2, Tile> stage)
        {
            TilesPositions = new List<Vector2>();
            Vector2 pPos = ScreenManager.GGPScreen.Player1.Coords;
            foreach (KeyValuePair<Vector2, Tile> tTile in stage)
            {
                if (tTile.Key.X >= pPos.X - MapSize.X &&
                        tTile.Key.X <= pPos.X + MapSize.X &&
                        tTile.Key.Y >= pPos.Y - MapSize.Y &&
                        tTile.Key.Y <= pPos.Y + MapSize.Y)
                {
                    TilesPositions.Add(tTile.Key);
                }
            }
            RefreshSize();
            UpdateItemsPosition();
        }

        public override void RefreshSize()
        {
            Size = (new Vector2((MapSize.X * 2) + 1, (MapSize.Y * 2) + 1)) * new Vector2(8, 8);
        }

        //public override void UpdateItemsPosition()
        //{
        //    switch (uiType)
        //    {
        //        case UIType.Basic:
        //        case UIType.BasicOpaque:
        //            rect = new Texture2D(ScreenManager.Sprites.GraphicsDevice, (int)Size.X, (int)Size.Y);
        //            data = new Color[(int)Size.X * (int)Size.Y];
        //            for (int i = 0; i < data.Length; ++i) data[i] = BackColor;
        //            rect.SetData(data);
        //            coor = Position;
        //            break;
        //        default:

        //            break;
        //    }

        //}


        public override void Draw(GameTime gameTime)
        {
            if (ToDraw)
            {
                Vector2 tHalf = ((new Vector2((MapSize.X * 2) + 1, (MapSize.Y * 2) + 1)) * new Vector2(8, 8)) / 2;
                Color SelectColor = Color.White;
                float tBack = BackAlpha;
                if (IsActive)
                {
                    SelectColor = Color.DarkGray;
                    tBack = 1f;
                }

                switch (uiType)
                {
                    case UIType.Basic:
                    case UIType.BasicOpaque:
                        ScreenManager.Sprites.Draw(rect, coor, SelectColor * tBack);
                        break;
                    default:

                        break;
                }
                string sIName;
                foreach(Vector2 tV in TilesPositions)
                {
                    if (tV.X == 0 || tV.Y == 0)
                        sIName = "dirt_8";
                    else
                        sIName = "cotton_green_8";
                    ScreenManager.Sprites.Draw(ScreenManager.Textures2D[sIName], ((tV * 8) + Position) + tHalf, null, Color.White);
                }
            }
        }
    }
}
