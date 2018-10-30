using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Swimma.GameLibrary;
using Swimma.Levels;
using REP_SLC;

namespace Swimma.Collectables
{
   public class OneUp
    {
        private Game game;

        public OneUp(Game game)
        {
            // TODO: Complete member initialization
            this.game = game;
            Speed = 3;
            Texture = game.Content.Load<Texture2D>(@"Sprites/Heart");
            Position = new Vector2(game.GraphicsDevice.Viewport.Bounds.Width, game.GraphicsDevice.Viewport.Bounds.Height / 2);
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


        public void OnCollision(Level level)
        {
            var player = level.Player;
            //update air health
            //Force the health to remain between 0 and 500
            player.LivesLeft += 1;
            //SoundLibrary.BubbleCollect.Play();
            player.IsHit = false;
            RumbleTimer.AddVibration(0.5f, 0.5f);
        }

    }
}
