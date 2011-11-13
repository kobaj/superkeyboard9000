using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Keyboard;

namespace TestGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public enum TestPhase { Setup, Options, InKeyboard }
        public enum SetupPhase { ChooseTester, ChooseTestee, SetupComplete }
        public enum KeyboardType { Default, Super, None }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D UIForeground;
        Texture2D Background;
        Texture2D Cover;
        KeyboardType keyboardType;
        
        PlayerIndex tester;
        bool testerChosen;

        PlayerIndex testee;
        bool testeeChosen;

        TestPhase currentPhase;
        SetupPhase currentSetupPhase;
        SpriteFont testingFont;
        SpriteFont smallTestingFont;

        int testTextIndex;
        List<String> testTexts;

        int colorSchemeIndex;
        List<KeyboardManager.ColorScheme> colorSchemes;
        bool passwordMode;
        double stopWatch;

        public static Vector2 DefaultLogPosition = new Vector2(490, 280);
        List<String> OutputLog;
        Vector2 LogPosition;
        Vector2 LogVelocity;

        IAsyncResult DefaultKeyboardResult;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            Components.Add(new GamerServicesComponent(this));
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            keyboardType = KeyboardType.None;
            currentPhase = TestPhase.Setup;
            currentSetupPhase = SetupPhase.ChooseTester;
            passwordMode = false;
            OutputLog = new List<string>();
            stopWatch = 0;
            LogPosition = DefaultLogPosition;
            LogVelocity = new Vector2(0, 0);
            InitializeTestText();
            InitializeColorSchemes();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            UIForeground = Content.Load<Texture2D>("TestUI");
            Background = Content.Load<Texture2D>("Background");
            Cover = Content.Load<Texture2D>("Cover");
            testingFont = Content.Load<SpriteFont>("Keyboard/Fonts/Arial");
            smallTestingFont = Content.Load<SpriteFont>("ArialSmall");
            KeyboardManager.Initialize(Content);

         

        }

        private void InitializeTestText()
        {
            testTextIndex = 0;
            testTexts = new List<string>();
            testTexts.Add("The quick brown fox jumps over a lazy dog.");
            testTexts.Add("password123");
            testTexts.Add("PGAyYvZ4");
            testTexts.Add("kwd7p6Vf");
            testTexts.Add("KyV396Ff");
            testTexts.Add("ZJjAMCua");
            testTexts.Add("e5pYrpHx");
            testTexts.Add("Nymphs blitz quick vex dwarf jog.");
            testTexts.Add("DJs flock by when MTV ax quiz prog.");
            testTexts.Add("Big fjords vex quick waltz nymph.");
            testTexts.Add("Bawds jog, flick quartz, vex nymph.");
            testTexts.Add("Junk MTV quiz graced by fox whelps.");
            testTexts.Add("Bawds jog, flick quartz, vex nymphs.");
            testTexts.Add("Waltz, bad nymph, for quick jigs vex!");
            testTexts.Add("Fox nymphs grab quick-jived waltz");
            testTexts.Add("Brick quiz whangs jumpy veldt fox.");
            testTexts.Add("Glib jocks quiz nymph to vex dwarf.");
            testTexts.Add("Bright vixens jump; dozy fowl quack.");
            testTexts.Add("Vexed nymphs go for quick waltz job.");
            testTexts.Add("Quick wafting zephyrs vex bold Jim.");
            testTexts.Add("Quick zephyrs blow, vexing daft Jim.");
            testTexts.Add("Quick blowing zephyrs vex daft Jim.");
            testTexts.Add("Sphinx of black quartz, judge my vow.");
            testTexts.Add("Sex-charged fop blew my junk TV quiz");
            testTexts.Add("Jack fox bids ivy-strewn phlegm quiz");
            testTexts.Add("How quickly daft jumping zebras vex.");
            testTexts.Add("Two driven jocks help fax my big quiz.");
            testTexts.Add("\"Now fax quiz Jack!\" my brave ghost pled.");
            testTexts.Add("Jack, love my big wad of sphinx quartz!");
            testTexts.Add("Vamp fox held quartz duck just by wing.");
            testTexts.Add("Five quacking zephyrs jolt my wax bed.");
            testTexts.Add("The five boxing wizards jump quickly.");
            testTexts.Add("Jackdaws love my big sphinx of quartz.");
            testTexts.Add("Kvetching, flummoxed by job, W. zaps Iraq.");
            testTexts.Add("My ex pub quiz crowd gave joyful thanks.");
            testTexts.Add("Cozy sphinx waves quart jug of bad milk.");
            testTexts.Add("A very bad quack might jinx zippy fowls.");
            testTexts.Add("Pack my box with five dozen liquor jugs.");
            testTexts.Add("Few quips galvanized the mock jury box.");
            testTexts.Add("The jay, pig, fox, zebra, and my wolves quack!");
            testTexts.Add("Blowzy red vixens fight for a quick jump.");
            testTexts.Add("Sex prof gives back no quiz with mild joy.");
            testTexts.Add("Joaquin Phoenix was gazed by MTV for luck.");
            testTexts.Add("JCVD might pique a sleazy boxer with funk.");
            testTexts.Add("Quizzical twins proved my hijack-bug fix.");
            testTexts.Add("Waxy and quivering, jocks fumble the pizza.");
            testTexts.Add("When zombies arrive, quickly fax judge Pat.");
            testTexts.Add("Heavy boxes perform quick waltzes and jigs.");
            testTexts.Add("A quick chop jolted my big sexy frozen wives.");
            testTexts.Add("A wizard's job is to vex chumps quickly in fog.");
            testTexts.Add("Sympathizing would fix Quaker objectives.");
            testTexts.Add("Pack my red box with five dozen quality jugs.");
            testTexts.Add("Fake bugs put in wax jonquils drive him crazy.");
            testTexts.Add("Watch \"Jeopardy!\", Alex Trebek's fun TV quiz game.");
            testTexts.Add("The big plump jowls of zany Dick Nixon quiver.");
            testTexts.Add("GQ jock wears vinyl tuxedo for showbiz promo.");
            testTexts.Add("The quick brown fox jumped over the lazy dogs.");
            testTexts.Add("Woven silk pyjamas exchanged for blue quartz.");
            testTexts.Add("Brawny gods just flocked up to quiz and vex him.");
            testTexts.Add("Twelve ziggurats quickly jumped a finch box.");
            testTexts.Add("My faxed joke won a pager in the cable TV quiz show.");
            testTexts.Add("The quick onyx goblin jumps over the lazy dwarf.");
            testTexts.Add("The lazy major was fixing Cupid's broken quiver.");
            testTexts.Add("Amazingly few discotheques provide jukeboxes.");
            testTexts.Add("Foxy diva Jennifer Lopez wasn't baking my quiche.");
            testTexts.Add("Cozy lummox gives smart squid who asks for job pen.");
            testTexts.Add("By Jove, my quick study of lexicography won a prize.");
            testTexts.Add("Painful zombies quickly watch a jinxed graveyard.");
            testTexts.Add("Fax back Jim's Gwyneth Paltrow video quiz.");
            testTexts.Add("My girl wove six dozen plaid jackets before she quit.");
            testTexts.Add("Six big devils from Japan quickly forgot how to waltz.");
            testTexts.Add("\"Who am taking the ebonics quiz?\", the prof jovially axed.");
            testTexts.Add("Why shouldn't a quixotic Kazakh vampire jog barefoot?");
            testTexts.Add("Within Jack's dark empty cave, boxes of quartz glisten.");
            testTexts.Add("Big July earthquakes confound zany experimental vow.");
            testTexts.Add("Foxy parsons quiz and cajole the lovably dim wiki-girl.");
            testTexts.Add("Cute, kind, jovial, foxy physique, amazing beauty? Wowser!");
            testTexts.Add("Have a pick: twenty six letters - no forcing a jumbled quiz!");
            testTexts.Add("A very big box sailed up then whizzed quickly from Japan.");
            testTexts.Add("Jack quietly moved up front and seized the big ball of wax.");
            testTexts.Add("Few black taxis drive up major roads on quiet hazy nights.");
            testTexts.Add("Just poets wax boldly as kings and queens march over fuzz.");

            testTexts.Add("Crazy Fredericka bought many very exquisite opal jewels.");
            testTexts.Add("Sixty zippers were quickly picked from the woven jute bag.");
            testTexts.Add("The job of waxing linoleum frequently peeves chintzy kids.");
            testTexts.Add("How razorback-jumping frogs can level six piqued gymnasts!");

            /*Removing lines that are too big to fit on screen.
            testTexts.Add("Grumpy wizards make toxic brew for the evil Queen and Jack.");
            testTexts.Add("A quick movement of the enemy will jeopardize six gunboats.");
            testTexts.Add("Back in June we delivered oxygen equipment of the same size.");
            testTexts.Add("Just keep examining every low bid quoted for zinc etchings.");
            testTexts.Add("All questions asked by five watched experts amaze the judge.");
            testTexts.Add("The wizard quickly jinxed the gnomes before they vaporized.");
            testTexts.Add("Bored? Craving a pub quiz fix? Why, just come to the Royal Oak!");*/


        }

        private void InitializeColorSchemes()
        {
            colorSchemeIndex = 0;
            colorSchemes = new List<KeyboardManager.ColorScheme>();
            colorSchemes.Add(KeyboardManager.ColorScheme.Default);
            colorSchemes.Add(KeyboardManager.ColorScheme.Blue);
            colorSchemes.Add(KeyboardManager.ColorScheme.Red);

        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardManager.Update(gameTime);

            switch (currentPhase)
            {
                case TestPhase.Setup:
                    UpdateSetup(gameTime);
                    break;
                case TestPhase.Options:
                    UpdateOptions(gameTime);
                    break;
                case TestPhase.InKeyboard:
                    stopWatch += gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (keyboardType == KeyboardType.Super)
                        UpdateSuper(gameTime);
                    else if (keyboardType == KeyboardType.Default)
                        UpdateDefault(gameTime);
                    else
                        currentPhase = TestPhase.Options;
                    break;
            }

     
                
            base.Update(gameTime);
        }

        public void UpdateSuper(GameTime gameTime)
        {
            if (!KeyboardManager.IsActive)
            {
                String Text = KeyboardManager.GetText();

                OutputLog.Add(GenerateOutputLine(Text,KeyboardType.Super));

                keyboardType = KeyboardType.None;
                currentPhase = TestPhase.Options;
            }
        }

        public void UpdateDefault(GameTime gameTime)
        {
            if (!Guide.IsVisible)
            {
                String Text = Guide.EndShowKeyboardInput(DefaultKeyboardResult);

                OutputLog.Add(GenerateOutputLine(Text, KeyboardType.Default));

                keyboardType = KeyboardType.None;
                currentPhase = TestPhase.Options;
            
            }
           
        }

        public String GenerateOutputLine(String text, KeyboardType type)
        {
            bool Match = (text == testTexts[testTextIndex]);

            return (type==KeyboardType.Super?"SK9000    ":"DEFAULT    ") + (passwordMode ? "PASSWORD MODE    " : "") + (type == KeyboardType.Super?"COLOR_" + colorSchemes[colorSchemeIndex].ToString():"") +"    " + testTexts[testTextIndex] + "    " + text + "    " + (Match ? "Match" : "Not a Match") + "    " + stopWatch.ToString("###,###.00") + " ms";
                
        }

        public void UpdateOptions(GameTime gameTime)
        {
            LogVelocity = new Vector2(5*InputManager.GetRightStickX(tester), -5*InputManager.GetRightStickY(tester));
            LogPosition += LogVelocity;

            if (InputManager.DPadLeftPressed(tester)||InputManager.LeftAnalogPressed(tester))
            {
                testTextIndex--;
                if (testTextIndex < 0)
                    testTextIndex = testTexts.Count - 1;
            }
            if (InputManager.DPadRightPressed(tester) ||InputManager.RightAnalogPressed(tester))
            {
                testTextIndex++;
                if (testTextIndex >= testTexts.Count)
                    testTextIndex = 0;
            }


            if (InputManager.LBPressed(tester))
            {
                colorSchemeIndex--;
                if (colorSchemeIndex < 0)
                    colorSchemeIndex = colorSchemes.Count - 1;
                KeyboardManager.ChangeColorScheme(colorSchemes[colorSchemeIndex]);
            }
            if (InputManager.RBPressed(tester))
            {
                colorSchemeIndex++;
                if (colorSchemeIndex >= colorSchemes.Count)
                    colorSchemeIndex = 0;
                KeyboardManager.ChangeColorScheme(colorSchemes[colorSchemeIndex]);
            }

            if (InputManager.APressed(tester))
            {
                KeyboardManager.ShowKeyboard(testee,passwordMode);
                keyboardType = KeyboardType.Super;
                currentPhase = TestPhase.InKeyboard;
                stopWatch = 0;
            }
            else if (InputManager.XPressed(tester))
            {
                DefaultKeyboardResult = Guide.BeginShowKeyboardInput(testee, "Microsoft Keyboard","", "", null, null,passwordMode);
                keyboardType = KeyboardType.Default;
                currentPhase = TestPhase.InKeyboard;
                stopWatch = 0;
            }

            if (InputManager.RStickClickedIn(tester))
            {
                LogPosition = DefaultLogPosition;
            }

            if (InputManager.GetRightTrigger(tester) >= .7f)
            {
                passwordMode = true;
            }
            else
            {
                passwordMode = false;
            }
            
        }

        public void UpdateSetup(GameTime gameTime)
        {
            switch (currentSetupPhase)
            {
                case SetupPhase.ChooseTester:
                    PollForTester();
                    if (testerChosen)
                        currentSetupPhase = SetupPhase.ChooseTestee;
                    break;
                case SetupPhase.ChooseTestee:
                    PollForTestee();
                    if (testeeChosen)
                    {
                        currentSetupPhase = SetupPhase.SetupComplete;
                        currentPhase = TestPhase.Options;
                    }
                    break;
                case SetupPhase.SetupComplete:
                    break;
            }
        }

        public void PollForTester()
        {
            if (InputManager.APressed(PlayerIndex.One))
            {
                tester = PlayerIndex.One;
                testerChosen = true;
            }
            if (InputManager.APressed(PlayerIndex.Two))
            {
                tester = PlayerIndex.Two;
                testerChosen = true;
            }
            if (InputManager.APressed(PlayerIndex.Three))
            {
                tester = PlayerIndex.Three;
                testerChosen = true;
            }
            if (InputManager.APressed(PlayerIndex.Four))
            {
                tester = PlayerIndex.Four;
                testerChosen = true;
            }
        }

        public void PollForTestee()
        {
            //Originally I was going to force things so that tester and testee couldn't use the same controller.
            //I took this out hence the commented out lines of code below.

            if (InputManager.APressed(PlayerIndex.One))// && tester != PlayerIndex.One)
            {
                testee = PlayerIndex.One;
                testeeChosen = true;
            }
            if (InputManager.APressed(PlayerIndex.Two))// && tester != PlayerIndex.Two)
            {
                testee = PlayerIndex.Two;
                testeeChosen = true;
            }
            if (InputManager.APressed(PlayerIndex.Three))// && tester != PlayerIndex.Three)
            {
                testee = PlayerIndex.Three;
                testeeChosen = true;
            }
            if (InputManager.APressed(PlayerIndex.Four))//&& tester != PlayerIndex.Four)
            {
                testee = PlayerIndex.Four;
                testeeChosen = true;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(255,226,226));

            spriteBatch.Begin();


            if (currentPhase == TestPhase.Setup)
                DrawSetup(spriteBatch);
            else
            {
                spriteBatch.Draw(Background, new Vector2(0, 0), new Color(255, 226, 226));
                DrawOutputLogDisplay(spriteBatch);
                spriteBatch.Draw(Cover, new Vector2(0, 0), new Color(255, 226, 226));
                DrawPlayerDisplay(spriteBatch);
                DrawTestTextDisplay(spriteBatch);
                spriteBatch.Draw(UIForeground, new Vector2(0, 0), Color.White);
            }
           
            KeyboardManager.Draw(spriteBatch);
            
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void DrawSetup(SpriteBatch spriteBatch)
        {
            String DisplayText = "";

            switch (currentSetupPhase)
            {
                case SetupPhase.ChooseTester:
                    DisplayText = "Tester - Press A";
                    break;
                case SetupPhase.ChooseTestee:
                    DisplayText = "Testee - Press A";
                    break;
            }
            Vector2 Measurement = testingFont.MeasureString(DisplayText);
            spriteBatch.DrawString(testingFont, DisplayText, new Vector2(640, 360) - Measurement / 2, Color.Black);

        }

        public void DrawTestTextDisplay(SpriteBatch spriteBatch)
        {
            String Text = testTexts[testTextIndex];

            Vector2 Measurement = testingFont.MeasureString(Text);
            spriteBatch.DrawString(testingFont, Text, new Vector2(845, 120) - Measurement / 2, Color.Black);

        }

        public void DrawOutputLogDisplay(SpriteBatch spriteBatch)
        {
            float y = LogPosition.Y;
            foreach (String aString in OutputLog)
            {
                spriteBatch.DrawString(smallTestingFont, aString, new Vector2(LogPosition.X, y), Color.Black);
                y += 25;
            }
        }

        public void DrawPlayerDisplay(SpriteBatch spriteBatch)
        {
            String TesterDisplay = "Tester: Controller " + tester.ToString();
            String TesteeDisplay = "Testee: Controller " + testee.ToString();
            String ColorDisplay = "Color Scheme: " + colorSchemes[colorSchemeIndex].ToString();

            spriteBatch.DrawString(testingFont, TesterDisplay, new Vector2(60, 60), Color.Black);
            spriteBatch.DrawString(testingFont, TesteeDisplay, new Vector2(60, 90), Color.Black);
            spriteBatch.DrawString(testingFont, ColorDisplay,  new Vector2(60, 120), Color.Black);


            String LeftAnaolog = "Change Text";
            String RightAnalog = "Scroll Log";
            String A = "Super Keyboard 9000";
            String X = "Default Keyboard";
            String Bumpers = "Change Color Scheme";

            spriteBatch.DrawString(smallTestingFont, LeftAnaolog, new Vector2(180, 280), Color.Black);
            spriteBatch.DrawString(smallTestingFont, RightAnalog, new Vector2(180, 360), Color.Black);
            spriteBatch.DrawString(smallTestingFont, A, new Vector2(180, 440), Color.Black);
            spriteBatch.DrawString(smallTestingFont, X, new Vector2(180, 500), Color.Black);
            spriteBatch.DrawString(smallTestingFont, Bumpers, new Vector2(180, 580), Color.Black);

        }
    }
}
