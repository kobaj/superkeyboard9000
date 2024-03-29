﻿using System;
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
        bool overFlow = false;
        Vector2[] extraLetOffset = { new Vector2(375, -250), //right
                                       new Vector2(-375, -250) };  //left
        Sprite extraLet;
        Sprite extraLet2; //dumb spritebatch...
        bool Password = false;

        Sprite Arrow;

        public const double magicNumber = Math.PI / 13.0D; // 0.24166096923076923076923076923077D; //2pi / 26;
        private const int TextBoxSize = 700;
        private const double CursorFlashTimerSetting = 550;

        //arrays make everything better. trust me. sleep. *pats*
        private static Vector2[] TriggerOffset = {new Vector2( 300, -100), //aButton (see TriggerName)
                                                 new Vector2( 300, -250), //bButton
                                                 new Vector2( -300, -100), //xButton
                                                 new Vector2( -300, -250), //yButton
                                                 new Vector2(-300, 90), //leftTrigger
                                                 new Vector2( 300, 90),//rightTrigger
                                                 new Vector2(-100, 150),//backButton
                                                 new Vector2( 100, 150)};//startButton 
        private static int NumberOfTriggers = 8;

        Trigger[] Triggers = new Trigger[NumberOfTriggers];
        
        private static Vector2 TextBoxOffset = new Vector2(0, -250);
        private static Vector2 TextCursorOffset = new Vector2(90, 40);
        private static Vector2 TextOffset = new Vector2(0, -4);
        Texture2D KeyTexture;

        SpriteFont KeyFont;
        SpriteFont InputFont;
        SpriteFont LabelFont;
        KeyboardStatus CurrentStatus;

        Sprite BackCircle;
        bool InnerCircleShowing = true;
        Sprite InnerCircle;

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

        KeyboardMode Mode;

        bool CursorInFlash;
        double CursorFlashTimer;

        bool ShowHelp;

        public bool IsActive { get { return CurrentStatus != KeyboardStatus.Inactive; } }

        PlayerIndex Player;
       
        public Keyboard(KeyboardTextures textures, KeyboardColors colors,KeyboardFont fonts)
        {
            extraLet2 = new Sprite(textures.ExtraLetters, new Vector2(0));
            extraLet2.Origin = new Vector2(extraLet2.Width / 2.0f, extraLet2.Height / 2.0f);

            extraLet = new Sprite(textures.ExtraLetters, new Vector2(0));
            extraLet.Origin = new Vector2(extraLet.Width / 2.0f, extraLet.Height / 2.0f);

            Arrow = new Sprite(textures.arrow, new Vector2(0, 0));
            Arrow.Origin = new Vector2(Arrow.Width / 2.0f, Arrow.Height / 2.0f);

            //If we get a chance, we should modify this to work with other resolutions
            InitializeRenderArea(0, 0, 1280, 720);

            //setup font
            KeyFont = fonts.Keys;
            LabelFont = fonts.Labels;
            InputFont = fonts.InputText;

            KeyTextColor = colors.KeyText;
            InputTextColor = colors.InputText;

            KeyColor = colors.Key;
            CurrentKeyColor = colors.CurrentKey;
            CurrentKeyColorPressed = colors.CurrentKeyPressed;

            //setup our lovely keys.
            BackCircle = new Sprite(textures.BackCircle, new Vector2(0, 0));
            BackCircle.Origin = new Vector2(BackCircle.Width / 2.0f, BackCircle.Height / 2.0f);
            BackCircle.Color = colors.BackCircle;

            InnerCircle = new Sprite(textures.InnerCircle, new Vector2(0, 0));
            InnerCircle.Origin = new Vector2(InnerCircle.Width / 2.0f, InnerCircle.Height / 2.0f);
            InnerCircle.Color = colors.CurrentKey;
    
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

            InitializeTriggers(textures);

            TextBox = new Sprite(textures.TextBox);
            TextBox.Origin = new Vector2(TextBox.Width / 2, TextBox.Height / 2);

            Cursor = new Sprite(textures.Cursor);
            Cursor.Color = colors.Cursor;

            CurrentStatus = KeyboardStatus.Inactive;
            Mode = KeyboardMode.Normal;

            TextIndex = 0;
            StartIndex = 0;

            ShowHelp = true;

            InitializeKeys(colors);

            ResetCursorFlashTimer();
        }

       

        private void InitializeTriggers(KeyboardTextures textures)
        {
            Triggers[(int)TriggerName.A] = new Trigger(new Sprite(textures.AButton),
               new Vector2(textures.AButton.Width / 2, textures.AButton.Height / 2),
               "Select");

            Triggers[(int)TriggerName.B] = new Trigger(new Sprite(textures.BButton),
                new Vector2(textures.BButton.Width / 2, textures.BButton.Height / 2),
                "Cancel");

            Triggers[(int)TriggerName.X] = new Trigger(new Sprite(textures.XButton),
                new Vector2(textures.XButton.Width / 2, textures.XButton.Height / 2),
                "Backspace");

            Triggers[(int)TriggerName.Y] = new Trigger(new Sprite(textures.YButton),
                new Vector2(textures.YButton.Width / 2, textures.YButton.Height / 2),
                "Space");

            Triggers[(int)TriggerName.Left] = new Trigger(new Sprite(textures.LTrigger),
                new Vector2(textures.LTrigger.Width / 2, textures.LTrigger.Height / 2),
                "Symbols");

            Triggers[(int)TriggerName.Right] = new Trigger(new Sprite(textures.RTrigger),
                new Vector2(textures.RTrigger.Width / 2, textures.RTrigger.Height / 2),
                "Capitals");

            Triggers[(int)TriggerName.Back] = new Trigger(new Sprite(textures.BackButton),
                new Vector2(textures.StartButton.Width / 2, textures.StartButton.Height / 2),
                "Toggle Help");

            Triggers[(int)TriggerName.Start] = new Trigger(new Sprite(textures.StartButton),
                new Vector2(textures.BackButton.Width / 2, textures.BackButton.Height / 2),
                "Enter");
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

                Keys[i] =new Key(KeyTexture, Alphabet[i], Symbols[i], KeyFont, colors,finalRotation,i);
            }

        }

        public void Enter(PlayerIndex player, bool passwordMode)
        {
            WhatTyped = "";
            Player = player;
            Password = passwordMode;
            CurrentStatus = KeyboardStatus.Entering;


        }

        public void Leave()
        {
            CurrentStatus = KeyboardStatus.Leaving;
        }


        public void Reset()
        {
            InputManager.SetRumble(Player, 0.0f, 0.0f);

            //Reset key states from selected to normal
            foreach (Key aKey in Keys)
            {
                if (aKey.State == KeyState.Selected)
                    aKey.State = KeyState.Normal;
            }

            Mode = KeyboardMode.Normal;

            //triggers
            Triggers[(int)TriggerName.Left].SetState(State.notpressed);
            Triggers[(int)TriggerName.Right].SetState(State.notpressed);

            InnerCircleShowing = false;
        }

        public void Update(GameTime gameTime)
        {
            
            
            switch (CurrentStatus)
            {
                case KeyboardStatus.Entering:
                    EnterUpdate(gameTime);
                    break;
                case KeyboardStatus.Leaving:
                    LeaveUpdate(gameTime);
                    break;
                case KeyboardStatus.Active:
                    //Give everything a blank slate
                    //But not when entering or leaving - trust me on this
                    Reset();
                    break;

            }


            //Updates keys to handle press "animation"
            foreach (Key aKey in Keys)
            {
                aKey.Update(gameTime);
            }

            //is this only supposed to be called once?
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
            //yes this is rotation in the positions update, but they never change so no worries
            extraLet.Position.X = Frame.FramePosition.X + (Frame.Width / 2.0f);
            extraLet.Position.Y = Frame.FramePosition.Y + (Frame.Height / 2.0f);
            extraLet.Position += extraLetOffset[0]; //left
            extraLet.Rotation = (float)(Math.PI / 2.0f);

            extraLet2.Position.X = Frame.FramePosition.X + (Frame.Width / 2.0f);
            extraLet2.Position.Y = Frame.FramePosition.Y + (Frame.Height / 2.0f);
            extraLet2.Position += extraLetOffset[1]; //right
            extraLet2.Rotation = (float)(-Math.PI / 2.0f);

            BackCircle.Position.X = Frame.FramePosition.X + (Frame.Width / 2.0f);
            BackCircle.Position.Y = Frame.FramePosition.Y + (Frame.Height / 2.0f);

            //cheating :3
            Arrow.Position = BackCircle.Position;

            InnerCircle.Position = new Vector2(
                Frame.FramePosition.X + (Frame.Width / 2.0f),
                Frame.FramePosition.Y + (Frame.Height / 2.0f));

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
            if (InputManager.LeftTriggerPressed(Player))
            {
                Mode = KeyboardMode.Symbols;
                Triggers[(int)TriggerName.Left].SetState(State.pressed);
            }
            else if (InputManager.RightTriggerPressed(Player))
            {
                Mode = KeyboardMode.Caps;
                Triggers[(int)TriggerName.Right].SetState(State.pressed);
            }

            double rotation = InputManager.GetLeftPolar(Player).getAngle();
            double roundedIndexValue = Math.Round(rotation / magicNumber); //very important number.

            Arrow.Rotation = (float)rotation;

            //Adjustment for an edge case;
            if (roundedIndexValue >= 26)
            {
                roundedIndexValue -= 26;
            }
            
            Key CurrentKey = null;

            if (roundedIndexValue < 26)
                CurrentKey = Keys[(int)roundedIndexValue];
            else//Should never reach here //Whenever the users curser is not over a key its "here"
            {
                //yes, technically it should never reach here by the definition of integers. But I'm actually cheating the arctan function
                //and the definition of NAN in order to indicate when the stick is not over a key
                //could it be handled better with states or something? yes. But I like to think I'm clever in this ;).


                InnerCircleShowing = true;
            }

            if (CurrentKey != null)
            {

                if (CurrentKey.State != KeyState.Pressed)
                    CurrentKey.State = KeyState.Selected;


                //addition of letters
                //bleh you use a different way of blending colors Seph.
                //Its ok. :3
                if (InputManager.APressed(Player))
                {
                    InputManager.SetRumble(Player, 100.0f, 0.0f);

                    //add the pressed key to our typed messege
                    WhatTyped = WhatTyped.Insert(TextIndex,CurrentKey.GetText(Mode));
                    TextIndex++;

                    //press the key, to kick start the key press "animation"
                    CurrentKey.Press();
                    ResetCursorFlashTimer();
                }
            }

            if(InputManager.APressed(Player))
                    Triggers[(int)TriggerName.A].SetState(State.pressed);
            if (InputManager.AReleased(Player))
                Triggers[(int)TriggerName.A].SetState(State.notpressed);

            //and subtraction
            if (InputManager.XPressed(Player))
            {
                InputManager.SetRumble(Player, 100.0f, 0.0f);

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
            if (InputManager.XReleased(Player))
                Triggers[(int)TriggerName.X].SetState(State.notpressed);

            //Y adds a space
            if (InputManager.YPressed(Player))
            {
                ResetCursorFlashTimer();
                WhatTyped = WhatTyped.Insert(TextIndex, " ");
                TextIndex++;

                Triggers[(int)TriggerName.Y].SetState(State.pressed);
            }
            if (InputManager.YReleased(Player))
                Triggers[(int)TriggerName.Y].SetState(State.notpressed);

            if (InputManager.DPadLeftPressed(Player) || InputManager.LBPressed(Player))
            {
                ResetCursorFlashTimer();
                TextIndex--;
                if (TextIndex < 0)
                    TextIndex = 0;
            }

            if (InputManager.DPadRightPressed(Player) || InputManager.RBPressed(Player))
            {
                ResetCursorFlashTimer();
                TextIndex++;
                if (TextIndex > WhatTyped.Length)
                    TextIndex = WhatTyped.Length;
            }


            if (InputManager.StartPressed(Player))
            {
                Leave();
            }
            if (InputManager.BPressed(Player))
            {
                //When canceling text should be reset
                TextIndex = 0;
                WhatTyped = "";
                Leave();
            }
            if (InputManager.BackPressed(Player))
            {
                ShowHelp = !ShowHelp;
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
                TextIndex = 0;
                StartIndex = 0;
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

        public String GetText()
        {
            return WhatTyped;
        }

        protected override void Draw(SpriteBatch spriteBatch)
        {
            //remember kids, first thing drawn is furthest away from you!
            Fade.Color = new Color(FadeHelper(FadeColor.R), FadeHelper(FadeColor.G), FadeHelper(FadeColor.B), FadeHelper(255));
            Fade.Draw(spriteBatch);
            Frame.Draw(spriteBatch);
            BackCircle.Draw(spriteBatch);

            if (InnerCircleShowing)
                InnerCircle.Draw(spriteBatch);

            DrawKeys(spriteBatch);
            DrawTriggers(spriteBatch);
            DrawText(spriteBatch);     

            //must be AFTER drawtext
            DrawArrows(spriteBatch);
        }

        private void DrawArrows(SpriteBatch spriteBatch)
        {
            Arrow.Draw(spriteBatch);

            if (StartIndex > 0)
            {
                extraLet2.Draw(spriteBatch);
            }

            if (overFlow)
            {
                extraLet.Draw(spriteBatch);
            }
        }

        private void DrawTriggers(SpriteBatch spriteBatch)
        {
            if (ShowHelp)
            {
                for (int i = 0; i < NumberOfTriggers; i++)
                    Triggers[i].Draw(spriteBatch, LabelFont);
            }

        }

        private void Correct()
        {
            if (StartIndex > TextIndex)
            {
                StartIndex--;
                Correct();
            }

            float CursorPoint = InputFont.MeasureString(WhatTyped.Substring(StartIndex, TextIndex - StartIndex)).X;
            if (CursorPoint > TextBoxSize)
            {
                StartIndex++;
                Correct();
            }
            
        }

        private void DrawText(SpriteBatch spriteBatch)
        {
            if (Password)
                TextBox.Color = Color.LightGray; //do you hate it when I shortcut this? -> Kind of X_X
            else
                TextBox.Color = Color.White;

            TextBox.Draw(spriteBatch);

            Correct();

            float CursorPoint = 0.0f;//InputFont.MeasureString(WhatTyped.Substring(StartIndex, TextIndex - StartIndex)).X;
           
            String DisplayText = WhatTyped.Substring(StartIndex);
            int DisplayLength = DisplayText.Length;

            string replacment = "";
            for (int i = 0; i < WhatTyped.Length; i++)
            {
                if (i == TextIndex - 1)
                    replacment += WhatTyped[TextIndex - 1];
                else
                    replacment += "*";
            }

            if (Password)
            {
                    DisplayText = replacment.Substring(StartIndex);
            }

            overFlow = false;
            while (InputFont.MeasureString(DisplayText).X > TextBoxSize)
            {
                overFlow = true;
                DisplayLength--;
                DisplayText = WhatTyped.Substring(StartIndex, DisplayLength);
                if (Password)
                    DisplayText = replacment.Substring(StartIndex, DisplayLength);
            }

            CursorPoint = InputFont.MeasureString(DisplayText.Substring(0, TextIndex - StartIndex)).X;

            spriteBatch.DrawString(InputFont, DisplayText, Frame.FramePosition + TextCursorOffset + TextOffset, InputTextColor);

            if (!CursorInFlash)
            {
                Cursor.Position = Frame.FramePosition + TextCursorOffset + new Vector2(CursorPoint, 0);
                Cursor.Draw(spriteBatch);
            }
        }

        private void DrawKeys(SpriteBatch spriteBatch)
        {
            bool odd = false;

            foreach (Key aKey in Keys)
            {
                odd = !odd;

                Vector2 Position = new Vector2(Frame.FramePosition.X + (Frame.Width / 2.0f), Frame.FramePosition.Y + (Frame.Height / 2.0f));
                aKey.Draw(Mode, Position, spriteBatch, odd);
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
