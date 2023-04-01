using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using MonoGame.Extended.VectorDraw;
using System.Diagnostics;

namespace Game_Demo
{
    public class Village1 : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;
        public Village1(Game1 game) : base(game) { }

        private SpriteBatch _spriteBatch;
        private OrthographicCamera _camera;
        private EntityTest NPC1 = new();
        private EntityTest NPC2 = new();

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _camera = new OrthographicCamera(GraphicsDevice);

            Tiled.LoadMap("village", Content, GraphicsDevice); //load map
            Transition.LoadTransition();
            _camera.LookAt(Tiled.startingPosition); //set starting position

            NPC1.sprite = Content.Load<Texture2D>("World/Village1_NPC1");
            NPC2.sprite = Content.Load<Texture2D>("World/Village1_NPC2");
            NPC1.position = new Vector2(100, 100);
            NPC2.position = new Vector2(300, 300);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            Tiled.Update_(gameTime); //tiledMapRenderer update
            Tiled.currentPosition = _camera.Center;
            Transition.TransitionCheck();

            if (Collision.CollisionCheck() == Color.Green) //if collided
                return;
            if (Collision.CollisionCheck_Entity(NPC1) == Color.Green)
                return;
            if (Collision.CollisionCheck_Entity(NPC2) == Color.Green)
                return;

            Vector2 movementDirection = World.Movement(); //get movement direction
            _camera.Move(movementDirection * World.movementSpeed * gameTime.GetElapsedSeconds()); //move camera
        }

        public override void Draw(GameTime gameTime)
        {
            Tiled.Draw(_camera); //map drawing
            var transformMatrix = _camera.GetViewMatrix();

            _spriteBatch.Begin(transformMatrix: transformMatrix);

            _spriteBatch.Draw(World.player, new Rectangle((int)_camera.Center.X, (int)_camera.Center.Y, Tiled.tileWidth, Tiled.tileWidth), Color.White);

            _spriteBatch.Draw(NPC1.sprite, new Rectangle((int)NPC1.position.X, (int)NPC1.position.Y, Tiled.tileWidth, Tiled.tileWidth), Color.White);
            _spriteBatch.Draw(NPC2.sprite, new Rectangle((int)NPC2.position.X, (int)NPC2.position.Y, Tiled.tileWidth, Tiled.tileWidth), Color.White);

            _spriteBatch.End();
        }
    }
}
