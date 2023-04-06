using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.Tiled;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Game_Demo
{
    internal class Tiled : Game
    {
        public static TiledMap _tiledMap;
        public static TiledMapRenderer _tiledMapRenderer;
        public static TiledMapTileLayer collision;
        public static TiledMapTileLayer grass;
        public static TiledMapTile? tile = null;

        public static int map = 0; //map index
        public static int tileWidth = 48;
        public static Vector2 startingPosition = Vector2.Zero; //player and camera starting position
        public static Vector2 currentPosition; //player and camera current position

        public static TiledMapEffect mapEffect; //map highlight

        public static bool BattleReturn; //"returned from battle" flag

        public static void LoadMap(string tilemap, ContentManager content, GraphicsDevice graphicsDevice)
        {
            _tiledMap = content.Load<TiledMap>("Maps/" + tilemap);   //load the tilemap
            _tiledMapRenderer = new TiledMapRenderer(graphicsDevice, _tiledMap);
            collision = _tiledMap.GetLayer<TiledMapTileLayer>("Collision");  //load collision layer
            grass = _tiledMap.GetLayer<TiledMapTileLayer>("Grass");

            //mapEffect = new TiledMapEffect(content.Load<Effect>("MapEffect")); //load highlight effect (just in case)
            byte[] bytecode = File.ReadAllBytes("Content/mapEffect2.mgfx");
            mapEffect = new TiledMapEffect(new Effect(graphicsDevice, bytecode));

            switch (tilemap) //switch based on map name
            {
                case "home":
                    if (Transition.Position != Vector2.Zero)
                        startingPosition = Transition.Position;
                    else if (BattleReturn) //if returned from battle
                    {
                        startingPosition = currentPosition;
                        BattleReturn = false;
                    }
                    else
                        startingPosition = new Vector2(_tiledMap.WidthInPixels / 2, _tiledMap.HeightInPixels / 2);
                    break;

                case "village":
                    if (Transition.Position != Vector2.Zero)
                        startingPosition = Transition.Position;
                    else if (BattleReturn) 
                    {
                        startingPosition = currentPosition;
                        BattleReturn = false;
                    }
                    else
                        startingPosition = new Vector2(190, 240);
                    break;

                case "forest":
                    if (Transition.Position != Vector2.Zero)
                        startingPosition = Transition.Position;
                    else if (BattleReturn) 
                    {
                        startingPosition = currentPosition;
                        BattleReturn = false;
                    }
                    else
                        startingPosition = new Vector2(95, 195);
                    break;

                case "city":
                    if (Transition.Position != Vector2.Zero)
                        startingPosition = Transition.Position;
                    else if (BattleReturn)
                    {
                        startingPosition = currentPosition;
                        BattleReturn = false;
                    }
                    else
                        startingPosition = new Vector2(900, 1880);
                    break;

                case "forestpath1":
                    if (Transition.Position != Vector2.Zero)
                        startingPosition = Transition.Position;
                    else if (BattleReturn)
                    {
                        startingPosition = currentPosition;
                        BattleReturn = false;
                    }
                    else
                        startingPosition = new Vector2(100, 480);
                    break;
                    
                case "forestpath2":
                    if (Transition.Position != Vector2.Zero)
                        startingPosition = Transition.Position;
                    else if (BattleReturn)
                    {
                        startingPosition = currentPosition;
                        BattleReturn = false;
                    }
                    else
                        startingPosition = new Vector2(100, 480);
                    break;

                case "middlevillage":
                    if (Transition.Position != Vector2.Zero)
                        startingPosition = Transition.Position;
                    else if (BattleReturn)
                    {
                        startingPosition = currentPosition;
                        BattleReturn = false;
                    }
                    else
                        startingPosition = new Vector2(96, 864);
                    break;

                case "City_Castle":
                    if (Transition.Position != Vector2.Zero)
                        startingPosition = Transition.Position;
                    else if (BattleReturn)
                    {
                        startingPosition = currentPosition;
                        BattleReturn = false;
                    }
                    else
                        startingPosition = new Vector2(960, 1056);
                    break;

                case "City_EquipShop":
                    if (Transition.Position != Vector2.Zero)
                        startingPosition = Transition.Position;
                    else if (BattleReturn)
                    {
                        startingPosition = currentPosition;
                        BattleReturn = false;
                    }
                    else
                        startingPosition = new Vector2(336, 336);
                    break;

                case "City_PotionShop":
                    if (Transition.Position != Vector2.Zero)
                        startingPosition = Transition.Position;
                    else if (BattleReturn)
                    {
                        startingPosition = currentPosition;
                        BattleReturn = false;
                    }
                    else
                        startingPosition = new Vector2(336, 336);
                    break;

                case "City_Bar":
                    if (Transition.Position != Vector2.Zero)
                        startingPosition = Transition.Position;
                    else if (BattleReturn)
                    {
                        startingPosition = currentPosition;
                        BattleReturn = false;
                    }
                    else
                        startingPosition = new Vector2(114, 576);
                    break;

                case "City_Bar_Inn":
                    if (Transition.Position != Vector2.Zero)
                        startingPosition = Transition.Position;
                    else if (BattleReturn)
                    {
                        startingPosition = currentPosition;
                        BattleReturn = false;
                    }
                    else
                        startingPosition = new Vector2(336, 144);
                    break;

                case "MountianEntrance":
                    if (Transition.Position != Vector2.Zero)
                        startingPosition = Transition.Position;
                    else if (BattleReturn)
                    {
                        startingPosition = currentPosition;
                        BattleReturn = false;
                    }
                    else
                        startingPosition = new Vector2(96, 1680);
                    break;

                case "Village2_EquipShop":
                    if (Transition.Position != Vector2.Zero)
                        startingPosition = Transition.Position;
                    else if (BattleReturn)
                    {
                        startingPosition = currentPosition;
                        BattleReturn = false;
                    }
                    else
                        startingPosition = new Vector2(336, 336);
                    break;

                case "Village2_PotionsShop":
                    if (Transition.Position != Vector2.Zero)
                        startingPosition = Transition.Position;
                    else if (BattleReturn)
                    {
                        startingPosition = currentPosition;
                        BattleReturn = false;
                    }
                    else
                        startingPosition = new Vector2(336, 336);
                    break;

                default:
                    break;
            }
        }

        public static void Update_(GameTime gameTime)
        {
            _tiledMapRenderer.Update(gameTime);
        }

        public static void Draw(OrthographicCamera _camera)
        {
            _tiledMapRenderer.Draw(_camera.GetViewMatrix()); //draw the tile map
            //_tiledMapRenderer.Draw(collision, _camera.GetViewMatrix(), null, mapEffect); //draw map with highlighted collision layer
        }
    }
}
