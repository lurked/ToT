using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ToT
{
    public class InputManager
    {
        KeyboardState prevKeyState, keyState;
        GamePadState prevButtonState, buttonState;
        MouseState prevMouseState, mouseState;
        PlayerIndex playerIndex = 0;

        public KeyboardState PrevKeyState
        {
            get { return prevKeyState; }
            set { prevKeyState = value; }
        }
        public KeyboardState KeyState
        {
            get { return KeyState; }
            set { KeyState = value; }
        }

        public void Update()
        {
            prevKeyState = keyState;
            keyState = Keyboard.GetState();
            prevButtonState = buttonState;
            buttonState = GamePad.GetState(playerIndex);
            prevMouseState = mouseState;
            mouseState = Mouse.GetState();
        }

        public bool MouseDown()
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
                return true;
            return false;
        }

        public bool MouseRightDown()
        {
            if (mouseState.RightButton == ButtonState.Pressed)
                return true;
            return false;
        }

        public bool MousePressed()
        {
            if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                return true;
            return false;
        }

        public bool MouseRightPressed()
        {
            if (mouseState.RightButton == ButtonState.Pressed && prevMouseState.RightButton == ButtonState.Released)
                return true;
            return false;
        }

        public Vector2 MousePosition()
        {
            return new Vector2(mouseState.Position.X, mouseState.Position.Y);
        }

        public bool KeyPressed(Keys key)
        {
            if (keyState.IsKeyDown(key) && prevKeyState.IsKeyUp(key))
                return true;
            return false;
        }

        public bool KeyPressed(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (keyState.IsKeyDown(key) && prevKeyState.IsKeyUp(key))
                    return true;
            }
            return false;
        }

        public bool ButtonPressed(Buttons button)
        {
            if (buttonState.IsButtonDown(button) && prevButtonState.IsButtonUp(button))
                return true;
            return false;
        }

        public bool ButtonPressed(params Buttons[] buttons)
        {
            foreach (Buttons button in buttons)
            {
                if (buttonState.IsButtonDown(button) && prevButtonState.IsButtonUp(button))
                    return true;
            }
            return false;
        }


        public bool KeyReleased(Keys key)
        {
            if (keyState.IsKeyDown(key) && prevKeyState.IsKeyUp(key))
                return true;
            return false;
        }

        public bool KeyReleased(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (keyState.IsKeyDown(key) && prevKeyState.IsKeyUp(key))
                    return true;
            }
            return false;
        }

        public bool KeyDown(Keys key)
        {
            if (keyState.IsKeyDown(key))
                return true;
            return false;

        }
        public bool ButtonDown(Buttons button)
        {
            if (buttonState.IsButtonDown(button))
                return true;
            return false;
        }

        public bool KeyDown(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (keyState.IsKeyDown(key))
                    return true;
            }
            return false;
        }

        public bool ButtonDown(params Buttons[] buttons)
        {
            foreach (Buttons button in buttons)
            {
                if (buttonState.IsButtonDown(button))
                    return true;
            }
            return false;
        }
    }
}
