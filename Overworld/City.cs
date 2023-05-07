using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Audio;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using System.Collections.Generic;

namespace Game_Demo
{
    public class City : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;
        public City(Game1 game) : base(game) { }

        private List<Item> chestCityinv = new List<Item> { new Item("Small potion", "Heals 20 health") };

        private SpriteBatch _spriteBatch;
        private OrthographicCamera _camera;

        private EntityTest NPCHcityChest = new(null, new Vector2(885, 1791), false, false);
        private EntityTest City_NPC1 = new(null, new Vector2(760, 1825), false, false);
        private EntityTest City_NPC2 = new(null, new Vector2(1350, 280), false, false);

        private bool talkToNPC1 = false;
        private bool talkToNPC2 = false;


        private bool talkToNPCcityChest = false;
        private bool checkNPCcityChest = false;
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

            Tiled.LoadMap("city", Content, GraphicsDevice); //load map
            Transition.LoadTransition();
            _camera.LookAt(Tiled.startingPosition); //set starting position

            NPCHcityChest.sprite = Content.Load<Texture2D>("World/chest");
            City_NPC1.sprite = Content.Load<Texture2D>("World/Village1_NPC2");
            City_NPC2.sprite = Content.Load<Texture2D>("World/npc3");

            World.LoadAnim(Content);
            //_dialog.MakeBox(DialogText.Demo, Game1.DialogFont, GraphicsDevice, new OrthographicCamera(GraphicsDevice));


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

            if (Collision.CollisionCheck_Entity(City_NPC1) == Color.Blue && talkToNPC1 == false) //if near NPC1 and not spoken to
                if (Input.SinglePress() == "enter")
                {
                    talkToNPC1 = true; //set flag to true
                    City_NPC1.MakeDialogBox(DialogText.City_NPC1, GraphicsDevice); //make box
                }
            if (talkToNPC1) //if flag is true
                if (City_NPC1.DialogUpdate() == "hidden") //when box is closed
                    talkToNPC1 = false; //clear flag
                else
                    City_NPC1.DialogUpdate(); //update box

            if (Collision.CollisionCheck_Entity(City_NPC2) == Color.Blue && talkToNPC2 == false) //if near NPC1 and not spoken to
                if (Input.SinglePress() == "enter")
                {
                    talkToNPC2 = true; //set flag to true
                    City_NPC2.MakeDialogBox(DialogText.City_NPC2, GraphicsDevice); //make box
                }
            if (talkToNPC2) //if flag is true
                if (City_NPC2.DialogUpdate() == "hidden") //when box is closed
                    talkToNPC2 = false; //clear flag
                else
                    City_NPC2.DialogUpdate(); //update box

            if (Collision.CollisionCheck_Entity(NPCHcityChest) == Color.Blue && talkToNPCcityChest == false) //if near NPC3 and not spoken to
                if (Input.SinglePress() == "enter")
                {
                    talkToNPCcityChest = true; //set flag to true
                    foreach (Item i in chestCityinv)
                    {
                        Game1.inventory.Add(i);
                    }
                    if (checkNPCcityChest == false)
                    {
                        NPCHcityChest.MakeDialogBox(Dialog.concatInventory(Game1.inventory), GraphicsDevice); //make box
                        checkNPCcityChest = true;
                    }

                }

            if (talkToNPCcityChest) //if flag is true
                if (NPCHcityChest.DialogUpdate() == "hidden")//when box is closed
                {
                    //talkToNPCHomeChest = false; //clear flag
                }
                else
                    NPCHcityChest.DialogUpdate(); //update box

            if (Collision.CollisionCheck_Entity(NPCHcityChest) == Color.Green)
                return;

            if (Collision.CollisionCheck_Entity(City_NPC1) == Color.Green)
                return;
            if (Collision.CollisionCheck_Entity(City_NPC2) == Color.Green)
                return;

            if (Collision.CollisionCheck() == Color.Green) //if collided
            {
                World.collided.Play();
                return;
            }
            World.collided.Stop();

            Vector2 movementDirection = World.Movement(); //get movement direction
            _camera.Move(movementDirection * World.movementSpeed * gameTime.GetElapsedSeconds()); //move camera

            //_dialog.Update();

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

            _spriteBatch.Draw(NPCHcityChest.sprite, new Rectangle((int)NPCHcityChest.position.X, (int)NPCHcityChest.position.Y, Tiled.tileWidth, Tiled.tileWidth), Color.White);
            _spriteBatch.Draw(City_NPC1.sprite, new Rectangle((int)City_NPC1.position.X, (int)City_NPC1.position.Y, Tiled.tileWidth, Tiled.tileWidth), Color.White);
            _spriteBatch.Draw(City_NPC2.sprite, new Rectangle((int)City_NPC2.position.X, (int)City_NPC2.position.Y, Tiled.tileWidth, Tiled.tileWidth), Color.White);

            World.DrawAnim(_spriteBatch);
            _spriteBatch.End();



            _spriteBatch.Begin();
            //_dialog.Draw(_spriteBatch);

            if (talkToNPCcityChest)
                NPCHcityChest.DialogDraw(_spriteBatch);
            if (talkToNPC1)
                City_NPC1.DialogDraw(_spriteBatch);
            if (talkToNPC2)
                City_NPC2.DialogDraw(_spriteBatch);

            _spriteBatch.End();
        }
    }
}
