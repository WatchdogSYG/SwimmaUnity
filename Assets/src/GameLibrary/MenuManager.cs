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
using System.Diagnostics;
using Common;
using REP_SLC;


namespace Swimma.GameLibrary
{
    /// <summary>
    /// manages the game states, the base class
    /// </summary>
    public class MenuManager
    {

        protected EventHandler menuEvent;
        protected SpriteBatch spriteBatch;

        public MenuManager(EventHandler theMenuEvent)
        {
            menuEvent = theMenuEvent;
            InputHandler.KeyboardState = Keyboard.GetState();
            InputHandler.GamepadState = GamePad.GetState(PlayerIndex.One);

        }
        /// <summary>
        /// alters the text that the pointer is currently pointed to, 
        /// scrolls up and down the list to select the next item
        /// </summary>
        /// <param name="list">the list of menu items</param>
        public void HandleMenuSelectionInput(List<Label> list, SoundEffect focusSFX, SoundEffect okSFX)
        {
            var inputSel = GameInput.GetThumbstickSelection();
            //if (InputHandler.KeyPressed(Keys.W) || InputHandler.JoystickUped() || InputHandler.KeyPressed(Keys.Up))
            bool changedInput = false;
            if (inputSel.Y > 0)
            {
                int items = list.Count - 1;

                for (int i = 0; i <= items; i++)
                {

                    if (list[i].HasFocus && i != 0)
                    {
                        list[i].HasFocus = false;
                        list[i - 1].HasFocus = true;
                        changedInput = true;
                        break;
                    }
                    else if (list[i].HasFocus && i == 0)
                    {
                        list[i].HasFocus = false;
                        list[items].HasFocus = true;
                        changedInput = true;
                        break;
                    }
                }
                focusSFX.Play(.5f,0,0);
            }

            //if (InputHandler.KeyPressed(Keys.S) || InputHandler.JoystickDowned() || InputHandler.KeyPressed(Keys.Down))
            if (inputSel.Y < 0)
            {
                int items = list.Count - 1;
                for (int i = 0; i <= items; i++)
                {
                    if (list[i].HasFocus && i != items)
                    {
                        list[i].HasFocus = false;
                        list[i + 1].HasFocus = true;
                        changedInput = true;
                        break;
                    }
                    else if (list[i].HasFocus && i == items)
                    {
                        list[i].HasFocus = false;
                        list[0].HasFocus = true;
                        changedInput = true;
                        break;
                    }
                }
                focusSFX.Play(.5f, 0, 0);
            }

            //if (InputHandler.KeyPressed(Keys.Enter) == true || InputHandler.ButtonPressed(Buttons.A))
            if (GameInput.GetButtonDown())
            {
                InputHandler.Flush();
                foreach (Label label in list)
                {
                    if (label.HasFocus)
                    {
                        okSFX.Play(.5f, 0, 0);
                        menuEvent.Invoke(label.Name, new EventArgs());
                    }

                }
                changedInput = true;
            }
            if (changedInput) {
                RumbleTimer.MenuRumble();
            }
        }


        /// <summary>
        /// virtual update and draw methods to allow for subclasses 
        /// to override and add any specific Update and Draw code they have. 
        /// </summary>
        /// <param name="gameTime"></param>

        public virtual void Update(GameTime gameTime)
        {
            InputHandler.PrevKeyboardState = InputHandler.KeyboardState;
            InputHandler.KeyboardState = Keyboard.GetState();

            InputHandler.PrevGamepadState = InputHandler.GamepadState;
            InputHandler.GamepadState = GamePad.GetState(PlayerIndex.One);

        }
        public virtual void Draw(SpriteBatch spriteBatch, GameTime gametime)
        {
        }

    }


}
