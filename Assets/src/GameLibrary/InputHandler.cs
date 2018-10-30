using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Swimma.GameLibrary
{
    /// <summary>
    /// This class handles specific input combinations
    /// </summary>
    public class InputHandler
    {
        #region Private Variables

        private static KeyboardState keyboardState;
        private static KeyboardState prevKeyboardState;
        private static GamePadState gamepadState;
        private static GamePadState prevGamepadState;
        private static MouseState mouseState;
        private static MouseState prevMouseState;


        #endregion

        #region Getters and Setters

        public static GamePadState PrevGamepadState
        {
            get { return prevGamepadState; }
            set { prevGamepadState = value; }
        }

        public static GamePadState GamepadState
        {
            get { return gamepadState; }
            set { gamepadState = value; }
        }

        public static KeyboardState KeyboardState
        {
            get { return keyboardState; }
            set { keyboardState = value; }
        }

        public static KeyboardState PrevKeyboardState
        {
            get { return prevKeyboardState; }
            set { prevKeyboardState = value; }
        }

        public static MouseState MouseState
        {
            get { return mouseState; }
            set { mouseState = value; }
        }

        public static MouseState PrevMouseState
        {
            get { return InputHandler.prevMouseState; }
            set { InputHandler.prevMouseState = value; }
        }
        #endregion

        #region Keyboard Methods

        public static bool KeyReleased(Keys key)
        {
            return keyboardState.IsKeyUp(key) &&
                prevKeyboardState.IsKeyDown(key);
        }

        public static bool KeyPressed(Keys key)
        {

            return keyboardState.IsKeyDown(key) &&
                prevKeyboardState.IsKeyUp(key);
        }

        public static bool KeyDown(Keys key)
        {
            return keyboardState.IsKeyDown(key);
        }
        #endregion

        #region GamePad Methods

        public static bool ButtonReleased(Buttons button)
        {
            return gamepadState.IsButtonUp(button) &&
                PrevGamepadState.IsButtonDown(button);
        }

        public static bool ButtonPressed(Buttons button)
        {
            return gamepadState.IsButtonDown(button) &&
                prevGamepadState.IsButtonUp(button);
        }

        public static bool ButtonDown(Buttons button)
        {
            return gamepadState.IsButtonDown(button);
        }

        public static bool JoystickUped()
        {
            return gamepadState.ThumbSticks.Left.Y > 0.5 &&
                prevGamepadState.ThumbSticks.Left.Y < 0.5;
        }

        public static bool JoystickDowned()
        {
            return gamepadState.ThumbSticks.Left.Y < -0.5 &&
                prevGamepadState.ThumbSticks.Left.Y > -0.5;
        }
        public static bool JoystickRighted()
        {
            return gamepadState.ThumbSticks.Left.X > 0.5 &&
                prevGamepadState.ThumbSticks.Left.X < 0.5;
        }

        public static bool JoystickLefted()
        {
            return gamepadState.ThumbSticks.Left.X < -0.5 &&
                prevGamepadState.ThumbSticks.Left.X > -0.5;
        }
        public static Vector2 LeftThumbStickMovement()
        {
            return new Vector2(gamepadState.ThumbSticks.Left.X, gamepadState.ThumbSticks.Left.Y);
        }

        #endregion

        #region Mouse Methods

        public static bool LeftMouseClicked()
        {
            return mouseState.LeftButton == ButtonState.Pressed
                && prevMouseState.LeftButton == ButtonState.Released;
        }

        public static bool RightMouseClicked()
        {
            return mouseState.RightButton == ButtonState.Pressed
                && prevMouseState.RightButton == ButtonState.Released;
        }

        public static bool LeftMousePressed()
        {
            return mouseState.LeftButton == ButtonState.Pressed;
        }

        public static bool RightMousePressed()
        {
            return mouseState.RightButton == ButtonState.Pressed;
        }

        public static bool MiddleMouseClicked()
        {
            return mouseState.MiddleButton == ButtonState.Pressed
                && prevMouseState.MiddleButton == ButtonState.Released;
        }
        #endregion


        /// <summary>
        /// reset keyboard State and the Gamepad state
        /// </summary>
        public static void Flush()
        {
            prevKeyboardState = keyboardState;
            prevGamepadState = GamepadState;
            prevMouseState = mouseState;
        }

    }
}
