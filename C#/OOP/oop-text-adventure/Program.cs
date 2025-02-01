// Prompt user for name
Console.WriteLine("Hello, what's your name?");

// Get user input
var name = Console.ReadLine();

// If the user dosen't return anything,
if (name == String.Empty)
{
    // set the name to "No Name"
    name = "No Name";
}

// Respond to the user
Console.WriteLine("Welcome {0} to your OOP adventure!", name);
