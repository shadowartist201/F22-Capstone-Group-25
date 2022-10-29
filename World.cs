using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Game_Demo
{
    public class World : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;  //batch of sprites
        private Texture2D player;    //player texture
        private Texture2D monster;   //monster texture
        private Texture2D chest;     //chest texture

        private SoundEffect soundEffect;
        private SoundEffectInstance instance;
        private AudioListener listener = new AudioListener();
        private AudioEmitter emitter = new AudioEmitter();

        /*
        //circle demo
        private float angle = 0;
        private float distance = 5;
        */

        private Point position = new Point(650, 375);   //player start position
        private Rectangle player_rec = new Rectangle(0, 0, 80, 100);   //player size/hitbox

        public World()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content/World";
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);  //initialize the batch

            player = Content.Load<Texture2D>("player");    //load the player texture
            monster = Content.Load<Texture2D>("monster");  //load the monster texture
            chest = Content.Load<Texture2D>("chest");      //load the chest texture

            soundEffect = Content.Load<SoundEffect>("thunk");
            instance = soundEffect.CreateInstance();
            instance.Apply3D(listener, emitter);

            /* //circle demo
            instance.IsLooped = true;
            instance.Play();
            */

            listener.Position = new Vector3((float)position.X / 400 - 1, listener.Position.Y, (float)position.Y / 400 - 1);
        }

        /*  //circle demo
        private Vector3 CalculateLocation(float angle, float distance)
        {
            return new Vector3(
                (float)Math.Cos(angle) * distance,
                0,
                (float)Math.Sin(angle) * distance);
        }*/

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState state = Keyboard.GetState();  //get state of keyboard

            /* //circle demo
            angle += 0.01f;
            listener.Position = Vector3.Zero;
            emitter.Position = CalculateLocation(angle, distance);
            instance.Apply3D(listener, emitter);
            */

            if (state.IsKeyDown(Keys.Up))  //if key up, move up
                position.Y -= 5;
            if (state.IsKeyDown(Keys.Down))  //if key down, move down
                position.Y += 5;
            if (state.IsKeyDown(Keys.Left))  //if key left, move left
                position.X -= 5;
            if (state.IsKeyDown(Keys.Right))  //if key right, move right
                position.X += 5;

            //wall collision
            if (player_rec.Location.X > 720) { //if too far right, push left
                position.X -= 5;
                instance = soundEffect.CreateInstance();
                emitter.Position = new Vector3(listener.Position.X + 0.1f, listener.Position.Y, listener.Position.Z); //play bump to the right
                instance.Play();
                instance.Apply3D(listener, emitter);
            }
            if (player_rec.Location.X < 0) {   //if too far left, push right
                position.X += 5;
                instance = soundEffect.CreateInstance();
                emitter.Position = new Vector3(listener.Position.X - 0.1f, listener.Position.Y, listener.Position.Z);  //play bump to the left
                instance.Play();
                instance.Apply3D(listener, emitter);
            }
            if (player_rec.Location.Y > 380) {  //if too far down, push up
                position.Y -= 5;
                instance = soundEffect.CreateInstance();
                emitter.Position = new Vector3(listener.Position.X, listener.Position.Y, listener.Position.Z + 0.1f);  //play bump to the back
                instance.Play();
                instance.Apply3D(listener, emitter);
            }
            if (player_rec.Location.Y < 0) {  //if too far up, push down
                position.Y += 5;
                instance = soundEffect.CreateInstance();
                emitter.Position = new Vector3(listener.Position.X, listener.Position.Y, listener.Position.Z - 0.1f);  //play bump to the front
                instance.Play();
                instance.Apply3D(listener, emitter);
            } 

            //chest collision
            if (player_rec.Location.X < 71 && player_rec.Location.Y > 65 && player_rec.Location.Y < 75) //coming from the top
                position.Y -= 5;
            if (player_rec.Location.X > 60 && player_rec.Location.X < 71 && player_rec.Location.Y > 60 && player_rec.Location.Y < 209) //coming from the side
                position.X += 5;
            if (player_rec.Location.X < 71 && player_rec.Location.Y > 200 && player_rec.Location.Y < 209) //coming from the bottom
                position.Y += 5;

            //monster collision
            if (player_rec.Location.Y < 81 && player_rec.Location.X > 420 && player_rec.Location.X < 430) //coming from the left
                position.X -= 5;
            if (player_rec.Location.Y > 70 && player_rec.Location.Y < 81 && player_rec.Location.X > 430 && player_rec.Location.X < 550) //bottom
                position.Y += 5;
            if (player_rec.Location.Y < 81 && player_rec.Location.X < 550 && player_rec.Location.X > 540) //right
                position.X += 5;

            player_rec.Location = position;
            listener.Position = new Vector3((float)position.X / 400 - 1, (float)position.Y / 400 - 1, listener.Position.Z);

            Debug.WriteLine("Emitter: X:{0:F}, Y:{1:F}, Z:{2:F} - Listener: X:{3:F}, Y:{4:F}, Z:{5:F}", emitter.Position.X, emitter.Position.Y, emitter.Position.Z, listener.Position.X, listener.Position.Y, listener.Position.Z);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Blue);  //background color

            _spriteBatch.Begin();

            _spriteBatch.Draw(player, player_rec, Color.White);   //draw player sprite
            _spriteBatch.Draw(chest, new Rectangle(1, 135, 70, 74), Color.White);   //draw chest sprite
            _spriteBatch.Draw(monster, new Rectangle(500, 1, 50, 80), Color.White);  //draw monster sprite

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}