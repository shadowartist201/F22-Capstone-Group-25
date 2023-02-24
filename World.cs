using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Collisions;
using MonoGame.Extended;

namespace Game_Demo
{
    public class World
    {
        public static Texture2D player;
        public static float movementSpeed = 150;

        public static Vector2 Movement()
        {
            return Input.Direction() switch
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
