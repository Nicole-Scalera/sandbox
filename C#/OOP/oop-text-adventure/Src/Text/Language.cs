namespace OOPAdventure;

// This file contains all the properties for all the text
// present in the game.

public abstract class Language
{
    // Create public properties that are protected
    public string Welcome { get; protected set; } = "";
    public string ChooseYourName { get; protected set; } = "";
    public string DefaultName { get; protected set; } = "";
}