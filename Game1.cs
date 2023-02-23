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
        public static GraphicsDeviceManager _graphics;
        public static List<Entity> enemies = new();
        public static List<Entity> squad = new();

        public static SpriteFont small_font;
        public static SpriteFont medium_font;
        public static SpriteFont large_font;

        public static ScreenManager _screenManager = new();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _screenManager = new ScreenManager();
            Components.Add(_screenManager);
        }

        public void LoadBattle()
        {
            //no map change!
            _screenManager.LoadScreen(new Battle(this), new FadeTransition(GraphicsDevice, Color.Black));
        }

        public void BattleReturn()
        {
            switch (Tiled.map)
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
            small_font = Content.Load<SpriteFont>("Battle/small");
            medium_font = Content.Load<SpriteFont>("Battle/medium");
            large_font = Content.Load<SpriteFont>("Battle/large");
            World.player = Content.Load<Texture2D>("World/player");
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
            else if (Tiled.BattleReturn)
            {
                BattleReturn();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }

        public void LoadHome()
        {
            Tiled.map = 1;
            _screenManager.LoadScreen(new Home(this), new FadeTransition(GraphicsDevice, Color.Black));
        }

        public void LoadVillage1()
        {
            Tiled.map = 2;
            _screenManager.LoadScreen(new Village1(this), new FadeTransition(GraphicsDevice, Color.Black));
        }

        public void LoadForest()
        {
            Tiled.map = 3;
            _screenManager.LoadScreen(new Forest(this), new FadeTransition(GraphicsDevice, Color.Black));
        }

        public void LoadCity()
        {
            Tiled.map = 4;
            _screenManager.LoadScreen(new City(this), new FadeTransition(GraphicsDevice, Color.Black));
        }
    }
}