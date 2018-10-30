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
using Common.DataLogging;
using REP_SLC;


namespace Swimma.Screens
{
    /// <summary>
    /// the Initial menu screen that appears
    /// </summary>
    class PlayerSelection : MenuManager
    {
        private Texture2D backgoundImage;
        private Label selectLabel;
        private Rectangle screenSize;
        private readonly PlayerAvatar[] types = { PlayerAvatar.Boy, PlayerAvatar.Girl };
        private int currentSelection = 0;
        private SpriteFont font;
        private Texture2D boy;
        private Texture2D girl;
        private Texture2D border;
        private Game game;

        private int centerPos;
        public PlayerSelection(Game game, Rectangle screenSize, EventHandler theMenuEvent)
            : base(theMenuEvent)
        {
            this.screenSize = screenSize;
            this.game = game;
            backgoundImage = game.Content.Load<Texture2D>(@"Images/Backgrounds/seaHQ");
            boy = game.Content.Load<Texture2D>(@"Images/Characters/boy");
            girl = game.Content.Load<Texture2D>(@"Images/Characters/girl");
            border = game.Content.Load<Texture2D>(@"Images/Characters/border");

            this.font = game.Content.Load<SpriteFont>(@"Fonts/MenuFont");
            selectLabel = new Label();
            selectLabel.Name = "PlayGame";
            selectLabel.Text = "Select Avatar";
            selectLabel.Color = Color.White;
            selectLabel.TabStop = true;
            selectLabel.HasFocus = true;
            selectLabel.SpriteFont = font;
            selectLabel.Position = new Vector2((screenSize.Width / 2) - selectLabel.Origin, screenSize.Height / 2 + boy.Height / 2 + 100);

        }

        public override void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Matrix.CreateScale(game.GraphicsDevice.Viewport.Width / 1280f, game.GraphicsDevice.Viewport.Height / 720f, 0));

            //  spriteBatch.Begin();

            spriteBatch.Draw(backgoundImage, screenSize, Color.White);
            string text = "Who Do You Want To Be?";
            spriteBatch.DrawString(font, text, new Vector2(screenSize.Width / 2 - text.Length * font.LineSpacing / 4, screenSize.Height/2- boy.Height/2 - 100), Color.White);
            selectLabel.Draw(spriteBatch);

            DrawAvatars(spriteBatch);

            spriteBatch.End();

            base.Draw(spriteBatch, gameTime);
        }

        private void DrawAvatars(SpriteBatch spriteBatch)
        {

            var totalWidth = boy.Width + girl.Width + 50;

            centerPos = screenSize.Width / 2 - totalWidth / 2;



            spriteBatch.Draw(boy, new Vector2(centerPos, screenSize.Height / 2 - boy.Height / 2), Color.White);
            spriteBatch.Draw(girl, new Vector2(centerPos + boy.Width + 50, screenSize.Height / 2 - boy.Height / 2), Color.White);

            //highlight the selected avatar
            switch (currentSelection)
            {
                case 0:
                    spriteBatch.Draw(border, new Vector2(centerPos, screenSize.Height / 2 - boy.Height / 2), Color.White);
                    break;
                case 1:
                    spriteBatch.Draw(border, new Vector2(centerPos + boy.Width + 50, screenSize.Height / 2 - boy.Height / 2), Color.White);
                    break;
            }



        }
        private void LoadAudio()
        {
            switch (currentSelection)
            {
                case 0:
                    SoundLibrary.EnemyHit = game.Content.Load<SoundEffect>(@"Audio/watchit");
                    SoundLibrary.PlayerDie = game.Content.Load<SoundEffect>(@"Audio/ergh");
                    SoundLibrary.LifeCollect = game.Content.Load<SoundEffect>(@"Audio/boyPowerUp");
                    break;
                case 1:
                    SoundLibrary.EnemyHit = game.Content.Load<SoundEffect>(@"Audio/girlHit");
                    SoundLibrary.PlayerDie = game.Content.Load<SoundEffect>(@"Audio/girldie");
                    SoundLibrary.LifeCollect = game.Content.Load<SoundEffect>(@"Audio/girlPowerUp");
                    break;
            }
        }
        public override void Update(GameTime gameTime)
        {
            int oldSel = currentSelection;

            //if (InputHandler.KeyPressed(Keys.Enter) == true || InputHandler.ButtonPressed(Buttons.A))
            if (GameInput.GetButtonDown())
            {
                SoundLibrary.MenuItemOK.Play();
                InputHandler.Flush();
                Player.Avatar = (PlayerAvatar)currentSelection;
                LoadAudio();
                menuEvent.Invoke("PlayGame", new EventArgs());

                var logger = (LoggingService)Game1.Game.Services.GetService(typeof(LoggingService));
                if (logger != null) logger.UpdateData(LogChannel.EventChannel, "Select" + (PlayerAvatar)currentSelection);
                oldSel = -1;
            }

            if (InputHandler.KeyPressed(Keys.Escape) == true || InputHandler.ButtonPressed(Buttons.B)
                || InputHandler.ButtonPressed(Buttons.Back))
            {
                SoundLibrary.MenuItemOK.Play();
                menuEvent.Invoke("Start", new EventArgs());

            }

            var inputSel = GameInput.GetThumbstickSelection();
            //if (InputHandler.JoystickLefted() || InputHandler.KeyPressed(Keys.Left) || InputHandler.KeyPressed(Keys.A))
            if (inputSel.X < 0)
            {
                SoundLibrary.MenuItemFocus.Play();
                currentSelection++;
                if (currentSelection > types.Length - 1)
                {
                    currentSelection = 0;
                }

            }

            //if (InputHandler.JoystickRighted() || InputHandler.KeyPressed(Keys.Right) || InputHandler.KeyPressed(Keys.D))
            if (inputSel.X > 0)
            {
                SoundLibrary.MenuItemFocus.Play();
                currentSelection--;
                if (currentSelection < 0)
                {
                    currentSelection = types.Length - 1;
                }

            }

            selectLabel.Update(gameTime);

            base.Update(gameTime);
            if (oldSel != currentSelection) {
                RumbleTimer.MenuRumble();
            }
        }
    }
}
