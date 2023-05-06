using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Threading.Tasks.Sources;

namespace Game_Demo
{
    internal class BattleUI : Game
    {
        public static Texture2D battle_message;       //center message box
        public static Texture2D inventory_box;        //inventory menu
        public static Texture2D menu_box;             //action menu
        public static Texture2D party_info;           //holds names and HP
        public static Texture2D enemy_box;            //holds enemy name

        public static Texture2D menu_up;              //action menu up arrow
        public static Texture2D menu_down;            //action menu down arrow
        public static Texture2D item_selection;       //red selection box
        public static Texture2D current_fighter;      //blue triangle to show current turn

        public static Texture2D hp_bar;               //empty bar
        public static Texture2D bar_fill;             //that which fills the bar

        public static int selection_index = 1;        //shows which action menu option is selected
        public static int inventory_index = 0;
        public static int current_character = 0;      //which character's turn is it, where 1 = Nobody and 2 = Cat
        public static bool squadTurn = true;

        public static float menu_alpha = 0.0f;        //action menu visibility, 0 = hidden and 1 = show
        public static float inventory_alpha = 0.0f;   //inventory menu visibility
        public static float message_alpha = 1.0f;     //message box visibility, initialized to 1 for initial message

        public static bool initial_message = true;    //initial message "A <thing> appeared!"
        public static bool attack_message = false;    //message "<chara> attacked!"
        public static bool magic_message = false;     //message "<chara> used magic!"
        public static bool cat_magic_message = false; //message "Cats can't use magic"
        public static bool flee_message = false;      //message when attempting to flee battle
        public static bool item_message = false;

        static int start = 0;                         //start index of inventory menu display
        static int end = 0;                           //end index of inventory menu display
        public static string output = "";             //the current key pressed

        ///TO DO: maybe a small hp/mana bar for each enemy on the field?
        public static void ResetScreen()
        {
            current_character = 0;      //which character's turn is it, where 1 = Nobody and 2 = Cat

            menu_alpha = 0.0f;        //action menu visibility, 0 = hidden and 1 = show
            inventory_alpha = 0.0f;   //inventory menu visibility
            message_alpha = 1.0f;     //message box visibility, initialized to 1 for initial message

            initial_message = true;    //initial message "A <thing> appeared!"
            attack_message = false;    //message "<chara> attacked!"
            magic_message = false;     //message "<chara> used magic!"
            cat_magic_message = false; //message "Cats can't use magic"
            flee_message = false;      //message when attempting to flee battle
            item_message = false;
            Battle.selection = false;
        }

        public static void LoadUI(ContentManager Content) //load assets
        {
            hp_bar = Content.Load<Texture2D>("Battle/hp-bar");
            bar_fill = Content.Load<Texture2D>("Battle/bar-fill");

            battle_message = Content.Load<Texture2D>("Battle/battle-message-box");
            inventory_box = Content.Load<Texture2D>("Battle/inventory-box");
            menu_box = Content.Load<Texture2D>("Battle/menu-box");
            party_info = Content.Load<Texture2D>("Battle/party-info-box");
            enemy_box = Content.Load<Texture2D>("Battle/enemy-name-box");

            menu_up = Content.Load<Texture2D>("Battle/menu-up");
            menu_down = Content.Load<Texture2D>("Battle/menu-down");
            item_selection = Content.Load<Texture2D>("Battle/item-selection");
            current_fighter = Content.Load<Texture2D>("Battle/current-fighter");
        }

        public static void Update_()
        {
            if (flee_message)  //if flee message activated
            {
                if (Input.SinglePress() == "enter")  //check for enter
                {
                    World.box_ok.Play();
                    flee_message = false;  //disable flee message
                    message_alpha = 0;    //hide message box
                }
            }
            else if (attack_message)  //if attack message activated
            {
                if (Input.SinglePress() == "enter")  //check for enter
                {
                    World.box_ok.Play();
                    attack_message = false;   //disable message
                    message_alpha = 0;        //hide box
                    advanceTurn();
                }
            }
            else if (magic_message)  //if magic message activated
            {
                if (Input.SinglePress() == "enter")  //check for enter
                {
                    World.box_ok.Play();
                    magic_message = false;  //disable message
                    message_alpha = 0;      //hide box
                    if (Game1.squad[current_character].mana > 0) //if have mana, edgecase:on emptying mana bar, take second turn
                    {
                        advanceTurn();
                    }
                }
            }
            else if (cat_magic_message)  //if cat magic message activated
            {
                if (Input.SinglePress() == "enter")  //check for enter
                {
                    World.box_ok.Play();
                    cat_magic_message = false;  //disable message
                    message_alpha = 0;   //hide box
                    advanceTurn();
                }
            }
            else if(item_message)
            {
                if(Input.SinglePress() == "enter")
                {
                    item_message = false;
                    message_alpha = 0;
                    advanceTurn();
                }
            }
            else if (message_alpha == 0)  //if no messages activated
            {
                if (squadTurn)
                {
                    menu_alpha = 1; //enable action menu
                    output = Input.SinglePress();
                    if (output == "backspace")  //check for backspace
                    {
                        if (Battle.selection)
                            Battle.selection = false;
                        if (inventory_alpha == 1)
                        {  //if inventory menu activated, hide it
                            World.box_ok.Play();
                            if (inventory_alpha == 1)  //if inventory menu activated, hide it
                                inventory_alpha = 0;
                            selection_index = 3;
                        }
                    }
                    else if (output == "up")  //check for up
                    {
                        //INSERT
                        if (Battle.selection)
                        {
                            if (Battle.target > 0)
                                Battle.target--;
                            selection_index++;
                        }
                        else if (inventory_alpha == 1)
                            if (inventory_index <= Game1.inventory.Count - 1 && inventory_index != 0)
                                inventory_index--;
                        //INSERT
                        if (selection_index != 1)
                        {//move selection box up (and stop at 1 so we don't go out of bounds)
                            selection_index--;
                            World.box_navi.Play();
                        }
                    }
                    else if (output == "down")  //check for down
                    {
                        //INSERT
                        if (Battle.selection)
                        {
                            if (Battle.target < Game1.enemies.Count - 1)
                                Battle.target++;
                            selection_index--;
                        }
                        else if (inventory_alpha == 1)
                            if (inventory_index < Game1.inventory.Count - 1)
                                inventory_index++;
                        //INSERT
                        if (selection_index != 4 && inventory_alpha != 1)
                        {//move selection box down (and stop at 4 so we don't go out of bounds)
                            selection_index++;
                            World.box_navi.Play();
                        }
                        else if (inventory_index <= Game1.inventory.Count - 1 && inventory_alpha == 1 && selection_index != Game1.inventory.Count && Game1.inventory.Count < 5)
                        {
                            selection_index++;
                            World.box_navi.Play();
                            //selection = 2, inventory = 1, 1 <= 1 
                        }
                        else if (selection_index != 4 && inventory_alpha == 1 && Game1.inventory.Count > 4)
                        {
                            selection_index++;
                            World.box_navi.Play();
                            //selection = 2, inventory = 1, 1 <= 1 
                        }
                    }
                    else if (output == "enter")  //check for enter
                    {
                        World.box_ok.Play();
                        if (inventory_alpha == 1) //if inventory showing
                        {
                            menu_alpha = 0f;
                            inventory_alpha = 0f;
                            item_message = true;
                            message_alpha = 1;
                        }
                        else if (Battle.selection)
                        {
                            Battle.selection = false;
                            if (selection_index == 1)
                                attack_message = true;  //enable attack message
                            if (selection_index == 2)
                                magic_message = true;
                            menu_alpha = 0;    //hide action menu
                            message_alpha = 1;   //show message box
                            Battle.alratk = false;
                        }
                        else
                        {
                            if (selection_index == 1) //attack
                            {
                                Battle.selection = true;
                                Battle.target = 0;
                            }
                            else if (selection_index == 2) //magic
                            {
                                if (current_character == 0 || current_character == 2)
                                {
                                    Battle.selection = true;
                                    Battle.target = 0;
                                }
                                else
                                {
                                    cat_magic_message = true;
                                    menu_alpha = 0;
                                    message_alpha = 1;
                                }
                            }
                            else if (selection_index == 3) //item
                            {
                                inventory_alpha = 1;  //show inventory menu
                                selection_index = 1;
                                start = 0;
                                end = 3;
                                inventory_index = 0;
                            }
                            else if (selection_index == 4) //flee
                            {
                                flee_message = true;  //enable flee message
                                menu_alpha = 0;  //hide action menu
                                message_alpha = 1;  //show message box
                            }
                        }
                    }
                }
                else
                {
                    attack_message = true;
                    menu_alpha = 0;
                    message_alpha = 1;
                    Battle.alratk = false;
                }
            }
        }

        public static void BattleEnd(GraphicsDevice graphicsDevice, SpriteBatch _spriteBatch)
        {
            graphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            _spriteBatch.DrawString(Game1.large_font, "Congrat, you is winner", new Vector2(300, 226), Color.White);
            _spriteBatch.DrawString(Game1.medium_font, "Press Enter to continue", new Vector2(300, 250), Color.White);
            _spriteBatch.End();
        }

        public static void BattleEndBad(GraphicsDevice graphicsDevice, SpriteBatch _spriteBatch)
        {
            graphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            _spriteBatch.DrawString(Game1.large_font, "You were defeated in battle", new Vector2(300, 226), Color.White);
            _spriteBatch.DrawString(Game1.medium_font, "Press Enter to continue", new Vector2(300, 250), Color.White);
            _spriteBatch.End();
        }

        public static void DrawBoxes(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(enemy_box, new Rectangle(14, 336, 160, 128), Color.White); //enemy info box
            _spriteBatch.Draw(party_info, new Rectangle(488, 336, 301, 128), Color.White); //party info box

            if (menu_alpha == 1f) //action menu
            {
                _spriteBatch.Draw(menu_box, new Rectangle(119, 308, 150, 155), Color.White); //options menu
                _spriteBatch.Draw(current_fighter, new Rectangle(495, 360 + (19 * current_character), 12, 14), Color.White); //point to current fighter
                if (inventory_alpha == 0f && !Battle.selection) //while inventory menu hidden, enable red selection box
                    _spriteBatch.Draw(item_selection, new Rectangle(130, 304 + (27 * selection_index), 105, 29), Color.White);
            }
            if (inventory_alpha == 1f) //inventory menu
            {
                _spriteBatch.Draw(inventory_box, new Rectangle(213, 308, 273, 155), Color.White); //box
                if (inventory_index != 0 && selection_index > 0)
                    _spriteBatch.Draw(menu_up, new Rectangle(254, 313, 13, 11), Color.White); //up arrow
                if (inventory_index != Game1.inventory.Count - 1 && selection_index < 5)
                _spriteBatch.Draw(menu_down, new Rectangle(254, 443, 13, 11), Color.White); //down arrow
                _spriteBatch.Draw(item_selection, new Rectangle(225, 304 + (27 * selection_index), 105, 29), Color.White);  //red selection box
                    
            }

            if (message_alpha == 1f) //message box
            {
                if (initial_message) //when initial message activated, draw it
                {
                    _spriteBatch.Draw(battle_message, new Rectangle(182, 336, 299, 128), Color.White);
                    if (Input.SinglePress() == "enter")  //if enter pressed, hide message
                    {
                        initial_message = false;
                        message_alpha = 0f;
                    }
                }
                if (flee_message) //when flee message activated, draw it
                {
                    menu_alpha = 0f;
                    _spriteBatch.Draw(battle_message, new Rectangle(182, 336, 299, 128), Color.White);
                }
                if (attack_message) //when attack message activated, draw it
                {
                    _spriteBatch.Draw(battle_message, new Rectangle(182, 336, 299, 128), Color.White);
                }
                if (magic_message) //when magic message activated, draw it
                {
                    if (Game1.squad[current_character].mana > 0)
                        _spriteBatch.Draw(battle_message, new Rectangle(182, 336, 299, 128), Color.White);
                    else
                        _spriteBatch.Draw(battle_message, new Rectangle(182, 336, 299, 128), Color.White);
                }
                if (cat_magic_message) //when cat message message activated, draw it
                {
                    menu_alpha = 0f;
                    _spriteBatch.Draw(battle_message, new Rectangle(182, 336, 299, 128), Color.White);
                }
                if (item_message)
                {
                    menu_alpha = 0f;
                    switch (Game1.inventory[inventory_index].name)
                    {
                        case "Small potion":
                            Battle.itemType = 1;
                            break;
                        case "Large potion":
                            Battle.itemType = 2;
                            break;
                        case "Mana potion":
                            Battle.itemType = 3;
                            break;
                        case "attack buff":
                            Battle.itemType = 4;
                            break;
                        case "defense buff":
                            Battle.itemType = 5;
                            break;
                        default:
                            Battle.itemType = -1;
                            break;
                    }
                    _spriteBatch.Draw(battle_message, new Rectangle(182, 336, 299, 128), Color.White);
                }
                if(item_message)
                {
                    menu_alpha = 0f;
                    switch (Game1.inventory[inventory_index].name)
                    {
                        case "Small potion":
                            Battle.itemType = 1;
                            break;
                        case "Large potion":
                            Battle.itemType = 2;
                            break;
                        case "Mana potion":
                            Battle.itemType = 3;
                            break;
                        case "attack buff":
                            Battle.itemType = 4;
                            break;
                        case "defense buff":
                            Battle.itemType = 5;
                            break;
                        default:
                            Battle.itemType = -1;
                            break;
                    }
                    _spriteBatch.Draw(battle_message, new Rectangle(182, 336, 299, 128), Color.White);
                }
            }
            if (Battle.selection)
            {
                _spriteBatch.Draw(menu_box, new Rectangle(213, 308, 150, 155), Color.White);
                _spriteBatch.DrawString(Game1.large_font, "Dragon", new Vector2(234, 334), Color.Black);
                _spriteBatch.DrawString(Game1.large_font, "TestEntity", new Vector2(234, 361), Color.Black);
                int i = 0;
                foreach (Entity e in Game1.enemies)
                {
                    //lists enemies to attack
                    //_spriteBatch.DrawString(Game1.medium_font, "*" + e.name, new Vector2(618, 364 + (i * 19)), Color.Black);
                    i++;
                }
                _spriteBatch.Draw(item_selection, new Rectangle(225, 304 + (27 * (Battle.target+1)), 105, 29), Color.White);
            }
            for (int placediff = 0; placediff < Game1.squad.Count; placediff++) //placement for HP and MP bars
            {
                Entity e = Game1.squad[placediff];

                _spriteBatch.Draw(hp_bar, new Rectangle(609, 361 + placediff * 19, 78, 17), Color.White); //empty HP bar
                _spriteBatch.Draw(bar_fill, new Rectangle(612, 364 + placediff * 19, ((int)((float)e.health / (float)e.mHealth * 72)), 11), Color.Green); //HP bar fill
                if (e.mMana != 0) //if MP not 0
                {
                    _spriteBatch.Draw(hp_bar, new Rectangle(703, 361, 78, 17), Color.White); //empty MP bar
                    _spriteBatch.Draw(bar_fill, new Rectangle(706, 364, ((int)((float)e.mana / (float)e.mMana * 72)), 11), Color.MediumBlue); //MP bar fill
                }
                else
                {
                    _spriteBatch.Draw(hp_bar, new Rectangle(703, 380, 78, 17), Color.White); //empty MP bar
                    _spriteBatch.Draw(bar_fill, new Rectangle(706, 383, 72, 11), Color.DarkGray); //"MP bar disabled" fill
                }
            }
        } //draw UI boxes

        public static void DrawText(SpriteBatch _spriteBatch)
        {
            //debug text
            _spriteBatch.DrawString(Game1.medium_font, "Enter - Select", new Vector2(310, 81), Color.White);
            _spriteBatch.DrawString(Game1.medium_font, "Backspace - Back", new Vector2(310, 98), Color.White);
            _spriteBatch.DrawString(Game1.medium_font, "Up/Down/Left/Right", new Vector2(310, 115), Color.White);
            _spriteBatch.DrawString(Game1.medium_font, "X - End Battle", new Vector2(310, 132), Color.White);

            //labels
            _spriteBatch.DrawString(Game1.large_font, "Dragon", new Vector2(41, 361), Color.Black);
            _spriteBatch.DrawString(Game1.medium_font, "Nobody", new Vector2(510, 360), Color.Black);
            _spriteBatch.DrawString(Game1.medium_font, "Cat", new Vector2(510, 379), Color.Black);
            _spriteBatch.DrawString(Game1.small_font, "NAME", new Vector2(512, 345), Color.Black);
            _spriteBatch.DrawString(Game1.small_font, "HP", new Vector2(612, 345), Color.Black);
            _spriteBatch.DrawString(Game1.small_font, "MP", new Vector2(707, 345), Color.Black);

            if (menu_alpha == 1f) //action menu
            {
                _spriteBatch.DrawString(Game1.large_font, "Attack", new Vector2(139, 334), Color.Black);
                _spriteBatch.DrawString(Game1.large_font, "Magic", new Vector2(139, 361), Color.Black);
                _spriteBatch.DrawString(Game1.large_font, "Item", new Vector2(139, 390), Color.Black);///TO DO: actually properly implement
                _spriteBatch.DrawString(Game1.large_font, "Flee", new Vector2(139, 417), Color.Black);
            }
            if (inventory_alpha == 1f) //inventory menu
            {
                //Debug.WriteLine(output);
                //INSERT
                if (Game1.inventory.Count < 5)
                {
                    end = Game1.inventory.Count-1;
                }
                if (inventory_index == end + 1)
                {
                    start++;
                    end++;
                }
                if (inventory_index == start - 1)
                {
                    start--;
                    end--;
                }
                for (int i = start; i <= end; i++)
                {
                    _spriteBatch.DrawString(Game1.large_font, Game1.inventory[i].name, new Vector2(234, 334 + ((i - start) * 27)), Color.Black);
                }
                /*if (inventory_index > 3)
                {
                    start = inventory_index - 3;
                }
                if(inventory_index<4)
                {
                    for (int i = start; i < 4; i++)
                    {
                        _spriteBatch.DrawString(Game1.large_font, Game1.inventory[i].name, new Vector2(234, 334 + ((i - start) * 27)), Color.Black);
                    }
                }
                else
                {
                    for (int i = start; i < inventory_index+1; i++)
                    {
                        _spriteBatch.DrawString(Game1.large_font, Game1.inventory[i].name, new Vector2(234, 334 + ((i - start) * 27)), Color.Black);
                    }
                }*/
                //INSERT

                //REMOVE
                /*_spriteBatch.DrawString(Game1.large_font, "Potion", new Vector2(234, 334), Color.Black);
                _spriteBatch.DrawString(Game1.large_font, "----------", new Vector2(234, 361), Color.Black);
                _spriteBatch.DrawString(Game1.large_font, "----------", new Vector2(234, 390), Color.Black);
                _spriteBatch.DrawString(Game1.large_font, "----------", new Vector2(234, 417), Color.Black);*/
                //REMOVE
                _spriteBatch.DrawString(Game1.small_font, "DETAILS", new Vector2(343, 314), Color.Black);
                //INSERT
                _spriteBatch.DrawString(Game1.medium_font, Game1.inventory[inventory_index].about, new Vector2(342, 335), Color.Black);
                //INSERT

                //REMOVE
                /*if (selection_index == 1) //change info text based on item
                    _spriteBatch.DrawString(Game1.medium_font, "Example text potion", new Vector2(342, 335), Color.Black);
                else if (selection_index == 2)
                    _spriteBatch.DrawString(Game1.medium_font, "Blank 1", new Vector2(342, 335), Color.Black);
                else if (selection_index == 3)
                    _spriteBatch.DrawString(Game1.medium_font, "Blank 2", new Vector2(342, 335), Color.Black);
                else if (selection_index == 4)
                    _spriteBatch.DrawString(Game1.medium_font, "Blank 3", new Vector2(342, 335), Color.Black);*/
                //REMOVE
            }
            if (message_alpha == 1f) //message box
            {
                if (initial_message) //when initial message activated, draw it
                {
                    for (int i = 0; i < Game1.enemies.Count; i++)
                    {
                        _spriteBatch.DrawString(Game1.medium_font, "*A(n) " + Game1.enemies[i].name + " appeared!", new Vector2(205, 361 + (21 * i)), Color.Black);
                    }
                    if (Input.SinglePress() == "enter")  //if enter pressed, hide message
                    {
                        initial_message = false;
                        message_alpha = 0f;
                    }
                }
                if (flee_message) //when flee message activated, draw it
                {
                    menu_alpha = 0f;
                    _spriteBatch.DrawString(Game1.large_font, "*You can't stop now!", new Vector2(205, 361), Color.Black);
                }
                if (attack_message) //when attack message activated, draw it
                {
                    //_spriteBatch.DrawString(Game1.large_font, "*" + Game1.squad[current_character] + " attacked the " + Game1.enemies[Battle.target].name + "!", new Vector2(205, 361), Color.Black);
                }
                if (magic_message) //when magic message activated, draw it
                {
                    //if (Game1.squad[current_character].mana > 0)
                        //_spriteBatch.DrawString(Game1.large_font, "*" + Game1.squad[current_character] + " summoned fire!", new Vector2(205, 361), Color.Black);
                    //else
                        //_spriteBatch.DrawString(Game1.large_font, "*" + Game1.squad[current_character] + " is out of mana!", new Vector2(205, 361), Color.Black);
                }
                if (cat_magic_message) //when cat message message activated, draw it
                {
                    menu_alpha = 0f;
                    _spriteBatch.DrawString(Game1.large_font, "*The cat winds its way around\nyour legs.", new Vector2(205, 361), Color.Black);
                    _spriteBatch.DrawString(Game1.large_font, "\n*Your party has been healed\nfor 15% health!", new Vector2(205, 381), Color.Black);
                }
                if (item_message)
                {
                    menu_alpha = 0f;
                    _spriteBatch.DrawString(Game1.large_font, "Somehow this isnt working", new Vector2(205, 361), Color.Black);
                }

            }
            for (int placediff = 0; placediff / 19 < Game1.squad.Count; placediff += 19) //placement for HP and MP labels
            {
                Entity e = Game1.squad[placediff / 19];

                _spriteBatch.DrawString(Game1.small_font, e.health + " / " + e.mHealth, new Vector2(618, 364 + placediff), Color.Black); //HP labels
                if (e.mMana != 0) //if MP not 0
                    _spriteBatch.DrawString(Game1.small_font, e.mana + " / " + e.mMana, new Vector2(732, 364), Color.Black); //MP labels
                else
                    _spriteBatch.DrawString(Game1.small_font, "XX", new Vector2(758, 383), Color.Black); //MP disabled
            }
            int j = 0;
            foreach (Entity e in Game1.enemies)
            {
                _spriteBatch.DrawString(Game1.medium_font, "Debug HP: " + e.health + " / " + e.mHealth, new Vector2(325, 217 + (j * 19)), Color.Cyan); //debug enemy HP
                j += 1;
            }
        } //draw text for boxes

        public static void advanceTurn()
        {
            current_character += 1;
            inventory_index = 0;
            if (current_character > Game1.squad.Count-1 && squadTurn)
            {
                current_character = 0;
                squadTurn = false;
                Battle.target = 0;
            }
            else if (current_character > Game1.enemies.Count - 1 && !squadTurn)
            {
                current_character = 0;
                squadTurn = true;
                Battle.target = 0;
            }
            if (squadTurn && Battle.atkbuf[current_character] > 0)
                Battle.atkbuf[current_character]--;
            if (Battle.atkbuf[current_character] == 0 && Battle.atkbool[current_character])
            {
                Game1.squad[current_character] = Game1.squad[BattleUI.current_character] = new Entity(Game1.squad[current_character].name, Game1.squad[current_character].health, Game1.squad[current_character].mHealth, Game1.squad[current_character].mana, Game1.squad[current_character].mMana, (int)Math.Ceiling((float)Game1.squad[current_character].attack / (float)1.25), (int)Math.Ceiling((float)Game1.squad[current_character].spattack / (float)1.1), Game1.squad[current_character].def, Game1.squad[current_character].spdef);
                Battle.atkbool[current_character] = false;
            }
            if (squadTurn && Battle.defbuf[current_character] > 0)
                Battle.defbuf[current_character]--;
            if (Battle.defbuf[current_character] == 0 && Battle.defbool[current_character])
            {
                Game1.squad[current_character] = Game1.squad[BattleUI.current_character] = new Entity(Game1.squad[current_character].name, Game1.squad[current_character].health, Game1.squad[current_character].mHealth, Game1.squad[current_character].mana, Game1.squad[current_character].mMana, Game1.squad[current_character].attack, Game1.squad[current_character].spattack, (int)Math.Ceiling((float)Game1.squad[current_character].def / (float)1.25), (int)Math.Ceiling((float)Game1.squad[current_character].spdef / (float)1.1));
                Battle.defbool[current_character] = false;
            }
            if (squadTurn)
            {
                if (Game1.squad[current_character].health < 1)
                {
                    advanceTurn();
                }
            }
            else
            {
                if (Game1.enemies[current_character].health < 1)
                {
                    advanceTurn();
                }
            }
        }
    }
}