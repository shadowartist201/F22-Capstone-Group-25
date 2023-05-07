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
    public class City_Bar : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;
        public City_Bar(Game1 game) : base(game) { }

        private SpriteBatch _spriteBatch;
        private OrthographicCamera _camera;
        private EntityTest CityBar_NPC1 = new(null, new Vector2(125, 100), false, false);

        private bool talkToNPC1 = false;

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _camera = new OrthographicCamera(GraphicsDevice);

            Tiled.LoadMap("City_Bar", Content, GraphicsDevice); //load map
            Transition.LoadTransition();
            _camera.LookAt(Tiled.startingPosition); //set starting position

            CityBar_NPC1.sprite = Content.Load<Texture2D>("World/npc5");

            World.LoadAnim(Content);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            Tiled.Update_(gameTime); //tiledMapRenderer update
            Tiled.currentPosition = _camera.Center;
            Transition.TransitionCheck();

            if (Collision.CollisionCheck_Entity(CityBar_NPC1) == Color.Blue && talkToNPC1 == false) //if near NPC1 and not spoken to
                if (Input.SinglePress() == "enter")
                {
                    talkToNPC1 = true; //set flag to true
                    CityBar_NPC1.MakeDialogBox(DialogText.CityBar_NPC1, GraphicsDevice); //make box
                }
            if (talkToNPC1) //if flag is true
                if (CityBar_NPC1.DialogUpdate() == "hidden") //when box is closed
                    talkToNPC1 = false; //clear flag
                else
                    CityBar_NPC1.DialogUpdate(); //update box


            if (Collision.CollisionCheck() == Color.Green) //if collided
                return;
            if (Collision.CollisionCheck_Entity(NPC1) == Color.Green)
                return;

            World.UpdateAnim(gameTime);

            if (!talkToNPC1) //if not speaking to an NPC, player can move
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
            _spriteBatch.Draw(CityBar_NPC1.sprite, new Rectangle((int)CityBar_NPC1.position.X, (int)CityBar_NPC1.position.Y, Tiled.tileWidth, Tiled.tileWidth), Color.White);

            World.DrawAnim(_spriteBatch);

            _spriteBatch.End();
            //-------------------- //Must be a different spriteBatch for box to appear correctly
            _spriteBatch.Begin();

            if (talkToNPC1)
                CityBar_NPC1.DialogDraw(_spriteBatch);


            _spriteBatch.End();
        }
    }
}
