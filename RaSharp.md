# Ra# - Own programming language for RadianceOS based on C#
> [!IMPORTANT]
> Ra# is still under development and is not suitable for handling larger projects at this time.
### Variables
**Supported Types:** Ra# currently supports two variable types: String and Int.<br>
**Variable Creation:** Variables can be created using the syntax similar to C#. Example:
```csharp
Int myNumber = 42;
String myText = "Hello, Ra#!";
```
**Operations on Variables**
Ra# supports operations on both strings and integers. For example:
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
### Void Functions
Ra# supports void functions, and the program always starts with `void Start()`
