using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Content;
using MonoGame.Extended.Sprites;

namespace Game_Demo
{
    public class Village1 : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;
        public Village1(Game1 game) : base(game) { }

        private SpriteBatch _spriteBatch;
        private OrthographicCamera _camera;
        private EntityTest NPC1 = new(null, new Vector2(100,100), false, false);
        private EntityTest NPC2 = new(null, new Vector2(300,300), false, false);
        private bool talkToNPC1 = false;
        private bool talkToNPC2 = false;

        private MonoGame.Extended.Sprites.AnimatedSprite _playerSprite;

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _camera = new OrthographicCamera(GraphicsDevice);

            Tiled.LoadMap("village", Content, GraphicsDevice); //load map
            Transition.LoadTransition();
            _camera.LookAt(Tiled.startingPosition); //set starting position

            NPC1.sprite = Content.Load<Texture2D>("World/Village1_NPC1"); //load sprite img
            NPC2.sprite = Content.Load<Texture2D>("World/Village1_NPC2");

            /*
            var spriteSheet = Content.Load<SpriteSheet>("player.sf", new JsonContentLoader()); //load sprite anim info
            var sprite = new MonoGame.Extended.Sprites.AnimatedSprite(spriteSheet);
            sprite.Play("idle"); //play idle animation
            _playerSprite = sprite;
            */

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            Tiled.Update_(gameTime); //tiledMapRenderer update
            Tiled.currentPosition = _camera.Center;
            Transition.TransitionCheck();

            if (Collision.CollisionCheck_Entity(NPC1) == Color.Blue && talkToNPC1 == false) //if near NPC1 and not spoken to
                if (Input.SinglePress() == "enter")
                {
                    talkToNPC1 = true; //set flag to true
                    NPC1.MakeDialogBox(DialogText.Village1_NPC1, GraphicsDevice); //make box
                }
            if (talkToNPC1) //if flag is true
                if (NPC1.DialogUpdate() == "hidden") //when box is closed
                    talkToNPC1 = false; //clear flag
                else
                    NPC1.DialogUpdate(); //update box

            if (Collision.CollisionCheck_Entity(NPC2) == Color.Blue && talkToNPC2 == false) //if near NPC2 and not spoken to
                if (Input.SinglePress() == "enter")
                {
                    talkToNPC2 = true; //set flag to true
                    NPC2.MakeDialogBox(DialogText.Village1_NPC2, GraphicsDevice); //make box
                }
            if (talkToNPC2) //if flag is true
                if (NPC2.DialogUpdate() == "hidden") //when box is closed
                    talkToNPC2 = false; //clear flag
                else
                    NPC2.DialogUpdate(); //update box

            if (Collision.CollisionCheck() == Color.Green) //if collided
                return;
            if (Collision.CollisionCheck_Entity(NPC1) == Color.Green)
                return;
            if (Collision.CollisionCheck_Entity(NPC2) == Color.Green)
                return;

            /*
            if (Input.Hold() == "down")
                _playerSprite.Play("walk_down"); //walk down animation
            if (Input.Hold() == "up")
                _playerSprite.Play("walk_up"); //walk up animation
            if (Input.Hold() == "left")
                _playerSprite.Play("walk_left"); //walk left animation
            if (Input.Hold() == "right")
                _playerSprite.Play("walk_right"); //walk right animation
            _playerSprite.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            */

            if (!talkToNPC1 && !talkToNPC2) //if not speaking to an NPC, player can move
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

            _spriteBatch.Draw(World.player, new Rectangle((int)_camera.Center.X, (int)_camera.Center.Y, Tiled.tileWidth, Tiled.tileWidth), Color.White);
            _spriteBatch.Draw(NPC1.sprite, new Rectangle((int)NPC1.position.X, (int)NPC1.position.Y, Tiled.tileWidth, Tiled.tileWidth), Color.White);
            _spriteBatch.Draw(NPC2.sprite, new Rectangle((int)NPC2.position.X, (int)NPC2.position.Y, Tiled.tileWidth, Tiled.tileWidth), Color.White);

            //_spriteBatch.Draw(_playerSprite, Tiled.currentPosition);

            _spriteBatch.End();
            //-------------------- //Must be a different spriteBatch for box to appear correctly
            _spriteBatch.Begin();

            if (talkToNPC1)
                NPC1.DialogDraw(_spriteBatch);
            if (talkToNPC2)
                NPC2.DialogDraw(_spriteBatch);

            _spriteBatch.End();
        }
    }
}
