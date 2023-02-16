using System.Collections.Generic;
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
        public int map;
        public Vector2 playerPos;
        public List<Entity> enemies = new List<Entity>();
        public List<Entity> squad = new List<Entity>();
        private readonly ScreenManager _screenManager;
        SpriteBatch _spriteBatch;

        public KeyboardState _keyboardState { get; private set; }
        public KeyboardState _previousKeyboardState { get; private set; }

        public SpriteFont DialogFont { get; private set; }

        public Vector2 CenterScreen 
            => new Vector2(_graphics.GraphicsDevice.Viewport.Width / 2f, _graphics.GraphicsDevice.Viewport.Height / 2f);

        private DialogBox _dialogBox;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _screenManager = new ScreenManager();
            Components.Add(_screenManager);
        }

        public void LoadHome()
        {
            map = 1;
            _screenManager.LoadScreen(new Home(this), new FadeTransition(GraphicsDevice, Color.Black));
        }

        public void LoadVillage1()
        {
            map = 2;
            _screenManager.LoadScreen(new Village1(this), new FadeTransition(GraphicsDevice, Color.Black));
        }

        public void LoadForest()
        {
            map = 3;
            _screenManager.LoadScreen(new Forest(this), new FadeTransition(GraphicsDevice, Color.Black));
        }

        public void LoadCity()
        {
            map = 4;
            _screenManager.LoadScreen(new City(this), new FadeTransition(GraphicsDevice, Color.Black));
        }

        public void LoadBattle()
        {
            //no map change!
            _screenManager.LoadScreen(new Battle(this), new FadeTransition(GraphicsDevice, Color.Black));
        }

        public void BattleReturn()
        {
            switch (map)
            {
                case 1:
                    LoadHome();
                    break;
                case 2:
                    LoadVillage1();
                    break;
                case 3:
                    LoadForest();
                    break;
                case 4:
                    LoadCity();
                    break;
            }
        }

        protected override void Initialize()
        {
            LoadHome();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            DialogFont = Content.Load<SpriteFont>("Fonts/dialog");

            _dialogBox = new DialogBox
            {
                Text = "Hello World! Press Enter to proceed.\n" +
                       "I will be on the next pane! " +
                       "And wordwrap will occur, especially if there are some longer words!\n" +
                       "Monospace fonts work best but you might not want Courier New.\n" +
                       "In this code sample, after this dialog box finishes, you can press the O key to open a new one."
            };

            _dialogBox.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.A))
            {
                LoadBattle();
            }
            else if (state.IsKeyDown(Keys.S))
            {
                LoadHome();
            }
            else if (state.IsKeyDown(Keys.D))
            {
                LoadVillage1();
            }
            else if (state.IsKeyDown(Keys.F))
            {
                LoadForest();
            }
            else if (state.IsKeyDown(Keys.G))
            {
                LoadCity();
            }

            _dialogBox.Update();

            _previousKeyboardState = _keyboardState;
            _keyboardState = Keyboard.GetState();


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            _dialogBox.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}