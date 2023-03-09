using MonoGame.Extended;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using MonoGame.Extended.Tiled;

namespace Game_Demo
{
    public class Transition
    {
        public static Rectangle homeTransition = new();
        public static Rectangle villageTransition = new();
        public static Rectangle forestTransition = new();
        public static Rectangle cityTransition = new();
        public static Rectangle forestPath1Transition = new();
        public static Rectangle forestPath2Transition = new();
        public static Rectangle middleVillageTransition = new();


        public static List<Transition> transitions = new();
        public string Name;
        public RectangleF Bounds;

        public static Vector2 Position = Vector2.Zero;

        public Transition()
        {
            Name = "Null";
            Bounds = new RectangleF();
        }

        public Transition(string name, RectangleF bounds)
        {
            Name = name;
            Bounds = bounds;
        }

        public static void LoadTransition()
        {
            Position = Vector2.Zero;
            transitions.Clear();
            if (Tiled._tiledMap.ObjectLayers.Count > 0)
            {
                foreach (TiledMapObject mapObject in Tiled._tiledMap.GetLayer<TiledMapObjectLayer>("Transitions").Objects)
                {
                    transitions.Add(new Transition(mapObject.Name, new RectangleF(mapObject.Position, mapObject.Size)));
                }

                if (Tiled.map == 1) //home map
                {
                    villageTransition = (Rectangle)transitions.Find((Transition s) => { return s.Name == "ToVillage"; }).Bounds;
                }
                if (Tiled.map == 2) //village map
                {
                    homeTransition = (Rectangle)transitions.Find((Transition s) => { return s.Name == "ToHome"; }).Bounds;
                    forestTransition = (Rectangle)transitions.Find((Transition s) => { return s.Name == "ToForest"; }).Bounds;
                }
                if (Tiled.map == 3) //forest map
                {
                    villageTransition = (Rectangle)transitions.Find((Transition s) => { return s.Name == "ToVillage"; }).Bounds;
                    cityTransition = (Rectangle)transitions.Find((Transition s) => { return s.Name == "ToCity"; }).Bounds;
                }
                if (Tiled.map == 4) //city map
                {
                    forestTransition = (Rectangle)transitions.Find((Transition s) => { return s.Name == "ToForest"; }).Bounds;
                    forestPath2Transition = (Rectangle)transitions.Find((Transition s) => { return s.Name == "ToForestPath2"; }).Bounds;
                }
                if (Tiled.map == 5) //forestpath1 map
                {
                    middleVillageTransition = (Rectangle)transitions.Find((Transition s) => { return s.Name == "ToMiddleVillage"; }).Bounds;
                }
                if (Tiled.map == 6) //forestpath2 map
                {
                    middleVillageTransition = (Rectangle)transitions.Find((Transition s) => { return s.Name == "ToMiddleVillage"; }).Bounds;
                    cityTransition = (Rectangle)transitions.Find((Transition s) => { return s.Name == "ToCity"; }).Bounds;
                }
                if (Tiled.map == 7) //middlevillage map
                {
                    forestPath2Transition = (Rectangle)transitions.Find((Transition s) => { return s.Name == "ToForestPath2"; }).Bounds;
                    forestPath1Transition = (Rectangle)transitions.Find((Transition s) => { return s.Name == "ToForestPath1"; }).Bounds;
                }
            }
        }
        public static void TransitionCheck()
        {
            if (Tiled._tiledMap.ObjectLayers.Count > 0 && Collision.CollisionCheck() == Color.Green)
            {
                if (Tiled.map == 1) //home map
                {
                    if (Collision.hitbox.Intersects(villageTransition) && Input.Hold() == "down")
                    {
                        Position = new Vector2(190,240); //village position at house door
                        Game1.SwitchVillage = true;
                    }
                }
                if (Tiled.map == 2) //village map
                {
                    if (Collision.hitbox.Intersects(homeTransition) && Input.Hold() == "up")
                    {
                        Position = new Vector2(195, 241); //home position near door
                        Game1.SwitchHome = true;
                    }
                    if (Collision.hitbox.Intersects(forestTransition) && Input.Hold() == "right")
                    {
                        Position = new Vector2(0, 193); //forest position at far left
                        Game1.SwitchForest = true;
                    }
                }
                if (Tiled.map == 3) //forest map
                {
                    if (Collision.hitbox.Intersects(villageTransition) && Input.Hold() == "left")
                    {
                        Position = new Vector2(769, 384); //village position at far right
                        Game1.SwitchVillage = true;
                    }
                    if (Collision.hitbox.Intersects(cityTransition) && Input.Hold() == "right")
                    {
                        Position = new Vector2(867, 2005); //city position at bottom
                        Game1.SwitchCity = true;
                    }
                }
                if (Tiled.map == 4) //city map
                {
                    if (Collision.hitbox.Intersects(forestTransition) && Input.Hold() == "down")
                    {
                        Position = new Vector2(912, 172); //forest position at bottom
                        Game1.SwitchForest = true;
                    }
                    if (Collision.hitbox.Intersects(forestPath2Transition) && Input.Hold() == "right")
                    {
                        Position = new Vector2(100, 480); //forestpath2 position at far right
                        Game1.SwitchForestPath2 = true; 
                    }
                }
                if (Tiled.map == 5) //forestpath1 map
                {
                    if (Collision.hitbox.Intersects(middleVillageTransition) && Input.Hold() == "left")
                    {
                        Position = new Vector2(1968, 192); //middlevillage position at left
                        Game1.SwitchMiddleVillage = true;
                    }
                }
                if (Tiled.map == 6) //forestpath2 map
                {
                    if (Collision.hitbox.Intersects(cityTransition) && Input.Hold() == "left")
                    {
                        Position = new Vector2(1968, 384); //city position at left
                        Game1.SwitchCity = true;
                    }
                    if (Collision.hitbox.Intersects(middleVillageTransition) && Input.Hold() == "right")
                    {
                        Position = new Vector2(100, 864); //middlevillage position at far right
                        Game1.SwitchMiddleVillage = true;
                    }
                }
                if (Tiled.map == 7) //middlevillage map
                {
                    if (Collision.hitbox.Intersects(forestPath1Transition) && Input.Hold() == "right")
                    {
                        Position = new Vector2(100, 480); //forestpath1 position at left
                        Game1.SwitchForestPath1 = true;
                    }
                    if (Collision.hitbox.Intersects(forestPath2Transition) && Input.Hold() == "left")
                    {
                        Position = new Vector2(3264, 480); //forestpath2 position at far right
                        Game1.SwitchForestPath2 = true;
                    }
                }
            }
        }
    }
}
