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
    /// overlays a black transparent texture above gamplay to pause game
    /// </summary>
    class PauseMenu : MenuManager
    {
        private Texture2D blankTexture;
        private Game game;
        private Texture2D pausedSign;
        private Rectangle screenSize;
        private Label ResumeGame;
        private Label ExitGame;
        private List<Label> list = new List<Label>();


        public PauseMenu(Game game, Rectangle screensize, EventHandler theMenuEvent)
            : base(theMenuEvent)
        {
          
            this.game = game;
            this.screenSize = screensize;
            blankTexture = game.Content.Load<Texture2D>(@"Images/blank");
            pausedSign = game.Content.Load<Texture2D>(@"Images/Menus/paused");

            ResumeGame = new Label();
            ResumeGame.Text = "Resume Game";
            ResumeGame.Name = "PlayGame";
            ResumeGame.HasFocus = true;
            list.Add(ResumeGame);
 
            ExitGame = new Label();
            ExitGame.Text = "Return to Main Menu";
            ExitGame.Name = "Start";
            ExitGame.HasFocus = false;
            list.Add(ExitGame);


            Vector2 position = new Vector2(screensize.Width/2, screensize.Height-200);
            foreach (Label l in list){
                l.Color = Color.White;
                l.SpriteFont = game.Content.Load<SpriteFont>(@"Fonts/MenuFont");
                position += new Vector2(0, l.SpriteFont.LineSpacing + 30);
                l.Position = position;
            }
        }

        public override void Update(GameTime gameTime)
        {
            //resume game 
            //if (InputHandler.KeyPressed(Keys.Space) || InputHandler.ButtonPressed(Buttons.Back) || InputHandler.ButtonPressed(Buttons.Start))
            /*if (GameInput.GetButtonDown())
            {
                GamePlayScreen.Paused = false;
                menuEvent.Invoke("PlayGame", new EventArgs());
            }*/

            foreach (Label label in list)
            {
                label.Update(gameTime);

            }

            HandleMenuSelectionInput(list, SoundLibrary.MenuItemFocus, SoundLibrary.MenuItemOK);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Matrix.CreateScale(game.GraphicsDevice.Viewport.Width / 1280f, game.GraphicsDevice.Viewport.Height / 720f, 0));

            //  spriteBatch.Begin();

            spriteBatch.Draw(blankTexture, new Rectangle(0, 0, screenSize.Width,
                                     screenSize.Height), new Color(0, 0, 0, 150));
            spriteBatch.Draw(pausedSign, new Vector2(screenSize.Width/2 - pausedSign.Width/2,
                                    screenSize.Height / 2 - pausedSign.Height / 2), Color.White);

            foreach (Label label in list)
            {
                label.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(spriteBatch, gameTime);
        }
    }
}
