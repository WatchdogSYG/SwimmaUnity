using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Swimma.GameLibrary;
using System.Diagnostics;
using Swimma.Enemies;
using Swimma.Screens;
using Swimma.Collectables;

namespace Swimma
{
   public  class BubbleGenerator
    {
        #region Private Variables

        // list of enemys
        private List<Bubble> bubbleList = new List<Bubble>();

        public List<Bubble> BubbleList
        {
            get { return bubbleList; }
            set { bubbleList = value; }
        }

        private float BubbleSpawnProbability = 0.001f;
        private Game game;
        private Random random = new Random();
        private Texture2D bubbleTexture;
        private bool spawned = false;
        private int spawnedTime;
        private Rectangle playArea;
        private float speed;
        #endregion



        public BubbleGenerator(Game game, Rectangle playArea, float spawnRate, float speed)
        {
            this.game = game;
            this.playArea = playArea;
            this.BubbleSpawnProbability = spawnRate;
            this.speed = speed;

        }
        public BubbleGenerator(Game game, Rectangle playArea)
        {
            this.game = game;
            this.playArea = playArea;
            this.speed = 3;
            LoadContent();
        }
        public void LoadContent()
        {
        }


        public void Update(GameTime gameTime)
        {


            int refillTime = gameTime.TotalGameTime.Seconds;

            //check if set bubble has been drawn if so dont draw again until it is time
            if (spawned && spawnedTime + 1 == refillTime)
            {
                spawned = false;
            }

            // Spawn new passing bubbles at random or at the set time
            if (random.NextDouble() < BubbleSpawnProbability || (refillTime % 10 == 0 && !spawned))
            {
                spawned = true;
                spawnedTime = (int)refillTime;

                float spawnPoint = (float)random.NextDouble() * (playArea.Height - 100);
                bubbleList.Add(new Bubble(game) { Position = new Vector2(playArea.Width + 100, spawnPoint), Speed = speed });

            }


            //for each bubble that was generated
            for (int i = 0; i < BubbleList.Count; i++)
            {

                BubbleList[i].Update(gameTime);


                // Remove this bubble if it has left the screen
                if (BubbleList[i].Position.X < playArea.Left - 100)
                {
                    BubbleList.RemoveAt(i);
                }
            }

        }

        internal void ResetBubbles()
        {
            BubbleList.Clear();
        }


        /// <summary>
        /// Draws a enemy
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {

            foreach (Bubble bubble in BubbleList)
            {
                bubble.Draw(spriteBatch);
            }

        }
        /// <summary>
        /// resets the enemy states to default
        /// </summary>
        public void ResetEnemy()
        {
            BubbleList.Clear();
        }
    }
}

