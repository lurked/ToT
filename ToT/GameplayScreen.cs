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


namespace ToT
{
    public class GameplayScreen : GameScreen
    {
        public Level CurrentLevel;
        public Player Player1;
        public int Turn = 1;
        public int CurrentTileLevel = 1;
        public Dictionary<ResourceType, int> Resources;
        public Dictionary<ResourceType, int> Income;
        public Dictionary<UITemplate, UI> GameUIs;
        public Dictionary<int, int> TileLevelReqs;
        public List<LogEntry> Log;
        public UI ActiveUI;
        public bool LogToggled = false;
        public bool TTToggled = false;
        public string TTText = "";

        public GameplayScreen()
        {

        }

        public void Initialize()
        {
            TileLevelReqs = new Dictionary<int, int>();
            TileLevelReqs.Add(0, 0);
            TileLevelReqs.Add(1, 1);
            TileLevelReqs.Add(2, 16);
            TileLevelReqs.Add(3, 40);
            TileLevelReqs.Add(4, 88);
            TileLevelReqs.Add(5, 188);
            TileLevelReqs.Add(6, 332);
            TileLevelReqs.Add(7, 528);
            TileLevelReqs.Add(8, 784);
            TileLevelReqs.Add(9, 1108);
            TileLevelReqs.Add(10, 1508);
            TileLevelReqs.Add(11, 2000);

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

            Log = new List<LogEntry>();
            Log.Add(new LogEntry("Generating Rivetting Tales of Tiles..."));

            GameUIs = new Dictionary<UITemplate, UI >();

            GameUIs.Add(UITemplate.toolbar01, GenerateUI(UITemplate.toolbar01));
            GameUIs.Add(UITemplate.turn01, GenerateUI(UITemplate.turn01));
            GameUIs.Add(UITemplate.log, GenerateUI(UITemplate.log));
            RefreshLogEntries(Log);
            GameUIs.Add(UITemplate.income, GenerateUI(UITemplate.income));
            GameUIs.Add(UITemplate.tileExpendNorth, GenerateUI(UITemplate.tileExpendNorth));
            GameUIs.Add(UITemplate.tileExpendEast, GenerateUI(UITemplate.tileExpendEast));
            GameUIs.Add(UITemplate.tileExpendSouth, GenerateUI(UITemplate.tileExpendSouth));
            GameUIs.Add(UITemplate.tileExpendWest, GenerateUI(UITemplate.tileExpendWest));
            GameUIs.Add(UITemplate.tileSheet, GenerateUI(UITemplate.tileSheet));
            GameUIs.Add(UITemplate.tooltip, GenerateUI(UITemplate.tooltip));

            IncrementResources();
            RefreshIncome();

            Player1.SetActiveRoom(CurrentLevel.Stage[Vector2.Zero]);

            TogTileSheet(false);
        }

        public void RefreshLogEntries(List<LogEntry> log)
        {
            List<UIItem> logUIIs = new List<UIItem>();
            logUIIs.Add(new UIItem(UIItemType.TextFix, "Events Journal", Color.CornflowerBlue, ScreenManager.Fonts[Font.menuItem02.ToString()], UIItemsFlow.Vertical, UIAction.ToggleLog));
            foreach (LogEntry tLE in log)
                if (LogToggled)
                    logUIIs.Add(new UIItem(UIItemType.TextFix, tLE.Text, tLE.TextColor, ScreenManager.Fonts[tLE.TextFont.ToString()], UIItemsFlow.Vertical));
                else if (tLE.Expiration() >= ScreenManager.TotalTime)
                    logUIIs.Add(new UIItem(UIItemType.TextFix, tLE.Text, tLE.TextColor, ScreenManager.Fonts[tLE.TextFont.ToString()], UIItemsFlow.Vertical));

            GameUIs[UITemplate.log].SetUIItems(logUIIs);
           
        }

        public void NextTurn()
        {
            Log.Add(new LogEntry("End of turn " + Turn + ". Beginning of turn " + (Turn + 1) + "."));
            Turn += 1;
            IncrementResources();
        }

        public void RefreshIncome()
        {
            Income[ResourceType.Gold] = 0;
            Income[ResourceType.Food] = 0;
            Income[ResourceType.Wood] = 0;
            Income[ResourceType.Production] = 0;
            Income[ResourceType.Energy] = 0;

            foreach (KeyValuePair<Vector2, Tile> room in CurrentLevel.Stage)
                foreach (KeyValuePair<ResourceType, int> res in room.Value.Resources)
                    Income[res.Key] += res.Value;

            RefreshIncomeUI(GameUIs[UITemplate.income]);
        }

        public void IncrementResources()
        {
            foreach (KeyValuePair<Vector2, Tile> room in CurrentLevel.Stage)
                foreach (KeyValuePair<ResourceType, int> res in room.Value.Resources)
                    Resources[res.Key] += res.Value;

            RefreshResourcesUI(GameUIs[UITemplate.toolbar01]);
        }

        public int GetCurrentTileLevel()
        {
            int iLevel = 0;
            int iTileLevel = 1;
            foreach (KeyValuePair<Vector2, Tile> tile in CurrentLevel.Stage)
                iLevel += tile.Value.Lvl;

            foreach (KeyValuePair<int, int> lvlReq in TileLevelReqs)
                if (lvlReq.Value < iLevel)
                    iTileLevel = lvlReq.Key;
            return iTileLevel;

        }

        public void AddRoom(string positionString)
        {
            string[] split = positionString.Split(':');
            Vector2 tV = new Vector2(float.Parse(split[0]), float.Parse(split[1]));
            CurrentLevel.AddRoom(tV, CurrentTileLevel);
            RefreshIncome();
            RefreshResourcesUI(GameUIs[UITemplate.toolbar01]);
            Player1.RefreshExpendUIs();
        }

        public override void Update(GameTime gameTime, InputManager input)
        { 
            base.Update(gameTime, input);
            CheckHoverUI(input.MousePosition() + ScreenManager.PlayerCamera.Position);
            if (ScreenManager.Input.MousePressed())
            {
                if (ActiveUI != null)
                {
                    if (ActiveUI.ToDraw)
                    {
                        foreach (UIItem uiI in ActiveUI.Items)
                        {
                            if (Tools.Intersects(input.MousePosition() + ScreenManager.PlayerCamera.Position, new Rectangle(uiI.ItemRect.X + (int)ActiveUI.Position.X, uiI.ItemRect.Y + (int)ActiveUI.Position.Y, uiI.ItemRect.Width, uiI.ItemRect.Height)))
                                ExecuteMenuAction(uiI.Action, uiI.ActionText);
                        }
                    }
                }
            }
        }

        #region Draw

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            foreach (KeyValuePair<Vector2, Tile> room in CurrentLevel.Stage)
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

        private void DrawRoom(Tile room)
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
                        for (int i = 0; i < res.Value; i++)
                        {
                            ScreenManager.Sprites.Draw(ScreenManager.Textures2D["resource_" + res.Key.ToString().ToLower()], (room.Position + new Vector2(resX + (ScreenManager.Textures2D["resource_" + res.Key.ToString().ToLower()].Width * i), resY)) + (room.Position * ScreenManager.TSize), null, Color.White);
                        }
                        resY += 16;
                        //ScreenManager.Sprites.DrawString(ScreenManager.Fonts[Font.debug02.ToString()], res.Value.ToString(), (room.Position + new Vector2(resX, resY)) + (room.Position * ScreenManager.TSize), Color.White, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
                        //resY += (resHeight * 0.6f) - 1f;
                        
                        

                    }
                    //if (ScreenManager.DebugMode)
                    //    ScreenManager.Sprites.DrawString(ScreenManager.Fonts[Font.debug02.ToString()], (int)tT.Coords.X + ":" + (int)tT.Coords.Y, (tT.Position - new Vector2(15, 15)) + (room.Position * ScreenManager.TSize), Color.DarkRed, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
                }
            if (ScreenManager.DebugMode)
                ScreenManager.Sprites.DrawString(ScreenManager.Fonts[Font.debug02.ToString()], (int)room.Position.X + ":" + (int)room.Position.Y, (room.Position - new Vector2(15, 15)) + (room.Position * ScreenManager.TSize), Color.DarkRed, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
        }

#endregion

        #region User Interface
        public void UpdateUIs(GameTime gameTime)
        {
            RefreshLogEntries(Log);
            foreach (KeyValuePair<UITemplate, UI> ui in GameUIs)
                ui.Value.Update();
        }
        
        private void RefreshResourcesUI(UI tUI)
        {
            List<UIItem> tUIItems = new List<UIItem>();
            tUIItems.Add(new UIItem(UIItemType.TextFix, "Turn " + Turn, Color.CornflowerBlue, ScreenManager.Fonts[Font.menuItem03.ToString()], UIItemsFlow.Horizontal, UIAction.None));
            //tUIItems.Add(new UIItem(UIItemType.TextFix, "Tile lvl " + CurrentTileLevel, Color.CornflowerBlue, ScreenManager.Fonts[Font.menuItem03.ToString()], UIItemsFlow.Vertical, UIAction.None));
            foreach (KeyValuePair<ResourceType, int> res in Resources)
                tUIItems.Add(new UIItem(UIItemType.ImageText, res.Value.ToString(), Color.White, ScreenManager.Fonts[Font.menuItem03.ToString()], UIItemsFlow.Horizontal, UIAction.None, "resource_" + res.Key.ToString().ToLower()));
            tUI.SetUIItems(tUIItems);
        }

        private void RefreshIncomeUI(UI tUI)
        {
            List<UIItem> tUIItems = new List<UIItem>();
            foreach (KeyValuePair<ResourceType, int> res in Income)
                tUIItems.Add(new UIItem(UIItemType.ImageText, res.Value.ToString(), Color.White, ScreenManager.Fonts[Font.menuItem03.ToString()], UIItemsFlow.Vertical, UIAction.None, "resource_" + res.Key.ToString().ToLower()));
            tUI.SetUIItems(tUIItems);
        }
        
        public void CheckHoverUI(Vector2 RealMousePosition)
        {
            ActiveUI = null;
            bool setTTToggleToFalse = true;
            foreach (KeyValuePair<UITemplate, UI> ui in GameUIs)
            {
                if (ui.Value.ToDraw)
                {
                    if (Tools.Intersects(RealMousePosition, new Rectangle((int)ui.Value.Position.X, (int)ui.Value.Position.Y, (int)ui.Value.Size.X, (int)ui.Value.Size.Y)))
                    {
                        ActiveUI = ui.Value;
                        if (ActiveUI.TTText != "" && ActiveUI.TTText != null)
                        {
                            TTToggled = true;
                            setTTToggleToFalse = false;
                            TTText = ActiveUI.TTText;
                            SetTTText(TTText);
                        }
                        ui.Value.IsActive = true;
                    }
                    else
                        ui.Value.IsActive = false;
                }
                else
                    ui.Value.IsActive = false;
            }
            if (setTTToggleToFalse)
            {
                TTToggled = false;
                TTText = "";
            }
        }

        public void SetTTText(string text)
        {
            List<UIItem> tUIIs = new List<UIItem>();

            tUIIs.Add(new UIItem(UIItemType.TextFix, text, Color.White, ScreenManager.Fonts[Font.menuItem03.ToString()], UIItemsFlow.Vertical));

            GameUIs[UITemplate.tooltip].SetUIItems(tUIIs);
        }

        public void TogTileSheet(bool tog)
        {
            for (int i = 1; i < GameUIs[UITemplate.tileSheet].Items.Count; i++)
                GameUIs[UITemplate.tileSheet].Items[i].ToShow = tog;
            GameUIs[UITemplate.tileSheet].RefreshSize();
            GameUIs[UITemplate.tileSheet].UpdateItemsPosition();
        }

        public void ToggleTileSheet()
        {
            if (GameUIs[UITemplate.tileSheet].Items[1].ToShow)
                TogTileSheet(false);
            else
                TogTileSheet(true);

        }

        public void ExecuteMenuAction(UIAction action, string actionText)
        {
            switch (action)
            {
                case UIAction.EndTurn:
                    NextTurn();
                    break;
                case UIAction.BuyTile:
                    AddRoom(actionText);
                    break;
                case UIAction.ToggleLog:
                    if (LogToggled)
                        LogToggled = false;
                    else
                        LogToggled = true;
                    break;
                case UIAction.TileSheet:
                    if (actionText != "")
                    {
                        string[] split = actionText.Split(':');
                        switch(split[1])
                        {
                            case "Gold":
                                Player1.ActiveRoom.AddResources(int.Parse(split[0]), ResourceType.Gold);
                                break;
                            case "Food":
                                Player1.ActiveRoom.AddResources(int.Parse(split[0]), ResourceType.Food);
                                break;
                            case "Wood":
                                Player1.ActiveRoom.AddResources(int.Parse(split[0]), ResourceType.Wood);
                                break;
                            case "Production":
                                Player1.ActiveRoom.AddResources(int.Parse(split[0]), ResourceType.Production);
                                break;
                            case "Energy":
                                Player1.ActiveRoom.AddResources(int.Parse(split[0]), ResourceType.Energy);
                                break;
                            case "":

                                break;
                        }
                    }
                    
                    ToggleTileSheet();
                    break;
                default:

                    break;
            }
        }

        private UI GenerateUI(UITemplate uiName)
        {
            UI tUI;
            switch (uiName)
            {
                case UITemplate.toolbar01:
                    tUI = new UI(UIType.Basic, uiName.ToString(), "Main Tool Bar", new Vector2(300, 300), new Vector2(2, 2));
                    tUI.BackAlpha = 0.35f;
                    tUI.ItemsFlow = UIItemsFlow.Horizontal;
                    RefreshResourcesUI(tUI);
                    tUI.Position = new Vector2(2, 2) + ScreenManager.PlayerCamera.Position;
                    break;
                case UITemplate.income:
                    tUI = new UI(UIType.Basic, uiName.ToString(), "Income Panel", new Vector2(300, 300), new Vector2(2, 2));
                    tUI.BackAlpha = 0.35f;
                    tUI.ItemsFlow = UIItemsFlow.Vertical;
                    RefreshIncomeUI(tUI);
                    tUI.Position = new Vector2(2 + ScreenManager.Resolution.X - 40, 2) + ScreenManager.PlayerCamera.Position;
                    break;
                case UITemplate.turn01:
                    tUI = new UI(UIType.Basic, uiName.ToString(), "Turns UI", new Vector2(300, 300), new Vector2(2, 2));
                    tUI.BackAlpha = 0.35f;
                    List<UIItem> tUIItems;
                    tUIItems = new List<UIItem>();
                    tUIItems.Add(new UIItem(UIItemType.TextFix, "End Turn", Color.CornflowerBlue, ScreenManager.Fonts[Font.menuItem01.ToString()], UIItemsFlow.Vertical, UIAction.EndTurn));
                    tUI.SetUIItems(tUIItems);
                    tUI.Position = new Vector2(ScreenManager.Resolution.X - tUI.Size.X, ScreenManager.Resolution.Y - tUI.Size.Y) + ScreenManager.PlayerCamera.Position;
                    break;
                case UITemplate.tileSheet:
                    tUI = new UI(UIType.Basic, uiName.ToString(), "Tile UI", new Vector2(300, 300), new Vector2(2, 2));
                    tUI.BackAlpha = 0.35f;
                    tUIItems = new List<UIItem>();
                    tUIItems.Add(new UIItem(UIItemType.ImageFix, "", Color.CornflowerBlue, ScreenManager.Fonts[Font.menuItem01.ToString()], UIItemsFlow.Vertical, UIAction.TileSheet, "gear_24"));
                    tUIItems.Add(new UIItem(UIItemType.TextFix, "Mine", Color.CornflowerBlue, ScreenManager.Fonts[Font.menuItem03.ToString()], UIItemsFlow.Vertical, UIAction.TileSheet, "", "1:Gold"));
                    tUIItems.Add(new UIItem(UIItemType.TextFix, "Farm", Color.CornflowerBlue, ScreenManager.Fonts[Font.menuItem03.ToString()], UIItemsFlow.Vertical, UIAction.TileSheet, "", "1:Food"));
                    tUIItems.Add(new UIItem(UIItemType.TextFix, "Lumbermill", Color.CornflowerBlue, ScreenManager.Fonts[Font.menuItem03.ToString()], UIItemsFlow.Vertical, UIAction.TileSheet, "", "1:Wood"));
                    tUIItems.Add(new UIItem(UIItemType.TextFix, "Workshop", Color.CornflowerBlue, ScreenManager.Fonts[Font.menuItem03.ToString()], UIItemsFlow.Vertical, UIAction.TileSheet, "", "1:Production"));
                    tUIItems.Add(new UIItem(UIItemType.TextFix, "Energy Source", Color.CornflowerBlue, ScreenManager.Fonts[Font.menuItem03.ToString()], UIItemsFlow.Vertical, UIAction.TileSheet, "", "1:Energy"));
                    tUI.SetUIItems(tUIItems);
                    tUI.Position = Player1.ActiveRoom.Position + new Vector2(ScreenManager.TileSize.X - 28, 0);
                    break;
                case UITemplate.tileExpendNorth:
                    tUI = new UI(UIType.Basic, uiName.ToString(), "Tile Expend - North UI", new Vector2(300, 300), new Vector2(2, 2));
                    tUI.TTText = "Discover a new tile - North";
                    tUI.BackAlpha = 0.35f;
                    tUIItems = new List<UIItem>();
                    tUIItems.Add(new UIItem(UIItemType.ImageFix, "", Color.CornflowerBlue, ScreenManager.Fonts[Font.menuItem01.ToString()], UIItemsFlow.Vertical, UIAction.BuyTile, "plus_24"));
                    tUI.SetUIItems(tUIItems);
                    tUI.Position = Player1.ActiveRoom.Position + new Vector2((ScreenManager.TileSize.X / 2) - 14, -14);
                    break;
                case UITemplate.tileExpendEast:
                    tUI = new UI(UIType.Basic, uiName.ToString(), "Tile Expend - East UI", new Vector2(300, 300), new Vector2(2, 2));
                    tUI.TTText = "Discover a new tile - East";
                    tUI.BackAlpha = 0.35f;
                    tUIItems = new List<UIItem>();
                    tUIItems.Add(new UIItem(UIItemType.ImageFix, "", Color.CornflowerBlue, ScreenManager.Fonts[Font.menuItem01.ToString()], UIItemsFlow.Vertical, UIAction.BuyTile, "plus_24"));
                    tUI.SetUIItems(tUIItems);
                    tUI.Position = Player1.ActiveRoom.Position + new Vector2((ScreenManager.TileSize.X) - 14, (ScreenManager.TileSize.Y / 2) - 14);
                    break;
                case UITemplate.tileExpendSouth:
                    tUI = new UI(UIType.Basic, uiName.ToString(), "Tile Expend - South UI", new Vector2(300, 300), new Vector2(2, 2));
                    tUI.TTText = "Discover a new tile - South";
                    tUI.BackAlpha = 0.35f;
                    tUIItems = new List<UIItem>();
                    tUIItems.Add(new UIItem(UIItemType.ImageFix, "", Color.CornflowerBlue, ScreenManager.Fonts[Font.menuItem01.ToString()], UIItemsFlow.Vertical, UIAction.BuyTile, "plus_24"));
                    tUI.SetUIItems(tUIItems);
                    tUI.Position = Player1.ActiveRoom.Position + new Vector2((ScreenManager.TileSize.X / 2) - 14, (ScreenManager.TileSize.Y) - 14);
                    break;
                case UITemplate.tileExpendWest:
                    tUI = new UI(UIType.Basic, uiName.ToString(), "Tile Expend - West UI", new Vector2(300, 300), new Vector2(2, 2));
                    tUI.TTText = "Discover a new tile - West";
                    tUI.BackAlpha = 0.35f;
                    tUIItems = new List<UIItem>();
                    tUIItems.Add(new UIItem(UIItemType.ImageFix, "", Color.CornflowerBlue, ScreenManager.Fonts[Font.menuItem01.ToString()], UIItemsFlow.Vertical, UIAction.BuyTile, "plus_24"));
                    tUI.SetUIItems(tUIItems);
                    tUI.Position = Player1.ActiveRoom.Position + new Vector2(-14, (ScreenManager.TileSize.Y / 2) - 14);
                    break;
                case UITemplate.log:
                    tUI = new UI(UIType.BasicInvis, uiName.ToString(), "Events Journal", new Vector2(300, 300), new Vector2(2, ScreenManager.Resolution.Y - 180));
                    tUI.BackAlpha = 0.35f;
                    tUI.ItemsFlow = UIItemsFlow.Vertical;
                    tUIItems = new List<UIItem>();
                    tUIItems.Add(new UIItem(UIItemType.TextFix, "Events Journal", Color.CornflowerBlue, ScreenManager.Fonts[Font.menuItem02.ToString()], UIItemsFlow.Vertical, UIAction.ToggleLog));
                    tUI.SetUIItems(tUIItems);
                    tUI.Position = new Vector2(2, ScreenManager.Resolution.Y) + ScreenManager.PlayerCamera.Position;
                    break;
                case UITemplate.tooltip:
                    tUI = new UI(UIType.BasicInvis, uiName.ToString(), "Tool Tips", new Vector2(300, 300), new Vector2(2, ScreenManager.Resolution.Y));
                    tUI.BackAlpha = 0.40f;
                    tUI.ItemsFlow = UIItemsFlow.Vertical;
                    tUI.Position = ScreenManager.PlayerCamera.Position;
                    tUI.ToDraw = false;
                    break;
                default:
                    tUI = new UI();
                    break;
            }
            return tUI;
        }
        #endregion
    }
}
