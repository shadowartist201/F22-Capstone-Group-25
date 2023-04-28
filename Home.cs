using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Audio;
using MonoGame.Extended;
using MonoGame.Extended.Screens;

namespace Game_Demo
{
    public class Home : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;
        public Home(Game1 game) : base(game) { }

        private SpriteBatch _spriteBatch;
        private OrthographicCamera _camera;

        private Dialog _dialog = new();

        /* 
         * private SoundEffect soundEffect;
         * private SoundEffectInstance instance;
         * private AudioListener listener = new AudioListener();
         * private AudioEmitter emitter = new AudioEmitter();
         */

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _camera = new OrthographicCamera(GraphicsDevice);

            Tiled.LoadMap("home", Content, GraphicsDevice); //load map
            Transition.LoadTransition();
            _camera.LookAt(Tiled.startingPosition); //set starting position

            World.LoadAnim(Content);
            _dialog.MakeBox(DialogText.Demo, Game1.DialogFont, GraphicsDevice, new OrthographicCamera(GraphicsDevice));


            /* 
             * soundEffect = Content.Load<SoundEffect>("thunk");
             * instance = soundEffect.CreateInstance();
             * instance.Apply3D(listener, emitter);
             * listener.Position = new Vector3((float)position.X / 400 - 1, listener.Position.Y, (float)position.Y / 400 - 1);
             */

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

            Vector2 movementDirection = World.Movement(); //get movement direction
            _camera.Move(movementDirection * World.movementSpeed * gameTime.GetElapsedSeconds()); //move camera
            if (movementDirection == new Vector2(0, 0))
                World.instance.Stop();
            else
                World.instance.Play();

            _dialog.Update();

            World.UpdateAnim(gameTime);

            /*
             * if (player_rec.Location.X > 720)
             * {
             *     position.X -= 5;
             *     instance = soundEffect.CreateInstance();
             *     emitter.Position = new Vector3(listener.Position.X + 0.1f, listener.Position.Y, listener.Position.Z); //play bump to the right
             *      instance.Play();
             *      instance.Apply3D(listener, emitter);
             *  }
             *  listener.Position = new Vector3((float)position.X / 400 - 1, (float)position.Y / 400 - 1, listener.Position.Z);
             */

        }

        public override void Draw(GameTime gameTime)
        {
            Tiled.Draw(_camera); //map drawing

            var transformMatrix = _camera.GetViewMatrix();
            _spriteBatch.Begin(transformMatrix: transformMatrix);
            //_spriteBatch.Draw(World.player, new Rectangle((int)_camera.Center.X, (int)_camera.Center.Y, Tiled.tileWidth, Tiled.tileWidth), Color.White);
            World.DrawAnim(_spriteBatch);
            _spriteBatch.End();

            _spriteBatch.Begin();
            _dialog.Draw(_spriteBatch);
            _spriteBatch.End();
        }
    }
}