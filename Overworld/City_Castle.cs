using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Screens;

namespace Game_Demo
{
    public class CityCastle : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;
        public CityCastle(Game1 game) : base(game) { }

        private SpriteBatch _spriteBatch;
        private OrthographicCamera _camera;

        private EntityTest Castle_NPC1 = new(null, new Vector2(935, 400), false, false);

        private bool talkToNPC1 = false;

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _camera = new OrthographicCamera(GraphicsDevice);

            Tiled.LoadMap("City_Castle", Content, GraphicsDevice); //load map
            Transition.LoadTransition();
            _camera.LookAt(Tiled.startingPosition); //set camera position

            Castle_NPC1.sprite = Content.Load<Texture2D>("World/Village1_NPC1");

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
                World.collided.Play();
                return;
            }
            World.collided.Stop();

            if (Collision.CollisionCheck_Entity(Castle_NPC1) == Color.Blue && talkToNPC1 == false) //if near NPC1 and not spoken to
                if (Input.SinglePress() == "enter")
                {
                    talkToNPC1 = true; //set flag to true
                    Castle_NPC1.MakeDialogBox(DialogText.Castle_NPC1, GraphicsDevice); //make box
                }
            if (talkToNPC1) //if flag is true
                if (Castle_NPC1.DialogUpdate() == "hidden") //when box is closed
                    talkToNPC1 = false; //clear flag
                else
                    Castle_NPC1.DialogUpdate(); //update box

            if (Collision.CollisionCheck_Entity(Castle_NPC1) == Color.Green)
                return;

            World.UpdateAnim(gameTime);

            Vector2 movementDirection = World.Movement(); //get movement direction
            _camera.Move(movementDirection * World.movementSpeed * gameTime.GetElapsedSeconds());
        }

        public override void Draw(GameTime gameTime)
        {
            Tiled.Draw(_camera); //map drawing
            var transformMatrix = _camera.GetViewMatrix();

            _spriteBatch.Begin(transformMatrix: transformMatrix);

            _spriteBatch.Draw(Castle_NPC1.sprite, new Rectangle((int)Castle_NPC1.position.X, (int)Castle_NPC1.position.Y, Tiled.tileWidth, Tiled.tileWidth), Color.White);

            //_spriteBatch.Draw(World.player, new Rectangle((int)_camera.Center.X, (int)_camera.Center.Y, Tiled.tileWidth, Tiled.tileWidth), Color.White);
            World.DrawAnim(_spriteBatch);

            _spriteBatch.End();

            _spriteBatch.Begin();

            if (talkToNPC1)
                Castle_NPC1.DialogDraw(_spriteBatch);


            _spriteBatch.End();
        }
    }
}
