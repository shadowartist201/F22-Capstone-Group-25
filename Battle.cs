using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using MonoGame.Extended.Screens;

namespace Game_Demo
{
    public class Battle : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;
        public Battle(Game1 game) : base(game) { }

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
        public static bool tpk = false;
        public static int itemType = 0;
        public static List<int> atkbuf = new List<int>{};
        public static List<int> defbuf = new List<int>{};
        public static List<bool> atkbool = new List<bool>{};
        public static List<bool> defbool = new List<bool>{};

        public override void Initialize()
        {
            //new Entity(name, HP, max HP, MP, max MP, attack, magic attack, defense, magic defense, speed)
            //TO DO: clear this out, testing inputs will need this cleared
            Game1.enemies.Clear();
            Game1.enemies.Add(new Entity("Dragon", 180, 180, 0, 0, 10, 15, 0, 0));
            Game1.enemies.Add(new Entity("TestEntity", 10, 10, 0, 0, 0, 0, 0, 0));
            atkbuf.Add(0);
            defbuf.Add(0);
            atkbuf.Add(0);
            defbuf.Add(0);
            atkbool.Add(false);
            defbool.Add(false);
            atkbool.Add(false);
            defbool.Add(false);

            Game1.squad.Clear();
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
            endBattle = false;
            int totH = 0;
            int tTotH = 0;
            foreach (Entity e in Game1.enemies)
            {
                totH += e.health;
            }
            foreach (Entity e in Game1.squad)
            {
                tTotH += e.health;
            }
            if (totH < 1)
            {
                endBattle = true;
            }
            if(tTotH<1)
            {
                tpk = true;
            }

            if (endBattle)
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
            else if (tpk)
            {
                BattleUI.BattleEndBad(GraphicsDevice, _spriteBatch);

                if (Input.Hold() == "enter")
                {
                    Tiled.BattleReturn = true;
                    //Tiled.LoadMap("home", Content, GraphicsDevice);
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
                                currenemy = Game1.squad[Game1.squad.Count - 1];
                                //attack last if not dead, then move up, idk yet
                                int i = 1;
                                if (Game1.squad[0].health > 0)
                                {
                                    while (currenemy.health < 1)
                                    {
                                        i++;
                                        currenemy = Game1.squad[Game1.squad.Count - i];
                                    }
                                }
                                target = Game1.squad.Count - i;
                            }
                            List<Entity> returned = new List<Entity>();
                            BattleUI.menu_alpha = 0f; //hide menu
                            returned = attack(ref currchar, ref currenemy, _spriteBatch, BattleUI.battle_message, Game1.large_font, Game1.small_font);
                            if (BattleUI.squadTurn)
                            {
                                Game1.squad[BattleUI.current_character] = returned[0];
                                Game1.enemies[target] = returned[1];
                            }
                            else
                            {
                                Game1.squad[target] = returned[1];
                                Game1.enemies[BattleUI.current_character] = returned[0];
                            }
                            alratk = true; //set attacked flag to true
                        }
                        _spriteBatch.Draw(BattleUI.battle_message, new Rectangle(182, 336, 299, 128), Color.White);
                        if (BattleUI.squadTurn)
                            _spriteBatch.DrawString(Game1.large_font, "*" + Game1.squad[BattleUI.current_character].name + " attacked the " + Game1.enemies[target].name + "!", new Vector2(205, 361), Color.Black);
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
                                if (BattleUI.squadTurn)
                                {
                                    Game1.squad[BattleUI.current_character] = returned[0];
                                    Game1.enemies[target] = returned[1];
                                }
                                else
                                {
                                    Game1.squad[target] = returned[1];
                                    Game1.enemies[BattleUI.current_character] = returned[0];
                                }
                                alratk = true; //set attacked flag to true
                            }
                        }
                        if (Game1.squad[BattleUI.current_character].mana > 0)
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
                        //INSERT
                        BattleUI.menu_alpha = 0f;
                        Entity recieve = Game1.squad[0];
                        Game1.squad[0] = healByPerc(ref recieve, 15, _spriteBatch, BattleUI.battle_message, Game1.large_font);
                        if (Game1.squad.Count>2)
                        {
                            recieve = Game1.squad[2];
                            Game1.squad[2] = healByPerc(ref recieve, 15, _spriteBatch, BattleUI.battle_message, Game1.large_font);
                        }
                        alratk = true;
                        //INSERT
                    }
                    //INSERT
                    if(BattleUI.item_message)
                    {
                        BattleUI.menu_alpha = 0f;
                        Entity recieve = Game1.squad[BattleUI.current_character];
                        switch(itemType)
                        {
                            case 1:
                                Game1.squad[BattleUI.current_character] = healByFlat(ref recieve, 20, _spriteBatch, BattleUI.battle_message, Game1.large_font);
                                break;
                            case 2:
                                Game1.squad[BattleUI.current_character] = healByFlat(ref recieve, 50, _spriteBatch, BattleUI.battle_message, Game1.large_font);
                                break;
                            case 3:
                                Game1.squad[BattleUI.current_character] = manaByFlat(ref recieve, 50, _spriteBatch, BattleUI.battle_message, Game1.large_font);
                                break;
                            case 4:
                                Game1.squad[BattleUI.current_character] = itemEffect(ref recieve, Game1.inventory[BattleUI.inventory_index], 0, 0, _spriteBatch, BattleUI.battle_message, Game1.large_font);
                                Game1.squad[BattleUI.current_character] = new Entity(recieve.name, recieve.health, recieve.mHealth, recieve.mana, recieve.mMana, (int)((float)recieve.attack * 1.25), (int)((float)recieve.spattack * 1.1), recieve.def, recieve.spdef);
                                atkbuf[BattleUI.current_character] = 2;
                                atkbool[BattleUI.current_character] = true;
                                break;
                            case 5:
                                Game1.squad[BattleUI.current_character] = itemEffect(ref recieve, Game1.inventory[BattleUI.inventory_index], 0, 0, _spriteBatch, BattleUI.battle_message, Game1.large_font);
                                Game1.squad[BattleUI.current_character] = new Entity(recieve.name, recieve.health, recieve.mHealth, recieve.mana, recieve.mMana, recieve.attack, recieve.spattack, (int)((float)recieve.def*1.25), (int)((float)recieve.spdef*1.1));
                                defbuf[BattleUI.current_character] = 2;
                                defbool[BattleUI.current_character] = true;
                                break;
                            default:
                                break;
                        }
                        alratk = true;
                    }
                    //INSERT
                }
                _spriteBatch.End();
            }

            List<Entity> attack(ref Entity a, ref Entity t, SpriteBatch _spriteBatch, Texture2D battle_message, SpriteFont large_font, SpriteFont small_font) //attack process
            {
                int delta = (int)(a.attack * (float)(25 / (float)(25 + t.def)));
                hpManip(ref t, delta); //HP decrease by target, attack strength * target's defense
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
                int delta = (int)(a.spattack * (float)(75 / (float)(75 + t.spdef)));
                hpManip(ref t, delta); //MP decrease by target, magic attack strength * target's magic defense
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

            Entity healByPerc(ref Entity t, int change, SpriteBatch _spriteBatch, Texture2D battle_message, SpriteFont large_font) //ref Entity target, int change
            {
                hpManip(ref t, (change * t.health) * -1); //increase HP by percentage of max
                BattleUI.menu_alpha = 0f; //hide menu
                _spriteBatch.Draw(battle_message, new Rectangle(182, 336, 299, 128), Color.White);
                _spriteBatch.DrawString(large_font, "*" + t.name + " was healed for " + change + "% HP!", new Vector2(205, 361), Color.Black);
                return t;
            }
            //INSERT
            Entity manaByFlat(ref Entity t, int change, SpriteBatch _spriteBatch, Texture2D battle_message, SpriteFont large_font) //ref Entity target, int change
            {
                manaManip(ref t, change*-1); //restore by flat rate
                BattleUI.menu_alpha = 0f; //hide menu
                _spriteBatch.Draw(battle_message, new Rectangle(182, 336, 299, 128), Color.White);
                _spriteBatch.DrawString(large_font, "*" + t.name + " had " + change + " mana restored!", new Vector2(205, 361), Color.Black);
                return t;
            }
            
            Entity itemEffect(ref Entity t, Item i, int hp, int mp, SpriteBatch _spriteBatch, Texture2D battle_message, SpriteFont large_font) //ref Entity target, int HP, int MP
            {
                hpManip(ref t, hp * -1); //increase HP by flat rate
                manaManip(ref t, mp * -1); //increase MP by flat rate
                BattleUI.menu_alpha = 0f; //hide menu
                _spriteBatch.Draw(battle_message, new Rectangle(182, 336, 299, 128), Color.White);
                _spriteBatch.DrawString(large_font, "*" + t.name + " is feeling the effects of " + i.name, new Vector2(205, 361), Color.Black);
                return t;
            }
            //INSERT
        }
    }
}