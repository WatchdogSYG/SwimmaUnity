using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Swimma.GameLibrary.BackgroundManager;
using System.Diagnostics;
using Swimma.GameLibrary;
using Swimma.Levels;
using REP_SLC;
using Common;
using Common.DataLogging;

namespace Swimma.Screens
{
    /// <summary>
    /// Class that manages the main gameplay
    /// </summary>
    class GamePlayScreen : MenuManager
    {
        #region Private Variables

        private Game game;

        private Label airLabel;
        private Label timeLabel;
        private Label scoreLabel;
        private Label livesLabel;
        private SpriteFont HudFont;
        private List<Label> list;

        private Rectangle screenSize;

        // Percentage of the screen on every side is the safe area
        private const float SafeAreaPortion = 0.05f;

        private AirBar airBar;
        private Texture2D heart;
        private Vector2 heartLocation;

        private static bool paused;
        private double deathDelay;
        private double lastBreathDelay;
        private Texture2D healthBar;
        private Rectangle titleSafeArea;
        private Vector2 hudLocation;
        private float timeHeight;
        private Texture2D map;

        private int levelIndex = 0;
        int totalScore;
        private const int initialLives = 5;
        private Level level;
        private int levelCounter = 1;
        private int maxEnMulpx;
        private float mulpx;
        private float playerMapPos;
        private Texture2D head;
        #endregion

        public static bool Paused
        {
            get { return paused; }
            set { paused = value; }
        }
        public int Score
        {
            get { return level.Player.Score; }
        }

        public void PauseGame(bool pause)
        {
            paused = pause;
        }

        public GamePlayScreen(Game game, GraphicsDeviceManager graphics, Rectangle screensize, EventHandler theMenuEvent)
            : base(theMenuEvent)
        {

            this.game = game;

            this.screenSize = screensize;


            heart = game.Content.Load<Texture2D>(@"Sprites/Heart");
            healthBar = game.Content.Load<Texture2D>(@"Sprites/HealthBar");
            map = game.Content.Load<Texture2D>(@"Images/Backgrounds/timeline");

            LoadNextLevel(initialLives);
            airBar = new AirBar(game, new Vector2(60, 8), healthBar);


            LoadHUD();

        }


        #region HUD Methods


        private void LoadHUD()
        {
            list = new List<Label>();
            titleSafeArea = screenSize;
            hudLocation = new Vector2(titleSafeArea.X + 10, titleSafeArea.Y + 20);
            Vector2 center = new Vector2(titleSafeArea.X + titleSafeArea.Width / 2.0f,
                                         titleSafeArea.Y + titleSafeArea.Height / 2.0f);

            HudFont = game.Content.Load<SpriteFont>(@"Fonts\Hud");
            timeLabel = new Label();
            timeLabel.Text = "Time: " + string.Format("{0:N2}", level.ElapsedTime / 1000);
            timeLabel.Name = "Time";
            timeLabel.Position = hudLocation + new Vector2(titleSafeArea.Width - timeLabel.Text.Length * HudFont.LineSpacing / 2, 0.0f); ;

            list.Add(timeLabel);

            timeHeight = HudFont.MeasureString(timeLabel.Text).Y;

            scoreLabel = new Label();
            scoreLabel.Text = "Score: " + level.Player.Score;
            scoreLabel.Name = "Score";
            scoreLabel.Position = hudLocation + new Vector2(titleSafeArea.Width - 15 - scoreLabel.Text.Length * HudFont.LineSpacing / 2, timeHeight * 1.2f);

            list.Add(scoreLabel);

            airLabel = new Label();
            airLabel.Text = "Air: ";
            airLabel.Name = "Air";
            airLabel.Position = hudLocation;

            list.Add(airLabel);

            livesLabel = new Label();
            livesLabel.Text = "Lives: ";
            livesLabel.Name = "Lives";
            livesLabel.Position = hudLocation + (new Vector2(0.0f, timeHeight * 1.2f));

            list.Add(livesLabel);


            foreach (Label label in list)
            {
                label.SpriteFont = HudFont;
                label.Size = label.SpriteFont.MeasureString(label.Text);
                label.Color = Color.White;
            }

            switch (Player.Avatar)
            {

                case PlayerAvatar.Boy:
                    head = game.Content.Load<Texture2D>(@"Images/Backgrounds/head");
                    break;
                case PlayerAvatar.Girl:
                    head = game.Content.Load<Texture2D>(@"Images/Backgrounds/headgirl");
                    break;
                default:
                    head = game.Content.Load<Texture2D>(@"Images/Backgrounds/head");
                    break;
            }
        }


        private void UpdateHUD(GameTime gt)
        {

            foreach (Label label in list)
            {
                if (label.Name.Equals("Time"))
                {
                    label.Text = "Time: " + string.Format("{0:N2}", level.ElapsedTime / 1000);
                    label.Position = hudLocation + new Vector2(titleSafeArea.Width - timeLabel.Text.Length * HudFont.LineSpacing / 2, 0.0f);
                }

                if (label.Name.Equals("Score"))
                {
                    label.Text = "Score: " + level.Player.Score;
                    // label.Position = hudLocation + new Vector2(timeLabel.Position.X, timeHeight * 1.2f);
                    label.Position = hudLocation + new Vector2(titleSafeArea.Width - 15 - scoreLabel.Text.Length * HudFont.LineSpacing / 2, timeHeight * 1.2f);

                }
            }

            if (level.DrawMap)
            {
                //update player position on map
                var mapPos = (map.Width) * (level.ElapsedTime / level.LevelDuration);
                if (level.StartBuffer)
                {
                    mapPos = (map.Width) * (0 / level.LevelDuration);
                }
                playerMapPos = MathHelper.Clamp(((screenSize.Width / 2) - (map.Width / 2)) + mapPos, (screenSize.Width / 2) - (map.Width / 2), ((screenSize.Width / 2) - (map.Width / 2)) + (map.Width - (head.Width / 2)));
            }
        }

        private void DrawHUD(SpriteBatch spriteBatch)
        {
            airBar.Draw(spriteBatch, level);

            heartLocation = new Vector2(85, 40);
            foreach (Label label in list)
            {
                label.Draw(spriteBatch);
            }

            for (int i = 0; i < level.Player.LivesLeft; i++)
            {
                spriteBatch.Draw(heart, new Rectangle((int)heartLocation.X, (int)heartLocation.Y, 30, 30), Color.White);
                heartLocation.X += 30;
            }

            if (level.DrawMap)
            {
                //draw the map          
                spriteBatch.Draw(map, new Rectangle(screenSize.Width / 2 - map.Width / 2, screenSize.Height - map.Height, map.Width, map.Height), Color.White);

                spriteBatch.Draw(head, new Vector2(playerMapPos, screenSize.Height - 50), Color.White);
            }
        }
        #endregion
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            //spriteBatch.Begin();
             spriteBatch.Begin(SpriteSortMode.BackToFront, null, null, null, null, null, Matrix.CreateScale(game.GraphicsDevice.Viewport.Width / 1280f, game.GraphicsDevice.Viewport.Height / 720f, 0));
            //spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            level.Draw(spriteBatch, gameTime);

            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Matrix.CreateScale(game.GraphicsDevice.Viewport.Width / 1280f, game.GraphicsDevice.Viewport.Height / 720f, 0));

            //  spriteBatch.Begin();

            DrawHUD(spriteBatch);

            spriteBatch.End();

            base.Draw(spriteBatch, gameTime);
        }



        public override void Update(GameTime gameTime)
        {
            if (!paused)
            {
                 Debug();
                level.Update(gameTime);
                //background.Update(gameTime)
                if (!level.StartBuffer)
                {
                    airBar.Update(gameTime, level);
                    UpdateHUD(gameTime);
                    HandleScoring(gameTime);
                    DyingBreathPulse(gameTime);
                    //check if player reached end of level
                    if (level.ReachedExit)
                    {
                        LoadNextLevel(level.Player.LivesLeft);
                    }
                }

                if (level != null && level.Player != null) {
                    var logger = (LoggingService)Game1.Game.Services.GetService(typeof(LoggingService));
                    if (logger != null) logger.UpdateData(LogChannel.ScoreChannel, level.Player.Score);
                }
            }
            CheckPausedState();
            base.Update(gameTime);
        }

        private void Debug()
        {
            if (InputHandler.KeyPressed(Keys.D1))
            {
                level.ElapsedTime = 50000;
            }
            if (InputHandler.KeyPressed(Keys.D2))
            {
                level.ElapsedTime = 100000;
            }
            if (InputHandler.KeyPressed(Keys.D3))
            {
                level.ElapsedTime = 150000;
            }
            if (InputHandler.KeyPressed(Keys.D4))
            {
                level.ElapsedTime = 200000;
            }
            if (InputHandler.KeyPressed(Keys.D5))
            {
                level.ReachedExit = true;
            }
            if (InputHandler.KeyPressed(Keys.D6))
            {
                level.GemsCollected = 100;
            }
        }
        private void LoadNextLevel(int currentLives)
        {
            maxEnMulpx = 0;
            mulpx = 1;

            if (level != null)
            {
                airBar.ResetHealthBar(level);
                totalScore = level.Player.Score;
                level.Dispose();
                //try { Level2.bgWorker.Abort(); }
                //catch { }
                level.ElapsedTime = 0;

                UpdateHUD(new GameTime());

                levelIndex++;

                if (levelIndex > 3)
                {
                    maxEnMulpx++;
                    mulpx += .5f;
                    levelCounter++;
                    levelIndex = levelIndex % 4;
                }

            }
            switch (levelIndex)
            {
                case 0:
                    level = new Level1(game, screenSize, totalScore, currentLives, mulpx, maxEnMulpx);
                    break;
                case 1:
                    levelCounter++;
                    level = new Level2(game, screenSize, totalScore, currentLives, mulpx, maxEnMulpx);
                    break;
                case 2:
                    levelCounter++;
                    level = new Level3(game, screenSize, totalScore, currentLives, mulpx, maxEnMulpx);
                    break;
                case 3:
                    levelCounter++;
                    level = new Level4(game, screenSize, totalScore, currentLives, mulpx, maxEnMulpx);
                    break;
                default:
                    level = new Level1(game, screenSize, totalScore, currentLives);
                    levelIndex = -1;
                    break;

            }
        }


        private void CheckPausedState()
        {
            //if ((InputHandler.KeyPressed(Keys.Space) || InputHandler.KeyPressed(Keys.Escape) || InputHandler.ButtonPressed(Buttons.Start)) && !paused || InputHandler.ButtonPressed(Buttons.Back))
            if(!paused) {
                if (GameInput.GetButtonDown()) {
                    paused = true;
                    menuEvent.Invoke("Pause", new EventArgs());
                }
            }
        }

     private void DyingBreathPulse(GameTime gameTime)
        {
            if (level.Player.LivesLeft == 1)
            {
                lastBreathDelay += gameTime.ElapsedGameTime.Milliseconds;
                if (!RumbleTimer.IsReady) GamePad.SetVibration(PlayerIndex.One, 0, 0);
               
                //pulse every one second
                if (lastBreathDelay > 1000)
                {
                    if (!level.Player.IsVibrating) { RumbleTimer.AddVibration(.3f,.3f, .1f); level.Player.IsVibrating = true; }
                    if (!RumbleTimer.IsReady) GamePad.SetVibration(PlayerIndex.One, .3f, .3f);
                    if (lastBreathDelay > 1100)
                    {
                        lastBreathDelay = 0; 
                        level.Player.IsVibrating = false;
                    }
                }
            }
        }

        private void HandleScoring(GameTime gameTime)
        {
            //check air bar
            if (level.Player.AirLeft <= 0)
            {
                airBar.ResetHealthBar(level);
                level.Player.LivesLeft -= 1;
                level.Player.IsHit = true;
                if (level.Player.LivesLeft == 0)
                {
                    SoundLibrary.PlayerDie.Play();
                }
            }

            if (level.Player.LivesLeft <= 0)
            {
                deathDelay += gameTime.ElapsedGameTime.Milliseconds;
                //wait one second before gameoverscreen
                if (deathDelay > 1000)
                {
                    SoundLibrary.GameOver.Play();
                    if (!RumbleTimer.IsReady) GamePad.SetVibration(PlayerIndex.One, 0, 0);
                    level.Player.IsVibrating = false;
                    //GameOver, load the Game over Screen
                    menuEvent.Invoke("GameOver", new EventArgs());

                }
            }
        }


        public void ResetGame()
        {
            levelIndex = -1;
            level.Dispose();
            level.Player.Score = 0;
            level.Player.LivesLeft = 0;
            level.ElapsedTime = 0;
            level.GemsCollected = 0;
            levelCounter = 1;
            //try { Level2.bgWorker.Abort(); }
            //catch { }
            LoadNextLevel(initialLives);
            UpdateHUD(new GameTime());
        }
    }
}
