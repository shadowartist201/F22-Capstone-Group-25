using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;


namespace Game_Demo
{
    public class World : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;  //batch of sprites
        private Texture2D player;    //player texture

        private Vector2 playerPos; //player position

        KeyboardState oldstate;

        TiledMap _tiledMap; 
        TiledMapRenderer _tiledMapRenderer;
        TiledMapTileLayer collision;
        TiledMapTile? tile = null;
        private OrthographicCamera _camera;

        int tileCameraOffset_X; //spacer between left and tilemap
        int tileCameraOffset_Y; //spacer between top and tilemap
        private Vector2 _cameraPosition;
        //Default camera position of (0,0) is (400,240) from the top-left edge of the map

        int tileWidth = 48;  //48x48 pixels
        ushort tileIndex_X;
        ushort tileIndex_Y;

        //private SoundEffect soundEffect;
        //private SoundEffectInstance instance;
        //private AudioListener listener = new AudioListener();
        //private AudioEmitter emitter = new AudioEmitter();

        public World()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            var viewportadapter = new BoxingViewportAdapter(Window, GraphicsDevice, 800, 480); //rhis must equal the size of the window (800x480)
            _camera = new OrthographicCamera(viewportadapter);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);  //initialize the batch

            player = Content.Load<Texture2D>("World/player");    //load the player texture

            _tiledMap = Content.Load<TiledMap>("Maps/home");   //load the tilemap
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap); 
            collision = _tiledMap.GetLayer<TiledMapTileLayer>("Collision");  //load collision layer

            tileCameraOffset_X = 400 - ((_graphics.GraphicsDevice.Viewport.Width - _tiledMap.WidthInPixels) / 2);  //camera thing - ((window width - tilemap width) / 2)
            tileCameraOffset_Y = 240 - ((_graphics.GraphicsDevice.Viewport.Height - _tiledMap.HeightInPixels) / 2); //camera thing - ((window height - tilemap height) / 2)
            _cameraPosition = new Vector2(tileCameraOffset_X, tileCameraOffset_Y);

            playerPos.X = 400 - tileCameraOffset_X + (3*tileWidth); //camera thing - offset + (tile amount * tile width)
            playerPos.Y = 240 - tileCameraOffset_Y + (3*tileWidth); //camera thing - offset + (tile amount * tile width)

            //soundEffect = Content.Load<SoundEffect>("thunk");
            //instance = soundEffect.CreateInstance();
            //instance.Apply3D(listener, emitter);
            //listener.Position = new Vector3((float)position.X / 400 - 1, listener.Position.Y, (float)position.Y / 400 - 1);
        }

        protected override void Update(GameTime gameTime)
        {
            _tiledMapRenderer.Update(gameTime);
            _camera.LookAt(_cameraPosition); //set camera position

            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Up) && !oldstate.IsKeyDown(Keys.Up))  
            { 
                collision.TryGetTile(tileIndex_X, (ushort)(tileIndex_Y-1), out tile); //grab collision value of tile up
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

            //if (player_rec.Location.X > 720)
            //{
            //    position.X -= 5;
            //    instance = soundEffect.CreateInstance();
            //    emitter.Position = new Vector3(listener.Position.X + 0.1f, listener.Position.Y, listener.Position.Z); //play bump to the right
            //    instance.Play();
            //    instance.Apply3D(listener, emitter);
            //}
            //listener.Position = new Vector3((float)position.X / 400 - 1, (float)position.Y / 400 - 1, listener.Position.Z);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _tiledMapRenderer.Draw( _camera.GetViewMatrix()); //draw the tile map

            _spriteBatch.Begin();

            _spriteBatch.Draw(player, new Rectangle((int)playerPos.X, (int)playerPos.Y, 48, 48), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}