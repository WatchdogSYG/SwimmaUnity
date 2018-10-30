using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Swimma.GameLibrary.BackgroundManager
{
    /// <summary>
    /// This class handles the parallax movement of the background images
    /// </summary>
    public class ParallaxBG
    {
        #region Private Variables

        private Vector2 windowSize;
        private List<BackgroundLayer> layerList = new List<BackgroundLayer>();

        #endregion

        public ParallaxBG(GraphicsDevice graphics)
        {
            windowSize.X = graphics.Viewport.Width;
            windowSize.Y = graphics.Viewport.Height;
        }

        public void AddLayer(Texture2D bgTexture, Vector2 position, float depth, float moveRate)
        {
            BackgroundLayer layer = new BackgroundLayer();
            layer.BgTexture = bgTexture;
            layer.Depth = depth;
            layer.MoveRate = moveRate;
            layer.PictureSize = new Vector2(bgTexture.Width, bgTexture.Height);
            layer.Position = position;
            layerList.Add(layer);
        }
        public void RemoveLayer(BackgroundLayer layer)
        {
            layerList.Remove(layer);
        }

        public void RemoveLayerAt(int pos)
        {
            layerList.RemoveAt(pos);
        }
        public BackgroundLayer GetLayerAt(int pos)
        {
            return layerList[pos];
        }
        public List<BackgroundLayer> GetAllBackgorunds()
        {
            return layerList;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < layerList.Count; i++)
            {
                if (layerList[i].Position.X < windowSize.X)
                {
                    spriteBatch.Draw(layerList[i].BgTexture, new Rectangle((int)layerList[i].Position.X,
                        (int)layerList[i].Position.Y, (int)layerList[i].PictureSize.X, (int)layerList[i].PictureSize.Y), layerList[i].BgTexture.Bounds,
                        Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, layerList[i].Depth);
                   // spriteBatch.Draw(layerList[i].BgTexture, new Vector2(layerList[i].Position.X, layerList[i].Position.Y), layerList[i].Color);
                }
                //draw before actual bg posiiton
                if (layerList[i].Position.X > 0f)
                {
                    spriteBatch.Draw(layerList[i].BgTexture, new Rectangle((int)(layerList[i].Position.X - layerList[i].PictureSize.X),
                        (int)layerList[i].Position.Y, (int)layerList[i].PictureSize.X, (int)layerList[i].PictureSize.Y), layerList[i].BgTexture.Bounds,
                   Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, layerList[i].Depth);
                   // spriteBatch.Draw(layerList[i].BgTexture, new Vector2(layerList[i].Position.X - layerList[i].PictureSize.X, layerList[i].Position.Y), layerList[i].Color);
                }
                else
                {
                    spriteBatch.Draw(layerList[i].BgTexture, new Rectangle((int)(layerList[i].Position.X + layerList[i].PictureSize.X),
                        (int)layerList[i].Position.Y, (int)layerList[i].PictureSize.X, (int)layerList[i].PictureSize.Y), layerList[i].BgTexture.Bounds,
                   Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, layerList[i].Depth);
                   
                    //spriteBatch.Draw(layerList[i].BgTexture, new Vector2(layerList[i].Position.X + layerList[i].PictureSize.X, layerList[i].Position.Y), layerList[i].Color);
                }

                if (layerList[i].Position.X < 0f && layerList[i].Position.X + (layerList[i].PictureSize.X*2) < windowSize.X)
                {
                    spriteBatch.Draw(layerList[i].BgTexture, new Rectangle((int)(layerList[i].Position.X + (layerList[i].PictureSize.X*2)),
                        (int)layerList[i].Position.Y, (int)layerList[i].PictureSize.X, (int)layerList[i].PictureSize.Y), layerList[i].BgTexture.Bounds,
                   Color.White, 0.0f, Vector2.Zero, SpriteEffects.None, layerList[i].Depth);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (BackgroundLayer layer in layerList)
            {
                float moveDist = layer.MoveRate / 60f;
                float xPosition = layer.Position.X + moveDist;
                layer.Position = new Vector2(xPosition % layer.PictureSize.X, layer.Position.Y);
            }
        }

    }
}
