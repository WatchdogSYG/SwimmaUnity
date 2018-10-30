using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Swimma.GameLibrary;
using Swimma.Screens;

using Common;
using Microsoft.Xna.Framework.Input;
using REP_SLC;
using Common.DataLogging;
namespace Swimma
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Private Variables

        public static TimeSpan UnlockAt { get { return TimeSpan.FromHours(1.5f); } }

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        // Percentage of the screen on every side is the safe area
        private const float SafeAreaPortion = 0.05f;

        //set the screen size
        private const int screenWidth = 1280;//1024;
        private const int screenHeight = 720;//768;

        //the menus states
        private MenuManager menuManager;
        private StartMenu startMenu;
        private MainMenu mainMenu;
        private GamePlayScreen gameplayScreen;
        private PauseMenu pauseMenu;
        private GameOverScreen gameOverScreen;
        private ControlScreen controlScreen;

        private Rectangle screenRectangle;
        private Rectangle screenSize;
        private Rectangle GamePlayArea;


        #endregion

        public static string Name { get { return "Swimma"; } }
        public static List<KeyValuePair<string, string>> GetHighScores()
        {
            var collection = new List<KeyValuePair<string, string>>();
            int[] scores = LoadHighScores(10);
            string[] users = LoadUsers(10);
            int count = Math.Min(scores.Length, users.Length);
            for (int s = 0; s < count; ++s)
            {
                if (scores[s] < 0) continue;
                collection.Add(new KeyValuePair<string, string>(users[s], scores[s].ToString()));
            }
            return collection;
        }

        public static Game1 Game;

        public Game1() : this(false) { }
        public Game1(bool useGraphics)
        {
            Game = this;

            Content.RootDirectory = "Content/Swimma";


            if (useGraphics)
            {


                graphics = new GraphicsDeviceManager(this);
                graphics.PreferredBackBufferWidth = screenWidth;
                graphics.PreferredBackBufferHeight = screenHeight;
                // graphics.IsFullScreen =true;
                // graphics.IsFullScreen = false;

                IsFixedTimeStep = true;

                if (!graphics.IsFullScreen)
                {
                    graphics.PreferredBackBufferWidth = 1280;
                    graphics.PreferredBackBufferHeight = 720;

                }
#if WINDOWS_PHONE
                graphics.PreparingDeviceSettings += (s, e) => {
                    e.GraphicsDeviceInformation.PresentationParameters.PresentationInterval = PresentInterval.Immediate;
                };
#endif

                //graphics.ApplyChanges();
            }
        }


        protected override void Initialize()
        {
            // Begin an indefninite vibration
            RumbleTimer.Initialize(this);

            screenSize = new Rectangle(0, 0, screenWidth, screenHeight);// GraphicsDevice.Viewport.Bounds;
          
            base.Initialize();
        }

        /// <summary>
        /// Destroys all game states.
        /// </summary>
        protected void DestroyContent()
        {
            menuManager = null;
            startMenu = null;
            mainMenu = null;
            gameplayScreen = null;
            gameOverScreen = null;
            controlScreen = null;
            pauseMenu = null;
        }

        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);

            ////Initialize the various screens in the game
            //startMenu = new StartMenu(this, screenSize, new EventHandler(MenuEvent));
            //mainMenu = new MainMenu(this, screenSize, new EventHandler(MenuEvent));
            //gameOverScreen = new GameOverScreen(this, screenSize, new EventHandler(MenuEvent));
            //controlScreen = new ControlScreen(this, screenSize, new EventHandler(MenuEvent));
            //pauseMenu = new PauseMenu(this, screenSize, new EventHandler(MenuEvent));


            startMenu = new StartMenu(this, screenSize, new EventHandler(MenuEvent));
            mainMenu = new MainMenu(this, screenSize, new EventHandler(MenuEvent));
            gameOverScreen = new GameOverScreen(this, screenSize, new EventHandler(MenuEvent));
            controlScreen = new ControlScreen(this, screenSize, new EventHandler(MenuEvent));
            pauseMenu = new PauseMenu(this, screenSize, new EventHandler(MenuEvent));
            //Set the current screen
            menuManager = startMenu;

            LoadAudio();
            base.LoadContent();
        }
        private void LoadAudio()
        {
            SoundLibrary.BackGroundMusic = Content.Load<Song>(@"Audio/themeTune");
            SoundLibrary.BackGroundSongStarted = false;
            SoundLibrary.BubbleCollect = Content.Load<SoundEffect>(@"Audio/pop");

            SoundLibrary.GemCollect = Content.Load<SoundEffect>(@"Audio/bling");
            SoundLibrary.MenuItemFocus = Content.Load<SoundEffect>(@"Audio/beep");
            SoundLibrary.MenuItemOK = Content.Load<SoundEffect>(@"Audio/okBeep");

            SoundLibrary.GameOver = Content.Load<SoundEffect>(@"Audio/laugh");

            var settings = (SettingsService)Services.GetService(typeof(SettingsService));
            MediaPlayer.Volume = settings.AugmentMusicVolume(.7f);
            MediaPlayer.IsRepeating = true;
        }

        /// <summary>
        /// Switches to the correct screen state when called
        /// </summary>
        /// <param name="obj">The object that is passed from the current menu state</param>
        /// <param name="e"></param>
        public void MenuEvent(object obj, EventArgs e)
        {
            var logger = (LoggingService)Services.GetService(typeof(LoggingService));
            switch (obj.ToString()) {

                case "Start": {
                    gameplayScreen = null;
                    menuManager = mainMenu;
                } break;
                case "Select": {
                    menuManager = new PlayerSelection(this, screenSize, new EventHandler(MenuEvent));
                } break;
                case "PlayGame": {
                    if (logger != null) logger.UpdateData(LogChannel.EventChannel, "GameStart");
                    if (gameplayScreen == null)
                        gameplayScreen = new GamePlayScreen(this, graphics, screenSize, new EventHandler(MenuEvent));
                    gameplayScreen.PauseGame(false);
                    menuManager = gameplayScreen;
                } break;
                case "ExitGame": {
                    this.Exit();
                } break;
                case "Pause": {
                    menuManager = pauseMenu;
                } break;
                case "Controls": {
                    menuManager = controlScreen;
                } break;
                case "GameOver": {
                    if (logger != null) logger.UpdateData(LogChannel.EventChannel, "GameOver");

                    gameOverScreen.Score = gameplayScreen.Score;
                    SaveHighScore(GetUserName(), "" + gameplayScreen.Score);
                    gameplayScreen.ResetGame();
                    menuManager = gameOverScreen;
                } break;
                default:
                    throw new Exception(String.Format("The GameState \"{0}\" Cannot Be found.", obj.ToString()));

            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            DestroyContent();
            //try
            //{
            //    Level2.bgWorker.Abort();
            //}
            //catch { }
            //this.UnloadContent();
            MediaPlayer.Stop();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            REP_SLC.DataLogger.Environment = (menuManager != gameplayScreen ? REP_SLC.LoggingEnvironment.MENU : REP_SLC.LoggingEnvironment.ACTIVE);

            //// Allows the game to exit
            //  if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    this.Exit();

            if (!SoundLibrary.BackGroundSongStarted && SoundLibrary.BackGroundMusic != null)
            {
                MediaPlayer.Play(SoundLibrary.BackGroundMusic);
                SoundLibrary.BackGroundSongStarted = true;
            }

            //update the current screen state
            //if we are pausing the game we need to update the game state as well
            if (menuManager == pauseMenu)
            {
                gameplayScreen.Update(gameTime);
            }
            menuManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //clears the current game state and its graphics content from the screen.
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //draw the current game state
            //if we are pausing the game we need to draw the gamestate before we can overlay the pause state
            if (menuManager == pauseMenu)
            {

                gameplayScreen.Draw(spriteBatch, gameTime);

            }
            menuManager.Draw(spriteBatch, gameTime);
            base.Draw(gameTime);
        }
        #region SaveAndLoad
        protected void SaveHighScore(String User, String Score)
        {
            int[] scores = LoadHighScores(10);
            string[] users = LoadUsers(10);
            int tempScore = -1;
            String scoreFormat = "";
            String comma = "";

            Int32.TryParse(Score, out tempScore);

            for (int i = 0; i < scores.Length && i < 10; i++)
            {
                if (i > 0)
                {
                    comma = ",";
                }
                if (tempScore > scores[i])
                {
                    scoreFormat += comma + User + ":" + tempScore;
                    tempScore = scores[i];
                }
                else
                    scoreFormat += comma + users[i] + ":" + scores[i];

            }

            System.IO.File.WriteAllText(@"UserFiles/SwimmaScore.txt", scoreFormat);
        }

        public static int[] LoadHighScores(int amount)
        {
            int[] result = new int[amount];
            int semiResult;
            String[] strResults;
            try
            {
                System.IO.StreamReader file2 = new System.IO.StreamReader("UserFiles/SwimmaScore.txt");
                strResults = file2.ReadToEnd().Split(',');
                file2.Close();
                for (int i = 0; i < result.Length; i++)
                {
                    semiResult = -1;
                    Int32.TryParse(strResults[i].Split(':')[1], out semiResult);
                    result[i] = semiResult;
                }
            }
            catch (Exception)
            {
                for (int i = 0; i < result.Length; i++)
                    result[i] = -1;
            }

            return result;
        }

        public static string[] LoadUsers(int amount)
        {
            string[] result = new string[amount];
            string semiResult;
            String[] strResults;
            try
            {
                System.IO.StreamReader file2 = new System.IO.StreamReader("UserFiles/SwimmaScore.txt");
                strResults = file2.ReadToEnd().Split(',');
                file2.Close();
                for (int i = 0; i < result.Length; i++)
                {
                    semiResult = "user";
                    if (i < strResults.Length)
                    {
                        result[i] = strResults[i].Split(':')[0];
                    }
                    else
                    {
                        result[i] = semiResult;
                    }
                }
            }
            catch (Exception)
            {
                for (int i = 0; i < result.Length; i++)
                    result[i] = "user";
            }

            return result;
        }

        #endregion
        #region CatalogFuntion
        public string GetUserName()
        {
            var sessionService = (SessionService)Services.GetService(typeof(SessionService));
            if (sessionService != null) return sessionService.Username;
            else return "User";
        }

        public new void Exit()
        {
            var hub = (GamesHubService)Services.GetService(typeof(GamesHubService));
            if (hub != null) hub.Exit();
            else base.Exit();
        }
        #endregion
    }
}
