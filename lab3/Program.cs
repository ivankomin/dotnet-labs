class Program
{
    static void Main()
    {
        // JSON.LoadDataJson("data.json");
        // MusicPlatform? platform = JSON.LoadFromJson("data.json");
        // TestLoadFromJson(platform);

        // JSON.AnalyzeWithJsonDocument("data.json");
        JSON.UpdateWithJsonNode("data.json");
    }

    static void TestLoadFromJson(MusicPlatform? platform)
    {
        if (platform != null)
        {
            Console.WriteLine($"Successfully loaded {platform.Artists.Count} artists.");
            Console.WriteLine($"Successfully loaded {platform.Users.Count} users.");

            var firstArtist = platform.Artists.FirstOrDefault();
            if (firstArtist != null)
            {
                Console.WriteLine($"First artist: {firstArtist.Name}");
                Console.WriteLine($"Their album count: {firstArtist.Albums.Count}");
            }
        }
        else
        {
            Console.WriteLine("Failed to load data (platform is null).");
        }
    }
}
