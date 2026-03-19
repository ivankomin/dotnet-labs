namespace Lab1;

public static class Program
{
    public static void Main()
    {
        var genres = new List<Genre>
        {
            new Genre(1, "Grunge"),
            new Genre(2, "Heavy Metal"),
            new Genre(3, "Thrash Metal"),
            new Genre(4, "Alternative Rock"),
            new Genre(5, "Hard Rock"),
        };

        var artists = new List<Artist>
        {
            new Artist(1, "Nirvana"),
            new Artist(2, "Alice in Chains"),
            new Artist(3, "Metallica"),
            new Artist(4, "Local H"),
        };

        var albums = new List<Album>
        {
            new Album(1, "Bleach", 1, 1989),
            new Album(2, "Dirt", 2, 1992),
            new Album(3, "Kill 'Em All", 3, 1983),
            new Album(4, "As Good As Dead", 4, 1996),
            new Album(5, "Load", 3, 1996),
        };

        var songs = new List<Song>
        {
            // Bleach (Nirvana)
            new Song(1, 1, 1, 162, 4.5, "Blew"),
            new Song(2, 1, 1, 148, 4.2, "Floyd the Barber"),
            new Song(3, 1, 1, 218, 4.8, "About a Girl"),
            new Song(4, 1, 1, 167, 4.1, "School"),
            new Song(5, 1, 1, 203, 3.5, "Love Buzz"),
            // Dirt (Alice in Chains)
            new Song(6, 2, 1, 369, 4.9, "Rain When I Die"),
            new Song(7, 2, 1, 311, 4.7, "Rooster"),
            new Song(8, 2, 1, 169, 4.5, "Them Bones"),
            //  Metallica (Kill Em All)
            new Song(9, 3, 3, 410, 4.0, "Seek & Destroy"),
            new Song(10, 3, 3, 299, 3.8, "Hit The Lights"),
            //Local H (As Good As Dead)
            new Song(11, 4, 4, 222, 4.2, "Bound To The Floor"),
            new Song(12, 4, 4, 290, 4.4, "High Fiving MF"),
            //Metallica (Load)
            new Song(13, 5, 5, 589, 4.9, "The Outlaw Torn"),
            new Song(14, 5, 5, 330, 3.8, "2x4"),
            new Song(15, 5, 2, 330, 3.8, "King Nothing"),
        };

        var users = new List<User>
        {
            new User(1, "Kris", 1), // 1 - Grunge
            new User(2, "Suz", 2), // 2 - Heavy Metal
            new User(3, "Ral", 3), // 3 - Thrash Metal
        };

        // Історія прослуховувань (Зв'язок M:M)
        var history = new List<UserHistory>
        {
            new UserHistory(1, 1, DateTime.Now.AddMonths(-2)),
            new UserHistory(1, 6, DateTime.Now.AddMonths(-1)),
            new UserHistory(1, 6, DateTime.Now.AddMonths(-1)),
            new UserHistory(1, 7, DateTime.Now.AddMonths(-1)),
            new UserHistory(1, 8, DateTime.Now.AddMonths(-1)),
            new UserHistory(1, 3, DateTime.Now.AddMonths(-1)),
            new UserHistory(1, 3, DateTime.Now.AddMonths(-1)),
            new UserHistory(1, 9, DateTime.Now.AddMonths(-1)),
            new UserHistory(1, 12, DateTime.Now.AddMonths(-1)),
            new UserHistory(1, 13, DateTime.Now.AddMonths(-1)),
            new UserHistory(1, 15, DateTime.Now.AddMonths(-1)),
            new UserHistory(2, 9, DateTime.Now.AddDays(-5)),
            new UserHistory(3, 7, DateTime.Now.AddDays(-10)),
        };

        //1 get grunge songs longer than 3 minutes and with rating above 4.5 (фільтрацію за декількома критеріями)
        var grungeDurationRating =
            from song in songs
            where song.GenreId == 1 && song.Duration > 180 && song.Rating > 4.5
            select song;

        // foreach (var s in grungeDurationRating)
        //     Console.WriteLine($"{s.Title} - {s.Duration} s - {s.Rating} stars");

        //2 song + album + release year (з’єднання)
        var songAlbumYear =
            from s in songs
            join a in albums on s.AlbumId equals a.Id
            select new
            {
                AlbumTitle = a.Title,
                SongTitle = s.Title,
                AlbumYear = a.Year,
            };

        // foreach (var entry in songAlbumYear)
        //     Console.WriteLine(
        //         $"{entry.SongTitle} from {entry.AlbumTitle} released in {entry.AlbumYear}"
        //     );

        //3 song longer than 5 minutes (let variable)
        var longSongsWithLet =
            from s in songs
            let mins = s.Duration / 60
            let secs = s.Duration % 60
            where mins >= 5
            select new { s.Title, Time = $"{mins}:{secs:D2}" };

        // foreach (var song in longSongsWithLet)
        //     Console.WriteLine($"{song.Title} - {song.Time}");

        //4 group songs by genre (групування)
        var songsByGenre = songs.GroupBy(s => s.GenreId);

        // foreach (var sGroup in songsByGenre)
        // {
        //     foreach (var song in sGroup)
        //     {
        //         Console.WriteLine($"{song.Title} - {song.GenreId}");
        //     }
        // }

        //5 group songs by albums + where avg song length is >200 (групування разом з фільтрацією)
        var songsByAlbumsAvgDuration =
            from s in songs
            join a in albums on s.AlbumId equals a.Id
            group s by a.Title into g
            let avg = g.Average(s => s.Duration)
            where avg > 200
            select new { AlbumTitle = g.Key, AvgDuration = avg };

        // foreach (var item in songsByAlbumsAvgDuration)
        // {
        //     Console.WriteLine($"Album: {item.AlbumTitle} - Avg Duration: {item.AvgDuration:F2}s");
        // }

        //6 total song duration (агрегування)
        var totalDuration = songs.Sum(s => s.Duration);
        // Console.WriteLine($"Total songs duration: {totalDuration / 60}:{totalDuration % 60}");

        //7 max song rating for each album (групування разом з агрегуванням)
        var maxRatingPerAlbum = songs
            .GroupBy(s => s.AlbumId)
            .Join(
                albums,
                group => group.Key,
                album => album.Id,
                (group, album) =>
                    new { AlbumTitle = album.Title, MaxRating = group.Max(s => s.Rating) }
            );

        // foreach (var item in maxRatingPerAlbum)
        //     Console.WriteLine($"Album: {item.AlbumTitle}: Max rating - {item.MaxRating}");

        //8 songs with rating >4, order by desc (rating) then by title (сортування з фільтрацією)
        var sortedRatingTitle =
            from s in songs
            where s.Rating > 4
            orderby s.Rating descending, s.Title ascending
            select s;

        // foreach (var song in sortedRatingTitle)
        //     Console.WriteLine($"{song.Title} - {song.Rating}");

        //9 long songs or with high rating (об'єднання результатів декількох запитів в один)
        var songsHighRating = songs.Where(s => s.Rating > 4.5);
        var longSongs = songs.Where(s => s.Duration > 350);
        var highAndLongSongs = songsHighRating.Union(longSongs);

        // foreach (var s in highAndLongSongs)
        //     Console.WriteLine(s.Title);

        //10 artist dictionary (перетворення в інші структури,)
        var artistsToDict = artists.ToDictionary(a => a.Id, a => a.Name);

        // if (artistsToDict.ContainsKey(1))
        //     Console.WriteLine($"ID 1: {artistsToDict[1]}");

        // albums where >=80% of songs have a rating >4
        var goodAlbums =
            from a in albums
            join s in songs on a.Id equals s.AlbumId
            group s by a into g
            where (double)g.Count(s => s.Rating > 4) / g.Count() >= 0.8
            select new { AlbumTitle = g.Key.Title };

        // foreach (var album in goodAlbums)
        //     Console.WriteLine($"{album.AlbumTitle}");

        // genres of users who have 50% over 5 minutes long tracks
        var longTrackUsers = users
            .Where(u =>
            {
                //get user history
                var userListenedSongs = history
                    .Where(h => h.UserId == u.Id)
                    //join with songs
                    .Join(songs, h => h.SongId, s => s.Id, (h, s) => s)
                    //toList so that we can count the amount of tracks
                    .ToList();
                if (userListenedSongs.Count() == 0)
                    return false;

                double longTracks = userListenedSongs.Count(s => s.Duration > 300);
                return longTracks / userListenedSongs.Count > 0.5;
            })
            //flatten the result to get the history records
            .SelectMany(u => history.Where(h => h.UserId == u.Id))
            //join with songs to get the genre
            .Join(songs, h => h.SongId, s => s.Id, (h, s) => s.GenreId)
            .Distinct()
            //join with genres to get the genre name
            .Join(genres, gId => gId, g => g.Id, (gId, g) => g.Name);

        // foreach (var genreName in longTrackUsers)
        //     Console.WriteLine($"Genre: {genreName}");

        //top3 artists among users who changed genres
        var topArtists = (
            from u in users
            // first we get the historical genre
            let historicalGenreId = history
                //get history of a user
                .Where(h => h.UserId == u.Id)
                //join with songs to get the genres
                .Join(songs, h => h.SongId, s => s.Id, (h, s) => s.GenreId)
                //group the genres by creating a group for each genre id
                .GroupBy(gId => gId)
                //then order the groups by the amount of songs they contain
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                //get the first one
                .FirstOrDefault()

            // then we check if the current genre is different from the historical genre
            where historicalGenreId != 0 && u.CurrentGenreId != historicalGenreId

            // then we get to the artists by first getting the history, then the song, then the album, then the artist
            join h in history on u.Id equals h.UserId
            join s in songs on h.SongId equals s.Id
            join a in albums on s.AlbumId equals a.Id
            join art in artists on a.ArtistId equals art.Id
            //group artists by name and get the most popular
            group art by art.Name into g
            orderby g.Count() descending
            select new { ArtistName = g.Key }
        ).Take(3); //then take the top 3

        // foreach (var item in topArtists)
        //     Console.WriteLine($"{item.ArtistName}");

        //albums with 50% songs listened by users with 5+ genres
        var songsFromMultiGenreUsers = history
            //get history and group by user
            .GroupBy(h => h.UserId)
            //get the genre of each song and check if there are at least 5 UNIQUE genres
            .Where(g =>
                g.Join(songs, h => h.SongId, s => s.Id, (h, s) => s.GenreId).Distinct().Count() >= 5
            )
            //flatten to get a list of song ids
            .SelectMany(g => g.Select(h => h.SongId))
            .Distinct()
            .ToList();

        var popularAlbums = albums.Where(a =>
        {
            //get all the songs from the album
            var albumSongs = songs.Where(s => s.AlbumId == a.Id).ToList();
            if (albumSongs.Count == 0)
                return false;
            //get the amount of songs that were listened by users with 5+ genres
            double favoriteCount = albumSongs.Count(s => songsFromMultiGenreUsers.Contains(s.Id));
            return (favoriteCount / albumSongs.Count) > 0.5;
        });

        foreach (var album in popularAlbums)
        {
            Console.WriteLine($"Album: {album.Title}");
        }
    }
}
