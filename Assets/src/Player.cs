using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Swimma.GameLibrary.BackgroundManager;
using System.Diagnostics;
using Swimma.GameLibrary;
using Swimma.Screens;
using System.Threading;
using REP_SLC;
using Common;
using Common.DataLogging;

namespace Swimma
{
    public enum PlayerAvatar
    {
        Boy, Girl
    }
    public class Player
    {
        public float RotationAngle = 0.5f;

        #region Private Variables

        private SpriteEffects spriteEffects = SpriteEffects.None;
        private int frameID;
        private int frameCount = 8;
        private int frameWidth = 135;
        private int frameHeight = 90;
        private int framesPerSecond = 6;
        private const int moveSpeed = 8;

   

        private Vector2 playerPosition;
        private Texture2D playerTexture;
        private Rectangle safeBounds;

     
        private int score = 0;
        private int livesLeft;
        private float airLeft;

        private Boolean enemyHit = false;
        private double hitElapsed;
        private Game game;
        private float maxAir;
        private int maxLives = 10;//default value
        private Vector3 gyroReading;
        private bool isVibrating;


        #endregion

        #region Getters and Setters


        public bool IsVibrating
        {
            get { return isVibrating; }
            set { isVibrating = value; }
        }

        public Rectangle SafeBounds
        {
            get { return safeBounds; }
            set { safeBounds = value; }
        }
        public int MoveSpeed
        {
            get { return moveSpeed; }
        } 
        public float AirLeft
        {
            get { return airLeft; }
            set { airLeft = value; }
        }
        public float MaxAir
        {
            get { return maxAir; }
            set { maxAir = value; }
        }
        public int MaxLives
        {
            get { return maxLives; }
            set { maxLives = value; }
        }
        public Boolean IsHit
        {
            get { return enemyHit; }
            set { enemyHit = value; }
        }

        public Vector2 Position
        {
            get { return playerPosition; }
            set { playerPosition = value; }
        }

        public int FrameID
        {
            get { return frameID; }
            set { frameID = value; }
        }

        public int LivesLeft
        {
            get { return livesLeft; }
            set { livesLeft = value; }
        }

        public int Score
        {
            get { return score; }
            set { score = value; }
        }


        public Texture2D Texture
        {
            get { return playerTexture; }
            set { playerTexture = value; }
        }

        public Rectangle BoundingRectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y,
                        frameWidth, playerTexture.Height);
            }
        }

        public static PlayerAvatar Avatar { get; set; }

        #endregion

        public Player(Game game, Rectangle safeBounds, int maxAir)
        {
            this.safeBounds = safeBounds;
            this.game = game;
            MaxAir = maxAir;
            airLeft = MaxAir;
            LoadPlayerImage();

            playerPosition.X = (safeBounds.Width - frameWidth) / 2;
            playerPosition.Y = safeBounds.Height - playerTexture.Height;
        }
        public Player(Game game, Rectangle safeBounds, int maxAir, Vector2 position)
        {
            this.safeBounds = safeBounds;
            this.game = game;
            MaxAir = maxAir;
            airLeft = maxAir;
            LoadPlayerImage();

            playerPosition = position;
        }
   
        private void LoadPlayerImage()
        {
            switch (Avatar)
            {
                case PlayerAvatar.Boy:
                    playerTexture = game.Content.Load<Texture2D>(@"Sprites/boy");
                    break;
                case PlayerAvatar.Girl:
                    playerTexture = game.Content.Load<Texture2D>(@"Sprites/girl");
                    break;
                default:
                    playerTexture = game.Content.Load<Texture2D>(@"Sprites/Player");
                    break;
            }
        }

        public void ResetPlayer()
        {
            playerPosition.X = (SafeBounds.Width - frameWidth) / 2;
            playerPosition.Y = SafeBounds.Height - playerTexture.Height;
            score = 0;
            livesLeft = maxLives;
            enemyHit = false;
            hitElapsed = 0.0f;
            spriteEffects = SpriteEffects.None;
        }

        public void Update(GameTime gameTime)
        {
            var logger = (LoggingService)Game1.Game.Services.GetService(typeof(LoggingService));
            if (logger != null) logger.UpdateData(LogChannel.ScreenPositionChannel, (short)Position.X, (short)Position.Y);

            // Prevent the person from moving off of the screen
            playerPosition.X = MathHelper.Clamp(playerPosition.X,
                SafeBounds.Left, SafeBounds.Right - frameWidth);
            playerPosition.Y = MathHelper.Clamp(playerPosition.Y,
              SafeBounds.Top, SafeBounds.Bottom - playerTexture.Height);
            //check for button press
            UpdateInput(gameTime);
        }



        public void UpdateInput(GameTime gameTime)
        {
          
            //move with the current automatically
            playerPosition.X -= 2;
            playerPosition.Y -= 2;

            if (InputHandler.KeyDown(Keys.W) || InputHandler.KeyDown(Keys.Up) )
            {
                playerPosition.Y -= MoveSpeed;

            }
            if (InputHandler.KeyDown(Keys.S) || InputHandler.KeyDown(Keys.Down) )
            {
                playerPosition.Y += MoveSpeed;

            }
            if (InputHandler.KeyDown(Keys.A) || InputHandler.KeyDown(Keys.Left))
            {
                playerPosition.X -= MoveSpeed;
                spriteEffects = SpriteEffects.FlipHorizontally;

            }
            if (InputHandler.KeyDown(Keys.D) || InputHandler.KeyDown(Keys.Right))
            {
                playerPosition.X += MoveSpeed;
                spriteEffects = SpriteEffects.None;

            }
            if (InputHandler.KeyReleased(Keys.A) || InputHandler.KeyReleased(Keys.Left))
            {
                spriteEffects = SpriteEffects.None;
            }

            var thumbinput = GameInput.GetThumbstickInput();
            if (thumbinput != Vector2.Zero)
            {
                playerPosition += (thumbinput * new Vector2(MoveSpeed, -MoveSpeed));

                if (thumbinput.X < 0)
                {
                    spriteEffects = SpriteEffects.FlipHorizontally;
                }
                else
                {
                    spriteEffects = SpriteEffects.None;
                }
            }

#if WINDOWS_PHONE
                 if (gyroReading != Vector3.Zero)
            {
                Matrix gyroTransformer =
                    Matrix.CreateRotationY(MathHelper.Pi * RotationAngle) *
                    Matrix.CreateRotationZ(MathHelper.PiOver2);
                Vector3 gyroLocalReading = Vector3.TransformNormal(gyroReading, gyroTransformer);
                
                Vector2 pos = new Vector2(-gyroLocalReading.X, -gyroLocalReading.Z);
                playerPosition += (pos *
                    new Vector2(-moveSpeed, -moveSpeed));

                if (pos.X > 0)
                {
                    spriteEffects = SpriteEffects.FlipHorizontally;
                }
                else
                {
                    spriteEffects = SpriteEffects.None;
                }

            }
#endif
            //calculate the frames to draw for player
            FrameID = 0;
            float time = (float)gameTime.TotalGameTime.TotalSeconds;
            FrameID += (int)(time * framesPerSecond) % frameCount;

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

            Rectangle frame = new Rectangle(FrameID * frameWidth, 0, frameWidth, frameHeight);
            //if the player hits an enemy, blink the sprite
            if (enemyHit)
            {
                if (!isVibrating) { RumbleTimer.AddVibration(.5f, 1.5f); isVibrating = true; }
                if (!RumbleTimer.IsReady) GamePad.SetVibration(PlayerIndex.One, .5f, .5f);
                hitElapsed += gameTime.ElapsedGameTime.Milliseconds;
                //turn player black if being injured
                if (livesLeft == 0) { spriteEffects = SpriteEffects.FlipVertically; }
                if (hitElapsed % 10 == 0 || hitElapsed % 10 == 1)
                {
                    spriteBatch.Draw(playerTexture, Position, frame,
                        Color.Black, 0.0f, Vector2.Zero, 1.0f, spriteEffects, 0.5f);
                }
            }
            else
            {
                //otherwise just draw the sprite normally
                spriteBatch.Draw(playerTexture, Position, frame,
                    Color.White, 0.0f, Vector2.Zero, 1.0f, spriteEffects, 0.5f);
            }
            //keep blinking the sprite for up to 1.5 seconds to give the player a chance to get away
            if (hitElapsed > 1500 && enemyHit)
            {
                if (!RumbleTimer.IsReady) GamePad.SetVibration(PlayerIndex.One, 0, 0);
                enemyHit = false;
                isVibrating= false;
                hitElapsed = 0;
            }
        }

    }
}
