using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace ToT
{
    public class UI
    {
        public string ID;
        public string Name;
        public UIType uiType;
        public List<UIItem> Items;
        public UIItemsFlow ItemsFlow = UIItemsFlow.Vertical;
        public Vector2 Position;
        public Vector2 Size;
        public Color BackColor;
        public float BackAlpha = 1f;
        public bool IsActive = false;
        public bool ToDraw = true;
        public string TTText;

        Vector2 coor;
        Texture2D rect;
        Color[] data;

        public UI()
        {
            ID = "Unkown";
            uiType = UIType.Basic;
            Items = new List<UIItem>();
            BackColor = Color.DarkSlateGray;
        }

        public UI(UIType type, string uiID, string name, Vector2 size, Vector2 position)
        {
            uiType = type;
            ID = uiID;
            Name = name;
            Size = size;
            Position = position;
            Items = new List<UIItem>();
            BackColor = Color.DarkSlateGray;
        }

        public void Unload()
        {
            uiType = UIType.Basic;
            ID = "";
            Name = "";
            Size = Vector2.Zero;
            Position = Vector2.Zero;
            Items = null;
            BackColor = Color.DarkSlateGray;
        }

        public void SetUIItems(List<UIItem> menuItems)
        {
            Items = menuItems;
            RefreshSize();
            UpdateItemsPosition();
        }

        public void RefreshSize()
        {
            float tX = 4f;
            float tY = 4f;
            bool exitLoop = false;

            foreach(UIItem tUII in Items)
            {
                if (tUII.ToShow)
                {
                    switch (ItemsFlow)
                    {
                        case UIItemsFlow.Vertical:
                            if (tX < tUII.ItemSize.X)
                                tX = tUII.ItemSize.X + 4f;
                            tY += tUII.ItemSize.Y;
                            break;
                        case UIItemsFlow.Horizontal:
                            if (tY < tUII.ItemSize.Y)
                                tY = tUII.ItemSize.Y + 4f;
                            tX += tUII.ItemSize.X + 2f;
                            break;
                        default:
                            tX = Size.X;
                            tY = Size.Y;
                            exitLoop = true;
                            break;
                    }
                    if (exitLoop)
                        break;
                }
                
            }

            Size = new Vector2(tX, tY);
        }

        public void UpdateItemsPosition()
        {
            float tX = 2f;
            float tY = 2f;

            foreach (UIItem tUII in Items)
            {
                tUII.Position = new Vector2(tX, tY);
                if (tUII.ToShow)
                {
                    switch (ItemsFlow)
                    {
                        case UIItemsFlow.Vertical:
                            tUII.ItemRect = new Rectangle((int)tX, (int)tY, (int)Size.X, (int)tUII.ItemSize.Y);
                            tY += tUII.ItemSize.Y;
                            break;
                        case UIItemsFlow.Horizontal:
                            tUII.ItemRect = new Rectangle((int)tX, (int)tY, (int)tUII.ItemSize.X, (int)Size.Y);
                            tX += tUII.ItemSize.X;
                            break;
                    }
                }
            }
            switch(uiType)
            {
                case UIType.Basic:
                case UIType.BasicOpaque:
                    rect = new Texture2D(ScreenManager.Sprites.GraphicsDevice, (int)Size.X, (int)Size.Y);
                    data = new Color[(int)Size.X * (int)Size.Y];
                    for (int i = 0; i < data.Length; ++i) data[i] = BackColor;
                    rect.SetData(data);
                    coor = Position;
                    break;
                default:

                    break;
            }
            
        }

        public void Update()
        {

            switch (uiType)
            {
                case UIType.Basic:
                case UIType.BasicOpaque:
                    coor = Position;
                    break;
                default:

                    break;
            }
        }

        public void Draw(GameTime gameTime)
        {
            if (ToDraw)
            {

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

                foreach (UIItem tUII in Items)
                {
                    if (tUII.ToShow)
                        switch (tUII.ItemType)
                        {
                            case UIItemType.ImageFix:
                                ScreenManager.Sprites.Draw(ScreenManager.Textures2D[tUII.ImageName], tUII.Position + Position, null, Color.White);
                                break;
                            case UIItemType.TextImage:
                                ScreenManager.Sprites.DrawString(tUII.TextFont, tUII.Text, tUII.Position + Position, tUII.TextColor);
                                ScreenManager.Sprites.Draw(ScreenManager.Textures2D[tUII.ImageName], tUII.Position + Position + new Vector2(tUII.ItemSize.X, 0), null, Color.White);
                                break;
                            case UIItemType.ImageText:
                                ScreenManager.Sprites.Draw(ScreenManager.Textures2D[tUII.ImageName], tUII.Position + Position, null, Color.White);
                                ScreenManager.Sprites.DrawString(tUII.TextFont, tUII.Text, tUII.Position + Position + new Vector2(ScreenManager.Textures2D[tUII.ImageName].Width, 0), tUII.TextColor);
                                break;
                            default:
                                ScreenManager.Sprites.DrawString(tUII.TextFont, tUII.Text, tUII.Position + Position, tUII.TextColor);
                                break;
                        }

                }
                if (ScreenManager.DebugMode)
                    ScreenManager.Sprites.DrawString(ScreenManager.Fonts[Font.debug01.ToString()], Position.X + ":" + Position.Y + "|Size=" + Size.X + ":" + Size.Y, Position, Color.Red, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
            }
        }
    }
}
