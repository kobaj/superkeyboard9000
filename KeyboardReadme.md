# Keyboard Readme #

This readme assumes you are attempting to use Super Keyboard 9000 through either the testing suite or another game/application.

For instructions on running the testing suite or incroporating Super Keyboard 9000 into your own game or application, see the developer readme.

# Testing Suite #

## Setup ##
######  ######
-Once the testing suite is launched you will be prompted with the statement "Tester - Press A"

-Using the desired Xbox 360 controller for the Tesster, press the A button.

-You will now be prompted with the statment "Testee - Press A"

-Using the desired Xbox 360 controller (can be the same as that of the tester) for the Tesstee, press the A button.

## Tester VS Testee ##

-The tester changes settings (text, colorschemes), launches the appropriate keyboard, and navigates through test results

-The testee enters text when there is an active keyboard.

-In actual controlled tests, the Tester and Testee should each have his or her own controller.

-When the Testee is entering text, the Tester has no control.

-If both Tester and Testee are assigned to the same controller, that controller can be used to perfrom both roles.

## User Interface ##

The testing suite consits of four panels:

1.UpperLeft:
> -Indicates which controllers are mapped to the Tester and Testee roles.
> -Indicates the current color scheme.

2.UpperRight:
> -Indicates the currently selected sentence.

3.LowerLeft:
> -Indicates the controls.
> -Note: there is some hidden functionality that is described below.

4.LowerRight:
> -Output log which displays each test's results.

## Controls ##

The following are the control options which the Tester has at his or her disposal:

-Left Analog Stick and DPad: Switches the currently chosen sentence.

-Right Analog Stick: Scrolls through the output log.

-Right Analog Stick (pressed in): Resets the output log to its default position (but not the contents).

-A Button: Launches Super Keyboard 9000 which is then controlled by the Testee.

-X Button: Launches the default Microsoft keyboard which is then controlled by the Testee. (Note: if using a PC, a different keyboard appears- see developer readme for details)

-Left/Right Bumpers: Changes the current color scheme.

-Right Trigger (Hold) While pressing A or X Button: Launches either keyboard in password mode.

## Usage ##
#####  #####
1.First, bind each role to a controller.

2.Choose a sentence using the left analog stick (or dpad).

3.Choose a color scheme using the left and right bumpers.

4.Launch either Super Keyboard 9000 or the default Microsoft keyboard using A or X.

`*`.If you want to launch in password mode (where letters are obsucred with asteriks), hold the right trigger while hitting A or X.

5.The Testee controller now can enter text using the active keyboard.

6.After the Testee enters text, the output log will contain an additional entry with data from the previous keyboard usage.

7.Goto step 2 to repeat as desired.

## Output Log ##

Each line of the output log contains several pieces of data seperated by several spaces. They are as follows:

-Keyboard Used: SK9000 indicates Super Keyboard 9000. DEFAULT indicates the default Microsoft Keyboard.

-Color Scheme Used: If SK9000 was used, the color scheme used will also be displayed.

-Target Sentence: The sentence the testee was aiming for (selected by the tester in the upper right panel)

-Actual Text: What the Testee actually typed.

-Match?: "Match" and "Not a Match" indicated whether the target sentence matches with the actual text. Note: additional accuracy testing can be conducted manually.

-Time: The time taken within the keyboard (in milliseconds).

# Super Keyboard  9000 #

## Controls ##

-Left Analog Stick: Navigate the circular letter/symbol ring. Hold the direction corresponding to the desired letter/symbol.

-A Button: Add the currently selected letter/symbol to the text field (shown at the top) at the cursor position.

-Y Button: Add a spcace character to the text field at the cursor position.

-X Button: Backspace. Delete the character directly to the left of the cursor position.

-B Button: Cancel out of the keyboard discarding all typed text.

-Start Button: Accept the currently typed text. Individual programs choose how to handle the inputted text.

-Back Button: Toggles the help (control indicators) on or off.

-Left Trigger (hold): While held, the keyboard will display a ring consisting of numbers and symbols.

-Right Trigger (hold): While held, the keybhoard will display a ring consisting of capital letters.

-DPad or Left/Right Bumpers: Move the cursor position (in the textfield at the top) to the left or right.

## Usage ##
#####  #####
Use the left analog stick to select a key and hit the A button to enter the selected key. Hold down the left or right triggers to acess either symbols or capitals.

Characters are input a the blinking cursor's current position. You can use either the dpad or the left/right bumpers to move the cursor position.

If characters begin to overflow the textbox, the box will scroll accordingly.  White arrows indicate additional hidden characters either to the left or right within the textbox.

Once you have entered the desired text, hit the start button to accept it. If you want to cancel out, discarding any changes, hit the B button.