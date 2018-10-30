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
    class Level4 : Level
    {
        private float EnemySpawnProbability = .995f;
        private Random random = new Random(4558);
        private const int PointsPerSecond = 5;
        private List<Enemies.Enemy> EnemyList = new List<Enemies.Enemy>();
        private Game game;
        private bool stopDrawing;
        private int maxEnemies = 7;
        private float speedMultx = 1;

        public Level4(Game game, Rectangle screenSize, int startingScore, int currentLives, float enemySpeedMultx = 1, int maxEnemy = 0)
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
            Gem = new GemGenerator(game, new Rectangle(PlayableArea.X, 240, PlayableArea.Width, 500));
            AirBubble = new BubbleGenerator(game, new Rectangle(PlayableArea.X, 240, PlayableArea.Width, 200));

            LevelDuration = 100000;
            Player.LivesLeft = currentLives;
            DrawMap = true;
            LoadBackgorund(game.GraphicsDevice);

            LoadStaticEnemies();
        }

        private void LoadStaticEnemies()
        {
            for (int i = 0; i < 4; i++)
            {
                //the random variable for spawning
                float spawnPoint = (float)random.NextDouble() * (PlayableArea.Width - 50);
                //no more than 6 enemies at a time
                if (EnemyList.Count < maxEnemies)
                {
                    EnemyList.Add(new JellyFish(game) { Position = new Vector2(spawnPoint, PlayableArea.Height - (100*(i*3))) });
                    EnemyList[EnemyList.Count - 1].Speed = (int)(EnemyList[EnemyList.Count - 1].Speed * speedMultx);

                }
            }
        }
        private void LoadBackgorund(GraphicsDevice graphics)
        {
            Background = new ParallaxBG(graphics);
            Texture2D back;

            back = Content.Load<Texture2D>("Images/Backgrounds/seaBlue");
            Background.AddLayer(back, Vector2.Zero, 1f, -70f);

            back = Content.Load<Texture2D>("Images/Backgrounds/boatInsidegreen2");
            Background.AddLayer(back, Vector2.Zero, .8f, 0f);
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
           

            if (ElapsedTime < 40000)
            {
                // Spawn new passing Stingrays at random
                if (random.NextDouble() > EnemySpawnProbability)
                {
                    //the random variable for spawning
                    float spawnPoint = (float)random.NextDouble() * (200);
                    if (spawnPoint < 240)
                    {
                        spawnPoint = 240 + 50;
                    }
                    //no more than 6 enemies at a time
                    if (EnemyList.Count < maxEnemies)
                    {
                        EnemyList.Add(new StingRay(game) { Position = new Vector2(PlayableArea.Width + 100, spawnPoint) });
                        EnemyList[EnemyList.Count - 1].Speed = (int)(EnemyList[EnemyList.Count - 1].Speed * speedMultx);

                    }
                }
            }

            if (ElapsedTime > 40000 && ElapsedTime < 60000)
            {

                // Spawn new passing Stingrays at random
                if (random.NextDouble() > EnemySpawnProbability)
                {
                    //the random variable for spawning
                    float spawnPoint = (float)random.NextDouble() * (200);
                    if (spawnPoint < 240)
                    {
                        spawnPoint = 240 + 50;
                    }
                    //no more than 6 enemies at a time
                    if (EnemyList.Count < maxEnemies)
                    {
                        EnemyList.Add(new PufferFish(game) { Position = new Vector2(-100, spawnPoint) });
                        EnemyList[EnemyList.Count - 1].Speed = (int)(EnemyList[EnemyList.Count - 1].Speed * speedMultx);
                    }
                }
            }
            if (ElapsedTime > 60000 && ElapsedTime < LevelDuration)
            {

                // Spawn new passing Stingrays at random
                if (random.NextDouble() > EnemySpawnProbability)
                {
                    //the random variable for spawning
                    float spawnPoint = (float)random.NextDouble() * (200);
                    if (spawnPoint < 240)
                    {
                        spawnPoint = 240 + 50;
                    }
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
                   
                    Player.SafeBounds = new Rectangle(PlayableArea.X, PlayableArea.Y, Viewport.Width - PlayableArea.X, PlayableArea.Height);
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

                    if (EnemyList[i].Position.Y < 50)
                    {
                        EnemyList[i].Speed = (int)(-2 * speedMultx);
                    }
                    else if (EnemyList[i].Position.Y + EnemyList[i].Texture.Height > PlayableArea.Bottom - 50)
                    {
                        EnemyList[i].Speed = (int)(2 * speedMultx);
                    }
                    //stop jellyfish from moving backwards like normal
                    EnemyList[i].Position += new Vector2(1, 0);

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

                if (ExitSpawned)
                {
                    if (Player.BoundingRectangle.Intersects(new Rectangle(1240, 220, 100, 220))) { ReachedExit = true; }
                }
                //check for enemy collisions
                for (int i = 0; i < EnemyList.Count; i++)
                {
                    // Check collision with person and enemy
                    if (Player.BoundingRectangle.Intersects(EnemyList[i].BoundingRectangle))
                    {
                        if (EnemyList[i] is Exit)
                        {
                            EnemyList[i].OnCollision(this, new Rectangle(1240, 220, 100, 220));
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
