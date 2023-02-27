using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace Game_Demo
{
    public class Input
    {
        static bool up, down, left, right, enter, back;
        public static string Hold()
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Up))
            {
                return "up";
            }
            if (state.IsKeyDown(Keys.Down))
            {
                return "down";
            }
            if (state.IsKeyDown(Keys.Left))
            {
                return "left";
            }
            if (state.IsKeyDown(Keys.Right))
            {
                return "right";
            }
            if (state.IsKeyDown(Keys.Enter))
            {
                return "enter";
            }
            if (state.IsKeyDown(Keys.Back))
            {
                return "backspace";
            }
            else
            {
                return "none";
            }
        }

        public static string SinglePress()
        {
            KeyboardState state = Keyboard.GetState();
            Debug.WriteLine(enter);
            string output = "none";

            if (state.IsKeyDown(Keys.Up) && !up)
            {
                up = true;
                output = "up";
            }
            if (state.IsKeyUp(Keys.Up) && up)
                up = false;
            //---------------------------------------
            if (state.IsKeyDown(Keys.Down) && !down)
            {
                down = true;
                output = "down";
            }
            if (state.IsKeyUp(Keys.Down) && down)
                down = false;
            //---------------------------------------
            if (state.IsKeyDown(Keys.Left) && !left)
            {
                left = true;
                output = "left";
            }
            if (state.IsKeyUp(Keys.Left) && left)
                left = false;
            //---------------------------------------
            if (state.IsKeyDown(Keys.Right) && !right)
            {
                right = true;
                output = "right";
            }
            if (state.IsKeyUp(Keys.Right) && right)
                right = false;
            //---------------------------------------
            if (state.IsKeyDown(Keys.Enter) && !enter)
            {
                enter = true;
                output = "enter";
            }
            if (state.IsKeyUp(Keys.Enter) && enter)
                enter = false;
            //---------------------------------------
            if (state.IsKeyDown(Keys.Back) && !back)
            {
                back = true;
                output = "backspace";
            }
            if (state.IsKeyUp(Keys.Back) && back)
                back = false;
            //---------------------------------------
            return output;
        }
    }
}
