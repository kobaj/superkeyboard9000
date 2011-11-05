using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Keyboard
{
    public enum TriggerName {A, B, X, Y, Left, Right, Back, Start};
    public enum State{pressed, notpressed};

    public class Trigger
    {
        private Sprite image;
        private string name;
        private State state;
        private Vector2 location;
        private Vector2 stringLength;

        public Trigger(Sprite img, Vector2 orig, string title)
        {
            image = img;
            name = title;
            image.Origin = orig;
            location = new Vector2(0, 0);
            state = State.notpressed;
        }

        public void SetLocation(Vector2 local)
        {
            image.Position = local;
            location = local;
        }

        public void SetState(State currentState)
        {
            state = currentState;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont Font)
        {
            //so dunno if you'll read this. But I had another idea of showing a 'depressed' button
            //instead of simply changing the color. But I don't have your art style/skill/assets, 
            //so I'll leave that up to you?

            //>>Seems like too much work :p

            if (stringLength == Vector2.Zero)
                stringLength = Font.MeasureString(name);

            if (state == State.pressed)
                image.Color = Color.Red;
            else
                image.Color = Color.White;

                image.Draw(spriteBatch);

            spriteBatch.DrawString(Font, name, new Vector2(location.X - stringLength.X / 2.0f, location.Y - image.Origin.Y - stringLength.Y), Color.White);
        }
    }
}
