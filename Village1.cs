using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.Screens;

namespace Game_Demo
{
    public class Village1 : GameScreen
    {
        private Game1 game => (Game1)base.Game;
        public Village1(Game1 game) : base(game) { }

        public SpriteBatch _spriteBatch;

        public Texture2D player;    //player texture
        private Vector2 playerPos; //player position

        public TiledMap _tiledMap;
        public TiledMapRenderer _tiledMapRenderer;
        public TiledMapTileLayer collision;
        public TiledMapTile? tile = null;
        public OrthographicCamera _camera;

        private KeyboardState oldstate;

        private Vector2 _cameraPosition;
        //Default camera position of (0,0) is (400,240) from the top-left edge of the map

        int tileCameraOffset_X;
        int tileCameraOffset_Y;

        int tileWidth = 48;  //48x48 pixels
        ushort tileIndex_X;
        ushort tileIndex_Y;

        public override void LoadContent()
        {
            player = Content.Load<Texture2D>("World/player");    //load the player texture
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _camera = new OrthographicCamera(GraphicsDevice);

            _tiledMap = Content.Load<TiledMap>("Maps/village");   //load the tilemap
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
            collision = _tiledMap.GetLayer<TiledMapTileLayer>("Collision");  //load collision layer

            tileCameraOffset_X = 400 - ((game._graphics.GraphicsDevice.Viewport.Width - _tiledMap.WidthInPixels) / 2);  //camera thing - ((window width - tilemap width) / 2)
            tileCameraOffset_Y = 240 - ((game._graphics.GraphicsDevice.Viewport.Height - _tiledMap.HeightInPixels) / 2); //camera thing - ((window height - tilemap height) / 2)
            _cameraPosition = new Vector2(tileCameraOffset_X, tileCameraOffset_Y);

            playerPos.X = 400 - tileCameraOffset_X + (3 * tileWidth); //camera thing - offset + (tile amount * tile width)
            playerPos.Y = 240 - tileCameraOffset_Y + (3 * tileWidth); //camera thing - offset + (tile amount * tile width)

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            _tiledMapRenderer.Update(gameTime);
            _camera.LookAt(_cameraPosition); //set camera position

            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Up) && !oldstate.IsKeyDown(Keys.Up))
            {
                collision.TryGetTile(tileIndex_X, (ushort)(tileIndex_Y - 1), out tile); //grab collision value of tile up
                if (tile.ToString() == "GlobalIdentifier: 0, Flags: None") //if tile up is free
                    playerPos.Y -= 48; //move up
            }
            if (state.IsKeyDown(Keys.Down) && !oldstate.IsKeyDown(Keys.Down))
            {
                collision.TryGetTile(tileIndex_X, (ushort)(tileIndex_Y + 1), out tile); //grab collision value of tile down
                if (tile.ToString() == "GlobalIdentifier: 0, Flags: None") //if tile down is free
                    playerPos.Y += 48;  //move down
            }
            if (state.IsKeyDown(Keys.Left) && !oldstate.IsKeyDown(Keys.Left))
            {
                collision.TryGetTile((ushort)(tileIndex_X - 1), tileIndex_Y, out tile); //grab collision value of tile left
                if (tile.ToString() == "GlobalIdentifier: 0, Flags: None") //if tile left is free
                    playerPos.X -= 48;  //move left
            }
            if (state.IsKeyDown(Keys.Right) && !oldstate.IsKeyDown(Keys.Right))
            {
                collision.TryGetTile((ushort)(tileIndex_X + 1), tileIndex_Y, out tile); //grab collision value of tile right
                if (tile.ToString() == "GlobalIdentifier: 0, Flags: None") //if tile right is free
                    playerPos.X += 48;  //move right
            }

            tileIndex_X = (ushort)((tileCameraOffset_X + playerPos.X - 400) / tileWidth);  //get current tile based on player position
            tileIndex_Y = (ushort)((tileCameraOffset_Y + playerPos.Y - 240) / tileWidth);

            oldstate = state;  //for player input handling
        }

        public override void Draw(GameTime gameTime)
        {
            _tiledMapRenderer.Draw(_camera.GetViewMatrix()); //draw the tile map

            _spriteBatch.Begin();

            _spriteBatch.Draw(player, new Rectangle((int)playerPos.X, (int)playerPos.Y, 48, 48), Color.White);

            _spriteBatch.End();
        }
    }
}
