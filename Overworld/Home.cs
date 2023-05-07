using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Audio;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using System.Collections.Generic;

namespace Game_Demo
{
    public class Home : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;
        public Home(Game1 game) : base(game) { }

        private List<Item> chestHomeinv = new List<Item> { new Item("Attack UP", "An attack booster") };

        private SpriteBatch _spriteBatch;
        private OrthographicCamera _camera;

        private Dialog _dialog = new();
        private EntityTest NPCHomeChest = new(null, new Vector2(270, 170), false, false);


        private bool talkToNPCHomeChest = false;
        private bool checkNPCChest = false;
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

            NPCHomeChest.sprite = Content.Load<Texture2D>("World/chest");

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

            if (Collision.CollisionCheck_Entity(NPCHomeChest) == Color.Blue && talkToNPCHomeChest == false) //if near NPC3 and not spoken to
                if (Input.SinglePress() == "enter")
                {
                    talkToNPCHomeChest = true; //set flag to true
                    foreach (Item i in chestHomeinv)
                    {
                        Game1.inventory.Add(i);
                    }
                    if(checkNPCChest == false)
                    {
                        NPCHomeChest.MakeDialogBox(Dialog.concatInventory(Game1.inventory), GraphicsDevice); //make box
                        checkNPCChest = true;
                    }
                    
                }

            if (talkToNPCHomeChest) //if flag is true
                if (NPCHomeChest.DialogUpdate() == "hidden")//when box is closed
                {
                    //talkToNPCHomeChest = false; //clear flag
                }
                else
                    NPCHomeChest.DialogUpdate(); //update box

            if (Collision.CollisionCheck_Entity(NPCHomeChest) == Color.Green)
                return;

            if (Collision.CollisionCheck() == Color.Green) //if collided
            {
                World.collided.Play();
                return;
            }
            World.collided.Stop();

            Vector2 movementDirection = World.Movement(); //get movement direction
            _camera.Move(movementDirection * World.movementSpeed * gameTime.GetElapsedSeconds()); //move camera

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

            _spriteBatch.Draw(NPCHomeChest.sprite, new Rectangle((int)NPCHomeChest.position.X, (int)NPCHomeChest.position.Y, Tiled.tileWidth, Tiled.tileWidth), Color.White);

            World.DrawAnim(_spriteBatch);
            _spriteBatch.End();

           

            _spriteBatch.Begin();
            _dialog.Draw(_spriteBatch);

            if (talkToNPCHomeChest)
                NPCHomeChest.DialogDraw(_spriteBatch);

            _spriteBatch.End();
        }
    }
}
