﻿/*using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using MonoGame.Extended.Screens;

namespace Game_Demo
{
    public class Battleref : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;
        public Battleref(Game1 game) : base(game) { }

        public SpriteBatch _spriteBatch;  //batch of sprites
        //TO DO: adjust this for a list of 6 enemies and the three player textures
        private Texture2D player_battle;    //player texture
        private Texture2D dragon_battle;   //monster texture
        private Texture2D cat_battle;      //cat texture
        private Texture2D healing_effect;

        public static int target = 0;

        public static bool selection = false;

        public static bool alratk = false; //already attacked
        private bool endBattle = false;

        public override void Initialize()
        {
            //new Entity(name, HP, max HP, MP, max MP, attack, magic attack, defense, magic defense, speed)
            //TO DO: clear this out, testing inputs will need this cleared
            Game1.enemies.Add(new Entity("Dragon", 200, 200, 0, 0, 10, 15, 0, 0));

            Game1.squad.Add(new Entity("Nobody", 100, 100, 10, 10, 5, 10, 10, 12));
            Game1.squad.Add(new Entity("Cat", 50, 50, 0, 0, 5, 10, 5, 7));

            base.Initialize();
        }

        public override void LoadContent()
        {
            //TO DO: logically separate loading to reduce memory pressure
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            player_battle = Content.Load<Texture2D>("Battle/player-battle");    //load the player texture
            dragon_battle = Content.Load<Texture2D>("Battle/dragon-battle");  //load the monster texture
            cat_battle = Content.Load<Texture2D>("Battle/cat-battle");      //load the cat texture
            healing_effect = Content.Load<Texture2D>("Battle/cat-battle");

            BattleUI.LoadUI(Content); //load UI textures

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            BattleUI.Update_(); //logic for UI
        }

        public override void Draw(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();
            endBattle = true;
            foreach (Entity e in Game1.enemies)
            {
                if (e.health != 0) //if battle over
                {
                    endBattle = false;
                }
            }

            if(endBattle || state.IsKeyDown(Keys.D1))
            {
                Entity thing = Game1.enemies[0];
                thing.health = 0;
                Game1.enemies[0] = thing;

                BattleUI.BattleEnd(GraphicsDevice, _spriteBatch);

                if (Input.Hold() == "enter")
                {
                    Tiled.BattleReturn = true;
                }

            }
            else  //if battle still going
            {
                GraphicsDevice.Clear(Color.Green);
                _spriteBatch.Begin();

                BattleUI.DrawBoxes(_spriteBatch); //draw UI boxes
                BattleUI.DrawText(_spriteBatch); //draw UI text

                _spriteBatch.Draw(player_battle, new Rectangle(630, 60, 54, 77), Color.White);   //draw player sprite
                _spriteBatch.Draw(dragon_battle, new Rectangle(70, 35, 239, 246), Color.White);   //draw monster sprite
                _spriteBatch.Draw(cat_battle, new Rectangle(630, 185, 71, 55), Color.White);  //draw cat sprite

                if (BattleUI.message_alpha == 1f) //if message box active
                {
                    if (BattleUI.attack_message) //when attack message activated
                    {
                        if (!alratk) //and not already attacked
                        {
                            Entity currchar;
                            Entity currenemy;
                            if (BattleUI.squadTurn)
                            {
                                currchar = Game1.squad[BattleUI.current_character]; //set current character
                                currenemy = Game1.enemies[target]; //set current enemy
                            }
                            else
                            {
                                currchar = Game1.enemies[BattleUI.current_character];
                                if (Game1.squad[Game1.squad.Count - 1].health > 0)
                                    currenemy = Game1.squad[Game1.squad.Count - 1];
                                else if (Game1.squad[Game1.squad.Count-1].health<1 && Game1.squad[Game1.squad.Count - 2].health > 0)
                                    currenemy = Game1.squad[Game1.squad.Count - 2];
                                else
                                    currenemy = Game1.squad[Game1.squad.Count - 3];
                            }
                            List<Entity> returned = new List<Entity>();
                            BattleUI.menu_alpha = 0f; //hide menu
                            returned = attack(ref currchar, ref currenemy, _spriteBatch, BattleUI.battle_message, Game1.large_font, Game1.small_font);
                            Game1.squad[BattleUI.current_character] = returned[0];
                            Game1.enemies[target] = returned[1];
                            alratk = true; //set attacked flag to true
                        }
                        _spriteBatch.Draw(BattleUI.battle_message, new Rectangle(182, 336, 299, 128), Color.White);
                        if (BattleUI.squadTurn)
                            _spriteBatch.DrawString(Game1.large_font, "*" + Game1.squad[BattleUI.current_character].name + " attacked the " + Game1.enemies[target - 1].name + "!", new Vector2(205, 361), Color.Black);
                        else
                            _spriteBatch.DrawString(Game1.large_font, "*" + Game1.enemies[BattleUI.current_character].name + " attacked " + Game1.squad[target].name + "!", new Vector2(205, 361), Color.Black);
                    }
                    if (BattleUI.magic_message) //when magic message activated
                    {
                        if ((Game1.squad[BattleUI.current_character].mana > 0 && BattleUI.squadTurn) || (Game1.enemies[BattleUI.current_character].mana > 0 && !BattleUI.squadTurn)) //if current character has MP
                        {
                            if (!alratk) //and not already attacked
                            {
                                Entity currchar = Game1.squad[BattleUI.current_character]; //set current character
                                Entity currenemy = Game1.enemies[0]; //set current enemy
                                List<Entity> returned = new List<Entity>();
                                BattleUI.menu_alpha = 0f; //hide menu
                                returned = spattack(ref currchar, ref currenemy, _spriteBatch, BattleUI.battle_message, Game1.medium_font);
                                Game1.squad[BattleUI.current_character] = returned[0];
                                Game1.enemies[0] = returned[1];
                                alratk = true; //set attacked flag to true
                            }
                        }
                        if (Game1.squad[BattleUI.current_character].mana>0)
                        {
                            _spriteBatch.Draw(BattleUI.battle_message, new Rectangle(182, 336, 299, 128), Color.White);
                            _spriteBatch.DrawString(Game1.large_font, "*" + Game1.squad[BattleUI.current_character].name + " summoned fire!", new Vector2(205, 361), Color.Black);
                        }
                        else
                        {
                            _spriteBatch.Draw(BattleUI.battle_message, new Rectangle(182, 336, 299, 128), Color.White);
                            _spriteBatch.DrawString(Game1.large_font, "*" + Game1.squad[BattleUI.current_character].name + " is out of mana!", new Vector2(205, 361), Color.Black);
                        }
                    }
                    if (BattleUI.cat_magic_message) //when cat message message activated, draw it
                    {
                        BattleUI.menu_alpha = 0f;
                        _spriteBatch.Draw(BattleUI.battle_message, new Rectangle(182, 336, 299, 128), Color.White);
                        _spriteBatch.DrawString(Game1.large_font, "*Cats can't do magic, silly!", new Vector2(205, 361), Color.Black);
                    }
                }
                _spriteBatch.End();
            }

            List<Entity> attack(ref Entity a, ref Entity t, SpriteBatch _spriteBatch, Texture2D battle_message, SpriteFont large_font, SpriteFont small_font) //attack process
            {
                hpManip(ref t, a.attack * (25 / 25 + t.def)); //HP decrease by target, attack strength * target's defense
                BattleUI.menu_alpha = 0f; //hide menu
                _spriteBatch.Draw(battle_message, new Rectangle(182, 336, 299, 128), Color.White);
                _spriteBatch.DrawString(large_font, "*" + a.name + " attacked " + t.name + "!", new Vector2(205, 361), Color.Black);
                _spriteBatch.DrawString(small_font, "*" + t.name + " took " + (a.attack * (25 / (25 + t.def))) + " damage!", new Vector2(205, 401), Color.Black);
                List<Entity> tr = new List<Entity>(); //to return
                tr.Add(a); //add attacker
                tr.Add(t); //add target
                return tr;
            }

            List<Entity> spattack(ref Entity a, ref Entity t, SpriteBatch _spriteBatch, Texture2D battle_message, SpriteFont medium_font) //magic attack process, (ref Entity attacker, ref Entity target)
            {
                hpManip(ref t, a.spattack * (75 / 75 + t.spdef)); //MP decrease by target, magic attack strength * target's magic defense
                manaManip(ref a, 1); //decrease MP of attacker by 1
                BattleUI.menu_alpha = 0f; //hide menu
                _spriteBatch.Draw(battle_message, new Rectangle(182, 336, 299, 128), Color.White);
                _spriteBatch.DrawString(medium_font, "*" + a.name + " used magic to attack " + t.name + "!", new Vector2(205, 361), Color.Black);
                List<Entity> tr = new List<Entity>(); //to return
                tr.Add(a); // add attacker
                tr.Add(t); //add target
                return tr;
            }

            void hpManip(ref Entity t, int change) //ref Entity target, int change
            {
                t.health -= change; //decrease target HP
                if (t.health < 1) //if HP < 1
                    t.health = 0; //set to 0 (and don't go below that)
                if (t.health > t.mHealth) //if current HP > max HP
                    t.health = t.mHealth; //set current HP = max HP (and don't go over)
            }

            void manaManip(ref Entity t, int change) //ref Entity target, int change
            {
                if (t.mana != 0 && change > 0) //if MP not already 0
                    t.mana -= change; //decrease target MP
                else if (change < 0) //Michael what does this mean
                    t.mana -= change;
                if (t.mana < 1) //if MP < 1
                    t.mana = 0; //set to 0
                if (t.mana > t.mMana) //if current MP > max MP
                    t.mana = t.mMana; //set current MP = max MP
            }

            bool BattleExit()
            {
                return true;
            }

            Entity healByFlat(ref Entity t, int change, SpriteBatch _spriteBatch, Texture2D battle_message, SpriteFont large_font) //ref Entity target, int change
            {
                hpManip(ref t, change * -1); //increase HP by flat rate
                BattleUI.menu_alpha = 0f; //hide menu
                _spriteBatch.Draw(battle_message, new Rectangle(182, 336, 299, 128), Color.White);
                _spriteBatch.DrawString(large_font, "*" + t.name + " was healed for " + change + " HP!", new Vector2(205, 361), Color.Black);
                return t;
            }

            Entity healByPerc(Entity t, int change, SpriteBatch _spriteBatch, Texture2D battle_message, SpriteFont large_font) //ref Entity target, int change
            {
                hpManip(ref t, (change * t.health) * -1); //increase HP by percentage of max
                BattleUI.menu_alpha = 0f; //hide menu
                _spriteBatch.Draw(battle_message, new Rectangle(182, 336, 299, 128), Color.White);
                _spriteBatch.DrawString(large_font, "*" + t.name + " was healed for " + change + "% HP!", new Vector2(205, 361), Color.Black);
                return t;
            }

            Entity itemEffect(ref Entity t, /*Entity a, item i, int hp, int mp, SpriteBatch _spriteBatch, Texture2D battle_message, SpriteFont large_font) //ref Entity target, int HP, int MP
            {
                hpManip(ref t, hp * -1); //increase HP by flat rate
                manaManip(ref t, mp * -1); //increase MP by flat rate
                BattleUI.menu_alpha = 0f; //hide menu
                _spriteBatch.Draw(battle_message, new Rectangle(182, 336, 299, 128), Color.White);
                _spriteBatch.DrawString(large_font, "*" + /*+ t.name + " is feeling the effects of " + i.name" wow, I can't believe I've done this", new Vector2(205, 361), Color.Black);
                return t;
            }
        }
    }
}*/