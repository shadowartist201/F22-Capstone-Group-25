using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;

namespace Game_Demo
{
    public class World
    {
        public static Texture2D player; //overworld player texture
        public static float movementSpeed = 250; //movement speed
        static SpriteSheet spriteSheet;
        static AnimatedSprite sprite;
        static AnimatedSprite _playerSprite;


        public static Vector2 Movement() //convert keyboard input to Vector2 for camera move
        {
            return Input.Hold() switch
            {
                "up" => new Vector2(0, -1),
                "down" => new Vector2(0, 1),
                "left" => new Vector2(-1, 0),
                "right" => new Vector2(1, 0),
                _ => new Vector2(0, 0),
            };
        }
        
        public static void LoadAnim(ContentManager Content)
        {
            spriteSheet = Content.Load<SpriteSheet>("player.sf", new JsonContentLoader()); //load sprite anim info
            sprite = new AnimatedSprite(spriteSheet);
            sprite.Play("idle");
            _playerSprite = sprite;
        }

        public static void UpdateAnim(GameTime gameTime)
        {
            if (Input.Hold() == "down")
                _playerSprite.Play("walk_down"); //walk down animation
            if (Input.Hold() == "up")
                _playerSprite.Play("walk_up"); //walk up animation
            if (Input.Hold() == "left")
                _playerSprite.Play("walk_left"); //walk left animation
            if (Input.Hold() == "right")
                _playerSprite.Play("walk_right"); //walk right animation
            _playerSprite.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        public static void DrawAnim(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(_playerSprite, Tiled.currentPosition);
        }
    }
}
