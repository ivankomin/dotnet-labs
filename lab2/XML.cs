using System.Xml;
using System.Xml.Linq;

public static class XML
{
    public static void WriteXml(string fileName)
    {
        var settings = new XmlWriterSettings { Indent = true };

        using XmlWriter writer = XmlWriter.Create(fileName, settings);

        writer.WriteStartDocument();
        writer.WriteStartElement("MusicPlatform");

        while (true)
        {
            Console.WriteLine("Enter artist name or 'exit' to quit");
            string artist = Console.ReadLine();
            if (artist?.ToLower() == "exit" || string.IsNullOrEmpty(artist))
                break;

            writer.WriteStartElement("Artist");
            writer.WriteAttributeString("Artist", artist);

            while (true)
            {
                Console.WriteLine("Enter album name or 'exit' to quit");
                string album = Console.ReadLine();
                if (album?.ToLower() == "exit" || string.IsNullOrEmpty(album))
                    break;

                Console.WriteLine("Enter the release year");
                string year = Console.ReadLine();

                Console.WriteLine("Enter total streams count");
                string listenCount = Console.ReadLine();

                writer.WriteStartElement("Album");
                writer.WriteAttributeString("Title", album);
                writer.WriteAttributeString("Year", year);
                writer.WriteAttributeString("ListenCount", listenCount);

                while (true)
                {
                    Console.WriteLine("Enter song name or 'exit' to quit");
                    string song = Console.ReadLine();
                    if (song?.ToLower() == "exit" || string.IsNullOrEmpty(song))
                        break;

                    Console.WriteLine("Enter the amount of streams");
                    string plays = Console.ReadLine();

                    Console.WriteLine("Enter the song rating");
                    string rating = Console.ReadLine();

                    writer.WriteStartElement("Song");
                    writer.WriteAttributeString("Title", song);
                    writer.WriteAttributeString("Plays", plays);
                    writer.WriteAttributeString("Rating", rating);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }
        writer.WriteEndElement();
        writer.WriteEndDocument();
    }

    public static void LoadWithXmlDocument(string fileName)
    {
        var doc = new XmlDocument();
        doc.Load(fileName);
        XmlNode root = doc.DocumentElement;

        foreach (XmlNode artist in root.ChildNodes)
        {
            string name = artist.Attributes["Name"]?.Value;
            Console.WriteLine($"Artist: {name}");

            foreach (XmlNode album in artist.SelectNodes("Album"))
            {
                string title = album.Attributes["Title"]?.Value;
                Console.WriteLine($"  Album: {title}");
            }
        }
    }

    public static void LoadWithSerializer(string fileName)
    {
        var serializer = new System.Xml.Serialization.XmlSerializer(typeof(MusicPlatformDto));
        using (FileStream fs = new FileStream(fileName, FileMode.Open))
        {
            var data = (MusicPlatformDto)serializer.Deserialize(fs);
            foreach (var artist in data.Artists)
            {
                Console.WriteLine($"Artist: {artist.Name}");
            }
        }
    }

    public static void DisplayData(string filename)
    {
        XDocument xmlDoc = XDocument.Load(filename);

        foreach (XElement artist in xmlDoc.Root.Elements("Artist"))
        {
            Console.WriteLine($"Artist: {artist.Attribute("Name")?.Value}");
            foreach (XElement album in artist.Elements("Album"))
            {
                Console.WriteLine(
                    $"  - Album: {album.Attribute("Title")?.Value} ({album.Attribute("Year")?.Value})"
                );
                foreach (XElement song in album.Elements("Song"))
                {
                    Console.WriteLine(
                        $"    > {song.Attribute("Title")?.Value} - Plays: {song.Attribute("Plays")?.Value}"
                    );
                }
            }
        }
    }

    public static void LinqQueries(string fileName)
    {
        var doc = XDocument.Load(fileName);

        //get total song count
        var allSongTitles = doc.Descendants("Song").Select(s => s.Attribute("Title")?.Value);
        Console.WriteLine($"1. Total amount of songs: {allSongTitles.Count()}");

        //get songs longer than 5 minutes
        var longSongs = doc.Descendants("Song")
            .Where(s => (int)s.Attribute("Duration") > 300)
            .Select(s => s.Attribute("Title")?.Value);
        Console.WriteLine($"2. Songs longer than 5 minutes: {string.Join(", ", longSongs)}");

        //get albums sorted by release year
        var albums = doc.Descendants("Album")
            .OrderBy(a => (int)a.Attribute("Year"))
            .Select(a => $"{a.Attribute("Title")?.Value} ({a.Attribute("Year")?.Value})");
        Console.WriteLine($"3. Albums sorted by release year: {string.Join(", ", albums)}");
    }

    public static void LoadData(string filename)
    {
        var artists = new List<Artist>
        {
            new(1, "Nirvana"),
            new(2, "Linkin Park"),
            new(3, "Pink Floyd"),
            new(4, "Unknown Band"),
        };

        var albums = new List<Album>
        {
            new(1, "Nevermind", 1, 1991, 1500000),
            new(2, "In Utero", 1, 1993, 1100000),
            new(3, "Hybrid Theory", 2, 2000, 2500000),
            new(4, "Meteora", 2, 2003, 800000),
            new(5, "The Dark Side of the Moon", 3, 1973, 3000000),
            new(6, "Demo Album", 4, 2027, 650),
        };

        var songs = new List<Song>
        {
            // Альбом 1: Nirvana - Nevermind (ID: 1, Genre: 1)
            new(1, 1, 1, "Smells Like Teen Spirit", 301, 600000, 5.0),
            new(2, 1, 1, "Come As You Are", 219, 450000, 4.8),
            new(3, 1, 1, "Lithium", 257, 350000, 4.7),
            new(4, 1, 1, "Polly", 177, 150000, 4.2),
            // Альбом 2: Nirvana - In Utero (ID: 2, Genre: 1)
            new(5, 2, 1, "Heart-Shaped Box", 281, 400000, 4.9),
            new(6, 2, 1, "All Apologies", 231, 300000, 4.6),
            new(7, 2, 1, "Rape Me", 170, 250000, 4.4),
            // Альбом 3: Linkin Park - Hybrid Theory (ID: 3, Genre: 2)
            new(8, 3, 2, "In the End", 216, 1200000, 5.0),
            new(9, 3, 2, "Crawling", 209, 800000, 4.9),
            new(10, 3, 2, "One Step Closer", 157, 500000, 4.7),
            new(11, 3, 2, "Papercut", 184, 400000, 4.6),
            // Альбом 4: Linkin Park - Meteora (ID: 4, Genre: 2)
            new(12, 4, 2, "Numb", 187, 950000, 5.0),
            new(13, 4, 2, "Somewhere I Belong", 213, 600000, 4.8),
            new(14, 4, 2, "Faint", 162, 700000, 4.9),
            // Альбом 5: Pink Floyd - Dark Side of the Moon (ID: 5, Genre: 3)
            new(15, 5, 3, "Time", 421, 900000, 4.9),
            new(16, 5, 3, "Money", 382, 850000, 4.8),
            new(17, 5, 3, "Us and Them", 462, 700000, 4.9),
            new(18, 5, 3, "Brain Damage", 226, 500000, 4.7),
            // Альбом 6: Unknown Band - Demo Album (ID: 6, Genre: 4)
            new(19, 6, 4, "Bad Track", 120, 100, 2.1),
            new(20, 6, 4, "Average Track", 180, 500, 3.5),
            new(21, 6, 4, "Experimental Noise", 600, 50, 1.5),
        };

        XmlWriterSettings settings = new XmlWriterSettings { Indent = true };

        using (XmlWriter writer = XmlWriter.Create(filename, settings))
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("MusicPlatform");

            foreach (var artist in artists)
            {
                writer.WriteStartElement("Artist");
                writer.WriteAttributeString("Name", artist.Name);

                var artistAlbums = albums.Where(a => a.ArtistId == artist.Id);
                foreach (var album in artistAlbums)
                {
                    writer.WriteStartElement("Album");
                    writer.WriteAttributeString("Title", album.Title);
                    writer.WriteAttributeString("Year", album.Year.ToString());
                    writer.WriteAttributeString("ListenCount", album.ListenCount.ToString());

                    var albumSongs = songs.Where(s => s.AlbumId == album.Id);
                    foreach (var song in albumSongs)
                    {
                        writer.WriteStartElement("Song");
                        writer.WriteAttributeString("Title", song.Title);
                        writer.WriteAttributeString("Duration", song.Duration.ToString());
                        writer.WriteAttributeString("Plays", song.Plays.ToString());
                        writer.WriteAttributeString("Rating", song.Rating.ToString());
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndDocument();
        }
    }
}
