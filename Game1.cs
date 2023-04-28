using System.Collections.Generic;
using DavyKager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;

namespace Game_Demo
{
    public class Game1 : Game
    {
        public static GraphicsDeviceManager _graphics;
        public static List<Entity> enemies = new List<Entity> {};
        public static List<Entity> squad = new List<Entity> {};

        public static SpriteFont small_font;
        public static SpriteFont medium_font;
        public static SpriteFont large_font;
        public static SpriteFont DialogFont;

        public static ScreenManager _screenManager = new();

        

        public static bool SwitchBattle, SwitchHome, SwitchVillage, SwitchForest, SwitchCity, SwitchForestPath1, SwitchForestPath2, SwitchMiddleVillage, 
            SwitchCityCastle, SwitchCity_Bar_Inn, SwitchCity_Bar, SwitchCity_PotionShop, SwitchCity_EquipShop, SwitchMountianEntrance, SwitchVillage2_EquipShop, SwitchVillage2_PotionsShop;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _screenManager = new ScreenManager();
            Components.Add(_screenManager);
            Tolk.Load();
            Tolk.TrySAPI(true);
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
                case 5:
                    LoadForestPath1();
                    break;
                case 6:
                    LoadForestPath2();
                    break;
                case 7:
                    LoadMiddleVillage();
                    break;
                case 8:
                    LoadCityCastle();
                    break;
                case 9:
                    LoadCity_EquipShop();
                    break;
                case 10:
                    LoadCity_PotionShop();
                    break;
                case 11:
                    LoadCity_Bar();
                    break;
                case 12:
                    LoadCity_Bar_Inn();
                    break;
                case 13:
                    LoadMountianEntrance();
                    break;
                case 14:
                    LoadVillage2_EquipShop();
                    break;
                case 15:
                    LoadVillage2_PotionsShop();
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
            DialogFont = Content.Load<SpriteFont>("Fonts/dialog");
            World.player = Content.Load<Texture2D>("World/player");
            World.soundEffects.Add(Content.Load<SoundEffect>("World/grass"));
            World.soundEffects.Add(Content.Load<SoundEffect>("World/box_navi"));
            World.soundEffects.Add(Content.Load<SoundEffect>("World/box_ok"));
            World.soundEffects.Add(Content.Load<SoundEffect>("World/Collision"));
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.A) || SwitchBattle)
            {
                SwitchBattle = false;
                Tolk.Silence();
                LoadBattle();
            }
            else if (state.IsKeyDown(Keys.S) || SwitchHome)
            {
                SwitchHome = false;
                LoadHome();
            }
            else if (state.IsKeyDown(Keys.D) || SwitchVillage)
            {
                SwitchVillage = false;
                LoadVillage1();
            }
            else if (state.IsKeyDown(Keys.F) || SwitchForest)
            {
                SwitchForest = false;
                LoadForest();
            }
            else if (state.IsKeyDown(Keys.G) || SwitchCity)
            {
                SwitchCity = false;
                LoadCity();
            }
            else if (state.IsKeyDown(Keys.M) || SwitchForestPath1)
            {
                SwitchForestPath1 = false;
                LoadForestPath1();
            }
            else if (state.IsKeyDown(Keys.N) || SwitchForestPath2)
            {
                SwitchForestPath2 = false;
                LoadForestPath2();
            }
            else if (state.IsKeyDown(Keys.B) || SwitchMiddleVillage)
            {
                SwitchMiddleVillage = false;
                LoadMiddleVillage();
            }
            else if (state.IsKeyDown(Keys.T) || SwitchCityCastle)
            {
                SwitchCityCastle = false;
                LoadCityCastle();
            }
            else if (state.IsKeyDown(Keys.P) || SwitchCity_Bar)
            {
                SwitchCity_Bar = false;
                LoadCity_Bar();
            }
            else if (state.IsKeyDown(Keys.O) || SwitchCity_Bar_Inn)
            {
                SwitchCity_Bar_Inn = false;
                LoadCity_Bar_Inn();
            }
            else if (state.IsKeyDown(Keys.L) || SwitchCity_EquipShop)
            {
                SwitchCity_EquipShop = false;
                LoadCity_EquipShop();
            }
            else if (state.IsKeyDown(Keys.K) || SwitchCity_PotionShop)
            {
                SwitchCity_PotionShop = false;
                LoadCity_PotionShop();
            }
            else if (state.IsKeyDown(Keys.H) || SwitchMountianEntrance)
            {
                SwitchMountianEntrance = false;
                LoadMountianEntrance();
            }
            else if (state.IsKeyDown(Keys.Y) || SwitchVillage2_EquipShop)
            {
                SwitchVillage2_EquipShop = false;
                LoadVillage2_EquipShop();
            }
            else if (state.IsKeyDown(Keys.V) || SwitchVillage2_PotionsShop)
            {
                SwitchVillage2_PotionsShop = false;
                LoadVillage2_PotionsShop();
            }
            else if (Tiled.BattleReturn)
            {
                BattleReturn();
            }
            else if (state.IsKeyDown(Keys.Tab))
            {
                _graphics.ToggleFullScreen();
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
        
        public void LoadForestPath1()
        {
            Tiled.map = 5;
            _screenManager.LoadScreen(new ForestPath1(this), new FadeTransition(GraphicsDevice, Color.Black));
        }
        
        public void LoadForestPath2()
        {
            Tiled.map = 6;
            _screenManager.LoadScreen(new ForestPath2(this), new FadeTransition(GraphicsDevice, Color.Black));
        }
        
        public void LoadMiddleVillage()
        {
            Tiled.map = 7;
            _screenManager.LoadScreen(new MiddleVillage(this), new FadeTransition(GraphicsDevice, Color.Black));
        }

        public void LoadCityCastle()
        {
            Tiled.map = 8;
            _screenManager.LoadScreen(new CityCastle(this), new FadeTransition(GraphicsDevice, Color.Black));
        }
        
        public void LoadCity_EquipShop()
        {
            Tiled.map = 9;
            _screenManager.LoadScreen(new City_EquipShop(this), new FadeTransition(GraphicsDevice, Color.Black));
        }

        public void LoadCity_PotionShop()
        {
            Tiled.map = 10;
            _screenManager.LoadScreen(new City_PotionShop(this), new FadeTransition(GraphicsDevice, Color.Black));
        }

        public void LoadCity_Bar()
        {
            Tiled.map = 11;
            _screenManager.LoadScreen(new City_Bar(this), new FadeTransition(GraphicsDevice, Color.Black));
        }

        public void LoadCity_Bar_Inn()
        {
            Tiled.map = 12;
            _screenManager.LoadScreen(new City_Bar_Inn(this), new FadeTransition(GraphicsDevice, Color.Black));
        }

        public void LoadMountianEntrance()
        {
            Tiled.map = 13;
            _screenManager.LoadScreen(new MountianEntrance(this), new FadeTransition(GraphicsDevice, Color.Black));
        }

        public void LoadVillage2_EquipShop()
        {
            Tiled.map = 14;
            _screenManager.LoadScreen(new Village2_EquipShop(this), new FadeTransition(GraphicsDevice, Color.Black));
        }

        public void LoadVillage2_PotionsShop()
        {
            Tiled.map = 15;
            _screenManager.LoadScreen(new Village2_PotionsShop(this), new FadeTransition(GraphicsDevice, Color.Black));
        }
    }
}