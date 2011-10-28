using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Keyboard
{
    public enum KeyboardStatus { Entering, Leaving, Active, Inactive }

    public class Keyboard : RenderArea
    {
        private readonly double magicNumber = Math.PI / 13.0D; // 0.24166096923076923076923076923077D; //2pi / 26;

        SpriteFont CourierNew;
        KeyboardStatus CurrentStatus;

        Sprite BackCircle;
        Sprite Key;
        String WhatTyped = "";
        //hand type this bitch (no seriously, this is how we make custom different versions, hand type).
        String[] Alphabet = {"a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z"}; 

        Color FadeColor;
        Sprite Fade;
        float FadeAlpha;
        float FadeAlphaMax;
        float FadeIncrement;

        KeyboardFrame Frame;
        int FrameOnScreenY;
        int FrameOffScreenY;
        int FrameVelocityY;

        public bool IsActive { get { return CurrentStatus != KeyboardStatus.Inactive; } }

        public Keyboard(KeyboardTextures textures, KeyboardColors colors, SpriteFont font)
        {
            //If we get a chance, we should modify this to work with other resolutions
            InitializeRenderArea(0, 0, 1280, 720);

            //setup font
            CourierNew = font;

            //setup our lovely keys.
            BackCircle = new Sprite(textures.BackCircle, new Vector2(0, 0));
            Key = new Sprite(textures.Key, new Vector2(0, 0));
            BackCircle.Origin = new Vector2(BackCircle.Width / 2.0f, BackCircle.Height / 2.0f);
            Key.Origin = new Vector2(Key.Width / 2.0f, Key.Height / 2.0f);

            Fade = new Sprite(textures.Fade, new Vector2(0, 0));
            FadeColor = colors.Fade;
            FadeAlpha = 0;
            FadeAlphaMax = 180;
            FadeIncrement = 15;

            FrameOnScreenY = 50;
            FrameOffScreenY = 720;
            FrameVelocityY = -5;

            Frame = new KeyboardFrame(200, FrameOffScreenY, textures.Frame, colors.Frame); 

            CurrentStatus = KeyboardStatus.Inactive;
        }



        public void Enter()
        {
            CurrentStatus = KeyboardStatus.Entering;
        }

        public void Leave()
        {
            CurrentStatus = KeyboardStatus.Leaving;
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
                    break;
            }

            BackCircle.Position.X = Frame.FramePosition.X + (Frame.Width / 2.0f);
            BackCircle.Position.Y = Frame.FramePosition.Y + (Frame.Height / 2.0f);
            Key.Position = BackCircle.Position;

            CalculateKey(true);

            Frame.Update(gameTime);
        }

        private void CalculateKey(bool snaps)
        {
            double rotation = InputManager.GetLeftPolar(PlayerIndex.One).getAngle();
            double roundedIndexValue = Math.Round(rotation / magicNumber); //very important number.


            if (!snaps)
            {
                Key.Rotation = (float)InputManager.GetLeftPolar(PlayerIndex.One).getAngle();
            }
            else
            {
                Key.Rotation = (float)(roundedIndexValue * magicNumber);
            }

            //blank slate
            Key.Color = new Color(130, 130, 200, 130);
            InputManager.SetRumble(PlayerIndex.One, 0.0f, 0.0f);

            //addition of letters
            //bleh you use a different way of blending colors Seph.
            //Its ok. :3
            if (InputManager.APressed(PlayerIndex.One))
            {
                InputManager.SetRumble(PlayerIndex.One, 100.0f, 0.0f);
                Key.Color = new Color(130, 200, 130, 130);

                //add the pressed key to our typed messege
                if(roundedIndexValue < 26)
                WhatTyped += Alphabet[(int)roundedIndexValue];
            }

            //and subtraction
            if (InputManager.YPressed(PlayerIndex.One))
            {
                InputManager.SetRumble(PlayerIndex.One, 100.0f, 0.0f);
                Key.Color = new Color(200, 130, 130, 130);

                //remove a key to our typed messege
                WhatTyped = WhatTyped.Remove(WhatTyped.Length - 1, 1);
            }
        }

        public void LeaveUpdate(GameTime gameTime)
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
        }

        public void EnterUpdate(GameTime gameTime)
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
        }

        protected override void Draw(SpriteBatch spriteBatch)
        {
            //remember kids, first thing drawn is furthest away from you!
            Fade.Color = new Color(FadeHelper(FadeColor.R), FadeHelper(FadeColor.G), FadeHelper(FadeColor.B), FadeHelper(255));
            Fade.Draw(spriteBatch);
            Frame.Draw(spriteBatch);
            BackCircle.Draw(spriteBatch);

            RenderLetters(spriteBatch);

            Key.Draw(spriteBatch);

            spriteBatch.DrawString(CourierNew, WhatTyped, new Vector2(Frame.FramePosition.X + 20, Frame.FramePosition.Y + 40), Color.White);
        }

        private void RenderLetters(SpriteBatch spriteBatch)
        {
            float radius = 160.0f;
            float initialangle = (float)(0 - (Math.PI / 2.0D)); //(magicNumber / 2.0D)

            for(int i = 0; i < 26; i++)
            {
                Vector2 location = polar.PolarToXY(new polar(radius, (float)(initialangle + (i * magicNumber))));
                location.X += Frame.FramePosition.X + (Frame.Width / 2.0f);
                location.Y += Frame.FramePosition.Y + (Frame.Height / 2.0f);

                //fine tuneing, for some reason this stuff is not right in the middle?
                location.X -= 10;
                location.Y -= 22;

                spriteBatch.DrawString(CourierNew, Alphabet[i], location, Color.Black);
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
