namespace OOPAdventure;

public static class Text
{
    private static Language _language;

    // Constructor that forces only a single
    // instance of Language to be created
    public static Language Language
    {
        get
        {
            // Check that no other instance of
            // the Language has been loaded yet
            if (_language == null)
            {
                throw new Exception("No language loaded yet.");
            }

            // If not, return the reference
            // to that instance of Language
            return _language;
        }
    }


    // Instantiate the language through a new public method
    // that registers a single instance of the Language class

    public static void LoadLanguage(Language language) {
        _language = language;
    }


}