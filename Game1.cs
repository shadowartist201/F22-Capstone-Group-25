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
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }
    }
}