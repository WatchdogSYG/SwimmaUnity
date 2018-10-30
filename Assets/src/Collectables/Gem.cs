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
using REP_SLC;

namespace Swimma.Collectables
{
    class Gem
    {
        private Color color;
        private Vector2 gemPosition;
        private string name = "Gem";
        private Texture2D texture;
        private Vector2 startPosition;

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }
        public Vector2 InitialPosition
        {
            get { return startPosition; }
            set { startPosition = value; }
        }
        public Vector2 Position
        {
            get { return gemPosition; }
            set { gemPosition = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }


        public Rectangle BoundingRectangle()
        {
            return new Rectangle((int)Position.X, (int)Position.Y,
                           Texture.Width, Texture.Height);
        }


        public Gem(Game game)
        {
            Texture = game.Content.Load<Texture2D>(@"Sprites/Gem");
        }

        /// <summary>
        ///  Called when this gem has been collected by a player and removed from the level.
        /// </summary>
        /// </summary>
        /// <param name="player">the player that collected the gem</param>
        /// <param name="color">the color of the collected gem</param>
        public void OnCollected(Level level, Color color) {
            SoundLibrary.GemCollect.Play();
            level.GemsCollected++;

            RumbleTimer.AddVibration(0.2f, 0.2f);

            if (color == Color.Aquamarine) {
                level.Player.Score += 1;
            } else if (color == Color.OrangeRed) {
                level.Player.Score += 2;
            } else if (color == Color.WhiteSmoke) {
                level.Player.Score += 3;
            } else if (color == Color.Yellow) {
                level.Player.Score += 5;
            } else if (color == Color.SeaGreen) {
                level.Player.Score += 6;
            } else if (color == Color.Orchid) {
                level.Player.Score += 10;
            } else if (color == Color.Gold) {
                level.Player.Score += 100;
            } else { level.Player.Score += 1; }
        }
    }
}
