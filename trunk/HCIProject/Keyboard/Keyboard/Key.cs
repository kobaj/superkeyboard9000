using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Keyboard
{

    public enum KeyState { Normal, Selected, Pressed}

    public class Key
    {

        public KeyState State;
        private SpriteFont Font;
        private String Letter;
        private String Symbol;

        private Sprite Representation;

        private Color NormalColor;
        private Color SelectedColor;
        private Color PressedColor;
        private Color KeyTextColor;

        private double Timer;
        private const double TimerSetting = 100;

        private Vector2 TextPosition;

        public Key(Texture2D texture, String letter, String symbol, SpriteFont font, KeyboardColors colors, float rotation, int index)
        {
            Font = font;

            Letter = letter;
            Symbol = symbol;

            NormalColor = colors.Key;
            SelectedColor = colors.CurrentKey;
            PressedColor = colors.CurrentKeyPressed;
            KeyTextColor = colors.KeyText;

            Representation = new Sprite(texture);
            Representation.Origin = new Vector2(Representation.Width / 2, Representation.Height / 2);
            Representation.Rotation = rotation;
            Timer = TimerSetting;

            State = KeyState.Normal;

            CalculateTextPosition(index);
        }


        private void CalculateTextPosition(int index)
        {
            float radius = 160.0f;
            float initialangle = (float)(0 - (Math.PI / 2.0D)); //(magicNumber / 2.0D)

            TextPosition  = polar.PolarToXY(new polar(radius, (float)(initialangle + (index * Keyboard.magicNumber))));
        }

        public void Press()
        {
            State = KeyState.Pressed;
            Timer = TimerSetting;

        }

        public void Update(GameTime gameTime)
        {
            if (State == KeyState.Pressed)
                UpdatePressed(gameTime);
        }

        private void UpdatePressed(GameTime gameTime)
        {
            Timer -= gameTime.ElapsedGameTime.TotalMilliseconds;

            if (Timer <= 0)
                State = KeyState.Normal;
        }


        public string GetText(KeyboardMode mode)
        {
             String text = "";

            switch (mode)
            {
                case KeyboardMode.Normal:
                    text = Letter;
                    break;
                case KeyboardMode.Caps:
                    text = Char.ToUpper(Letter[0]).ToString();
                    break;
                case KeyboardMode.Symbols:
                    text = Symbol;
                    break;
            }

            return text;

        }



        public void Draw(KeyboardMode mode, Vector2 offset, SpriteBatch spriteBatch, bool odd)
        {
            Color CurrentColor = Color.White;
            Representation.Scale = 1;

            switch (State)
            {
                case KeyState.Normal:
                    CurrentColor = NormalColor;
                    break;
                case KeyState.Selected:
                    Representation.Scale = 1.025f;
                    CurrentColor = SelectedColor;
                    break;
                case KeyState.Pressed:
                    Representation.Scale = 1.05f;
                    CurrentColor = PressedColor;
                    break;
            }

            Representation.Color = CurrentColor;
            Representation.Position = offset;

            Representation.Draw(spriteBatch);


            String text = GetText(mode);
           
            const string vowels = "aeiou";
            Color oldKeyTextColor = KeyTextColor;
            if (text.Count(chr => vowels.Contains(char.ToLower(chr))) != 0)
            {
                KeyTextColor = Microsoft.Xna.Framework.Color.LightGreen;
                //alternatively
                //KeyTextColor.G = (byte)(KeyTextColor.G - (byte)80);
            }
            else if (odd)
            {
                KeyTextColor = Microsoft.Xna.Framework.Color.LightGray;
                //alternatively
                //KeyTextColor.B = (byte)(100);
            }

            Vector2 TextOffset = Font.MeasureString(text) / 2;
            spriteBatch.DrawString(Font, text, TextPosition + offset - TextOffset, KeyTextColor);

            KeyTextColor = oldKeyTextColor;
        }
    }
}
