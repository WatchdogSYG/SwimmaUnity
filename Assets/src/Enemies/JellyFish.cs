using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Swimma.Enemies
{
    class JellyFish : Enemy
    {

        private SpriteEffects spriteEffects = SpriteEffects.None;

        private Random random = new Random();
        private float newYPosition;
        private int StartingFrame;

        public JellyFish(Game game)
        {
            Atk = 1;
            Speed = 2;
            FrameCount = 5;
            FramesPerSecond = 8;
            FrameWidth = 100;
            FrameHeight = 100;
            Texture = game.Content.Load<Texture2D>(@"Sprites/jellyfish");
            StartingFrame = random.Next(FrameCount);
        }

        public override void Update(GameTime gameTime, Player player)
        {

            newYPosition = Position.Y;

            // Update the enemy state when passing by
            //stop enemy when it is big
            if (FrameID == 0)
            {
                Position = new Vector2(Position.X - 1, newYPosition);
            }
            //slow down when deflated
            else if (FrameID == 2)
            {
                Position = new Vector2(Position.X - 1, newYPosition - Speed - 2);
            }
            else
            {
                Position = new Vector2(Position.X - 1, Position.Y - Speed);
            }

            //reset framecount
            if (FrameID == 5)
                FrameID = 0;

            float time = (float)gameTime.TotalGameTime.TotalSeconds;

            FrameID = (int)(((time * FramesPerSecond) + StartingFrame) % FrameCount);


        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            Rectangle frame = new Rectangle(FrameID * FrameWidth, 0, FrameWidth, FrameHeight);

            spriteBatch.Draw(Texture, Position, frame,
                Color.White, 0.0f, Vector2.Zero, 1.0f, spriteEffects, 0.5f);


        }

    }


}