using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata;

namespace Game_Demo
{
    public class Battle : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;  //batch of sprites

        private Texture2D player;    //player texture
        private Texture2D dragon;   //monster texture
        private Texture2D cat;      //cat texture

        private Texture2D battle_message;  //center message box
        private Texture2D inventory_box;   //inventory menu
        private Texture2D menu_box;        //action menu
        private Texture2D party_info;      //holds names and HP
        private Texture2D enemy_box;       //holds enemy name

        private Texture2D menu_up;         //action menu up arrow
        private Texture2D menu_down;       //action menu down arrow
        private Texture2D item_selection;  //red selection box
        private Texture2D current_fighter; //blue triangle to show current turn

        private Texture2D healing_effect;

        private SpriteFont small_font;
        private SpriteFont medium_font;
        private SpriteFont large_font;

        private Texture2D hp_bar;       //empty bar
        private Texture2D bar_fill;     //that which fills the bar

        List<Entity> enemies = new List<Entity>();
        List<Entity> squad = new List<Entity>();
        
        private int player_hp_curr = 100; //current player HP, should be initialized as player's max HP
        private int cat_hp_curr = 50;
        private int player_mp_curr = 10;

        private int dragon_hp_curr = 200;

        private int selection_index = 1;  //shows which action menu option is selected
        private int current_character = 1;  //which character's turn is it, where 1 = Nobody and 2 = Cat
        private string[] characters = { null, "Nobody", "Cat" };  //array to hold character names

        private float menu_alpha = 0.0f;  //action menu visibility, 0 = hidden and 1 = show
        private float inventory_alpha = 0.0f; //inventory menu visibility
        private float message_alpha = 1.0f;  //message box visibility, initialized to 1 for initial message

        private bool initial_message = true; //initial message "A <thing> appeared!"
        private bool attack_message = false; //message "<chara> attacked!"
        private bool magic_message = false;  //message "<chara> used magic!"
        private bool cat_magic_message = false; //message "Cats can't use magic"
        private bool flee_message = false; //message when attempting to flee battle
        private bool alratk = false; //already attacked

        private KeyboardState oldState;

        public Battle()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content/Battle";
            IsMouseVisible = true;
        }

        public Battle(List<Entity> e, List<Entity> s)
        {
            _graphics = new GraphicsDeviceManager(this);
            enemies = e;
            squad = s;
            Content.RootDirectory = "Content/Battle";
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);  //initialize the batch

            player = Content.Load<Texture2D>("player-battle");    //load the player texture
            dragon = Content.Load<Texture2D>("dragon-battle");  //load the monster texture
            cat = Content.Load<Texture2D>("cat-battle");      //load the cat texture

            hp_bar = Content.Load<Texture2D>("hp-bar");
            bar_fill = Content.Load<Texture2D>("bar-fill");

            battle_message = Content.Load<Texture2D>("battle-message-box");
            inventory_box = Content.Load<Texture2D>("inventory-box");
            menu_box = Content.Load<Texture2D>("menu-box");
            party_info = Content.Load<Texture2D>("party-info-box");
            enemy_box = Content.Load<Texture2D>("enemy-name-box");

            menu_up = Content.Load<Texture2D>("menu-up");
            menu_down = Content.Load<Texture2D>("menu-down");
            item_selection = Content.Load<Texture2D>("item-selection");
            current_fighter = Content.Load<Texture2D>("current-fighter");

            healing_effect = Content.Load<Texture2D>("cat-battle");

            small_font = Content.Load<SpriteFont>("small");
            medium_font = Content.Load<SpriteFont>("medium");
            large_font = Content.Load<SpriteFont>("large");
        }

        /*
              Because Update() runs multiple times per second, pressing a key can
              keep it in the "activated" state for multiple Update() loops. You
              can see this in the World demo when you hit the wall and the sound
              effect plays repeatedely. To avoid this, we edit the if() statements
              to check the status from last loop.
        
              If a key was deactivated last loop and freshly activated for this
              loop, this reflects the "single press" behavior. We check for this
              in Battle demo by using oldState and newState.
        */

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState newState = Keyboard.GetState();


            if (flee_message)  //if flee message activated
            {
                if (newState.IsKeyDown(Keys.Enter) && !oldState.IsKeyDown(Keys.Enter))  //check for enter pressed
                {
                    flee_message = false;  //disable flee message
                    message_alpha = 0;    //hide message box
                    if (current_character == 1)  //move to next character
                        current_character = 2;
                    else if (current_character == 2)
                        current_character = 1;
                }
            }
            else if (attack_message)  //if attack message activated
            {
                if (newState.IsKeyDown(Keys.Enter) && !oldState.IsKeyDown(Keys.Enter))  //check for enter
                {
                    attack_message = false;   //disable message
                    message_alpha = 0;        //hide box
                    /*if (dragon_hp_curr != 0)  //lower enemy HP and stop at 0 so we don't go negative
                        dragon_hp_curr = dragon_hp_curr - 10*/;
                    if (current_character == 1)  //move to next character
                        current_character = 2;
                    else if (current_character == 2)
                        current_character = 1;
                }
            }
            else if (magic_message)  //if magic message activated
            {
                if (newState.IsKeyDown(Keys.Enter) && !oldState.IsKeyDown(Keys.Enter))  //check for enter
                {
                    magic_message = false;  //disable message
                    message_alpha = 0;      //hide box
                    /*if (player_mp_curr != 0)   //lower player MP and stop at 0
                        player_mp_curr--;
                    if (dragon_hp_curr != 0)   //lower enemy HP and stop at 0
                        dragon_hp_curr = dragon_hp_curr - 20;*/
                    if (squad[current_character - 1].mana > 0)
                    {
                        if (current_character == 1)  //move to next character
                            current_character = 2;
                        else if (current_character == 2)
                            current_character = 1;
                    }
                }
            }
            else if (cat_magic_message)  //if cat magic message activated
            {
                if (newState.IsKeyDown(Keys.Enter) && !oldState.IsKeyDown(Keys.Enter))  //check for enter
                {
                    cat_magic_message = false;  //disable message
                    message_alpha = 0;   //hide box
                }
            }


            else if (message_alpha == 0)  //if no messages activated
            {
                    menu_alpha = 1; //enable action menu
                    if (oldState.IsKeyUp(Keys.Back) && newState.IsKeyDown(Keys.Back))  //check for backspace
                    {
                        if (inventory_alpha == 1)  //if inventory menu activated, hide it
                            inventory_alpha = 0;
                    }
                    if (oldState.IsKeyUp(Keys.Up) && newState.IsKeyDown(Keys.Up))  //check for up
                    {
                        if (selection_index != 1)  //move selection box up (and stop at 1 so we don't go out of bounds)
                            selection_index--;
                    }
                    if (oldState.IsKeyUp(Keys.Down) && newState.IsKeyDown(Keys.Down))  //check for down
                    {
                        if (selection_index != 4)  //move selection box down (and stop at 4 so we don't go out of bounds)
                            selection_index++;
                    }
                    if (oldState.IsKeyUp(Keys.Enter) && newState.IsKeyDown(Keys.Enter))  //check for enter
                    {
                        if (selection_index == 1) //attack
                        {
                            attack_message = true;  //enable attack message
                            menu_alpha = 0;    //hide action menu
                            message_alpha = 1;   //show message box
                            alratk = false;
                        }
                        else if (selection_index == 2) //magic
                        {
                            if (current_character == 1)
                                magic_message = true;  //enable magic message
                            else
                                cat_magic_message = true;
                            menu_alpha = 0;    //hide action menu
                            message_alpha = 1;  //show message box
                            alratk = false;
                        }
                        else if (selection_index == 3) //item
                        {
                            inventory_alpha = 1;  //show inventory menu
                        }
                        else if (selection_index == 4) //flee
                        {
                            flee_message = true;  //enable flee message
                            menu_alpha = 0;  //hide action menu
                            message_alpha = 1;  //show message box
                        }
                    }
                
            }

            oldState = newState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (enemies[0].health == 0)  //if dragon defeated
            {
                GraphicsDevice.Clear(Color.Black);
                _spriteBatch.Begin();
                _spriteBatch.DrawString(large_font, "Congrat, you is winner", new Vector2(300, 226), Color.White);
                _spriteBatch.End();
            }
            else  //if battle still going
            {
                GraphicsDevice.Clear(Color.Green);  //background color

                _spriteBatch.Begin();

                //control text
                _spriteBatch.DrawString(medium_font, "Enter - Select", new Vector2(310, 81), Color.White);
                _spriteBatch.DrawString(medium_font, "Backspace - Back", new Vector2(310, 98), Color.White);
                _spriteBatch.DrawString(medium_font, "Up/Down/Left/Right", new Vector2(310, 115), Color.White);

                //entities
                _spriteBatch.Draw(player, new Rectangle(630, 60, 54, 77), Color.White);   //draw player sprite
                _spriteBatch.Draw(dragon, new Rectangle(70, 35, 239, 246), Color.White);   //draw monster sprite
                _spriteBatch.Draw(cat, new Rectangle(630, 185, 71, 55), Color.White);  //draw cat sprite

                //box set 1
                _spriteBatch.Draw(enemy_box, new Rectangle(14, 336, 160, 128), Color.White);
                _spriteBatch.Draw(party_info, new Rectangle(488, 336, 301, 128), Color.White);

                //enemy info
                /*
                 * foreach(Entity e in enemies)
                 * {
                 *  _spriteBatch.DrawString(medium_font, e.name, new Vector(41, 361+(i*43)), Color.Black);
                 * }
                 */
                _spriteBatch.DrawString(large_font, "Dragon", new Vector2(41, 361), Color.Black);

                //party info
                /*
                 * foreach(Entity e in squad)
                 * {
                 *  _spriteBatch.DrawString(medium_font, e.name, new Vector(510, 360+(i*19)), Color.Black);
                 * }
                 */
                _spriteBatch.DrawString(medium_font, "Nobody", new Vector2(510, 360), Color.Black);
                _spriteBatch.DrawString(medium_font, "Cat", new Vector2(510, 379), Color.Black);
                _spriteBatch.DrawString(small_font, "NAME", new Vector2(512, 345), Color.Black);
                _spriteBatch.DrawString(small_font, "HP", new Vector2(612, 345), Color.Black);
                _spriteBatch.DrawString(small_font, "MP", new Vector2(707, 345), Color.Black);

                if (menu_alpha == 1f) //action menu
                {
                    //draw menu options
                    _spriteBatch.Draw(menu_box, new Rectangle(119, 308, 150, 155), Color.White);
                    _spriteBatch.DrawString(large_font, "Attack", new Vector2(139, 334), Color.Black);
                    _spriteBatch.DrawString(large_font, "Magic", new Vector2(139, 361), Color.Black);
                    _spriteBatch.DrawString(large_font, "Item", new Vector2(139, 390), Color.Black);///TO DO: actually properly implement
                    _spriteBatch.DrawString(large_font, "Flee", new Vector2(139, 417), Color.Black);

                    if (current_character == 1) //if Nobody's turn, put triangle next to their name
                    {
                        _spriteBatch.Draw(current_fighter, new Rectangle(495, 360, 12, 14), Color.White);
                    }
                    else if (current_character == 2) //if Cat's turn, put triangle next to their name
                    {
                        _spriteBatch.Draw(current_fighter, new Rectangle(495, 379, 12, 14), Color.White);
                    }

                    if (inventory_alpha == 0f) //while inventory menu hidden, enable red selection box
                        _spriteBatch.Draw(item_selection, new Rectangle(130, 304 + (27 * selection_index), 105, 29), Color.White);
                }

                if (inventory_alpha == 1f) //inventory menu
                {
                    //draw inventory items
                    _spriteBatch.Draw(inventory_box, new Rectangle(213, 308, 273, 155), Color.White);
                    _spriteBatch.Draw(menu_up, new Rectangle(254, 313, 13, 11), Color.White);
                    _spriteBatch.Draw(menu_down, new Rectangle(254, 443, 13, 11), Color.White);
                    _spriteBatch.DrawString(large_font, "Potion", new Vector2(234, 334), Color.Black);
                    _spriteBatch.DrawString(large_font, "----------", new Vector2(234, 361), Color.Black);
                    _spriteBatch.DrawString(large_font, "----------", new Vector2(234, 390), Color.Black);
                    _spriteBatch.DrawString(large_font, "----------", new Vector2(234, 417), Color.Black);
                    _spriteBatch.DrawString(small_font, "DETAILS", new Vector2(343, 314), Color.Black);

                    if (selection_index == 1) //change info text based on item
                        _spriteBatch.DrawString(medium_font, "Example text potion", new Vector2(342, 335), Color.Black);
                    else if (selection_index == 2)
                        _spriteBatch.DrawString(medium_font, "Blank 1", new Vector2(342, 335), Color.Black);
                    else if (selection_index == 3)
                        _spriteBatch.DrawString(medium_font, "Blank 2", new Vector2(342, 335), Color.Black);
                    else if (selection_index == 4)
                        _spriteBatch.DrawString(medium_font, "Blank 3", new Vector2(342, 335), Color.Black);

                    _spriteBatch.Draw(item_selection, new Rectangle(225, 304 + (27 * selection_index), 105, 29), Color.White);  //red selection box
                }

                if (message_alpha == 1f) //message box
                {
                    if (initial_message) //when initial message activated, draw it
                    {
                        _spriteBatch.Draw(battle_message, new Rectangle(182, 336, 299, 128), Color.White);
                        _spriteBatch.DrawString(large_font, "*A dragon appeared!", new Vector2(205, 361), Color.Black);
                        if (oldState.IsKeyDown(Keys.Enter))  //if enter pressed, hide message
                        {
                            initial_message = false;
                            message_alpha = 0f;
                        }
                    }
                    if (flee_message) //when flee message activated, draw it
                    {
                        menu_alpha = 0f;
                        _spriteBatch.Draw(battle_message, new Rectangle(182, 336, 299, 128), Color.White);
                        _spriteBatch.DrawString(large_font, "*You can't stop now!", new Vector2(205, 361), Color.Black);
                    }
                    if (attack_message) //when attack message activated, draw it
                    {
                        if (!alratk) //and not already attacked
                        {
                            Entity currchar = squad[current_character - 1]; //set current character
                            Entity currenemy = enemies[0]; //set current enemy
                            List<Entity> returned = new List<Entity>(); 
                            menu_alpha = 0f; //hide menu
                            returned = attack(ref currchar, ref currenemy);
                            squad[current_character - 1] = returned[0];
                            enemies[0] = returned[1];
                            alratk = true; //set attacked flag to true
                        }
                        _spriteBatch.Draw(battle_message, new Rectangle(182, 336, 299, 128), Color.White);
                        _spriteBatch.DrawString(large_font, "*" + characters[current_character] + " attacked the dragon!", new Vector2(205, 361), Color.Black);
                    }
                    if (magic_message) //when magic message activated, draw it
                    {
                        if (squad[current_character - 1].mana > 0) //if current character has MP
                        {
                            if (!alratk) //and not already attacked
                            {
                                Entity currchar = squad[current_character - 1]; //set current character
                                Entity currenemy = enemies[0]; //set current enemy
                                List<Entity> returned = new List<Entity>(); 
                                menu_alpha = 0f; //hide menu
                                returned = spattack(ref currchar, ref currenemy);
                                squad[current_character - 1] = returned[0];
                                enemies[0] = returned[1];
                                alratk = true; //set attacked flag to true
                            }
                        }
                        if (squad[current_character-1].mana>0)
                        {
                            _spriteBatch.Draw(battle_message, new Rectangle(182, 336, 299, 128), Color.White);
                            _spriteBatch.DrawString(large_font, "*" + characters[current_character] + " summoned fire!", new Vector2(205, 361), Color.Black);
                        }
                        else
                        {
                            _spriteBatch.Draw(battle_message, new Rectangle(182, 336, 299, 128), Color.White);
                            _spriteBatch.DrawString(large_font, "*" + characters[current_character] + " is out of mana!", new Vector2(205, 361), Color.Black);
                        }
                    }
                    if (cat_magic_message) //when cat message message activated, draw it
                    {
                        menu_alpha = 0f;
                        _spriteBatch.Draw(battle_message, new Rectangle(182, 336, 299, 128), Color.White);
                        _spriteBatch.DrawString(large_font, "*Cats can't do magic, silly!", new Vector2(205, 361), Color.Black);
                    }
                }
                /// TO DO:
                /// replace most instances of hp_curr with associated Entity.healh
                
                int placediff = 0; //placing difference based on current character
                foreach(Entity e in squad) //Entity entity in party
                {
                    _spriteBatch.Draw(hp_bar, new Rectangle(609, 361+placediff, 78, 17), Color.White); //entity's HP, draw based on placediff offset
                    _spriteBatch.Draw(bar_fill, new Rectangle(612, 364+placediff, ((int)((float)e.health / (float)e.mHealth * 72)), 11), Color.Green); //fill based on placediff offset
                    _spriteBatch.DrawString(small_font, e.health + " / " + e.mHealth, new Vector2(618, 364+placediff), Color.Black); //draw visible value based on placediff offset
                    if(e.mMana!=0) //if MP not 0
                    {
                        _spriteBatch.Draw(hp_bar, new Rectangle(703, 361, 78, 17), Color.White); //entity's MP box
                        _spriteBatch.Draw(bar_fill, new Rectangle(706, 364, ((int)((float)e.mana / (float)e.mMana * 72)), 11), Color.MediumBlue); //entity's MP bar
                        _spriteBatch.DrawString(small_font, e.mana + " / " + e.mMana, new Vector2(732, 364), Color.Black); //entity's visible MP value
                    }
                    else
                    {
                        _spriteBatch.Draw(hp_bar, new Rectangle(703, 380, 78, 17), Color.White); //entity's MP, manaless
                        _spriteBatch.Draw(bar_fill, new Rectangle(706, 383, 72, 11), Color.DarkGray);
                        _spriteBatch.DrawString(small_font, "XX", new Vector2(758, 383), Color.Black);
                    }
                    placediff+=19;
                }
                placediff = 0;
                 
                /*//player hp
                _spriteBatch.Draw(hp_bar, new Rectangle(609, 361, 78, 17), Color.White); //player's HP
                _spriteBatch.Draw(bar_fill, new Rectangle(612, 364, ((int)(player_hp_curr / 100.0 * 72)), 11), Color.Green);
                _spriteBatch.DrawString(small_font, player_hp_curr + " / 100", new Vector2(618, 364), Color.Black);

                //player mp
                
                _spriteBatch.Draw(hp_bar, new Rectangle(703, 361, 78, 17), Color.White); //player's MP
                _spriteBatch.Draw(bar_fill, new Rectangle(706, 364, ((int)(player_mp_curr / 10.0 * 72)), 11), Color.MediumBlue);
                _spriteBatch.DrawString(small_font, player_mp_curr + " / 10", new Vector2(732, 364), Color.Black);

                //cat hp
                _spriteBatch.Draw(hp_bar, new Rectangle(609, 380, 78, 17), Color.White); //cat's HP
                _spriteBatch.Draw(bar_fill, new Rectangle(612, 383, ((int)(cat_hp_curr / 50.0 * 72)), 11), Color.Green);
                _spriteBatch.DrawString(small_font, cat_hp_curr + " / 50", new Vector2(625, 383), Color.Black);

                //cat mp
                
                _spriteBatch.Draw(hp_bar, new Rectangle(703, 380, 78, 17), Color.White); //cat's MP, 72px space within
                _spriteBatch.Draw(bar_fill, new Rectangle(706, 383, 72, 11), Color.DarkGray);
                _spriteBatch.DrawString(small_font, "XX", new Vector2(758, 383), Color.Black);*/

                //dragon hp
                _spriteBatch.DrawString(medium_font, "Debug HP: " + enemies[0].health + " / " + enemies[0].mHealth, new Vector2(325, 217), Color.Cyan);
                ///TO DO:
                ///maybe a small hp/mana bar for each enemy on the field?
                _spriteBatch.End();
            }

            base.Draw(gameTime);
        }

        List<Entity> attack(ref Entity a, ref Entity t) //attack process
        {
            hpManip(ref t, a.attack * (25 / 25 + t.def)); //HP decrease by target, attack strength * target's defense
            menu_alpha = 0f; //hide menu
            _spriteBatch.Draw(battle_message, new Rectangle(182, 336, 299, 128), Color.White);
            _spriteBatch.DrawString(large_font, "*" + a.name + " attacked " + t.name + "!", new Vector2(205, 361), Color.Black);
            _spriteBatch.DrawString(small_font, "*" + t.name + " took " + (a.attack * (25 / (25 + t.def))) + " damage!", new Vector2(205, 401), Color.Black);
            List<Entity> tr = new List<Entity>(); //to return
            tr.Add(a); //add attacker
            tr.Add(t); //add target
            return tr;
        }

        List<Entity> spattack(ref Entity a, ref Entity t) //magic attack process, (ref Entity attacker, ref Entity target)
        {
            hpManip(ref t, a.spattack * (75 / 75 + t.spdef)); //MP decrease by target, magic attack strength * target's magic defense
            manaManip(ref a, 1); //decrease MP of attacker by 1
            menu_alpha = 0f; //hide menu
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

        Entity healByFlat(ref Entity t, int change) //ref Entity target, int change
        {
            hpManip(ref t, change * -1); //increase HP by flat rate
            menu_alpha = 0f; //hide menu
            _spriteBatch.Draw(battle_message, new Rectangle(182, 336, 299, 128), Color.White);
            _spriteBatch.DrawString(large_font, "*" + t.name + " was healed for " + change + " HP!", new Vector2(205, 361), Color.Black);
            return t;
        }

        Entity healByPerc(Entity t, int change) //ref Entity target, int change
        {
            hpManip(ref t, (change*t.health) * -1); //increase HP by percentage of max
            menu_alpha = 0f; //hide menu
            _spriteBatch.Draw(battle_message, new Rectangle(182, 336, 299, 128), Color.White);
            _spriteBatch.DrawString(large_font, "*" + t.name + " was healed for " + change + "% HP!", new Vector2(205, 361), Color.Black);
            return t;
        }

        Entity itemEffect(ref Entity t, /*Entity a, item i,*/ int hp, int mp) //ref Entity target, int HP, int MP
        {
            hpManip(ref t,hp*-1); //increase HP by flat rate
            manaManip(ref t, mp*-1); //increase MP by flat rate
            menu_alpha = 0f; //hide menu
            _spriteBatch.Draw(battle_message, new Rectangle(182, 336, 299, 128), Color.White);
            _spriteBatch.DrawString(large_font, "*" + /*+ t.name + " is feeling the effects of " + i.name*/" fuck, i can't believe i've done this", new Vector2(205, 361), Color.Black);
            return t;
        }
    }
}
