using System.Text.Json;
using System.Text.Json.Nodes;

public static class JSON
{
    public static MusicPlatform? LoadFromJson(string fileName)
    {
        try
        {
            if (!File.Exists(fileName))
            {
                Console.WriteLine("File does not exist!");
                return null;
            }

            string jsonString = File.ReadAllText(fileName);
            return JsonSerializer.Deserialize<MusicPlatform>(jsonString);
        }
        catch (JsonException ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public static void AnalyzeWithJsonDocument(string fileName)
    {
        string jsonString = File.ReadAllText(fileName);
        using JsonDocument doc = JsonDocument.Parse(jsonString);
        JsonElement root = doc.RootElement;

        Console.WriteLine("Analyzing with JsonDocument:");

        if (root.TryGetProperty("Artists", out JsonElement artists))
        {
            foreach (JsonElement artist in artists.EnumerateArray())
            {
                string? name = artist.GetProperty("Name").GetString();
                Console.WriteLine($"-> Artist found in DOM: {name}");
                foreach (JsonElement album in artist.GetProperty("Albums").EnumerateArray())
                {
                    string? title = album.GetProperty("Title").GetString();
                    Console.WriteLine($"\t-> Album found in DOM: {title}");
                    foreach (JsonElement song in album.GetProperty("Songs").EnumerateArray())
                    {
                        string? songTitle = song.GetProperty("Title").GetString();
                        Console.WriteLine(
                            $"\t\t-> Song found in DOM: {songTitle} with rating {song.GetProperty("Rating").GetDouble()}"
                        );
                    }
                }
            }
        }

        if (root.TryGetProperty("Users", out JsonElement users))
        {
            foreach (JsonElement user in users.EnumerateArray())
            {
                Console.WriteLine(
                    $"-> User found in DOM: {user.GetProperty("Username").GetString()}"
                );
                foreach (JsonElement song in user.GetProperty("History").EnumerateArray())
                {
                    Console.WriteLine($"\t-> Song found in DOM: {song.GetString()}");
                }
            }
        }
    }

    public static void UpdateWithJsonNode(string fileName)
    {
        string jsonString = File.ReadAllText(fileName);
        JsonNode? rootNode = JsonNode.Parse(jsonString);
        rootNode?["Artists"]?[0]?["Name"] = "Nirvana (Legendary Edition)";

        if (rootNode?["Users"]?[0]?["History"] is JsonArray history)
        {
            history.Add("Smells Like Teen Spirit - Live");
        }

        File.WriteAllText(
            fileName,
            rootNode?.ToJsonString(new JsonSerializerOptions { WriteIndented = true })
        );
        Console.WriteLine("Updated data.json with JsonNode.");
    }

    public static void LoadDataJson(string filename)
    {
        var platform = new MusicPlatform();
        // Nirvana
        var nirvana = new Artist { Name = "Nirvana" };

        var nevermind = new Album { Title = "Nevermind", Year = 1991 };
        nevermind.Songs.AddRange([
            new Song
            {
                Title = "Smells Like Teen Spirit",
                Duration = 301,
                Plays = 600000,
                Rating = 5.0,
            },
            new Song
            {
                Title = "Come As You Are",
                Duration = 219,
                Plays = 450000,
                Rating = 4.8,
            },
            new Song
            {
                Title = "Lithium",
                Duration = 257,
                Plays = 350000,
                Rating = 4.7,
            },
            new Song
            {
                Title = "Polly",
                Duration = 177,
                Plays = 150000,
                Rating = 4.2,
            },
        ]);
        nevermind.ListenCount = nevermind.Songs.Sum(s => (long)s.Plays);

        var inUtero = new Album { Title = "In Utero", Year = 1993 };
        inUtero.Songs.AddRange([
            new Song
            {
                Title = "Heart-Shaped Box",
                Duration = 281,
                Plays = 400000,
                Rating = 4.9,
            },
            new Song
            {
                Title = "All Apologies",
                Duration = 231,
                Plays = 300000,
                Rating = 4.6,
            },
            new Song
            {
                Title = "Rape Me",
                Duration = 170,
                Plays = 250000,
                Rating = 4.4,
            },
        ]);
        inUtero.ListenCount = inUtero.Songs.Sum(s => (long)s.Plays);

        nirvana.Albums.Add(nevermind);
        nirvana.Albums.Add(inUtero);

        // Linkin Park
        var lp = new Artist { Name = "Linkin Park" };
        var hybridTheory = new Album { Title = "Hybrid Theory", Year = 2000 };
        hybridTheory.Songs.AddRange([
            new Song
            {
                Title = "In the End",
                Duration = 216,
                Plays = 1200000,
                Rating = 5.0,
            },
            new Song
            {
                Title = "Crawling",
                Duration = 209,
                Plays = 800000,
                Rating = 4.9,
            },
            new Song
            {
                Title = "One Step Closer",
                Duration = 157,
                Plays = 500000,
                Rating = 4.7,
            },
            new Song
            {
                Title = "Papercut",
                Duration = 184,
                Plays = 400000,
                Rating = 4.6,
            },
        ]);
        hybridTheory.ListenCount = hybridTheory.Songs.Sum(s => (long)s.Plays);
        lp.Albums.Add(hybridTheory);

        platform.Artists.Add(nirvana);
        platform.Artists.Add(lp);

        platform.Users.Add(
            new User { Username = "Meloman99", History = ["Smells Like Teen Spirit", "In the End"] }
        );
        platform.Users.Add(new User { Username = "RockFan", History = ["Lithium", "Crawling"] });

        var options = new JsonSerializerOptions { WriteIndented = true };

        string jsonString = JsonSerializer.Serialize(platform, options);
        File.WriteAllText(filename, jsonString);

        Console.WriteLine($"JSON data successfully adapted and saved to {filename}");
    }
}
