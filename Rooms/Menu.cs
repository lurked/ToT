using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Rooms
{
    public class Menu
    {
        public List<MenuItem> Items { get; set; }
        public bool IsActive { get; set; }
        public Vector2 Position { get; set; }
        public int CurrentItem { get; set; } //The currently selected MenuItem.
        public string CurrentAction { get; set; } //Contains the next action to do, is empty if there is no action to execute.
        public bool IsSelectable { get; set; }

        public Menu()
        {
            Position = new Vector2(10, 320);
            Items = new List<MenuItem>();
            IsActive = true;
            CurrentItem = -1;
            IsSelectable = false;
        }

        public class MenuItem
        {
            public MenuType ItemType { get; set; }
            public string DisplayText { get; set; }
            public string Action { get; set; }
            public string ImageName { get; set; }
            public bool IsActive { get; set; }
            public float Order { get; set; }
            public Vector2 Position { get; set; }
            public Color TextColor { get; set; }

            public MenuItem()
            {
                ItemType = MenuType.Text;
                IsActive = true;
                TextColor = Color.White;
            }

            public MenuItem(string displayText)
            {
                ItemType = MenuType.Text;
                DisplayText = displayText;
                IsActive = true;
                TextColor = Color.White;
            }

            public MenuItem(string displayText, MenuType itemType, Color textColor, string action = "", string imageName = "")
            {
                ItemType = itemType;
                DisplayText = displayText;
                IsActive = true;
                ImageName = imageName;
                TextColor = textColor;
                Action = action;
            }
        }

        public void Update(GameTime gameTime, InputManager input)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (Items[i].IsActive)
                    if (Items[i].Position == Vector2.Zero || Items[i].Position == null)
                    {
                        Items[i].Position = Position + new Vector2(0, i * 24);
                    }
            }
            if (IsActive && IsSelectable)
            {
                if (input.KeyPressed(Keys.Down, Keys.Right, Keys.S, Keys.D) || input.ButtonPressed(Buttons.DPadDown, Buttons.DPadRight))
                    if (CurrentItem < Items.Count - 1)
                        CurrentItem++;
                if (input.KeyPressed(Keys.Up, Keys.Left, Keys.W, Keys.A) || input.ButtonPressed(Buttons.DPadUp, Buttons.DPadLeft))
                    if (CurrentItem > 0)
                        CurrentItem--;
                if (input.KeyPressed(Keys.Enter, Keys.Space) || input.ButtonPressed(Buttons.Start, Buttons.X, Buttons.A))
                    CurrentAction = Items[CurrentItem].Action;

            }

            
        }

        public void Draw(GameTime gameTime)
        {
            for (int i = 0; i < Items.Count; i++)
            {
                if (IsActive)
                {
                    if (IsSelectable && i == CurrentItem)
                        ScreenManager.Sprites.DrawString(ScreenManager.Fonts[Font.menuItem02.ToString()], "> " + Items[i].DisplayText, Items[i].Position, Color.Red);
                    else
                        ScreenManager.Sprites.DrawString(ScreenManager.Fonts[Font.menuItem02.ToString()], Items[i].DisplayText, Items[i].Position, Items[i].TextColor);
                }
                    
            }
        }


    }
}
