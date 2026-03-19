public class Artist(int id, string name)
{
    public int Id { get; set; } = id;
    public string Name { get; set; } = name;

    public override string ToString() => $"Artist: {Name} (ID: {Id})";
}

public class Song(int id, int albumId, int genreId, int duration, double rating, string title)
{
    public int Id { get; set; } = id;
    public int AlbumId { get; set; } = albumId;
    public int GenreId { get; set; } = genreId;
    public int Duration { get; set; } = duration;
    public double Rating { get; set; } = rating;
    public string Title { get; set; } = title;

    public override string ToString() => $"(ID: {Id}) {Title} - [{Duration}s] Rating: {Rating}";
}

public class Genre(int id, string name)
{
    public int Id { get; set; } = id;
    public string Name { get; set; } = name;

    public override string ToString() => Name;
}

public class Album(int id, string title, int artistId, int year)
{
    public int Id { get; set; } = id;
    public string Title { get; set; } = title;
    public int ArtistId { get; set; } = artistId;
    public int Year { get; set; } = year;

    public override string ToString() => $"Album: '{Title}' ({Year})";
}

public class User(int id, string name, int currentGenreId)
{
    public int Id { get; set; } = id;
    public string Name { get; set; } = name;
    public int CurrentGenreId { get; set; } = currentGenreId;

    public override string ToString() => $"User: {Name} (Genre ID: {CurrentGenreId})";
}

public class UserHistory(int userId, int songId, DateTime listenDate)
{
    public int UserId { get; set; } = userId;
    public int SongId { get; set; } = songId;
    public DateTime ListenDate { get; set; } = listenDate;
}
