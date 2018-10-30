using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using Swimma.GameLibrary;
using Swimma.Levels;

namespace Swimma.Enemies
{
    class Exit : Enemy
    {

        private SpriteEffects spriteEffects = SpriteEffects.None;

        private Random random = new Random();
        private float newYPosition;
        private int StartingFrame;
        private Rectangle collisionBounds;
        private Texture2D collisionTexture;



        public Exit(Game game)
        {

            Speed = 1;

            // Texture = game.Content.Load<Texture2D>(@"Sprites/stingray");

        }

        public override void Update(GameTime gameTime, Player player)
        {

            Position = new Vector2(Position.X - Speed, Position.Y);

            float time = (float)gameTime.TotalGameTime.TotalSeconds;

            FrameID = (int)(((time * FramesPerSecond) + StartingFrame) % FrameCount);

        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            Rectangle frame = new Rectangle(FrameID * FrameWidth, 0, FrameWidth, FrameHeight);
            if (frame != Rectangle.Empty)
            {

                spriteBatch.Draw(Texture, Position, frame,
                    Color.White, 0.0f, Vector2.Zero, 1.0f, spriteEffects, .6f);
            }
            else
            {
                spriteBatch.Draw(Texture, Position, Texture.Bounds,
                    Color.White, 0.0f, Vector2.Zero, 1.0f, spriteEffects, .6f);
            }

        }
        public override void OnCollision(Level level)
        {
            if(Speed==0)
                if (((int)level.Player.Position.X + level.Player.BoundingRectangle.Width) >= (Position.X + Texture.Width / 2))
                {
                    level.ReachedExit = true;
                }
        }
        public override  void OnCollision(Level level, Rectangle SpecificBounds)
        {
            if (Speed == 0)
                if(level.Player.BoundingRectangle.Intersects(SpecificBounds)){ level.ReachedExit = true;}

        }
    }


}