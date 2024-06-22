namespace MoodMediaKata;

public static class IdGenerator
{
    private static readonly object Lock = new object();
    private static readonly Dictionary<string, long> NextIds = new Dictionary<string, long>();

    public static long NextId(string scope)
    {
        lock (Lock)
        {
            if (!NextIds.TryGetValue(scope, out long nextId))
            {
                NextIds[scope] = 0;
                nextId = 0;
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
}