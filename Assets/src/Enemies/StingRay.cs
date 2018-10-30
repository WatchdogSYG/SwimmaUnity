using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Swimma.Enemies
{
    class StingRay : Enemy
    {

        private SpriteEffects spriteEffects = SpriteEffects.None;

        private Random random = new Random();
        private float newYPosition;
        private int StartingFrame;

        public StingRay(Game game)
        {
            Atk = 1;
            Speed = 2;
            FrameCount = 7;
            FramesPerSecond = 4;
            FrameWidth = 100;
            FrameHeight = 50;
            Texture = game.Content.Load<Texture2D>(@"Sprites/stingray");
            StartingFrame = random.Next(FrameCount);
        }

        public override void Update(GameTime gameTime, Player player)
        {
            newYPosition = Position.Y;

            //follow the player when score is higher than 100 - sort of
            //if player is behind enemy, enemy will ignore player
            if (Position.X > player.Position.X)
            {
                //if player is infront of enemy, make enemy Y the same as player Y
                if (Position.Y < player.Position.Y)
                {
                    newYPosition = Position.Y + (Speed - 2);
                }
                else if (Position.Y > player.Position.Y)
                {
                    newYPosition = Position.Y- (Speed - 2);
                }
                else
                {
                    newYPosition = Position.Y;
                }

            }

            //// Update the enemy state when passing by
            ////stop enemy when it is big
            //if (FrameID == 0)
            //{
            //    Position = new Vector2(Position.X, newYPosition);
            //}
            ////slow down when deflated
            //else if (FrameID == 2)
            //{
            //    Position = new Vector2(Position.X - Speed - 2, newYPosition);
            //}
            //else
            //{
               Position = new Vector2(Position.X - Speed, newYPosition);
            //}

            //// FrameID += 1;
            ////reset framecount
            //if (FrameID == 7)
            //    FrameID = 0;

            float time = (float)gameTime.TotalGameTime.TotalSeconds;

            FrameID = (int)(((time * FramesPerSecond) + StartingFrame) % FrameCount);

            
        }
        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            Rectangle frame = new Rectangle(FrameID * FrameWidth, 0,FrameWidth, FrameHeight);

            spriteBatch.Draw(Texture, Position, frame,
                Color.White, 0.0f, Vector2.Zero, 1.0f, spriteEffects, 0.5f);
          

        }
    }


}