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
        public const int TextBoxSize = 700;
        public const double CursorFlashTimerSetting = 550;

        //arrays make everything better. trust me. sleep. *pats*
        public static Vector2[] TriggerOffset = {new Vector2( 300, -100), //a (see TriggerName)
                                                 new Vector2( 300, -250), //b
                                                 new Vector2( -300, -100), //x
                                                 new Vector2( -300, -250), //y
                                                 new Vector2(-300, 90), //left
                                                 new Vector2( 300, 90)}; //right
        public static int NumberOfTriggers = 6;

        public static Vector2 TextBoxOffset = new Vector2(0, -250);
        public static Vector2 TextOffset = new Vector2(90, 40);

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
        
        int TextIndex;
        int StartIndex;

        Sprite TextBox;
        Sprite Cursor;

        //Sprite LTrigger;
        //Sprite RTrigger;
        Trigger[] Triggers = new Trigger[6];

        KeyboardMode Mode;

        bool CursorInFlash;
        double CursorFlashTimer;

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

            //change these once we have all 4 buttons.
            Triggers[(int)TriggerName.A] = new Trigger(new Sprite(textures.ATrigger),
                new Vector2(textures.ATrigger.Width / 2, textures.ATrigger.Height / 2),
                "Select");
            Triggers[(int)TriggerName.B] = new Trigger(new Sprite(textures.ATrigger),
    new Vector2(textures.ATrigger.Width / 2, textures.ATrigger.Height / 2),
    "Back");
            Triggers[(int)TriggerName.X] = new Trigger(new Sprite(textures.ATrigger),
    new Vector2(textures.ATrigger.Width / 2, textures.ATrigger.Height / 2),
    "BackSpace");
            Triggers[(int)TriggerName.Y] = new Trigger(new Sprite(textures.ATrigger),
    new Vector2(textures.ATrigger.Width / 2, textures.ATrigger.Height / 2),
    "Space");
            Triggers[(int)TriggerName.Left] = new Trigger(new Sprite(textures.LTrigger),
    new Vector2(textures.LTrigger.Width / 2, textures.LTrigger.Height / 2),
    "Characters");
            Triggers[(int)TriggerName.Right] = new Trigger(new Sprite(textures.RTrigger),
    new Vector2(textures.RTrigger.Width / 2, textures.RTrigger.Height / 2),
    "Capitalize");

            TextBox = new Sprite(textures.TextBox);
            TextBox.Origin = new Vector2(TextBox.Width / 2, TextBox.Height / 2);

            Cursor = new Sprite(textures.Cursor);
            Cursor.Color = colors.Cursor;

            CurrentStatus = KeyboardStatus.Inactive;
            Mode = KeyboardMode.Normal;

            TextIndex = 0;
            StartIndex = 0;

            InitializeKeys(colors);

            ResetCursorFlashTimer();
        }

        //Needs to be reset (or "forced on") whenever you scroll the cursor
        private void ResetCursorFlashTimer()
        {
            CursorInFlash = false;
            CursorFlashTimer = CursorFlashTimerSetting;
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

            //triggers
            /*for (int i = 0; i < NumberOfTriggers; i++)
                Triggers[i].SetState(State.notpressed);*/
            Triggers[(int)TriggerName.Left].SetState(State.notpressed);
            Triggers[(int)TriggerName.Right].SetState(State.notpressed);
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

            UpdatePositions();

            //So you can't use the keyboard while it's entering or leaving
            if (CurrentStatus == KeyboardStatus.Active)
            {
                UpdateActive(gameTime);
            }
        }

        //Updates positions relative to frame
        private void UpdatePositions()
        {
            BackCircle.Position.X = Frame.FramePosition.X + (Frame.Width / 2.0f);
            BackCircle.Position.Y = Frame.FramePosition.Y + (Frame.Height / 2.0f);

            //told you arrays make everything better.
            for (int i = 0; i < NumberOfTriggers; i++)
            {
                Triggers[i].SetLocation(new Vector2(
                    Frame.FramePosition.X + (Frame.Width / 2.0f) + TriggerOffset[i].X,
                    Frame.FramePosition.Y + (Frame.Width / 2.0f) + TriggerOffset[i].Y));

            }

            TextBox.Position.X = Frame.FramePosition.X + (Frame.Width / 2.0f) + TextBoxOffset.X;
            TextBox.Position.Y = Frame.FramePosition.Y + (Frame.Height / 2.0f) + TextBoxOffset.Y;
        }

        private void UpdateActive(GameTime gameTime)
        {
            UpdateInput(gameTime);

            CursorFlashTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;

            if (CursorFlashTimer <= 0)
            {
                CursorFlashTimer = CursorFlashTimerSetting;
                CursorInFlash = !CursorInFlash;
            }
         
        }

        private void UpdateInput(GameTime gameTime)
        {
            if (InputManager.LeftTriggerPressed(PlayerIndex.One))
            {
                Mode = KeyboardMode.Symbols;
                Triggers[(int)TriggerName.Left].SetState(State.pressed);
            }
            else if (InputManager.RightTriggerPressed(PlayerIndex.One))
            {
                Mode = KeyboardMode.Caps;
                Triggers[(int)TriggerName.Right].SetState(State.pressed);
            }

            double rotation = InputManager.GetLeftPolar(PlayerIndex.One).getAngle();
            double roundedIndexValue = Math.Round(rotation / magicNumber); //very important number.
            
            //Adjustment for an edge case;
            if (roundedIndexValue >= 26)
            {
                roundedIndexValue -= 26;
            }
            
            Key CurrentKey = null;

            if (roundedIndexValue < 26)
                CurrentKey = Keys[(int)roundedIndexValue];
            else//Should never reach here
                Console.WriteLine("ERROR" + roundedIndexValue);


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
                    WhatTyped = WhatTyped.Insert(TextIndex,CurrentKey.GetText(Mode));
                    TextIndex++;

                    //press the key, to kick start the key press "animation"
                    CurrentKey.Press();
                    ResetCursorFlashTimer();
                }
            }

            if(InputManager.APressed(PlayerIndex.One))
                    Triggers[(int)TriggerName.A].SetState(State.pressed);
            if (InputManager.AReleased(PlayerIndex.One))
                Triggers[(int)TriggerName.A].SetState(State.notpressed);

            //and subtraction
            if (InputManager.XPressed(PlayerIndex.One))
            {
                InputManager.SetRumble(PlayerIndex.One, 100.0f, 0.0f);

                //remove a key to our typed messege
                if (TextIndex >0)
                {
                    WhatTyped = WhatTyped.Remove(TextIndex - 1, 1);
                    TextIndex--;
                }
                //We want to force the cursor to display even if nothing is deleted, so the user can get his bearings
                ResetCursorFlashTimer();

                Triggers[(int)TriggerName.X].SetState(State.pressed);
            }
            if (InputManager.XReleased(PlayerIndex.One))
                Triggers[(int)TriggerName.X].SetState(State.notpressed);

            //Y adds a space
            if (InputManager.YPressed(PlayerIndex.One))
            {
                ResetCursorFlashTimer();
                WhatTyped = WhatTyped.Insert(TextIndex, " ");
                TextIndex++;

                Triggers[(int)TriggerName.Y].SetState(State.pressed);
            }
            if (InputManager.YReleased(PlayerIndex.One))
                Triggers[(int)TriggerName.Y].SetState(State.notpressed);

            if (InputManager.DPadLeftPressed(PlayerIndex.One) || InputManager.LBPressed(PlayerIndex.One))
            {
                ResetCursorFlashTimer();
                TextIndex--;
                if (TextIndex < 0)
                    TextIndex = 0;
            }

            if (InputManager.DPadRightPressed(PlayerIndex.One) || InputManager.RBPressed(PlayerIndex.One))
            {
                ResetCursorFlashTimer();
                TextIndex++;
                if (TextIndex > WhatTyped.Length)
                    TextIndex = WhatTyped.Length;
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

            for (int i = 0; i < NumberOfTriggers; i++)
                Triggers[i].Draw(spriteBatch, Font);
            TextBox.Draw(spriteBatch);

            DrawText(spriteBatch);     
        }

        private void Correct()
        {
            if (StartIndex > TextIndex)
            {
                StartIndex--;
                Correct();
            }

            float CursorPoint = Font.MeasureString(WhatTyped.Substring(StartIndex, TextIndex - StartIndex)).X;
            if (CursorPoint > TextBoxSize)
            {
                StartIndex++;
                Correct();
            }
            
        }

        private void DrawText(SpriteBatch spriteBatch)
        {
            Correct();
                     
            float CursorPoint = Font.MeasureString(WhatTyped.Substring(StartIndex, TextIndex - StartIndex)).X;
           
            String DisplayText = WhatTyped.Substring(StartIndex);
            int DisplayLength = DisplayText.Length;

            while (Font.MeasureString(DisplayText).X > TextBoxSize)
            {

                DisplayLength--;
                DisplayText = WhatTyped.Substring(StartIndex, DisplayLength);
            }

            spriteBatch.DrawString(Font, DisplayText, Frame.FramePosition + TextOffset, InputTextColor);

            if (!CursorInFlash)
            {
                Cursor.Position = Frame.FramePosition + TextOffset + new Vector2(CursorPoint, 0);
                Cursor.Draw(spriteBatch);
            }
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
