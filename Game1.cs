using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;

namespace Game_Demo
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager _graphics;
        //public SpriteBatch _spriteBatch;  //batch of sprites

        private readonly ScreenManager _screenManager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _screenManager = new ScreenManager();
            Components.Add(_screenManager);
        }

        public void LoadScreen1()
        {
            _screenManager.LoadScreen(new World(this), new FadeTransition(GraphicsDevice, Color.Black));
        }

        public void LoadScreen2()
        {
            _screenManager.LoadScreen(new Battle(this), new FadeTransition(GraphicsDevice, Color.Black));
        }

        protected override void Initialize()
        {
            LoadScreen1();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            //_spriteBatch = new SpriteBatch(GraphicsDevice);  //initialize the batch
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Escape))
            {
                LoadScreen2();
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }
    }
}