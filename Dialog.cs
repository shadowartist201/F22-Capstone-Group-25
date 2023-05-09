using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Game_Demo
{
    public class Dialog //master class
    {
        DialogBox dialogBox;

        public static string concatInventory(List<Item> item)
        {
            string inventList = "";

            foreach (Item i in item)
                inventList = inventList + i.name + "        ";

            return inventList;
        }

        public void MakeBox(string text, SpriteFont dialogFont, GraphicsDevice graphicsDevice, OrthographicCamera _camera)
        {
            World.box_ok.Play();
            dialogBox = new DialogBox(graphicsDevice, _camera)
            {
                Text = text,
                DialogFont = dialogFont
            };
            dialogBox.Initialize();
        }

        public string Update()
        {
            return dialogBox.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            dialogBox.Draw(spriteBatch);
        }
    }

    public class DialogBox //handles dialog box
    {
        public string Text { get; set; } // All text contained in this dialog box
        public bool Active { get; private set; } // Bool that determines active state of this dialog box
        public Vector2 Position { get; set; } // X,Y coordinates of this dialog box
        public Vector2 Size { get; set; } // Width and Height of this dialog box
        public Color FillColor { get; set; } // Color used to fill dialog box background
        public Color BorderColor { get; set; } // Color used for border around dialog box
        public Color DialogColor { get; set; } // Color used for text in dialog box

        public int BorderWidth { get; set; } // Thickness of border

        private readonly Texture2D _fillTexture; // Background fill texture (built from FillColor)
        private readonly Texture2D _borderTexture; // Border fill texture(built from BorderColor)

        private List<string> _pages; // Collection of pages contained in this dialog box

        private const float DialogBoxMargin = 24f; // Margin surrounding the text inside the dialog box

        public SpriteFont DialogFont; // SpriteFont for dialog box font

        private Vector2 _characterSize; //Size of the widest alphabet letter (W)
         
        private int MaxCharsPerLine => (int)Math.Floor((Size.X - DialogBoxMargin) / _characterSize.X);
        private int MaxLines => (int)Math.Floor((Size.Y - DialogBoxMargin) / _characterSize.Y) - 1;
        private int _currentPage;
        private int _interval; // The stopwatch interval (used for blinking indicator)

        private Rectangle TextRectangle => new Rectangle(Position.ToPoint(), Size.ToPoint());

        // The position and size of the bordering sides on the edges of the dialog box
        private List<Rectangle> BorderRectangles => new List<Rectangle>
        {
            // Top (contains top-left & top-right corners)
            new Rectangle(TextRectangle.X - BorderWidth, TextRectangle.Y - BorderWidth, TextRectangle.Width + BorderWidth*2, BorderWidth),

            // Right
            new Rectangle(TextRectangle.X + TextRectangle.Size.X, TextRectangle.Y, BorderWidth, TextRectangle.Height),

            // Bottom (contains bottom-left & bottom-right corners)
            new Rectangle(TextRectangle.X - BorderWidth, TextRectangle.Y + TextRectangle.Size.Y, TextRectangle.Width + BorderWidth*2, BorderWidth),

            // Left
            new Rectangle(TextRectangle.X - BorderWidth, TextRectangle.Y, BorderWidth, TextRectangle.Height)
        };

        private Vector2 TextPosition => new Vector2(Position.X + DialogBoxMargin / 2, Position.Y + DialogBoxMargin / 2);

        private Stopwatch _stopwatch;

  
        public DialogBox(GraphicsDevice _graphicsDevice, OrthographicCamera _camera)
        {
            BorderWidth = 2;
            DialogColor = Color.Black;

            FillColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);

            BorderColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);

            _fillTexture = new Texture2D(_graphicsDevice, 1, 1);
            _fillTexture.SetData(new[] { FillColor });

            _borderTexture = new Texture2D(_graphicsDevice, 1, 1);
            _borderTexture.SetData(new[] { BorderColor });

            _pages = new List<string>();
            _currentPage = 0;

            var sizeX = (int)(_graphicsDevice.Viewport.Width * 0.5);
            var sizeY = (int)(_graphicsDevice.Viewport.Height * 0.2);

            Size = new Vector2(sizeX, sizeY);

            var posX = _camera.Center.X - (Size.X / 2f);
            var posY = _graphicsDevice.Viewport.Height - Size.Y - 30;

            Position = new Vector2(posX, posY);
        }

        public void Initialize(string text = null)
        {
            Text = text ?? Text;

            _characterSize = DialogFont.MeasureString(new StringBuilder("W", 1));

            _currentPage = 0;

            Show();
        }

        public void Show()
        {
            Active = true;
            _stopwatch = new Stopwatch(); // use stopwatch to manage blinking indicator

            _stopwatch.Start();

            _pages = WordWrap(Text);

            Game1.speech.SpeakAsync(Regex.Replace(_pages[_currentPage], @"\t|\n|\r", ""));
        }

        public void Hide()
        {
            Active = false;

            _stopwatch.Stop();

            _stopwatch = null;

            Game1.speech.SpeakAsyncCancelAll();
        }

        public string Update()
        {
            string status = "null";
            if (Active)
            {
                if (Input.SinglePress() == "enter") // Button press will proceed to the next page of the dialog box
                {
                    World.box_navi.Play();
                    if (_currentPage >= _pages.Count - 1)
                    {
                        Hide();
                        status = "hidden";
                    }
                    else
                    {
                        _currentPage++;
                        _stopwatch.Restart();
                        Game1.speech.SpeakAsync(Regex.Replace(_pages[_currentPage], @"\t|\n|\r", ""));
                    }
                }

                if (Input.SinglePress() == "x") // Shortcut button to skip entire dialog box
                {
                    Hide();
                    status = "hidden";
                    Game1.speech.SpeakAsyncCancelAll();
                }
            }
            return status;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Active)
            {
                foreach (var side in BorderRectangles) // Draw each side of the border rectangle
                {
                    spriteBatch.Draw(_borderTexture, side, BorderColor);
                }

                spriteBatch.Draw(_fillTexture, TextRectangle, FillColor); // Draw background fill texture
                spriteBatch.DrawString(DialogFont, _pages[_currentPage], TextPosition, DialogColor); // Draw the current page onto the dialog box

                if (BlinkIndicator() || _currentPage == _pages.Count - 1) // Draw a blinking indicator
                {
                    var indicatorPosition = new Vector2(TextRectangle.X + TextRectangle.Width - (_characterSize.X) - 4, TextRectangle.Y + TextRectangle.Height - (_characterSize.Y));
                    spriteBatch.DrawString(DialogFont, ">", indicatorPosition, Color.Red);
                }
            }
        }

        private bool BlinkIndicator()
        {
            _interval = (int)Math.Floor((double)(_stopwatch.ElapsedMilliseconds % 1000));
            return _interval < 500;
        }

        private List<string> WordWrap(string text)
        {
            var pages = new List<string>();
            var capacity = MaxCharsPerLine * MaxLines > text.Length ? text.Length : MaxCharsPerLine * MaxLines;
            var result = new StringBuilder(capacity);
            var resultLines = 0;
            var currentWord = new StringBuilder();
            var currentLine = new StringBuilder();

            for (var i = 0; i < text.Length; i++)
            {
                var currentChar = text[i];
                var isNewLine = text[i] == '\n';
                var isLastChar = i == text.Length - 1;

                currentWord.Append(currentChar);

                if (char.IsWhiteSpace(currentChar) || isLastChar)
                {
                    var potentialLength = currentLine.Length + currentWord.Length;

                    if (potentialLength > MaxCharsPerLine)
                    {
                        result.AppendLine(currentLine.ToString());
                        currentLine.Clear();
                        resultLines++;
                    }

                    currentLine.Append(currentWord);
                    currentWord.Clear();

                    if (isLastChar || isNewLine)
                        result.AppendLine(currentLine.ToString());

                    if (resultLines > MaxLines || isLastChar || isNewLine)
                    {
                        pages.Add(result.ToString());
                        result.Clear();
                        resultLines = 0;

                        if (isNewLine)
                            currentLine.Clear();
                    }
                }
            }
            return pages;
        }
    }

    public class DialogText //handles dialog text for the box
    {
        public static string Demo = "Hello! Welcome to our game!\n"+
                                    "We hope you enjoy it!\n";

        public static string Village1_NPC1 = "Hey! You! You shouldn't go any further unless you know how to fight!\n" + 
                                              "I can teach you if you want. It's pretty simple.\n"+
                                              "You just need to decide whether you want to attack with your weapon, with your magic, or use an item.\n"+
                                              "If you don't have a weapon now, that's ok. Just attack with your fists!\n";

        public static string Village1_NPC2 = "Get out of my face, loser. You're Nobody.\n";

        public static string City_NPC1 =     "Welcome to the city! Here you can buy items with the gold you've collected.\n"+
                                             "You can also talk with everyone here!\n"+
                                             "Make sure you talk with our leader. He said he wanted to talk to anyone suspicious that comes in.\n"+
                                             "He's in the big castle to the north of here.\n";

        public static string City_NPC2 = "Hey! What are you doing in this city?\n" +
                                         "I heard a story of a traveller with great powers that would come here one day.\n"+
                                         "You need to talk to our leader. He'll be in the castle in the North part of town.\n"+
                                         "You'll need to go back to the South, then West, and then back up North.\n";

        public static string Castle_NPC1 = "Hello traveller. I had heard of your arrival to our city.\n" +
                                           "It's not often that we get travellers here.\n"+
                                           "If you are who I think you are, we're all in grave danger.\n"+
                                           "There exists a prophecy that foretells the end of the world!\n"+
                                           "I believe that you are the one who will bring it.\n"+
                                           "You must fight with me to prove your strength if you want to change my mind about you!\n";

        public static string CityBar_NPC1 = "Welcome to the city bar!\n" +
                                            "Unfortunately we don't have any refreshments to offer at this time.\n"+
                                            "Come back soon!\n";

        public static string EquipShop_NPC1 = "Welcome to the city's equipment shop!\n" +
                                              "Unfortunately we don't have any items for sale right now.\n"+
                                              "Come back soon!\n";

        public static string Potion_NPC1 = "Welcome to the city's potion shop!\n" +
                                           "Unfortunately we don't have any potions for sale right now.\n"+
                                           "Come back soon!\n";

        public static string Village1_NPC3 = Dialog.concatInventory(Game1.inventory);
        public static string Home_NPC4 = Dialog.concatInventory(Game1.inventory);
    }
}
