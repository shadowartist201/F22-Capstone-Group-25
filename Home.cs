using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Audio;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Screens;
using System.Collections.Generic;
using System;
using MonoGame.Extended.Tiled;
using System.Reflection.Emit;
using System.Diagnostics;

namespace Game_Demo
{
    public class Home : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;
        public Home(Game1 game) : base(game) { }

        private SpriteBatch _spriteBatch;
        private OrthographicCamera _camera;

        //private SoundEffect soundEffect;
        //private SoundEffectInstance instance;
        //private AudioListener listener = new AudioListener();
        //private AudioEmitter emitter = new AudioEmitter();

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _camera = new OrthographicCamera(GraphicsDevice);

            Tiled.LoadMap("home", Content, GraphicsDevice);
            _camera.LookAt(Tiled.startingPosition);

            //soundEffect = Content.Load<SoundEffect>("thunk");
            //instance = soundEffect.CreateInstance();
            //instance.Apply3D(listener, emitter);
            //listener.Position = new Vector3((float)position.X / 400 - 1, listener.Position.Y, (float)position.Y / 400 - 1);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            Tiled.Update_(gameTime);
            Tiled.currentPosition = _camera.Center;

            Vector2 movementDirection = World.Movement();

            if (Collision.CollisionCheck() == Color.Green)
            {
                return;
            }
            
            _camera.Move(movementDirection * World.movementSpeed * gameTime.GetElapsedSeconds());

            //if (player_rec.Location.X > 720)
            //{
            //    position.X -= 5;
            //    instance = soundEffect.CreateInstance();
            //    emitter.Position = new Vector3(listener.Position.X + 0.1f, listener.Position.Y, listener.Position.Z); //play bump to the right
            //    instance.Play();
            //    instance.Apply3D(listener, emitter);
            //}
            //listener.Position = new Vector3((float)position.X / 400 - 1, (float)position.Y / 400 - 1, listener.Position.Z);

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