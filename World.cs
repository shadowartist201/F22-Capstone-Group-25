using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Demo
{
    public class World
    {
        public static Texture2D player; //overworld player texture
        public static float movementSpeed = 150; //movement speed

        public static Vector2 Movement() //convert keyboard input to Vector2 for camera move
        {
            return Input.Hold() switch
            {
                "up" => new Vector2(0, -1),
                "down" => new Vector2(0, 1),
                "left" => new Vector2(-1, 0),
                "right" => new Vector2(1, 0),
                _ => new Vector2(0, 0),
            };
        }
    }
}
