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
using System.Diagnostics;
using Swimma.GameLibrary;
using Swimma.Levels;


namespace Swimma.Enemies
{
    /// <summary>
    /// The base class for the enemies
    /// </summary>
    public class Enemy
    {

        private int dmg;

        public int Atk
        {
            get { return dmg; }
            set { dmg = value; }
        }
        private int speed;

        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }
       
        private int direction;

        public int Direction
        {
            get { return direction; }
            set { direction = value; }
        }
        private Texture2D enemyTexture;

        public Texture2D Texture
        {
            get { return enemyTexture; }
            set { enemyTexture = value; }
        }

        private Vector2 position;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        private int frameID;

        public int FrameID
        {
            get { return frameID; }
            set { frameID = value; }
        }
        private int frameCount;

        public int FrameCount
        {
            get { return frameCount; }
            set { frameCount = value; }
        }
        private int frameWidth;

        public int FrameWidth
        {
            get { return frameWidth; }
            set { frameWidth = value; }
        }
        private int frameHeight;

        public int FrameHeight
        {
            get { return frameHeight; }
            set { frameHeight = value; }
        }
        private int framesPerSecond;

        public int FramesPerSecond
        {
            get { return framesPerSecond; }
            set { framesPerSecond = value; }
        }

        public virtual Rectangle BoundingRectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y,
                               FrameWidth, Texture.Height);
            }
        }

        public virtual void Update(GameTime gameTime, Player player)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

        }
        /// <summary>
        /// Called when this enemy has been hit by a player.
        /// </summary>
        /// <param name="player">
        /// The player who collided with this enemy. 
        /// </param>
        public virtual void OnCollision(Level level)
        {
            var player = level.Player;
            if (player.LivesLeft > 1)
            {
                SoundLibrary.EnemyHit.Play();
            }
            else
            {
                SoundLibrary.PlayerDie.Play();
            }
            if (!player.IsHit)
            {
                player.LivesLeft -= Atk;
            }

            player.IsHit = true;
        }
          /// <summary>
        /// Called when this enemy has been hit by a player.
        /// </summary>
        /// <param name="player">
        /// The player who collided with this enemy. 
        /// </param>
        public virtual void OnCollision(Level level, Rectangle SpecificBounds)
        { 
        
        }
    }
}
