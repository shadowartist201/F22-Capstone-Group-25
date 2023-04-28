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

            Tiled.LoadMap("city", Content, GraphicsDevice); //load map
            Transition.LoadTransition();
            _camera.LookAt(Tiled.startingPosition); //set camera position

            World.LoadAnim(Content);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            Tiled.Update_(gameTime); //tiledMapRenderer update
            Tiled.currentPosition = _camera.Center;
            Transition.TransitionCheck();

            if (Collision.CollisionCheck() == Color.Green) //if collided
            {
                return;
            }

            World.UpdateAnim(gameTime);

            Vector2 movementDirection = World.Movement(); //get movement direction
            _camera.Move(movementDirection * World.movementSpeed * gameTime.GetElapsedSeconds()); //move camera
        }

        public override void Draw(GameTime gameTime)
        {
            Tiled.Draw(_camera); //map drawing
            var transformMatrix = _camera.GetViewMatrix();

            _spriteBatch.Begin(transformMatrix: transformMatrix);

            //_spriteBatch.Draw(World.player, new Rectangle((int)_camera.Center.X, (int)_camera.Center.Y, Tiled.tileWidth, Tiled.tileWidth), Color.White);
            World.DrawAnim(_spriteBatch);

            _spriteBatch.End();
        }
    }
}
