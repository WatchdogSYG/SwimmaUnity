using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Swimma.GameLibrary;
using Swimma.Screens;
using Swimma.Levels;
using Swimma.Collectables;

namespace Swimma
{
    class GemGenerator
    {
        #region Private Variables

        // list of gems
        private List<Gem> gems = new List<Gem>();
        private float GemSpawnProbability = 0.03f;
        private int gemSpeed = 2;

        private Random random = new Random(1236);
        private Game game;

        private Rectangle playArea;
        private bool final;


        #endregion

        #region Getters and Setters

        public List<Gem> GemList
        {
            get { return gems; }
            set { gems = value; }
        }
        #endregion



        public GemGenerator(Game game, Rectangle screenSize)
        {
            this.game = game;
            this.playArea = screenSize;
        }


        /// <summary>
        /// Bounces up and down in the air to entice players to collect them.
        /// </summary>
        public void Update(GameTime gameTime, float ElapsedTime, Level level)
        {
            var player = level.Player;
            if (ElapsedTime < level.LevelDuration)
            {
                // Spawn new passing gems at random
                if (random.NextDouble() < GemSpawnProbability)
                {
                    //the random variable
                    float spawnPoint = (float)random.NextDouble() * (playArea.Height - 50);
                    //make sure spawning inside bounds
                    if (spawnPoint < playArea.Top)
                    {
                        spawnPoint = playArea.Top + 50;
                    }

                    gems.Add(new Gem(game)
                    {
                        Position = new Vector2(playArea.Right + 100, spawnPoint),
                        Color = GenerateRandomColour(),
                        InitialPosition = new Vector2(playArea.Right + 100, spawnPoint)
                    });
                }
            }
            else
            {
                if (!final)
                    gems.Add(new Gem(game) { Position = new Vector2(playArea.Right - 100, playArea.Height - 100), Color = Color.Gold });
                final = true;

            }

            // Bounce control constants
            const float BounceHeight = 0.02f;
            const float BounceRate = 1.5f;
            const float BounceSync = -0.75f;


            //for each gem that was generated
            for (int i = 0; i < GemList.Count; i++)
            {
                if (!GemList[i].Name.Equals("KingGem"))
                {

                    // Bounce along a sine curve over time.
                    // Include the X coordinate so that neighboring gems bounce in a nice wave pattern.            
                    double t = gameTime.TotalGameTime.TotalSeconds * BounceRate + (GemList[i].InitialPosition.Y * BounceSync);
                    var bounce = (float)Math.Sin(t) * BounceHeight * GemList[i].Texture.Height;
                    // Animate this gem passing by
                    GemList[i].Position = new Vector2(GemList[i].Position.X - gemSpeed, GemList[i].Position.Y + bounce);

                    // Remove this gem if it has left the screen
                    if (GemList[i].Position.X < playArea.Left - 100)
                    {
                        GemList.RemoveAt(i);
                    }
                }
            }
        }

        /// <summary>
        /// Draws a gem in the appropriate color.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {

            // Draw the gems
            foreach (Gem blockPosition in GemList)
            {

                spriteBatch.Draw(blockPosition.Texture, blockPosition.Position, blockPosition.Color);
            }

        }


        /// <summary>
        /// generates a random color for the gem from the selected
        /// </summary>
        /// <returns></returns>
        private Color GenerateRandomColour()
        {
            int random = (new Random().Next(0, 5));
            switch (random)
            {
                case 1: return Color.Aquamarine;
                case 2: return Color.OrangeRed;
                case 3: return Color.WhiteSmoke;
                case 4: return Color.Yellow;
                case 5: return Color.SeaGreen;
                default: return Color.Orchid;
            }

        }

        internal void ResetGems()
        {
            gems.Clear();
            gemSpeed = 2;
        }
    }
}

