using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.Screens;
using System.Diagnostics;

namespace Game_Demo
{
    public class City : GameScreen
    {
        private Game1 game => (Game1)base.Game;
        public City(Game1 game) : base(game) { }

        public SpriteBatch _spriteBatch;

        public Texture2D player;    //player texture

        public TiledMap _tiledMap;
        public TiledMapRenderer _tiledMapRenderer;
        public TiledMapTileLayer collision;
        public TiledMapTile? tile = null;
        public OrthographicCamera _camera;

        private Vector2 movementDirection;

        int tileWidth = 48;  //48x48 pixels
        ushort tileIndex_X;
        ushort tileIndex_Y;

        public override void LoadContent()
        {
            player = Content.Load<Texture2D>("World/player");    //load the player texture
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _camera = new OrthographicCamera(GraphicsDevice);

            _tiledMap = Content.Load<TiledMap>("Maps/city");   //load the tilemap
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
            collision = _tiledMap.GetLayer<TiledMapTileLayer>("Collision");  //load collision layer

            _camera.LookAt(new Vector2(900, 1880)); //starting position

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            _tiledMapRenderer.Update(gameTime);

            KeyboardState state = Keyboard.GetState();
            movementDirection = Vector2.Zero;

            if (state.IsKeyDown(Keys.Up))
            {
                collision.TryGetTile(tileIndex_X, tileIndex_Y, out tile); //grab collision value of tile up
                if (tile.ToString() == "GlobalIdentifier: 0, Flags: None") //if tile up is free
                    movementDirection.Y -= 1;
                else
                    movementDirection.Y += 1;
            }
            if (state.IsKeyDown(Keys.Down))
            {
                collision.TryGetTile(tileIndex_X, tileIndex_Y, out tile); //grab collision value of tile down
                if (tile.ToString() == "GlobalIdentifier: 0, Flags: None") //if tile down is free
                    movementDirection.Y += 1;
                else
                    movementDirection.Y -= 1;
            }
            if (state.IsKeyDown(Keys.Left))
            {
                collision.TryGetTile((ushort)(tileIndex_X - 1), tileIndex_Y, out tile); //grab collision value of tile left
                if (tile.ToString() == "GlobalIdentifier: 0, Flags: None") //if tile left is free
                    movementDirection.X -= 1;
                else
                    movementDirection.X += 1;
            }
            if (state.IsKeyDown(Keys.Right))
            {
                collision.TryGetTile((ushort)(tileIndex_X + 1), tileIndex_Y, out tile); //grab collision value of tile right
                if (tile.ToString() == "GlobalIdentifier: 0, Flags: None") //if tile right is free
                    movementDirection.X += 1;
                else
                    movementDirection.X -= 1;
            }

            const float movementSpeed = 150;
            _camera.Move(movementDirection * movementSpeed * gameTime.GetElapsedSeconds());

            tileIndex_X = (ushort)((_camera.Center.X) / tileWidth);  //get current tile based on player position
            tileIndex_Y = (ushort)((_camera.Center.Y) / tileWidth);
        }

        public override void Draw(GameTime gameTime)
        {
            _tiledMapRenderer.Draw(_camera.GetViewMatrix()); //draw the tile map

            var transformMatrix = _camera.GetViewMatrix();
            _spriteBatch.Begin(transformMatrix: transformMatrix);

            _spriteBatch.Draw(player, new Rectangle((int)_camera.Center.X, (int)_camera.Center.Y, 48, 48), Color.White);
            Debug.WriteLine(_camera.Center);

            _spriteBatch.End();
        }
    }
}
