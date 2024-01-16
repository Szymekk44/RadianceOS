# Ra# - Own programming language for RadianceOS based on C#
> [!CAUTION]
> The new better version of Ra# has not been finished yet. More features will be added over time!
## How to start?
Open a notepad and save your file as .ras! (currently you can only run it from the desktop, we will finish the file explorer soon ;p)
### Variables
**Supported Types:** Ra# currently supports two variable types: String and Int.<br>
**Variable Creation:** Variables can be created using the syntax similar to C#. Example:
```csharp
Int myNumber = 42;
String myText = "Hello, Ra#!";
```
**Operations on Variables**
Ra# supports operations on both strings and )integers. For example:
```csharp
myNumber += 10 * 4 / 2;  // Perform operations on integers
myText += " Concatenate strings";  // Concatenate strings
```
### Console Operations
Ra# provides console operations similar to C#:
```csharp
Console.WriteLine(myNumber.ToString());  // Display an integer
Console.WriteLine("Test " + myText);  // Concatenate and display strings
Console.ForegroundColor = ConsoleColor.Red;  // Change console text color
string userInput = Console.ReadLine();  // Get user input
```
### Window mode
**mode**
```csharp
Window.mode = 0; //console window (Default)
Window.mode = 1; //window
```
**size**<br> 
*Also works to console!*
```csharp
Window.sizeAble = 0; //removes the ability to resize your window
Window.sizeAble = 1; //restores the ability to resize your window (Default)

Window.height or Window.sizeY = 100; //Sets the window height to 100
Window.width or Window.sizeX = 100; //Sets the window width to 100

Window.minHeight or Window.minSizeY = 100; //Sets the minimum window height to 100
Window.minWidth or Window.minSizeX = 100; //Sets the minimum window width to 100
```
**position**<br> 
*Also works to console!*
```csharp
Window.moveAble = 0; //removes the ability to move your window
Window.moveAble = 1; //restores the ability to move your window (Default)

Window.posX = 100; //Sets the X position;
Window.posY = 100; //Sets the Y position;
```
### Drawing in window mode
**Text**
```csharp
Draw.text("Hello World!", "font", PosX, PosY); //Draws "Hello World"
Draw.textC("Hello World!", "font", PosY);  //Draws a centered "Hello World"
```
**Fonts**
```
"16" - zap-ext-light16
"18" - zap-ext-light18
"lt" - lat9w-16
"rs" - ruscii_8x16
```

