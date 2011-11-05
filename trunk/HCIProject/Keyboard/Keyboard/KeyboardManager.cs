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

        private static bool Active;
        private static Keyboard CurrentKeyboard;

        public static bool IsActive { get { return Active; } }
        public static void Initialize(ContentManager content)
        {
            KeyboardTextures Textures = new KeyboardTextures()
            {
                Frame = content.Load<Texture2D>("Keyboard/Graphics/Frame"),
                Fade = content.Load<Texture2D>("Keyboard/Graphics/Fade"),
                BackCircle = content.Load<Texture2D>("Keyboard/Graphics/backcircle"),
                InnerCircle = content.Load<Texture2D>("keyboard/Graphics/innercircle"),
                Key = content.Load<Texture2D>("Keyboard/Graphics/key"),
                LTrigger = content.Load<Texture2D>("Keyboard/Graphics/lTrigger"),
                RTrigger = content.Load<Texture2D>("Keyboard/Graphics/rTrigger"),
                ATrigger = content.Load<Texture2D>("Keyboard/graphics/aTrigger"),
                TextBox = content.Load<Texture2D>("Keyboard/Graphics/TextBox"),
                Cursor = content.Load<Texture2D>("Keyboard/Graphics/Cursor")
            };

            //Blue scheme
            KeyboardColors Colors = new KeyboardColors()
            {
                Frame = new Color(14, 83, 167),
                Fade = new Color(104,153,211),
                BackCircle = new Color(4, 52, 108),
                Key = new Color(66,132,211),
                CurrentKey = new Color(39,78,125),
                KeyText = Color.White,
                InputText = Color.Black,
                CurrentKeyPressed = new Color(27, 34, 114),
                TextBox = new Color(255,255,255),
                Cursor = new Color(0,0,0)
            };

            //you handle sprites in a different way than I, so I was a bit confused.
            //thus if you want to make a wraper for font like you did for sprites
            //feel free, but this is my current quick and dirty font implamentation.
            CurrentKeyboard = new Keyboard(Textures,Colors, content.Load<SpriteFont>("Keyboard/Courier New"));
            Active = false;
        }

        public static void ShowKeyboard()
        {
            CurrentKeyboard.Enter();
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
