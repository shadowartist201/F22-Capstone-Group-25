﻿using Microsoft.Xna.Framework.Input;

namespace Game_Demo
{
    public class Input
    {
        public static bool up, down, left, right, enter, back, o, x, tab; //flags for single press
        public static string Hold() //constant press
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
            if (state.IsKeyDown(Keys.O))
            {
                return "o";
            }
            if (state.IsKeyDown(Keys.X))
            {
                return "x";
            }
            if (state.IsKeyDown(Keys.Tab))
            {
                return "tab";
            }
            else
            {
                return "none";
            }
        }

        public static string SinglePress() //only one press
        {
            KeyboardState state = Keyboard.GetState();
            string output = "none";

            if (state.IsKeyDown(Keys.Up) && !up) //if key pressed and flag = false
            {
                up = true; //set flag to true
                output = "up";
            }
            if (state.IsKeyUp(Keys.Up) && up) //if key not pressed and flag = true
                up = false; //set flag to false
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
            if (state.IsKeyDown(Keys.O) && !o)
            {
                back = true;
                output = "o";
            }
            if (state.IsKeyUp(Keys.O) && o)
                back = false;
            //---------------------------------------
            if (state.IsKeyDown(Keys.X) && !x)
            {
                back = true;
                output = "x";
            }
            if (state.IsKeyUp(Keys.X) && x)
                back = false;
            //---------------------------------------
            if (state.IsKeyDown(Keys.Tab) && !tab)
            {
                tab = true;
                output = "tab";
            }
            if (state.IsKeyUp(Keys.Tab) && tab)
                tab = false;
            //---------------------------------------
            return output;
        }
    }
}
