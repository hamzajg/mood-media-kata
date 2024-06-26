namespace MoodMediaKata.App;

public static class IdGenerator
{
    private static readonly object Lock = new object();
    private static readonly Dictionary<string, long> NextIds = new Dictionary<string, long>();
    private static long _startFrom = 0;

    public static long NextId(string scope)
    {
        lock (Lock)
        {
            if (!NextIds.TryGetValue(scope, out long nextId))
            {
                NextIds[scope] = _startFrom;
                nextId = _startFrom;
            }

            NextIds[scope] = Interlocked.Increment(ref nextId);
            return nextId;
        }
    }

    public static void Reset()
    {
        lock (Lock)
        {
            NextIds.Clear();
        }
    }

    public static void StartFrom(long startFrom)
    {
        _startFrom = startFrom;
    }
}