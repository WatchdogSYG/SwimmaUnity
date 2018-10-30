using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Swimma.GameLibrary;
using REP_SLC;
using Common.DataLogging;
using Common;

namespace Swimma.Collectables
{
   public class Bubble
    {
        private Game game;

        public Bubble(Game game)
        {
            // TODO: Complete member initialization
            this.game = game;
            Speed = 3;
            Texture = game.Content.Load<Texture2D>(@"Sprites/bubble");
        }

        public Vector2 Position { get; set; }

        public void Update(GameTime gameTime)
        {
                // Animate this bubble passing by
                Position= new Vector2(Position.X - Speed, Position.Y);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position,Texture.Bounds,
            Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
        }
       
        public Texture2D Texture { get; set; }

        public float Speed { get; set; }

        public Rectangle BoundingRectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y,
                           Texture.Width, Texture.Height);
            }
        }


        public void OnCollision(Player player)
        {

            //update air health
            //Force the health to remain between 0 and 500
            player.AirLeft = (int)MathHelper.Clamp(player.AirLeft + 40, 0, player.MaxAir);
            SoundLibrary.BubbleCollect.Play();
            RumbleTimer.AddVibration(0.5f, 0.5f);
            player.IsHit = false;

            var logger = (LoggingService)Game1.Game.Services.GetService(typeof(LoggingService));
            if (logger != null) logger.UpdateData(LogChannel.EventChannel, "Bubble");
        }

    }
}
