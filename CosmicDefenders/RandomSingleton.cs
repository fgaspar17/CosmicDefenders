namespace CosmicDefenders;

internal static class RandomSingleton
{
    private static readonly Random _instance = new();
    public static Random Instance => _instance;
}