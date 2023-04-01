using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Graphics;
using System.Diagnostics;
using System;

namespace Game_Demo
{
    internal class Collision
    {
        public static Color color;
        public static Rectangle hitbox;
        public static Rectangle entitybox;
        private static TiledMapTile previous_tile = new();

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
            hitbox = new((int)Tiled.currentPosition.X, (int)Tiled.currentPosition.Y, 48, 48);

            TiledMapTile top_tile = new();
            TiledMapTile bottom_tile = new();
            TiledMapTile left_tile = new();
            TiledMapTile right_tile = new();

            // Get next tile based on player position
            if (hitbox.Top >= 0)
                top_tile = Tiled.collision.GetTile((ushort)(hitbox.Center.X / Tiled.tileWidth), (ushort)(hitbox.Top / Tiled.tileWidth));
            if (hitbox.Bottom <= Tiled._tiledMap.HeightInPixels-1)
                bottom_tile = Tiled.collision.GetTile((ushort)(hitbox.Center.X / Tiled.tileWidth), (ushort)(hitbox.Bottom / Tiled.tileWidth));
            if (hitbox.Left >= 0)
                left_tile = Tiled.collision.GetTile((ushort)(hitbox.Left / Tiled.tileWidth), (ushort)(hitbox.Center.Y / Tiled.tileWidth));
            if (hitbox.Right <= Tiled._tiledMap.WidthInPixels-1)
                right_tile = Tiled.collision.GetTile((ushort)(hitbox.Right / Tiled.tileWidth), (ushort)(hitbox.Center.Y / Tiled.tileWidth));

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

        public static Color CollisionCheck_Entity(EntityTest entity)
        {
            hitbox = new((int)Tiled.currentPosition.X, (int)Tiled.currentPosition.Y, 48, 48);
            entitybox = new((int)entity.position.X, (int)entity.position.Y, 48, 48);

            if (hitbox.Intersects(entitybox))
            {
                color = Color.Blue;
                if (hitbox.Left < entitybox.Right && entitybox.Right - hitbox.Left < 10 && Input.Hold() == "left")
                    color = Color.Green;
                if (hitbox.Right > entitybox.Left && hitbox.Right - entitybox.Left < 10 && Input.Hold() == "right")
                    color = Color.Green;
                if (hitbox.Top < entitybox.Bottom && entitybox.Bottom - hitbox.Top < 10 && Input.Hold() == "up")
                    color = Color.Green;
                if (hitbox.Bottom > entitybox.Top && hitbox.Bottom - entitybox.Top < 10 && Input.Hold() == "down")
                    color = Color.Green;
                return color;
            }
            else
            {
                color = Color.White;
                return color;
            }
        }

        public static void RandomBattle()
        {
            hitbox = new((int)Tiled.currentPosition.X, (int)Tiled.currentPosition.Y, 48, 48);
            int num = 0;
            var random = new Random();
            TiledMapTile current_tile = Tiled.grass.GetTile((ushort)(hitbox.Center.X / Tiled.tileWidth), (ushort)(hitbox.Center.Y / Tiled.tileWidth));

            if (!current_tile.Equals(previous_tile))
            {
                num = random.Next(1, 11); //between 1 and 10
                Debug.WriteLine("Random Battle Chance: " + num);
                if (num < 3) //20% chance
                {
                    Game1.SwitchBattle = true;
                }
            }

            previous_tile = current_tile;
        }
    }
}
