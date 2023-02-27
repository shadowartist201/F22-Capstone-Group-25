using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.Tiled;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game_Demo
{
    internal class Tiled : Game
    {
        public static TiledMap _tiledMap;
        public static TiledMapRenderer _tiledMapRenderer;
        public static TiledMapTileLayer collision;
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

            mapEffect = new TiledMapEffect(content.Load<Effect>("MapEffect")); //load highlight effect (just in case)

            switch (tilemap) //switch based on map name
            {
                case "home":
                    if (!BattleReturn) //if normal
                        startingPosition = new Vector2(_tiledMap.WidthInPixels / 2, _tiledMap.HeightInPixels / 2);
                    else //if returned from battle
                        startingPosition = currentPosition;
                        BattleReturn = false;
                    break;
                case "village":
                    if (!BattleReturn)
                        startingPosition = new Vector2(190, 240);
                    else
                        startingPosition = currentPosition;
                        BattleReturn = false;
                    break;
                case "forest":
                    if (!BattleReturn)
                        startingPosition = new Vector2(95, 195);
                    else
                        startingPosition = currentPosition;
                        BattleReturn = false;
                    break;
                case "city":
                    if (!BattleReturn)
                        startingPosition = new Vector2(900, 1880);
                    else
                        startingPosition = currentPosition;
                        BattleReturn = false;
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
            //_tiledMapRenderer.Draw(collision, _camera.GetViewMatrix(), mapEffect); //draw map with highlighted collision layer
        }
    }
}
