// 1. Subject Interface
public interface IArchiveService
{
    string GetDocument(string documentId);
}

// 2. Real Subject (Offline Storage)
public class RealOfflineArchive : IArchiveService
{
    private readonly string _storageLocation;

    public RealOfflineArchive(string location)
    {
        _storageLocation = location;
        ConnectToPhysicalMedia(); // Simulated expensive operation
    }

    private void ConnectToPhysicalMedia()
    {
        Console.WriteLine(
            $"[System]: Initializing physical connection to storage at {_storageLocation}..."
        );
        // Simulate 3 seconds delay for mechanical spin-up or robotic arm
        Thread.Sleep(3000);
        Console.WriteLine("[System]: Offline storage is now ONLINE.");
    }

    public string GetDocument(string documentId)
    {
        return $"Content of document '{documentId}' retrieved from high-security offline vault at {_storageLocation}.";
    }
}

// 3. Proxy (Virtual Proxy for Delayed Connection)
public class ArchiveProxy(string location) : IArchiveService
{
    private RealOfflineArchive? _realArchive;
    private readonly string _storageLocation = location;
    private readonly List<string> _accessHistory = [];

    public string GetDocument(string documentId)
    {
        // Logging access even before connection
        _accessHistory.Add($"{DateTime.Now}: Requested {documentId}");
        Console.WriteLine($"[Proxy]: Request received for ID: {documentId}. Processing...");

        // Delayed connection (Lazy Initialization)
        if (_realArchive == null)
        {
            Console.WriteLine(
                "[Proxy]: Offline vault is not connected. Initiating delayed connection..."
            );
            _realArchive = new RealOfflineArchive(_storageLocation);
        }
        else
        {
            Console.WriteLine("[Proxy]: Using existing connection to vault.");
        }

        return _realArchive.GetDocument(documentId);
    }

    public void ShowAccessHistory()
    {
        Console.WriteLine("\n\tACCESS HISTORY LOG:");
        foreach (var log in _accessHistory)
            Console.WriteLine(log);
    }
}

// 4. Main Program
class Program
{
    static void Main()
    {
        const string vaultLocation = "Vault-Sector-7G";
        // Client works with the Proxy instead of the Real Subject
        IArchiveService archiveAccess = new ArchiveProxy(vaultLocation);

        Console.WriteLine("Step 1: Browsing metadata (no connection needed yet)...");
        // Imagine metadata is stored in a fast DB, but actual content is offline.

        Console.WriteLine("\nStep 2: Requesting Document #101...");
        // This will trigger the slow connection
        string doc1 = archiveAccess.GetDocument("DOC-101-SECRET");
        Console.WriteLine($"RESULT: {doc1}");

        Console.WriteLine("\nStep 3: Requesting Document #102...");
        // This will be fast as the connection is already established
        string doc2 = archiveAccess.GetDocument("DOC-102-PUBLIC");
        Console.WriteLine($"RESULT: {doc2}");

        ((ArchiveProxy)archiveAccess).ShowAccessHistory();
    }
}
