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
using Swimma.Levels;


namespace Swimma
{
    /// <summary>
    /// Class for the Air healthbar
    /// </summary>
    public class AirBar
    {
        private Game game;
        private Vector2 position;
        private int elapsedTime;
        private Texture2D mHealthBar;
        // private float mCurrentHealth = 500;

        public AirBar(Game game, Vector2 position, Texture2D image)
        {
            this.game = game;
            this.position = position;
            mHealthBar = image;
        }

        public void Update(GameTime gameTime,Level level)
        {
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            if (elapsedTime >= 100)//decrement the healthbar every 100 milliseconds
            {
                level.Player.AirLeft -= .6f;
                //Debug.WriteLine(player.AirLeft);
                elapsedTime = 0;
            }
            //Force the health to remain between 0 and 500
            level.Player.AirLeft = MathHelper.Clamp(level.Player.AirLeft, 0, level.Player.MaxAir);
        }


        public void Draw(SpriteBatch spriteBatch, Level level)
        {
            //Draw the negative space for the health bar
            spriteBatch.Draw(mHealthBar, new Rectangle((int)position.X, (int)position.Y, mHealthBar.Width, 26), new Rectangle(0, 45, mHealthBar.Width, 26), Color.Gray,0.0f,Vector2.Zero,SpriteEffects.None,0f);

            //Draw the current health level based on the current Health
            if (level.Player.AirLeft < 150)
            {
                spriteBatch.Draw(mHealthBar, new Rectangle((int)position.X, (int)position.Y, (int)(mHealthBar.Width * ((double)level.Player.AirLeft / level.Player.MaxAir)), 26), new Rectangle(0, 45, mHealthBar.Width, 26), Color.Red, 0.0f, Vector2.Zero, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(mHealthBar, new Rectangle((int)position.X, (int)position.Y, (int)(mHealthBar.Width * ((double)level.Player.AirLeft / level.Player.MaxAir)), 26), new Rectangle(0, 45, mHealthBar.Width, 26), Color.Aqua, 0.0f, Vector2.Zero, SpriteEffects.None, 0f);
            }
            //Draw the box around the health bar
            spriteBatch.Draw(mHealthBar, new Rectangle((int)position.X, (int)position.Y, mHealthBar.Width, 26), new Rectangle(0, 0, mHealthBar.Width, 26), Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, 0f);

        }
        /// <summary>
        /// resets the healthbar
        /// </summary>
        public void ResetHealthBar(Level level)
        {
            level.Player.AirLeft = level.Player.MaxAir;
        }
    }
}
