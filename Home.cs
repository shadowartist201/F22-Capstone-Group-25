using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Audio;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.Screens;
using System.Collections.Specialized;
using System.Diagnostics;

namespace Game_Demo
{
    public class Home : GameScreen
    {
        private Game1 game => (Game1)base.Game;
        public Home(Game1 game) : base(game) { }

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

        int tileWidth = 48;  //48x48 pixels
        ushort tileIndex_X;
        ushort tileIndex_Y;

        //private SoundEffect soundEffect;
        //private SoundEffectInstance instance;
        //private AudioListener listener = new AudioListener();
        //private AudioEmitter emitter = new AudioEmitter();

        public override void LoadContent()
        {
            player = Content.Load<Texture2D>("World/player");    //load the player texture
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _camera = new OrthographicCamera(GraphicsDevice);

            _tiledMap = Content.Load<TiledMap>("Maps/home");   //load the tilemap
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
            collision = _tiledMap.GetLayer<TiledMapTileLayer>("Collision");  //load collision layer

            _cameraPosition = new Vector2(2 * tileWidth, 2 * tileWidth);
            playerPos = Vector2.Zero;

            //soundEffect = Content.Load<SoundEffect>("thunk");
            //instance = soundEffect.CreateInstance();
            //instance.Apply3D(listener, emitter);
            //listener.Position = new Vector3((float)position.X / 400 - 1, listener.Position.Y, (float)position.Y / 400 - 1);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            _tiledMapRenderer.Update(gameTime);
            _camera.LookAt(_cameraPosition); //set camera position

            KeyboardState state = Keyboard.GetState();

            Debug.WriteLine("Camera: (" + _cameraPosition + ", " + _cameraPosition + "), Tile: (" + tileIndex_X + ", " + tileIndex_Y + ")");

            if (state.IsKeyDown(Keys.Up))  //&& !oldstate.IsKeyDown(Keys.Up)
            {
                collision.TryGetTile(tileIndex_X, tileIndex_Y, out tile); //grab collision value of tile up
                if (tile.ToString() == "GlobalIdentifier: 0, Flags: None") //if tile up is free
                    _cameraPosition.Y -= 2;
                else
                    _cameraPosition.Y += 2;
            }
            if (state.IsKeyDown(Keys.Down)) // && !oldstate.IsKeyDown(Keys.Down)
            {
                collision.TryGetTile(tileIndex_X, tileIndex_Y, out tile); //grab collision value of tile down
                if (tile.ToString() == "GlobalIdentifier: 0, Flags: None") //if tile down is free
                    _cameraPosition.Y += 2;
                else
                    _cameraPosition.Y -= 2;
            }
            if (state.IsKeyDown(Keys.Left)) // && !oldstate.IsKeyDown(Keys.Left)
            {
                collision.TryGetTile((ushort)(tileIndex_X - 1), tileIndex_Y, out tile); //grab collision value of tile left
                if (tile.ToString() == "GlobalIdentifier: 0, Flags: None") //if tile left is free
                    _cameraPosition.X -= 2;
                else
                    _cameraPosition.X += 2;
            }
            if (state.IsKeyDown(Keys.Right)) // && !oldstate.IsKeyDown(Keys.Right)
            {
                collision.TryGetTile((ushort)(tileIndex_X + 1), tileIndex_Y, out tile); //grab collision value of tile right
                if (tile.ToString() == "GlobalIdentifier: 0, Flags: None") //if tile right is free
                    _cameraPosition.X += 2;
                else
                    _cameraPosition.X -= 2;
            }

            tileIndex_X = (ushort)((_cameraPosition.X) / tileWidth);  //get current tile based on player position
            tileIndex_Y = (ushort)((_cameraPosition.Y) / tileWidth);

            //oldstate = state;  //for player input handling

            //if (player_rec.Location.X > 720)
            //{
            //    position.X -= 5;
            //    instance = soundEffect.CreateInstance();
            //    emitter.Position = new Vector3(listener.Position.X + 0.1f, listener.Position.Y, listener.Position.Z); //play bump to the right
            //    instance.Play();
            //    instance.Apply3D(listener, emitter);
            //}
            //listener.Position = new Vector3((float)position.X / 400 - 1, (float)position.Y / 400 - 1, listener.Position.Z);

        }

        public override void Draw(GameTime gameTime)
        {
            _tiledMapRenderer.Draw(_camera.GetViewMatrix()); //draw the tile map

            _spriteBatch.Begin();

            _spriteBatch.Draw(player, new Rectangle((int)(playerPos.X + _camera.Origin.X), (int)(playerPos.Y + _camera.Origin.Y), 48, 48), Color.White);

            _spriteBatch.End();
        }
    }
}