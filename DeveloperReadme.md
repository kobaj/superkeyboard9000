# Developer Readme #

## Requirements ##

-Microsoft XNA Game Studio 4.0  Available at: http://www.microsoft.com/download/en/details.aspx?id=23714

-Microsoft Visual Studio (or Visual C# Express)

-Wired USB Xbox 360 Controller (or wireless dongle and a wireless Xbox 360 controller)

## Optional Requirements (Required to run code on the Xbox 360) ##

-Xbox 360 game console connected to your local network

-App Hub (formerly XNA creators club) membership or trial

-Free trials are available through MSDNAA and Dreamspark

## Running the Test Suite on a PC ##

1.Make sure XNA Game Studio and a compatabile IDE are installed (I will refer to this as Visual Studio although C# Express works as well)

2.Using Visual Studio open up the TestGame.sln solution file (in TestGame folder)

3.In the Solution Explorer on the right, right-click TestGame and choose debug -> start new instance.

4.Use one or two connected Xbox 360 controllers to interact with the Test Suite.

## Running the Test Suite on a Xbox 360 ##

1.Make sure XNA is properly set up on both the PC and Xbox 360. There is plenty of documentation available online as for how to do this.

2.Using Visual Studio open up the TestGame.sln solution file (in TestGame folder)

3.In the Solution Explorer on the right, right-click TestGame and choose Create Copy of Project for Xbox 360

4.A new project will appear in the Solution Explorer titled, "Xbox 360 Copy of TestGame".

5.Make sure XNA Game Studio Connect is running on your Xbox 360.

6.Right click this new project (in Visual Studio) and choose debug -> start new instance.

7.The test suite should now be running on your Xbox 360.

## Differences between Xbox 360 and PC ##

-Due to the nature of the XNA Framework, we cannot display the default Microsoft keyboard on the PC

-The show keyboard function launches a different keyboard depending on the platform.

-As a result, launching the default keyboard on a PC makes a textbox appear which requires input from a physical keyboard.

-The functionality of Super Keyboard 9000 is exactly the same on both the Xbox 360 and PC

## Implementing Super Keyboard 9000 in your XNA Game/Application ##

1.In Visual Studio right click your program's solution and select Add -> Existing Project.

2.Navigate to and choose the Keyboard.csproj file. Root Folder -> Keyboard -> Keyboard

3.You will need to add all graphical files to your main program's content project.

In Windows Explorer, navigate to the TestGameContent folder. TestGame -> TestGame -> TestGameContent

Click and drag the Keyboard folder (inside TestGameContent) to your program's content project. (look at the TestGame project if this is confusing)

4.Add "using Keyboard;" to your using statements in the C# file where you need to launch the keyboard.

5.In your Update loop you will need to add this line "KeyboardManager.Update(gameTime);" to handle all Keyboard inputs. Feel free to use the InputManager for handling other input in your program.

6.When you need to launch Super Keyboard 900 call "KeyboardManager.ShowKeyboard(x,y);"

where X is a player index indicating the controller that has control of the keyboard and y is boolean indicating whether the keyboard should enter in password mode.

7.Elsewhere in the code, you need to check for when the keyboard is inactive "if (KeyboardManager.IsActive)"

8.If so, you can extract the text using:"KeyboardManager.GetText();"
For an example of how to do this, look at the TestGame code.

## A Few Notes on Working With the Super Keyboard 9000 ##

-We adopted something of a modular design philosiphy when designing the keyboard.

-New color schemes can easily be added by adding KeyboardColors objects. (look at the code)

-Similarly, new key layouts are easy enough to add by changing the Alphabet and Symbols arrays in the Keyboard.cs file.

-Changing graphics should be easy as well. Just change the textures which the KeyboardTextures object points to.

-Other changes will require you to be familliar with your code. We believe it written and documented fairly well, so it should be relatively intuitve to understand.