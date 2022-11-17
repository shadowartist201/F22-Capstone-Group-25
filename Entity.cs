using System;
namespace Game_Demo
{
    public struct Entity
    {
        public string name;
        public int health;
        public int mHealth;
        public int mana;
        public int mMana;
        public int attack;
        public int spattack;
        public int def;
        public int spdef;
        public int speed;
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
            speed = 0;
        }
        public Entity(string n, int h, int mh,int m, int mm, int a, int sa, int d, int sd, int s)
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
            speed = s;
        }
        public Entity(Entity e)
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
            speed = e.speed;
        }
    }
}

