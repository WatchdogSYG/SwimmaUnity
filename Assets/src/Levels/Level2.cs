using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Swimma.GameLibrary.BackgroundManager;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Swimma.Enemies;
using System.Threading;
using Swimma.Screens;
using Swimma.GameLibrary;
using Swimma.Collectables;

namespace Swimma.Levels
{
    public class Level2 : Level
    {
        private float EnemySpawnProbability = .993f;
        private Random random = new Random();
        private const int PointsPerSecond = 5;
        private List<Enemies.Enemy> EnemyList = new List<Enemies.Enemy>();
        private List<OneUp> BonusLifeList = new List<OneUp>();
        private Game game;
        private Thread bgWorker;
        private Texture2D topBG;
        private Texture2D botBG;
        private bool StopDrawing;
        private int maxEnemies = 4;
        private float speedMultx = 1;

        public Level2(Game game, Rectangle screenSize, int startingScore, int currentLives, float enemySpeedMultx = 1, int maxEnemy = 0)
        {

            Viewport = screenSize;
            PlayableArea = SetSafePlayArea(screenSize);
            this.game = game;
            maxEnemies += maxEnemy;
            speedMultx = enemySpeedMultx;

            // Create a new content manager to load content used just by this level.
            Content = new ContentManager(game.Services);
            Content.RootDirectory = "Content/Swimma";
            //load the level variables
            Player = new Player(game, PlayableArea, 500, new Vector2(0, Viewport.Height / 2));
            Player.Score = startingScore;

            topBG = Content.Load<Texture2D>(@"Images/Backgrounds/caveTop");
            botBG = Content.Load<Texture2D>(@"Images/Backgrounds/caveBot");

            //set bounds for enemies and collectables
            PlayableArea = new Rectangle(PlayableArea.X, topBG.Height, PlayableArea.Width, Viewport.Height - botBG.Height);

            Gem = new GemGenerator(game, PlayableArea);
            AirBubble = new BubbleGenerator(game, PlayableArea);
            LevelDuration = 100000;
            Player.LivesLeft = currentLives;
            LoadBackgorund(game.GraphicsDevice);
            DrawMap = true;
            //bgWorker = new Thread(HandleBGCollision);
            //bgWorker.Start();

        }

        private void LoadBackgorund(GraphicsDevice graphics)
        {
            Background = new ParallaxBG(graphics);
            Texture2D back;

            back = Content.Load<Texture2D>("Images/Backgrounds/caveinside1");
            Background.AddLayer(back, Vector2.Zero, 1f, -20f);
            back = Content.Load<Texture2D>("Images/Backgrounds/caveinsidefront1280x720");
            Background.AddLayer(back, Vector2.Zero, 0f, -50f);

        }

        public override void Update(GameTime gameTime)
        {
            ElapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            if (!StartBuffer)
            {

                Player.Update(gameTime);

                AirBubble.Update(gameTime);

                GenerateEnemies();
                UpdateEnemy(gameTime, Player);
                if (!StopDrawing)
                    Background.Update(gameTime);
                Gem.Update(gameTime, ElapsedTime, this);

                HandleCollision();
            }
        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            Background.Draw(spriteBatch);

            if (!DrawFirstLoading(spriteBatch, gameTime) || !ExitSpawned)
            {
                Gem.Draw(spriteBatch);
                AirBubble.Draw(spriteBatch);
                DrawEnemies(spriteBatch, gameTime);
            }
            Player.Draw(spriteBatch, gameTime);
        }

        public override void Dispose()
        {
            //  if (bgWorker.IsAlive) bgWorker.Abort();
            base.Dispose();
        }
        #region Enemy Methods

        private void DrawEnemies(SpriteBatch spriteBatch, GameTime gameTime)
        {

            foreach (Enemies.Enemy enemy in EnemyList)
            {
                enemy.Draw(spriteBatch, gameTime);
            }

        }

        private void GenerateEnemies()
        {
            // elapsedTime+= gameTime.ElapsedGameTime.Milliseconds;
            if (ElapsedTime < 25000)
            {


                // Spawn new passing Stingrays at random
                if (random.NextDouble() > EnemySpawnProbability)
                {
                    //the random variable for spawning
                    float spawnPoint = PlayableArea.Top + (float)random.NextDouble() * (PlayableArea.Height);
                    //no more than 6 enemies at a time
                    if (EnemyList.Count < maxEnemies)
                    {
                        EnemyList.Add(new StingRay(game) { Position = new Vector2(PlayableArea.Right + 100, spawnPoint) });
                        EnemyList[EnemyList.Count - 1].Speed = (int)(EnemyList[EnemyList.Count - 1].Speed * speedMultx);

                    }
                }

            }

            if (ElapsedTime > 50000 && ElapsedTime < 75000)
            {

                // Spawn new passing enemys at random

                if (random.NextDouble() > EnemySpawnProbability)
                {
                    //the random variable for spawning
                    float spawnPoint = PlayableArea.Top + (float)random.NextDouble() * (PlayableArea.Height);
                    //no more than 6 enemies at a time
                    if (EnemyList.Count < maxEnemies)
                    {
                        EnemyList.Add(new Turtle(game) { Position = new Vector2(PlayableArea.Right + 100, spawnPoint) });
                        EnemyList[EnemyList.Count - 1].Speed = (int)(EnemyList[EnemyList.Count - 1].Speed * speedMultx);

                    }
                }
            }

            if (ElapsedTime > 75000 && ElapsedTime < LevelDuration)
            {

                // Spawn new passing Stingrays at random
                if (random.NextDouble() > EnemySpawnProbability)
                {
                    //the random variable for spawning
                    float spawnPoint = PlayableArea.Top + (float)random.NextDouble() * (PlayableArea.Height - 50);
                    //no more than 6 enemies at a time
                    if (EnemyList.Count < 6)
                    {
                        EnemyList.Add(new PufferFish(game) { Position = new Vector2(-100, spawnPoint) });
                        EnemyList[EnemyList.Count - 1].Speed = (int)(EnemyList[EnemyList.Count - 1].Speed * speedMultx);

                    }
                }
            }

            if (ElapsedTime > LevelDuration)
            {
                //spawn exit
                if (!ExitSpawned)
                {
                    var texture = Content.Load<Texture2D>(@"Images/Backgrounds/caveexit");

                    EnemyList.Add(new Exit(game)
                    {
                        Position = new Vector2(PlayableArea.Width + 100, 0),

                        Texture = texture,
                        FrameWidth = texture.Width,
                        FrameHeight = texture.Height,
                        FrameCount = 1
                    });

                    ExitSpawned = true;
                }
            }

            if (GemsCollected % 100 == 0 && GemsCollected > 0)
            {

                BonusLifeList.Add(new OneUp(game));
                GemsCollected++;
            }

        }

        private void UpdateEnemy(GameTime gameTime, Player player)
        {
            for (int i = 0; i < EnemyList.Count; i++)
            {
                EnemyList[i].Update(gameTime, player);

                if (EnemyList[i] is PufferFish)
                {
                    // Remove this enemy if it has left the screen
                    if (EnemyList[i].Position.X > Viewport.Right)
                    {
                        EnemyList.RemoveAt(i);
                    }
                }
                else if (EnemyList[i] is Turtle)
                {

                    if (EnemyList[i].Position.X + EnemyList[i].FrameWidth < Viewport.Left)
                    {
                        EnemyList.RemoveAt(i);
                    }


                }
                else if (EnemyList[i] is Exit)
                {

                    if (EnemyList[i].Position.X <= Viewport.Right - EnemyList[i].Texture.Width)
                    {
                        //EnemyList.RemoveAt(i);
                        EnemyList[i].Speed = 0;
                        StopDrawing = true;
                        // ReachedExit = true;
                        return;
                    }


                }
                else if (EnemyList[i] is StingRay)
                {
                    if (EnemyList[i].Position.X + EnemyList[i].FrameWidth < 0)
                    {
                        EnemyList.RemoveAt(i);
                    }
                }
                else { }

            }
        }
        #endregion

        #region Collision Detection
        #region Background Collision
        //private void HandleBGCollision()
        //{
        //    while (true)
        //    {

        //        if (!GamePlayScreen.Paused)
        //        {
        //            if (!StartBuffer)
        //            {
        //                if (!Player.IsHit)
        //                {
        //                    var textureBounds = new Rectangle();
        //                    var topBounds = new Rectangle();
        //                    var botBounds = new Rectangle();

        //                    if (Player.BoundingRectangle.X + Player.BoundingRectangle.Width > (int)(Background.GetLayerAt(1).Position.X + Background.GetLayerAt(1).BgTexture.Width))
        //                    {
        //                        textureBounds = new Rectangle((int)Background.GetLayerAt(1).Position.X + (int)Background.GetLayerAt(1).BgTexture.Width, 0,
        //                            (int)Background.GetLayerAt(1).BgTexture.Width, (int)Background.GetLayerAt(1).BgTexture.Height);


        //                        topBounds = new Rectangle(textureBounds.X, 0, topBG.Width, topBG.Height);
        //                        botBounds = new Rectangle(textureBounds.X, Viewport.Height - botBG.Height, botBG.Width, botBG.Height);

        //                    }
        //                    else
        //                    {
        //                        textureBounds = new Rectangle((int)Background.GetLayerAt(1).Position.X, 0,
        //                            (int)Background.GetLayerAt(1).BgTexture.Width, (int)Background.GetLayerAt(1).BgTexture.Height);
        //                        topBounds = new Rectangle(textureBounds.X, 0, topBG.Width, topBG.Height);
        //                        botBounds = new Rectangle(textureBounds.X, Viewport.Height - botBG.Height, botBG.Width, botBG.Height);

        //                    }

        //                    if (Player.BoundingRectangle.Intersects(topBounds))
        //                    {
        //                        if (PixelCollision(Player.Texture, Background.GetLayerAt(1).BgTexture, Player.FrameID,
        //                                               0, Player.BoundingRectangle, topBounds))
        //                        {
        //                            Player.IsHit = true;
        //                            Player.AirLeft -= 30;
        //                            Player.Position = new Vector2(Player.Position.X, Player.Position.Y + 5);
        //                            SoundLibrary.EnemyHit.Play();
        //                        }
        //                    }
        //                    if (Player.BoundingRectangle.Intersects(botBounds))
        //                    {
        //                        if (PixelCollision(Player.Texture, Background.GetLayerAt(1).BgTexture, Player.FrameID,
        //                                                  0, Player.BoundingRectangle, botBounds))
        //                        {
        //                            Player.IsHit = true;
        //                            Player.AirLeft -= 30;
        //                            Player.Position = new Vector2(Player.Position.X, Player.Position.Y - 5);
        //                            SoundLibrary.EnemyHit.Play();
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        #endregion
        #region Game object Collsions
        private void HandleCollision()
        {
            //check for collions only if plyaer is not taking damage
            if (!Player.IsHit)
            {
                //check for gem collisions

                for (int i = 0; i < Gem.GemList.Count; i++)
                {
                    // Check collision with person and gem
                    if (Player.BoundingRectangle.Intersects(Gem.GemList[i].BoundingRectangle()))
                    {
                        Gem.GemList[i].OnCollected(this, Gem.GemList[i].Color);
                        Gem.GemList.RemoveAt(i);

                    }
                }

                //check for bubble collisions
                for (int i = 0; i < AirBubble.BubbleList.Count; i++)
                {
                    // Check collision with person and bubble
                    if (Player.BoundingRectangle.Intersects(AirBubble.BubbleList[i].BoundingRectangle))
                    {
                        AirBubble.BubbleList[i].OnCollision(Player);
                        AirBubble.BubbleList.RemoveAt(i);
                    }
                }
                //check for enemy collisions
                for (int i = 0; i < EnemyList.Count; i++)
                {
                    // Check collision with person and bubble
                    if (Player.BoundingRectangle.Intersects(EnemyList[i].BoundingRectangle))
                    {
                        if (EnemyList[i] is Exit)
                        {

                            EnemyList[i].OnCollision(this, new Rectangle(PlayableArea.Right, 100, 100, 550));
                        }
                        else
                        {
                            EnemyList[i].OnCollision(this);
                        }
                    }
                }

                //check for bonuslife collisions
                for (int i = 0; i < BonusLifeList.Count; i++)
                {
                    // Check collision with person and bubble
                    if (Player.BoundingRectangle.Intersects(BonusLifeList[i].BoundingRectangle))
                    {
                        BonusLifeList[i].OnCollision(this);
                        BonusLifeList.RemoveAt(i);
                    }
                }

            }
        }
        #endregion
        #endregion

    }
}
