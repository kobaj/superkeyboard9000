using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Keyboard
{
    public static class KeyboardManager
    {
        public enum ColorScheme { Blue, Red, Default}
        private static bool Active;
        private static Keyboard CurrentKeyboard;
        private static KeyboardTextures Textures;
        private static KeyboardColors Colors;
        private static KeyboardFont Fonts;

        public static bool IsActive { get { return Active; } }


        public static void Initialize(ContentManager content)
        {
            Textures = new KeyboardTextures()
            {
                Frame = content.Load<Texture2D>("Keyboard/Graphics/Frame"),
                Fade = content.Load<Texture2D>("Keyboard/Graphics/Fade"),
                BackCircle = content.Load<Texture2D>("Keyboard/Graphics/backcircle"),
                InnerCircle = content.Load<Texture2D>("keyboard/Graphics/innercircle"),
                Key = content.Load<Texture2D>("Keyboard/Graphics/key"),
                LTrigger = content.Load<Texture2D>("Keyboard/Graphics/lTrigger"),
                RTrigger = content.Load<Texture2D>("Keyboard/Graphics/rTrigger"),
                AButton = content.Load<Texture2D>("Keyboard/Graphics/aButton"),
                BButton = content.Load<Texture2D>("Keyboard/Graphics/bButton"),
                XButton = content.Load<Texture2D>("Keyboard/Graphics/xButton"),
                YButton = content.Load<Texture2D>("Keyboard/Graphics/yButton"),
                BackButton = content.Load<Texture2D>("Keyboard/Graphics/backButton"),
                StartButton = content.Load<Texture2D>("Keyboard/Graphics/startButton"),
                TextBox = content.Load<Texture2D>("Keyboard/Graphics/TextBox"),
                Cursor = content.Load<Texture2D>("Keyboard/Graphics/Cursor"),

                arrow = content.Load<Texture2D>("Keyboard/Graphics/arrow"),
                ExtraLetters = content.Load<Texture2D>("Keyboard/Graphics/up")
            };

            SetColorScheme(ColorScheme.Default);

            Fonts = new KeyboardFont()
            {
                Keys = content.Load<SpriteFont>("Keyboard/Fonts/Arial"),
                InputText = content.Load<SpriteFont>("Keyboard/Fonts/Arial Black"),
                Labels = content.Load<SpriteFont>("Keyboard/Fonts/Arial Black")
            };

            CurrentKeyboard = new Keyboard(Textures,Colors, Fonts);
            Active = false;
        }

        public static void ChangeColorScheme(ColorScheme scheme)
        {
            SetColorScheme(scheme);

            CurrentKeyboard = new Keyboard(Textures, Colors, Fonts);
            Active = false;

        }

        public static String GetText()
        {
            return CurrentKeyboard.GetText();
        }

        private static void SetColorScheme(ColorScheme scheme)
        {
            switch (scheme)
            {
                case ColorScheme.Blue:
                    Colors = new KeyboardColors()
                    {
                        Frame = new Color(14, 83, 167),
                        Fade = new Color(104, 153, 211),
                        BackCircle = new Color(4, 52, 108),
                        Key = new Color(66, 132, 211),
                        CurrentKey = new Color(39, 78, 125),
                        KeyText = Color.White,
                        AlternateKeyText = Color.LightGray,
                        KeyVowelText = Color.LightGreen,
                        InputText = Color.Black,
                        CurrentKeyPressed = new Color(27, 34, 114),
                        TextBox = new Color(255, 255, 255),
                        Cursor = new Color(0, 0, 0)
                    };
                    break;
                case ColorScheme.Red:
                    Colors = new KeyboardColors()
                    {
                        Frame = new Color(237, 144, 144),
                        Fade = new Color(237, 169, 169),
                        BackCircle = new Color(219, 104, 104),
                        Key = new Color(164, 100, 100),
                        CurrentKey = new Color(142, 34, 34),
                        KeyText = Color.White,
                        AlternateKeyText = Color.LightGray,
                        KeyVowelText = Color.LightGreen,
                        InputText = Color.Black,
                        CurrentKeyPressed = new Color(73, 4, 4),
                        TextBox = new Color(255, 255, 255),
                        Cursor = new Color(0, 0, 0)
                    };
                    break;
                case ColorScheme.Default:
                    Colors = new KeyboardColors()
                    {
                        Frame = new Color(113, 120, 122),
                        Fade = new Color(20, 20, 20),
                        BackCircle = new Color(217, 223, 226),
                        Key = new Color(197, 203, 206),
                        CurrentKey = new Color(83, 145, 3),
                        KeyText = Color.White,
                        AlternateKeyText = new Color(50,50,50),
                        KeyVowelText = Color.DarkGreen,
                        InputText = Color.Black,
                        CurrentKeyPressed = new Color(63, 125, 3),
                        TextBox = new Color(255, 255, 255),
                        Cursor = new Color(0, 0, 0)
                    };
                    break;
                    
            }

        }

        public static void ShowKeyboard(PlayerIndex player, bool passwordMode)
        {
            CurrentKeyboard.Enter(player,passwordMode);
        }

        public static void HideKeyboard()
        {
            CurrentKeyboard.Leave();
        }

        public static void Update(GameTime gameTime)
        {
            InputManager.AllUpdateInput();

            Active = CurrentKeyboard.IsActive;

            CurrentKeyboard.Update(gameTime);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            if(Active)
                CurrentKeyboard.Render(spriteBatch);
        }


    }

        
}
