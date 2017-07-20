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

        public static void TogTileSheet(bool tog)
        {
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

        public static void ToggleMainMenu(bool visibleOrNot)
        {
            GameUIs[UITemplate.mainNew].ToDraw = visibleOrNot;
            GameUIs[UITemplate.mainLoad].ToDraw = visibleOrNot;
            GameUIs[UITemplate.mainOptions].ToDraw = visibleOrNot;
            GameUIs[UITemplate.mainExit].ToDraw = visibleOrNot;
        }

        public void LoadSavedGames()
        {
            
            if (LoadToggled)
            {
                
                LoadToggled = false;
            }
            else
            {
                List<UIItem> tUIItems = new List<UIItem>();
                List<string> tSaves = FileManager.GetSaves(SAVESPATH);
                foreach(string save in tSaves)
                    tUIItems.Add(new UIItem(UIItemType.TextFix, save, Color.White, Fonts[Font.menuItem03.ToString()], UIItemsFlow.Vertical, UIAction.LoadGame, "", save));

                GameUIs[UITemplate.mainLoadSaves].Position = new Vector2(152, Resolution.Y - 90);
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
                case UIAction.NewGame:
                    GGPScreen = new GameplayScreen();
                    GGPScreen.Initialize();

                    ToggleMainMenu(false);
                    ChangeScreens(MMenuScreen, GGPScreen);
                    State = ClientState.Game;
                    break;
                case UIAction.LoadGame:
                    if (actionText == "")
                    {
                        LoadSavedGames();
                    }
                    else
                    {
                        GGPScreen = new GameplayScreen();
                        GGPScreen.Initialize(actionText);

                        ToggleMainMenu(false);
                        ChangeScreens(MMenuScreen, GGPScreen);
                        State = ClientState.Game;
                    }
                    break;
                case UIAction.EndTurn:
                    GGPScreen.NextTurn();
                    break;
                case UIAction.BuyTile:
                    GGPScreen.AddRoom(actionText);
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
                        switch (split[1])
                        {
                            case "Gold":
                                GGPScreen.Player1.ActiveRoom.AddResources(int.Parse(split[0]), ResourceType.Gold);
                                break;
                            case "Food":
                                GGPScreen.Player1.ActiveRoom.AddResources(int.Parse(split[0]), ResourceType.Food);
                                break;
                            case "Wood":
                                GGPScreen.Player1.ActiveRoom.AddResources(int.Parse(split[0]), ResourceType.Wood);
                                break;
                            case "Production":
                                GGPScreen.Player1.ActiveRoom.AddResources(int.Parse(split[0]), ResourceType.Production);
                                break;
                            case "Energy":
                                GGPScreen.Player1.ActiveRoom.AddResources(int.Parse(split[0]), ResourceType.Energy);
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
                    tUIItems.Add(new UIItem(UIItemType.TextFix, "New Game", Color.White, Fonts[Font.menuItem01.ToString()], UIItemsFlow.Vertical, UIAction.NewGame, ""));
                    tUI.Position = new Vector2(2, Resolution.Y - 120);
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
                    tUIItems.Add(new UIItem(UIItemType.TextFix, "Mine", Color.CornflowerBlue, Fonts[Font.menuItem03.ToString()], UIItemsFlow.Vertical, UIAction.TileSheet, "", "1:Gold"));
                    tUIItems.Add(new UIItem(UIItemType.TextFix, "Farm", Color.CornflowerBlue, Fonts[Font.menuItem03.ToString()], UIItemsFlow.Vertical, UIAction.TileSheet, "", "1:Food"));
                    tUIItems.Add(new UIItem(UIItemType.TextFix, "Lumbermill", Color.CornflowerBlue, Fonts[Font.menuItem03.ToString()], UIItemsFlow.Vertical, UIAction.TileSheet, "", "1:Wood"));
                    tUIItems.Add(new UIItem(UIItemType.TextFix, "Workshop", Color.CornflowerBlue, Fonts[Font.menuItem03.ToString()], UIItemsFlow.Vertical, UIAction.TileSheet, "", "1:Production"));
                    tUIItems.Add(new UIItem(UIItemType.TextFix, "Energy Source", Color.CornflowerBlue, Fonts[Font.menuItem03.ToString()], UIItemsFlow.Vertical, UIAction.TileSheet, "", "1:Energy"));
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

            foreach (KeyValuePair<UITemplate, UI> ui in GameUIs)
                ui.Value.Draw(gameTime);

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