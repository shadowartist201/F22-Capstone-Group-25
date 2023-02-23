using Microsoft.Xna.Framework.Input;

namespace Game_Demo
{
    public class Input
    {
        public static string Direction()
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
            else
            {
                return "none";
            }
        }
    }
}
