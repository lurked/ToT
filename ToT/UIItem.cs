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
    public class UIItem
    {
        public UIItemType ItemType;
        public string Text;
        public Vector2 Position;
        public UIAction Action;
        public string ActionText;
        public Vector2 ItemSize;
        public Rectangle ItemRect;  
        public Color TextColor;
        public SpriteFont TextFont;
        public string ImageName;
        private UIItemsFlow uiFlow;

        public UIItem()
        {
            ItemType = UIItemType.TextFix;
            Text = "Item not properly initialized.";
            Position = Vector2.Zero;
            Action = UIAction.None;
            TextFont = ScreenManager.Fonts[Font.menuItem02.ToString()];
            ItemSize = TextFont.MeasureString(Text);
            TextColor = Color.White;
            ImageName = "resource_gold";
            uiFlow = UIItemsFlow.Vertical;
        }

        public UIItem(UIItemType iType, string text, Color textColor, SpriteFont textFont, UIItemsFlow flow, UIAction action = UIAction.None, string imageName = "resource_gold")
        {
            uiFlow = flow;
            ImageName = imageName;
            ItemType = iType;
            Text = text;
            Position = Vector2.Zero;
            TextFont = textFont;
            Vector2 vText = TextFont.MeasureString(Text);
            if (ItemType == UIItemType.ImageFix || ItemType == UIItemType.ImageFloating || ItemType == UIItemType.ImageText || ItemType == UIItemType.TextImage)
            {
                Vector2 vImage = new Vector2(ScreenManager.Textures2D[ImageName].Width, ScreenManager.Textures2D[ImageName].Height);
                ItemSize = new Vector2(vText.X + vImage.X, vText.Y < vImage.Y ? vImage.Y : vText.Y);
            }
            else
                ItemSize = vText;
            TextColor = textColor;
            Action = action;
        }

    }
}
