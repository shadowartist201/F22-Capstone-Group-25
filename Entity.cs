﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

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
    public struct EntityTest
    {
        public string name;
        public Texture2D sprite;
        public string dialogID;
        public Vector2 position;
        public bool isShopKeep;
        public bool isStory;

        public EntityTest()
        {
            name = string.Empty;
            sprite = null;
            dialogID = string.Empty;
            position = Vector2.Zero;
            isShopKeep = false;
            isStory = false;
        }

        public EntityTest(string Name, Texture2D Sprite, string DialogID, Vector2 Position, bool IsShopKeep, bool IsStory)
        {
            name = Name;
            sprite = Sprite;
            dialogID = DialogID;
            position = Position;
            isShopKeep = IsShopKeep;
            isStory = IsStory;
        }

        public EntityTest(EntityTest entity)
        {
            name = entity.name;
            sprite = entity.sprite;
            dialogID = entity.dialogID;
            position = entity.position;
            isShopKeep = entity.isShopKeep;
            isStory = entity.isStory;
        }
    }
}

