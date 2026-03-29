using System.Xml.Serialization;

// Описуємо структуру для серіалізатора
[XmlRoot("MusicPlatform")]
public class MusicPlatformDto
{
    [XmlElement("Artist")]
    public List<ArtistDto> Artists { get; set; } = [];
}

public class ArtistDto
{
    [XmlAttribute("Name")]
    public required string Name { get; set; }

    [XmlElement("Album")]
    public List<AlbumDto> Albums { get; set; } = [];
}

public class AlbumDto
{
    [XmlAttribute("Title")]
    public required string Title { get; set; }

    [XmlElement("Song")]
    public List<SongDto> Songs { get; set; } = [];
}

public class SongDto
{
    [XmlAttribute("Title")]
    public required string Title { get; set; }

    [XmlAttribute("Plays")]
    public int Plays { get; set; }

    [XmlAttribute("Rating")]
    public double Rating { get; set; }
}

public record Genre(int Id, string Name);

public record Artist(int Id, string Name);

public record Album(int Id, string Title, int ArtistId, int Year, long ListenCount);

public record Song(
    int Id,
    int AlbumId,
    int GenreId,
    string Title,
    int Duration,
    int Plays,
    double Rating
);
