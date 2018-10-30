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
    /// the Initial menu screen that appears
    /// </summary>
    class StartMenu : MenuManager
    {





        private Texture2D backgoundImage;
        private Label startLabel;
        private Rectangle screenSize;
        private Game game;


        public StartMenu(Game game, Rectangle screenSize, EventHandler theMenuEvent)
            : base(theMenuEvent)
        {
            this.game = game;
            this.screenSize = screenSize;
         
            backgoundImage = game.Content.Load<Texture2D>(@"Images/Menus/StartMenuBG");

            startLabel = new Label();
            startLabel.Name = "Start";
            startLabel.Text = "Press Button to Start";
            startLabel.Color = Color.White;
            startLabel.TabStop = true;
            startLabel.HasFocus = true;
            startLabel.SpriteFont = game.Content.Load<SpriteFont>(@"Fonts/MenuFont");
            startLabel.Position = new Vector2((screenSize.Width / 2) - startLabel.Origin, screenSize.Height - (startLabel.SpriteFont.LineSpacing * 3));


        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Matrix.CreateScale(game.GraphicsDevice.Viewport.Width / 1280f, game.GraphicsDevice.Viewport.Height / 720f, 0));
          
          //  spriteBatch.Begin();

            spriteBatch.Draw(backgoundImage, screenSize, Color.White);
            startLabel.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(spriteBatch, gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            //animate the text
            startLabel.Update(gameTime);

            HandleInput();


            base.Update(gameTime);
        }

        private void HandleInput()
        {
            /*if (InputHandler.KeyPressed(Keys.Enter) == true || InputHandler.ButtonPressed(Buttons.Start)
                || InputHandler.ButtonPressed(Buttons.A))*/
            if (GameInput.GetButtonDown())
            {
                SoundLibrary.MenuItemOK.Play();
                InputHandler.Flush();

                menuEvent.Invoke("Start", new EventArgs());

            }
        }
    }
}
