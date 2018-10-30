using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Swimma.GameLibrary.BackgroundManager;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Swimma.Enemies;
using Swimma.Collectables;

namespace Swimma.Levels
{
    class Level1 : Level
    {
        private float EnemySpawnProbability = .995f;
        private Random random = new Random();
        private List<Enemy> EnemyList = new List<Enemy>();
        private List<OneUp> BonusLifeList = new List<OneUp>();
        private Game game;
        private bool stopDrawing;
        private int maxEnemies = 3;
        private float speedMultx = 1;
        

        public Level1(Game game, Rectangle screenSize, int startingScore, int currentLives, float enemySpeedMultx = 1, int maxEnemy = 0)
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

            LevelDuration = 100000;
            Player.LivesLeft = currentLives;

            LoadBackgorund(game.GraphicsDevice);
            DrawMap = true;
        }
        private void LoadBackgorund(GraphicsDevice graphics)
        {
            Background = new ParallaxBG(graphics);
            Texture2D back;

            back = Content.Load<Texture2D>("Images/Backgrounds/seaBlue");
            Background.AddLayer(back, Vector2.Zero, 1f, -20f);

        }

        public override void Update(GameTime gameTime)
        {
            ElapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            if (!StartBuffer)
            {

                Player.Update(gameTime);
                if (!stopDrawing)
                    Background.Update(gameTime);
                Gem.Update(gameTime, ElapsedTime, this);

                AirBubble.Update(gameTime);

                GenerateEnemies();
                UpdateEnemy(gameTime, Player);
                UpdateBonusLives(gameTime);

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
                DrawBonusLives(spriteBatch);
            }
            Player.Draw(spriteBatch, gameTime);
        }

        private void DrawBonusLives(SpriteBatch spriteBatch)
        {
            foreach (OneUp life in BonusLifeList)
            {
                life.Draw(spriteBatch);
            }
        }

        private void UpdateBonusLives(GameTime gameTime)
        {
            foreach (OneUp life in BonusLifeList)
            {
                life.Update(gameTime);
            }
        }


        #region Enemy Methods

        private void DrawEnemies(SpriteBatch spriteBatch, GameTime gameTime)
        {

            foreach (Enemy enemy in EnemyList)
            {
                enemy.Draw(spriteBatch, gameTime);
            }

        }

        private void GenerateEnemies()
        {
            // elapsedTime+= gameTime.ElapsedGameTime.Milliseconds;
            if (ElapsedTime < 50000)
            {
                // Spawn new passing enemys at random

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

            if (ElapsedTime > 50000 && ElapsedTime < LevelDuration)
            {

                // Spawn new passing Stingrays at random
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
            if (GemsCollected % 100 == 0 && GemsCollected > 0)
            {

                BonusLifeList.Add(new OneUp(game));
                GemsCollected++;
            }

            //boss....
            if (ElapsedTime > LevelDuration)
            {
                //no more than 1 enemies at a time
                if (!ExitSpawned)
                {
                    var texture = Content.Load<Texture2D>(@"Images/Backgrounds/caveentrance");

                    EnemyList.Add(new Exit(game)
                    {
                        Position = new Vector2(PlayableArea.Width + 100, 0),

                        Texture = texture,
                        FrameWidth = texture.Width,
                        FrameHeight = texture.Height,
                        FrameCount = 1
                    });
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
                else { }

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
                    // Check collision with person and bubble
                    if (Player.BoundingRectangle.Intersects(EnemyList[i].BoundingRectangle))
                    {
                        if (EnemyList[i] is Exit)
                        {
                            if (EnemyList[i].Speed != 0)
                            {
                                Player.Position += Player.MoveSpeed * new Vector2(-1,0);
                                return;
                            }

                                EnemyList[i].OnCollision(this, new Rectangle(1240, 170, 235, 45));
                        }

                        if (PixelCollision(Player.Texture, EnemyList[i].Texture, Player.FrameID,
                           EnemyList[i].FrameID, Player.BoundingRectangle, EnemyList[i].BoundingRectangle))
                        {
                            if (EnemyList[i] is Exit)
                            {
                                Player.Position += Player.MoveSpeed * new Vector2(-1, -1);

                            }
                            else
                            {
                                EnemyList[i].OnCollision(this);
                            }

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

    }
}
