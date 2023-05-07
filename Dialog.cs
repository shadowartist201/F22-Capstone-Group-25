using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using DavyKager;
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
            Debug.WriteLine(item.Count + " | " + Game1.inventory.Count);
            string inventList = "";
            foreach (Item i in item)
            {
                inventList += i.name;
                //Debug.WriteLine(inventList);
            }
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
            string status;
            status = dialogBox.Update();
            return status;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            dialogBox.Draw(spriteBatch);
        }
    }
    public class DialogBox //handles dialog box
    {

        /// <summary>
        /// All text contained in this dialog box
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Bool that determines active state of this dialog box
        /// </summary>
        public bool Active { get; private set; }

        /// <summary>
        /// X,Y coordinates of this dialog box
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Width and Height of this dialog box
        /// </summary>
        public Vector2 Size { get; set; }

        /// <summary>
        /// Color used to fill dialog box background
        /// </summary>
        public Color FillColor { get; set; }

        /// <summary>
        /// Color used for border around dialog box
        /// </summary>
        public Color BorderColor { get; set; }

        /// <summary>
        /// Color used for text in dialog box
        /// </summary>
        public Color DialogColor { get; set; }

        /// <summary>
        /// Thickness of border
        /// </summary>
        public int BorderWidth { get; set; }

        /// <summary>
        /// Background fill texture (built from FillColor)
        /// </summary>
        private readonly Texture2D _fillTexture;

        /// <summary>
        /// Border fill texture (built from BorderColor)
        /// </summary>
        private readonly Texture2D _borderTexture;

        /// <summary>
        /// Collection of pages contained in this dialog box
        /// </summary>
        private List<string> _pages;

        /// <summary>
        /// Margin surrounding the text inside the dialog box
        /// </summary>
        private const float DialogBoxMargin = 24f;

        /// <summary>
        /// SpriteFont for dialog box font
        /// </summary>
        public SpriteFont DialogFont;

        /// <summary>
        /// Size (in pixels) of a wide alphabet letter (W is the widest letter in almost every font) 
        /// </summary>
        private Vector2 _characterSize;

        /// <summary>
        /// The amount of characters allowed on a given line
        /// NOTE: If you want to use a font that is not monospaced, this will need to be reevaluated
        /// </summary>
        private int MaxCharsPerLine => (int)Math.Floor((Size.X - DialogBoxMargin) / _characterSize.X);

        /// <summary>
        /// Determine the maximum amount of lines allowed per page
        /// NOTE: This will change automatically with font size
        /// </summary>
        private int MaxLines => (int)Math.Floor((Size.Y - DialogBoxMargin) / _characterSize.Y) - 1;

        /// <summary>
        /// The index of the current page
        /// </summary>
        private int _currentPage;

        /// <summary>
        /// The stopwatch interval (used for blinking indicator)
        /// </summary>
        private int _interval;

        /// <summary>
        /// The position and size of the dialog box fill rectangle
        /// </summary>
        private Rectangle TextRectangle => new Rectangle(Position.ToPoint(), Size.ToPoint());

        /// <summary>
        /// The position and size of the bordering sides on the edges of the dialog box
        /// </summary>
        private List<Rectangle> BorderRectangles => new List<Rectangle>
        {
            // Top (contains top-left & top-right corners)
            new Rectangle(TextRectangle.X - BorderWidth, TextRectangle.Y - BorderWidth,
                TextRectangle.Width + BorderWidth*2, BorderWidth),

            // Right
            new Rectangle(TextRectangle.X + TextRectangle.Size.X, TextRectangle.Y, BorderWidth, TextRectangle.Height),

            // Bottom (contains bottom-left & bottom-right corners)
            new Rectangle(TextRectangle.X - BorderWidth, TextRectangle.Y + TextRectangle.Size.Y,
                TextRectangle.Width + BorderWidth*2, BorderWidth),

            // Left
            new Rectangle(TextRectangle.X - BorderWidth, TextRectangle.Y, BorderWidth, TextRectangle.Height)
        };

        /// <summary>
        /// The starting position of the text inside the dialog box
        /// </summary>
        private Vector2 TextPosition => new Vector2(Position.X + DialogBoxMargin / 2, Position.Y + DialogBoxMargin / 2);

        /// <summary>
        /// Stopwatch used for the blinking (next page) indicator
        /// </summary>
        private Stopwatch _stopwatch;

        /// <summary>
        /// Default constructor
        /// </summary>
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

        /// <summary>
        /// Initialize a dialog box
        /// - can be used to reset the current dialog box in case of "I didn't quite get that..."
        /// </summary>
        /// <param name="text"></param>
        public void Initialize(string text = null)
        {
            Text = text ?? Text;

            _characterSize = DialogFont.MeasureString(new StringBuilder("W", 1));

            _currentPage = 0;

            Show();
        }

        /// <summary>
        /// Show the dialog box on screen
        /// - invoke this method manually if Text changes
        /// </summary>
        public void Show()
        {
            Active = true;

            // use stopwatch to manage blinking indicator
            _stopwatch = new Stopwatch();

            _stopwatch.Start();

            _pages = WordWrap(Text);

            Tolk.Speak(Regex.Replace(_pages[_currentPage], @"\t|\n|\r", ""), true);
            //Debug.WriteLine(Regex.Replace(_pages[_currentPage], @"\t|\n|\r", ""));
        }

        /// <summary>
        /// Manually hide the dialog box
        /// </summary>
        public void Hide()
        {
            Active = false;

            _stopwatch.Stop();

            _stopwatch = null;

            Tolk.Silence();
        }

        /// <summary>
        /// Process input for dialog box
        /// </summary>

        public string Update()
        {
            string status = "null";
            if (Active)
            {
                // Button press will proceed to the next page of the dialog box
                if (Input.SinglePress() == "enter")
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
                        Tolk.Speak(Regex.Replace(_pages[_currentPage], @"\t|\n|\r", ""), true);
                       // Debug.WriteLine(Regex.Replace(_pages[_currentPage], @"\t|\n|\r", ""));
                    }
                }

                // Shortcut button to skip entire dialog box
                if (Input.SinglePress() == "x")
                {
                    Hide();
                    status = "hidden";
                }
            }
            return status;
        }

        /// <summary>
        /// Draw the dialog box on screen if it's currently active
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (Active)
            {
                // Draw each side of the border rectangle
                foreach (var side in BorderRectangles)
                {
                    spriteBatch.Draw(_borderTexture, side, BorderColor);
                }

                // Draw background fill texture (in this example, it's 0% transparent white)
                spriteBatch.Draw(_fillTexture, TextRectangle, FillColor);

                // Draw the current page onto the dialog box
                spriteBatch.DrawString(DialogFont, _pages[_currentPage], TextPosition, DialogColor);

                // Draw a blinking indicator to guide the player through to the next page
                // This stops blinking on the last page
                // NOTE: You probably want to use an image here instead of a string
                if (BlinkIndicator() || _currentPage == _pages.Count - 1)
                {
                    var indicatorPosition = new Vector2(TextRectangle.X + TextRectangle.Width - (_characterSize.X) - 4,
                        TextRectangle.Y + TextRectangle.Height - (_characterSize.Y));

                    spriteBatch.DrawString(DialogFont, ">", indicatorPosition, Color.Red);
                }
            }
        }

        /// <summary>
        /// Whether the indicator should be visible or not
        /// </summary>
        /// <returns></returns>
        private bool BlinkIndicator()
        {
            _interval = (int)Math.Floor((double)(_stopwatch.ElapsedMilliseconds % 1000));

            return _interval < 500;
        }

        /// <summary>
        /// Wrap words to the next line where applicable
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
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
                    {
                        result.AppendLine(currentLine.ToString());
                    }

                    if (resultLines > MaxLines || isLastChar || isNewLine)
                    {
                        pages.Add(result.ToString());

                        result.Clear();

                        resultLines = 0;

                        if (isNewLine)
                        {
                            currentLine.Clear();
                        }
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
