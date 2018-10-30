using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Swimma.GameLibrary
{
    /// <summary>
    /// Class to manage the animation of the linking text for the menus
    /// </summary>
    public class Label
    {
        #region Private Variables

        private string name;
        private string text;
        private Vector2 size;
        private Vector2 position;
        private object value;
        private bool hasFocus;
        private bool enabled;
        private bool visible = true;
        private bool tabStop;
        private SpriteFont spriteFont;
        private Color color = Color.DarkOrange;
        private string type;
        private Color selectedColor = Color.Red;
        private float selectionFade;
        private int origin;
        private GameTime gTime = new GameTime();

        #endregion

        #region Getters and Setters
        public int Origin
        {
            get { return (Text.Length * SpriteFont.LineSpacing ) / 4; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public Vector2 Size
        {
            get { return size; }
            set { size = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                position.Y = (int)position.Y;
            }
        }

        public object Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public bool HasFocus
        {
            get { return hasFocus; }
            set { hasFocus = value; }
        }

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        public bool TabStop
        {
            get { return tabStop; }
            set { tabStop = value; }
        }

        public SpriteFont SpriteFont
        {
            get { return spriteFont; }
            set { spriteFont = value; }
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public Color SelectedColor
        {
            get { return selectedColor; }
            set { selectedColor = value; }
        }
        #endregion

        public void Update(GameTime gameTime)
        {
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;
            gTime = gameTime;

            selectionFade = Math.Min(selectionFade + fadeSpeed, 1);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            double time = gTime.TotalGameTime.TotalSeconds;
            float pulsate = (float)Math.Sin(time * 6) + 1;

            Vector2 origin = new Vector2(0, SpriteFont.LineSpacing / 2);
            float scale = 1 + pulsate * 0.02f * selectionFade;
            //if the label is selected pulsate red
            if (hasFocus)
            {
                spriteBatch.DrawString(SpriteFont, Text, Position, selectedColor, 0, origin, scale, SpriteEffects.None, 0);
            }
            else
            {
                spriteBatch.DrawString(SpriteFont, Text, Position, Color, 0, origin, 1, SpriteEffects.None, 0);
            }
        }

    }

}
