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
        public int nbTilesToDraw;

        public GameplayScreen()
        {

        }

        public void Initialize(GameType gameMode = GameType.Survival, string levelToLoad = "")
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
                CurrentLevel.GameMode = gameMode;
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

            ScreenManager.AddOrReplaceUI(UITemplate.toolbar01, ScreenManager.GenerateUI(UITemplate.toolbar01));                        //
            ScreenManager.AddOrReplaceUI(UITemplate.turn01, ScreenManager.GenerateUI(UITemplate.turn01));                              //Displays the "End of turn" options.
            ScreenManager.AddOrReplaceUI(UITemplate.income, ScreenManager.GenerateUI(UITemplate.income));                              
            ScreenManager.AddOrReplaceUI(UITemplate.tileExpendNorth, ScreenManager.GenerateUI(UITemplate.tileExpendNorth));
            ScreenManager.AddOrReplaceUI(UITemplate.tileExpendEast, ScreenManager.GenerateUI(UITemplate.tileExpendEast));
            ScreenManager.AddOrReplaceUI(UITemplate.tileExpendSouth, ScreenManager.GenerateUI(UITemplate.tileExpendSouth));
            ScreenManager.AddOrReplaceUI(UITemplate.tileExpendWest, ScreenManager.GenerateUI(UITemplate.tileExpendWest));              
            ScreenManager.AddOrReplaceUI(UITemplate.tileSheet, ScreenManager.GenerateUI(UITemplate.tileSheet));                        //Available actions for the current tile.
            ScreenManager.AddOrReplaceUI(UITemplate.log, ScreenManager.GenerateUI(UITemplate.log));                                    //Event Journal.
            ScreenManager.RefreshLogEntries(ScreenManager.Log);
            ScreenManager.AddOrReplaceUI(UITemplate.tooltip, ScreenManager.GenerateUI(UITemplate.tooltip));
            ScreenManager.AddOrReplaceUI(UITemplate.improveUI, ScreenManager.GenerateUI(UITemplate.improveUI));                        //Improvements available to build on the current tile.
            ScreenManager.AddOrReplaceUI(UITemplate.buildUI, ScreenManager.GenerateUI(UITemplate.buildUI));                            //Buildings available to build on the current tile.
            ScreenManager.AddOrReplaceUI(UITemplate.selectionUI, ScreenManager.GenerateUI(UITemplate.selectionUI));                    //Shows actions for the currently selected thing.

            IncrementResources();
            RefreshIncome();

            Player1.SetActiveRoom(CurrentLevel.Stage[Vector2.Zero]);

            ScreenManager.TogTileSheet(false);
            ScreenManager.TogImproveUI(false);
            ScreenManager.TogBuildUI(false);
            ScreenManager.TogSelectionUI(false);
        }

        private void InitTileLevelCosts()
        {
            TileLevelCosts = new Dictionary<int, Dictionary<ResourceType, int>>();
            
            for(int i = 1; i < 12; i++)
            {
                Dictionary<ResourceType, int> tTLCs = new Dictionary<ResourceType, int>();
                tTLCs.Add(ResourceType.Gold, i * 1);
                tTLCs.Add(ResourceType.Energy, i * 1);
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
            MoveEnemies();
            SpawnEnemies();
            IncrementResources();            
        }

        public void MoveEnemies()
        {
            foreach(Enemy tE in CurrentLevel.Enemies)
                if (tE.Position.X == 1 ||
                    tE.Position.X == -1 ||
                    tE.Position.Y == 1 ||
                    tE.Position.Y == -1)
                {
                    CurrentLevel.Stage[Vector2.Zero].Durability--;
                }
                else
                {
                    if (tE.TileProgress < 1)
                        tE.TileProgress += tE.GetStat("movespeed");
                    if (tE.TileProgress >= 1)
                    {
                        if (tE.Position.X != 0)
                            if (tE.Position.X > 0)
                                tE.Position = new Vector2(tE.Position.X - 1, tE.Position.Y);
                            else
                                tE.Position = new Vector2(tE.Position.X + 1, tE.Position.Y);

                        if (tE.Position.Y != 0)
                            if (tE.Position.Y > 0)
                                tE.Position = new Vector2(tE.Position.X, tE.Position.Y - 1);
                            else
                                tE.Position = new Vector2(tE.Position.X, tE.Position.Y + 1);

                        tE.TileProgress--;
                    }
                }
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
                    if (res.Key != ResourceType.Empty)
                        Income[res.Key] += res.Value;

            RefreshIncomeUI(ScreenManager.GameUIs[UITemplate.income]);
        }

        public void IncrementResources()
        {
            foreach (KeyValuePair<Vector2, Tile> room in CurrentLevel.Stage)
                foreach (KeyValuePair<ResourceType, int> res in room.Value.Resources)
                    if (res.Key != ResourceType.Empty)
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
            nbTilesToDraw = 0;
            foreach (KeyValuePair<Vector2, Tile> room in CurrentLevel.Stage)
                if (room.Key.X > Player1.ActiveRoom.Position.X + 10 ||
                    room.Key.X < Player1.ActiveRoom.Position.X - 10 ||
                    room.Key.Y > Player1.ActiveRoom.Position.Y + 10 ||
                    room.Key.Y < Player1.ActiveRoom.Position.Y - 10)
                {

                }
                else
                {
                    DrawRoom(room.Value);
                    nbTilesToDraw++;
                }

            ScreenManager.Sprites.Draw(ScreenManager.Textures2D[Player1.ImageName], Player1.Position + (ScreenManager.TileSize / 2), null, Color.White, 0f, new Vector2(Player1.Rect.Width / 2, Player1.Rect.Height / 2), 1f, SpriteEffects.None, 0f);
            if (ScreenManager.DebugMode)
            {
                ScreenManager.Sprites.DrawString(ScreenManager.Fonts[Font.debug01.ToString()], (int)Player1.Position.X + ":" + (int)Player1.Position.Y, Player1.Position - new Vector2(24, 24), Color.Red, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
                ScreenManager.Sprites.DrawString(ScreenManager.Fonts[Font.debug01.ToString()], (ScreenManager.Input.MousePosition() + ScreenManager.PlayerCamera.Position).ToString(), ScreenManager.Input.MousePosition() + ScreenManager.PlayerCamera.Position, Color.Red, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
                ScreenManager.Sprites.DrawString(ScreenManager.Fonts[Font.debug02.ToString()], "NRTD:" + nbTilesToDraw, Player1.Position - new Vector2(24, 38), Color.Blue, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
            }

            foreach(Enemy tE in CurrentLevel.Enemies)
            {
                if (tE.Position.X > Player1.ActiveRoom.Position.X + 10 ||
                    tE.Position.X < Player1.ActiveRoom.Position.X - 10 ||
                    tE.Position.Y > Player1.ActiveRoom.Position.Y + 10 ||
                    tE.Position.Y < Player1.ActiveRoom.Position.Y - 10)
                {

                }
                else
                {
                    DrawEnemy(tE);
                }
            }
        }

        private void DrawEnemy(Enemy enemy)
        {
            ScreenManager.Sprites.Draw(ScreenManager.Textures2D[enemy.ImageName], enemy.DrawPosition(), null, Color.White);
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
                    //ScreenManager.Sprites.Draw(ScreenManager.Textures2D[tT.ImageName], tT.Position + (room.Position * ScreenManager.TSize), null, room.IsSpawn ? Color.Red : tCol);
                    ScreenManager.Sprites.Draw(ScreenManager.Textures2D[tT.ImageName], tT.Position + (room.Position * ScreenManager.TSize), null, tCol);

                    if (room.Resources.ContainsKey(ResourceType.Empty))
                    {
                        for (int i = 0; i < room.Resources[ResourceType.Empty]; i++)
                        {
                            ScreenManager.Sprites.Draw(ScreenManager.Textures2D["resource_" + ResourceType.Empty.ToString().ToLower()], (room.Position + new Vector2(resX + (ScreenManager.Textures2D["resource_" + ResourceType.Empty.ToString().ToLower()].Width * i), resY)) + (room.Position * ScreenManager.TSize), null, Color.White * 0.4f);
                        }
                        resY += 16;
                    }

                    foreach (KeyValuePair<ResourceType, int> res in room.Resources)
                    {
                        if (res.Key != ResourceType.Empty)
                        {
                            for (int i = 0; i < res.Value; i++)
                            {
                                ScreenManager.Sprites.Draw(ScreenManager.Textures2D["resource_" + res.Key.ToString().ToLower()], (room.Position + new Vector2(resX + (ScreenManager.Textures2D["resource_" + res.Key.ToString().ToLower()].Width * i), resY)) + (room.Position * ScreenManager.TSize), null, Color.White);
                            }
                            resY += 16;
                        }
                    }
                }
            
            if (ScreenManager.DebugMode)
            {
                ScreenManager.Sprites.DrawString(ScreenManager.Fonts[Font.debug02.ToString()], (int)room.Position.X + ":" + (int)room.Position.Y, (room.Position - new Vector2(15, 15)) + (room.Position * ScreenManager.TSize), Color.DarkRed, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
            }

            if (room.TileBuilding != null)
            {
                string imgName;
                switch(room.TileBuilding.TType)
                {
                    case BuildingType.Tower_Normal:
                        imgName = "tower_" + room.TileBuilding.Level;
                        break;
                    case BuildingType.Spawn_Enemy_Basic:
                        imgName = "spawn_0";
                        break;
                    default:
                        imgName = "tower_0";
                        break;
                }
                ScreenManager.Sprites.Draw(ScreenManager.Textures2D[imgName], (room.BuildingPosition), null, Color.White);
            }
                

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
            tUIItems.Add(new UIItem(UIItemType.TextFix, CurrentLevel.GameMode.ToString() + " : Turn " + CurrentLevel.Turn + "  ", Color.CornflowerBlue, ScreenManager.Fonts[Font.menuItem03.ToString()], UIItemsFlow.Horizontal, UIAction.None));
            //tUIItems.Add(new UIItem(UIItemType.TextFix, "Tile lvl " + CurrentTileLevel, Color.CornflowerBlue, ScreenManager.Fonts[Font.menuItem03.ToString()], UIItemsFlow.Vertical, UIAction.None));
            foreach (KeyValuePair<ResourceType, int> res in CurrentLevel.Resources)
                tUIItems.Add(new UIItem(UIItemType.ImageText, res.Value.ToString() + "  ", Color.White, ScreenManager.Fonts[Font.menuItem03.ToString()], UIItemsFlow.Horizontal, UIAction.None, "resource_" + res.Key.ToString().ToLower()));
            tUIItems.Add(new UIItem(UIItemType.ImageText, CurrentLevel.Stage[Vector2.Zero].Durability.ToString(), Color.White, ScreenManager.Fonts[Font.menuItem03.ToString()], UIItemsFlow.Horizontal, UIAction.None, "durability_16"));

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
