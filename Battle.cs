using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using MonoGame.Extended.Screens;
using System;

namespace Game_Demo
{
    public class Battle : GameScreen
    {
        private new Game1 Game => (Game1)base.Game;
        public Battle(Game1 game) : base(game) { }

        public SpriteBatch _spriteBatch;  //batch of sprites

        private Texture2D player_battle;    //player texture
        private Texture2D dragon_battle;   //monster texture
        private Texture2D cat_battle;      //cat texture
        private Texture2D healing_effect;

        public static int target = 0;

        public static bool selection = false;

        public static bool postAttack = false; //already attacked
        public static bool endBattle = false;
        public static bool partyWipe = false;
        public static int itemType = 0;

        public static List<int> attackBuff = new List<int>{};
        public static List<int> defenseBuff = new List<int>{};
        public static List<bool> attackBool = new List<bool>{};
        public static List<bool> defenseBool = new List<bool>{};

        public override void Initialize()
        {
            Game1.enemies.Clear();
            Game1.enemies.Add(new Entity("Dragon", 180, 180, 0, 0, 10, 15, 0, 0));

            attackBuff.Add(0);
            defenseBuff.Add(0);
            attackBuff.Add(0);
            defenseBuff.Add(0);
            attackBool.Add(false);
            defenseBool.Add(false);
            attackBool.Add(false);
            defenseBool.Add(false);

            Game1.party.Clear();
            Game1.party.Add(new Entity("Nobody", 100, 100, 10, 10, 5, 10, 10, 12));
            Game1.party.Add(new Entity("Cat", 50, 50, 0, 0, 5, 10, 5, 7));

            base.Initialize();
        }

        public override void LoadContent()
        {
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
            //endBattle = false;
            int totalEnemyHP = 0;
            int totalPartyHP = 0;
            foreach (Entity entity in Game1.enemies)
            {
                totalEnemyHP += entity.HP;
            }
            foreach (Entity entity in Game1.party)
            {
                totalPartyHP += entity.HP;
            }
            if (BattleUI.output == "x")
            {
                //endBattle = true;
            }
            if(totalPartyHP <1)
            {
                partyWipe = true;
            }
            if (totalEnemyHP <1)
            {
                endBattle = true;
            }

            if (endBattle)
            {
                Entity thing = Game1.enemies[0];
                thing.HP = 0;
                Game1.enemies[0] = thing;

                BattleUI.BattleEnd(GraphicsDevice, _spriteBatch);

                if (Input.Hold() == "enter")
                    Tiled.BattleReturn = true;
            }
            else if (partyWipe)
            {
                Entity thing = Game1.party[0];
                thing.HP = 0;
                Game1.party[0] = thing;

                BattleUI.BattleEndBad(GraphicsDevice, _spriteBatch);

                if (Input.Hold() == "enter")
                    Tiled.BattleReturn = true;
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
                        if (!postAttack) //and not already attacked
                        {
                            Entity currentCharacter;
                            Entity currentEnemy;
                            if (BattleUI.partyTurn)
                            {
                                currentCharacter = Game1.party[BattleUI.current_character]; //set current character
                                currentEnemy = Game1.enemies[target]; //set current enemy
                            }
                            else
                            {
                                currentCharacter = Game1.enemies[BattleUI.current_character];
                                currentEnemy = Game1.party[Game1.party.Count - 1];
                                //attack last if not dead, then move up, idk yet
                                int i = 1;
                                if (Game1.party[0].HP > 0)
                                {
                                    while (currentEnemy.HP < 1)
                                    {
                                        i++;
                                        currentEnemy = Game1.party[Game1.party.Count - i];
                                    }
                                }
                                target = Game1.party.Count - i;
                            }
                            List<Entity> returned = new();
                            BattleUI.menu_alpha = 0f; //hide menu
                            returned = attack(ref currentCharacter, ref currentEnemy, _spriteBatch, BattleUI.battle_message, Game1.large_font, Game1.small_font);
                            if (BattleUI.partyTurn)
                            {
                                Game1.party[BattleUI.current_character] = returned[0];
                                Game1.enemies[target] = returned[1];
                            }
                            else
                            {
                                Game1.party[target] = returned[1];
                                Game1.enemies[BattleUI.current_character] = returned[0];
                            }
                            postAttack = true; //set attacked flag to true
                        }
                        _spriteBatch.Draw(BattleUI.battle_message, new Rectangle(182, 336, 299, 128), Color.White);

                        if (BattleUI.partyTurn)
                            _spriteBatch.DrawString(Game1.large_font, "*" + Game1.party[BattleUI.current_character].name + " attacked the " + Game1.enemies[target].name + "!", new Vector2(205, 361), Color.Black);
                        else
                            _spriteBatch.DrawString(Game1.large_font, "*" + Game1.enemies[BattleUI.current_character].name + " attacked " + Game1.party[target].name + "!", new Vector2(205, 361), Color.Black);
                    }

                    if (BattleUI.magic_message) //when magic message activated
                    {
                        if ((Game1.party[BattleUI.current_character].MP > 0 && BattleUI.partyTurn) || (Game1.enemies[BattleUI.current_character].MP > 0 && !BattleUI.partyTurn)) //if current character has MP
                        {
                            if (!postAttack) //and not already attacked
                            {
                                Entity currentCharacter = Game1.party[BattleUI.current_character]; //set current character
                                Entity currentEnemy = Game1.enemies[0]; //set current enemy
                                List<Entity> returned = new List<Entity>();
                                BattleUI.menu_alpha = 0f; //hide menu
                                returned = spattack(ref currentCharacter, ref currentEnemy, _spriteBatch, BattleUI.battle_message, Game1.medium_font);
                                if (BattleUI.partyTurn)
                                {
                                    Game1.party[BattleUI.current_character] = returned[0];
                                    Game1.enemies[target] = returned[1];
                                }
                                else
                                {
                                    Game1.party[target] = returned[1];
                                    Game1.enemies[BattleUI.current_character] = returned[0];
                                }
                                postAttack = true; //set attacked flag to true
                            }
                        }
                        if (Game1.party[BattleUI.current_character].MP > 0)
                        {
                            _spriteBatch.Draw(BattleUI.battle_message, new Rectangle(182, 336, 299, 128), Color.White);
                            _spriteBatch.DrawString(Game1.large_font, "*" + Game1.party[BattleUI.current_character].name + " summoned fire!", new Vector2(205, 361), Color.Black);
                        }
                        else
                        {
                            _spriteBatch.Draw(BattleUI.battle_message, new Rectangle(182, 336, 299, 128), Color.White);
                            _spriteBatch.DrawString(Game1.large_font, "*" + Game1.party[BattleUI.current_character].name + " is out of mana!", new Vector2(205, 361), Color.Black);
                        }
                    }

                    if (BattleUI.cat_magic_message) //when cat message message activated, draw it
                    {
                        if (!postAttack)
                        {
                            BattleUI.menu_alpha = 0f;
                            Entity recieve = Game1.party[0];
                            Game1.party[0] = healByPerc(ref recieve, 15, _spriteBatch, BattleUI.battle_message, Game1.large_font);
                            if (Game1.party.Count > 2)
                            {
                                recieve = Game1.party[2];
                                Game1.party[2] = healByPerc(ref recieve, 15, _spriteBatch, BattleUI.battle_message, Game1.large_font);
                            }
                        }
                        postAttack = true;
                    }

                    if(BattleUI.item_message)
                    {
                        BattleUI.menu_alpha = 0f;
                        //if (!postAttack)
                        //{
                            Entity recieve = Game1.party[BattleUI.current_character];
                            switch (itemType)
                            {
                                case 1:
                                    Game1.party[BattleUI.current_character] = healByFlat(ref recieve, 20, _spriteBatch, BattleUI.battle_message, Game1.large_font);
                                    break;
                                case 2:
                                    Game1.party[BattleUI.current_character] = healByFlat(ref recieve, 50, _spriteBatch, BattleUI.battle_message, Game1.large_font);
                                    break;
                                case 3:
                                    Game1.party[BattleUI.current_character] = manaByFlat(ref recieve, 50, _spriteBatch, BattleUI.battle_message, Game1.large_font);
                                    break;
                                case 4:
                                    Game1.party[BattleUI.current_character] = itemEffect(ref recieve, Game1.inventory[BattleUI.inventory_index], 0, 0, _spriteBatch, BattleUI.battle_message, Game1.large_font);
                                    Game1.party[BattleUI.current_character] = new Entity(recieve.name, recieve.HP, recieve.maxHP, recieve.MP, recieve.maxMP, (int)Math.Floor((float)recieve.attack * (float)1.25), (int)Math.Floor((float)recieve.spAttack * (float)1.1), recieve.defense, recieve.spDefense);
                                    attackBuff[BattleUI.current_character] = 2;
                                    attackBool[BattleUI.current_character] = true;
                                    break;
                                case 5:
                                    Game1.party[BattleUI.current_character] = itemEffect(ref recieve, Game1.inventory[BattleUI.inventory_index], 0, 0, _spriteBatch, BattleUI.battle_message, Game1.large_font);
                                    Game1.party[BattleUI.current_character] = new Entity(recieve.name, recieve.HP, recieve.maxHP, recieve.MP, recieve.maxMP, recieve.attack, recieve.spAttack, (int)Math.Floor((float)recieve.defense * (float)1.25), (int)Math.Floor((float)recieve.spDefense * (float)1.1));
                                    defenseBuff[BattleUI.current_character] = 2;
                                    defenseBool[BattleUI.current_character] = true;
                                    break;
                                default:
                                    break;
                            }
                        //}
                        postAttack = true;
                    }
                }
                _spriteBatch.End();
            }
        }

        public static void advanceTurn()
        {
            BattleUI.current_character += 1;
            BattleUI.inventory_index = 0;
            if (BattleUI.current_character > Game1.party.Count - 1 && BattleUI.partyTurn) 
            {
                BattleUI.current_character = 0; 
                BattleUI.partyTurn = false;
                target = 0;
            }
            else if (BattleUI.current_character > Game1.enemies.Count - 1 && !BattleUI.partyTurn)
            {
                BattleUI.current_character = 0;
                BattleUI.partyTurn = true;
                target = 0;
            }
            if (BattleUI.partyTurn && attackBuff[BattleUI.current_character] > 0)
                attackBuff[BattleUI.current_character]--;

            if (attackBuff[BattleUI.current_character] == 0 && attackBool[BattleUI.current_character])
            {
                Game1.party[BattleUI.current_character] = Game1.party[BattleUI.current_character] = new Entity(Game1.party[BattleUI.current_character].name, Game1.party[BattleUI.current_character].HP, Game1.party[BattleUI.current_character].maxHP, Game1.party[BattleUI.current_character].MP, Game1.party[BattleUI.current_character].maxMP, (int)Math.Ceiling((float)Game1.party[BattleUI.current_character].attack / (float)1.25), (int)Math.Ceiling((float)Game1.party[BattleUI.current_character].spAttack / (float)1.1), Game1.party[BattleUI.current_character].defense, Game1.party[BattleUI.current_character].spDefense);
                attackBool[BattleUI.current_character] = false;
            }
            if (BattleUI.partyTurn && Battle.defenseBuff[BattleUI.current_character] > 0)
                Battle.defenseBuff[BattleUI.current_character]--;

            if (Battle.defenseBuff[BattleUI.current_character] == 0 && Battle.defenseBool[BattleUI.current_character])
            {
                Game1.party[BattleUI.current_character] = Game1.party[BattleUI.current_character] = new Entity(Game1.party[BattleUI.current_character].name, Game1.party[BattleUI.current_character].HP, Game1.party[BattleUI.current_character].maxHP, Game1.party[BattleUI.current_character].MP, Game1.party[BattleUI.current_character].maxMP, Game1.party[BattleUI.current_character].attack, Game1.party[BattleUI.current_character].spAttack, (int)Math.Ceiling((float)Game1.party[BattleUI.current_character].defense / (float)1.25), (int)Math.Ceiling((float)Game1.party[BattleUI.current_character].spDefense / (float)1.1));
                Battle.defenseBool[BattleUI.current_character] = false;
            }
            if (BattleUI.partyTurn)
            {
                if (Game1.party[BattleUI.current_character].HP < 1)
                    advanceTurn();
            }
            else
            {
                if (Game1.enemies[BattleUI.current_character].HP < 1)
                    advanceTurn();
            }
        }

        List<Entity> attack(ref Entity attacker, ref Entity target, SpriteBatch _spriteBatch, Texture2D battle_message, SpriteFont large_font, SpriteFont small_font) //attack process
        {
            int delta = (int)(attacker.attack * (float)(25 / (float)(25 + target.defense)));
            hpManip(ref target, delta); //HP decrease by target, attack strength * target's defense
            BattleUI.menu_alpha = 0f; //hide menu

            _spriteBatch.Draw(battle_message, new Rectangle(182, 336, 299, 128), Color.White);
            _spriteBatch.DrawString(large_font, "*" + attacker.name + " attacked " + target.name + "!", new Vector2(205, 361), Color.Black);
            _spriteBatch.DrawString(small_font, "*" + target.name + " took " + (attacker.attack * (25 / (25 + target.defense))) + " damage!", new Vector2(205, 401), Color.Black);

            return new List<Entity> { attacker, target };
        }
        List<Entity> spattack(ref Entity attacker, ref Entity target, SpriteBatch _spriteBatch, Texture2D battle_message, SpriteFont medium_font) //magic attack process, (ref Entity attacker, ref Entity targetarget)
        {
            int delta = (int)(attacker.spAttack * (float)(75 / (float)(75 + target.spDefense)));
            hpManip(ref target, delta); //MP decrease by target, magic attack strength * target's magic defense
            manaManip(ref attacker, 1); //decrease MP of attacker by 1
            BattleUI.menu_alpha = 0f; //hide menu

            _spriteBatch.Draw(battle_message, new Rectangle(182, 336, 299, 128), Color.White);
            _spriteBatch.DrawString(medium_font, "*" + attacker.name + " used magic to attack " + target.name + "!", new Vector2(205, 361), Color.Black);

            return new List<Entity> { attacker, target };
        }

        void hpManip(ref Entity target, int change) //ref Entity targetarget, int change
        {
            target.HP -= change; //decrease target HP

            if (target.HP < 1) //if HP < 1
                target.HP = 0; //set to 0 (and don't go below that)
            if (target.HP > target.maxHP) //if current HP > max HP
                target.HP = target.maxHP; //set current HP = max HP (and don't go over)
        }
        void manaManip(ref Entity target, int change) //ref Entity targetarget, int change
        {
            if (target.MP != 0 && change > 0) //if MP not already 0
                target.MP -= change; //decrease target MP
            else if (change < 0) //Michael what does this mean
                target.MP -= change;
            if (target.MP < 1) //if MP < 1
                target.MP = 0; //set to 0
            if (target.MP > target.maxMP) //if current MP > max MP
                target.MP = target.maxMP; //set current MP = max MP
        }

        Entity healByFlat(ref Entity target, int change, SpriteBatch _spriteBatch, Texture2D battle_message, SpriteFont large_font) //ref Entity targetarget, int change
        {
            hpManip(ref target, change * -1); //increase HP by flat rate
            BattleUI.menu_alpha = 0f; //hide menu

            _spriteBatch.Draw(battle_message, new Rectangle(182, 336, 299, 128), Color.White);
            _spriteBatch.DrawString(large_font, "*" + target.name + " was healed for " + change + " HP!", new Vector2(205, 361), Color.Black);

            return target;
        }
        Entity healByPerc(ref Entity target, int change, SpriteBatch _spriteBatch, Texture2D battle_message, SpriteFont large_font) //ref Entity targetarget, int change
        {
            hpManip(ref target, (change * target.HP) * -1); //increase HP by percentage of max
            BattleUI.menu_alpha = 0f; //hide menu

            _spriteBatch.Draw(battle_message, new Rectangle(182, 336, 299, 128), Color.White);
            _spriteBatch.DrawString(large_font, "*" + target.name + " was healed for " + change + "% HP!", new Vector2(205, 361), Color.Black);

            return target;
        }
        Entity manaByFlat(ref Entity target, int change, SpriteBatch _spriteBatch, Texture2D battle_message, SpriteFont large_font) //ref Entity targetarget, int change
        {
            manaManip(ref target, change * -1); //restore by flat rate
            BattleUI.menu_alpha = 0f; //hide menu

            _spriteBatch.Draw(battle_message, new Rectangle(182, 336, 299, 128), Color.White);
            _spriteBatch.DrawString(large_font, "*" + target.name + " had " + change + " mana restored!", new Vector2(205, 361), Color.Black);

            return target;
        }
        Entity itemEffect(ref Entity target, Item item, int hp, int mp, SpriteBatch _spriteBatch, Texture2D battle_message, SpriteFont large_font) //ref Entity targetarget, int HP, int MP
        {
            hpManip(ref target, hp * -1); //increase HP by flat rate
            manaManip(ref target, mp * -1); //increase MP by flat rate
            BattleUI.menu_alpha = 0f; //hide menu

            _spriteBatch.Draw(battle_message, new Rectangle(182, 336, 299, 128), Color.White);
            _spriteBatch.DrawString(large_font, "*" + target.name + " is feeling the effects of " + item.name, new Vector2(205, 361), Color.Black);

            return target;
        }
    }
}