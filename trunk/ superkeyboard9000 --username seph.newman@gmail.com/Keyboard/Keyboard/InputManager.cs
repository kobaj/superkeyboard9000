using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Keyboard
{
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
        public static bool BPressed(PlayerIndex player)
        {
            return (GetCurrentInput(player).b && !GetPreviousInput(player).b);
        }
        public static bool XPressed(PlayerIndex player)
        {
            return (GetCurrentInput(player).x && !GetPreviousInput(player).x);
        }
        public static bool YPressed(PlayerIndex player)
        {
            return (GetCurrentInput(player).y && !GetPreviousInput(player).y);
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


        public static void SetRumble(PlayerIndex player,float left, float right)
        {
            GamePad.SetVibration(player, left, right);
        }

    }
}
