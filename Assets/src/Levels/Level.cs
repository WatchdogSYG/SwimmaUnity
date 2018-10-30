using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Swimma.GameLibrary.BackgroundManager;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Swimma.Levels
{
    public class Level : IDisposable
    {
        private GemGenerator gem;

        internal GemGenerator Gem
        {
            get { return gem; }
            set { gem = value; }
        }
        private BubbleGenerator airBubble;

        public BubbleGenerator AirBubble
        {
            get { return airBubble; }
            set { airBubble = value; }
        }

        private ParallaxBG background;

        public ParallaxBG Background
        {
            get { return background; }
            set { background = value; }
        }
        private bool startBuffer = true;

        public bool StartBuffer
        {
            get { return startBuffer; }
            set { startBuffer = value; }
        }
        // Entities in the level.
        private bool bossSpawned;

        public bool ExitSpawned
        {
            get { return bossSpawned; }
            set { bossSpawned = value; }
        }
        Player player;

        public Player Player
        {
            get { return player; }
            set { player = value; }
        }

        float levelDuration;

        public float LevelDuration
        {
            get { return levelDuration; }
            set { levelDuration = value; }
        }
        int gemsCollected;

        public int GemsCollected
        {
            get { return gemsCollected; }
            set { gemsCollected = value; }
        }

        bool reachedExit;

        public bool ReachedExit
        {
            get { return reachedExit; }
            set { reachedExit = value; }
        }
        private const int PointsPerSecond = 5;

        // Level content.        

        ContentManager content;

        public ContentManager Content
        {
            get { return content; }
            set { content = value; }
        }
        private float elapsedTime;
        private Rectangle screenSize;

        public Rectangle PlayableArea
        {
            get { return screenSize; }
            set { screenSize = value; }
        }

        public float ElapsedTime
        {
            get { return elapsedTime; }
            set { elapsedTime = value; }
        }
        private Rectangle viewport;
        public bool DrawMap;

        public Rectangle Viewport
        {
            get { return viewport; }
            set { viewport = value; }
        }
        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {

        }
        protected Rectangle SetSafePlayArea(Rectangle viewport, float safePercentatge = 0.05f)
        {
            float SafeAreaPortion = safePercentatge;
            return new Rectangle(
           (int)(viewport.Width * SafeAreaPortion),
           (int)(viewport.Height * SafeAreaPortion),
           (int)(viewport.Width * (1 - 2 * SafeAreaPortion)),
           (int)(viewport.Height * (1 - 2 * SafeAreaPortion)));

        }
        protected bool DrawFirstLoading(SpriteBatch spriteBatch, GameTime gameTime)
        {
            
            if (StartBuffer)
            {
                Texture2D tex = null;
                Vector2 pos = Vector2.Zero;
                if (ElapsedTime <= 1000)
                {
                    tex = Content.Load<Texture2D>(@"Images/CountDown/3");
                    pos = new Vector2((Viewport.Width / 2) - (tex.Bounds.Width / 2),
                         (Viewport.Height / 2) - (tex.Bounds.Height / 2));
                }
                else if (ElapsedTime > 1000 && ElapsedTime <= 2000)
                {
                    tex = Content.Load<Texture2D>(@"Images/CountDown/2");
                    pos = new Vector2((Viewport.Width / 2) - (tex.Bounds.Width / 2),
                       (Viewport.Height / 2) - (tex.Bounds.Height / 2));
                }
                else if (ElapsedTime > 2000 && ElapsedTime <= 3000)
                {
                    tex = Content.Load<Texture2D>(@"Images/CountDown/1");
                    pos = new Vector2((Viewport.Width / 2) - (tex.Bounds.Width / 2),
                       (Viewport.Height / 2) - (tex.Bounds.Height / 2));
                }
                else if (ElapsedTime > 3000 && ElapsedTime <= 4000)
                {
                    tex = Content.Load<Texture2D>(@"Images/CountDown/go");
                    pos = new Vector2((Viewport.Width / 2) - (tex.Bounds.Width / 2),
                       (Viewport.Height / 2) - (tex.Bounds.Height / 2));
                }
                if (tex != null)
                    spriteBatch.Draw(tex, pos, Color.White);
                else
                {
                    ElapsedTime = 0;
                    StartBuffer = false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }
        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
            Content.Unload();
         

        }


        #region Collision Detection

        protected bool PixelCollision(Texture2D playerTexture, Texture2D npcTexture, int playerFrame, int npcFrame, Rectangle playerBounds, Rectangle npcBounds)
        {
            try
            {
                Rectangle src = new Rectangle(playerFrame * playerBounds.Width, 0, playerBounds.Width, playerBounds.Height);
                Rectangle src2 = new Rectangle(npcFrame * npcBounds.Width, 0, npcBounds.Width, npcBounds.Height);
                Color[] colorData1 = new Color[playerTexture.Width * playerTexture.Height];
                Color[] colorData2 = new Color[npcTexture.Width * npcTexture.Height];

                try
                {
                    playerTexture.GetData<Color>(0, src, colorData1, 0, playerBounds.Width * playerBounds.Height);
                    npcTexture.GetData<Color>(0, src2, colorData2, 0, npcBounds.Width * npcBounds.Height);
                }
                catch (AccessViolationException ae) { return true; }

                // Calculate the intersecting rectangle
                int x1 = Math.Max(playerBounds.X, playerBounds.X);
                int x2 = Math.Min(playerBounds.X + playerBounds.Width, playerBounds.X + playerBounds.Width);

                int y1 = Math.Max(playerBounds.Y, npcBounds.Y);
                int y2 = Math.Min(playerBounds.Y + playerBounds.Height, npcBounds.Y + npcBounds.Height);

                // For each single pixel in the intersecting rectangle
                for (int y = y1; y < y2; ++y)
                {
                    for (int x = x1; x < x2; ++x)
                    {
                        // Get the color from each texture
                        Color a = colorData1[Math.Min(colorData1.Length - 1, (Math.Max(0, (x - playerBounds.X)) +
                            Math.Max(0, (y - playerBounds.Y) * playerBounds.Width)))];
                        Color b = colorData2[Math.Min(colorData2.Length - 1, (Math.Max(0, (x - npcBounds.X)) +
                            Math.Max(0, (y - npcBounds.Y) * npcBounds.Width)))];

                        if (a.A != 0 && b.A != 0) // If both colors are not transparent
                        //(the alpha channel is not 0), then there is a collision
                        {
                            return true;
                        }

                    }

                }
            }
            catch (Exception)
            {
                return true;
                // Debug.WriteLine("Bad things happened when detecting pixel collision. Error: " + ex.StackTrace);
            }
            //finally { }
            return false;
        }

        #endregion

    }
}
