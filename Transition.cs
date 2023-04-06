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
        public static Rectangle cityCastleTransition = new();
        public static Rectangle cityBarInnTransition = new();
        public static Rectangle cityBarTransition = new();
        public static Rectangle cityPotionTransition = new();
        public static Rectangle cityEquipTransition = new();
        public static Rectangle middleVillagePotionTransition = new();
        public static Rectangle middleVillageEquipTransition = new();
        public static Rectangle mountianEntranceTransition = new();


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
                    cityCastleTransition = (Rectangle)transitions.Find((Transition s) => { return s.Name == "ToCity_Castle"; }).Bounds;
                    cityBarTransition = (Rectangle)transitions.Find((Transition s) => { return s.Name == "ToCity_Bar"; }).Bounds;
                    cityPotionTransition = (Rectangle)transitions.Find((Transition s) => { return s.Name == "ToCity_PotionShop"; }).Bounds;
                    cityEquipTransition = (Rectangle)transitions.Find((Transition s) => { return s.Name == "ToCity_EquipShop"; }).Bounds;
                }
                if (Tiled.map == 5) //forestpath1 map
                {
                    middleVillageTransition = (Rectangle)transitions.Find((Transition s) => { return s.Name == "ToMiddleVillage"; }).Bounds;
                    mountianEntranceTransition = (Rectangle)transitions.Find((Transition s) => { return s.Name == "ToMountianEntrance"; }).Bounds;
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
                    middleVillagePotionTransition = (Rectangle)transitions.Find((Transition s) => { return s.Name == "ToVillage2_PotionsShop"; }).Bounds;
                    middleVillageEquipTransition = (Rectangle)transitions.Find((Transition s) => { return s.Name == "ToVillage2_EquipShop"; }).Bounds;
                }
                if (Tiled.map == 8) //city castle map
                {
                    cityTransition = (Rectangle)transitions.Find((Transition s) => { return s.Name == "ToCity"; }).Bounds;
                }
                if (Tiled.map == 9) //city equiptment map
                {
                    cityTransition = (Rectangle)transitions.Find((Transition s) => { return s.Name == "ToCity"; }).Bounds;
                }
                if (Tiled.map == 10) //city potion map
                {
                    cityTransition = (Rectangle)transitions.Find((Transition s) => { return s.Name == "ToCity"; }).Bounds;
                }
                if (Tiled.map == 11) //city bar map
                {
                    cityTransition = (Rectangle)transitions.Find((Transition s) => { return s.Name == "ToCity"; }).Bounds;
                    cityBarInnTransition = (Rectangle)transitions.Find((Transition s) => { return s.Name == "ToCity_Bar_Inn"; }).Bounds;
                }
                if (Tiled.map == 12) //city inn map
                {
                    cityBarTransition = (Rectangle)transitions.Find((Transition s) => { return s.Name == "ToCity_Bar"; }).Bounds;
                }
                if (Tiled.map == 13) //mountian entrance map
                {
                    forestPath1Transition = (Rectangle)transitions.Find((Transition s) => { return s.Name == "ToForestPath1"; }).Bounds;
                }
                if (Tiled.map == 14) //middle village equiptment map
                {
                    middleVillageTransition = (Rectangle)transitions.Find((Transition s) => { return s.Name == "ToMiddleVillage"; }).Bounds;
                }
                if (Tiled.map == 15) //middle village potions map
                {
                    middleVillageTransition = (Rectangle)transitions.Find((Transition s) => { return s.Name == "ToMiddleVillage"; }).Bounds;
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
                    if (Collision.hitbox.Intersects(cityCastleTransition) && Input.Hold() == "up")
                    {
                        Position = new Vector2(960, 1056); //citycastle position at bottom middle of 'rug'
                        Game1.SwitchCityCastle = true;
                    }
                    if (Collision.hitbox.Intersects(cityEquipTransition) && Input.Hold() == "up")
                    {
                        Position = new Vector2(240, 336); //city equiptment shop
                        Game1.SwitchCity_EquipShop = true;
                    }
                    if (Collision.hitbox.Intersects(cityPotionTransition) && Input.Hold() == "up")
                    {
                        Position = new Vector2(240, 336); //city potion shop
                        Game1.SwitchCity_PotionShop = true;
                    }
                    if (Collision.hitbox.Intersects(cityBarTransition) && Input.Hold() == "up")
                    {
                        Position = new Vector2(114, 576); //city bar
                        Game1.SwitchCity_Bar = true;
                    }
                }
                if (Tiled.map == 5) //forestpath1 map
                {
                    if (Collision.hitbox.Intersects(middleVillageTransition) && Input.Hold() == "left")
                    {
                        Position = new Vector2(1968, 192); //middlevillage position at right
                        Game1.SwitchMiddleVillage = true;
                    }
                    if (Collision.hitbox.Intersects(mountianEntranceTransition) && Input.Hold() == "right")
                    {
                        Position = new Vector2(96, 1680); //mountian entrance position at bottom left
                        Game1.SwitchMountianEntrance = true;
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
                    if (Collision.hitbox.Intersects(middleVillageEquipTransition) && Input.Hold() == "up")
                    {
                        Position = new Vector2(336, 336); //middle village equiptment shop
                        Game1.SwitchVillage2_EquipShop = true;
                    }
                    if (Collision.hitbox.Intersects(middleVillagePotionTransition) && Input.Hold() == "up")
                    {
                        Position = new Vector2(336, 336); //middle village potion shop
                        Game1.SwitchVillage2_PotionsShop = true;
                    }
                }
                if (Tiled.map == 8) //city castle map
                {
                    if (Collision.hitbox.Intersects(cityTransition) && Input.Hold() == "down")
                    {
                        Position = new Vector2(648, 432); //city position just below castle
                        Game1.SwitchCity = true;
                    }
                }
                if (Tiled.map == 9) //city Equiptment Shop map
                {
                    if (Collision.hitbox.Intersects(cityTransition) && Input.Hold() == "down")
                    {
                        Position = new Vector2(1488, 336); //city position below equiptment shop
                        Game1.SwitchCity = true;
                    }
                }
                if (Tiled.map == 10) //city Potion shop map
                {
                    if (Collision.hitbox.Intersects(cityTransition) && Input.Hold() == "down")
                    {
                        Position = new Vector2(1344, 1824); //city position to the left of the potion shop
                        Game1.SwitchCity = true;
                    }
                }
                if (Tiled.map == 11) //city bar map
                {
                    if (Collision.hitbox.Intersects(cityTransition) && Input.Hold() == "down")
                    {
                        Position = new Vector2(1728, 1392); //city position to the left of the bar
                        Game1.SwitchCity = true;
                    }
                    if (Collision.hitbox.Intersects(cityBarInnTransition) && Input.Hold() == "up") 
                    {
                        Position = new Vector2(336, 144); //city bar inn position
                        Game1.SwitchCity_Bar_Inn = true;
                    }
                }
                if (Tiled.map == 12) //city bar inn map
                {
                    if (Collision.hitbox.Intersects(cityBarTransition) && Input.Hold() == "down")
                    {
                        Position = new Vector2(336, 384); //city bar position from inn
                        Game1.SwitchCity_Bar = true;
                    }
                }
                if (Tiled.map == 13) //mountian entrance map
                {
                    if (Collision.hitbox.Intersects(forestPath1Transition) && Input.Hold() == "left")
                    {
                        Position = new Vector2(3120, 480); //forestpath1 position before grass on right side
                        Game1.SwitchForestPath1 = true;
                    }
                }
                if (Tiled.map == 14) //village 2 equiptment shop map
                {
                    if (Collision.hitbox.Intersects(middleVillageTransition) && Input.Hold() == "down")
                    {
                        Position = new Vector2(432, 1392); //middle village position to the right of equiptment shop
                        Game1.SwitchMiddleVillage = true;
                    }
                }
                if (Tiled.map == 15) //village 2 potion shop map
                {
                    if (Collision.hitbox.Intersects(middleVillageTransition) && Input.Hold() == "down")
                    {
                        Position = new Vector2(720, 1392); //middle village position to the left of equiptment shop
                        Game1.SwitchMiddleVillage = true;
                    }
                }
            }
        }
    }
}
