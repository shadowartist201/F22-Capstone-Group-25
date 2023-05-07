using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Content;
using MonoGame.Extended.Sprites;
using System.Collections.Generic;

namespace Game_Demo
{
    public class ForestPath1 : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;
        public ForestPath1(Game1 game) : base(game) { }

        private List<Item> chestinv = new List<Item> { new Item("Small potion", "Heals 20 health") };

        private SpriteBatch _spriteBatch;
        private OrthographicCamera _camera;
        private EntityTest NPC3 = new(null, new Vector2(1966, 816), false, false);

        private bool talkToNPC3 = false;
        private bool checkNPC3 = false;

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _camera = new OrthographicCamera(GraphicsDevice);

            Tiled.LoadMap("forestpath1", Content, GraphicsDevice); //load map
            Transition.LoadTransition();
            _camera.LookAt(Tiled.startingPosition); //set starting position

            NPC3.sprite = Content.Load<Texture2D>("World/chest");

            World.LoadAnim(Content);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            Tiled.Update_(gameTime); //tiledMapRenderer update
            Tiled.currentPosition = _camera.Center;
            Transition.TransitionCheck();

            if (Collision.CollisionCheck_Entity(NPC3) == Color.Blue && talkToNPC3 == false) //if near NPC3 and not spoken to
                if (Input.SinglePress() == "enter")
                {
                    talkToNPC3 = true; //set flag to true
                    foreach (Item i in chestinv)
                    {
                        Game1.inventory.Add(i);
                    }

                    if (checkNPC3 == false)
                    {
                        NPC3.MakeDialogBox(Dialog.concatInventory(Game1.inventory), GraphicsDevice); //make box
                        checkNPC3 = true;
                    }
                }

            if (talkToNPC3) //if flag is true
                NPC3.DialogUpdate(); //update box

            if (Collision.CollisionCheck() == Color.Green) //if collided
                return;
            if (Collision.CollisionCheck_Entity(NPC3) == Color.Green)
                return;

            World.UpdateAnim(gameTime);
            if (!talkToNPC3) //if not speaking to an NPC, player can move
            {
                Vector2 movementDirection = World.Movement(); //get movement direction
                _camera.Move(movementDirection * World.movementSpeed * gameTime.GetElapsedSeconds()); //move camera
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Tiled.Draw(_camera); //map drawing
            var transformMatrix = _camera.GetViewMatrix();

            _spriteBatch.Begin(transformMatrix: transformMatrix);

            //_spriteBatch.Draw(World.player, new Rectangle((int)_camera.Center.X, (int)_camera.Center.Y, Tiled.tileWidth, Tiled.tileWidth), Color.White);
            _spriteBatch.Draw(NPC3.sprite, new Rectangle((int)NPC3.position.X, (int)NPC3.position.Y, Tiled.tileWidth, Tiled.tileWidth), Color.White);

            World.DrawAnim(_spriteBatch);

            _spriteBatch.End();
            //-------------------- //Must be a different spriteBatch for box to appear correctly
            _spriteBatch.Begin();

            if (talkToNPC3)
                NPC3.DialogDraw(_spriteBatch);

            _spriteBatch.End();
        }
    }
}