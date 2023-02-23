using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Screens;

namespace Game_Demo
{
    public class City : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;
        public City(Game1 game) : base(game) { }

        private SpriteBatch _spriteBatch;
        private OrthographicCamera _camera;

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _camera = new OrthographicCamera(GraphicsDevice);

            Tiled.LoadMap("city", Content, GraphicsDevice);
            _camera.LookAt(Tiled.startingPosition);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            Tiled.Update_(gameTime);
            Tiled.currentPosition = _camera.Center;

            Vector2 movementDirection = World.Movement();
            _camera.Move(movementDirection * World.movementSpeed * gameTime.GetElapsedSeconds());
        }

        public override void Draw(GameTime gameTime)
        {
            Tiled.Draw(_camera);
            var transformMatrix = _camera.GetViewMatrix();

            _spriteBatch.Begin(transformMatrix: transformMatrix);

            _spriteBatch.Draw(World.player, new Rectangle((int)_camera.Center.X, (int)_camera.Center.Y, Tiled.tileWidth, Tiled.tileWidth), Color.White);

            _spriteBatch.End();
        }
    }
}
