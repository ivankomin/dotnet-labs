public class Song
{
    public required string Title { get; set; }
    public int Duration { get; set; }
    public int Plays { get; set; }
    public double Rating { get; set; }
}

public class Album
{
    public required string Title { get; set; }
    public int Year { get; set; }
    public long ListenCount { get; set; }
    public List<Song> Songs { get; set; } = [];
}

public class Artist
{
    public required string Name { get; set; }
    public List<Album> Albums { get; set; } = [];
}

public class User
{
    public required string Username { get; set; }
    public List<string> History { get; set; } = [];
}

public class MusicPlatform
{
    public List<Artist> Artists { get; set; } = [];
    public List<User> Users { get; set; } = [];
}
