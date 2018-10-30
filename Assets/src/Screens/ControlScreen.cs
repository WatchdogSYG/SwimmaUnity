using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Swimma.GameLibrary;
using System.Diagnostics;
using Common;

namespace Swimma.Screens
{
    /// <summary>
    /// Class for the gamestate that displays the games Controls
    /// </summary>
    class ControlScreen : MenuManager
    {
        private Texture2D BackgoundImage;
        private Label backLabel;
        private Rectangle screenSize;
        private Texture2D GamePadImage;
        private Texture2D KeyboardImage;
        private Game game;


        public ControlScreen(Game game, Rectangle screenSize, EventHandler theMenuEvent)
            : base(theMenuEvent)
        {
            this.game = game;
            this.screenSize = screenSize;
            BackgoundImage = game.Content.Load<Texture2D>(@"Images/Menus/ControlsBG");
            GamePadImage = game.Content.Load<Texture2D>(@"Images/Menus/xboxControl");
            //KeyboardImage = game.Content.Load<Texture2D>(@"Images/Menus/KeyboardControl");

            backLabel = new Label();
            backLabel.SpriteFont = game.Content.Load<SpriteFont>(@"Fonts/MenuFont");
            backLabel.Name = "GoBack";
            backLabel.Text = "Return To Main Menu";
            backLabel.Color = Color.White;
            backLabel.TabStop = true;
            backLabel.HasFocus = true;
            backLabel.Position = new Vector2((screenSize.Width / 2) - backLabel.Origin, screenSize.Height - backLabel.SpriteFont.LineSpacing * 2);
        }




        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Matrix.CreateScale(game.GraphicsDevice.Viewport.Width / 1280f, game.GraphicsDevice.Viewport.Height / 720f, 0));

            //  spriteBatch.Begin();
            spriteBatch.Draw(BackgoundImage, screenSize, Color.White);

            spriteBatch.Draw(GamePadImage,
                new Rectangle(screenSize.Width / 3, screenSize.Height / 2 - 50, screenSize.Width / 3, screenSize.Height / 3), Color.White);

            /*spriteBatch.Draw(KeyboardImage,
                new Rectangle(screenSize.Width / 2 + 50, screenSize.Height / 2 - 50, screenSize.Width / 3, screenSize.Height / 3), Color.White);*/

            backLabel.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(spriteBatch, gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            /*if (InputHandler.KeyPressed(Keys.Enter) == true || InputHandler.ButtonPressed(Buttons.Start) ||
                InputHandler.ButtonPressed(Buttons.Back) || InputHandler.ButtonPressed(Buttons.A))*/
            if (GameInput.GetButtonDown())
            {
                SoundLibrary.MenuItemOK.Play();
                InputHandler.Flush();
                menuEvent.Invoke("Start", new EventArgs());

            }

            backLabel.Update(gameTime);

            base.Update(gameTime);
        }
    }
}
