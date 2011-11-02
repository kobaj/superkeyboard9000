using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Keyboard
{
    public enum KeyboardStatus { Entering, Leaving, Active, Inactive }

    public enum KeyboardMode { Normal, Caps, Symbols }

    public class Keyboard : RenderArea
    {
        public const double magicNumber = Math.PI / 13.0D; // 0.24166096923076923076923076923077D; //2pi / 26;
        public static Vector2 LTriggerOffset = new Vector2(-300, 150);
        public static Vector2 RTriggerOffset = new Vector2( 300, 150);

        Texture2D KeyTexture;

        SpriteFont Font;
        KeyboardStatus CurrentStatus;

        Sprite BackCircle;

        String WhatTyped = "";

        //hand type this bitch (no seriously, this is how we make custom different versions, hand type).
        String[] Alphabet = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
        String[] Symbols  = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", ".", "!", "?", ",", "'", "\"", "+", "=", "(", ")", "-", "@", "#", "$", "&", "%" };
        Key[] Keys;

        Color KeyTextColor;
        Color InputTextColor;

        Color KeyColor;
        Color CurrentKeyColor;
        Color CurrentKeyColorPressed;

        Color FadeColor;
        Sprite Fade;
        float FadeAlpha;
        float FadeAlphaMax;
        float FadeIncrement;

        KeyboardFrame Frame;
        int FrameOnScreenY;
        int FrameOffScreenY;
        int FrameVelocityY;

        Sprite LTrigger;
        Sprite RTrigger;

        KeyboardMode Mode;

        public bool IsActive { get { return CurrentStatus != KeyboardStatus.Inactive; } }

        public Keyboard(KeyboardTextures textures, KeyboardColors colors, SpriteFont font)
        {
            //If we get a chance, we should modify this to work with other resolutions
            InitializeRenderArea(0, 0, 1280, 720);

            //setup font
            Font = font;

            KeyTextColor = colors.KeyText;
            InputTextColor = colors.InputText;

            KeyColor = colors.Key;
            CurrentKeyColor = colors.CurrentKey;
            CurrentKeyColorPressed = colors.CurrentKeyPressed;

            //setup our lovely keys.
            BackCircle = new Sprite(textures.BackCircle, new Vector2(0, 0));
            BackCircle.Origin = new Vector2(BackCircle.Width / 2.0f, BackCircle.Height / 2.0f);
            BackCircle.Color = colors.BackCircle;

    
            Fade = new Sprite(textures.Fade, new Vector2(0, 0));
            FadeColor = colors.Fade;
            FadeAlpha = 0;
            FadeAlphaMax = 180;
            FadeIncrement = 15;

            KeyTexture = textures.Key;

            FrameOnScreenY = 50;
            FrameOffScreenY = 720;
            FrameVelocityY = -5;

            Frame = new KeyboardFrame(200, FrameOffScreenY, textures.Frame, colors.Frame);

            LTrigger = new Sprite(textures.LTrigger);
            LTrigger.Origin = new Vector2(LTrigger.Width / 2, LTrigger.Height / 2);

            RTrigger = new Sprite(textures.RTrigger);
            RTrigger.Origin = new Vector2(RTrigger.Width / 2, RTrigger.Height / 2);


            CurrentStatus = KeyboardStatus.Inactive;
            Mode = KeyboardMode.Normal;

            InitializeKeys(colors);
        }

        private void InitializeKeys(KeyboardColors colors)
        {
            Keys = new Key[26];

            for (int i = 0; i < 26; i++)
            {
                double rotation = i * magicNumber;
                double roundedIndexValue = Math.Round(rotation / magicNumber); //very important number.
                float finalRotation = (float)(roundedIndexValue * magicNumber);

                Keys[i] =new Key(KeyTexture, Alphabet[i], Symbols[i], Font, colors,finalRotation,i);
            }

        }

        public void Enter()
        {
            CurrentStatus = KeyboardStatus.Entering;
        }

        public void Leave()
        {
            CurrentStatus = KeyboardStatus.Leaving;
        }


        public void Reset()
        {
            InputManager.SetRumble(PlayerIndex.One, 0.0f, 0.0f);

            //Reset key states from selected to normal
            foreach (Key aKey in Keys)
            {
                if (aKey.State == KeyState.Selected)
                    aKey.State = KeyState.Normal;
            }

            Mode = KeyboardMode.Normal;
        }

        public void Update(GameTime gameTime)
        {
            //Give everything a blank slate
            Reset();
            
            switch (CurrentStatus)
            {
                case KeyboardStatus.Entering:
                    EnterUpdate(gameTime);
                    break;
                case KeyboardStatus.Leaving:
                    LeaveUpdate(gameTime);
                    break;
            }


            //Updates keys to handle press "animation"
            foreach (Key aKey in Keys)
            {
                aKey.Update(gameTime);
            }

            BackCircle.Position.X = Frame.FramePosition.X + (Frame.Width / 2.0f);
            BackCircle.Position.Y = Frame.FramePosition.Y + (Frame.Height / 2.0f);
            
            LTrigger.Position.X = Frame.FramePosition.X + (Frame.Width / 2.0f) + LTriggerOffset.X;
            LTrigger.Position.Y = Frame.FramePosition.Y + (Frame.Height / 2.0f) + LTriggerOffset.Y;

            RTrigger.Position.X = Frame.FramePosition.X + (Frame.Width / 2.0f) + RTriggerOffset.X;
            RTrigger.Position.Y = Frame.FramePosition.Y + (Frame.Height / 2.0f) + RTriggerOffset.Y;
            
            //So you can't use the keyboard while it's entering or leaving
            if (CurrentStatus == KeyboardStatus.Active)
            {
                UpdateInput(gameTime);
            }

            

            
        }

        private void UpdateInput(GameTime gameTime)
        {
            

            if (InputManager.LeftTriggerPressed(PlayerIndex.One))
                Mode = KeyboardMode.Symbols;
            else if (InputManager.RightTriggerPressed(PlayerIndex.One))
                Mode = KeyboardMode.Caps;

            double rotation = InputManager.GetLeftPolar(PlayerIndex.One).getAngle();
            double roundedIndexValue = Math.Round(rotation / magicNumber); //very important number.
            
            //Adjustment for an edge case;
            if (roundedIndexValue >= 26)
                roundedIndexValue -= 26;
            
            Key CurrentKey = null;

            if (roundedIndexValue < 26)
                CurrentKey = Keys[(int)roundedIndexValue];
            else//Should never reach here
                Console.WriteLine("ERROR");


            if (CurrentKey != null)
            {

                if (CurrentKey.State != KeyState.Pressed)
                    CurrentKey.State = KeyState.Selected;


                //addition of letters
                //bleh you use a different way of blending colors Seph.
                //Its ok. :3
                if (InputManager.APressed(PlayerIndex.One))
                {
                    InputManager.SetRumble(PlayerIndex.One, 100.0f, 0.0f);

                    //add the pressed key to our typed messege
                     WhatTyped += CurrentKey.GetText(Mode);

                    //press the key, to kick start the key press "animation"
                     CurrentKey.Press();
                }
            }

            //and subtraction
            if (InputManager.XPressed(PlayerIndex.One))
            {
                InputManager.SetRumble(PlayerIndex.One, 100.0f, 0.0f);

                //remove a key to our typed messege
                if (WhatTyped.Length > 0)
                    WhatTyped = WhatTyped.Remove(WhatTyped.Length - 1, 1);
            }

            //Y adds a space
            if (InputManager.YPressed(PlayerIndex.One))
            {
                WhatTyped += " ";
            }

            
        }

        private void LeaveUpdate(GameTime gameTime)
        {
            Frame.FramePosition.Y-= FrameVelocityY * ((float)gameTime.ElapsedGameTime.TotalMilliseconds);
            FadeAlpha -= FadeIncrement;

            bool MoveComplete = false;
            bool FadeComplete = false;

            if (Frame.FramePosition.Y >= FrameOffScreenY)
            {
                Frame.FramePosition.Y = FrameOffScreenY;
                MoveComplete = true;
            }

            if (FadeAlpha <= 0)
            {
                FadeAlpha = 0;
                FadeComplete = true;
            }

            if (MoveComplete && FadeComplete)
            {
                CurrentStatus = KeyboardStatus.Inactive;
            }

            Frame.Update(gameTime);
        }

        private void EnterUpdate(GameTime gameTime)
        {
            Frame.FramePosition.Y +=(FrameVelocityY * ((float)gameTime.ElapsedGameTime.TotalMilliseconds));
            FadeAlpha += FadeIncrement;

            bool MoveComplete = false;
            bool FadeComplete = false;

            if (Frame.FramePosition.Y <= FrameOnScreenY)
            {
                Frame.FramePosition.Y =FrameOnScreenY;
                MoveComplete = true;
            }

            if (FadeAlpha >= FadeAlphaMax)
            {
                FadeAlpha = FadeAlphaMax;
                FadeComplete = true;
            }

            if (MoveComplete && FadeComplete)
            {
                CurrentStatus = KeyboardStatus.Active;
            }

            Frame.Update(gameTime);
        }

        protected override void Draw(SpriteBatch spriteBatch)
        {
            //remember kids, first thing drawn is furthest away from you!
            Fade.Color = new Color(FadeHelper(FadeColor.R), FadeHelper(FadeColor.G), FadeHelper(FadeColor.B), FadeHelper(255));
            Fade.Draw(spriteBatch);
            Frame.Draw(spriteBatch);
            BackCircle.Draw(spriteBatch);

            

            RenderKeys(spriteBatch);

            LTrigger.Draw(spriteBatch);
            RTrigger.Draw(spriteBatch);
            

            

            spriteBatch.DrawString(Font, WhatTyped, new Vector2(Frame.FramePosition.X + 20, Frame.FramePosition.Y + 40), InputTextColor);
        }

        private void RenderKeys(SpriteBatch spriteBatch)
        {

            foreach (Key aKey in Keys)
            {
                Vector2 Position = new Vector2(Frame.FramePosition.X + (Frame.Width / 2.0f), Frame.FramePosition.Y + (Frame.Height / 2.0f));
                aKey.Draw(Mode, Position, spriteBatch);
            }
        }


        //I don't think we will need this for our keyboard.  I will go back and edit RenderArea later
        protected override void PostDraw(Texture2D screen, SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public byte FadeHelper(int max)
        {
            return (byte)MathHelper.Clamp((FadeAlpha / 255) * max, 0, max);
        }
    }
}
