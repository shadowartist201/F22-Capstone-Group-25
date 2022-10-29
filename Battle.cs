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
        private Texture2D cat; //cat texture

        private Texture2D battle_message;
        private Texture2D inventory_box;
        private Texture2D menu_box;
        private Texture2D party_info;
        private Texture2D enemy_box;

        private Texture2D menu_up;
        private Texture2D menu_down;
        private Texture2D item_selection;
        private Texture2D current_fighter;

        private Texture2D healing_effect;

        private SpriteFont small_font;
        private SpriteFont medium_font;
        private SpriteFont large_font;

        private Texture2D hp_bar;
        private Texture2D bar_fill;

        private int player_hp_curr = 100;
        private int cat_hp_curr = 50;
        private int player_mp_curr = 10;

        private int dragon_hp_curr = 200;

        private int selection_index = 1;
        private int current_character = 1;
        private string[] characters = { null, "Nobody", "Cat" };

        private float menu_alpha = 0.0f;
        private float inventory_alpha = 0.0f;
        private float message_alpha = 1.0f;

        private bool initial_message = true;
        private bool attack_message = false;
        private bool magic_message = false;
        private bool cat_magic_message = false;
        private bool flee_message = false;

        private KeyboardState oldState;

        public Battle()
        {
            _graphics = new GraphicsDeviceManager(this);
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

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState newState = Keyboard.GetState();


            if (flee_message)
            {
                if (newState.IsKeyDown(Keys.Enter) && !oldState.IsKeyDown(Keys.Enter))
                {
                    flee_message = false;
                    message_alpha = 0;
                    if (current_character == 1)
                        current_character = 2;
                    else if (current_character == 2)
                        current_character = 1;
                }
            }
            else if (attack_message)
            {
                if (newState.IsKeyDown(Keys.Enter) && !oldState.IsKeyDown(Keys.Enter))
                {
                    attack_message = false;
                    message_alpha = 0;
                    if (dragon_hp_curr != 0)
                        dragon_hp_curr = dragon_hp_curr - 10;
                    if (current_character == 1)
                        current_character = 2;
                    else if (current_character == 2)
                        current_character = 1;
                }
            }
            else if (magic_message)
            {
                if (newState.IsKeyDown(Keys.Enter) && !oldState.IsKeyDown(Keys.Enter))
                {
                    magic_message = false;
                    message_alpha = 0;
                    if (player_mp_curr != 0)
                        player_mp_curr--;
                    if (dragon_hp_curr != 0)
                        dragon_hp_curr = dragon_hp_curr - 20;
                    if (current_character == 1)
                        current_character = 2;
                    else if (current_character == 2)
                        current_character = 1;
                }
            }
            else if (cat_magic_message)
            {
                if (newState.IsKeyDown(Keys.Enter) && !oldState.IsKeyDown(Keys.Enter))
                {
                    cat_magic_message = false;
                    message_alpha = 0;
                }
            }


            else if (message_alpha == 0)
            {
                if (current_character == 1)
                {
                    menu_alpha = 1; //draw menu
                    if (oldState.IsKeyUp(Keys.Back) && newState.IsKeyDown(Keys.Back))
                    {
                        if (inventory_alpha == 1)
                            inventory_alpha = 0;
                    }
                    if (oldState.IsKeyUp(Keys.Up) && newState.IsKeyDown(Keys.Up))
                    {
                        if (selection_index != 1)
                            selection_index--;
                    }
                    if (oldState.IsKeyUp(Keys.Down) && newState.IsKeyDown(Keys.Down))
                    {
                        if (selection_index != 4)
                            selection_index++;
                    }
                    if (oldState.IsKeyUp(Keys.Enter) && newState.IsKeyDown(Keys.Enter))
                    {
                        if (selection_index == 1) //attack
                        {
                            attack_message = true;
                            menu_alpha = 0;
                            message_alpha = 1;
                        }
                        else if (selection_index == 2) //magic
                        {
                            magic_message = true;
                            menu_alpha = 0;
                            message_alpha = 1;
                        }
                        else if (selection_index == 3) //item
                        {
                            inventory_alpha = 1;  //open inventory
                        }
                        else if (selection_index == 4) //flee
                        {
                            flee_message = true;
                            menu_alpha = 0;
                            message_alpha = 1;
                        }
                    }
                }
                else if (current_character == 2)
                {
                    menu_alpha = 1; //draw menu
                    if (oldState.IsKeyUp(Keys.Back) && newState.IsKeyDown(Keys.Back))
                    {
                        if (inventory_alpha == 1)
                            inventory_alpha = 0;
                    }
                    if (oldState.IsKeyUp(Keys.Up) && newState.IsKeyDown(Keys.Up))
                    {
                        if (selection_index != 1)
                            selection_index--;
                    }
                    if (oldState.IsKeyUp(Keys.Down) && newState.IsKeyDown(Keys.Down))
                    {
                        if (selection_index != 4)
                            selection_index++;
                    }
                    if (oldState.IsKeyUp(Keys.Enter) && newState.IsKeyDown(Keys.Enter))
                    {
                        if (selection_index == 1) //attack
                        {
                            attack_message = true;
                            menu_alpha = 0;
                            message_alpha = 1;
                        }
                        else if (selection_index == 2) //magic
                        {
                            cat_magic_message = true;
                            menu_alpha = 0;
                            message_alpha = 1;
                        }
                        else if (selection_index == 3) //item
                        {
                            inventory_alpha = 1;  //open inventory
                        }
                        else if (selection_index == 4) //flee
                        {
                            flee_message = true;
                            menu_alpha = 0;
                            message_alpha = 1;
                        }
                    }
                }
            }

            oldState = newState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (dragon_hp_curr == 0)
            {
                GraphicsDevice.Clear(Color.Black);
                _spriteBatch.Begin();
                _spriteBatch.DrawString(large_font, "Congrat, you is winner", new Vector2(300, 226), Color.White);
                _spriteBatch.End();
            }
            else
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

                //enemy box
                _spriteBatch.DrawString(large_font, "Dragon", new Vector2(41, 361), Color.Black);

                //party box
                _spriteBatch.DrawString(large_font, "Nobody", new Vector2(510, 375), Color.Black);
                _spriteBatch.DrawString(large_font, "Cat", new Vector2(510, 418), Color.Black);
                _spriteBatch.DrawString(small_font, "NAME", new Vector2(512, 345), Color.Black);
                _spriteBatch.DrawString(small_font, "HP", new Vector2(612, 345), Color.Black);
                _spriteBatch.DrawString(small_font, "MP", new Vector2(707, 345), Color.Black);

                if (menu_alpha == 1f) //action menu
                {
                    _spriteBatch.Draw(menu_box, new Rectangle(119, 308, 150, 155), Color.White);
                    _spriteBatch.DrawString(large_font, "Attack", new Vector2(139, 334), Color.Black);
                    _spriteBatch.DrawString(large_font, "Magic", new Vector2(139, 361), Color.Black);
                    _spriteBatch.DrawString(large_font, "Item", new Vector2(139, 390), Color.Black);
                    _spriteBatch.DrawString(large_font, "Flee", new Vector2(139, 417), Color.Black);
                    if (current_character == 1)
                    {
                        _spriteBatch.Draw(current_fighter, new Rectangle(495, 377, 12, 14), Color.White);
                    }
                    else if (current_character == 2)
                    {
                        _spriteBatch.Draw(current_fighter, new Rectangle(495, 421, 12, 14), Color.White);
                    }
                    if (inventory_alpha == 0f)
                        _spriteBatch.Draw(item_selection, new Rectangle(130, 304 + (27 * selection_index), 105, 29), Color.White);
                }

                if (inventory_alpha == 1f) //inventory menu
                {
                    _spriteBatch.Draw(inventory_box, new Rectangle(213, 308, 273, 155), Color.White);
                    _spriteBatch.Draw(menu_up, new Rectangle(254, 313, 13, 11), Color.White);
                    _spriteBatch.Draw(menu_down, new Rectangle(254, 443, 13, 11), Color.White);
                    _spriteBatch.DrawString(large_font, "Potion", new Vector2(234, 334), Color.Black);
                    _spriteBatch.DrawString(large_font, "----------", new Vector2(234, 361), Color.Black);
                    _spriteBatch.DrawString(large_font, "----------", new Vector2(234, 390), Color.Black);
                    _spriteBatch.DrawString(large_font, "----------", new Vector2(234, 417), Color.Black);
                    _spriteBatch.DrawString(small_font, "DETAILS", new Vector2(343, 314), Color.Black);
                    if (selection_index == 1)
                        _spriteBatch.DrawString(medium_font, "Example text potion", new Vector2(342, 335), Color.Black);
                    else if (selection_index == 2)
                        _spriteBatch.DrawString(medium_font, "Blank 1", new Vector2(342, 335), Color.Black);
                    else if (selection_index == 3)
                        _spriteBatch.DrawString(medium_font, "Blank 2", new Vector2(342, 335), Color.Black);
                    else if (selection_index == 4)
                        _spriteBatch.DrawString(medium_font, "Blank 3", new Vector2(342, 335), Color.Black);
                    _spriteBatch.Draw(item_selection, new Rectangle(225, 304 + (27 * selection_index), 105, 29), Color.White);
                }

                if (message_alpha == 1f) //message box
                {
                    if (initial_message)
                    {
                        _spriteBatch.Draw(battle_message, new Rectangle(182, 336, 299, 128), Color.White);
                        _spriteBatch.DrawString(large_font, "*A dragon appeared!", new Vector2(205, 361), Color.Black);
                        if (oldState.IsKeyDown(Keys.Enter))
                        {
                            initial_message = false;
                            message_alpha = 0f;
                        }
                    }
                    if (flee_message)
                    {
                        menu_alpha = 0f;
                        _spriteBatch.Draw(battle_message, new Rectangle(182, 336, 299, 128), Color.White);
                        _spriteBatch.DrawString(large_font, "*You can't stop now!", new Vector2(205, 361), Color.Black);
                    }
                    if (attack_message)
                    {
                        menu_alpha = 0f;
                        _spriteBatch.Draw(battle_message, new Rectangle(182, 336, 299, 128), Color.White);
                        _spriteBatch.DrawString(large_font, "*" + characters[current_character] + " attacked the dragon!", new Vector2(205, 361), Color.Black);
                    }
                    if (magic_message)
                    {
                        menu_alpha = 0f;
                        _spriteBatch.Draw(battle_message, new Rectangle(182, 336, 299, 128), Color.White);
                        _spriteBatch.DrawString(large_font, "*" + characters[current_character] + " summoned fire!", new Vector2(205, 361), Color.Black);
                    }
                    if (cat_magic_message)
                    {
                        menu_alpha = 0f;
                        _spriteBatch.Draw(battle_message, new Rectangle(182, 336, 299, 128), Color.White);
                        _spriteBatch.DrawString(large_font, "*Cats can't do magic, silly!", new Vector2(205, 361), Color.Black);
                    }
                }

                //player hp
                _spriteBatch.DrawString(medium_font, player_hp_curr + " / 100", new Vector2(618, 372), Color.Black);
                _spriteBatch.Draw(hp_bar, new Rectangle(609, 390, 78, 11), Color.White); //player's HP
                _spriteBatch.Draw(bar_fill, new Rectangle(612, 393, ((int)(player_hp_curr / 100.0 * 72)), 5), Color.Green);

                //player mp
                _spriteBatch.DrawString(medium_font, player_mp_curr + " / 10", new Vector2(732, 372), Color.Black);
                _spriteBatch.Draw(hp_bar, new Rectangle(703, 390, 78, 11), Color.White); //player's MP
                _spriteBatch.Draw(bar_fill, new Rectangle(706, 393, ((int)(player_mp_curr / 10.0 * 72)), 5), Color.MediumBlue);

                //cat hp
                _spriteBatch.DrawString(medium_font, cat_hp_curr + " / 50", new Vector2(625, 417), Color.Black);
                _spriteBatch.Draw(hp_bar, new Rectangle(609, 435, 78, 11), Color.White); //cat's HP
                _spriteBatch.Draw(bar_fill, new Rectangle(612, 438, ((int)(cat_hp_curr / 50.0 * 72)), 5), Color.Green);

                //cat mp
                _spriteBatch.DrawString(medium_font, "XX", new Vector2(758, 417), Color.Black);
                _spriteBatch.Draw(hp_bar, new Rectangle(703, 435, 78, 11), Color.White); //cat's MP, 72px space within
                _spriteBatch.Draw(bar_fill, new Rectangle(706, 438, 72, 5), Color.DarkGray);

                //dragon hp
                _spriteBatch.DrawString(medium_font, "Debug HP: " + dragon_hp_curr + " / 200", new Vector2(325, 217), Color.Cyan);

                _spriteBatch.End();
            }

            base.Draw(gameTime);
        }

    }
}
