using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Rooms
{
    public class UIItem
    {
        public UIItemType ItemType;
        public string Text;
        public Vector2 Position;
        public UIAction Action;
        public Vector2 ItemSize;
        public Color TextColor;
        public SpriteFont TextFont;

        public UIItem()
        {
            ItemType = UIItemType.TextFix;
            Text = "Item not properly initialized.";
            Position = Vector2.Zero;
            Action = UIAction.None;
            TextFont = ScreenManager.Fonts[Font.menuItem02.ToString()];
            ItemSize = TextFont.MeasureString(Text);
            TextColor = Color.White;
        }

        public UIItem(UIItemType iType, string text, Color textColor, SpriteFont textFont, UIAction action = UIAction.None)
        {
            ItemType = iType;
            Text = text;
            Position = Vector2.Zero;
            TextFont = textFont;
            ItemSize = TextFont.MeasureString(Text);
            TextColor = textColor;
        }

    }
}
