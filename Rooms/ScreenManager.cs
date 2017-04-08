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


namespace Rooms
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
        public static Vector2 TileSize = new Vector2(32, 32);
        public static Vector2 Resolution = new Vector2(1440, 900);
        public static Camera PlayerCamera;
        private FrameCounter _frameCounter = new FrameCounter();
        public const string CONTENTPATH = "C:/Prog/Rooms/Rooms/Content/";
        public const string IMAGESPATH = CONTENTPATH + "Images/";
        public static Dictionary<int, Vector2> RoomSizes;
        public static float Delta;

        public static void InitRoomSizes()
        {
            RoomSizes.Add(0, new Vector2(20, 20));
            RoomSizes.Add(1, new Vector2(20, 20));
            RoomSizes.Add(2, new Vector2(24, 24));
            RoomSizes.Add(3, new Vector2(28, 28));
            RoomSizes.Add(4, new Vector2(32, 32));
            RoomSizes.Add(5, new Vector2(36, 36));
            RoomSizes.Add(6, new Vector2(40, 40));
            RoomSizes.Add(7, new Vector2(44, 44));
            RoomSizes.Add(8, new Vector2(48, 48));
        }


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

            RoomSizes = new Dictionary<int, Vector2>();
            InitRoomSizes();

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

            InitializeTextures();

            GraphicsDeviceMgr.SynchronizeWithVerticalRetrace = false;

            IsFixedTimeStep = false;
            GraphicsDeviceMgr.ApplyChanges();

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

        protected override void Update(GameTime gameTime)
        {
            try
            {
                Delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
                Input.Update();
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
                {
                    ScreenList[i].Update(gameTime, Input);
                    if (ScreenList[i].MenuAction != null && ScreenList[i].MenuAction != "")
                    {
                        string tA = ScreenList[i].MenuAction.ToLower();
                        switch (tA)
                        {
                            case "newgame":
                                GGPScreen = new GameplayScreen();
                                GGPScreen.Initialize();

                                ChangeScreens(MMenuScreen, GGPScreen);
                                State = ClientState.Game;
                                break;
                            case "loadgame":

                                break;
                            case "options":

                                break;
                            case "exit":
                                Exit();
                                break;
                        }

                        ScreenList[i].MenuAction = "";
                    }
                    
                }


                switch (State)
                {
                    case ClientState.Splashscreen:
                        if (Input.KeyDown(Keys.Space, Keys.Enter) || Input.ButtonDown(Buttons.A, Buttons.Start))
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
        

        protected override void Draw(GameTime gameTime)
        {
            var startIndex = ScreenList.Count - 1;
            while (ScreenList[startIndex].IsPopup)
            {
                startIndex--;
            }

            GraphicsDevice.Clear(ScreenList[startIndex].BackgroundColor);
            GraphicsDeviceMgr.GraphicsDevice.Clear(ScreenList[startIndex].BackgroundColor);
            Sprites.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, PlayerCamera.ViewMatrix);

            for (var i = startIndex; i < ScreenList.Count; i++)
            {
                ScreenList[i].Draw(gameTime);
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