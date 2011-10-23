using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Keyboard
{

    public class Input
    {
        PlayerIndex player;


        public float lx;
        public float ly;
        public float rx;
        public float ry;

        public float rt;
        public float lt;

        public bool a;
        public bool b;
        public bool x;
        public bool y;

        public bool rb;
        public bool lb;

        public bool start;
        public bool back;

        public bool dup;
        public bool dright;
        public bool ddown;
        public bool dleft;

        public Input()
        {
            a = false;
            b = false;
            x = false;
            y = false;

            rb = false;
            lb = false;

            start = false;
            back = false;

            dup = false;
            dright = false;
            ddown = false;
            dleft = false;

            lx = 0;
            ly = 0;

            rx = 0;
            ry = 0;

            rt = 0;
            lt = 0;
        }

        public Input(PlayerIndex playerGiven)
        {
            player = playerGiven;

            a = false;
            b = false;
            x = false;
            y = false;

            rb = false;
            lb = false;

            start = false;
            back = false;

            dup = false;
            dright = false;
            ddown = false;
            dleft = false;

            lx = 0;
            ly = 0;

            rx = 0;
            ry = 0;

            rt = 0;
            lt = 0;
        }

        public Input Copy()
        {
            Input newInput = new Input(player);
            newInput.a = a;
            newInput.b = b;
            newInput.x = x;
            newInput.y = y;
            newInput.start = start;
            newInput.back = back;
            newInput.lb = lb;
            newInput.rb = rb;
            newInput.lx = lx;
            newInput.ly = ly;
            newInput.rx = rx;
            newInput.ry = ry;
            newInput.ddown = ddown;
            newInput.dup = dup;
            newInput.dleft = dleft;
            newInput.dright = dright;

            return newInput;
        }

        public void Update()
        {
            GamePadState currentState = GamePad.GetState(player);

            if (currentState.Buttons.A == ButtonState.Pressed)
                a = true;
            else
                a = false;

            if (currentState.Buttons.B == ButtonState.Pressed)
                b = true;
            else
                b = false;

            if (currentState.Buttons.X == ButtonState.Pressed)
                x = true;
            else
                x = false;

            if (currentState.Buttons.Y == ButtonState.Pressed)
                y = true;
            else
                y = false;

            if (currentState.Buttons.LeftShoulder == ButtonState.Pressed)
                lb = true;
            else
                lb = false;

            if (currentState.Buttons.RightShoulder == ButtonState.Pressed)
                rb = true;
            else
                rb = false;

            if (currentState.DPad.Up == ButtonState.Pressed)
                dup = true;
            else
                dup = false;

            if (currentState.DPad.Left == ButtonState.Pressed)
                dleft = true;
            else
                dleft = false;

            if (currentState.DPad.Right == ButtonState.Pressed)
                dright = true;
            else
                dright = false;

            if (currentState.DPad.Down == ButtonState.Pressed)
                ddown = true;
            else
                ddown = false;

            if (currentState.Buttons.Start == ButtonState.Pressed)
                start = true;
            else
                start = false;

            if (currentState.Buttons.Back == ButtonState.Pressed)
                back = true;
            else
                back = false;

            lx = currentState.ThumbSticks.Left.X;
            ly = currentState.ThumbSticks.Left.Y;

            rx = currentState.ThumbSticks.Right.X;
            ry = currentState.ThumbSticks.Right.Y;


        }




    }
}
