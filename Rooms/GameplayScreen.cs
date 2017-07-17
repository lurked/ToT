using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Rooms
{
    public class GameplayScreen : GameScreen
    {
        public Level CurrentLevel;
        public Player Player1;
        public static int Turn = 0;
        public Dictionary<ResourceType, int> Resources;
        public Dictionary<ResourceType, int> Income;
        public Dictionary<UITemplate, UI> GameUIs;
        public UI ActiveUI;

        public GameplayScreen()
        {

        }

        public void Initialize()
        {
            Resources = new Dictionary<ResourceType, int>();
            Resources.Add(ResourceType.Gold, 0);
            Resources.Add(ResourceType.Food, 0);
            Resources.Add(ResourceType.Wood, 0);
            Resources.Add(ResourceType.Energy, 0);
            Resources.Add(ResourceType.Production, 0);

            Income = new Dictionary<ResourceType, int>();
            Income.Add(ResourceType.Gold, 0);
            Income.Add(ResourceType.Food, 0);
            Income.Add(ResourceType.Wood, 0);
            Income.Add(ResourceType.Energy, 0);
            Income.Add(ResourceType.Production, 0);

            CurrentLevel = new Level();
            CurrentLevel.GenerateLevel(LevelType.Static, "base01", "");

            Player1 = new Player("Noob", ThingType.Player, ScreenManager.TileSize / 2, "player01", "A noob player");
            Player1.Initialize();
            Player1.SetStage(CurrentLevel.Stage);
            Player1.SetActiveRoom(CurrentLevel.Stage[Vector2.Zero]);

            GameUIs = new Dictionary<UITemplate, UI >();
            UI tUI = GenerateUI(UITemplate.toolbar01);
            GameUIs.Add(UITemplate.toolbar01, tUI);
            UI tUI2 = GenerateUI(UITemplate.turn01);
            GameUIs.Add(UITemplate.turn01, tUI2);
            UI tUI3 = GenerateUI(UITemplate.log);
            GameUIs.Add(UITemplate.log, tUI3);
            UI tUI4 = GenerateUI(UITemplate.income);
            GameUIs.Add(UITemplate.income, tUI4);

            RefreshResources();
            RefreshIncome();
        }



        public void UpdateUIs(GameTime gameTime)
        {
            foreach (KeyValuePair<UITemplate, UI> ui in GameUIs)
                ui.Value.Update();
        }

        private UI GenerateUI(UITemplate uiName)
        {
            UI tUI;
            switch(uiName)
            {
                case UITemplate.toolbar01:
                    tUI = new UI(UIType.Basic, uiName.ToString(), "Main Tool Bar", new Vector2(300, 300), new Vector2(2, 2));
                    tUI.BackAlpha = 0.35f;
                    tUI.ItemsFlow = UIItemsFlow.Vertical;
                    RefreshResourcesUI(tUI);
                    tUI.Position = new Vector2(2, 2) + ScreenManager.PlayerCamera.Position;
                    break;
                case UITemplate.income:
                    tUI = new UI(UIType.Basic, uiName.ToString(), "Income Panel", new Vector2(300, 300), new Vector2(2, 2));
                    tUI.BackAlpha = 0.35f;
                    tUI.ItemsFlow = UIItemsFlow.Vertical;
                    RefreshIncomeUI(tUI);
                    tUI.Position = new Vector2(2 + ScreenManager.Resolution.X - 100, 2) + ScreenManager.PlayerCamera.Position;
                    break;
                case UITemplate.turn01:
                    tUI = new UI(UIType.Basic, uiName.ToString(), "Turns UI", new Vector2(300, 300), new Vector2(2, 2));
                    tUI.BackAlpha = 0.35f;
                    List<UIItem> tUIItems;
                    tUIItems = new List<UIItem>();
                    tUIItems.Add(new UIItem(UIItemType.TextFix, "End Turn", Color.CornflowerBlue, ScreenManager.Fonts[Font.menuItem01.ToString()], UIAction.EndTurn));
                    tUI.SetUIItems(tUIItems);
                    tUI.Position = new Vector2(ScreenManager.Resolution.X - tUI.Size.X, ScreenManager.Resolution.Y - tUI.Size.Y) + ScreenManager.PlayerCamera.Position;
                    break;
                case UITemplate.log:
                    tUI = new UI(UIType.BasicInvis, uiName.ToString(), "Events Journal", new Vector2(300, 300), new Vector2(2, ScreenManager.Resolution.Y - 180));
                    tUI.BackAlpha = 0.35f;
                    tUI.ItemsFlow = UIItemsFlow.Horizontal;
                    tUIItems = new List<UIItem>();
                    tUIItems.Add(new UIItem(UIItemType.TextFix, "Events Journal", Color.CornflowerBlue, ScreenManager.Fonts[Font.menuItem02.ToString()], UIAction.None));
                    tUI.SetUIItems(tUIItems);
                    tUI.Position = new Vector2(2, ScreenManager.Resolution.Y - 180) + ScreenManager.PlayerCamera.Position;
                    break;
                default:
                    tUI = new UI();
                    break;
            }
            return tUI;
        }

        public void CheckHoverUI(Vector2 RealMousePosition)
        {
            ActiveUI = null;
            foreach (KeyValuePair<UITemplate, UI> ui in GameUIs)
            {
                if (Tools.Intersects(RealMousePosition, new Rectangle((int)ui.Value.Position.X, (int)ui.Value.Position.Y, (int)ui.Value.Size.X, (int)ui.Value.Size.Y)))
                {
                    ActiveUI = ui.Value;
                    ui.Value.IsActive = true;
                }
                else
                    ui.Value.IsActive = false;
            }
        }

        public void RefreshResources()
        {
            Resources[ResourceType.Gold] = 0;
            Resources[ResourceType.Food] = 0;
            Resources[ResourceType.Wood] = 0;
            Resources[ResourceType.Production] = 0;
            Resources[ResourceType.Energy] = 0;
            foreach (KeyValuePair<Vector2, Room> room in CurrentLevel.Stage)
                foreach (KeyValuePair<ResourceType, int> res in room.Value.Resources)
                    Resources[res.Key] += res.Value;

            RefreshResourcesUI(GameUIs[UITemplate.toolbar01]);
        }

        public void RefreshIncome()
        {
            Income[ResourceType.Gold] = 0;
            Income[ResourceType.Food] = 0;
            Income[ResourceType.Wood] = 0;
            Income[ResourceType.Production] = 0;
            Income[ResourceType.Energy] = 0;

            foreach (KeyValuePair<Vector2, Room> room in CurrentLevel.Stage)
                foreach (KeyValuePair<ResourceType, int> res in room.Value.Resources)
                    Income[res.Key] += res.Value;

            RefreshIncomeUI(GameUIs[UITemplate.income]);
        }

        public void IncrementResources()
        {
            foreach (KeyValuePair<Vector2, Room> room in CurrentLevel.Stage)
                foreach (KeyValuePair<ResourceType, int> res in room.Value.Resources)
                    Resources[res.Key] += res.Value;

            RefreshResourcesUI(GameUIs[UITemplate.toolbar01]);
        }

        private void RefreshResourcesUI(UI tUI)
        {
            List<UIItem> tUIItems = new List<UIItem>();
            tUIItems.Add(new UIItem(UIItemType.TextFix, "Turn " + Turn, Color.CornflowerBlue, ScreenManager.Fonts[Font.menuItem03.ToString()], UIAction.None));
            foreach (KeyValuePair<ResourceType, int> res in Resources)
                tUIItems.Add(new UIItem(UIItemType.TextFix, res.Value.ToString() + " " + res.Key.ToString(), Color.White, ScreenManager.Fonts[Font.menuItem03.ToString()]));
            tUI.SetUIItems(tUIItems);
        }

        private void RefreshIncomeUI(UI tUI)
        {
            List<UIItem> tUIItems = new List<UIItem>();
            foreach (KeyValuePair<ResourceType, int> res in Income)
                tUIItems.Add(new UIItem(UIItemType.TextFix, res.Value.ToString() + " " + res.Key.ToString(), Color.White, ScreenManager.Fonts[Font.menuItem03.ToString()]));
            tUI.SetUIItems(tUIItems);
        }

        public void ExecuteMenuAction(UIAction action, string actionText)
        {
            switch(action)
            {
                case UIAction.EndTurn:
                    Turn += 1;
                    IncrementResources();
                    break;
                default:

                    break;
            }
        }

        public override void Update(GameTime gameTime, InputManager input)
        { 
            base.Update(gameTime, input);
            CheckHoverUI(input.MousePosition() + ScreenManager.PlayerCamera.Position);
            if (ScreenManager.Input.MousePressed())
            {
                if (ActiveUI != null)
                {
                    foreach(UIItem uiI in ActiveUI.Items)
                    {
                        //if (Tools.Intersects(input.MousePosition() + ScreenManager.PlayerCamera.Position, uiI.ItemRect))
                        if (Tools.Intersects(input.MousePosition() + ScreenManager.PlayerCamera.Position, new Rectangle(uiI.ItemRect.X + (int)ActiveUI.Position.X, uiI.ItemRect.Y + (int)ActiveUI.Position.Y, uiI.ItemRect.Width, uiI.ItemRect.Height)))
                            ExecuteMenuAction(uiI.Action, uiI.ActionText);
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            foreach (KeyValuePair<Vector2, Room> room in CurrentLevel.Stage)
                DrawRoom(room.Value);

            ScreenManager.Sprites.Draw(ScreenManager.Textures2D[Player1.ImageName], Player1.Position + (ScreenManager.TileSize / 2), null, Color.White, 0f, new Vector2(Player1.Rect.Width / 2, Player1.Rect.Height / 2), 1f, SpriteEffects.None, 0f);
            if (ScreenManager.DebugMode)
            {
                ScreenManager.Sprites.DrawString(ScreenManager.Fonts[Font.debug01.ToString()], (int)Player1.Position.X + ":" + (int)Player1.Position.Y, Player1.Position - new Vector2(24, 24), Color.Red, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
                ScreenManager.Sprites.DrawString(ScreenManager.Fonts[Font.debug01.ToString()], (ScreenManager.Input.MousePosition() + ScreenManager.PlayerCamera.Position).ToString(), ScreenManager.Input.MousePosition() + ScreenManager.PlayerCamera.Position, Color.Red, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
            }

            foreach (KeyValuePair<UITemplate, UI> ui in GameUIs)
                ui.Value.Draw(gameTime);
        }

        private void DrawRoom(Room room)
        {
            float resHeight = ScreenManager.Fonts[Font.debug02.ToString()].MeasureString("Gold:").Y;

            foreach (Thing tT in room.Decors)
                if (tT.ToDraw)
                {
                    Color tCol = Color.White;
                    if (room.IsActive)
                        tCol = Color.LightGray;
                    float resX = 1f;
                    float resY = 1f;
                    ScreenManager.Sprites.Draw(ScreenManager.Textures2D[tT.ImageName], tT.Position + (room.Position * ScreenManager.TSize), null, tCol);
                    foreach(KeyValuePair<ResourceType, int> res in room.Resources)
                    {
                        ScreenManager.Sprites.DrawString(ScreenManager.Fonts[Font.debug02.ToString()], res.Value.ToString() + " " + res.Key.ToString(), (room.Position + new Vector2(resX, resY)) + (room.Position * ScreenManager.TSize), Color.White, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
                        resY += (resHeight * 0.6f) - 1f;
                    }
                    //if (ScreenManager.DebugMode)
                    //    ScreenManager.Sprites.DrawString(ScreenManager.Fonts[Font.debug02.ToString()], (int)tT.Coords.X + ":" + (int)tT.Coords.Y, (tT.Position - new Vector2(15, 15)) + (room.Position * ScreenManager.TSize), Color.DarkRed, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
                }
            if (ScreenManager.DebugMode)
                ScreenManager.Sprites.DrawString(ScreenManager.Fonts[Font.debug02.ToString()], (int)room.Position.X + ":" + (int)room.Position.Y, (room.Position - new Vector2(15, 15)) + (room.Position * ScreenManager.TSize), Color.DarkRed, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
        }
    }
}
