using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Keyboard
{
    public class KeyboardFrame
    {
        public Vector2 FramePosition;
        Sprite Frame;

        

        public KeyboardFrame(int x, int y, Texture2D background, Color color)
        {
            FramePosition = new Vector2(x, y);
            Frame = new Sprite(background, FramePosition);
            Frame.Color = color;
        }

        public void Update(GameTime gameTime)
        {
            //Need to specify all components relative to FramePosition
            Frame.Position = FramePosition;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            Frame.Draw(spriteBatch);
        }

    }
}
