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

namespace Swimma.Screens
{
    /// <summary>
    /// Class for displaying items on the main menu
    /// </summary>
    class MainMenu : MenuManager
    {
        private Texture2D BackgoundImage;
        private Label startGame;
        private Label Controls;
        private Label exitGame;
        private List<Label> list = new List<Label>();
        private Rectangle screenSize;
      
        private Game game;

        public MainMenu(Game game, Rectangle screensize, EventHandler theMenuEvent)
            : base(theMenuEvent)
        {
            this.game = game;
            this.screenSize = screensize;
            BackgoundImage = game.Content.Load<Texture2D>(@"Images/Menus/MainMenuBG");
            generateMenuItems(game.Content);
        }
        private void generateMenuItems(ContentManager content)
        {
            startGame = new Label();
            startGame.Text = "Start Game";
            startGame.Name = "Select";
            startGame.HasFocus = true;


            list.Add(startGame);

            Controls = new Label();
            Controls.Name = "Controls";
            Controls.Text = "Controls";

            list.Add(Controls);

            exitGame = new Label();
            exitGame.Name = "ExitGame";
            exitGame.Text = "Exit Game";

            list.Add(exitGame);

            Vector2 position = new Vector2(20 + screenSize.Width / 2, screenSize.Height / 4);
            foreach (Label l in list)
            {
                position.X = 20 + screenSize.Width / 2;
                l.SpriteFont = content.Load<SpriteFont>(@"Fonts/MenuFont");
                position += new Vector2(-l.Origin, l.SpriteFont.LineSpacing + 50);
                l.Position = position;

            }

        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Matrix.CreateScale(game.GraphicsDevice.Viewport.Width / 1280f, game.GraphicsDevice.Viewport.Height / 720f, 0));

            //  spriteBatch.Begin();
            spriteBatch.Draw(BackgoundImage, screenSize, Color.White);
            foreach (Label label in list)
            {
                label.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(spriteBatch, gameTime);
        }

        public override void Update(GameTime gameTime)
        {
           

            foreach (Label label in list)
            {

                label.Update(gameTime);

            }

            HandleMenuSelectionInput(list, SoundLibrary.MenuItemFocus, SoundLibrary.MenuItemOK);

            base.Update(gameTime);

        }
    }
}

