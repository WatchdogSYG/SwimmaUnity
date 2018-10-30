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

namespace Swimma.Levels
{
    class Level3 : Level
    {
        private float EnemySpawnProbability = .995f;
        private Random random = new Random();
        private const int PointsPerSecond = 5;
        private List<Enemies.Enemy> EnemyList = new List<Enemies.Enemy>();
        private Game game;
        private bool stopDrawing;
        private int maxEnemies = 6;
        private float speedMultx = 1;

        public Level3(Game game, Rectangle screenSize, int startingScore, int currentLives, float enemySpeedMultx = 1, int maxEnemy = 0)
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
            Gem = new GemGenerator(game, PlayableArea);
            AirBubble = new BubbleGenerator(game, PlayableArea);

            LevelDuration = 150000;
            Player.LivesLeft = currentLives;
            DrawMap = true;

            LoadBackgorund(game.GraphicsDevice);

        }
        private void LoadBackgorund(GraphicsDevice graphics)
        {
            Background = new ParallaxBG(graphics);
            Texture2D back;

            back = Content.Load<Texture2D>("Images/Backgrounds/seagreen");
            Background.AddLayer(back, Vector2.Zero, 1f, -20f);
            back = Content.Load<Texture2D>("Images/Backgrounds/whale1");
            Background.AddLayer(back, Vector2.Zero, .7f, -50f);
            back = Content.Load<Texture2D>("Images/Backgrounds/whale2");
            Background.AddLayer(back, Vector2.Zero, .2f, -70f);
        }

        public override void Update(GameTime gameTime)
        {
            ElapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            if (!StartBuffer)
            {
                if (!stopDrawing)
                    Background.Update(gameTime);
                Player.Update(gameTime);
                Gem.Update(gameTime, ElapsedTime, this);
                AirBubble.Update(gameTime);

                GenerateEnemies();
                UpdateEnemy(gameTime, Player);
                HandleCollision();
            }
        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Background.Draw(spriteBatch);
            Player.Draw(spriteBatch, gameTime);
            if (!DrawFirstLoading(spriteBatch, gameTime))
            {
                Gem.Draw(spriteBatch);
                AirBubble.Draw(spriteBatch);

                DrawEnemies(spriteBatch, gameTime);
            }

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
            if (ElapsedTime < 60000)
            {
                // Spawn new passing enemys at random

                if (random.NextDouble() > EnemySpawnProbability)
                {

                    //the random variable for spawning
                    float spawnPoint = (float)random.NextDouble() * (PlayableArea.Width - 50);
                    //no more than 6 enemies at a time
                    if (EnemyList.Count < maxEnemies)
                    {
                        EnemyList.Add(new JellyFish(game) { Position = new Vector2(spawnPoint, PlayableArea.Height + 100) });
                        EnemyList[EnemyList.Count - 1].Speed = (int)(EnemyList[EnemyList.Count - 1].Speed * speedMultx);

                    }

                }
            }

            if (ElapsedTime > 55000 && ElapsedTime < 100000)
            {
                // Spawn new passing Stingrays at random
                if (random.NextDouble() > EnemySpawnProbability)
                {
                    //the random variable for spawning
                    float spawnPoint = (float)random.NextDouble() * (PlayableArea.Height - 50);
                    //no more than 6 enemies at a time
                    if (EnemyList.Count < maxEnemies)
                    {
                        EnemyList.Add(new StingRay(game) { Position = new Vector2(PlayableArea.Width + 100, spawnPoint) });
                        EnemyList[EnemyList.Count - 1].Speed = (int)(EnemyList[EnemyList.Count - 1].Speed * speedMultx);

                    }
                }
            }

            if (ElapsedTime > 95000 && ElapsedTime < LevelDuration)
            {

                // Spawn new passing Stingrays at random
                if (random.NextDouble() > EnemySpawnProbability)
                {
                    //the random variable for spawning
                    float spawnPoint = (float)random.NextDouble() * (PlayableArea.Height - 50);
                    //no more than 6 enemies at a time
                    if (EnemyList.Count < maxEnemies)
                    {
                        EnemyList.Add(new PufferFish(game) { Position = new Vector2(-100, spawnPoint) });
                        EnemyList[EnemyList.Count - 1].Speed = (int)(EnemyList[EnemyList.Count - 1].Speed * speedMultx);
                    }
                }
            }
            //boss....
            if (ElapsedTime > LevelDuration)
            {
                //no more than 1 enemies at a time
                if (!ExitSpawned)
                {
                    var texture = Content.Load<Texture2D>(@"Images/Backgrounds/boatEntrance");

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

        }

        private void UpdateEnemy(GameTime gameTime, Player player)
        {
            for (int i = 0; i < EnemyList.Count; i++)
            {
                EnemyList[i].Update(gameTime, player);

                if (EnemyList[i] is PufferFish)
                {
                    // Remove this enemy if it has left the screen
                    if (EnemyList[i].Position.X > PlayableArea.Right)
                    {
                        EnemyList.RemoveAt(i);
                    }
                }
                else if (EnemyList[i] is JellyFish)
                {

                    if (EnemyList[i].Position.X + EnemyList[i].FrameWidth < 0 ||
                        EnemyList[i].Position.Y + EnemyList[i].Texture.Height < 0)
                    {
                        EnemyList.RemoveAt(i);
                    }


                }
                else if (EnemyList[i] is Exit)
                {


                    if (EnemyList[i].Position.X <= Viewport.Right - EnemyList[i].FrameWidth)
                    {
                        //EnemyList.RemoveAt(i);
                        EnemyList[i].Speed = 0;
                        stopDrawing = true;
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

            }
        }
        #endregion

        #region Collision Detection

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
                    // Check collision with person and enemy
                    if (Player.BoundingRectangle.Intersects(EnemyList[i].BoundingRectangle))
                    {
                        if (EnemyList[i] is Exit)
                        {
                            EnemyList[i].OnCollision(this, new Rectangle(1150, 430, 100, 200));
                        }
                        else
                        {
                            if (PixelCollision(Player.Texture, EnemyList[i].Texture, Player.FrameID,
                               EnemyList[i].FrameID, Player.BoundingRectangle, EnemyList[i].BoundingRectangle))
                            {

                                EnemyList[i].OnCollision(this);

                            }
                        }
                    }
                }

            }
        }
        #endregion

    }
}
