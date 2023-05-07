using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Game_Demo
{
    public struct Entity
    {
        public string name; //entity name
        public int health; //entity HP
        public int mHealth; //max HP
        public int mana; //entity MP
        public int mMana; //max MP
        public int attack; //attack stat
        public int spattack; //magic attack stat
        public int def; //defense stat
        public int spdef; //magic defense stat
        public Entity()
        {
            name = "Something's wrong";
            health = 1;
            mHealth = 1;
            mana = 0;
            mMana = 0;
            attack = 0;
            spattack = 0;
            def = 0;
            spdef = 0;
        }
        public Entity(string n, int h, int mh,int m, int mm, int a, int sa, int d, int sd)
                  //string name, HP, max HP, MP, max MP, attack, magic attack, defense, magic defense, speed
        {
            name = n;
            health = h;
            mHealth = mh;
            mana = m;
            mMana = mm;
            attack = a;
            spattack = sa;
            def = d;
            spdef = sd;
        }
        public Entity(Entity e) //Entity entity
        {
            name = e.name;
            health = e.health;
            mHealth = e.mHealth;
            mana = e.mana;
            mMana = e.mMana;
            attack = e.attack;
            spattack = e.spattack;
            def = e.def;
            spdef = e.spdef;
        }
    }
    public class EntityTest //for NPCs
    {
        public Texture2D sprite;
        public Vector2 position;
        public bool isShopKeep; //is NPC a shopkeep (potions/equipment)
        public bool isStory; //is NPC in a story event
        private Dialog dialog = new();

        public EntityTest()
        {
            sprite = null;
            position = Vector2.Zero;
            isShopKeep = false;
            isStory = false;
        }

        public EntityTest(Texture2D Sprite, Vector2 Position, bool IsShopKeep, bool IsStory)
        {
            sprite = Sprite;
            position = Position;
            isShopKeep = IsShopKeep;
            isStory = IsStory;
        }

        public EntityTest(EntityTest entity)
        {
            sprite = entity.sprite;
            position = entity.position;
            isShopKeep = entity.isShopKeep;
            isStory = entity.isStory;
        }

        public void MakeDialogBox (string text, GraphicsDevice graphicsDevice)
        {
            dialog.MakeBox(text, Game1.DialogFont, graphicsDevice, new OrthographicCamera(graphicsDevice));
        }
        public string DialogUpdate ()
        {
            string status;
            status = dialog.Update();
            return status;
        }

        public void DialogDraw (SpriteBatch spriteBatch)
        {
            dialog.Draw(spriteBatch);
        }
    }
}

