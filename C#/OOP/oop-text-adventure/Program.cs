// Include namespace
using OOPAdventure;

// Create instance of English class
var language = new English();

// Prompt user for name
Console.WriteLine(language.ChooseYourName);

// Get user input
var name = Console.ReadLine();

// If the user dosen't return anything,
if (name == String.Empty)
{
    // set the name to "No Name"
    name = "No Name";
}

// Create an instance of the Player class
var player = new Player(name);

// Respond to the user
Console.WriteLine("Welcome {0} to your OOP adventure!", player.Name);
