using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Swimma.GameLibrary.BackgroundManager
{
    public class BackgroundLayer
    {
        private Texture2D bgTexture;
        private Vector2 position;
        private Vector2 offset = Vector2.Zero;
        private Color color = Color.White;
        private Vector2 pictureSize = Vector2.Zero;
        private float depth = 0f;
        private float moveRate = 0f;

        public Texture2D BgTexture
        {
            get { return bgTexture; }
            set { bgTexture = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector2 Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        public float Depth
        {
            get { return depth; }
            set { depth = value; }
        }

        public float MoveRate
        {
            get { return moveRate; }
            set { moveRate = value; }
        }

        public Vector2 PictureSize
        {
            get { return pictureSize; }
            set { pictureSize = value; }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

    }

}
