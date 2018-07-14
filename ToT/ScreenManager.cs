using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace ToT
{
    public class ScreenManager : Game
    {
        public static GraphicsDeviceManager GraphicsDeviceMgr;
        public static SpriteBatch Sprites;
        public static Dictionary<string, Texture2D> Textures2D;
        public static Dictionary<string, Texture3D> Textures3D;
        public static Dictionary<string, SpriteFont> Fonts;
        public static Dictionary<string, Model> Models;
        public static List<GameScreen> ScreenList;
        public static ContentManager ContentMgr;
        public static InputManager Input;
        public static ClientState State;
        public static SplashScreen SSplashScreen;
        public static GameOverScreen GOScreen;
        public static MainMenuScreen MMenuScreen;
        public static GameplayScreen GGPScreen;
        public static bool DebugMode = false;
        public static bool ShowFPS = false;
        public static int TSize = 144;
        public static Vector2 TileSize = new Vector2(TSize, TSize);
        public static Vector2 Resolution = new Vector2(1440, 900);
        public static Camera PlayerCamera;
        private FrameCounter _frameCounter = new FrameCounter();
        public const string CONTENTPATH = "C:/Prog/ToT/ToT/Content/";
        public const string IMAGESPATH = CONTENTPATH + "Images/";
        public const string SAVESPATH = CONTENTPATH + "Saves/";
        public static float Delta;
        public static float TotalTime;
        public static Dictionary<UITemplate, UI> GameUIs;
        public static List<LogEntry> Log;
        public static bool LogToggled = false;
        public static bool TTToggled = false;
        public static string TTText = "";
        public static UI ActiveUI;
        public static bool LoadToggled = false;

        public ScreenManager()
        {
            GraphicsDeviceMgr = new GraphicsDeviceManager(this);
            GraphicsDeviceMgr.PreferredBackBufferWidth = (int)Resolution.X;
            GraphicsDeviceMgr.PreferredBackBufferHeight = (int)Resolution.Y;
            GraphicsDeviceMgr.IsFullScreen = false;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Textures2D = new Dictionary<string, Texture2D>();
            Textures3D = new Dictionary<string, Texture3D>();
            Models = new Dictionary<string, Model>();
            Fonts = new Dictionary<string, SpriteFont>();
            base.Initialize();
            Input = new InputManager();
            Camera tCam = new Camera(GraphicsDeviceMgr.PreferredBackBufferWidth, GraphicsDeviceMgr.PreferredBackBufferHeight);
            this.IsMouseVisible = true;
            PlayerCamera = tCam;
            PlayerCamera.SetFocalPoint(new Vector2(GraphicsDeviceMgr.PreferredBackBufferWidth / 2, GraphicsDeviceMgr.PreferredBackBufferHeight / 2));


            SpriteFont logoFont = ContentMgr.Load<SpriteFont>("Fonts/AlinCartoon1");
            Fonts.Add(Font.logo01.ToString(), logoFont);
            SpriteFont debug01Font = ContentMgr.Load<SpriteFont>("Fonts/Earth2073");
            Fonts.Add(Font.debug01.ToString(), debug01Font);
            SpriteFont debug02Font = ContentMgr.Load<SpriteFont>("Fonts/Earth2073");
            Fonts.Add(Font.debug02.ToString(), debug02Font);
            SpriteFont menuItem01 = ContentMgr.Load<SpriteFont>("Fonts/TECHNOLIN");
            Fonts.Add(Font.menuItem01.ToString(), menuItem01);
            SpriteFont menuItem02 = ContentMgr.Load<SpriteFont>("Fonts/nasalization-rg");
            Fonts.Add(Font.menuItem02.ToString(), menuItem02);
            SpriteFont menuItem03 = ContentMgr.Load<SpriteFont>("Fonts/nasalization-rg-small");
            Fonts.Add(Font.menuItem03.ToString(), menuItem03);

            InitializeTextures();

            GraphicsDeviceMgr.SynchronizeWithVerticalRetrace = false;

            IsFixedTimeStep = false;
            GraphicsDeviceMgr.ApplyChanges();

            
            Log = new List<LogEntry>();
            GameUIs = new Dictionary<UITemplate, UI>();

        }

        private void InitializeTextures()
        {
            

            string[] files = Directory.GetFiles(@IMAGESPATH, "*.png");
            foreach (string tS in files) 
            {
                FileStream filestream = new FileStream(tS, FileMode.Open);
                Texture2D tTexture = Texture2D.FromStream(GraphicsDevice, filestream);
                
                Textures2D.Add(Path.GetFileName(tS).Replace(".png", ""), tTexture);
                filestream.Close();
            }
        }

        protected override void LoadContent()
        {
            ContentMgr = Content;
            Sprites = new SpriteBatch(GraphicsDevice);

            // Load any full game assets here

            State = ClientState.Splashscreen;
            SSplashScreen = new SplashScreen();
            AddScreen(SSplashScreen);

        }

        protected override void UnloadContent()
        {
            foreach (var screen in ScreenList)
            {
                screen.UnloadAssets();
            }
            Textures2D.Clear();
            Textures3D.Clear();
            Fonts.Clear();
            Models.Clear();
            ScreenList.Clear();
            Content.Unload();
            Input = null;
        }




        
        public static void RefreshLogEntries(List<LogEntry> log)
        {
            List<UIItem> logUIIs = new List<UIItem>();
            logUIIs.Add(new UIItem(UIItemType.TextFix, "Events Journal", Color.CornflowerBlue, Fonts[Font.menuItem02.ToString()], UIItemsFlow.Vertical, UIAction.ToggleLog));
            foreach (LogEntry tLE in log)
                if (LogToggled)
                    logUIIs.Add(new UIItem(UIItemType.TextFix, tLE.Text, tLE.TextColor, Fonts[tLE.TextFont.ToString()], UIItemsFlow.Vertical));
                else if (tLE.Expiration() >= TotalTime)
                    logUIIs.Add(new UIItem(UIItemType.TextFix, tLE.Text, tLE.TextColor, Fonts[tLE.TextFont.ToString()], UIItemsFlow.Vertical));

            GameUIs[UITemplate.log].SetUIItems(logUIIs);

        }

        public static void AddOrReplaceUI(UITemplate tUIT, UI tUI)
        {
            if (GameUIs.ContainsKey(tUIT))
                GameUIs[tUIT] = tUI;
            else
                GameUIs.Add(tUIT, tUI);
        }

        protected override void Update(GameTime gameTime)
        {
            try
            {
                TotalTime = (float)gameTime.TotalGameTime.TotalSeconds;
                Delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
                Input.Update();

                CheckHoverUI(Input.MousePosition() + PlayerCamera.Position);
                if (Input.MousePressed())
                {
                    if (State == ClientState.GameOver)
                    {
                        State = ClientState.MainMenu;
                        MMenuScreen = new MainMenuScreen();

                        ChangeScreens(GOScreen, MMenuScreen);
                    }
                    else
                    {
                        if (ActiveUI != null)
                        {
                            if (ActiveUI.ToDraw)
                            {
                                foreach (UIItem uiI in ActiveUI.Items)
                                {
                                    if (Tools.Intersects(Input.MousePosition() + PlayerCamera.Position, new Rectangle(uiI.ItemRect.X + (int)ActiveUI.Position.X, uiI.ItemRect.Y + (int)ActiveUI.Position.Y, uiI.ItemRect.Width, uiI.ItemRect.Height)))
                                        ExecuteMenuAction(uiI.Action, uiI.ActionText);
                                }
                            }
                        }
                    }
                }

                if (Input.KeyPressed(Keys.F12))
                {
                    if (DebugMode)
                        DebugMode = false;
                    else
                        DebugMode = true;
                }

                var startIndex = ScreenList.Count - 1;
                while (ScreenList[startIndex].IsPopup && ScreenList[startIndex].IsActive)
                {
                    startIndex--;
                }
                for (var i = startIndex; i < ScreenList.Count; i++)
                    ScreenList[i].Update(gameTime, Input);


                switch (State)
                {
                    case ClientState.Splashscreen:
                        if (Input.KeyDown(Keys.Space, Keys.Enter) || Input.ButtonDown(Buttons.A, Buttons.Start) || Input.MousePressed())
                        {
                            State = ClientState.MainMenu;
                            MMenuScreen = new MainMenuScreen();

                            ChangeScreens(SSplashScreen, MMenuScreen);
                        }
                        else if (Input.KeyPressed(Keys.Escape) || Input.ButtonPressed(Buttons.Back))
                        {
                            Exit();
                        }
                        break;
                    case ClientState.MainMenu:
                        if (Input.KeyPressed(Keys.Escape) || Input.ButtonPressed(Buttons.Back))
                        {
                            //State = ClientState.Splashscreen;
                            //ChangeScreens(MMenuScreen, SSplashScreen);
                            Exit();
                        }
                        break;
                    case ClientState.Game:
                        GGPScreen.Player1.Update(gameTime);
                        GGPScreen.UpdateUIs(gameTime);
                        break;
                    case ClientState.GameOver:
                        GOScreen.Update(gameTime, Input);
                        break;
                    default:
                        if (Input.KeyPressed(Keys.Escape) || Input.ButtonPressed(Buttons.Back))
                        {
                            //State = ClientState.Splashscreen;
                            //ChangeScreens(MMenuScreen, SSplashScreen);
                            Exit();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                // ErrorLog.AddError(ex);
                throw ex;
            }
            finally
            {
                PlayerCamera.Update();
                base.Update(gameTime);
            }
        }

        public static void CheckHoverUI(Vector2 RealMousePosition)
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

        public static void AddFont(string fontName)
        {
            if (Fonts == null)
            {
                Fonts = new Dictionary<string, SpriteFont>();
            }
            if (!Fonts.ContainsKey(fontName))
            {
                Fonts.Add(fontName, ContentMgr.Load<SpriteFont>(fontName));
            }
        }

        public static void RemoveFont(string fontName)
        {
            if (Fonts.ContainsKey(fontName))
            {
                Fonts.Remove(fontName);
            }
        }

        public static void AddTexture2D(string textureName)
        {
            if (Textures2D == null)
            {
                Textures2D = new Dictionary<string, Texture2D>();
            }
            if (!Textures2D.ContainsKey(textureName))
            {
                Textures2D.Add(textureName, ContentMgr.Load<Texture2D>(textureName));
            }
        }

        public static void RemoveTexture2D(string textureName)
        {
            if (Textures2D.ContainsKey(textureName))
            {
                Textures2D.Remove(textureName);
            }
        }

        public static void AddTexture3D(string textureName)
        {
            if (Textures3D == null)
            {
                Textures3D = new Dictionary<string, Texture3D>();
            }
            if (!Textures3D.ContainsKey(textureName))
            {
                Textures3D.Add(textureName, ContentMgr.Load<Texture3D>(textureName));
            }
        }

        public static void RemoveTexture3D(string textureName)
        {
            if (Textures3D.ContainsKey(textureName))
            {
                Textures3D.Remove(textureName);
            }
        }

        public static void AddModel(string modelName)
        {
            if (Models == null)
            {
                Models = new Dictionary<string, Model>();
            }
            if (!Models.ContainsKey(modelName))
            {
                Models.Add(modelName, ContentMgr.Load<Model>(modelName));
            }
        }

        public static void RemoveModel(string modelName)
        {
            if (Models.ContainsKey(modelName))
            {
                Models.Remove(modelName);
            }
        }
        
        public static void AddScreen(GameScreen gameScreen)
        {
            if (ScreenList == null)
            {
                ScreenList = new List<GameScreen>();
            }
            ScreenList.Add(gameScreen);
            gameScreen.LoadAssets();
        }

        public static void RemoveScreen(GameScreen gameScreen)
        {
            gameScreen.UnloadAssets();
            ScreenList.Remove(gameScreen);
            if (ScreenList.Count < 1)
                AddScreen(SSplashScreen);
        }

        public static void ChangeScreens(GameScreen currentScreen, GameScreen targetScreen)
        {
            RemoveScreen(currentScreen);
            AddScreen(targetScreen);
        }



        public static void SetTTText(string text)
        {
            List<UIItem> tUIIs = new List<UIItem>();

            tUIIs.Add(new UIItem(UIItemType.TextFix, text, Color.White, ScreenManager.Fonts[Font.menuItem03.ToString()], UIItemsFlow.Vertical));

            GameUIs[UITemplate.tooltip].SetUIItems(tUIIs);
        }

        public static void HideTileUIs()
        {
            GameUIs[UITemplate.buildUI].ToDraw = false;
            GameUIs[UITemplate.improveUI].ToDraw = false;
        }

        public static void TogTileSheet(bool tog)
        {
            HideTileUIs();

            for (int i = 1; i < GameUIs[UITemplate.tileSheet].Items.Count; i++)
                GameUIs[UITemplate.tileSheet].Items[i].ToShow = tog;
            GameUIs[UITemplate.tileSheet].RefreshSize();
            GameUIs[UITemplate.tileSheet].UpdateItemsPosition();
        }

        public static void ToggleTileSheet()
        {
            if (GameUIs[UITemplate.tileSheet].Items[1].ToShow)
                TogTileSheet(false);
            else
                TogTileSheet(true);

        }

        public static void TogImproveUI(bool tog)
        {
            HideTileUIs();

            GameUIs[UITemplate.improveUI].ToDraw = tog;
            if (tog)
                for (int i = 1; i < GameUIs[UITemplate.improveUI].Items.Count; i++)
                    GameUIs[UITemplate.improveUI].Items[i].ToShow = tog;
            GameUIs[UITemplate.improveUI].RefreshSize();
            GameUIs[UITemplate.improveUI].UpdateItemsPosition();
        }

        public static void ToggleImproveUI()
        {
            if (GameUIs[UITemplate.improveUI].Items[1].ToShow)
                TogImproveUI(false);
            else
                TogImproveUI(true);

        }

        public static void TogBuildUI(bool tog)
        {
            HideTileUIs();

            GameUIs[UITemplate.buildUI].ToDraw = tog;
            if (tog)
                for (int i = 1; i < GameUIs[UITemplate.buildUI].Items.Count; i++)
                    GameUIs[UITemplate.buildUI].Items[i].ToShow = tog;
            GameUIs[UITemplate.buildUI].RefreshSize();
            GameUIs[UITemplate.buildUI].UpdateItemsPosition();
        }

        public static void ToggleBuildUI()
        {
            if (GameUIs[UITemplate.buildUI].Items[1].ToShow)
                TogBuildUI(false);
            else
                TogBuildUI(true);

        }

        public static void TogSelectionUI(bool tog)
        {
            HideTileUIs();
            TogTileSheet(false);

            GameUIs[UITemplate.buildUI].ToDraw = tog;
            if (tog)
                for (int i = 1; i < GameUIs[UITemplate.selectionUI].Items.Count; i++)
                    GameUIs[UITemplate.selectionUI].Items[i].ToShow = tog;
            GameUIs[UITemplate.selectionUI].RefreshSize();
            GameUIs[UITemplate.selectionUI].UpdateItemsPosition();
        }

        public static void ToggleSelectionUI()
        {
            if (GameUIs[UITemplate.selectionUI].Items[1].ToShow)
                TogSelectionUI(false);
            else
                TogSelectionUI(true);

        }

        public static void ToggleMainMenu(bool visibleOrNot)
        {
            GameUIs[UITemplate.mainNew].ToDraw = visibleOrNot;
            GameUIs[UITemplate.mainNewMenu].ToDraw = false;
            GameUIs[UITemplate.mainLoad].ToDraw = visibleOrNot;
            GameUIs[UITemplate.mainLoadSaves].ToDraw = visibleOrNot;
            GameUIs[UITemplate.mainOptions].ToDraw = visibleOrNot;
            GameUIs[UITemplate.mainExit].ToDraw = visibleOrNot;
            LoadToggled = visibleOrNot;
        }

        public void LoadSavedGames()
        {

            if (LoadToggled)
            {
                LoadToggled = false;
                GameUIs[UITemplate.mainLoadSaves].ToDraw = false;
            }
            else
            {
                List<UIItem> tUIItems = new List<UIItem>();
                List<string> tSaves = FileManager.GetSaves(SAVESPATH);
                tSaves.Reverse();
                foreach (string save in tSaves)
                    tUIItems.Add(new UIItem(UIItemType.TextFix, save, Color.White, Fonts[Font.menuItem03.ToString()], UIItemsFlow.Vertical, UIAction.LoadGame, "", save));

                GameUIs[UITemplate.mainLoadSaves].Position = new Vector2(GameUIs[UITemplate.mainLoad].Size.X + 4, Resolution.Y - 90);
                GameUIs[UITemplate.mainLoadSaves].SetUIItems(tUIItems);
                GameUIs[UITemplate.mainLoadSaves].ToDraw = true;
                LoadToggled = true;
            }
        }

        public void ExecuteMenuAction(UIAction action, string actionText)
        {
            switch (action)
            {
                case UIAction.Exit:
                    Exit();
                    break;
                case UIAction.NewGameMenu:
                    GameUIs[UITemplate.mainLoadSaves].ToDraw = false;
                    LoadToggled = false;
                    GameUIs[UITemplate.mainNewMenu].Toggle();
                    break;
                case UIAction.NewGameSurvival:
                    GGPScreen = new GameplayScreen();
                    GGPScreen.Initialize();

                    ToggleMainMenu(false);
                    ChangeScreens(MMenuScreen, GGPScreen);
                    State = ClientState.Game;
                    break;
                case UIAction.NewGame:
                    GGPScreen = new GameplayScreen();
                    GGPScreen.Initialize();

                    ToggleMainMenu(false);
                    ChangeScreens(MMenuScreen, GGPScreen);
                    State = ClientState.Game;
                    break;
                case UIAction.LoadGame:
                    GameUIs[UITemplate.mainNewMenu].ToDraw = false;
                    if (actionText == "")
                    {
                        LoadSavedGames();
                    }
                    else
                    {
                        GGPScreen = new GameplayScreen();
                        GGPScreen.Initialize(GameType.Load, actionText);

                        ToggleMainMenu(false);
                        ChangeScreens(MMenuScreen, GGPScreen);
                        State = ClientState.Game;
                    }
                    break;
                case UIAction.EndTurn:
                    GGPScreen.NextTurn();

                    if (GGPScreen.CurrentLevel.Stage[Vector2.Zero].Durability <= 0)
                    {
                        State = ClientState.GameOver;
                        GOScreen = new GameOverScreen();
                        ChangeScreens(GGPScreen, GOScreen);

                        PlayerCamera.SetFocalPoint(Resolution / 2);
                    }
                        
                    break;
                case UIAction.BuyTile:

                    Dictionary<ResourceType, int> tReqs = GGPScreen.TileLevelCosts[GGPScreen.GetCurrentTileLevel()];
                    if (GGPScreen.CheckIfPlayerHasResources(tReqs))
                    {
                        GGPScreen.SpendResources(tReqs);
                        GGPScreen.AddRoom(actionText);
                    }                        
                    else
                        Log.Add(new LogEntry("Need more resources to afford a new tile."));
                    break;
                case UIAction.UpgradeTile:
                    switch (actionText.ToLower())
                    {
                        case "buildui":
                            TogTileSheet(false);
                            TogBuildUI(true);
                            break;
                        case "improveui":
                            TogTileSheet(false);
                            TogImproveUI(true);
                            break;
                    }
                    //ToggleTileSheet();
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
                        if (actionText.Contains(':'))
                        {
                            string[] split = actionText.Split(':');

                            switch (split[1].ToLower())
                            {
                                case "gold":
                                    GGPScreen.Player1.ActiveRoom.AddResources(int.Parse(split[0]), ResourceType.Gold);
                                    TogImproveUI(false);
                                    break;
                                case "food":
                                    GGPScreen.Player1.ActiveRoom.AddResources(int.Parse(split[0]), ResourceType.Food);
                                    TogImproveUI(false);
                                    break;
                                case "production":
                                    GGPScreen.Player1.ActiveRoom.AddResources(int.Parse(split[0]), ResourceType.Production);
                                    TogImproveUI(false);
                                    break;
                                case "energy":
                                    GGPScreen.Player1.ActiveRoom.AddResources(int.Parse(split[0]), ResourceType.Energy);
                                    TogImproveUI(false);
                                    break;
                                case "tower":
                                    GGPScreen.Player1.ActiveRoom.AddThing(int.Parse(split[0]), ThingType.Tower);
                                    TogImproveUI(false);
                                    break;
                                case "guard":
                                    GGPScreen.Player1.ActiveRoom.AddThing(int.Parse(split[0]), ThingType.Guard);
                                    TogImproveUI(false);
                                    break;
                                case "special":
                                    GGPScreen.Player1.ActiveRoom.AddThing(int.Parse(split[0]), ThingType.Special);
                                    TogImproveUI(false);
                                    break;
                                case "":
                                    break;
                            }
                        }                      
                    }
                    else
                    {
                        ToggleTileSheet();
                    }
                    break;
                default:

                    break;
            }
        }

        public static UI UpdateSelectionUI(UITemplate uiName)
        {
            UI tUI;
            //ICITTE
            tUI = new UI(UIType.Basic, uiName.ToString(), "Building UI", new Vector2(300, 300), new Vector2(2, 2));
            tUI.BackAlpha = 0.35f;
            List<UIItem> tUIItems = new List<UIItem>();
            tUIItems.Add(new UIItem(UIItemType.ImageFix, "", Color.CornflowerBlue, Fonts[Font.menuItem01.ToString()], UIItemsFlow.Vertical, UIAction.TileSheet, "gear_24"));
            tUIItems.Add(new UIItem(UIItemType.TextFix, "Guard", Color.CornflowerBlue, Fonts[Font.menuItem03.ToString()], UIItemsFlow.Vertical, UIAction.TileSheet, "", "0:Guard"));
            tUIItems.Add(new UIItem(UIItemType.TextFix, "Tower", Color.CornflowerBlue, Fonts[Font.menuItem03.ToString()], UIItemsFlow.Vertical, UIAction.TileSheet, "", "0:Tower"));
            tUIItems.Add(new UIItem(UIItemType.TextFix, "Special", Color.CornflowerBlue, Fonts[Font.menuItem03.ToString()], UIItemsFlow.Vertical, UIAction.TileSheet, "", "0:Special"));

            tUI.SetUIItems(tUIItems);
            tUI.Position = GGPScreen.Player1.ActiveRoom.Position + new Vector2(TileSize.X - 28, 0);

            return tUI;
        }

        public static UI GenerateUI(UITemplate uiName)
        {
            UI tUI;
            switch (uiName)
            {
                case UITemplate.mainNew:
                    tUI = new UI(UIType.Basic, uiName.ToString(), "Main Menu - New Game", new Vector2(300, 300), Resolution / 2);
                    tUI.BackAlpha = 0.0f;
                    tUI.ItemsFlow = UIItemsFlow.Vertical;
                    List<UIItem> tUIItems = new List<UIItem>();
                    tUIItems.Add(new UIItem(UIItemType.TextFix, "New Game", Color.White, Fonts[Font.menuItem01.ToString()], UIItemsFlow.Vertical, UIAction.NewGameMenu, ""));
                    tUI.Position = new Vector2(2, Resolution.Y - 120);
                    tUI.SetUIItems(tUIItems);
                    break;
                case UITemplate.mainNewMenu:
                    tUI = new UI(UIType.Basic, uiName.ToString(), "Main Menu - New Game Menu", new Vector2(300, 300), Resolution / 2);
                    tUI.BackAlpha = 0.0f;
                    tUI.ItemsFlow = UIItemsFlow.Vertical;
                    tUIItems = new List<UIItem>();
                    tUIItems.Add(new UIItem(UIItemType.TextFix, GameType.Survival.ToString(), Color.CornflowerBlue, Fonts[Font.menuItem02.ToString()], UIItemsFlow.Vertical, UIAction.NewGameSurvival, ""));
                    tUIItems.Add(new UIItem(UIItemType.TextFix, GameType.Exploration.ToString(), Color.CornflowerBlue, Fonts[Font.menuItem02.ToString()], UIItemsFlow.Vertical, UIAction.NewGameExploration, ""));
                    tUIItems.Add(new UIItem(UIItemType.TextFix, GameType.Defense.ToString(), Color.CornflowerBlue, Fonts[Font.menuItem02.ToString()], UIItemsFlow.Vertical, UIAction.NewGameDefense, ""));
                    tUI.Position = new Vector2(GameUIs[UITemplate.mainLoad].Size.X + 4, Resolution.Y - 120);
                    tUI.SetUIItems(tUIItems);
                    break;
                case UITemplate.mainLoad:
                    tUI = new UI(UIType.Basic, uiName.ToString(), "Main Menu - Load Game", new Vector2(300, 300), Resolution / 2);
                    tUI.BackAlpha = 0.0f;
                    tUI.ItemsFlow = UIItemsFlow.Vertical;
                    tUIItems = new List<UIItem>();
                    tUIItems.Add(new UIItem(UIItemType.TextFix, "Load Game", Color.White, Fonts[Font.menuItem01.ToString()], UIItemsFlow.Vertical, UIAction.LoadGame, "", ""));
                    tUI.Position = new Vector2(2, Resolution.Y - 90);
                    tUI.SetUIItems(tUIItems);
                    break;
                case UITemplate.mainLoadSaves:
                    tUI = new UI(UIType.Basic, uiName.ToString(), "Main Menu - Saved Games", new Vector2(300, 300), Resolution / 2);
                    tUI.BackAlpha = 0.0f;
                    tUI.ItemsFlow = UIItemsFlow.Vertical;
                    tUI.ToDraw = false;
                    break;
                case UITemplate.mainOptions:
                    tUI = new UI(UIType.Basic, uiName.ToString(), "Main Menu - Options", new Vector2(300, 300), Resolution / 2);
                    tUI.BackAlpha = 0.0f;
                    tUI.ItemsFlow = UIItemsFlow.Vertical;
                    tUIItems = new List<UIItem>();
                    tUIItems.Add(new UIItem(UIItemType.TextFix, "Options", Color.White, Fonts[Font.menuItem01.ToString()], UIItemsFlow.Vertical, UIAction.Options, "", ""));
                    tUI.Position = new Vector2(2, Resolution.Y - 60);
                    tUI.SetUIItems(tUIItems);
                    break;
                case UITemplate.mainExit:
                    tUI = new UI(UIType.Basic, uiName.ToString(), "Main Menu - Exit", new Vector2(300, 300), Resolution / 2);
                    tUI.BackAlpha = 0.0f;
                    tUI.ItemsFlow = UIItemsFlow.Vertical;
                    tUIItems = new List<UIItem>();
                    tUIItems.Add(new UIItem(UIItemType.TextFix, "Exit", Color.White, Fonts[Font.menuItem01.ToString()], UIItemsFlow.Vertical, UIAction.Exit, "", ""));
                    tUI.Position = new Vector2(2, Resolution.Y - 30);
                    tUI.SetUIItems(tUIItems);
                    break;
                case UITemplate.toolbar01:
                    tUI = new UI(UIType.Basic, uiName.ToString(), "Main Tool Bar", new Vector2(300, 300), new Vector2(2, 2));
                    tUI.BackAlpha = 0.35f;
                    tUI.ItemsFlow = UIItemsFlow.Horizontal;
                    GGPScreen.RefreshResourcesUI(tUI);
                    tUI.Position = new Vector2(2, 2) + PlayerCamera.Position;
                    break;
                case UITemplate.income:
                    tUI = new UI(UIType.Basic, uiName.ToString(), "Income Panel", new Vector2(300, 300), new Vector2(2, 2));
                    tUI.BackAlpha = 0.35f;
                    tUI.ItemsFlow = UIItemsFlow.Vertical;
                    GGPScreen.RefreshIncomeUI(tUI);
                    tUI.Position = new Vector2(2 + Resolution.X - 40, 2) + PlayerCamera.Position;
                    break;
                case UITemplate.turn01:
                    tUI = new UI(UIType.Basic, uiName.ToString(), "Turns UI", new Vector2(300, 300), new Vector2(2, 2));
                    tUI.BackAlpha = 0.35f;
                    tUIItems = new List<UIItem>();
                    tUIItems.Add(new UIItem(UIItemType.TextFix, "End Turn", Color.CornflowerBlue, Fonts[Font.menuItem01.ToString()], UIItemsFlow.Vertical, UIAction.EndTurn));
                    tUI.SetUIItems(tUIItems);
                    tUI.Position = new Vector2(Resolution.X - tUI.Size.X, Resolution.Y - tUI.Size.Y) + PlayerCamera.Position;
                    break;
                case UITemplate.tileSheet:
                    tUI = new UI(UIType.Basic, uiName.ToString(), "Tile UI", new Vector2(300, 300), new Vector2(2, 2));
                    tUI.BackAlpha = 0.35f;
                    tUIItems = new List<UIItem>();
                    tUIItems.Add(new UIItem(UIItemType.ImageFix, "", Color.CornflowerBlue, Fonts[Font.menuItem01.ToString()], UIItemsFlow.Vertical, UIAction.TileSheet, "gear_24"));
                    tUIItems.Add(new UIItem(UIItemType.TextFix, "Upgrade", Color.CornflowerBlue, Fonts[Font.menuItem03.ToString()], UIItemsFlow.Vertical, UIAction.UpgradeTile, "", "ImproveUI"));
                    tUIItems.Add(new UIItem(UIItemType.TextFix, "Building", Color.CornflowerBlue, Fonts[Font.menuItem03.ToString()], UIItemsFlow.Vertical, UIAction.UpgradeTile, "", "BuildUI"));
                    tUI.SetUIItems(tUIItems);
                    tUI.Position = GGPScreen.Player1.ActiveRoom.Position + new Vector2(TileSize.X - 28, 0);
                    break;
                case UITemplate.improveUI:
                    tUI = new UI(UIType.Basic, uiName.ToString(), "Upgrade UI", new Vector2(300, 300), new Vector2(2, 2));
                    tUI.BackAlpha = 0.35f;
                    tUIItems = new List<UIItem>();
                    tUIItems.Add(new UIItem(UIItemType.ImageFix, "", Color.CornflowerBlue, Fonts[Font.menuItem01.ToString()], UIItemsFlow.Vertical, UIAction.TileSheet, "gear_24"));
                    tUIItems.Add(new UIItem(UIItemType.TextFix, "Mine", Color.CornflowerBlue, Fonts[Font.menuItem03.ToString()], UIItemsFlow.Vertical, UIAction.TileSheet, "", "1:Gold"));
                    tUIItems.Add(new UIItem(UIItemType.TextFix, "Farm", Color.CornflowerBlue, Fonts[Font.menuItem03.ToString()], UIItemsFlow.Vertical, UIAction.TileSheet, "", "1:Food"));
                    tUIItems.Add(new UIItem(UIItemType.TextFix, "Workshop", Color.CornflowerBlue, Fonts[Font.menuItem03.ToString()], UIItemsFlow.Vertical, UIAction.TileSheet, "", "1:Production"));
                    tUIItems.Add(new UIItem(UIItemType.TextFix, "Energy Source", Color.CornflowerBlue, Fonts[Font.menuItem03.ToString()], UIItemsFlow.Vertical, UIAction.TileSheet, "", "1:Energy"));

                    tUI.SetUIItems(tUIItems);
                    tUI.Position = GGPScreen.Player1.ActiveRoom.Position + new Vector2(TileSize.X - 28, 0);
                    break;
                case UITemplate.buildUI:
                    tUI = new UI(UIType.Basic, uiName.ToString(), "Building UI", new Vector2(300, 300), new Vector2(2, 2));
                    tUI.BackAlpha = 0.35f;
                    tUIItems = new List<UIItem>();
                    tUIItems.Add(new UIItem(UIItemType.ImageFix, "", Color.CornflowerBlue, Fonts[Font.menuItem01.ToString()], UIItemsFlow.Vertical, UIAction.TileSheet, "gear_24"));
                    tUIItems.Add(new UIItem(UIItemType.TextFix, "Guard", Color.CornflowerBlue, Fonts[Font.menuItem03.ToString()], UIItemsFlow.Vertical, UIAction.TileSheet, "", "0:Guard"));
                    tUIItems.Add(new UIItem(UIItemType.TextFix, "Tower", Color.CornflowerBlue, Fonts[Font.menuItem03.ToString()], UIItemsFlow.Vertical, UIAction.TileSheet, "", "0:Tower"));
                    tUIItems.Add(new UIItem(UIItemType.TextFix, "Special", Color.CornflowerBlue, Fonts[Font.menuItem03.ToString()], UIItemsFlow.Vertical, UIAction.TileSheet, "", "0:Special"));

                    tUI.SetUIItems(tUIItems);
                    tUI.Position = GGPScreen.Player1.ActiveRoom.Position + new Vector2(TileSize.X - 28, 0);
                    break;
                case UITemplate.tileExpendNorth:
                    tUI = new UI(UIType.Basic, uiName.ToString(), "Tile Expend - North UI", new Vector2(300, 300), new Vector2(2, 2));
                    tUI.TTText = "Discover a new tile - North";
                    tUI.BackAlpha = 0.35f;
                    tUIItems = new List<UIItem>();
                    tUIItems.Add(new UIItem(UIItemType.ImageFix, "", Color.CornflowerBlue, Fonts[Font.menuItem01.ToString()], UIItemsFlow.Vertical, UIAction.BuyTile, "plus_24"));
                    tUI.SetUIItems(tUIItems);
                    tUI.Position = GGPScreen.Player1.ActiveRoom.Position + new Vector2((TileSize.X / 2) - 14, -14);
                    break;
                case UITemplate.tileExpendEast:
                    tUI = new UI(UIType.Basic, uiName.ToString(), "Tile Expend - East UI", new Vector2(300, 300), new Vector2(2, 2));
                    tUI.TTText = "Discover a new tile - East";
                    tUI.BackAlpha = 0.35f;
                    tUIItems = new List<UIItem>();
                    tUIItems.Add(new UIItem(UIItemType.ImageFix, "", Color.CornflowerBlue, Fonts[Font.menuItem01.ToString()], UIItemsFlow.Vertical, UIAction.BuyTile, "plus_24"));
                    tUI.SetUIItems(tUIItems);
                    tUI.Position = GGPScreen.Player1.ActiveRoom.Position + new Vector2((TileSize.X) - 14, (TileSize.Y / 2) - 14);
                    break;
                case UITemplate.tileExpendSouth:
                    tUI = new UI(UIType.Basic, uiName.ToString(), "Tile Expend - South UI", new Vector2(300, 300), new Vector2(2, 2));
                    tUI.TTText = "Discover a new tile - South";
                    tUI.BackAlpha = 0.35f;
                    tUIItems = new List<UIItem>();
                    tUIItems.Add(new UIItem(UIItemType.ImageFix, "", Color.CornflowerBlue, Fonts[Font.menuItem01.ToString()], UIItemsFlow.Vertical, UIAction.BuyTile, "plus_24"));
                    tUI.SetUIItems(tUIItems);
                    tUI.Position = GGPScreen.Player1.ActiveRoom.Position + new Vector2((TileSize.X / 2) - 14, (TileSize.Y) - 14);
                    break;
                case UITemplate.tileExpendWest:
                    tUI = new UI(UIType.Basic, uiName.ToString(), "Tile Expend - West UI", new Vector2(300, 300), new Vector2(2, 2));
                    tUI.TTText = "Discover a new tile - West";
                    tUI.BackAlpha = 0.35f;
                    tUIItems = new List<UIItem>();
                    tUIItems.Add(new UIItem(UIItemType.ImageFix, "", Color.CornflowerBlue, Fonts[Font.menuItem01.ToString()], UIItemsFlow.Vertical, UIAction.BuyTile, "plus_24"));
                    tUI.SetUIItems(tUIItems);
                    tUI.Position = GGPScreen.Player1.ActiveRoom.Position + new Vector2(-14, (TileSize.Y / 2) - 14);
                    break;
                case UITemplate.log:
                    tUI = new UI(UIType.BasicInvis, uiName.ToString(), "Events Journal", new Vector2(300, 300), new Vector2(2, Resolution.Y - 180));
                    tUI.BackAlpha = 0.35f;
                    tUI.ItemsFlow = UIItemsFlow.Vertical;
                    tUIItems = new List<UIItem>();
                    tUIItems.Add(new UIItem(UIItemType.TextFix, "Events Journal", Color.CornflowerBlue, Fonts[Font.menuItem02.ToString()], UIItemsFlow.Vertical, UIAction.ToggleLog));
                    tUI.SetUIItems(tUIItems);
                    tUI.Position = new Vector2(2, Resolution.Y) + PlayerCamera.Position;
                    break;
                case UITemplate.tooltip:
                    tUI = new UI(UIType.Basic, uiName.ToString(), "Tool Tips", new Vector2(300, 300), new Vector2(2, Resolution.Y));
                    tUI.BackAlpha = 0.40f;
                    tUI.ItemsFlow = UIItemsFlow.Vertical;
                    tUI.Position = Input.MousePosition();
                    tUI.ToDraw = false;
                    break;
                default:
                    tUI = new UI();
                    break;
            }
            return tUI;
        }


        protected override void Draw(GameTime gameTime)
        {
            var startIndex = ScreenList.Count - 1;
            while (ScreenList[startIndex].IsPopup)
            {
                startIndex--;
            }

            GraphicsDevice.Clear(ScreenList[startIndex].BackgroundColor);
            GraphicsDeviceMgr.GraphicsDevice.Clear(ScreenList[startIndex].BackgroundColor);
            Sprites.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null, null, PlayerCamera.ViewMatrix);

            for (var i = startIndex; i < ScreenList.Count; i++)
            {
                ScreenList[i].Draw(gameTime);
            }

            int maxUII = 999;
            int currUII = 0;
            foreach (KeyValuePair<UITemplate, UI> ui in GameUIs)
            {
                if (State == ClientState.MainMenu || State == ClientState.GameOver)
                    maxUII = 5;

                if (currUII <= maxUII || maxUII == 0)
                    ui.Value.Draw(gameTime);
                else
                    break;
                currUII++;
            }                

            if (DebugMode)
            {
                //FPS COUNTER
                var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                _frameCounter.Update(deltaTime);
                var fps = string.Format("FPS: {0}", Math.Round(_frameCounter.AverageFramesPerSecond));
                Sprites.DrawString(Fonts[Font.debug01.ToString()], fps, PlayerCamera.Position + new Vector2(PlayerCamera.ScreenDimensions.X - 64, 0), Color.CornflowerBlue, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
            }

            Sprites.End();
            base.Draw(gameTime);
        }

        public static void Main()
        {
            using (ScreenManager manager = new ScreenManager())
            {
                manager.Run();
            }
        }
    }
}