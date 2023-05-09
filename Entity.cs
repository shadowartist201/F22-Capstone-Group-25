using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Game_Demo
{
    public struct Entity
    {
        public string name; //entity name
        public int HP; //entity HP
        public int maxHP; //max HP
        public int MP; //entity MP
        public int maxMP; //max MP
        public int attack; //attack stat
        public int spAttack; //magic attack stat
        public int defense; //defense stat
        public int spDefense; //magic defense stat
        public Entity()
        {
            name = "Something's wrong";
            HP = 1;
            maxHP = 1;
            MP = 0;
            maxMP = 0;
            attack = 0;
            spAttack = 0;
            defense = 0;
            spDefense = 0;
        }
        public Entity(string name_, int HP_, int maxHP_,int MP_, int maxMP_, int attack_, int spAttack_, int defense_, int spDefense_)
        {
            name = name_;
            HP = HP_;
            maxHP = maxHP_;
            MP = MP_;
            maxMP = maxMP_;
            attack = attack_;
            spAttack = spAttack_;
            defense = defense_;
            spDefense = spDefense_;
        }
        public Entity(Entity entity) //Entity entity
        {
            name = entity.name;
            HP = entity.HP;
            maxHP = entity.maxHP;
            MP = entity.MP;
            maxMP = entity.maxMP;
            attack = entity.attack;
            spAttack = entity.spAttack;
            defense = entity.defense;
            spDefense = entity.spDefense;
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

