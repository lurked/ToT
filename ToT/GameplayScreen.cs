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
        public int CurrentTileLevel = 1;
        public Dictionary<ResourceType, int> Income;
        public Dictionary<int, int> TileLevelReqs;
        public Dictionary<int, Dictionary<ResourceType, int>> TileLevelCosts;

        public GameplayScreen()
        {

        }

        public void Initialize(string levelToLoad = "")
        {
            InitTileLevelReqs();

            InitTileLevelCosts();

            Income = new Dictionary<ResourceType, int>();
            Income.Add(ResourceType.Gold, 0);
            Income.Add(ResourceType.Food, 0);
            Income.Add(ResourceType.Energy, 0);
            Income.Add(ResourceType.Production, 0);

            CurrentLevel = new Level();
            if (levelToLoad == "")
            {
                CurrentLevel.GenerateLevel(LevelType.Static, "base01", "");

                CurrentLevel.Resources.Add(ResourceType.Gold, 0);
                CurrentLevel.Resources.Add(ResourceType.Food, 0);
                CurrentLevel.Resources.Add(ResourceType.Energy, 0);
                CurrentLevel.Resources.Add(ResourceType.Production, 0);
            }
            else
            {
                CurrentLevel = FileManager.LoadLevel(ScreenManager.SAVESPATH + levelToLoad + FileManager.GAMEFILES_EXT_LEVEL);
                CurrentLevel.Stage[Vector2.Zero].IsActive = true;
            }



            Player1 = new Player("Noob", ThingType.Player, ScreenManager.TileSize / 2, "player01", "A noob player");
            Player1.Initialize();
            Player1.SetStage(CurrentLevel.Stage);
            ScreenManager.Log.Add(new LogEntry("Generating Rivetting Tales of Tiles..."));

            ScreenManager.GameUIs.Add(UITemplate.toolbar01, ScreenManager.GenerateUI(UITemplate.toolbar01));
            ScreenManager.GameUIs.Add(UITemplate.turn01, ScreenManager.GenerateUI(UITemplate.turn01));
            ScreenManager.GameUIs.Add(UITemplate.income, ScreenManager.GenerateUI(UITemplate.income));
            ScreenManager.GameUIs.Add(UITemplate.tileExpendNorth, ScreenManager.GenerateUI(UITemplate.tileExpendNorth));
            ScreenManager.GameUIs.Add(UITemplate.tileExpendEast, ScreenManager.GenerateUI(UITemplate.tileExpendEast));
            ScreenManager.GameUIs.Add(UITemplate.tileExpendSouth, ScreenManager.GenerateUI(UITemplate.tileExpendSouth));
            ScreenManager.GameUIs.Add(UITemplate.tileExpendWest, ScreenManager.GenerateUI(UITemplate.tileExpendWest));
            ScreenManager.GameUIs.Add(UITemplate.tileSheet, ScreenManager.GenerateUI(UITemplate.tileSheet));
            ScreenManager.GameUIs.Add(UITemplate.log, ScreenManager.GenerateUI(UITemplate.log));
            ScreenManager.RefreshLogEntries(ScreenManager.Log);
            ScreenManager.GameUIs.Add(UITemplate.tooltip, ScreenManager.GenerateUI(UITemplate.tooltip));
            ScreenManager.GameUIs.Add(UITemplate.improveUI, ScreenManager.GenerateUI(UITemplate.improveUI));
            ScreenManager.GameUIs.Add(UITemplate.buildUI, ScreenManager.GenerateUI(UITemplate.buildUI));

            IncrementResources();
            RefreshIncome();

            Player1.SetActiveRoom(CurrentLevel.Stage[Vector2.Zero]);

            ScreenManager.TogTileSheet(false);
            ScreenManager.TogImproveUI(false);
            ScreenManager.TogBuildUI(false);
        }

        private void InitTileLevelCosts()
        {
            TileLevelCosts = new Dictionary<int, Dictionary<ResourceType, int>>();
            
            for(int i = 1; i < 12; i++)
            {
                Dictionary<ResourceType, int> tTLCs = new Dictionary<ResourceType, int>();
                tTLCs.Add(ResourceType.Gold, i * 10);
                tTLCs.Add(ResourceType.Energy, i * 10);
                TileLevelCosts.Add(i, tTLCs);
            }
        }

        public bool CheckIfPlayerHasResources(Dictionary<ResourceType, int> requiredResources)
        {
            bool tB = true;

            foreach (KeyValuePair<ResourceType, int> res in requiredResources)
                if (CurrentLevel.Resources[res.Key] < res.Value)
                {
                    tB = false;
                    break;
                }

            return tB;
        }

        public string GetTileLevelCostString()
        {
            string tS = "";

            foreach (KeyValuePair<ResourceType, int> res in TileLevelCosts[CurrentTileLevel])
            {
                if (tS != "")
                    tS += " + ";
                tS += res.Value + res.Key.ToString();
            }
                
            return tS;
        }

        public void SpendResources(Dictionary<ResourceType, int> tReqs)
        {
            foreach (KeyValuePair<ResourceType, int> res in tReqs)
                CurrentLevel.Resources[res.Key] -= res.Value;
            RefreshResourcesUI(ScreenManager.GameUIs[UITemplate.toolbar01]);
        }

        private void InitTileLevelReqs()
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
        }

        public void NextTurn()
        {
            CurrentLevel.Save();
            ScreenManager.Log.Add(new LogEntry("End of turn " + CurrentLevel.Turn + ". Beginning of turn " + (CurrentLevel.Turn + 1) + "."));

            CurrentLevel.Turn += 1;
            SpawnEnemies();
            IncrementResources();            
        }

        public void SpawnEnemies()
        {
            foreach(KeyValuePair<Vector2, Tile> tile in CurrentLevel.Stage)
            {
                if (tile.Value.IsSpawn)
                {
                    List<Enemy> tEs = GetEnemiesToSpawn(tile.Value.Lvl, tile.Value.Position);
                    CurrentLevel.Enemies.AddRange(tEs);
                }
            }
        }

        public List<Enemy> GenRandomEnemies(int lvl, Vector2 position, int min, int max)
        {
            List<Enemy> tEs = new List<Enemy>();
            int iRand = StaticRandom.Instance.Next(min, max + 1);
            for (int i = 0; i < iRand; i++)
                tEs.Add(new Enemy("base_" + lvl, ThingType.Creature, position));
            return tEs;
        }

        public List<Enemy> GetEnemiesToSpawn(int lvl, Vector2 position)
        {
            List<Enemy> tEs = new List<Enemy>();
            
            tEs = GenRandomEnemies(lvl, position, 1, 2);
            
            return tEs;
        }


        public void RefreshIncome()
        {
            Income[ResourceType.Gold] = 0;
            Income[ResourceType.Food] = 0;
            Income[ResourceType.Production] = 0;
            Income[ResourceType.Energy] = 0;

            foreach (KeyValuePair<Vector2, Tile> room in CurrentLevel.Stage)
                foreach (KeyValuePair<ResourceType, int> res in room.Value.Resources)
                    Income[res.Key] += res.Value;

            RefreshIncomeUI(ScreenManager.GameUIs[UITemplate.income]);
        }

        public void IncrementResources()
        {
            foreach (KeyValuePair<Vector2, Tile> room in CurrentLevel.Stage)
                foreach (KeyValuePair<ResourceType, int> res in room.Value.Resources)
                    CurrentLevel.Resources[res.Key] += res.Value;

            RefreshResourcesUI(ScreenManager.GameUIs[UITemplate.toolbar01]);
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
            RefreshResourcesUI(ScreenManager.GameUIs[UITemplate.toolbar01]);
            Player1.RefreshExpendUIs();
            CurrentLevel.Save();
        }

        public override void Update(GameTime gameTime, InputManager input)
        { 
            base.Update(gameTime, input);
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
                    ScreenManager.Sprites.Draw(ScreenManager.Textures2D[tT.ImageName], tT.Position + (room.Position * ScreenManager.TSize), null, room.IsSpawn ? Color.Red : tCol);
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
            
            if (room.TileBuilding != null)
                ScreenManager.Sprites.Draw(ScreenManager.Textures2D["tower_" + room.TileBuilding.Level], (room.BuildingPosition), null, Color.White);
        }

#endregion

        #region User Interface
        public void UpdateUIs(GameTime gameTime)
        {
            ScreenManager.RefreshLogEntries(ScreenManager.Log);
            foreach (KeyValuePair<UITemplate, UI> ui in ScreenManager.GameUIs)
                ui.Value.Update();
        }
        
        public void RefreshResourcesUI(UI tUI)
        {
            List<UIItem> tUIItems = new List<UIItem>();
            tUIItems.Add(new UIItem(UIItemType.TextFix, "Turn " + CurrentLevel.Turn, Color.CornflowerBlue, ScreenManager.Fonts[Font.menuItem03.ToString()], UIItemsFlow.Horizontal, UIAction.None));
            //tUIItems.Add(new UIItem(UIItemType.TextFix, "Tile lvl " + CurrentTileLevel, Color.CornflowerBlue, ScreenManager.Fonts[Font.menuItem03.ToString()], UIItemsFlow.Vertical, UIAction.None));
            foreach (KeyValuePair<ResourceType, int> res in CurrentLevel.Resources)
                tUIItems.Add(new UIItem(UIItemType.ImageText, res.Value.ToString(), Color.White, ScreenManager.Fonts[Font.menuItem03.ToString()], UIItemsFlow.Horizontal, UIAction.None, "resource_" + res.Key.ToString().ToLower()));
            tUI.SetUIItems(tUIItems);
        }

        public void RefreshIncomeUI(UI tUI)
        {
            List<UIItem> tUIItems = new List<UIItem>();
            foreach (KeyValuePair<ResourceType, int> res in Income)
                tUIItems.Add(new UIItem(UIItemType.ImageText, res.Value.ToString(), Color.White, ScreenManager.Fonts[Font.menuItem03.ToString()], UIItemsFlow.Vertical, UIAction.None, "resource_" + res.Key.ToString().ToLower()));
            tUI.SetUIItems(tUIItems);
        }

        #endregion
    }
}
