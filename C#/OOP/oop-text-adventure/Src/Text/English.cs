using OOPAdventure;

// English.cs is an extension of Language.cs

public class English : Language
{
    public English()
    {
        // Set the value of the public string to the following
        Welcome = "Welcome {0} to your OOP adventure!";
        ChooseYourName = "Hello, what's your name?";
        DefaultName = "No Name";
    }
}