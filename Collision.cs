using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled;
using System.Diagnostics;

namespace Game_Demo
{
    internal class Collision
    {
        public static Color color;

        private static bool OutOfBounds(Rectangle hitbox)
        {
            if (hitbox.Left < 0 || hitbox.Top < 0 || hitbox.Right > Tiled._tiledMap.WidthInPixels-1 || hitbox.Bottom > Tiled._tiledMap.HeightInPixels-1) 
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Color CollisionCheck()
        {
            Rectangle hitbox = new((int)Tiled.currentPosition.X, (int)Tiled.currentPosition.Y, 48, 48);
            TiledMapTile top_tile = new();
            TiledMapTile bottom_tile = new();
            TiledMapTile right_tile = new();
            TiledMapTile left_tile = new();

            // Get next tile based on player position
            if (!OutOfBounds(hitbox))
            {
                top_tile = Tiled.collision.GetTile((ushort)(hitbox.Center.X / Tiled.tileWidth), (ushort)(hitbox.Top / Tiled.tileWidth));
                bottom_tile = Tiled.collision.GetTile((ushort)(hitbox.Center.X / Tiled.tileWidth), (ushort)(hitbox.Bottom / Tiled.tileWidth));
                left_tile = Tiled.collision.GetTile((ushort)(hitbox.Left / Tiled.tileWidth), (ushort)(hitbox.Center.Y / Tiled.tileWidth));
                right_tile = Tiled.collision.GetTile((ushort)(hitbox.Right / Tiled.tileWidth), (ushort)(hitbox.Center.Y / Tiled.tileWidth));
            }

            if (!top_tile.IsBlank || !bottom_tile.IsBlank || !left_tile.IsBlank || !right_tile.IsBlank || OutOfBounds(hitbox)) //if collided
            {
                color = Color.Blue;
                if ((!left_tile.IsBlank || hitbox.Left < 0) && Input.Hold() == "left")
                    color = Color.Green;
                if ((!right_tile.IsBlank || hitbox.Right > Tiled._tiledMap.WidthInPixels-1) && Input.Hold() == "right")
                    color = Color.Green;
                if ((!top_tile.IsBlank || hitbox.Top < 0) && Input.Hold() == "up")
                    color = Color.Green;
                if ((!bottom_tile.IsBlank || hitbox.Bottom > Tiled._tiledMap.HeightInPixels-1) && Input.Hold() == "down")
                    color = Color.Green;
                return color;
            }
            else //not collided
            {
                color = Color.White;
                return color;
            }
        }
    }
}
