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
    /// Class for the game state that displays the the game over screen
    /// </summary>
    class GameOverScreen : MenuManager
    {
        private Texture2D BackgoundImage;
        private Label replayLabel;
        private Rectangle screenSize;
        private List<Label> list = new List<Label>();
        private Label returnLabel;
        private SpriteFont scoreFont;
        private static int score;
        private Vector2 scorePosition;
       
        private Game game;

        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        public GameOverScreen(Game game, Rectangle screenSize, EventHandler theMenuEvent)
            : base(theMenuEvent)
        {
            this.game = game;
            this.screenSize = screenSize;
            BackgoundImage = game.Content.Load<Texture2D>(@"Images/Menus/HighScoresBG");
            scoreFont = game.Content.Load<SpriteFont>(@"Fonts/ScoreFont");
           
            replayLabel = new Label();
            replayLabel.Name = "PlayGame";
            replayLabel.SpriteFont = game.Content.Load<SpriteFont>(@"Fonts/MenuFont");
            replayLabel.Position = new Vector2(screenSize.Width / 2 , screenSize.Height- replayLabel.SpriteFont.LineSpacing*3);
            replayLabel.Text = "Replay Game";
            replayLabel.Color = Color.White;
            replayLabel.TabStop = true;
            replayLabel.HasFocus = true;
            
            list.Add(replayLabel);

            returnLabel = new Label();
            returnLabel.Name = "Start";
            returnLabel.SpriteFont = game.Content.Load<SpriteFont>(@"Fonts/MenuFont");
            returnLabel.Position = new Vector2(screenSize.Width / 2, screenSize.Height - returnLabel.SpriteFont.LineSpacing );
            returnLabel.Text = "Return To Main Menu";
            returnLabel.Color = Color.White;
            returnLabel.TabStop = true;
            returnLabel.HasFocus = false;

            list.Add(returnLabel);

          
        }


        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Matrix.CreateScale(game.GraphicsDevice.Viewport.Width / 1280f, game.GraphicsDevice.Viewport.Height / 720f, 0));

            //  spriteBatch.Begin();
            spriteBatch.Draw(BackgoundImage, screenSize, Color.White);

            scorePosition = new Vector2((screenSize.Width / 2) - ((score.ToString().Length *
                scoreFont.LineSpacing / 2) / 2), screenSize.Height/2-scoreFont.LineSpacing/4);

            spriteBatch.DrawString(scoreFont, score.ToString(), scorePosition, Color.White);

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
         
            HandleMenuSelectionInput(list, SoundLibrary.MenuItemFocus,SoundLibrary.MenuItemOK);

            base.Update(gameTime);
        }
    }
}
