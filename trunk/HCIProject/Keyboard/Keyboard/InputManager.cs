using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Keyboard
{
    public class polar
    {
        public static Vector2 PolarToXY(polar input)
        {
            //I've lost points on tests for using the name "returnable"
            //yes its a stupid thing to do.
            //but its a habit I can't break for some reason.
            Vector2 returnable = new Vector2();

            returnable.X = (float)(input.getRadius() * Math.Cos(input.getAngle()));
            returnable.Y = (float)(input.getRadius() * Math.Sin(input.getAngle()));

            return returnable;
        }

        private double radius; //this is the length
        private double angle; //this is the radian

        public double getRadius()
        {
            return radius;
        }

        public void setRadius(double rad)
        {
            radius = rad;
        }

        public double getAngle()
        {
            return angle;
        }

        public void setAngle(double ang)
        {
            if (ang >= 0 && ang < (Math.PI * 2.0D))
            {
                angle = ang;
                return;
            }

            //"tricky" math to make sure our angle is always between 0 and 2pi.
            double multiplicity = ang / (Math.PI * 2.0D);
            multiplicity = multiplicity - Math.Floor(multiplicity);

            angle = multiplicity * (Math.PI * 2.0D);
        }

        public double getDegree()
        {
            return angle * (180.0D / Math.PI);
        }

        public void setDegree(double deg)
        {
            setAngle(deg * (Math.PI / 180.0D));
        }

        public polar()
        {
            radius = 0.0D;
            angle = 0.0D;
        }

        public polar(float rad, float ang)
        {
            setRadius(rad);
            setAngle(ang);
        }
    }

    public static class InputManager
    {
        public const float stickThreshold = .7f;
        public const float triggerThreshold = .7f;

        public static Input p1CurrentInput = new Input(PlayerIndex.One);
        public static Input p1PreviousInput = new Input(PlayerIndex.One);

        public static Input p2CurrentInput = new Input(PlayerIndex.Two);
        public static Input p2PreviousInput = new Input(PlayerIndex.Two);

        public static Input p3CurrentInput = new Input(PlayerIndex.Three);
        public static Input p3PreviousInput = new Input(PlayerIndex.Three);

        public static Input p4CurrentInput = new Input(PlayerIndex.Four);
        public static Input p4PreviousInput = new Input(PlayerIndex.Four);


        public static Input GetCurrentInput(PlayerIndex playerGiven)
        {
            switch (playerGiven)
            {
                case PlayerIndex.One:
                    return p1CurrentInput;
                case PlayerIndex.Two:
                    return p2CurrentInput;
                case PlayerIndex.Three:
                    return p3CurrentInput;
                case PlayerIndex.Four:
                    return p4CurrentInput;
                default:
                    return new Input(); 

            }
        }
        public static Input GetPreviousInput(PlayerIndex playerGiven)
        {
            switch (playerGiven)
            {
                case PlayerIndex.One:
                    return p1PreviousInput;
                case PlayerIndex.Two:
                    return p2PreviousInput;
                case PlayerIndex.Three:
                    return p3PreviousInput;
                case PlayerIndex.Four:
                    return p4PreviousInput;
                default:
                    return new Input();

            }
        }


        public static void AllUpdateInput()
        {
            UpdateInput(PlayerIndex.One);
            UpdateInput(PlayerIndex.Two);
            UpdateInput(PlayerIndex.Three);
            UpdateInput(PlayerIndex.Four);
        }
        public static void UpdateInput(PlayerIndex playerGiven)
        {
            switch (playerGiven)
            {
                case PlayerIndex.One:
                    p1PreviousInput = p1CurrentInput.Copy();
                    p1CurrentInput.Update();
                    break;
                case PlayerIndex.Two:
                    p2PreviousInput = p2CurrentInput.Copy();
                    p2CurrentInput.Update();
                    break;
                case PlayerIndex.Three:
                    p3PreviousInput = p3CurrentInput.Copy();
                    p3CurrentInput.Update();
                    break;
                case PlayerIndex.Four:
                    p4PreviousInput = p4CurrentInput.Copy();
                    p4CurrentInput.Update();
                    break;
                default:
                    break;

            }


        }

        //I work better in polar >.>
        public static polar GetLeftPolar(PlayerIndex player)
        {
            polar myRad = new polar();

            myRad.setRadius(Math.Sqrt(GetLeftStickX(player) * GetLeftStickX(player) +
                GetLeftStickY(player) * GetLeftStickY(player)));

            double tempangle = Math.Atan(-GetLeftStickY(player) / GetLeftStickX(player)) + (Math.PI / 2.0D);

            if(GetLeftStickX(player) < 0.0f)
                tempangle += Math.PI;
            myRad.setAngle(tempangle);

            return myRad;
        }

        public static float GetLeftStickX(PlayerIndex player)
        {
            return (GetCurrentInput(player).lx);
        }
        public static float GetLeftStickY(PlayerIndex player)
        {
            return (GetCurrentInput(player).ly);
        }

        public static float GetRightStickX(PlayerIndex player)
        {
            return (GetCurrentInput(player).rx);
        }
        public static float GetRightStickY(PlayerIndex player)
        {
            return (GetCurrentInput(player).ry);
        }

        public static float GetLeftTrigger(PlayerIndex player)
        {
            return GetCurrentInput(player).lt;
        }
        public static float GetRightTrigger(PlayerIndex player)
        {
            return GetCurrentInput(player).rt;
        }


        public static bool APressed(PlayerIndex player)
        {
            return (GetCurrentInput(player).a &&!GetPreviousInput(player).a);
        }
        public static bool AReleased(PlayerIndex player)
        {
            return (GetPreviousInput(player).a && !GetCurrentInput(player).a);
        }
        public static bool BPressed(PlayerIndex player)
        {
            return (GetCurrentInput(player).b && !GetPreviousInput(player).b);
        }
        public static bool BReleased(PlayerIndex player)
        {
            return (GetPreviousInput(player).b && !GetCurrentInput(player).b);
        }
        public static bool XPressed(PlayerIndex player)
        {
            return (GetCurrentInput(player).x && !GetPreviousInput(player).x);
        }
        public static bool XReleased(PlayerIndex player)
        {
            return (GetPreviousInput(player).x && !GetCurrentInput(player).x);
        }
        public static bool YPressed(PlayerIndex player)
        {
            return (GetCurrentInput(player).y && !GetPreviousInput(player).y);
        }
        public static bool YReleased(PlayerIndex player)
        {
            return (GetPreviousInput(player).y && !GetCurrentInput(player).y);
        }

        public static bool LBPressed(PlayerIndex player)
        {
            return (GetCurrentInput(player).lb && !GetPreviousInput(player).lb);
        }
        public static bool RBPressed(PlayerIndex player)
        {
            return (GetCurrentInput(player).rb && !GetPreviousInput(player).rb);
        }

        public static bool StartPressed(PlayerIndex player)
        {
            return (GetCurrentInput(player).start && !GetPreviousInput(player).start);
        }
        public static bool BackPressed(PlayerIndex player)
        {
            return (GetCurrentInput(player).back && !GetPreviousInput(player).back);
        }

        public static bool LeftTriggerPressed(PlayerIndex player)
        {
            return (GetCurrentInput(player).lt >= triggerThreshold && GetPreviousInput(player).lt < triggerThreshold);
        }
        public static bool RightTriggerPressed(PlayerIndex player)
        {
            return (GetCurrentInput(player).rt >= triggerThreshold && GetPreviousInput(player).rt < triggerThreshold);
        }

        public static bool DPadLeftPressed(PlayerIndex player)
        {
            return (GetCurrentInput(player).dleft && !GetPreviousInput(player).dleft);
        }
        public static bool LeftAnalogPressed(PlayerIndex player)
        {
            return (GetCurrentInput(player).lx <= -stickThreshold && GetPreviousInput(player).lx > -stickThreshold);
        }
        public static bool LeftPressed(PlayerIndex player)
        {
            return DPadLeftPressed(player) || LeftAnalogPressed(player);
        }


        public static bool DPadRightPressed(PlayerIndex player)
        {
            return (GetCurrentInput(player).dright && !GetPreviousInput(player).dright);
        }
        public static bool RightAnalogPressed(PlayerIndex player)
        {
            return (GetCurrentInput(player).lx >= stickThreshold && GetPreviousInput(player).lx < stickThreshold);
        }
        public static bool RightPressed(PlayerIndex player)
        {
            return DPadRightPressed(player) || RightAnalogPressed(player);
        }


        public static bool DPadUpPressed(PlayerIndex player)
        {
            return (GetCurrentInput(player).dup && !GetPreviousInput(player).dup);
        }
        public static bool UpAnalogPressed(PlayerIndex player)
        {
            return (GetCurrentInput(player).ly >= stickThreshold && GetPreviousInput(player).ly < stickThreshold);
        }
        public static bool UpPressed(PlayerIndex player)
        {
            return DPadUpPressed(player) || UpAnalogPressed(player);
        }


        public static bool DPadDownPressed(PlayerIndex player)
        {
            return (GetCurrentInput(player).ddown && !GetPreviousInput(player).ddown);
        }
        public static bool DownAnalogPressed(PlayerIndex player)
        {
            return (GetCurrentInput(player).ly <= -stickThreshold && GetPreviousInput(player).ly > -stickThreshold);
        }
        public static bool DownPressed(PlayerIndex player)
        {
            return DPadDownPressed(player) || DownAnalogPressed(player);
        }

        public static bool LStickClickedIn(PlayerIndex player)
        {
            return (GetCurrentInput(player).lin && !GetPreviousInput(player).lin);
        }
        public static bool RStickClickedIn(PlayerIndex player)
        {
            return (GetCurrentInput(player).rin && !GetPreviousInput(player).rin);
        }

        //what are the min and max values? is it just 0 and 100?
        public static void SetRumble(PlayerIndex player,float left, float right)
        {
            GamePad.SetVibration(player, left, right);
        }

    }
}
