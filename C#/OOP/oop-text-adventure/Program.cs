// Include namespace
using OOPAdventure;

// Reference the Text class
Text.LoadLanguage(new English());

// Prompt user for name
Console.WriteLine(Text.Language.ChooseYourName);

// Get user input
var name = Console.ReadLine();

// If the user dosen't return anything,
if (name == String.Empty)
{
    // set the name to "No Name"
    name = Text.Language.DefaultName;
}

// Create an instance of the Player class
var player = new Player(name);

// Respond to the user
Console.WriteLine(Text.Language.Welcome, player.Name);
